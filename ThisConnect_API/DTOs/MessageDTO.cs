namespace ThisConnect_API.DTOs
{
    public class MessageDTO
    {
        public string? MessageId { get; set; }

        public string ChatRoomId { get; set; } = null!;

        public string SenderUserId { get; set; } = null!;

        public string RecieverUserId { get; set; } = null!;

        public string? AttachmentId { get; set; }

        public string Content { get; set; } = null!;

        public string CreatedAt { get; set; } = null!;

        public string? ReadedAt { get; set; }
    }
}
