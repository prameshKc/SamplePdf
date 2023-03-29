using iText.Html2pdf;
using iText.Kernel.Pdf;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IActionResult Get()
        {
            var pdfBytes = GeneratePdfFromHtml("<html><body><h1>Hello, Health Teams!</h1></body></html>");
            // Return the PDF to the client
            return new FileContentResult(pdfBytes, "application/pdf")
            {
                FileDownloadName = "example.pdf"
            };
        }

        [NonAction]

        public byte[] GeneratePdfFromHtml(string html)
        {
            // Create a new MemoryStream object to write the PDF to
            using (var stream = new MemoryStream())
            {
                // Create a new PdfWriter object to write to the stream
                using (var writer = new PdfWriter(stream))
                {
                    // Create a new PdfDocument object
                    using (var pdf = new PdfDocument(writer))
                    {
                        // Create a new ConverterProperties object to specify the conversion options
                        var props = new ConverterProperties();
                        // Use the HtmlConverter to convert the HTML to PDF and write to the PDF document
                        HtmlConverter.ConvertToPdf(html, pdf, props);
                        // Flush the PDF document to ensure that all content is written to the stream
                        pdf.Close();
                        writer.Flush();
                        stream.Flush();
                        // Return the stream as a byte array
                        return stream.ToArray();
                    }
                }
            }
        }
    }
}