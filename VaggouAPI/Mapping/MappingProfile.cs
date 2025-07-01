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
            CreateMap<Adress, AdressDto>();
            CreateMap<AdressDto, Adress>();

            //Favorite mapping
            CreateMap<Favorite, FavoriteDto>();
            CreateMap<FavoriteDto, Favorite>()
                .ForMember(dest => dest.Client, opt => opt.Ignore())
                .ForMember(dest => dest.ParkingLot, opt => opt.Ignore());

            //MonthlyReport mapping
            CreateMap<MonthlyReport, MonthlyReportDto>()
                .ForMember(dest => dest.ParkingLotId, opt => opt.Ignore());
            CreateMap<MonthlyReportDto, MonthlyReport>()
                .ForMember(dest => dest.ParkingLot, opt => opt.Ignore());

            //ParkingLot mapping
            CreateMap<ParkingLot, ParkingLotDto>();
            CreateMap<ParkingLotDto, ParkingLot>()
                .ForMember(dest => dest.Owner, opt => opt.Ignore())
                .ForMember(dest => dest.Adress, opt => opt.Ignore());

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
        }
    }
}
