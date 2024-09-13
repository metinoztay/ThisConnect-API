using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using ThisConnect_API.Models;
using ThisConnect_API.DTOs;

namespace ThisConnect_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatRoomController : ControllerBase
    {
        private readonly Db7877Context _context;

        public ChatRoomController(Db7877Context context)
        {
            _context = context;
        }

        [HttpPost("CreateChatRoom")]
        public async Task<IActionResult> CreateChatRoom([FromBody] ChatRoomDTO chatRoom)
        {
            var existingChatRoom = await _context.ChatRooms
                .FirstOrDefaultAsync(cr =>
                    (cr.Participant1Id == chatRoom.Participant1Id && cr.Participant2Id == chatRoom.Participant2Id) ||
                    (cr.Participant1Id == chatRoom.Participant2Id && cr.Participant2Id == chatRoom.Participant1Id));

            if (existingChatRoom != null)
            {
                return Conflict("Chat room already exists for these participants.");
            }

            DateTime utcNow = DateTime.UtcNow;
            TimeZoneInfo turkeyTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Turkey Standard Time");
            DateTime turkeyTime = TimeZoneInfo.ConvertTimeFromUtc(utcNow, turkeyTimeZone);
            string formattedTime = turkeyTime.ToString("dd.MM.yyyy HH:mm:ss");

            ChatRoom tblChatRoom = new ChatRoom
            {
                Participant1Id = chatRoom.Participant1Id,
                Participant2Id = chatRoom.Participant2Id,
                LastMessageId = null,
                CreatedAt = formattedTime
            };

            await _context.ChatRooms.AddAsync(tblChatRoom);
            await _context.SaveChangesAsync();

            return Ok("Chat room created successfully with participants.");
        }


        [HttpPost("FindChatRoom")]
        public async Task<IActionResult> FindChatRoom([FromBody] ChatRoomDTO chatRoom)
        {
            var existingChatRoom = await _context.ChatRooms
                .FirstOrDefaultAsync(cr =>
                    (cr.Participant1Id == chatRoom.Participant1Id && cr.Participant2Id == chatRoom.Participant2Id) ||
                    (cr.Participant1Id == chatRoom.Participant2Id && cr.Participant2Id == chatRoom.Participant1Id));
            

            return Ok(existingChatRoom);
        }

        [HttpGet("GetChatRoomsByParticipant")]
        public async Task<IActionResult> GetChatRoomsByParticipant(string participantId)
        {
            var chatRooms = await _context.ChatRooms
                .Where(cr => cr.Participant1Id == participantId || cr.Participant2Id == participantId)
                    .Join(
                        _context.Messages,
                        chatRoom => chatRoom.LastMessageId,  
                        message => message.MessageId,               
                        (chatRoom, message) => new { ChatRoom = chatRoom, MessageCreatedAt = message.CreatedAt }
                        )
                        .OrderByDescending(chatRoomWithMessage => chatRoomWithMessage.MessageCreatedAt)  
                        .Select(chatRoomWithMessage => chatRoomWithMessage.ChatRoom)
                        .ToListAsync();


            if (chatRooms == null || chatRooms.Count == 0)
            {
                return NotFound("No chat rooms found for this participant.");
            }

            return Ok(chatRooms);
        }

    }
}
