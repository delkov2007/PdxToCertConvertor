using System.Security.Cryptography.X509Certificates;
using PfxToCertConverter.Core.Exceptions;

namespace PfxToCertConverter.Core.Services;

public interface ICertificateConverter
{
    Task<byte[]> ConvertPfxToCerAsync(byte[] pfxContent, string password);
}

public class CertificateConverter : ICertificateConverter
{
    public Task<byte[]> ConvertPfxToCerAsync(byte[] pfxContent, string password)
    {
        try
        {
            if (pfxContent == null || pfxContent.Length == 0)
            {
                throw new CertificateConversionException("PFX file content is empty", CertificateErrorType.InvalidFileFormat);
            }

            // Load the PFX certificate and export as CER (X.509) on a background thread
            return Task.Run(() =>
            {
                try
                {
                    var certificate = new X509Certificate2(pfxContent, password);
                    return certificate.Export(X509ContentType.Cert);
                }
                catch (Exception ex)
                {
                    throw new CertificateConversionException(
                        "Failed to convert PFX to CER: " + ex.Message,
                        CertificateErrorType.ConversionError);
                }
            });
        }
        catch (CertificateConversionException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new CertificateConversionException(
                "Failed to convert PFX to CER: " + ex.Message,
                CertificateErrorType.ConversionError);
        }
    }
}

public enum CertificateErrorType
{
    InvalidFileFormat,
    InvalidPassword,
    ConversionError
}

public class CertificateConversionException : Exception
{
    public CertificateErrorType ErrorType { get; }

    public CertificateConversionException(string message, CertificateErrorType errorType)
        : base(message)
    {
        ErrorType = errorType;
    }
} 