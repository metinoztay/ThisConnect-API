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

        
        #region User File Upload
        [HttpPost("UploadFile/{userId}")]
        public async Task<IActionResult> UploadFile(string userId, [FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded");

            // Kullanıcı klasör yolu
            var userFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles", "UserFiles");

            // Kullanıcı klasörü yoksa oluştur
            if (!Directory.Exists(userFolderPath))
            {
                Directory.CreateDirectory(userFolderPath);
            }

            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return NotFound("User not found");

            string extension = Path.GetExtension(file.FileName);
            string originalFileName = Path.GetFileNameWithoutExtension(file.FileName);

            DateTime utcNow = DateTime.UtcNow;
            TimeZoneInfo turkeyTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Turkey Standard Time");
            DateTime turkeyTime = TimeZoneInfo.ConvertTimeFromUtc(utcNow, turkeyTimeZone);

            string timeStamp = turkeyTime.ToString("yyyyMMddHHmmssfff"); // Zaman damgası
            string uniqueFileName = $"{originalFileName}_{timeStamp}{extension}";

            var filePath = Path.Combine(userFolderPath, uniqueFileName);

            var fileUrl = $"{Request.Scheme}://{Request.Host}/UploadedFiles/UserFiles/{uniqueFileName}";

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            
            Attachment attachment = new Attachment
            {
                FileType = file.ContentType,
                FileUrl = fileUrl,
                FileName = file.FileName,
            };
            _context.Attachments.Add(attachment);
            await _context.SaveChangesAsync();
            
            return Ok(attachment);
        }
        #endregion

        

        [HttpGet("GetAttachmentById/{attachmentId}")]
        public async Task<ActionResult<IEnumerable<Qr>>> GetAttachmentById(string attachmentId)
        {
            var attachment = await _context.Attachments
                .FirstOrDefaultAsync(m => m.AttachmentId == attachmentId);
            if (attachment == null)
            {
                return NotFound();
            }

            return attachment == null ? NotFound() : Ok(attachment);
        }
    }
}
