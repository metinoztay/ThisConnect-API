using System;
using System.Collections.Generic;

namespace ThisConnect_WebApi.Models;

public partial class ChatRoom
{
    public string ChatRoomId { get; set; } = null!;

    public string Participant1Id { get; set; } = null!;

    public string Participant2Id { get; set; } = null!;

    public string? LastMessageId { get; set; }

    public string CreatedAt { get; set; } = null!;

    public virtual Message? LastMessage { get; set; }

    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();

    public virtual User Participant1 { get; set; } = null!;

    public virtual User Participant2 { get; set; } = null!;
}
