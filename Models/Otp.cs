using System;
using System.Collections.Generic;

namespace ThisConnect_WebApi.Models;

public partial class Otp
{
    public string OtpId { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string OtpValue { get; set; } = null!;

    public DateTime ExpirationTime { get; set; }
}
