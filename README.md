# Staff Pro — Restaurant Staff Management & Scheduling

A standalone, full-featured staff management and scheduling system built for restaurants and hospitality businesses.

## Architecture

```
Staff Management & Scheduling/
├── staff-backend/                    # .NET 8 Clean Architecture API
│   ├── src/
│   │   ├── StaffPro.Domain/         # Entities, Enums, Events, Interfaces
│   │   ├── StaffPro.Application/    # DTOs, Commands, Queries, Validators, Mappings
│   │   ├── StaffPro.Infrastructure/ # EF Core DbContext, Repositories, Services
│   │   └── StaffPro.Api/            # Controllers, Middleware, SignalR Hubs
│   ├── Dockerfile
│   └── StaffPro.sln
├── staff-frontend/                   # React 18 + Vite + TypeScript + Ant Design 5
│   ├── src/
│   │   ├── api/                     # RTK Query API slices
│   │   ├── store/                   # Redux Toolkit store + auth slice
│   │   ├── pages/                   # Page components (12 pages)
│   │   ├── components/              # Reusable UI components
│   │   ├── types/                   # TypeScript type definitions
│   │   └── hooks/                   # Custom React hooks
│   ├── Dockerfile
│   └── package.json
├── docker-compose.yml                # Full stack orchestration
├── STAFF_BACKEND_REPORT.md          # Complete backend specification
└── STAFF_FRONTEND_REPORT.md         # Complete frontend specification
```

## Tech Stack

### Backend
- **.NET 8.0** with ASP.NET Core
- **Clean Architecture** with CQRS pattern (MediatR)
- **Entity Framework Core 8** with SQL Server 2022
- **ASP.NET Identity** with JWT Bearer authentication
- **SignalR** for real-time notifications
- **AutoMapper** for object mapping
- **FluentValidation** for request validation
- **Serilog** for structured logging
- **Swagger/OpenAPI** for API documentation

### Frontend
- **React 18** with TypeScript
- **Vite 5** for development/build
- **Ant Design 5** UI component library
- **Redux Toolkit** with RTK Query for state/API management
- **React Router 6** for navigation
- **dayjs** for date handling
- **SignalR client** for real-time notifications

### Infrastructure
- **Docker Compose** for local development
- **SQL Server 2022** database
- **Redis** for caching and SignalR backplane
- **Nginx** for frontend serving and API proxy

## Quick Start

### Option 1: Docker (Recommended)

```bash
cd "Staff Management & Scheduling"
docker-compose up -d
```

- Frontend: http://localhost:5173
- Backend API: http://localhost:5000
- Swagger: http://localhost:5000/swagger

### Option 2: Local Development

**Backend:**
```bash
cd staff-backend
dotnet restore
dotnet run --project src/StaffPro.Api
```

**Frontend:**
```bash
cd staff-frontend
npm install
npm run dev
```

## MVP Features (Phase 1)

| Feature | Status |
|---------|--------|
| Employee Directory (CRUD) | Built |
| Contracts & Multi-Role | Built |
| Role & Station Management | Built |
| Recurring Availability | Built |
| Shift Templates | Built |
| Schedule Builder (Create/Assign/Publish/Lock) | Built |
| Conflict Detection Engine (8 checks) | Built |
| Copy Previous Schedule | Built |
| Staffing Requirements | Built |
| Time-Off Requests & Approvals | Built |
| Leave Balance Tracking | Built |
| Clock In/Out (with GPS) | Built |
| Break Tracking | Built |
| Timesheet Summary & Approval | Built |
| Overtime Calculation | Built |
| Payroll CSV Export | Built |
| In-App Notifications (SignalR) | Built |
| Announcements (broadcast + read receipts) | Built |
| Audit Trail | Built |
| JWT Authentication + RBAC | Built |
| Manager & Employee Dashboards | Built |
| Settings (Roles, Stations, Templates, Leave Types, Locations) | Built |
| Responsive Layout (Desktop + Mobile) | Built |

## API Endpoints

| Controller | Endpoints |
|-----------|-----------|
| Auth | POST login, register, refresh, invite, change-password; GET me |
| Employees | GET list/detail; POST create; PUT update; DELETE; roles, availability, leave-balances |
| Schedules | GET list/detail; POST create/publish/lock/copy; assignments CRUD; conflict check |
| Time Off | GET list; POST create/review/cancel; staffing impact |
| Clock | POST action/override; GET status/entries |
| Timesheets | GET list/detail; POST submit/approve; payroll summary/export |
| Notifications | GET list; POST mark-read/mark-all-read |
| Announcements | GET list; POST create/mark-read |
| Dashboard | GET manager/employee |
| Settings | CRUD for roles, stations, departments, locations, shift-templates, leave-types, scheduling-rules |

## Project Reports

- **[Backend Report](STAFF_BACKEND_REPORT.md)** — Complete backend specification with data models, API endpoints, and architecture
- **[Frontend Report](STAFF_FRONTEND_REPORT.md)** — Complete frontend specification with page designs, components, and UX patterns
