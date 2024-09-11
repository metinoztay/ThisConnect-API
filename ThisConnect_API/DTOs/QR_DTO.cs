﻿namespace ThisConnect_API.DTOs
{
    public class QR_DTO
    {
        public string QrId { get; set; } = null!;

        public string UserId { get; set; } = null!;

        public string Title { get; set; } = null!;

        public bool ShareEmail { get; set; }

        public bool SharePhone { get; set; }

        public bool ShareNote { get; set; }

        public string? Note { get; set; }

        public string CreatedAt { get; set; } = null!;

        public string? UpdatedAt { get; set; }

        public bool IsActive { get; set; }

    }
}
