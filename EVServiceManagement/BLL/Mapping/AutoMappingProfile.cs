using AutoMapper;
using BLL.DTOs.AccountDtos;
using BLL.DTOs.AppointmentDtos;
using BLL.DTOs.PartDtos;
using BLL.DTOs.PaymentDtos;
using BLL.DTOs.ServiceDtos;
using BLL.DTOs.VehicleDtos;
using BLL.Service;
using DAL.Entities;

namespace BLL.Mapping
{
    public class AutoMappingProfile : Profile
    {
        public AutoMappingProfile()
        {
            CreateMap<Staff, StaffDto>();
            CreateMap<Customer, CustomerDto>()
                .ForMember(d => d.AccountDto, opt => opt.MapFrom(s => s.Account));
            CreateMap<Technician, TechnicianDto>()
                .ForMember(dest => dest.AccountDto, opt => opt.MapFrom(src => src.Account));
            CreateMap<Manager, ManagerDto>();
            CreateMap<Account, AccountDto>()
                .ForMember(dest => dest.CustomerDto, opt => opt.MapFrom(src => src.Customer))
                .ForMember(dest => dest.StaffDto, opt => opt.MapFrom(src => src.Staff))
                .ForMember(dest => dest.TechnicianDto, opt => opt.MapFrom(src => src.Technician))
                .ForMember(dest => dest.ManagerDto, opt => opt.MapFrom(src => src.Manager));

            CreateMap<Vehicle, VehicleDto>();
            CreateMap<CreateVehicleDto, Vehicle>();
            CreateMap<UpdateVehicleDto, Vehicle>();

            // Part
            CreateMap<Part, PartDto>().ReverseMap();
            CreateMap<CreatePartDto, Part>();
            CreateMap<UpdatePartDto, Part>();

            // Payments
            CreateMap<Payment, PaymentDto>()
                .ForMember(d => d.PaymentStatus, opt => opt.MapFrom(s => s.Status));

            // Services
            CreateMap<ServiceOrderDetail, ServiceOrderDetailDto>()
                .ForMember(dest => dest.ServiceDto, opt => opt.MapFrom(src => src.Service));
            CreateMap<DAL.Entities.Service, ServiceDto>()
                .ForMember(d => d.PartDtos, opt => opt.MapFrom(s => s.ServiceParts.Select(x => x.Part)));

            // Technicians
            CreateMap<TechnicianAssignment, TechnicianAssignmentDto>()
                .ForMember(d => d.TechnicianDto, opt => opt.MapFrom(s => s.Technician));
            CreateMap<TechnicianAssignmentDto, TechnicianAssignment>();

            // Appointments
            CreateMap<Appointment, AppointmentDto>()
                .ForMember(dest => dest.TechnicianAssignmentDtos, opt => opt.MapFrom(src => src.TechnicianAssignments))
                .ForMember(dest => dest.CustomerDto, opt => opt.MapFrom(src => src.Customer))
                .ForMember(dest => dest.VehicleDto, opt => opt.MapFrom(src => src.Vehicle))
                .ForMember(dest => dest.ServiceOrderDetailDtos, opt => opt.MapFrom(src => src.ServiceOrderDetails))
                .ForMember(dest => dest.PaymentDtos, opt => opt.MapFrom(src => src.Payment != null ? new[] { src.Payment } : Array.Empty<Payment>()))
                .ReverseMap()
                // Avoid mapping nested graphs back; we only update scalar fields and technician assignments
                .ForMember(dest => dest.Customer, opt => opt.Ignore())
                .ForMember(dest => dest.Vehicle, opt => opt.Ignore())
                .ForMember(dest => dest.Payment, opt => opt.Ignore())
                .ForMember(dest => dest.ServiceOrderDetails, opt => opt.Ignore())
                .ForMember(dest => dest.TechnicianAssignments, opt => opt.MapFrom(src => src.TechnicianAssignmentDtos));

            // PartRequest
            CreateMap<PartRequest, PartRequestDto>()
                .ForMember(d => d.PartDto, opt => opt.MapFrom(s => s.Part))
                .ForMember(d => d.RequestedByNavigation, opt => opt.Ignore())
                .ForMember(d => d.ApprovedByNavigation, opt => opt.Ignore());

            CreateMap<CreateAccountDto, Account>();
            CreateMap<CreateCustomerDto, Customer>()
                .ForMember(dest => dest.Account, opt => opt.MapFrom(src => src.CreateAccountDto));

            CreateMap<CreatePaymentDto, Payment>();
            CreateMap<CreateServiceOrderDetail, ServiceOrderDetail>();
            CreateMap<CreateAppointmentDto, Appointment>()
                .ForMember(dest => dest.ServiceOrderDetails, opt => opt.MapFrom(src => src.CreateServiceOrderDetailDtos));
        }
    }
}
