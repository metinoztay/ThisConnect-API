using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThisConnect_API.Models;
using ThisConnect_API.DTOs;

namespace ThisConnect_WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OTPController : ControllerBase
    {
        private readonly Db7877Context _context;

        public OTPController(Db7877Context context)
        {
            _context = context;
        }

        [HttpPost("OTPVerification")]
        public async Task<ActionResult<User>> OTPVerification([FromBody] OTP_DTO otp)
        {
            try
            {
                Otp tblOtp = await _context.Otps.FirstOrDefaultAsync(o => o.Phone == otp.Phone && o.OtpValue == otp.OtpValue);
                User tempuser = new User();
                if (tblOtp == null || tblOtp.ExpirationTime < DateTime.Now)
                {                   
                    return tempuser;
                }

                tempuser = await _context.Users.FirstOrDefaultAsync(u => u.Phone == otp.Phone);

                return tempuser;


            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("CreateOTP")]
        public async Task<IActionResult> CreateOTP(String phone)
        {
            try
            {
                Otp? Otp = _context.Otps.FirstOrDefault(o => o.Phone == phone);

               

                if (Otp == null)
                {
                    Otp = new Otp();
                    Otp.Phone = phone;
                    Otp.OtpValue = phone.Substring(0, 6);
                    Otp.ExpirationTime = DateTime.Now.AddMinutes(5);
                    await _context.Otps.AddAsync(Otp);
                }
                else {
                    Otp.Phone = phone;
                    Otp.OtpValue = phone.Substring(0, 6);
                    Otp.ExpirationTime = DateTime.Now.AddMinutes(5);
                    _context.Otps.Update(Otp);
                }

                await _context.SaveChangesAsync();           
                


            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

            return NoContent();
        }

    }
}
