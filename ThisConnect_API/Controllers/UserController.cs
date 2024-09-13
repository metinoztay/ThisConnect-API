using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using ThisConnect_API.DTOs;
using ThisConnect_API.Models;

namespace ThisConnect_API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserController : ControllerBase
	{
		private readonly Db7877Context _context;

		public UserController(Db7877Context context)
		{
			_context = context;
		}

		[HttpGet("GetAllUsers")]
		public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
		{
			return await _context.Users.ToListAsync();
		}

		[HttpGet("GetUserById/{id}")]
		public async Task<ActionResult<User>> GetUserById(string id)
		{
			var user = await _context.Users.FindAsync(id);
			if (user == null)
			{
				return NotFound();
			}
			return user;
		}

		
		[HttpPost("CreateUser")]
		public async Task<IActionResult> CreateUser([FromBody] UserDTO temp)
		{
			if (temp == null)
			{
				return BadRequest();
			}

            User user = new User();
			user.Phone = temp.Phone;
			user.Email = temp.Email;
			user.Title = temp.Title;
			user.Name = temp.Name;
			user.Surname = temp.Surname;
			user.AvatarUrl = temp.AvatarUrl;
            DateTime utcNow = DateTime.UtcNow;
            TimeZoneInfo turkeyTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Turkey Standard Time");
            DateTime turkeyTime = TimeZoneInfo.ConvertTimeFromUtc(utcNow, turkeyTimeZone);
            string formattedTime = turkeyTime.ToString("dd.MM.yyyy HH:mm:ss");
			user.CreatedAt = DateTime.UtcNow;
            user.LastSeenAt = formattedTime;

            try
            {
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"Internal server error: {ex.Message} + + {ex.ToString()}");
            }
            return Ok(user);
		}

		[HttpPut("UpdateUser/{id}")]
		public async Task<IActionResult> UpdateUser(string id, [FromBody] User user)
		{
			if (id != user.UserId || user == null)
			{
				return BadRequest();
			}

			var existingUser = await _context.Users.FindAsync(id);
			if (existingUser == null)
			{
				return NotFound();
			}

			existingUser.Phone = user.Phone;
			existingUser.Email = user.Email;
			existingUser.Title = user.Title;
			existingUser.Name = user.Name;
			existingUser.Surname = user.Surname;
			existingUser.AvatarUrl = user.AvatarUrl;
			existingUser.LastSeenAt = user.LastSeenAt;

			_context.Users.Update(existingUser);
			await _context.SaveChangesAsync();
			return NoContent();
		}

        [HttpPut("UpdateLastSeenAt")]
        public async Task<IActionResult> UpdateLastSeenAt(string userId)
        {
            var user = await _context.Users.FindAsync(userId);
			user.LastSeenAt = DateTime.Now.ToString();
			_context.Users.Update(user);
            await _context.SaveChangesAsync();
            return NoContent();
        }


        [HttpDelete("DeleteUser/{id}")]
		public async Task<IActionResult> DeleteUser(string id)
		{
			var user = await _context.Users.FindAsync(id);
			if (user == null)
			{
				return NotFound();
			}

			_context.Users.Remove(user);
			await _context.SaveChangesAsync();
			return NoContent();
		}
	}
}
