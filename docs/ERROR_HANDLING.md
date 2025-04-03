# Error Handling Guide

This document describes the error handling mechanisms in the PFX to CER Certificate Converter application.

## Error Types

The application uses a custom exception type `CertificateConversionException` with the following error categories:

### 1. InvalidFileFormat

- **Description**: The input file is not a valid PFX certificate
- **Cause**: The file is either not a PFX file or is corrupted
- **Resolution**: Ensure you're selecting a valid PFX certificate file
- **HTTP Status**: 400 Bad Request

### 2. InvalidPassword

- **Description**: The provided password is incorrect
- **Cause**: Wrong password entered for the PFX certificate
- **Resolution**: Verify and re-enter the correct certificate password
- **HTTP Status**: 400 Bad Request

### 3. ConversionError

- **Description**: Error during certificate conversion
- **Causes**:
  - Certificate corruption
  - Unsupported certificate format
  - Internal conversion error
- **Resolution**: Verify the certificate integrity or try with a different certificate
- **HTTP Status**: 400 Bad Request

## Error Handling in API

The application handles errors at multiple levels:

1. **Controller Level**:

   ```csharp
   try
   {
       var cerContent = await _certificateConverter.ConvertPfxToCerAsync(pfxContent, password);
       return File(cerContent, "application/x-x509-ca-cert", "certificate.cer");
   }
   catch (Exception ex)
   {
       return BadRequest($"Conversion failed: {ex.Message}");
   }
   ```

2. **Service Level**:

   ```csharp
   public async Task<byte[]> ConvertPfxToCerAsync(byte[] pfxContent, string password)
   {
       try
       {
           if (pfxContent == null || pfxContent.Length == 0)
           {
               throw new CertificateConversionException(
                   "PFX file content is empty",
                   CertificateErrorType.InvalidFileFormat);
           }

           var certificate = new X509Certificate2(pfxContent, password);
           return certificate.Export(X509ContentType.Cert);
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
   ```

## Error Handling in UI

The web interface handles errors by:

1. **Displaying Error Messages**:

   ```javascript
   try {
     const response = await fetch("/api/certificate/convert", {
       method: "POST",
       body: formData,
     });

     if (!response.ok) {
       const error = await response.text();
       throw new Error(error);
     }
   } catch (error) {
     errorMessage.textContent = error.message;
     errorMessage.style.display = "block";
   }
   ```

2. **Visual Feedback**:
   - Error messages in red
   - Success messages in green
   - Button state changes
   - Loading indicators

## Best Practices

1. **Input Validation**:

   - Verify file type before upload
   - Validate password presence
   - Check file size limits

2. **Security**:

   - Never store passwords
   - Process files in memory
   - Clean up resources properly

3. **User Experience**:
   - Clear error messages
   - Visual feedback
   - Recovery suggestions

## Common Issues and Solutions

1. **"Invalid PFX file format"**

   - Verify the file is a valid PFX certificate
   - Check if the file is corrupted
   - Try exporting the certificate again

2. **"Incorrect password"**

   - Double-check the password
   - Verify caps lock is off
   - Ensure no extra spaces

3. **"Conversion failed"**
   - Check if the certificate is valid
   - Verify the certificate hasn't expired
   - Try with a different browser

## Error Prevention

To prevent errors:

1. **Validate Input**

   - Check file existence
   - Verify file permissions
   - Validate file format before processing

2. **Check Resources**

   - Verify available disk space
   - Check memory availability
   - Validate system capabilities

3. **Handle Edge Cases**
   - Empty files
   - Very large files
   - Network interruptions
   - System resource constraints

## Troubleshooting

Common issues and solutions:

1. **"Invalid PDX file format"**

   - Verify file extension
   - Check file contents
   - Ensure file is not corrupted

2. **"File access denied"**

   - Check file permissions
   - Close other applications using the file
   - Run application with appropriate privileges

3. **"Conversion failed"**
   - Check available disk space
   - Verify file is not read-only
   - Ensure file is not locked

## Logging

The application logs errors with the following information:

```json
{
  "timestamp": "2024-03-03T12:34:56Z",
  "errorType": "InvalidFileFormat",
  "message": "Detailed error message",
  "file": {
    "name": "example.pdx",
    "size": 1234,
    "path": "/path/to/file"
  },
  "stackTrace": "...",
  "innerException": "..."
}
```
