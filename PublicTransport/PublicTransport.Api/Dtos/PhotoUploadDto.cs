using Microsoft.AspNetCore.Http;

namespace PublicTransport.Api.Dtos
{
    public class PhotoUploadDto
    {
        public string Url { get; set; }
        public IFormFile File { get; set; }
        public string PublicId { get; set; }
    }
}