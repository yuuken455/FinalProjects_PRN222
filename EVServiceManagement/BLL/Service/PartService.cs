using AutoMapper;
using BLL.DTOs.PartDtos;
using BLL.IService;
using DAL.Entities;
using DAL.IRepository;

namespace BLL.Service
{
    public class PartService : IPartService
    {
        private readonly IPartRepository _repo;
        private readonly IMapper _mapper;
        public PartService(IPartRepository repo, IMapper mapper)
        {
            _repo = repo; _mapper = mapper;
        }

        public async Task<List<PartDto>> GetAllAsync(string? keyword = null) =>
            (await _repo.GetAllAsync(keyword)).Select(_mapper.Map<PartDto>).ToList();

        public async Task<PartDto?> GetAsync(int id)
        {
            var p = await _repo.GetByIdAsync(id);
            return p == null ? null : _mapper.Map<PartDto>(p);
        }

        public async Task<int> CreateAsync(CreatePartDto dto)
        {
            var entity = _mapper.Map<Part>(dto);
            await _repo.AddAsync(entity);
            return entity.PartId;
        }

        public async Task UpdateAsync(UpdatePartDto dto)
        {
            var entity = await _repo.GetByIdAsync(dto.PartId) ?? throw new KeyNotFoundException();
            _mapper.Map(dto, entity);
            await _repo.UpdateAsync(entity);
        }

        public Task DeleteAsync(int id) => _repo.DeleteAsync(id);
        public Task<List<PartDto>> GetAllParts() => GetAllAsync();

    }
}
