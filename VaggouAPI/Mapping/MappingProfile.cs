using AutoMapper;

namespace VaggouAPI
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // CreateMap<Source, Destination>();
            // Example: CreateMap<User, UserDto>();

            //Adress mapping
            CreateMap<Address, AddressDto>();
            CreateMap<AddressDto, Address>();

            CreateMap<Client, ClientDto>();
            CreateMap<ClientDto, Client>();

            //Favorite mapping
            CreateMap<Favorite, FavoriteDto>();
            CreateMap<FavoriteDto, Favorite>()
                .ForMember(dest => dest.Client, opt => opt.Ignore())
                .ForMember(dest => dest.ParkingLot, opt => opt.Ignore());

            //MonthlyReport mapping
            CreateMap<MonthlyReportService, MonthlyReportDto>()
                .ForMember(dest => dest.ParkingLotId, opt => opt.Ignore());
            CreateMap<MonthlyReportDto, MonthlyReportService>()
                .ForMember(dest => dest.ParkingLot, opt => opt.Ignore());

            //ParkingLot mapping
            CreateMap<ParkingLot, ParkingLotDto>();
            CreateMap<ParkingLotDto, ParkingLot>()
                .ForMember(dest => dest.Owner, opt => opt.Ignore())
                .ForMember(dest => dest.Address, opt => opt.Ignore());

            //ParkingSpot mapping
            CreateMap<ParkingSpot, ParkingSpotDto>();
            CreateMap<ParkingSpotDto, ParkingSpot>()
                .ForMember(dest => dest.ParkingLot, opt => opt.Ignore());

            //Reservation mapping
            CreateMap<Reservation, ReservationDto>();
            CreateMap<ReservationDto, Reservation>()
                .ForMember(dest => dest.Client, opt => opt.Ignore())
                .ForMember(dest => dest.Vehicle, opt => opt.Ignore())
                .ForMember(dest => dest.ParkingSpot, opt => opt.Ignore())
                .ForMember(dest => dest.Payment, opt => opt.Ignore());

            //Vehicle mapping
            CreateMap<Vehicle, VehicleDto>();
            CreateMap<VehicleDto, Vehicle>()
                .ForMember(dest => dest.VehicleModel, opt => opt.Ignore())
                .ForMember(dest => dest.Owner, opt => opt.Ignore());

            //VehicleModel mapping
            CreateMap<VehicleModel, VehicleModelDto>();
            CreateMap<VehicleModelDto, VehicleModel>();
        }
    }
}
