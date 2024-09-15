using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp.Formats.Jpeg;
using ThisConnect_API.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Jpeg;

namespace ThisConnect_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly Db7877Context _context;
        private readonly IWebHostEnvironment _env;

        public UploadController(Db7877Context context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        #region User Photo Upload
        [HttpPost("UploadProfilePhoto/{userId}")]
        public async Task<IActionResult> UploadProfilePhoto(string userId, [FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded");

            var uploadsFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "Photos", "UserProfilePhotos");

            if (!Directory.Exists(uploadsFolderPath))
            {
                Directory.CreateDirectory(uploadsFolderPath);
            }

            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return NotFound("User not found");

            string extension = Path.GetExtension(file.FileName);
            string fileName = $"{user.UserId}{extension}";
            var filePath = Path.Combine(uploadsFolderPath, fileName);

            
            using (var image = Image.Load(file.OpenReadStream()))
            {
                
                image.Mutate(x => x.Resize(new ResizeOptions
                {
                    Mode = ResizeMode.Max,
                    Size = new Size(300, 300) 
                }));

                
                var encoder = new JpegEncoder
                {
                    Quality = 75 
                };

                await image.SaveAsync(filePath, encoder);
            }

            var fileUrl = $"{Request.Scheme}://{Request.Host}/Photos/UserProfilePhotos/{fileName}";

            
            user.AvatarUrl = fileUrl;
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(new { fileUrl });
        }
        #endregion
    }
}
