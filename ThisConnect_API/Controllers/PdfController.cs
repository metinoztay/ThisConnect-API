using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PdfSharpCore;
using PdfSharpCore.Pdf;
using TheArtOfDev.HtmlRenderer.PdfSharp;
using ThisConnect_API.Models;

namespace ThisConnect_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PdfController : ControllerBase
    {
        private readonly Db7877Context _context;

        public PdfController(Db7877Context context)
        {
            _context = context;
        }

        [HttpGet("DownloadPDF")]
        public async Task<IActionResult> DownloadPDF(string qrId)
        {
            var document = new PdfDocument();

            
            //string appDownloadQrCodeUrl = "data:image/png;base64," + Convert.ToBase64String(System.IO.File.ReadAllBytes("C:\\Projects\\ThisConnect-API\\ThisConnect_API\\Assets/Images/app_download_qr.png"));
            string appDownloadQrCodeUrl = "https://api.qrserver.com/v1/create-qr-code/?size=200x200&data=" + qrId;
            // URL for the QR code to send a message
            string messageQrCodeUrl = appDownloadQrCodeUrl;

            // HTML content for the PDF
            string htmlelement = "<div style='width:100%; height:100%; text-align:center; font-family: Arial, sans-serif;'>";
            htmlelement += "<h1 style='color:#333;'>This Connect!</h1>";
            htmlelement += "<p style='font-size:16px; color:#555;'>Scan the QR code below to send a message to the QR code owner.</p>";
            htmlelement += "<img style='width:200px; height:200px; border:2px solid #000;' src='" + messageQrCodeUrl + "'/>";
            htmlelement += "<p style='font-size:14px; color:#777;'>If you have any issues scanning the QR code, please contact support@thisconnect.com.</p>";
            htmlelement += "<hr style='margin:20px 0; border:0; border-top:1px solid #ccc;'/>";
            htmlelement += "<h2 style='color:#333;'>Download the ThisConnect App</h2>";
            htmlelement += "<p style='font-size:16px; color:#555;'>To get started, scan the QR code below to download the ThisConnect app.</p>";
            htmlelement += "<img style='width:100px; height:100px; border:2px solid #000;' src='" + appDownloadQrCodeUrl + "'/>";
            htmlelement += "</div>";

            PdfGenerator.AddPdfPages(document, htmlelement, PageSize.A4);

            Qr qr = _context.Qrs.Find(qrId);
            User user = _context.Users.Find(qr.UserId);
            string fileName = user.Name + " " + user.Surname + " - " + qr.Title +" QR Code"+ ".pdf";
            byte[] response = null;
            using (MemoryStream ms = new MemoryStream())
            {
                document.Save(ms);
                response = ms.ToArray();
            }
            return File(response, "application/pdf", fileName);
        }

    }

}