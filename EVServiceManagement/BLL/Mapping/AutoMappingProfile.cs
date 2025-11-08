using AutoMapper;
using BLL.DTOs.AccountDtos;
using BLL.DTOs.AppointmentDtos;
using BLL.DTOs.PartDtos;
using BLL.DTOs.PaymentDtos;
using BLL.DTOs.ServiceDtos;
using BLL.DTOs.VehicleDtos;
using DAL.Entities;

namespace BLL.Mapping
{
    public class AutoMappingProfile : Profile
    {
        public AutoMappingProfile()
        {
            CreateMap<Staff, StaffDto>();
            CreateMap<Customer, CustomerDto>();
            CreateMap<Technician, TechnicianDto>();
            CreateMap<Manager, ManagerDto>();
            CreateMap<Account, AccountDto>()
                .ForMember(dest => dest.CustomerDto, opt => opt.MapFrom(src => src.Customer))
                .ForMember(dest => dest.StaffDto, opt => opt.MapFrom(src => src.Staff))
                .ForMember(dest => dest.TechnicianDto, opt => opt.MapFrom(src => src.Technician))
                .ForMember(dest => dest.ManagerDto, opt => opt.MapFrom(src => src.Manager));

            CreateMap<Vehicle, VehicleDto>();
            CreateMap<CreateVehicleDto, Vehicle>();

            CreateMap<Part, PartDto>();
            CreateMap<DAL.Entities.Service, ServiceDto>()
                .ForMember(dest => dest.PartDtos, opt => opt.MapFrom(src => src.ServiceParts.Select(sp => sp.Part)));

            CreateMap<Payment, PaymentDto>();
            CreateMap<ServiceOrderDetail, ServiceOrderDetailDto>();
            CreateMap<Technician, TechnicianDto>();
            CreateMap<Appointment, AppointmentDto>()
                .ForMember(dest => dest.TechnicianDtos, opt => opt.MapFrom(src => src.TechnicianAssignments.Select(ta => ta.Technician)));

            CreateMap<PartRequest, PartRequestDto>()
                .ForMember(dest => dest.PartDto, opt => opt.MapFrom(src => src.Part));
            CreateMap<CreatePartRequestDto, PartRequest>(); 
        }
    }
}
