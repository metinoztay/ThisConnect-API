using System;
using System.Collections.Generic;

namespace ThisConnect_WebApi.Models;

public partial class User
{
    public string UserId { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Tittle { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public string AvatarUrl { get; set; } = null!;

    public string LastSeenAt { get; set; } = null!;

    public virtual ICollection<ChatRoom> ChatRoomParticipant1s { get; set; } = new List<ChatRoom>();

    public virtual ICollection<ChatRoom> ChatRoomParticipant2s { get; set; } = new List<ChatRoom>();

    public virtual ICollection<Message> MessageRecieverUsers { get; set; } = new List<Message>();

    public virtual ICollection<Message> MessageSenderUsers { get; set; } = new List<Message>();

    public virtual ICollection<Qr> Qrs { get; set; } = new List<Qr>();
}
