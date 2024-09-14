using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThisConnect_API.Models;

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

            // Klasör var mı kontrol et ve oluştur
            if (!Directory.Exists(uploadsFolderPath))
            {
                Directory.CreateDirectory(uploadsFolderPath);
            }
            var user = await _context.Users.FindAsync(userId);
            string fileName = file.FileName;
            string extension = Path.GetExtension(fileName);
            var filePath = Path.Combine(uploadsFolderPath, user.UserId.ToString() + extension);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }


            var fileUrl = $"{Request.Scheme}://{Request.Host}/Photos/UserProfilePhotos/{user.UserId + extension}";


            user.AvatarUrl = fileUrl;

            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(new { fileUrl });
        }
        #endregion
    }
}
