namespace PfxToCertConverter.Core.Exceptions;

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