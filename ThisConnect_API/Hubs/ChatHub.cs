using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore.Diagnostics.Internal;
using ThisConnect_API.Models;
using ThisConnect_API.DTOs;

namespace ThisConnect_API.Hubs
{
    public class ChatHub:Hub
    {
        private readonly Db7877Context _context;

        public ChatHub(Db7877Context context)
        {
            _context = context;
        }
        public async Task JoinRoom(string chatRoomId, string? lastmessageId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, chatRoomId);            
        }


        public async Task LeaveRoom(string chatRoomId)
        {
            
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatRoomId);
            ChatRoom? tblChatRoom = _context.ChatRooms.FirstOrDefault(c => c.ChatRoomId == chatRoomId);
            if (tblChatRoom != null && tblChatRoom.LastMessageId == null)
            {
                _context.ChatRooms.Remove(tblChatRoom);
                await _context.SaveChangesAsync();
            }
        }

        public async Task SendMessage(MessageDTO tempmessage)
        {
            await Clients.Group(tempmessage.ChatRoomId).SendAsync("ReceiveMessage", tempmessage);
            try
            {
                

                Message message = new Message
                {
                    ChatRoomId = tempmessage.ChatRoomId,
                    SenderUserId = tempmessage.SenderUserId,
                    RecieverUserId = tempmessage.RecieverUserId,
                    AttachmentId = null,
                    Content = tempmessage.Content,
                    ReadedAt = null
                };

                DateTime utcNow = DateTime.UtcNow;
                TimeZoneInfo turkeyTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Turkey Standard Time");
                DateTime turkeyTime = TimeZoneInfo.ConvertTimeFromUtc(utcNow, turkeyTimeZone);
                string formattedTime = turkeyTime.ToString("dd.MM.yyyy HH:mm:ss");
                message.CreatedAt = turkeyTime; // Eğer `DateTime` olarak saklıyorsan

                await _context.Messages.AddAsync(message);
                await _context.SaveChangesAsync();

                ChatRoom? chatRoom = _context.ChatRooms.FirstOrDefault(c => c.ChatRoomId == message.ChatRoomId);
                if (chatRoom != null)
                {
                    chatRoom.LastMessageId = message.MessageId;
                    _context.ChatRooms.Update(chatRoom);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    // Hata mesajı istemciye gönderiliyor
                    await Clients.Caller.SendAsync("ErrorMessage", "Chat room not found.");
                }
            }
            catch (Exception ex)
            {
                // İstemciye hata mesajı gönder
                await Clients.Caller.SendAsync("ErrorMessage", $"An error occurred: {ex}");
            }
        }

    }
}
