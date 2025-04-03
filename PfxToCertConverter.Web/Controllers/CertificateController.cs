using Microsoft.AspNetCore.Mvc;
using PfxToCertConverter.Core.Services;

namespace PfxToCertConverter.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CertificateController : ControllerBase
{
    private readonly ICertificateConverter _certificateConverter;

    public CertificateController(ICertificateConverter certificateConverter)
    {
        _certificateConverter = certificateConverter;
    }

    [HttpPost("convert")]
    public async Task<IActionResult> ConvertPfxToCer(IFormFile pfxFile, [FromForm] string password)
    {
        if (pfxFile == null || pfxFile.Length == 0)
        {
            return BadRequest("No file uploaded");
        }

        try
        {
            using var stream = pfxFile.OpenReadStream();
            using var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream);
            var pfxContent = memoryStream.ToArray();

            var cerContent = await _certificateConverter.ConvertPfxToCerAsync(pfxContent, password);
            return File(cerContent, "application/x-x509-ca-cert", "certificate.cer");
        }
        catch (Exception ex)
        {
            return BadRequest($"Conversion failed: {ex.Message}");
        }
    }
} 