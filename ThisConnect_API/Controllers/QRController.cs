using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Xml;
using ThisConnect_API.Models;
using ThisConnect_API.DTOs;

namespace ThisConnect_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QRController : ControllerBase
    {
        private readonly Db7877Context _context;

        public QRController(Db7877Context context)
        {
            _context = context;

        }


        [HttpGet("GetAllQRs")]
        public async Task<ActionResult<IEnumerable<Qr>>> GetAllQRs()
        {
            return await _context.Qrs.ToListAsync();
        }

        [HttpGet("GetQRByID/{qrid}")]
        public async Task<ActionResult<Qr>> GetQRByID(string qrid)
        {
            var myEntity = await _context.Qrs.FindAsync(qrid);


            if (myEntity == null)
            {
                return NotFound();
            }

            return myEntity;
        }

        [HttpGet("GetQRsByUserID/{userid}")]
        public async Task<ActionResult<IEnumerable<Qr>>> GetQRsByUserID(string userid)
        {
            var user = await _context.Users
                .Include(u => u.Qrs)                
                .FirstOrDefaultAsync(u => u.UserId == userid);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user.Qrs);
        }

        [HttpPut("UpdateQR/{qrid}")]
        public async Task<IActionResult> UpdateQR(string qrid, [FromBody] QR_DTO updatedQr)
        {
            if (qrid != updatedQr.QrId)
            {
                return BadRequest("QR ID eşleşmiyor.");
            }

            var existingQr = await _context.Qrs.FindAsync(qrid);
            if (existingQr == null)
            {
                return NotFound();
            }

            existingQr.Note = updatedQr.Note;
            existingQr.SharePhone = updatedQr.SharePhone;
            existingQr.ShareEmail = updatedQr.ShareEmail;
            existingQr.ShareNote = updatedQr.ShareNote;
            existingQr.Title = updatedQr.Title;
            existingQr.IsActive = updatedQr.IsActive;
            DateTime utcNow = DateTime.UtcNow;
            TimeZoneInfo turkeyTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Turkey Standard Time");
            DateTime turkeyTime = TimeZoneInfo.ConvertTimeFromUtc(utcNow, turkeyTimeZone);
            string formattedTime = turkeyTime.ToString("dd.MM.yyyy HH:mm:ss");
            existingQr.UpdatedAt = formattedTime;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QRExists(qrid))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        private bool QRExists(string id)
        {
            return _context.Qrs.Any(e => e.QrId == id);
        }


        [HttpDelete("DeleteQR/{qrId}")]
        public async Task<IActionResult> DeleteQR(string qrId)
        {
            var qrToDelete = _context.Qrs.FirstOrDefault(qr => qr.QrId == qrId);
            if (qrToDelete == null)
            {
                return NotFound();
            }

            _context.Qrs.Remove(qrToDelete);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        [HttpPost("AddQR")]
        public async Task<IActionResult> AddQR([FromBody] QR_DTO newQr)
        {
             try
            {
                DateTime utcNow = DateTime.UtcNow;
                TimeZoneInfo turkeyTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Turkey Standard Time");
                DateTime turkeyTime = TimeZoneInfo.ConvertTimeFromUtc(utcNow, turkeyTimeZone);
                string formattedTime = turkeyTime.ToString("dd.MM.yyyy HH:mm:ss");
                var qrToAdd = new Qr
                {
                    QrId = null,
                    UserId = newQr.UserId,
                    Title = newQr.Title,
                    ShareEmail = newQr.ShareEmail,
                    SharePhone = newQr.SharePhone,
                    ShareNote = newQr.ShareNote,
                    CreatedAt = formattedTime,
                    UpdatedAt = null,
                    Note = newQr.Note,
                    IsActive = newQr.IsActive,
                };

                _context.Qrs.Add(qrToAdd);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(AddQR), new { qrid = qrToAdd.QrId }, qrToAdd);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message} + + {ex.ToString()}");
            }
        }
    }
}