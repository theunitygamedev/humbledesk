# HumbleDesk - AI-Powered Ticketing System

A multi-tenant SaaS helpdesk platform designed for IT departments, service vendors, and managed providers to manage, triage, and resolve support requests efficiently with AI assistance.

## Architecture

This project follows **Clean Architecture** principles with clear separation of concerns:

- **Domain Layer** (`HD.Domain`): Core business entities, enums, and domain logic - no external dependencies
- **Application Layer** (`HD.Application`): Use cases, DTOs, interfaces, MediatR commands/queries, FluentValidation
- **Infrastructure Layer** (`HD.Infrastructure`): EF Core, database configurations, external service implementations
- **API Layer** (`HD.API`): ASP.NET Core Web API, controllers, authentication, middleware
- **Client** (`client/`): React 18 + TypeScript SPA with Vite, Tailwind CSS, Okta integration

## Tech Stack

### Backend
- **.NET 8** - ASP.NET Core Web API
- **Entity Framework Core 8** - ORM with Azure SQL Database
- **MediatR** - CQRS pattern implementation
- **FluentValidation** - Input validation
- **AutoMapper** - Object mapping
- **Serilog** - Structured logging with Application Insights
- **Swashbuckle** - OpenAPI/Swagger documentation
- **Okta OIDC** - JWT Bearer authentication

### Frontend
- **React 18** with TypeScript
- **Vite** - Build tool and dev server
- **React Router v6** - Client-side routing
- **TanStack Query** (React Query) - Server state management
- **Zustand** - Client state management
- **React Hook Form + Zod** - Form validation
- **@okta/okta-react** - Okta authentication with PKCE
- **Axios** - HTTP client with interceptors
- **Tailwind CSS** - Utility-first styling
- **Radix UI** / **shadcn/ui** - Accessible UI components

### Azure Hosting & Cloud
- **Azure App Service** (Linux) - Separate apps for SPA and API
- **Azure Front Door + WAF** - Custom domains, TLS, caching
- **Azure SQL Database** - Production database
- **Azure Key Vault** - Secrets management via Managed Identity
- **Azure Application Insights + Log Analytics** - Monitoring and observability
- **Azure App Configuration** - Feature flags

## Features

### Core Functionality
- **Ticket Management**: Full CRUD for tickets with custom fields per tenant
- **AI Ticket Assistant**: Natural-language input to generate title, category, urgency, and description
- **AI Classification & Routing**: Auto-categorizes and assigns tickets based on past patterns
- **AI Summarization**: Summarizes threads and activity into brief updates
- **AI Resolution Suggestions**: Recommends solutions based on knowledge base and past tickets (RAG)
- **AI Insights Dashboard**: Identifies recurring issues and automation opportunities

### Workflow & Automation
- **Ticket Lifecycle**: New → In Progress → Waiting → Resolved → Closed
- **SLA Management**: Rules and escalation triggers
- **Auto-close**: Inactive tickets after set duration
- **Macros**: Bulk updates
- **Notifications**: Email, Teams, Slack

### Multi-Tenant Structure
- Logical tenant isolation in Azure SQL schemas
- Configurable branding (theme, logo, email templates)
- Role-based access per tenant
- Shared codebase with isolated data domains

### Security
- **Okta SSO** with OIDC Authorization Code + PKCE flow
- **JWT Bearer Authentication** on API
- **Role-based Authorization** (System Admin, Tenant Admin, Agent/Technician, End User, Vendor)
- **CORS** protection
- **HTTPS** enforcement
- **Input validation** on all endpoints
- **PII encryption** at rest and in transit
- **GDPR compliance** (data export/deletion)

## Getting Started

### Prerequisites
- .NET 8 SDK
- Node.js 18+ and npm
- SQL Server (LocalDB for development, Azure SQL for production)
- Okta Developer Account

### Backend Setup

1. **Restore dependencies**:
   ```bash
   dotnet restore
   ```

2. **Update connection string** in `src/HD.API/appsettings.json`:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=HumbleDeskDB;Trusted_Connection=True;"
   }
   ```

3. **Configure Okta** in `appsettings.json`:
   ```json
   "Okta": {
     "Authority": "https://your-domain.okta.com/oauth2/default",
     "Audience": "api://default"
   }
   ```

4. **Create database migration** (first time only):
   ```bash
   cd src/HD.Infrastructure
   dotnet ef migrations add InitialCreate --startup-project ../HD.API/HD.API.csproj
   dotnet ef database update --startup-project ../HD.API/HD.API.csproj
   ```

5. **Run the API**:
   ```bash
   cd src/HD.API
   dotnet run
   ```

   API will be available at `https://localhost:7001` (or configured port)

### Frontend Setup

1. **Navigate to client directory**:
   ```bash
   cd client
   ```

2. **Install dependencies**:
   ```bash
   npm install
   ```

3. **Create `.env.local`** file (copy from `.env.example`):
   ```env
   VITE_API_BASE_URL=https://localhost:7001/api
   VITE_OKTA_ISSUER=https://your-domain.okta.com/oauth2/default
   VITE_OKTA_CLIENT_ID=your-spa-client-id
   ```

4. **Run development server**:
   ```bash
   npm run dev
   ```

   Client will be available at `http://localhost:5173`

### Okta Configuration

1. Create an **API Services** app integration for the backend:
   - Note the Issuer and Audience values
   - Configure trusted origins

2. Create a **Single-Page Application** for the frontend:
   - Grant type: Authorization Code with PKCE
   - Sign-in redirect: `http://localhost:5173/login/callback`
   - Sign-out redirect: `http://localhost:5173`
   - Trusted Origins: `http://localhost:5173`

3. Assign users to the application and configure groups/roles

## Project Structure

```
humbledesk/
├── src/
│   ├── HD.Domain/              # Core domain entities
│   │   ├── Common/             # Base classes, interfaces
│   │   ├── Entities/           # Domain entities
│   │   └── Enums/              # Enumerations
│   ├── HD.Application/         # Application layer
│   │   ├── Common/Interfaces/  # Abstraction interfaces
│   │   ├── QuestionSets/       # Feature folders (to be updated to Tickets)
│   │   │   ├── Commands/       # Write operations
│   │   │   ├── Queries/        # Read operations
│   │   │   └── DTOs/           # Data transfer objects
│   │   └── DependencyInjection.cs
│   ├── HD.Infrastructure/      # Infrastructure layer
│   │   ├── Data/               # EF Core DbContext
│   │   │   └── Configurations/ # Entity configurations
│   │   └── DependencyInjection.cs
│   └── HD.API/                 # Web API
│       ├── Controllers/        # API controllers
│       ├── Services/           # API services
│       └── Program.cs          # Entry point
├── client/                     # React frontend
│   ├── src/
│   │   ├── config/            # Configuration
│   │   ├── lib/               # Utilities, API client
│   │   ├── pages/             # Page components
│   │   ├── types/             # TypeScript types
│   │   ├── App.tsx            # Root component
│   │   └── main.tsx           # Entry point
│   └── package.json
├── reqs/                      # Requirements and PRD
├── HumbleDesk.sln             # Solution file
└── README.md
```

## Database Schema

Key entities (to be implemented):
- **Tenant**: Multi-tenant organization data
- **User**: Users within tenants with roles
- **Ticket**: Support tickets with AI-enhanced fields
- **Comment**: Threaded discussions on tickets (public/private)
- **Attachment**: File uploads linked to tickets
- **SLA**: Service level agreement rules per tenant
- **AIInsight**: AI-generated suggestions and insights
- **AuditEvent**: Complete audit trail

## API Endpoints

### Tickets (to be implemented)
- `GET /api/tickets` - List tickets with filtering
- `GET /api/tickets/{id}` - Get ticket details
- `POST /api/tickets` - Create new ticket with AI assistance
- `PUT /api/tickets/{id}` - Update ticket
- `POST /api/tickets/{id}/ai-suggest` - Get AI resolution suggestions

## Development

### Building the Backend
```bash
dotnet build
```

### Running Tests
```bash
dotnet test
```

### Building for Production
```bash
# Backend
dotnet publish -c Release -o ./publish

# Frontend
cd client
npm run build
```

## Deployment

### Azure App Services (Recommended)
1. Backend: Deploy as Azure App Service (Linux)
2. Frontend: Deploy SPA to separate App Service or Azure Static Web Apps
3. Database: Azure SQL Database
4. Configure Azure Key Vault for secrets
5. Set up Application Insights for monitoring
6. Optional: Azure Front Door + WAF for global routing and security

### Environment Variables
- Backend: Configure in Azure App Service Configuration
- Frontend: Set build-time variables in deployment pipeline

## Contributing

This project follows Clean Architecture and SOLID principles. When contributing:
- Keep domain layer pure (no external dependencies)
- Use MediatR for all use cases
- Follow existing patterns for commands/queries
- Add FluentValidation validators for DTOs
- Write unit tests for business logic

## License

[Your License Here]

## Support

For issues and questions, please open an issue in the repository.
