# Quick Start Guide

## Prerequisites Checklist

- [ ] .NET 8 SDK installed
- [ ] Node.js 18+ and npm installed
- [ ] SQL Server or LocalDB installed
- [ ] Okta Developer Account created
- [ ] Git installed

## 1. Clone and Setup (First Time)

```bash
# Clone repository
git clone https://github.com/yourusername/humbledesk.git
cd humbledesk

# Verify .gitignore files are present
ls -la .gitignore
ls -la client/.gitignore
```

## 2. Backend Setup

### Configure Environment

Create `src/HD.API/appsettings.Development.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=HumbleDeskDB;Trusted_Connection=True;MultipleActiveResultSets=true"
  },
  "Okta": {
    "Authority": "https://dev-XXXXXXX.okta.com/oauth2/default",
    "Audience": "api://default"
  },
  "Cors": {
    "AllowedOrigins": ["http://localhost:5174"]
  }
}
```

### Install and Run

```bash
# Restore packages
dotnet restore

# Create database
cd src/HD.Infrastructure
dotnet ef migrations add InitialCreate --startup-project ../HD.API
dotnet ef database update --startup-project ../HD.API

# Run API
cd ../HD.API
dotnet run

# API will be available at https://localhost:7200
# Swagger UI at https://localhost:7200/swagger
```

## 3. Frontend Setup

### Configure Environment

```bash
cd client

# Copy environment template
cp .env.example .env.local
```

Edit `client/.env.local`:
```env
VITE_API_BASE_URL=https://localhost:7200/api
VITE_OKTA_ISSUER=https://dev-XXXXXXX.okta.com/oauth2/default
VITE_OKTA_CLIENT_ID=0oaXXXXXXXXXXXXXXXX
```

### Install and Run

```bash
# Install dependencies
npm install

# Run development server
npm run dev

# Open browser to http://localhost:5174
```

## 4. Okta Configuration

### Create API Services Application (Backend)

1. Sign in to Okta Admin Console
2. Applications → Create App Integration
3. Choose "API Services"
4. Note the **Issuer** (in Security → API)
5. Note the **Audience** (usually `api://default`)

### Create Single-Page Application (Frontend)

1. Applications → Create App Integration
2. Choose "OIDC - OpenID Connect"
3. Choose "Single-Page Application"
4. Configure:
   - **App Name:** HumbleDesk
   - **Grant Type:** Authorization Code (PKCE enabled automatically)
   - **Sign-in redirect URIs:** `http://localhost:5174/login/callback`
   - **Sign-out redirect URIs:** `http://localhost:5174`
   - **Trusted Origins:**
     - `http://localhost:5174` (Type: CORS and Redirect)
5. Save and note the **Client ID**

### Add Users and Groups

1. Directory → Groups → Add Group
   - Create groups: `Employees`, `BoardDirectors`, `LegalAdmin`, `Reviewers`
2. Directory → People → Add Person
3. Assign people to groups
4. Assign application to users

## 5. Verify Setup

### Test Backend

```bash
# Health check
curl https://localhost:7200/health

# Swagger UI
# Open https://localhost:7200/swagger in browser
```

### Test Frontend

1. Open http://localhost:5174
2. Click "Sign in with Okta"
3. Complete Okta login
4. Should redirect to Dashboard

### Test Authentication Flow

1. Frontend → Login → Redirects to Okta
2. Okta → Authenticate → Redirects back with code
3. Frontend → Exchanges code for tokens (PKCE)
4. Frontend → Makes API call with Bearer token
5. Backend → Validates JWT → Returns data

## Common Issues and Solutions

### Issue: "Unable to connect to database"

**Solution:**
```bash
# Check LocalDB is running
sqllocaldb info
sqllocaldb start MSSQLLocalDB

# Or update connection string to use SQL Server instance
```

### Issue: "401 Unauthorized" from API

**Solution:**
1. Verify Okta Authority matches in both backend and frontend
2. Check token in browser DevTools → Application → Local Storage
3. Verify API is configured to trust Okta issuer
4. Check CORS settings allow frontend origin

### Issue: "Okta redirect loop"

**Solution:**
1. Verify redirect URIs in Okta app exactly match
2. Check Trusted Origins includes frontend URL
3. Clear browser cache and local storage
4. Ensure PKCE is enabled (default for SPA)

### Issue: "CORS error" in browser console

**Solution:**
1. Check backend `appsettings.json` → `Cors:AllowedOrigins`
2. Verify frontend URL matches exactly (including port)
3. Ensure API is running with CORS middleware

### Issue: "Cannot find module" errors in frontend

**Solution:**
```bash
cd client
rm -rf node_modules package-lock.json
npm install
```

### Issue: Build errors in backend

**Solution:**
```bash
# Clean and rebuild
dotnet clean
dotnet restore
dotnet build
```

## Project Structure Quick Reference

```
conflict/
├── src/
│   ├── HD.Domain/           # ← Entities, enums (no dependencies)
│   ├── HD.Application/      # ← Use cases, DTOs, interfaces
│   ├── HD.Infrastructure/   # ← EF Core, database
│   └── HD.API/              # ← Web API, controllers
├── client/                   # ← React frontend
│   ├── src/
│   │   ├── config/          # ← Okta config
│   │   ├── lib/             # ← API client
│   │   ├── pages/           # ← Components
│   │   └── types/           # ← TypeScript types
│   └── .env.local          # ← Environment (not committed!)
├── .gitignore               # ← Root git ignore
└── README.md                # ← Full documentation
```

## Development Workflow

### Making Changes

```bash
# Create feature branch
git checkout -b feature/your-feature

# Make changes to code
# ...

# Check what changed
git status
git diff

# Stage changes
git add .

# Verify no sensitive files
git status | grep -E "\.env|Development\.json"

# Commit
git commit -m "Add your feature"

# Push
git push origin feature/your-feature
```

### Database Changes

```bash
# Add migration
cd src/HD.Infrastructure
dotnet ef migrations add YourMigrationName --startup-project ../HD.API

# Review generated migration in Migrations/ folder

# Apply migration
dotnet ef database update --startup-project ../HD.API
```

### Frontend Changes

```bash
cd client

# Install new package
npm install package-name

# Update package
npm update package-name

# Build for production
npm run build

# Preview production build
npm run preview
```

## Environment Files Checklist

### Backend
- [ ] `appsettings.json` - Template (committed)
- [ ] `appsettings.Development.json` - Local dev (NOT committed)
- [ ] Azure App Service Configuration - Production (not in repo)

### Frontend
- [ ] `.env.example` - Template (committed)
- [ ] `.env.local` - Local dev (NOT committed)
- [ ] CI/CD secrets - Production (in GitHub/Azure)

## Next Steps

1. ✅ Backend running at https://localhost:7200
2. ✅ Frontend running at http://localhost:5174
3. ✅ Can authenticate with Okta
4. ✅ Can access Swagger UI

**Now you can start developing:**

- Create more API endpoints in `src/HD.API/Controllers/`
- Add use cases in `src/HD.Application/`
- Build UI components in `client/src/`
- Add database entities in `src/HD.Domain/Entities/`

## Resources

- [Full Documentation](README.md)
- [Deployment Guide](DEPLOYMENT.md)
- [Git Configuration](GIT_SETUP.md)
- [Okta Developer Docs](https://developer.okta.com/docs/)
- [.NET Documentation](https://docs.microsoft.com/dotnet/)
- [React Documentation](https://react.dev/)

## Getting Help

- Check existing issues in repository
- Review logs in `src/HD.API/logs/`
- Use browser DevTools for frontend issues
- Check Application Insights (if configured)
