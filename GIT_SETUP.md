# Git Configuration Guide

## .gitignore Files

This project has comprehensive `.gitignore` files to ensure sensitive and generated files are not tracked in version control.

### Root .gitignore (/)

Located at the repository root, this file ignores:

**Build Artifacts:**
- `bin/`, `obj/` - .NET build outputs
- `Debug/`, `Release/` - Build configurations
- `dist/`, `dist-ssr/` - Frontend build outputs
- `publish/` - Published applications

**IDE and Editor Files:**
- `.vs/` - Visual Studio cache
- `.vscode/` - VS Code settings (except specific files)
- `.idea/` - JetBrains Rider
- `*.suo`, `*.user` - User-specific settings

**Dependencies:**
- `node_modules/` - npm packages
- `packages/` - NuGet packages (except build tools)

**Environment and Secrets:**
- `.env`, `.env.local`, `.env.*.local` - Environment variables
- `appsettings.Development.json` - Local development settings (if created)

**Logs:**
- `logs/`, `*.log` - Application logs
- `npm-debug.log*` - npm debug logs

**Database Files:**
- `*.db`, `*.sqlite` - Local database files

**OS Files:**
- `.DS_Store` - macOS
- `Thumbs.db` - Windows

**Solution Files:**
- ✅ `*.sln` files ARE tracked (negated with `!*.sln`)
- ✅ `*.csproj` files ARE tracked (not excluded)

### Client .gitignore (client/)

Additional frontend-specific exclusions:

**Build and Dependencies:**
- `node_modules/` - npm packages
- `dist/` - Production build
- `dist-ssr/` - SSR build
- `build/` - Alternative build output

**Environment:**
- `.env` - All environment files
- `.env.local`, `.env.*.local`

**Testing:**
- `coverage/` - Test coverage reports
- `.nyc_output/` - NYC coverage tool output

**Temporary:**
- `*.local` - Local temporary files

## .gitattributes

Ensures consistent line endings and proper handling of different file types:

**Text Files:**
- `*.cs`, `*.ts`, `*.tsx`, `*.js`, `*.jsx` - Auto LF normalization
- `*.json`, `*.xml`, `*.md` - Text handling
- `*.sh` - Unix line endings (LF)
- `*.ps1` - Windows line endings (CRLF)

**Binary Files:**
- `*.dll`, `*.exe` - .NET binaries
- `*.png`, `*.jpg`, etc. - Images
- `*.woff`, `*.ttf` - Fonts
- `*.zip`, `*.gz` - Archives

## What SHOULD Be Committed

### Backend
✅ Source code (`*.cs`)
✅ Project files (`*.csproj`)
✅ Solution file (`*.sln`)
✅ Configuration templates (`appsettings.json`)
✅ Database migrations (`/Migrations/*.cs`)
✅ Documentation (`*.md`)

### Frontend
✅ Source code (`*.ts`, `*.tsx`)
✅ Package configuration (`package.json`, `package-lock.json`)
✅ Build configuration (`vite.config.ts`, `tsconfig.json`)
✅ Environment template (`.env.example`)
✅ Tailwind config (`tailwind.config.js`)
✅ Public assets (`public/`)

## What MUST NOT Be Committed

❌ **Environment Variables:**
- `.env`, `.env.local` with real API keys
- `appsettings.Development.json` with secrets
- Any file containing Okta credentials

❌ **Build Outputs:**
- `bin/`, `obj/` directories
- `dist/`, `build/` directories
- `node_modules/` directory

❌ **Sensitive Data:**
- Database connection strings with passwords
- JWT signing keys
- API keys and secrets
- User data or PII

❌ **IDE/User-Specific:**
- `.vs/`, `.vscode/settings.json` (user-specific)
- `*.suo`, `*.user` files
- Local debugging configurations

## Setting Up Environment Files

### Backend Environment Setup

1. **Development:** Create `appsettings.Development.json` (git-ignored):
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=HumbleDeskDB;Trusted_Connection=True;"
  },
  "Okta": {
    "Authority": "https://dev-xxxxx.okta.com/oauth2/default",
    "Audience": "api://default"
  }
}
```

2. **Production:** Use Azure App Service Configuration or Key Vault

### Frontend Environment Setup

1. **Development:** Copy `.env.example` to `.env.local`:
```bash
cd client
cp .env.example .env.local
```

2. Edit `.env.local` with your values:
```env
VITE_API_BASE_URL=https://localhost:7001/api
VITE_OKTA_ISSUER=https://dev-xxxxx.okta.com/oauth2/default
VITE_OKTA_CLIENT_ID=0oaxxxxxxxxxxxxx
```

3. **Production:** Set environment variables in CI/CD pipeline

## Initial Git Setup

```bash
# Initialize repository (if not already done)
git init

# Add all files (respecting .gitignore)
git add .

# Check what will be committed
git status

# Verify no sensitive files are staged
git status | grep -E "\.env|appsettings\.Development"

# First commit
git commit -m "Initial commit: HumbleDesk AI-Powered Ticketing System with Clean Architecture"

# Add remote and push
git remote add origin https://github.com/yourusername/humbledesk.git
git branch -M main
git push -u origin main
```

## Verifying .gitignore is Working

```bash
# Check if environment files are ignored
git check-ignore -v .env
git check-ignore -v client/.env.local

# List all ignored files
git status --ignored

# See what would be committed
git add --dry-run .
```

## Common Issues and Solutions

### Issue: Accidentally Committed .env File

```bash
# Remove from Git but keep local file
git rm --cached .env
git rm --cached client/.env.local

# Commit the removal
git commit -m "Remove environment files from version control"

# Push changes
git push
```

### Issue: .gitignore Not Working for Already Tracked Files

```bash
# Clear Git cache
git rm -r --cached .

# Re-add all files (now respecting .gitignore)
git add .

# Commit
git commit -m "Apply .gitignore rules to existing files"
```

### Issue: Binary Files Showing as Modified

- This is likely a line ending issue
- Ensure `.gitattributes` is committed
- Run `git add --renormalize .`

## Security Checklist Before Pushing

- [ ] No `.env` files in staging area
- [ ] No connection strings with passwords
- [ ] No Okta client secrets (SPA uses PKCE, no secret needed)
- [ ] No API keys or tokens
- [ ] No user data or PII
- [ ] `appsettings.json` only has placeholder values
- [ ] `.env.example` only has placeholder values

## Pre-commit Hook (Optional)

Create `.git/hooks/pre-commit`:

```bash
#!/bin/sh

# Check for sensitive files
if git diff --cached --name-only | grep -E "\.env$|\.env\.local|appsettings\.Development\.json"; then
    echo "ERROR: Attempting to commit sensitive environment files!"
    echo "Please unstage these files before committing."
    exit 1
fi

# Check for potential secrets in code
if git diff --cached | grep -iE "password|secret|apikey"; then
    echo "WARNING: Potential secrets detected in staged changes!"
    echo "Please review your changes carefully."
    read -p "Continue with commit? (y/n) " -n 1 -r
    echo
    if [[ ! $REPLY =~ ^[Yy]$ ]]; then
        exit 1
    fi
fi
```

Make it executable:
```bash
chmod +x .git/hooks/pre-commit
```

## GitHub Repository Settings

### Branch Protection Rules (Recommended)

For `main` branch:
- ✅ Require pull request reviews before merging
- ✅ Require status checks to pass before merging
- ✅ Require branches to be up to date before merging
- ✅ Include administrators

### Repository Secrets (for CI/CD)

Add these secrets in GitHub repository settings:
- `AZURE_WEBAPP_PUBLISH_PROFILE` - For API deployment
- `AZURE_STATIC_WEB_APPS_API_TOKEN` - For SPA deployment
- `API_BASE_URL` - Production API URL
- `OKTA_ISSUER` - Okta issuer URL
- `OKTA_CLIENT_ID` - Okta client ID for production

### .gitignore Template Used

This project uses a combination of:
- [github/gitignore/VisualStudio.gitignore](https://github.com/github/gitignore/blob/main/VisualStudio.gitignore)
- [github/gitignore/Node.gitignore](https://github.com/github/gitignore/blob/main/Node.gitignore)
- Custom additions for this specific project structure
