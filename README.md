# Conflict of Interest (COI) Management System

A comprehensive web application for managing Conflict of Interest disclosures with Clean Architecture, ASP.NET Core 8, React, and Okta authentication.

## Architecture

This project follows **Clean Architecture** principles with clear separation of concerns:

- **Domain Layer** (`COI.Domain`): Core business entities, enums, and domain logic - no external dependencies
- **Application Layer** (`COI.Application`): Use cases, DTOs, interfaces, MediatR commands/queries, FluentValidation
- **Infrastructure Layer** (`COI.Infrastructure`): EF Core, database configurations, external service implementations
- **API Layer** (`COI.API`): ASP.NET Core Web API, controllers, authentication, middleware
- **Client** (`client/`): React 18 + TypeScript SPA with Vite, Tailwind CSS, Okta integration

## Tech Stack

### Backend
- **.NET 8** - ASP.NET Core Web API
- **Entity Framework Core 8** - ORM with SQL Server
- **MediatR** - CQRS pattern implementation
- **FluentValidation** - Input validation
- **AutoMapper** - Object mapping
- **Serilog** - Structured logging
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
- **Radix UI** - Accessible UI components

## Features

### Core Functionality
- **Question Builder**: WYSIWYG editor for creating configurable questionnaires
- **Multiple Answer Types**: Yes/No, Text, Conditional, Select, Date, Number, File Upload, Attestation
- **Branching Logic**: Conditional question display based on answers
- **Assignment Management**: Assign questionnaires to employees/board members
- **Submission Workflow**: Draft, Submit, Review, Approve process
- **Review Queue**: Dedicated interface for reviewers
- **Audit Trail**: Complete history of all actions
- **Reporting**: Dashboards, exports (CSV/PDF)

### Security
- **Okta SSO** with OIDC Authorization Code + PKCE flow
- **JWT Bearer Authentication** on API
- **Role-based Authorization** (Employee, Director, LegalAdmin, Reviewer, SystemAdmin)
- **CORS** protection
- **HTTPS** enforcement
- **Input validation** on all endpoints

## Getting Started

### Prerequisites
- .NET 8 SDK
- Node.js 18+ and npm
- SQL Server (LocalDB for development)
- Okta Developer Account

### Backend Setup

1. **Restore dependencies**:
   ```bash
   dotnet restore
   ```

2. **Update connection string** in `src/COI.API/appsettings.json`:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=COIDB;Trusted_Connection=True;"
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
   cd src/COI.Infrastructure
   dotnet ef migrations add InitialCreate --startup-project ../COI.API/COI.API.csproj
   dotnet ef database update --startup-project ../COI.API/COI.API.csproj
   ```

5. **Run the API**:
   ```bash
   cd src/COI.API
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
conflict/
├── src/
│   ├── COI.Domain/              # Core domain entities
│   │   ├── Common/              # Base classes, interfaces
│   │   ├── Entities/            # Domain entities
│   │   └── Enums/               # Enumerations
│   ├── COI.Application/         # Application layer
│   │   ├── Common/Interfaces/   # Abstraction interfaces
│   │   ├── QuestionSets/        # Feature folders
│   │   │   ├── Commands/        # Write operations
│   │   │   ├── Queries/         # Read operations
│   │   │   └── DTOs/            # Data transfer objects
│   │   └── DependencyInjection.cs
│   ├── COI.Infrastructure/      # Infrastructure layer
│   │   ├── Data/                # EF Core DbContext
│   │   │   └── Configurations/ # Entity configurations
│   │   └── DependencyInjection.cs
│   └── COI.API/                 # Web API
│       ├── Controllers/         # API controllers
│       ├── Services/            # API services
│       └── Program.cs           # Entry point
├── client/                      # React frontend
│   ├── src/
│   │   ├── config/             # Configuration
│   │   ├── lib/                # Utilities, API client
│   │   ├── pages/              # Page components
│   │   ├── types/              # TypeScript types
│   │   ├── App.tsx             # Root component
│   │   └── main.tsx            # Entry point
│   └── package.json
├── COI.sln                      # Solution file
└── README.md
```

## Database Schema

Key entities:
- **QuestionSet**: Questionnaire templates with versioning
- **Section**: Logical groupings of questions
- **Question**: Individual questions with types and constraints
- **Option**: Answer choices for select-type questions
- **BranchRule**: Conditional logic for question visibility
- **Assignment**: Questionnaires assigned to users
- **Submission**: User responses with attestation
- **Answer**: Individual question responses
- **Review**: Review workflow and decisions
- **Comment**: Threaded discussions on submissions
- **Attachment**: File uploads
- **AuditEvent**: Complete audit trail

## API Endpoints

### Question Sets
- `GET /api/questionsets/{id}` - Get question set by ID
- `POST /api/questionsets` - Create new question set (LegalAdmin only)

More endpoints to be implemented for full CRUD operations.

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
