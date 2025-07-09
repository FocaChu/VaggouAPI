using Microsoft.AspNetCore.Mvc;

namespace VaggouAPI
{
    public class UploadImageRequest
    {
        [FromForm]
        public IFormFile File { get; set; }

        [FromForm]
        public ImageType Type { get; set; }
    }
}
