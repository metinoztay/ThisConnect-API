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

            Message message = new Message();
            message.ChatRoomId = tempmessage.ChatRoomId;
            message.SenderUserId = tempmessage.SenderUserId;
            message.RecieverUserId = tempmessage.RecieverUserId;
            message.AttachmentId = null;
            message.Content = tempmessage.Content;
            message.CreatedAt = DateTime.Now.ToString(); 
            message.ReadedAt = null;

            try
            {
                await _context.Messages.AddAsync(message);
                await _context.SaveChangesAsync(); 
                ChatRoom? chatRoom = new ChatRoom();
                chatRoom = _context.ChatRooms.FirstOrDefault(c => c.ChatRoomId == message.ChatRoomId);
                chatRoom.LastMessageId = message.MessageId;
                _context.ChatRooms.Update(chatRoom);
                _context.SaveChanges();

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Veritabanı mesaj ekleme işlemi sırasında hata oluştu: {ex.Message}");
                throw; 
            }
        }
    }
}
