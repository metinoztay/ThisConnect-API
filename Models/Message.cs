using System;
using System.Collections.Generic;

namespace ThisConnect_WebApi.Models;

public partial class Message
{
    public string MessageId { get; set; } = null!;

    public string ChatRoomId { get; set; } = null!;

    public string SenderUserId { get; set; } = null!;

    public string RecieverUserId { get; set; } = null!;

    public string? AttachmentId { get; set; }

    public string Content { get; set; } = null!;

    public string CreatedAt { get; set; } = null!;

    public string? ReadedAt { get; set; }

    public virtual Attachment? Attachment { get; set; }

    public virtual ChatRoom ChatRoom { get; set; } = null!;

    public virtual ICollection<ChatRoom> ChatRooms { get; set; } = new List<ChatRoom>();

    public virtual User RecieverUser { get; set; } = null!;

    public virtual User SenderUser { get; set; } = null!;
}
