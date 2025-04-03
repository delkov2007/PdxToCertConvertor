using System.Security.Cryptography.X509Certificates;
using PfxToCertConverter.Core.Services;
using PfxToCertConvertor.Core.Tests;
using Xunit;
namespace PfxToCertConverter.Core.Tests;

public class CertificateConverterTests : IDisposable
{
    private readonly ICertificateConverter _converter;
    private readonly string _testPfxPath;
    private readonly string _testPassword;

    public CertificateConverterTests()
    {
        _converter = new CertificateConverter();
        _testPassword = "test123";
        
        // Create TestData directory if it doesn't exist
        var testDataDir = Path.Combine(Directory.GetCurrentDirectory(), "TestData");
        Directory.CreateDirectory(testDataDir);
        
        _testPfxPath = Path.Combine(testDataDir, "test.pfx");
        
        // Create test certificate
        TestCertificateHelper.CreateTestCertificate(_testPfxPath, _testPassword);
    }

    public void Dispose()
    {
        // Clean up test certificate
        if (File.Exists(_testPfxPath))
        {
            File.Delete(_testPfxPath);
        }
    }

    [Fact]
    public async Task ConvertPfxToCerAsync_WithValidPfx_ReturnsCerContent()
    {
        // Arrange
        var pfxContent = File.ReadAllBytes("TestData/test.pfx");
        var password = "test123";

        // Act
        var result = await _converter.ConvertPfxToCerAsync(pfxContent, password);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Length > 0);
    }

    [Fact]
    public async Task ConvertPfxToCerAsync_WithInvalidPfx_ThrowsException()
    {
        // Arrange
        var invalidPfxContent = new byte[] { 1, 2, 3 };
        var password = "test123";

        // Act & Assert
        await Assert.ThrowsAsync<CertificateConversionException>(
            () => _converter.ConvertPfxToCerAsync(invalidPfxContent, password));
    }

    [Fact]
    public async Task ConvertPfxToCerAsync_WithEmptyPfx_ThrowsException()
    {
        // Arrange
        var emptyPfxContent = Array.Empty<byte>();
        var password = "test123";

        // Act & Assert
        var exception = await Assert.ThrowsAsync<CertificateConversionException>(
            () => _converter.ConvertPfxToCerAsync(emptyPfxContent, password));
        
        Assert.Equal(CertificateErrorType.InvalidFileFormat, exception.ErrorType);
    }

    [Fact]
    public async Task ConvertPfxToCerAsync_InvalidPassword_ThrowsException()
    {
        // Arrange
        var pfxContent = await File.ReadAllBytesAsync(_testPfxPath);
        var wrongPassword = "wrongpassword";

        // Act & Assert
        var exception = await Assert.ThrowsAsync<CertificateConversionException>(
            () => _converter.ConvertPfxToCerAsync(pfxContent, wrongPassword));
        
        Assert.Equal(CertificateErrorType.ConversionError, exception.ErrorType);
    }

    [Fact]
    public async Task ConvertPfxToCerAsync_NullContent_ThrowsException()
    {
        // Arrange
        byte[]? pfxContent = null;

        // Act & Assert
        var exception = await Assert.ThrowsAsync<CertificateConversionException>(
            () => _converter.ConvertPfxToCerAsync(pfxContent!, _testPassword));
        
        Assert.Equal(CertificateErrorType.InvalidFileFormat, exception.ErrorType);
    }

    [Fact]
    public async Task ConvertPfxToCerAsync_InvalidPfxContent_ThrowsException()
    {
        // Arrange
        var invalidContent = new byte[] { 1, 2, 3, 4, 5 }; // Not a valid PFX file

        // Act & Assert
        var exception = await Assert.ThrowsAsync<CertificateConversionException>(
            () => _converter.ConvertPfxToCerAsync(invalidContent, _testPassword));
        
        Assert.Equal(CertificateErrorType.ConversionError, exception.ErrorType);
    }
} 