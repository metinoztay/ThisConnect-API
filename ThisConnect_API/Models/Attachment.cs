using System;
using System.Collections.Generic;

namespace ThisConnect_API.Models;

public partial class Attachment
{
    public string AttachmentId { get; set; } = null!;

    public string FileType { get; set; } = null!;

    public string FileUrl { get; set; } = null!;

    public string? FileName { get; set; }

    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();
}
