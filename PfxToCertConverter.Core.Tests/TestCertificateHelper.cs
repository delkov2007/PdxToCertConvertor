using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace PfxToCertConvertor.Core.Tests;

public static class TestCertificateHelper
{
    public static void CreateTestCertificate(string outputPath, string password)
    {
        // Create a new certificate
        var distinguishedName = new X500DistinguishedName("CN=Test Certificate");
        using var rsa = RSA.Create(2048);
        var request = new CertificateRequest(distinguishedName, rsa, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        
        // Add basic constraints
        request.CertificateExtensions.Add(
            new X509BasicConstraintsExtension(true, false, 0, true));

        // Add key usage
        request.CertificateExtensions.Add(
            new X509KeyUsageExtension(
                X509KeyUsageFlags.KeyEncipherment | 
                X509KeyUsageFlags.DataEncipherment | 
                X509KeyUsageFlags.DigitalSignature, 
                true));

        // Create the certificate
        var certificate = request.CreateSelfSigned(
            DateTimeOffset.UtcNow.AddDays(-1),
            DateTimeOffset.UtcNow.AddDays(365));

        // Export as PFX with password
        var pfxBytes = certificate.Export(X509ContentType.Pfx, password);
        File.WriteAllBytes(outputPath, pfxBytes);
    }
} 