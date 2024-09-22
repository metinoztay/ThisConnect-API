using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PdfSharpCore;
using PdfSharpCore.Pdf;
using WkHtmlToPdfDotNet;
using System;
using System.Reflection.Metadata;
using TheArtOfDev.HtmlRenderer.PdfSharp;
using ThisConnect_API.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static TheArtOfDev.HtmlRenderer.Adapters.RGraphicsPath;
using Azure;

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
        /*
        [HttpGet("DownloadPDF")]
        public async Task<IActionResult> DownloadPDF(string qrId)
        {
            var document = new PdfDocument();

            // Generate the URL for the app download QR code
            string appDownloadQrCodeUrl = "https://api.qrserver.com/v1/create-qr-code/?size=300x300&data=" + qrId;

            // URL for the QR code to send a message
            string messageQrCodeUrl = appDownloadQrCodeUrl;

            // HTML content for the PDF
            string htmlElement = "<div style='width:100%; height:100%; text-align:center; font-family: Arial, sans-serif;'>";
            htmlElement += "<h1 style='color:#333;'>This Connect!</h1>";
            htmlElement += "<p style='font-size:16px; color:#555;'>Scan the QR code below to send a message to the QR code owner.</p>";
            htmlElement += "<img style='width:200px; height:200px; border:2px solid #000;' src='" + messageQrCodeUrl + "'/>";
            htmlElement += "<p style='font-size:14px; color:#777;'>If you have any issues scanning the QR code, please contact support@thisconnect.com.</p>";
            htmlElement += "<hr style='margin:20px 0; border:0; border-top:1px solid #ccc;'/>";
            htmlElement += "<h2 style='color:#333;'>Download the ThisConnect App</h2>";
            htmlElement += "<p style='font-size:16px; color:#555;'>To get started, scan the QR code below to download the ThisConnect app.</p>";
            htmlElement += "<img style='width:100px; height:100px; border:2px solid #000;' src='" + appDownloadQrCodeUrl + "'/>";
            htmlElement += "</div>";

            // Add the HTML content as a page to the document
            PdfGenerator.AddPdfPages(document, htmlElement, PageSize.A4);


            // Retrieve QR and user information
            Qr qr = _context.Qrs.Find(qrId);
            if (qr == null)
            {
                throw new Exception("QR code not found.");
            }

            User user = _context.Users.Find(qr.UserId);
            if (user == null)
            {
                throw new Exception("User not found.");
            }

            // Create a file name for the PDF
            string fileName = $"{user.Name} {user.Surname} - {qr.Title} QR Code.pdf";

            // Convert the document to a byte array
            byte[] response;
            using (MemoryStream ms = new MemoryStream())
            {
                document.Save(ms);
                response = ms.ToArray();
            }

            // Return the PDF as a file response
            return File(response, "application/pdf", fileName);

        }*/
       
        [HttpGet("DownloadPDF")]
        public async Task<IActionResult> DownloadPDF(string qrId)
        {
            Qr qr = _context.Qrs.Find(qrId);
            if (qr == null)
            {
                throw new Exception("QR code not found.");
            }

            User user = _context.Users.Find(qr.UserId);
            if (user == null)
            {
                throw new Exception("User not found.");
            }

            string appDownloadQrCodeUrl = "https://api.qrserver.com/v1/create-qr-code/?size=300x300&data=" + qrId;

            // URL for the QR code to send a message
            string messageQrCodeUrl = appDownloadQrCodeUrl;

            // HTML content for the PDF
            string htmlElement = "<div style='width:100%; height:100%; text-align:center; font-family: Arial, sans-serif;'>";
            htmlElement += "<h1 style='color:#333;'>This Connect!</h1>";
            htmlElement += "<p style='font-size:16px; color:#555;'>Scan the QR code below to send a message to the QR code owner.</p>";
            htmlElement += "<img style='width:200px; height:200px; border:2px solid #000;' src='" + messageQrCodeUrl + "'/>";
            htmlElement += "<p style='font-size:14px; color:#777;'>If you have any issues scanning the QR code, please contact support@thisconnect.com.</p>";
            htmlElement += "<hr style='margin:20px 0; border:0; border-top:1px solid #ccc;'/>";
            htmlElement += "<h2 style='color:#333;'>Download the ThisConnect App</h2>";
            htmlElement += "<p style='font-size:16px; color:#555;'>To get started, scan the QR code below to download the ThisConnect app.</p>";
            htmlElement += "<img style='width:100px; height:100px; border:2px solid #000;' src='" + appDownloadQrCodeUrl + "'/>";
            htmlElement += "</div>";


            // Create a file name for the PDF
            string fileName = $"{user.Name} {user.Surname} - {qr.Title} QR Code.pdf";

            // Create a new converter instance for each request
            using (var converter = new SynchronizedConverter(new PdfTools()))
            {
                // ... (HTML content setup)

                var doc = new HtmlToPdfDocument()
                {
                    GlobalSettings = {
                        ColorMode = ColorMode.Color,
                        Orientation = Orientation.Portrait,
                        PaperSize = PaperKind.A4Plus,
                        },
                  Objects = {
                    new ObjectSettings() {
                        PagesCount = true,
                        HtmlContent = htmlElement,
                        WebSettings = { DefaultEncoding = "utf-8" },
                     }
                   }
                };

                byte[] pdf = converter.Convert(doc);
                return File(pdf, "application/pdf", fileName);
            }
        }


    }


}