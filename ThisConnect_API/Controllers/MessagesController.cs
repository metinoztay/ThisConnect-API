using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThisConnect_API.Models;
using ThisConnect_API.DTOs;


namespace ThisConnect_WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly Db7877Context _context;

        public MessagesController(Db7877Context context)
        {
            _context = context;
        }

        //[HttpGet]
        [HttpGet("GetMessagesByChatRoomId")]
        public async Task<ActionResult<IEnumerable<Message>>> GetMessagesByChatRoomId(string chatRoomId, string? lastmessageId)
        {
            List<MessageDTO> messages = new List<MessageDTO>();
            List<Message> messagesTempList;
            

            if (string.IsNullOrEmpty(lastmessageId))
            {
                messagesTempList = _context.Messages
                    .Where(m => m.ChatRoomId == chatRoomId)
                    .OrderBy(m => m.CreatedAt)
                    .ToList();
            }
            else
            {
                messagesTempList = _context.Messages
                    .Where(m => m.ChatRoomId == chatRoomId && string.Compare(m.MessageId, lastmessageId) > 0)
                    .OrderBy(m => m.CreatedAt)
                    .ToList();
            }

            foreach (var message in messagesTempList)
            {
                MessageDTO tempMessage = new MessageDTO();
                tempMessage.ChatRoomId = message.ChatRoomId;
                tempMessage.SenderUserId = message.SenderUserId;
                tempMessage.RecieverUserId = message.RecieverUserId;
                tempMessage.AttachmentId = message.AttachmentId;
                tempMessage.Content = message.Content;
                tempMessage.CreatedAt = message.CreatedAt;
                tempMessage.ReadedAt = message.ReadedAt;
                tempMessage.MessageId = message.MessageId;
                messages.Add(tempMessage);

            }

            return Ok(messages);
        }

        [HttpGet("GetMessageById")]
        public async Task<ActionResult<IEnumerable<Message>>> GetMessageById(string messageId)
        {
            Message? message = new Message();

            if (string.IsNullOrEmpty(messageId))
            {
               
            }
            else
            {
                message = _context.Messages
                   .FirstOrDefault(m => m.MessageId == messageId);
            }


                MessageDTO tempMessage = new MessageDTO();
                tempMessage.ChatRoomId = message.ChatRoomId;
                tempMessage.SenderUserId = message.SenderUserId;
                tempMessage.RecieverUserId = message.RecieverUserId;
                tempMessage.AttachmentId = message.AttachmentId;
                tempMessage.Content = message.Content;
                tempMessage.CreatedAt = message.CreatedAt;
                tempMessage.ReadedAt = message.ReadedAt;
                tempMessage.MessageId = message.MessageId;

            

            return Ok(tempMessage);
        }



    }
}
