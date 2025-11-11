using AutoMapper;
using BLL.DTOs.PartDtos;
using BLL.IService;
using DAL;
using DAL.Entities;
using DAL.IRepository;
using Microsoft.AspNetCore.SignalR;
using BLL.Hubs;
namespace BLL.Service
{
    public class PartRequestService : IPartRequestService
    {
        private readonly IPartRequestRepository _reqRepo;
        private readonly IPartRepository _partRepo;
        private readonly EVServiceManagementContext _ctx;
        private readonly IMapper _mapper;
        private readonly IHubContext<NotificationsHub> _hub;

        public PartRequestService(
            IPartRequestRepository reqRepo,
            IPartRepository partRepo,
            EVServiceManagementContext ctx,
            IMapper mapper,
            IHubContext<NotificationsHub> hub)
        {
            _reqRepo = reqRepo;
            _partRepo = partRepo;
            _ctx = ctx;
            _mapper = mapper;
            _hub = hub;
        }

        public async Task<int> CreateAsync(CreatePartRequestDto dto)
        {
            if (dto.PartId == null || !await _partRepo.ExistsAsync(dto.PartId.Value))
                throw new ArgumentException("Part not found");
            if (dto.Quantity <= 0) throw new ArgumentException("Quantity must be > 0");

            var entity = new PartRequest
            {
                RequestedBy = dto.RequestedBy,
                PartId = dto.PartId,
                Quantity = dto.Quantity,
                Notes = dto.Notes,
                Status = PartRequestStatus.Pending.ToString(),
                RequestDate = DateTime.UtcNow
            };
            await _reqRepo.AddAsync(entity);

            // (tuỳ chọn) lấy tên linh kiện cho UI đẹp hơn
            string? partName = (await _partRepo.GetByIdAsync(dto.PartId.Value))?.Name;

            // 🔔 báo cho Manager có yêu cầu mới
            await _hub.Clients.All.SendAsync("PartRequested", new
            {
                requestId = entity.RequestId,
                partId = entity.PartId,
                partName,
                quantity = entity.Quantity,
                staffId = entity.RequestedBy,
                requestedAt = entity.RequestDate
            });

            return entity.RequestId;
        }

        // Manager duyệt: không đổi kho
        public async Task ApproveAsync(ApprovePartRequestDto dto)
        {
            var req = await _reqRepo.GetByIdAsync(dto.RequestId) ?? throw new KeyNotFoundException("Request not found");
            if (req.Status != PartRequestStatus.Pending.ToString())
                throw new InvalidOperationException("Only pending requests can be approved/rejected");

            using var tx = await _ctx.Database.BeginTransactionAsync();
            try
            {
                req.ApprovedBy = dto.ManagerId;
                req.ApprovalDate = DateTime.UtcNow;
                req.Notes = dto.Notes;
                req.Status = dto.Approve ? PartRequestStatus.Approved.ToString() : PartRequestStatus.Rejected.ToString();
                await _reqRepo.UpdateAsync(req);
                await tx.CommitAsync();

                // 🔔 thông báo thay đổi trạng thái
                await _hub.Clients.All.SendAsync("RequestStatusChanged", new
                {
                    requestId = req.RequestId,
                    status = req.Status
                });
            }
            catch { await tx.RollbackAsync(); throw; }
        }

        // Staff nhận: CỘNG kho và báo SignalR
        public async Task ReceiveAsync(ReceivePartRequestDto dto)
        {
            var req = await _reqRepo.GetByIdAsync(dto.RequestId) ?? throw new KeyNotFoundException("Request not found");
            if (req.Status != PartRequestStatus.Approved.ToString())
                throw new InvalidOperationException("Only approved requests can be marked received");
            if (req.RequestedBy != dto.StaffId)
                throw new InvalidOperationException("Only the requester can confirm receipt");

            using var tx = await _ctx.Database.BeginTransactionAsync();
            try
            {
                await _partRepo.AdjustStockAsync(req.PartId!.Value, req.Quantity); // ✅ cộng kho

                req.Status = PartRequestStatus.Received.ToString();
                if (!string.IsNullOrWhiteSpace(dto.Notes)) req.Notes = dto.Notes;
                await _reqRepo.UpdateAsync(req);

                await tx.CommitAsync();

                var stockAfter = (await _partRepo.GetByIdAsync(req.PartId.Value))!.StockQuantity;

                // 🔔 tồn kho cập nhật
                await _hub.Clients.All.SendAsync("PartReceived", new
                {
                    requestId = req.RequestId,
                    partId = req.PartId,
                    quantity = req.Quantity,
                    stockAfter,
                    receivedAt = DateTime.UtcNow
                });

                // 🔔 trạng thái request đổi sang Received
                await _hub.Clients.All.SendAsync("RequestStatusChanged", new
                {
                    requestId = req.RequestId,
                    status = PartRequestStatus.Received.ToString()
                });
            }
            catch { await tx.RollbackAsync(); throw; }
        }

        public async Task<PartRequestDto?> GetAsync(int id)
        {
            var req = await _reqRepo.GetByIdAsync(id, includeNav: true);
            return req == null ? null : _mapper.Map<PartRequestDto>(req);
        }

        public async Task<List<PartRequestDto>> ListAsync(string? status = null, int? staffId = null)
        {
            var list = await _reqRepo.ListAsync(status, staffId);
            return list.Select(_mapper.Map<PartRequestDto>).ToList();
        }
    }
}
