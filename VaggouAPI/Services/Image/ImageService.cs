using Microsoft.EntityFrameworkCore;

namespace VaggouAPI
{
    public class ImageService : IImageService
    {
        private readonly Db _context;
        private const long MaxFileSize = 10 * 1024 * 1024; 

        public ImageService(Db context)
        {
            _context = context;
        }

        #region Uploads

        public async Task<Image> UploadParkingLotImageAsync(Guid parkingLotId, IFormFile file, ImageType imageType, Guid loggedInUserId)
        {
            ValidateFile(file);

            var parkingLot = await _context.ParkingLots.FindAsync(parkingLotId)
                ?? throw new NotFoundException("Parking lot not found.");

            if (parkingLot.OwnerId != loggedInUserId)
                throw new UnauthorizedException("You do not have permission to manage this parking lot.");

            if (imageType == ImageType.ParkingLotIcon || imageType == ImageType.ParkingLotBanner)
            {
                await ReplaceSingleImageAsync(parkingLotId, null, null, imageType);
            }

            return await CreateImageEntityAsync(file, imageType, parkingLotId, null, null);
        }

        public async Task<Image> CreateClientProfileImageAsync(IFormFile file, Guid clientId)
        {
            ValidateFile(file);

            return await CreateImageEntityAsync(file, ImageType.ClientProfile, null, clientId, null);
        }

        public async Task<Image> UploadClientProfileImageAsync(IFormFile file, Guid loggedInUserId)
        {
            ValidateFile(file);

            var client = await _context.Clients.FindAsync(loggedInUserId)
                ?? throw new NotFoundException("Client not found.");

            await ReplaceSingleImageAsync(null, loggedInUserId, null, ImageType.ClientProfile);

            return await CreateImageEntityAsync(file, ImageType.ClientProfile, null, loggedInUserId, null);
        }

        public async Task<Image> UploadVehicleImageAsync(Guid vehicleId, IFormFile file)
        {
            ValidateFile(file);

            var vehicle = await _context.Vehicles.FindAsync(vehicleId)
                ?? throw new NotFoundException("vehicle not found.");

            await ReplaceSingleImageAsync(null, null, vehicleId, ImageType.VehicleImage);

            return await CreateImageEntityAsync(file, ImageType.VehicleImage, null, null, vehicleId);
        }

        #endregion

        #region Downloads

        public async Task<(byte[] content, string contentType)?> GetImageAsync(Guid imageId)
        {
            var image = await _context.Images
                .AsNoTracking()
                .Select(i => new { i.Id, i.Content, i.ContentType })
                .FirstOrDefaultAsync(i => i.Id == imageId);

            return image != null ? (image.Content, image.ContentType) : null;
        }

        #endregion

        #region Deletes

        public async Task DeleteImageAsync(Guid imageId, Guid loggedInUserId)
        {
            var image = await _context.Images.FindAsync(imageId)
                ?? throw new NotFoundException("Image not found.");

            bool hasPermission = false;
            if (image.ParkingLotId.HasValue)
            {
                var parkingLot = await _context.ParkingLots.FindAsync(image.ParkingLotId.Value);
                if (parkingLot?.OwnerId == loggedInUserId) hasPermission = true;
            }
            else if (image.ClientId.HasValue && image.ClientId.Value == loggedInUserId)
            {
                hasPermission = true;
            }
            else if (image.VehicleId.HasValue)
            {
                var vehicle = await _context.Vehicles.FindAsync(image.VehicleId.Value);
                if (vehicle?.OwnerId == loggedInUserId) hasPermission = true;
            }

            if (!hasPermission)
                throw new UnauthorizedException("You do not have permission to delete this image.");

            _context.Images.Remove(image);
            await _context.SaveChangesAsync();
        }

        #endregion

        #region Private Helpers

        private void ValidateFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new BusinessException("No file was sent.");

            if (file.Length > MaxFileSize)
                throw new BusinessException($"The file size exceeds the limit of {MaxFileSize / 1024 / 1024}MB.");

            if (!file.ContentType.StartsWith("image/"))
                throw new BusinessException("The uploaded file is not a valid image.");
        }

        private async Task ReplaceSingleImageAsync(Guid? parkingLotId, Guid? clientId, Guid? vehicleId, ImageType imageType)
        {
            var existingImage = await _context.Images.FirstOrDefaultAsync(i =>
                (parkingLotId.HasValue && i.ParkingLotId == parkingLotId) ||
                (clientId.HasValue && i.ClientId == clientId) ||
                (vehicleId.HasValue && i.VehicleId == vehicleId) &&
                i.Type == imageType
            );

            if (existingImage != null)
            {
                _context.Images.Remove(existingImage);
            }
        }

        private async Task<Image> CreateImageEntityAsync(IFormFile file, ImageType imageType, Guid? parkingLotId, Guid? clientId, Guid? vehicleId)
        {
            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);

            var image = new Image
            {
                Content = memoryStream.ToArray(),
                ContentType = file.ContentType,
                Type = imageType,
                ParkingLotId = parkingLotId,
                ClientId = clientId,
                VehicleId = vehicleId
            };

            await _context.Images.AddAsync(image);
            await _context.SaveChangesAsync();
            return image;
        }

        #endregion
    }
}
