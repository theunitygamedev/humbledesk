# Deployment Guide

## Azure Deployment (Recommended)

### Prerequisites
- Azure Subscription
- Azure CLI installed
- Okta account configured

### 1. Create Azure Resources

```bash
# Login to Azure
az login

# Create resource group
az group create --name coi-rg --location eastus

# Create App Service Plan (Linux)
az appservice plan create \
  --name coi-plan \
  --resource-group coi-rg \
  --sku B1 \
  --is-linux

# Create API App Service
az webapp create \
  --resource-group coi-rg \
  --plan coi-plan \
  --name coi-api \
  --runtime "DOTNETCORE:8.0"

# Create SQL Server
az sql server create \
  --name coi-sql-server \
  --resource-group coi-rg \
  --location eastus \
  --admin-user sqladmin \
  --admin-password [SecurePassword123!]

# Create SQL Database
az sql db create \
  --resource-group coi-rg \
  --server coi-sql-server \
  --name coiDB \
  --service-objective S0

# Create Azure Key Vault
az keyvault create \
  --name coi-keyvault \
  --resource-group coi-rg \
  --location eastus
```

### 2. Configure App Service

```bash
# Enable managed identity
az webapp identity assign \
  --resource-group coi-rg \
  --name coi-api

# Configure connection string
az webapp config connection-string set \
  --resource-group coi-rg \
  --name coi-api \
  --settings DefaultConnection="Server=tcp:coi-sql-server.database.windows.net,1433;Database=coiDB;User ID=sqladmin;Password=[SecurePassword123!];" \
  --connection-string-type SQLAzure

# Configure app settings
az webapp config appsettings set \
  --resource-group coi-rg \
  --name coi-api \
  --settings \
    ASPNETCORE_ENVIRONMENT=Production \
    Okta__Authority=https://your-domain.okta.com/oauth2/default \
    Okta__Audience=api://default
```

### 3. Deploy Backend

```bash
# Publish the application
cd src/COI.API
dotnet publish -c Release -o ./publish

# Deploy to Azure
az webapp deploy \
  --resource-group coi-rg \
  --name coi-api \
  --src-path ./publish
```

### 4. Run Database Migrations

Option A: Run locally against Azure SQL:
```bash
# Update connection string in appsettings.json temporarily
dotnet ef database update --project ../COI.Infrastructure --startup-project .
```

Option B: Enable migration on startup (add to Program.cs):
```csharp
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
}
```

### 5. Deploy Frontend

Option A: Azure Static Web Apps
```bash
cd client
npm run build

# Create static web app
az staticwebapp create \
  --name coi-spa \
  --resource-group coi-rg \
  --source ./dist
```

Option B: Azure App Service
```bash
# Create App Service for SPA
az webapp create \
  --resource-group coi-rg \
  --plan coi-plan \
  --name coi-spa \
  --runtime "NODE:18-lts"

# Deploy built files
cd client
npm run build
az webapp deploy \
  --resource-group coi-rg \
  --name coi-spa \
  --src-path ./dist \
  --type static
```

### 6. Configure Azure Front Door (Optional but Recommended)

```bash
# Create Front Door
az afd profile create \
  --profile-name coi-afd \
  --resource-group coi-rg \
  --sku Standard_AzureFrontDoor

# Add endpoint
az afd endpoint create \
  --resource-group coi-rg \
  --profile-name coi-afd \
  --endpoint-name coi-endpoint

# Configure origins (API and SPA)
# Configure WAF policy
# Configure custom domains and SSL
```

### 7. Configure Application Insights

```bash
# Create Application Insights
az monitor app-insights component create \
  --app coi-insights \
  --location eastus \
  --resource-group coi-rg

# Get instrumentation key
az monitor app-insights component show \
  --app coi-insights \
  --resource-group coi-rg \
  --query instrumentationKey -o tsv

# Add to App Service settings
az webapp config appsettings set \
  --resource-group coi-rg \
  --name coi-api \
  --settings APPLICATIONINSIGHTS_CONNECTION_STRING="[connection-string]"
```

## Environment Variables

### Backend (Azure App Service Configuration)
```
ConnectionStrings__DefaultConnection=[Azure SQL connection string]
Okta__Authority=https://your-domain.okta.com/oauth2/default
Okta__Audience=api://default
Cors__AllowedOrigins__0=https://your-spa-domain.azurewebsites.net
APPLICATIONINSIGHTS_CONNECTION_STRING=[App Insights connection]
```

### Frontend (Build-time variables)
```
VITE_API_BASE_URL=https://coi-api.azurewebsites.net/api
VITE_OKTA_ISSUER=https://your-domain.okta.com/oauth2/default
VITE_OKTA_CLIENT_ID=[your-spa-client-id]
```

## Security Checklist

- [ ] SQL Server firewall configured (Azure services + specific IPs)
- [ ] Managed Identity enabled for App Service
- [ ] Key Vault access policies configured
- [ ] HTTPS Only enabled on App Services
- [ ] CORS configured with specific origins (no wildcards)
- [ ] Okta applications configured with correct redirect URIs
- [ ] Application Insights configured
- [ ] WAF enabled on Front Door (if using)
- [ ] Custom domain and SSL certificates configured
- [ ] Backup policy configured for SQL Database
- [ ] Monitoring and alerts set up

## CI/CD with GitHub Actions

Create `.github/workflows/deploy.yml`:

```yaml
name: Deploy to Azure

on:
  push:
    branches: [ main ]

jobs:
  build-and-deploy-api:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v2

      - name: Setup .NET
        uses: actions/setup-dotnet@v1
        with:
          dotnet-version: '8.0.x'

      - name: Build
        run: dotnet build --configuration Release
        working-directory: ./src/COI.API

      - name: Publish
        run: dotnet publish -c Release -o ./publish
        working-directory: ./src/COI.API

      - name: Deploy to Azure
        uses: azure/webapps-deploy@v2
        with:
          app-name: 'coi-api'
          publish-profile: ${{ secrets.AZURE_WEBAPP_PUBLISH_PROFILE }}
          package: ./src/COI.API/publish

  build-and-deploy-spa:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v2

      - name: Setup Node.js
        uses: actions/setup-node@v2
        with:
          node-version: '18'

      - name: Install dependencies
        run: npm ci
        working-directory: ./client

      - name: Build
        run: npm run build
        working-directory: ./client
        env:
          VITE_API_BASE_URL: ${{ secrets.API_BASE_URL }}
          VITE_OKTA_ISSUER: ${{ secrets.OKTA_ISSUER }}
          VITE_OKTA_CLIENT_ID: ${{ secrets.OKTA_CLIENT_ID }}

      - name: Deploy to Azure Static Web Apps
        uses: Azure/static-web-apps-deploy@v1
        with:
          azure_static_web_apps_api_token: ${{ secrets.AZURE_STATIC_WEB_APPS_API_TOKEN }}
          repo_token: ${{ secrets.GITHUB_TOKEN }}
          action: 'upload'
          app_location: './client/dist'
```

## Monitoring

### Application Insights Queries

**Failed Requests:**
```kusto
requests
| where success == false
| project timestamp, name, resultCode, duration
| order by timestamp desc
```

**Slow Queries:**
```kusto
dependencies
| where type == "SQL"
| where duration > 1000
| project timestamp, name, duration, resultCode
| order by duration desc
```

**User Activity:**
```kusto
customEvents
| where name == "UserAction"
| summarize count() by tostring(customDimensions.Action)
```

## Troubleshooting

### API not responding
1. Check App Service logs: `az webapp log tail --name coi-api --resource-group coi-rg`
2. Verify connection string in configuration
3. Check SQL Server firewall rules
4. Verify managed identity has access to resources

### Authentication issues
1. Verify Okta configuration matches environment
2. Check CORS settings
3. Verify JWT token expiration
4. Check Okta trusted origins configuration

### Database connection issues
1. Verify SQL Server firewall allows Azure services
2. Test connection string locally
3. Check managed identity permissions
4. Verify database migrations have run
