# PFX to CER Certificate Converter

A .NET Core web application for converting PFX (Personal Information Exchange) certificates to CER (X.509 Certificate) format. This application provides a modern web interface for converting PFX files into the standardized CER format.

## Features

- Modern, user-friendly web interface
- Drag-and-drop file upload
- Password-protected PFX file support
- Secure certificate conversion
- Detailed error reporting
- Cross-platform support
- Immediate file download after conversion

## Requirements

- .NET 8.0 SDK or later
- A modern web browser (Chrome, Firefox, Safari, Edge)

## Installation

1. Clone the repository:

   ```bash
   git clone https://github.com/yourusername/PfxToCertConverter.git
   ```

2. Navigate to the project directory:

   ```bash
   cd PfxToCertConverter
   ```

3. Run the application:

   ```bash
   cd PfxToCertConverter.Web
   dotnet run
   ```

4. Open your web browser and navigate to:
   - Web Interface: http://localhost:5281
   - Swagger API Documentation: http://localhost:5281/swagger

## Usage

1. Open the web interface in your browser
2. Either drag and drop your PFX file onto the upload area or click to select a file
3. Enter the password for your PFX certificate
4. Click "Convert to CER"
5. The converted CER file will be automatically downloaded

## Project Structure

- `PfxToCertConverter.Web`: ASP.NET Core web application
  - `Controllers`: API endpoints
  - `wwwroot`: Static web files
  - `Program.cs`: Application configuration
- `PfxToCertConverter.Core`: Core conversion logic
  - `Services`: Certificate conversion implementation
  - `Exceptions`: Custom exception types
- `PfxToCertConverter.Core.Tests`: Unit tests

## API Endpoints

### POST /api/certificate/convert

Converts a PFX certificate to CER format.

**Request:**

- Content-Type: `multipart/form-data`
- Parameters:
  - `pfxFile`: The PFX certificate file
  - `password`: The certificate password

**Response:**

- Content-Type: `application/x-x509-ca-cert`
- File download with `.cer` extension

## Error Handling

The application handles various error scenarios:

- Invalid file format
- Incorrect password
- File corruption
- I/O errors
- Server errors

Each error returns an appropriate HTTP status code and a descriptive message.

## Contributing

1. Fork the repository
2. Create a feature branch
3. Commit your changes
4. Push to the branch
5. Create a Pull Request

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Security Considerations

- Certificate passwords are only used for conversion and are never stored
- Files are processed in memory and not saved to disk
- HTTPS is enforced in production
- Cross-Origin Resource Sharing (CORS) is configured for security

## Azure Deployment (Free Tier)

### Prerequisites

- Azure subscription
- Azure CLI installed
- .NET 8.0 SDK
- Git

### Free Tier Limitations

- 60 minutes of compute per day
- 1GB RAM and 1GB storage
- Windows hosting only (no Linux support)
- Shared infrastructure
- No custom domain support
- No SSL for custom domains
- Limited scaling capabilities

### Deployment Steps

1. Login to Azure:

   ```bash
   az login
   ```

2. Create a resource group:

   ```bash
   az group create --name pfx-converter-rg --location westeurope
   ```

3. Create a Free tier App Service plan:

   ```bash
   az appservice plan create --name pfx-converter-plan --resource-group pfx-converter-rg --sku F1 --is-linux false
   ```

4. Create a Web App:

   ```bash
   az webapp create --name pfx-converter-app --resource-group pfx-converter-rg --plan pfx-converter-plan --runtime "DOTNETCORE:8.0"
   ```

5. Configure the Web App:

   ```bash
   az webapp config set --name pfx-converter-app --resource-group pfx-converter-rg --http20-enabled true
   ```

6. Deploy from local Git repository:

   ```bash
   # Build the application
   dotnet publish -c Release

   # Set up deployment credentials
   az webapp deployment user set --user-name $YOUR_USERNAME --password $YOUR_PASSWORD

   # Get the deployment URL
   az webapp deployment source config-local-git --name pfx-converter-app --resource-group pfx-converter-rg

   # Add Azure as a remote
   git remote add azure <deployment_url>

   # Push to Azure
   git push azure main
   ```

7. Access your application:
   - Web Interface: http://pfx-converter-app.azurewebsites.net
   - Swagger Documentation: http://pfx-converter-app.azurewebsites.net/swagger

### Free Tier Optimization

1. Resource Usage:

   - Monitor daily compute usage to stay within 60 minutes limit
   - Implement efficient caching strategies
   - Minimize background processes
   - Use client-side processing where possible

2. Storage Management:

   - Keep deployment package size small
   - Clean up temporary files
   - Implement proper garbage collection
   - Monitor storage usage to stay within 1GB limit

3. Performance Tips:
   - Minimize application startup time
   - Optimize database queries
   - Use compression for responses
   - Implement client-side caching

### Monitoring in Free Tier

1. Basic Metrics:

   - Monitor CPU time usage
   - Track memory consumption
   - Watch storage utilization
   - Check response times

2. Logs:
   - Enable basic logging
   - Monitor application errors
   - Track usage patterns
   - Set up alerts for resource limits

### Troubleshooting Free Tier Issues

1. Application Stops:

   - Check if daily compute time limit is reached
   - Verify memory usage
   - Monitor concurrent connections

2. Slow Performance:

   - Check if running on shared infrastructure
   - Optimize application code
   - Implement caching where possible

3. Deployment Issues:
   - Verify package size is within limits
   - Check deployment logs
   - Ensure correct runtime version

## Azure-Specific Security

- Enable Azure AD authentication
- Use Managed Identities for Azure resources
- Configure IP restrictions if needed
- Enable Azure Web Application Firewall
- Set up SSL/TLS certificates
- Configure backup and disaster recovery
