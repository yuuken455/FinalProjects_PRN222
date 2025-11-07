using AutoMapper;
using BLL.DTOs.Account;
using BLL.DTOs.Vehicle;
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
        }
    }
}
