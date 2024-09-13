/*using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PdfSharpCore;
using PdfSharpCore.Pdf;
using TheArtOfDev.HtmlRenderer.PdfSharp;

namespace ThisConnect_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PdfController : ControllerBase
    {
        [HttpGet("generatepdf")]
        public async Task<IActionResult> GeneratePDF()
        {
            var document = new PdfDocument();
            string imgeurl = "data:image/png;base64, " + Convert.ToBase64String(System.IO.File.ReadAllBytes("C:/Projects/ThisConnect/ThisConnect_WebApi/Assets/Images/qr.png"));
            string htmlelement = "<div style='width:100%; height:100%; text-align:center'>";
            htmlelement += "<h2>Welcome to Nihira Techiees</h2>";
            htmlelement += "<img style='width:200px;height:200px' src='" + imgeurl + "'/>";
            
            htmlelement += "</div>";
            PdfGenerator.AddPdfPages(document, htmlelement, PageSize.A4);
            byte[] response = null;
            using (MemoryStream ms = new MemoryStream())
            {
                document.Save(ms);
                response = ms.ToArray();
            }
            return File(response, "application/pdf", "PDFwithImage.pdf");
        }

    }

}
*/