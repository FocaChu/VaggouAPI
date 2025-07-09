namespace VaggouAPI
{
    public interface IImageService
    {
        Task<Image> UploadParkingLotImageAsync(Guid parkingLotId, IFormFile file, ImageType imageType, Guid loggedInUserId);

        Task<Image> CreateClientProfileImageAsync(IFormFile file, Guid clientId);

        Task<Image> UploadClientProfileImageAsync(IFormFile file, Guid loggedInUserId);

        Task<Image> UploadVehicleImageAsync(Guid vehicleId, IFormFile file);

        Task<(byte[] content, string contentType)?> GetImageAsync(Guid imageId);

        Task DeleteImageAsync(Guid imageId, Guid loggedInUserId);
    }
}
