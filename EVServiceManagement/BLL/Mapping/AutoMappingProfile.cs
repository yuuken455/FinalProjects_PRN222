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
            CreateMap<UpdateVehicleDto, Vehicle>();

            // Part
            CreateMap<Part, PartDto>().ReverseMap();
            CreateMap<CreatePartDto, Part>();
            CreateMap<UpdatePartDto, Part>();

            CreateMap<Payment, PaymentDto>();
            CreateMap<ServiceOrderDetail, ServiceOrderDetailDto>();
            CreateMap<Technician, TechnicianDto>();
            CreateMap<Appointment, AppointmentDto>()
                .ForMember(dest => dest.TechnicianDtos, opt => opt.MapFrom(src => src.TechnicianAssignments.Select(ta => ta.Technician)));

            // PartRequest
            CreateMap<PartRequest, PartRequestDto>()
                .ForMember(d => d.PartDto, opt => opt.MapFrom(s => s.Part))
                .ForMember(d => d.RequestedByNavigation, opt => opt.Ignore())   // map ở nơi khác nếu có DTO Staff/Manager
                .ForMember(d => d.ApprovedByNavigation, opt => opt.Ignore());

            CreateMap<CreateAccountDto, Account>();
            CreateMap<CreateCustomerDto, Customer>()
                .ForMember(dest => dest.Account, opt => opt.MapFrom(src => src.CreateAccountDto));

            CreateMap<CreatePaymentDto, Payment>();
            CreateMap<CreateServiceOrderDetail, ServiceOrderDetail>();
            CreateMap<CreateAppointmentDto, Appointment>()
                .ForMember(dest => dest.ServiceOrderDetails, opt => opt.MapFrom(src => src.CreateServiceOrderDetailDtos)); ;
        }
    }
}
