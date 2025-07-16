using AutoMapper;

namespace VaggouAPI
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region Auth & User Mappings

            CreateMap<RegisterRequestDto, User>();
            CreateMap<RegisterRequestDto, Client>();

            CreateMap<User, UserSummaryDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Client.FullName))
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.Roles.Select(r => r.Name)));

            CreateMap<User, UserDetailDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Client.FullName))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Client.Phone))
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.Roles.Select(r => r.Name)))
                .ForMember(dest => dest.OwnedParkingLotsCount, opt => opt.MapFrom(src => src.Client.OwnedParkingLots.Count));

            #endregion

            #region Client Mappings

            CreateMap<UpdateClientProfileRequestDto, Client>();

            CreateMap<Client, ClientProfileResponseDto>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email));

            CreateMap<Client, ClientSummaryResponseDto>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email));

            #endregion

            #region Address Mappings

            CreateMap<Address, AddressResponseDto>();
            CreateMap<CreateAddressRequestDto, Address>();
            CreateMap<UpdateAddressRequestDto, Address>();

            #endregion

            #region ParkingLot Mappings

            CreateMap<CreateParkingLotRequestDto, ParkingLot>();

            CreateMap<UpdateParkingLotRequestDto, ParkingLot>();

            CreateMap<ParkingLot, ParkingLotResponseDto>()
                .ForMember(dest => dest.ImageIds, opt => opt.MapFrom(src => src.Images.Select(i => i.Id)));

            CreateMap<ParkingLot, CreateParkingLotResponseDto>()
                .ForMember(dest => dest.CreatedParkingLot, opt => opt.MapFrom(src => src));


            #endregion

            #region ParkingSpot Mappings

            CreateMap<ParkingSpot, ParkingSpotSummaryResponseDto>();
            CreateMap<CreateParkingSpotRequestDto, ParkingSpot>();
            CreateMap<UpdateParkingSpotRequestDto, ParkingSpot>();

            #endregion

            #region Reservation Mappings

            CreateMap<CreateReservationRequestDto, Reservation>();

            CreateMap<Reservation, ReservationResponseDto>();

            #endregion

            #region Review Mappings

            CreateMap<CreateReviewRequestDto, Review>();

            CreateMap<Review, ReviewResponseDto>()
                .ForMember(dest => dest.ClientName, opt => opt.MapFrom(src => src.Client.FullName));

            #endregion

            #region Vehicle Mappings

            CreateMap<Vehicle, VehicleSummaryResponseDto>()
                .ForMember(dest => dest.ModelName, opt => opt.MapFrom(src => src.VehicleModel.ModelName))
                .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.VehicleModel.Brand));

            CreateMap<VehicleModel, VehicleModelDto>().ReverseMap();
            CreateMap<VehicleDto, Vehicle>(); 

            #endregion
        }
    }
}