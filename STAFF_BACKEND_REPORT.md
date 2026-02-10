# Staff Pro - Backend Development Report

**Generated:** February 9, 2026 (Updated with Industry Research)  
**Project:** Staff Pro — Standalone Restaurant Staff Management & Scheduling System  
**Framework:** .NET 8.0 | ASP.NET Core  
**Architecture:** Clean Architecture + CQRS/MediatR (standalone project — NOT part of BonApp)  
**Repository:** `staff-backend` (separate Git repository)  
**Database:** Own SQL Server database (`StaffProDb`)  
**Cache:** Redis (distributed cache + SignalR backplane)  
**Status:** Planning Phase

---

## 0. Market Context & Competitive Analysis

> This section is based on research of the leading restaurant workforce management platforms as of early 2026: **7shifts** (market leader, 4.7/5 rating, 35+ POS integrations), **HotSchedules/Fourth** (enterprise-focused, compliance-heavy), **Deputy** (global, geofencing pioneer), **CrunchTime** (AI forecasting leader used by Chipotle, Five Guys, Dunkin'), **Homebase** (SMB-focused, free tier), **Harri** (engagement + turnover prediction), **Mapal** (EU compliance), **Push Operations** (all-in-one HCM), and **When I Work** (simplicity-focused).

### Industry Landscape (2026)

| Metric | Value | Source |
|--------|-------|--------|
| Restaurant labor cost as % of revenue | 30–35% (full-service), 25–30% (QSR) | Industry benchmark |
| AI labor forecasting market size | $1.37B → $10.7B by 2033 (24.8% CAGR) | Market research |
| AI forecast accuracy (best-in-class) | Up to 95% accuracy, within 13 cents of actual sales | CrunchTime |
| Gen Z workforce share | 27% of restaurant workforce | Industry data |
| #1 operator concern | Recruiting & retention (65% say market is "Tight" or "Very Tight") | 7shifts 2025 Report |
| Sub-90-day turnover | #1 cause of failed restaurant growth | Harri |

### Feature Comparison: Staff Pro vs. Market Leaders

| Feature | 7shifts | HotSchedules | Deputy | Homebase | Connecteam | Staff Pro (Planned) |
|---------|---------|-------------|--------|----------|------------|-------------------|
| Drag-drop scheduling | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ |
| Conflict detection | ✅ | ✅ | ✅ | ❌ | ❌ | ✅ (14 checks) |
| **Auto-scheduling (AI)** | ✅ | ✅ (Fourth) | ✅ | ✅ | ✅ | ✅ (constraint solver) |
| Shift swaps/marketplace | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ |
| **Shift bidding / preference** | ✅ | ❌ | ❌ | ❌ | ❌ | ✅ |
| **Schedule templates (full week)** | ✅ | ✅ | ✅ | ✅ | ❌ | ✅ |
| Clock-in/out | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ |
| Geofencing clock | ❌ | ❌ | ✅ | ✅ | ✅ | ✅ |
| Photo verification clock | ❌ | ❌ | ✅ | ✅ | ❌ | ✅ |
| **Facial recognition clock** | ❌ | ❌ | ✅ (biometric) | ❌ | ❌ | ✅ (Phase 3) |
| **Multiple clock methods** | App only | App + kiosk | App + kiosk + bio | App + kiosk | App + kiosk | App + kiosk + PIN + QR + photo + facial |
| **IP address lock** | ❌ | ❌ | ❌ | ❌ | ❌ | ✅ |
| Team messaging | ✅ | ❌ | ❌ | ✅ | ✅ | ✅ |
| **Company newsfeed** | ❌ | ❌ | ❌ | ❌ | ✅ | ✅ |
| Manager logbook | ✅ | ❌ | ❌ | ❌ | ❌ | ✅ |
| Task management | ✅ | ❌ | ✅ | ❌ | ✅ | ✅ |
| **Digital forms builder** | ❌ | ❌ | ❌ | ❌ | ✅ | ✅ |
| Post-shift engagement surveys | ✅ | ❌ | ❌ | ❌ | ❌ | ✅ |
| **Employee recognition feed** | ✅ | ❌ | ❌ | ❌ | ❌ | ✅ |
| Tip management | ✅ | ❌ | ❌ | ❌ | ❌ | ✅ |
| AI demand forecasting | ❌ | ✅ (Fourth) | ✅ | ✅ (basic) | ❌ | ✅ |
| **Earned vs. actual hours** | ✅ | ✅ | ❌ | ❌ | ❌ | ✅ |
| Predictive scheduling compliance | ❌ | ✅ | ❌ | ❌ | ❌ | ✅ |
| **Minor work rules** | ✅ | ✅ | ❌ | ❌ | ❌ | ✅ |
| SPLH / labor % tracking | ✅ | ✅ | ✅ | ❌ | ❌ | ✅ |
| **Real-time intraday labor dashboard** | ❌ | ✅ | ❌ | ❌ | ❌ | ✅ |
| **Hiring & applicant tracking (ATS)** | ✅ | ❌ | ❌ | ✅ (AI) | ❌ | ✅ |
| **Employee onboarding portal** | ✅ | ❌ | ❌ | ✅ | ✅ | ✅ |
| **Training / LMS / courses** | ❌ | ❌ | ❌ | ❌ | ✅ (AI) | ✅ |
| **Knowledge base / handbook** | ❌ | ❌ | ❌ | ❌ | ✅ | ✅ |
| **Performance reviews** | ❌ | ❌ | ❌ | ❌ | ❌ | ✅ |
| **Employee earnings estimate** | ✅ | ❌ | ❌ | ❌ | ❌ | ✅ |
| Payroll integration API | ✅ (16 providers) | ✅ | ✅ (10 providers) | ✅ | ✅ | ✅ |
| POS integrations | 35+ | 10 | 6 | 10+ | ❌ | API-first (any POS) |
| Built-in payroll | ✅ (US only) | ❌ | ❌ | ✅ | ✅ | ❌ (export-focused) |
| Multi-location | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ |
| Offline clock-in | ❌ | ❌ | ✅ | ❌ | ✅ | ✅ (PWA) |
| GDPR compliance | ❌ | ✅ | ✅ | ❌ | ✅ | ✅ (Swiss + EU) |
| **GPS live employee map** | ❌ | ❌ | ❌ | ❌ | ✅ | ✅ |

### Staff Pro Differentiators

1. **API-first POS integration** — Works with ANY POS system via documented REST API, not limited to pre-built integrations
2. **Swiss + EU labor law compliance** — Built-in support for Swiss working time regulations, EU Working Time Directive, Fair Workweek (US), configurable per jurisdiction
3. **Complete data sovereignty** — Self-hostable, own database, no vendor lock-in (unlike SaaS-only competitors)
4. **BonApp ecosystem** — Future deep integration with BonApp POS for unified restaurant operations
5. **Geofencing + photo verification** — Combined anti-fraud clock-in that neither 7shifts nor HotSchedules offer
6. **AI forecasting without vendor dependency** — Own forecasting engine that improves with each POS data sync
7. **Full employee lifecycle** — Only platform combining hiring (ATS), onboarding, training (LMS), scheduling, time tracking, engagement, performance reviews, and offboarding in one system. Connecteam has training but no scheduling AI; 7shifts has scheduling AI but no training; Staff Pro has both.
8. **6 clock-in methods** — App, kiosk PIN, QR code, photo selfie, facial recognition (Phase 3), and IP lock. No competitor offers all six.
9. **Constraint-based auto-scheduling** — True optimization algorithm (not just "auto-fill") that considers availability, skills, labor cost, fairness, preferences, compliance, and demand forecasts simultaneously
10. **Digital forms engine** — Custom form builder for operational checklists, inspections, and compliance documents — a feature only Connecteam offers among scheduling apps

---

## 1. Executive Summary

This report defines the complete backend development plan for **Staff Pro**, a fully **autonomous, standalone** restaurant staff management and scheduling system. This is an **independent application** — it has its own codebase, database, authentication, and deployment. It does NOT live inside the BonApp backend.

The application is designed for any restaurant or hospitality business, regardless of which POS system they use. It works with:

- **BonApp POS** — Connected via REST API / webhooks (future integration)
- **Lightspeed, Toast, Square, Orderbird** — Connected via REST API / webhooks
- **Any POS system** — Connected via the documented External POS API (`/api/pos/v1/`)
- **No POS at all** — Standalone staff management with manual data entry, CSV imports/exports

### Architectural Independence

This system is **completely decoupled** from BonApp:
- **Own database** — `StaffProDb` with its own schema (no shared tables)
- **Own authentication** — ASP.NET Identity with JWT Bearer tokens (independent user accounts)
- **Own entities** — `Organization` (restaurant), `Employee`, `Shift`, `Schedule`, `AppUser` — NOT BonApp's `YumYumYard`, `Staff`, `ApplicationUser`
- **Own deployment** — Separate Docker container, separate Azure App Service, separate CI/CD pipeline
- **API-first integration** — Any future connection to BonApp or other POS systems happens exclusively through the External POS API endpoints (`/api/pos/v1/`) using API keys

### Future BonApp Integration (Phase 2)

When the time comes to connect Staff Pro to BonApp, it will happen through:
1. BonApp registers as an External POS Connection in Staff Pro (gets an API key)
2. BonApp calls `POST /api/pos/v1/clock-in` when a staff member logs into the POS terminal (attendance sync)
3. BonApp calls `POST /api/pos/v1/sales-summary` to provide hourly sales data (staffing optimization)
4. Staff Pro calls BonApp's webhook URL when schedule changes affect assigned POS roles (outbound webhook)
5. Staff Pro can read BonApp's role/permission data to sync staff permissions across both systems
6. **No shared database. No shared code. No shared authentication. API calls only.**

---

## 2. Core Features to Build

> **BUILD PRIORITY:** Sections 2.1–2.12 are the absolute core (Tier 1 + Tier 2 features). Build these first. Sections 2.13–2.19 are standard competitive features (Tier 2/3). Sections 2.20–2.29 are advanced differentiators or platform extensions (Tier 3/4) — build only after the core is rock-solid and customers are requesting them. See Section 11 for the full Priority Matrix.

### 2.1 Employee Management (HR Basics)

| Feature | Description | Priority |
|---------|-------------|----------|
| **Employee Directory** | Full CRUD for all staff members. Each employee has: first name, last name, email, phone, profile photo, emergency contact (name + phone + relationship), address, date of birth, national ID/work permit number (encrypted), hire date, termination date (nullable), and status (Active, On Leave, Suspended, Terminated). Soft-delete only — never hard-delete employee records because they link to historical schedules and timesheets. | P0 |
| **Contract Management** | Each employee has one active contract at a time with: contract type (Full-Time, Part-Time, Hourly/Casual, Internship, Seasonal), contracted hours per week, hourly rate (decimal, stored in cents to avoid floating-point errors), salary (for salaried staff), start date, end date (nullable for permanent contracts), probation end date, and notice period in days. Contract history is preserved — when terms change, the old contract ends and a new one begins. | P0 |
| **Multi-Role Assignment** | Employees can hold multiple roles simultaneously. E.g., "Marie is a Waiter AND a Bartender AND can work Kitchen Prep." Each EmployeeRole has a proficiency level (Trainee, Junior, Senior, Lead) and a flag for primary role. This enables flexible scheduling — the system knows which stations each person can work. | P0 |
| **Skill & Certification Tracking** | Track specific skills and certifications per employee: food safety certificate (with expiry date), alcohol serving license, allergy training, first aid, barista training, sommelier certification, fire safety, etc. The system alerts when certifications are about to expire (30-day, 7-day warnings). Mandatory certifications can be enforced per role — e.g., "all Kitchen staff MUST have valid food safety certificate." | P1 |
| **Document Storage** | Store employee-related documents: signed contracts, ID copies, work permits, certificates, disciplinary records. Files stored in Azure Blob Storage with metadata (document type, upload date, expiry date, uploaded by). Access restricted by permission — only Admin/HR can see sensitive documents. | P1 |
| **Employee Notes & History** | Internal notes on employees (performance notes, verbal warnings, commendations). Full audit trail: who added the note, when, and what changed. Visible only to managers and above. | P2 |
| **Onboarding Checklist** | Configurable onboarding checklist per role. When a new employee is created, the system generates their checklist: uniform issued, locker assigned, training completed, bank details submitted, emergency contact provided, etc. Track completion percentage. | P2 |

### 2.2 Roles, Stations & Organizational Structure

| Feature | Description | Priority |
|---------|-------------|----------|
| **Role Management** | Define all roles in the restaurant: Waiter, Bartender, Host, Chef, Sous Chef, Line Cook, Prep Cook, Dishwasher, Manager, Assistant Manager, Delivery Driver, Cashier, etc. Each role has: name, description, default hourly rate, required certifications, color (for schedule UI). Roles are per-organization (each restaurant defines its own). | P0 |
| **Station/Area Management** | Define physical work stations or areas: Main Dining Room, Terrace, Bar, Kitchen Station A (Hot), Kitchen Station B (Cold), Kitchen Station C (Pastry), Delivery Zone, Reception, etc. Each station has: name, description, max capacity (max people), and the roles that can work there. | P0 |
| **Department Grouping** | Group roles and stations into departments: Front of House (FOH), Back of House (BOH), Management, Delivery. This enables department-level reporting (labor cost per department) and department-based permissions (FOH manager can only schedule FOH staff). | P1 |
| **Permission System** | Fine-grained permissions beyond simple Admin/Manager/Employee: Can Create Schedule, Can Approve Time-Off, Can View Payroll Data, Can Edit Employee Records, Can Export Reports, Can Manage Roles, Can View All Departments, Can Approve Timesheets, Can Override Conflicts. Permissions are assigned to roles, not individual users (role-based access control). | P0 |
| **Multi-Location Support** | An Organization can have multiple Locations (restaurants/branches). Each location has its own staff roster, schedules, and settings, but the organization-level admin can see everything. Employees can be assigned to one or more locations. Cross-location transfer/scheduling supported. | P1 |

### 2.3 Availability & Constraints

| Feature | Description | Priority |
|---------|-------------|----------|
| **Recurring Availability** | Employees define their weekly availability pattern: day of week + start time + end time + type (Available, Preferred, Unavailable). E.g., "Available Mon–Fri 09:00–23:00, Unavailable weekends." These repeat every week until changed. The system uses this as the baseline for scheduling suggestions. | P0 |
| **One-Off Availability Overrides** | Employees can override their recurring availability for specific dates. E.g., "I'm usually available Tuesday, but not next Tuesday." Each override has: date, start time, end time, type, and optional note. Overrides take priority over recurring patterns. | P0 |
| **Availability Submission Window** | Configurable rule: "Employees must submit availability changes at least X days before the schedule is published." Late changes require manager approval. This prevents last-minute chaos. | P1 |
| **Scheduling Constraints (Business Rules)** | Configurable rules enforced during schedule creation: minimum rest between shifts (e.g., 11 hours — EU Working Time Directive), maximum consecutive working days (e.g., 6 days), maximum hours per week (e.g., 48 hours per contract), maximum hours per day (e.g., 10 hours), mandatory break rules (e.g., 30 min break after 6 hours). Rules can be set at organization level and overridden per contract type. | P0 |
| **Staffing Requirements Templates** | Define minimum and maximum staffing per shift per station. E.g., "Friday Evening shift needs: 3-4 Waiters, 1-2 Bartenders, 2-3 Kitchen, 1 Host." These become targets when building the schedule. The system highlights when a shift is understaffed or overstaffed. | P0 |
| **Cost Budgets per Shift** | Optionally set a labor cost budget per shift or per day. The system calculates projected labor cost in real-time as the manager assigns staff. Warns when approaching or exceeding budget. | P2 |

### 2.4 Shift Scheduling (Core Feature)

| Feature | Description | Priority |
|---------|-------------|----------|
| **Shift Templates** | Define reusable shift patterns: Morning (07:00–15:00), Afternoon (11:00–19:00), Evening (17:00–01:00), Split (11:00–14:00 + 18:00–23:00), etc. Each template has: name, start time, end time, break duration, break is paid (bool), applicable stations, color code. Templates speed up schedule creation — drag a template onto a day instead of typing times. | P0 |
| **Schedule Periods** | Schedules are created for a defined period: weekly (most common), bi-weekly, or monthly. Each period has a status lifecycle: Draft → Published → Locked. Draft: only managers see it and can edit freely. Published: visible to all assigned employees, edits trigger notifications. Locked: no further changes (for payroll accuracy). | P0 |
| **Shift Assignment** | Assign an employee to a shift on a specific date, at a specific station, in a specific role. Each ShiftAssignment record has: employee, shift template (or custom times), date, station, role, status (Scheduled, Confirmed, No-Show, Cancelled), and notes. Employees can be assigned to multiple shifts on the same day (split shifts). | P0 |
| **Conflict Detection Engine** | When assigning a shift, the system checks ALL constraints in real-time and returns a list of conflicts — each classified as Error (hard block — cannot save) or Warning (soft block — can override with reason). Conflict checks include: employee not available on this date/time, overlapping with another shift, insufficient rest since last shift, exceeds weekly/daily hour limit, exceeds consecutive days limit, employee lacks required skill/certification for station, employee on approved leave, shift exceeds staffing maximum, employee not assigned to this location. The conflict engine is a dedicated service (`IConflictDetectionService`) so it can be unit-tested independently. | P0 |
| **Copy Previous Schedule** | One-click copy of last week's schedule to the current draft. The system copies all assignments, then re-runs conflict detection against current availability and time-off. Flagged conflicts are highlighted for manual resolution. | P0 |
| **Auto-Fill Suggestions** | Optional AI-lite feature: given staffing requirements and employee availability, suggest an optimal assignment. Algorithm considers: availability, role match, hours fairness (distribute hours evenly among similar employees), cost optimization (prefer lower-rate employees when skill is equal), consecutive days, and employee preferences. This is NOT mandatory for MVP — manual scheduling is the primary mode. | P2 |
| **Open Shifts (Shift Marketplace)** | Managers can publish unfilled shifts as "Open Shifts." Employees who are available and qualified receive a notification and can claim the shift. First-come-first-served or manager-approval mode. This reduces the manager's phone calls for last-minute coverage. | P1 |
| **Schedule Publishing & Notifications** | When a schedule moves from Draft to Published, all assigned employees receive a notification (email + push). The notification includes their shifts for the period with dates, times, stations, and roles. Any subsequent changes to a published schedule also trigger change notifications with diff (what changed). | P0 |

### 2.5 Time-Off & Leave Management

| Feature | Description | Priority |
|---------|-------------|----------|
| **Leave Types** | Configurable leave categories: Vacation/Holiday, Sick Leave, Personal Day, Unpaid Leave, Maternity/Paternity, Bereavement, Training/Conference, Compensatory Leave (time off in lieu of overtime). Each type has: name, is paid (bool), requires document (bool), max days per year (nullable), accrual rate (nullable), color for calendar. | P0 |
| **Time-Off Requests** | Employees submit requests: leave type, start date, end date, start time (optional for partial days), end time, note/reason, attachment (optional — e.g., doctor's note). Request status lifecycle: Pending → Approved / Denied. Denied requests require a reason from the manager. | P0 |
| **Leave Balance Tracking** | Track remaining leave balance per employee per leave type per year. Accrual-based: e.g., full-time employees earn 2.08 vacation days per month (25 days/year). Deduction happens when leave is approved. Balance can carry over to next year (configurable: max carry-over days). Balance displayed on employee profile and in the request form. | P1 |
| **Staffing Impact Preview** | Before approving a time-off request, the manager sees: how many staff are already scheduled for those dates, how many are already on leave, whether minimum staffing requirements will be met, and who else is available to cover. This prevents approving leave that creates an understaffed shift. | P0 |
| **Team Calendar View** | A shared calendar showing all approved time-off for the team. Managers see the full picture when planning schedules. Employees see who else is off (without seeing leave type details for privacy). | P1 |
| **Blackout Dates** | Managers can define dates where no leave is allowed: Christmas week, Valentine's Day, major local events. Requests for blackout dates are auto-denied or flagged for special approval. | P1 |

### 2.6 Shift Swap & Trade System

| Feature | Description | Priority |
|---------|-------------|----------|
| **Swap Requests** | Employee A can request to swap a shift with Employee B. Both must confirm. The system verifies both employees are qualified (role, station, certification) and no conflicts arise. Manager approval is configurable (always required, or auto-approve if no conflicts). | P1 |
| **Drop Requests** | Employee requests to drop a shift. This creates an Open Shift that others can claim, or the manager must find a replacement. Drop requests require a reason and manager approval. | P1 |
| **Cover Requests** | Employee requests someone to cover their shift (one-way — not a swap). The system suggests eligible employees who are available and qualified. The covering employee accepts, manager approves, and the original employee is released. | P1 |

### 2.7 Timesheets & Worked Hours Tracking

| Feature | Description | Priority |
|---------|-------------|----------|
| **Clock-In / Clock-Out** | Employees clock in and out via the web app (mobile-optimized). Records timestamp, GPS location (optional), and IP address. Grace period rules: e.g., clocking in up to 5 minutes early counts as shift start; clocking in more than 15 minutes late triggers a flag. Configurable rounding rules (round to nearest 5/10/15 minutes). | P0 |
| **Break Tracking** | Employees clock out for break and clock back in. Break duration is tracked separately. Rules: minimum break duration, paid vs. unpaid breaks (from shift template), auto-deduct break if not clocked (configurable). | P1 |
| **Manager Override** | Managers can manually edit clock times for corrections: employee forgot to clock out, clocked in at wrong time, system was down. Every manual edit is logged in the audit trail with reason and editor identity. | P0 |
| **Timesheet Summary** | For each employee per period (week/bi-weekly/month): total regular hours, total overtime hours (with overtime threshold from contract — e.g., overtime after 40h/week or 8h/day), total night hours (configurable: e.g., 23:00–06:00), total weekend/holiday hours, total break time, net payable hours. Each line item links back to the original clock entries for audit. | P0 |
| **Timesheet Approval Workflow** | At the end of each period: Employee reviews and submits their timesheet (optional — can be auto-submitted). Manager reviews, corrects if needed, and approves. Approved timesheets are locked — no further edits without HR/Admin override. Status lifecycle: Auto-Generated → Employee Submitted → Manager Approved → Locked. | P0 |
| **Overtime Rules Engine** | Configurable overtime calculation: daily overtime (after X hours/day), weekly overtime (after X hours/week), double-time rules (after X hours or on holidays), split between regular/overtime pay rates. Swiss labor law defaults available but customizable per organization. | P1 |
| **Absence Tracking** | Automatically detect when a scheduled employee does not clock in (No-Show). Flag for manager review. No-shows are tracked in the employee's history for pattern analysis. | P1 |

### 2.8 Payroll Export & Cost Reporting

| Feature | Description | Priority |
|---------|-------------|----------|
| **Payroll Summary Report** | Per employee per period: regular hours × regular rate, overtime hours × overtime rate, night premium hours × night rate, weekend/holiday premium, total gross pay, deductions (if any — e.g., meals, uniform). Exportable as CSV, Excel, or PDF. Designed to be imported into payroll software (Abacus, Sage, etc.) — NOT a payroll system itself. | P0 |
| **Labor Cost by Department** | Break down labor costs by department (FOH, BOH, Management) for the period. Shows: total hours, total cost, cost per hour, percentage of revenue (if POS data available). | P1 |
| **Labor Cost by Day/Shift** | Daily and per-shift labor cost breakdown. Helps managers identify expensive shifts and optimize staffing. | P1 |
| **Scheduled vs. Actual Hours Report** | Compare what was scheduled vs. what was actually worked. Highlights variance: early departures, late arrivals, overtime, no-shows. Helps identify scheduling accuracy and problem patterns. | P1 |
| **Custom CSV Export Mapping** | Allow admins to configure CSV export columns and format to match their payroll software's import requirements. Save export templates for reuse. | P2 |
| **Payroll Integration API** | REST API endpoints that payroll systems can call to pull approved timesheet data. Authenticated with API keys. Avoids manual CSV export for organizations that want automation. | P2 |

### 2.9 POS Integration Layer

All POS systems (including BonApp, in the future) connect through the same API. There is no "internal" vs "external" distinction — every POS is an external connection.

| Feature | Description | Priority |
|---------|-------------|----------|
| **POS Connection Management** | Any POS system (BonApp, Lightspeed, Toast, Square, Orderbird, or custom) registers as a connection and receives an API key. All data flows through the same API endpoints. No special treatment for any POS system. | P1 |
| **POS Clock-In/Out Sync** | POS systems can send clock-in/out events when staff logs into/out of the POS terminal. This supplements or replaces the built-in clock feature. Endpoint: `POST /api/pos/v1/clock`. | P1 |
| **POS Sales Data Receiver** | POS systems can send hourly/daily sales data. Staff Pro uses this for labor-cost-to-revenue ratio calculations and staffing optimization recommendations. Endpoint: `POST /api/pos/v1/sales-summary`. | P2 |
| **POS Role Sync** | Sync staff roles between POS and Staff Pro. When a role changes in Staff Pro (e.g., employee promoted from Waiter to Shift Leader), notify the POS via webhook so POS permissions update. | P2 |
| **Outbound Webhooks** | Staff Pro sends webhook notifications to registered POS systems when: schedule is published (so POS knows who's working), employee role changes, employee is terminated (POS should revoke access), shift swap is approved. HMAC-SHA256 signed payloads. | P2 |

### 2.10 Notifications

| Feature | Description | Priority |
|---------|-------------|----------|
| **In-App Notifications** | Real-time notifications stored in the database and delivered via SignalR (WebSocket). Bell icon with unread count. Types: new schedule published, shift assigned, time-off request status, shift swap request, open shift available, timesheet reminder, certification expiring. | P0 |
| **Email Notifications** | Configurable email notifications using SendGrid or SMTP. Each notification type can be individually enabled/disabled per user. Templates stored as Razor views. | P1 |
| **Push Notifications** | Web push notifications via Firebase Cloud Messaging (FCM) for mobile browsers. Employees get push alerts for new shifts, schedule changes, and urgent coverage requests even when the app is closed. | P2 |
| **Notification Preferences** | Per-user settings: which notifications to receive, via which channel (in-app, email, push), and quiet hours (no push between 23:00–07:00). | P1 |

### 2.11 Audit & Compliance

| Feature | Description | Priority |
|---------|-------------|----------|
| **Full Audit Trail** | Every data change is logged: who changed what, when, old value, new value. Covers: employee records, schedules, timesheets, approvals, role changes, settings changes. Audit logs are append-only (never deleted or modified). Queryable by entity, date range, and user. | P0 |
| **Data Retention Policy** | Configurable retention: active employee data kept indefinitely, terminated employee data kept for X years (default 10 — Swiss legal requirement), audit logs kept for X years, clock entries archived after X months. Automated cleanup jobs. | P1 |
| **GDPR / Data Privacy** | Employee data export (GDPR Article 15 — right of access). Employee data deletion request (GDPR Article 17 — right to erasure) with legal retention exceptions. Sensitive fields (national ID, bank details) encrypted at rest. Access logging for sensitive data views. | P1 |
| **Labor Law Compliance Reports** | Generate reports proving compliance: minimum rest periods met, maximum working hours respected, break rules followed. Exportable for labor inspections. Configurable per jurisdiction (Swiss, EU, custom). | P2 |

### 2.12 Team Communication (Inspired by 7shifts — cuts calls/texts/emails by 50%)

> 7shifts reports that in-app team communication reduces phone calls, text messages, and emails by 50%. This is the #1 most-requested feature after scheduling itself.

| Feature | Description | Priority |
|---------|-------------|----------|
| **Direct Messages** | One-to-one messaging between any two employees in the organization. Messages stored in the database, delivered via SignalR in real-time. Supports text, and file/image attachments (stored in Azure Blob). Read receipts (sender sees when recipient read the message). | P1 |
| **Group Channels** | Named group conversations: "Kitchen Team", "Friday Evening Crew", "All Staff". Channels can be open (anyone joins) or private (invite-only). Channel members receive notifications for new messages. | P1 |
| **Announcements (One-Way Broadcast)** | Managers post one-way announcements to all staff, a department, a location, or specific roles. Employees see announcements but cannot reply (reduces noise). **Read receipts** — manager sees who has read the announcement and who hasn't. Useful for policy changes, menu updates, event reminders. | P0 |
| **Shift-Context Messaging** | Messages can be linked to a specific shift or schedule period. E.g., "Everyone on Friday Evening: we have a private event, please wear black." Only employees assigned to that shift see the message. | P1 |
| **Message Search** | Full-text search across all conversations the user has access to. Filter by date, sender, channel. | P2 |

### 2.13 Manager Logbook (Inspired by 7shifts — 90% improved communication)

> 7shifts reports 90% of teams see improved communication using the Manager Logbook. This feature bridges the gap between shift handovers and keeps management aligned.

| Feature | Description | Priority |
|---------|-------------|----------|
| **Daily Shift Log** | Managers write shift notes at the end of each shift: what happened, issues, highlights, incidents. Notes are tagged by category (Operations, Staff, Maintenance, Customer Feedback, Safety, Inventory). Each entry has: author, timestamp, shift (morning/evening), location, and category. | P1 |
| **Auto-Populated Metrics** | If POS data is available, the logbook auto-inserts: total sales for the shift, labor cost %, covers served, average ticket size. If no POS, these fields are manually entered (optional). This gives managers a single place to see both qualitative notes and quantitative metrics. | P2 |
| **Task Status Integration** | The logbook automatically shows which tasks were completed during the shift and which are outstanding (links to Task Management module). | P2 |
| **Searchable History** | Managers can search past logbook entries by date, category, author, or keyword. Useful for tracking recurring issues ("How many times has the dishwasher broken down this month?"). | P2 |
| **Daily Email Digest** | Optionally send a daily summary email to all managers with the day's logbook entries. Keeps off-duty managers informed. | P2 |

### 2.14 Task Management (Inspired by 7shifts — 37% increase in task completion)

> 7shifts reports a 37% increase in task completion rates when using in-app task management. Tasks tied to shifts ensure accountability.

| Feature | Description | Priority |
|---------|-------------|----------|
| **Task Templates** | Define reusable task lists per shift or per role. E.g., "Opening Checklist" (check refrigerator temps, restock napkins, set up terrace), "Closing Checklist" (clean grills, mop floors, lock doors, count cash). Tasks can be daily, weekly, or one-time. Each task has: name, description, category, estimated duration, and a flag for "requires proof." | P1 |
| **Shift-Linked Tasks** | When a schedule is published, tasks are automatically assigned to the employees working that shift. E.g., the Opening shift always gets the Opening Checklist. | P1 |
| **Proof of Completion** | Tasks marked as "requires proof" need evidence: photo upload (e.g., photo of clean kitchen), temperature reading (e.g., walk-in cooler at 3°C), numeric value (e.g., cash count total), or text note. This is critical for food safety compliance. | P1 |
| **Real-Time Progress Tracking** | Managers see a live dashboard of task completion: how many tasks are done, in progress, overdue. Filter by shift, location, or employee. | P1 |
| **Task Completion Reports** | Weekly/monthly reports showing: completion rate per task, per employee, per shift. Identify which tasks are consistently skipped and which employees need coaching. | P2 |
| **Recurring Task Scheduler** | Tasks can be scheduled to recur: daily, specific days of week, weekly, monthly, or on specific dates. The system auto-generates task instances based on the schedule. | P1 |

### 2.15 Employee Engagement & Retention (Inspired by 7shifts + Harri)

> 65% of restaurant managers describe the labor market as "Tight" or "Very Tight" (7shifts 2025 Report). Sub-90-day turnover is the #1 cause of failed restaurant growth (Harri). Engagement tools directly address retention.

| Feature | Description | Priority |
|---------|-------------|----------|
| **Post-Shift Surveys** | After each shift, employees receive a short (1-3 question) survey: "How was your shift?" (1-5 stars), optional comment. Surveys are anonymous by default (configurable). Results aggregated by shift, location, role, and time period. Trend analysis: is morale improving or declining? | P2 |
| **Employee Recognition (Shout-Outs)** | Managers and peers can send public recognition: "Great job handling the rush tonight, Marie!" Recognition appears on a team feed visible to all staff. Categorized: Teamwork, Customer Service, Going Above & Beyond, Reliability, Leadership. | P2 |
| **Engagement Score Dashboard** | Aggregate metric combining: survey scores, attendance reliability (% of shifts with no-shows or late), shift acceptance rate (how often they accept open shifts), tenure, and recognition received. Displayed as a trend over time. | P2 |
| **Flight Risk Detection** | Algorithm flags employees who may be at risk of quitting based on: declining survey scores, increased time-off requests, reduced availability submissions, pattern of late arrivals, low engagement score trend. Alert sent to manager with suggested actions. | P3 |
| **Milestone Celebrations** | Automatic notifications when employees hit milestones: 30 days, 90 days, 6 months, 1 year, etc. Manager receives a reminder to acknowledge the milestone (reduces sub-90-day turnover). | P2 |
| **Exit Surveys** | When an employee is terminated or resigns, trigger an optional exit survey to capture reasons for leaving. Data feeds into retention analytics. | P3 |

### 2.16 AI-Powered Demand Forecasting & Labor Optimization (Inspired by CrunchTime + Lineup.ai)

> CrunchTime's AI forecasting achieves up to 95% accuracy and improvements up to 27% over traditional methods. This section defines Staff Pro's forecasting engine that uses POS sales data to predict optimal staffing levels.

| Feature | Description | Priority |
|---------|-------------|----------|
| **Sales Data Ingestion** | Receive hourly sales data from POS systems via `POST /api/pos/v1/sales-summary`. Store historical sales by: date, hour, day-of-week, location. Minimum 4 weeks of data needed before forecasting activates. | P2 |
| **Demand Forecasting Engine** | Predict expected sales/covers for future dates using: historical sales patterns (same day-of-week averages), seasonal trends (monthly patterns), weather data integration (optional — via OpenWeatherMap API), local events calendar (optional — manually entered events like "Football match nearby" or "Festival week"). Algorithm: weighted moving average initially, upgradeable to ML model as data grows. | P2 |
| **Staffing Recommendations** | Based on forecasted demand, calculate recommended labor hours per role per shift. Uses configurable ratios: "1 waiter per 4 tables", "1 kitchen staff per X covers/hour". Output: "Friday Evening forecast: 180 covers → need 4 waiters, 2 bartenders, 3 kitchen." Displayed as a recommendation layer on the schedule builder. | P2 |
| **SPLH Tracking (Sales Per Labor Hour)** | Calculate and track Sales Per Labor Hour — the industry-standard efficiency metric. SPLH = Total Sales ÷ Total Labor Hours. Target SPLH configurable per location/shift. Dashboard shows SPLH trends over time, compared to target. | P2 |
| **Labor Cost % Target** | Set target labor cost as percentage of revenue (e.g., 30%). The system calculates projected labor cost % for each schedule and warns when exceeding target. Real-time during schedule building: "This schedule projects 33.5% labor cost (target: 30%)." | P1 |
| **Base & Flex Scheduling** | Advanced pattern: schedule a "base" crew (guaranteed hours) + "flex" hours that are only activated if demand materializes. Flex employees are notified 24-48 hours in advance based on updated forecast. | P3 |
| **Forecast vs. Actual Report** | Compare forecasted sales/staffing with actual results. Track forecast accuracy over time. Helps tune the forecasting model. | P2 |

### 2.17 Geofencing & Anti-Fraud Clock System (Inspired by Deputy + Homebase + ClockIt)

> Deputy pioneered geofencing clock-in for restaurants. Studies show geofenced time clocks reduce time theft by 10-15%. Combined with photo verification, this provides the strongest anti-fraud clock system in the market.

| Feature | Description | Priority |
|---------|-------------|----------|
| **Geofence Zones** | Define virtual GPS boundaries around each location. Configurable radius (default 100m, range 50m-500m). Employees can only clock in when their device GPS confirms they are inside the geofence. If outside the geofence, clock-in is blocked with a message: "You are not at the restaurant location." | P1 |
| **GPS Location Capture** | Every clock entry records GPS coordinates (latitude + longitude) with accuracy radius. Stored alongside the clock entry for audit. Managers can view a map showing where each clock-in occurred. | P1 |
| **Photo Verification (Selfie Clock)** | Optionally require a front-camera selfie at clock-in. The photo is stored with the clock entry. Prevents buddy punching (one employee clocking in for another). Photo compared to profile photo (visual check by manager — full facial recognition is Phase 3). | P2 |
| **IP Address Restriction** | Optionally restrict clock-in to specific IP addresses (the restaurant's WiFi network). Useful for fixed locations with reliable WiFi. Can be combined with or used instead of geofencing. | P2 |
| **Kiosk Mode** | Shared tablet at the restaurant entrance serves as a time clock. Employees identify themselves via PIN, QR code scan (from their phone), or facial recognition (Phase 3). Kiosk mode locks the tablet to the clock screen (no other app access). | P1 |
| **Anti-Fraud Alerts** | Automatic alerts sent to manager when: clock-in from outside geofence (if override is allowed), GPS accuracy is too low (>100m radius, possible spoofing), multiple rapid clock-in/out attempts, clock-in from an unrecognized IP address. | P1 |
| **Offline Clock with GPS** | If the employee's phone is offline, the clock action is queued locally with the GPS coordinates captured at that moment. When connectivity returns, the entry syncs with the stored timestamp and location. Banner shows "Pending sync — 1 clock entry queued." | P1 |

### 2.18 Predictive Scheduling Compliance (Inspired by Fourth + CrunchTime)

> Predictive scheduling laws (Fair Workweek) now apply in New York City, Philadelphia, Chicago, Seattle, San Francisco, Los Angeles, and Oregon. Penalties range from $10-$75 per schedule change per employee. Staff Pro's compliance engine prevents costly violations.

| Feature | Description | Priority |
|---------|-------------|----------|
| **Advance Notice Rules** | Configure minimum notice period for schedule publication: 14 days (NYC, Chicago), 10 days (Oregon), or custom. The system blocks publishing a schedule if it falls within the notice window and calculates the earliest allowed publication date. | P2 |
| **Schedule Change Premium Calculator** | When a published schedule is modified within the notice period, automatically calculate the penalty premium owed to affected employees. Configurable premium tiers: changes >14 days = $0, changes 7-14 days = $10-$20, changes <7 days = $15-$45, changes <24 hours = $15-$75. Premium amounts stored per jurisdiction. | P2 |
| **Right to Refuse Tracking** | Track that employees were offered the right to refuse unscheduled shifts. When adding a shift to a published schedule, the system records: employee notified, employee accepted/refused, timestamp of notification. This documentation protects against labor complaints. | P2 |
| **Existing Employee First-Offer** | Before hiring new employees or posting an open shift externally, the system requires that existing qualified employees are offered the hours first. Tracks the offer chain for compliance documentation. | P2 |
| **Clopening Prevention** | Specific rule for jurisdictions that ban "clopening" (closing shift followed by opening shift with <11 hours rest). The conflict detection engine already checks minimum rest hours — this adds a specific compliance flag and premium calculation if a clopening is forced. | P1 |
| **Compliance Report Generator** | Generate jurisdiction-specific compliance reports showing: all schedule changes within notice period + premiums owed, advance notice proof (publication dates vs. schedule dates), right-to-refuse documentation, rest period compliance. Exportable as PDF for labor inspections. | P2 |
| **Jurisdiction Presets** | Pre-configured rule sets for: Swiss labor law, EU Working Time Directive, NYC Fair Workweek, Chicago Fair Workweek, Oregon predictive scheduling, Seattle secure scheduling, San Francisco Formula Retail. Admin selects jurisdiction(s) — rules auto-populate. | P2 |

### 2.19 Tip Management (Inspired by 7shifts)

> 7shifts is one of the few scheduling platforms that includes tip management. For restaurants with tipped employees, this eliminates spreadsheet-based tip distribution.

| Feature | Description | Priority |
|---------|-------------|----------|
| **Tip Pool Configuration** | Define tip pooling rules: which roles participate, what percentage each role receives, whether tips are distributed by hours worked or equal split. Multiple pool configurations (e.g., different rules for lunch vs. dinner). | P2 |
| **Tip Entry** | At the end of each shift, manager enters total tips collected. The system distributes tips according to the configured pool rules and each employee's worked hours. Individual tip amounts calculated automatically. | P2 |
| **Tip Distribution Preview** | Before finalizing, manager sees a preview of how tips will be distributed. Can make manual adjustments if needed (with audit trail). | P2 |
| **Tip Reporting** | Per employee per period: total tips received, average tips per hour, tip trends over time. Included in payroll export for tax reporting. | P2 |
| **POS Tip Integration** | If POS sends tip data via `POST /api/pos/v1/tips`, tips are auto-ingested and distributed without manual entry. | P3 |

### 2.20 Auto-Scheduling Engine (Inspired by 7shifts + Deputy + Lineup.ai — Saves 80% Scheduling Time)

> 7shifts reports auto-scheduling saves managers up to 80% of their scheduling time. Deputy's AI scheduler reduces labor costs by 2-4% while maintaining service levels. Lineup.ai achieves 95% forecast-to-schedule accuracy. Staff Pro combines constraint-based optimization with demand-driven staffing.

| Feature | Description | Priority |
|---------|-------------|----------|
| **One-Click Schedule Generation** | Manager clicks "Auto-Schedule" and the system generates a complete schedule for the period. The algorithm fills all staffing requirements by assigning the best-fit employees to each shift based on a scoring model. Output is a Draft schedule that the manager can review and adjust before publishing. Takes <30 seconds for up to 50 employees across a week. | P2 |
| **Constraint Solver Algorithm** | The auto-scheduler uses a weighted constraint satisfaction algorithm. Hard constraints (must satisfy): employee availability, maximum hours (contract/legal), minimum rest between shifts, required certifications, approved time-off, location assignment. Soft constraints (optimized): employee preferences (preferred shifts), hours fairness (distribute evenly among similar roles), labor cost (prefer lower-cost employees when skill is equal), consecutive working days (minimize), seniority preference, shift history (avoid always giving someone the undesirable shift). Each soft constraint has a configurable weight (0-100). | P2 |
| **Demand-Driven Staffing** | When demand forecast data is available (from POS integration), the auto-scheduler adjusts staffing levels per hour. Instead of fixed staffing templates, it uses forecasted covers to calculate: "Friday 18:00-19:00 expects 45 covers → need 5 waiters, not the usual 4." Staffing ratios (covers-per-server) are configurable per role. Falls back to staffing templates when no forecast data exists. | P2 |
| **Preference-Based Scheduling** | Employees can submit shift preferences: "I prefer morning shifts", "I'd like Friday evenings off", "I want at least 30 hours this week." Preferences are soft constraints — the system tries to honor them but prioritizes business needs. Preference satisfaction rate is tracked and displayed (e.g., "78% of preferences honored this week"). | P2 |
| **Shift Bidding** | For open or flexible shifts, employees can bid on preferred shifts before auto-scheduling runs. Bids are ranked by: seniority (configurable), past schedule fairness, manager priority override. The auto-scheduler considers bids as strong preferences. Employees see: "Your bid for Friday Evening was accepted" / "Your bid was not selected — you were assigned Saturday Evening instead." | P2 |
| **Schedule Templates (Full Week)** | Save an entire week's schedule as a named template: "Summer Weekday Pattern", "Holiday Season", "Special Event Layout." Templates store all shift assignments (role + station + times) without employee names. When loading a template, the auto-scheduler fills in employees based on current availability. Templates can be shared across locations. | P1 |
| **Fairness Algorithm** | Track cumulative hours, shift desirability scores, and weekend/holiday assignments per employee over a rolling period (configurable: 4-12 weeks). The auto-scheduler uses this history to ensure fair distribution. Metrics tracked: total hours assigned vs. contracted hours, number of weekend shifts, number of closing shifts, number of holiday shifts. Dashboard shows fairness scores per employee. | P2 |
| **What-If Scenarios** | Manager can generate multiple auto-schedule variants: "Cost-optimized" (minimize labor cost), "Employee-preferred" (maximize preference satisfaction), "Balanced" (default weights). Compare variants side-by-side before selecting one as the draft. | P3 |

### 2.21 Hiring & Applicant Tracking System (Inspired by Homebase + 7shifts + Push Operations)

> Homebase's AI Hiring Assistant reduces time-to-hire by 50% and posts to 5+ job boards in one click. 7shifts reports that restaurants using their hiring tools fill positions 3x faster. Push Operations covers the complete employee lifecycle from applicant to alumni. Staff Pro integrates hiring directly into the scheduling ecosystem.

| Feature | Description | Priority |
|---------|-------------|----------|
| **Job Posting Builder** | Create job listings with: role (links to existing roles in the system), location, employment type (full-time, part-time, seasonal, internship), pay range, description, requirements (certifications, experience, languages), shift expectations (e.g., "must be available weekends"). AI-assisted description generator (optional — generates professional listing from bullet points). | P2 |
| **Multi-Board Distribution** | Publish job postings to multiple job boards simultaneously. Integration via API with: Indeed, Glassdoor, LinkedIn, Google Jobs, and local Swiss job boards (Jobs.ch, Jobscout24). Each board has its own field mapping. Track which board generates the most applicants per role. | P2 |
| **Applicant Pipeline** | Track candidates through configurable stages: Applied → Screening → Phone Screen → Interview → Trial Shift → Offer → Hired / Rejected. Each stage has: status, notes, assigned reviewer, timestamp, automated email triggers. Kanban board view for visual pipeline management. | P2 |
| **AI Candidate Screening** | Optional AI-powered screening that ranks applicants based on: availability match (do their available hours match the role's typical schedule?), certification match, experience keywords, location proximity. Score: 0-100 with breakdown. Managers can filter: "Show me candidates scoring >70." | P3 |
| **Interview Scheduling** | Built-in interview scheduling: manager selects available time slots, candidate receives email with slot options, candidate picks a slot, both parties get calendar confirmation. Supports in-person and video (link to Zoom/Teams/Google Meet). Automated reminder emails (24h and 1h before). | P2 |
| **Trial Shift Management** | Restaurant-specific feature: schedule a trial shift for a candidate. The trial shift appears on the schedule (labeled "Trial") and the candidate receives instructions. After the trial, the manager submits an evaluation form. Trial shift hours can optionally be tracked for payment. | P2 |
| **Offer Letter Generation** | Generate offer letters from templates with auto-populated fields: candidate name, role, start date, hourly rate, contracted hours, probation period, manager name. Digital signature via email (candidate signs electronically). Signed offer stored in the employee's document folder. | P2 |
| **Applicant-to-Employee Conversion** | When a candidate is hired, one-click conversion creates an Employee record pre-filled with: name, email, phone, role, contract details from the offer letter. Triggers the onboarding checklist automatically. No duplicate data entry. | P2 |
| **Hiring Analytics** | Dashboard showing: time-to-hire per role (average days from posting to hire), applicants per source (which job board works best), pipeline conversion rates (% moving from each stage to the next), cost per hire (if job board fees are tracked), seasonal hiring trends. | P3 |
| **Referral Tracking** | Employees can refer candidates. If the referred candidate is hired and stays past probation, the referring employee is flagged for a referral bonus. Tracks: who referred whom, hire date, probation completion date, bonus eligibility. | P3 |

### 2.22 Employee Training & Knowledge Base (Inspired by Connecteam + Push Operations)

> Connecteam's AI course creator builds professional training courses in seconds. Restaurants using digital training see 40% faster onboarding and 25% fewer compliance violations. Staff Pro's training module links directly to role requirements and certification tracking.

| Feature | Description | Priority |
|---------|-------------|----------|
| **Course Builder** | Create training courses with: title, description, assigned roles (e.g., "all Kitchen staff must complete Food Safety"), modules (ordered sections), and completion criteria. Each module contains: text content (rich text / Markdown), embedded videos (YouTube/Vimeo URL or uploaded MP4), images, downloadable documents (PDFs, checklists), and quizzes. Courses can be mandatory (auto-assigned to new hires in the role) or optional. | P2 |
| **Quiz Engine** | Multiple-choice and true/false quizzes within courses. Configurable: passing score (e.g., 80%), maximum attempts, time limit (optional), randomize question order, show correct answers after submission (configurable). Quiz results stored per employee per attempt. Failed quizzes can trigger re-training requirements. | P2 |
| **Training Assignment & Tracking** | Assign courses to: individual employees, roles, departments, locations, or all staff. Track progress: not started, in progress (% complete), completed, failed (quiz score below passing). Due dates with automated reminders (7 days, 1 day before due). Manager dashboard shows: completion rates by course, overdue training, and employees needing attention. | P2 |
| **Certification Linkage** | Completing a training course can auto-update an employee's certification record. E.g., completing "Food Safety Level 2" course auto-sets the Food Safety certification expiry to +1 year. This closes the loop between training and the existing Skill & Certification Tracking module (2.1). | P2 |
| **Knowledge Base** | A searchable library of company documents, SOPs (Standard Operating Procedures), policies, and reference materials. Organized by category: Food Safety, Customer Service, Equipment Guides, HR Policies, Menu Knowledge, Allergen Information. Each article has: title, content (rich text), category, tags, last updated date, author. Employees can search by keyword. Version history tracked. | P2 |
| **Employee Handbook (Digital)** | A structured employee handbook that new hires must read and acknowledge during onboarding. Sections: Welcome, Company Values, Dress Code, Attendance Policy, Break Policy, Communication Guidelines, Disciplinary Process, Health & Safety, Data Privacy, etc. Each section has an "I have read and understood" checkbox. Acknowledgement records stored per employee with timestamp. | P2 |
| **Onboarding Workflow (Enhanced)** | Upgrade from basic checklist to a structured onboarding flow: Step 1: Digital paperwork (personal details, bank info, emergency contacts — employee self-service form). Step 2: Document uploads (ID, work permit, food safety cert). Step 3: Policy acknowledgement (handbook sections). Step 4: Training courses (auto-assigned based on role). Step 5: Manager checklist (uniform issued, locker assigned, systems access granted, buddy assigned). Progress tracked as percentage. Manager notified when employee completes each step. Target: new hire fully onboarded within 3 days. | P1 |
| **Training Analytics** | Reports: course completion rates per department, average time to complete, quiz pass rates, most-failed quiz questions (content improvement signals), training hours per employee per month, compliance training coverage (% of staff with required certifications current). | P3 |

### 2.23 Employee Self-Service Portal (Inspired by 7shifts + When I Work)

> 7shifts shows employees their estimated earnings from upcoming shifts, helping them plan financially. When I Work's self-service reduces HR inquiries by 60%. Staff Pro's self-service portal gives employees control over their own data and schedule preferences.

| Feature | Description | Priority |
|---------|-------------|----------|
| **Earnings Estimate** | Employees see projected earnings for the current/upcoming schedule period: sum of (scheduled hours × hourly rate) per shift, plus estimated tips (based on historical average), plus projected overtime premium. Shows: "This week: ~CHF 1,250 (32h regular + 4h overtime)." Updates in real-time as schedule changes. Displayed on the Employee Dashboard. | P1 |
| **Earnings History** | Historical earnings breakdown per pay period: regular pay, overtime pay, night/weekend premiums, tips, total gross. Trend chart showing earnings over the last 6 months. Employees can see exactly how their pay is calculated without asking HR. | P2 |
| **Profile Self-Service** | Employees can update their own: phone number, address, emergency contact, bank details (encrypted), profile photo, preferred language, notification preferences. Changes to sensitive fields (bank details, address) are flagged for HR review before activation. | P0 |
| **Document Access** | Employees can view their own documents: signed contracts, pay stubs (if generated), training certificates, performance reviews. Can upload documents (certificates, updated IDs) that go to HR for approval. | P1 |
| **Shift Preferences Submission** | Employees submit shift preferences for upcoming periods: preferred days, preferred shift times (morning/afternoon/evening), minimum desired hours, maximum desired hours, specific shift bids (if shift bidding is enabled). Preferences are used by the auto-scheduler. Employees see their preference satisfaction rate over time. | P2 |
| **Hours & Overtime Tracker** | Live view of: hours worked this week / this pay period, overtime hours accumulated, remaining hours until overtime threshold, projected end-of-period total. Helps employees track their own overtime and plan accordingly. | P1 |
| **Leave Balance & History** | Detailed view of all leave balances: vacation days remaining / total, sick days used, personal days remaining. Includes accrual schedule: "You earn 2.08 vacation days per month. Next accrual: March 1." Full history of past leave requests with status. | P1 |

### 2.24 Digital Forms & Checklists Builder (Inspired by Connecteam)

> Connecteam's digital forms replace paper-based processes in restaurants. Staff Pro's form builder extends beyond shift tasks to cover any operational form: incident reports, maintenance requests, inventory checks, health & safety inspections.

| Feature | Description | Priority |
|---------|-------------|----------|
| **Form Template Builder** | Drag-and-drop form builder with field types: text input, number input, date picker, dropdown select, multi-select checkboxes, radio buttons, photo capture (camera), file upload, signature pad, GPS location stamp, temperature reading, yes/no toggle, rating (1-5 stars), section header, instructional text. Forms can have conditional logic: "If answer to Q3 is 'Yes', show Q4." | P2 |
| **Form Categories** | Pre-defined categories: Food Safety (temperature logs, HACCP checklists), Health & Safety (incident reports, first aid logs), Operations (opening/closing checklists, equipment checks), HR (employee feedback forms, exit interviews), Maintenance (repair requests, equipment inspection), Custom. | P2 |
| **Form Assignment & Scheduling** | Assign forms to: specific shifts (auto-triggered), specific roles, specific employees, or available to all. Scheduling: one-time, daily, weekly, monthly, or event-triggered (e.g., "fill out incident report when accident occurs"). Due date/time enforcement with reminders. | P2 |
| **Form Submission & Storage** | Completed forms stored with: submitter, timestamp, GPS location (optional), all field responses, attached photos/files. Submissions are immutable (no editing after submission — new submission required for corrections). Exportable as PDF for regulatory compliance. | P2 |
| **Form Analytics** | Dashboard: completion rates per form, average completion time, common responses (for dropdown/rating fields), photo gallery for visual inspections, flagged submissions (values outside normal range — e.g., temperature >5°C triggers alert). | P3 |
| **Pre-Built Form Templates** | Ready-to-use templates: Daily Temperature Log (walk-in, fridge, freezer), Opening Checklist, Closing Checklist, Incident/Accident Report, Equipment Maintenance Request, Food Safety Inspection (HACCP), Cash Handling Reconciliation, Customer Complaint Form, Employee Feedback Form, Fire Safety Check, First Aid Log. Admins can customize or create from scratch. | P2 |

### 2.25 Company Newsfeed & Social Feed (Inspired by Connecteam + Sling)

> Connecteam's social feed increases employee engagement by 35%. Sling's newsfeed feature replaces scattered WhatsApp groups with a centralized company communication channel. Staff Pro's newsfeed complements the existing Announcements (one-way) and Team Chat (two-way) with a social-media-style feed.

| Feature | Description | Priority |
|---------|-------------|----------|
| **Company Feed** | A social-media-style feed where managers and employees can post updates visible to the whole team or specific groups. Posts support: text, images (multiple), file attachments, links, and emoji. Different from Announcements (which are one-way, no replies) and Chat (which is conversational). The feed is for company culture, celebrations, updates, and community building. | P2 |
| **Post Targeting** | Posts can target: all staff, specific department (FOH, BOH), specific location, specific role, or custom group. Employees only see posts targeted to their groups. | P2 |
| **Reactions & Comments** | Employees can react to posts (like, love, celebrate, funny, insightful — configurable emoji set) and leave comments. Threaded comments for organized discussion. Post author can disable comments for announcement-style posts. | P2 |
| **Recognition Integration** | Employee Recognition "shout-outs" (from section 2.15) appear in the feed automatically. When a manager or peer recognizes someone, it shows as a feed post with the recognition category badge. Other employees can react and comment. | P2 |
| **Milestone Auto-Posts** | Employment milestones (30 days, 90 days, 6 months, 1 year, etc.) auto-generate celebratory feed posts. Birthday posts (optional — employee must opt in). New hire welcome posts. Promotion/role-change announcements. | P2 |
| **Pinned Posts** | Managers can pin important posts to the top of the feed. Maximum 3 pinned posts at a time. Useful for: current week's specials, event reminders, policy updates. | P2 |
| **Content Moderation** | Configurable: all posts require manager approval before publishing, or open posting with manager ability to remove inappropriate content. Audit trail for removed posts. | P2 |

### 2.26 Employee Performance Reviews (Inspired by Push Operations)

> Push Operations reports restaurants using structured performance reviews see 20% lower turnover. Regular feedback reduces sub-90-day attrition — the #1 growth killer for restaurants (Harri). Staff Pro's reviews integrate with scheduling data for data-driven evaluations.

| Feature | Description | Priority |
|---------|-------------|----------|
| **Review Templates** | Configurable review templates per role. Each template has: review period (30-day, 90-day, 6-month, annual), sections with criteria (e.g., "Punctuality", "Customer Service", "Teamwork", "Technical Skills", "Initiative"), rating scale (1-5 stars or Exceeds/Meets/Below Expectations), free-text comment fields per section, and an overall rating. | P2 |
| **Automated Review Scheduling** | System auto-triggers review reminders based on: hire date milestones (30-day probation review, 90-day review, annual reviews), or configurable recurring schedules. Manager receives notification: "Pierre L.'s 90-day review is due in 7 days." | P2 |
| **Data-Driven Review Pre-fill** | Reviews auto-populate with scheduling data from the review period: attendance rate (% of shifts without late/no-show), average clock-in punctuality (+/- minutes), hours worked vs. contracted, overtime frequency, shift swap/drop frequency, task completion rate (from Task Management), recognition received count, engagement survey average. Manager uses this data to support their qualitative assessment. | P2 |
| **Self-Assessment** | Employees complete a self-assessment before the manager review. Same template, same criteria. Manager sees the self-assessment alongside their own rating to facilitate discussion. Highlights discrepancies (employee rates themselves 5 on "Teamwork" but manager rates 3). | P2 |
| **Review Meeting & Sign-off** | After manager completes the review, both parties meet (in-person or virtual). Review is shared with the employee. Employee can add comments. Both digitally sign the review (timestamp + electronic signature). Signed review stored in the employee's document history. | P2 |
| **Goal Setting** | During reviews, set goals for the next period: specific, measurable objectives (e.g., "Complete barista certification by March", "Reduce late arrivals to <2 per month", "Take on 2 closing shifts per week"). Goals tracked in the employee profile. Next review references goal completion. | P2 |
| **Performance Analytics** | Dashboard: average rating by department/role, rating distribution (how many employees at each level), trend over time, employees with declining ratings, top performers, correlation between review scores and retention. | P3 |

### 2.27 Minor Work Rules & Age Restriction Compliance (Inspired by 7shifts + Fourth)

> 7shifts automatically enforces minor labor laws, preventing costly violations. US jurisdictions have strict rules for workers under 18 (restricted hours, mandatory breaks, prohibited tasks). Swiss law (ArG Art. 29-32) has additional protections for young workers. Staff Pro's engine covers all jurisdictions.

| Feature | Description | Priority |
|---------|-------------|----------|
| **Age-Based Rules Engine** | Employees flagged as minors (calculated from date of birth) trigger additional scheduling constraints. Configurable rules per jurisdiction: maximum hours per day (e.g., 8h for 16-17, 6h for under 16), maximum hours per week (e.g., 40h for 16-17, 30h for under 16), prohibited hours (e.g., no work between 22:00-06:00 for under 18 in Switzerland), mandatory breaks (longer than adults — e.g., 30 min after 4.5 hours instead of 6 hours). | P2 |
| **Prohibited Task Enforcement** | Certain tasks/stations can be marked as "adults only." Minors cannot be assigned to these stations. Examples: operating heavy kitchen equipment (deep fryer, meat slicer), serving alcohol (jurisdiction-dependent), working alone without adult supervision. The conflict detection engine blocks these assignments with an Error-level conflict. | P2 |
| **School Schedule Integration** | For minor employees, the system can track school schedules: school days (restricted work hours), school holidays (relaxed hours), exam periods (special consideration). Availability is auto-limited on school days. | P3 |
| **Parental/Guardian Consent** | Track whether parental consent for employment has been obtained and stored (required for minors in most jurisdictions). Document stored in the employee's file. System alerts if consent is missing or expired. | P2 |
| **Minor Work Hours Report** | Compliance report specifically for minor employees: total hours per week (vs. legal maximum), shift times (any violations of prohibited hours), break compliance (enhanced requirements), total working days (consecutive day limits often stricter for minors). Exportable for labor inspections. | P2 |

### 2.28 Real-Time Intraday Labor Dashboard (Inspired by Restaurant365 + CrunchTime)

> Restaurant365's Operations Dashboard provides real-time intraday labor tracking, comparing actual vs. scheduled labor throughout the day. CrunchTime shows managers exactly when they're over or understaffed relative to sales. This is the #1 feature operators request for daily decision-making.

| Feature | Description | Priority |
|---------|-------------|----------|
| **Live Labor vs. Sales Tracker** | Real-time dashboard updated every 15 minutes (or on each clock event). Shows: current labor cost (based on who is clocked in right now × their hourly rates), current sales (from POS data), live labor cost percentage (labor ÷ sales), target labor % (configurable), variance (over/under target). Color-coded: green (under target), yellow (approaching), red (over target). | P2 |
| **Intraday Hourly Breakdown** | Hour-by-hour chart showing: scheduled labor hours, actual labor hours (clocked in), sales per hour, SPLH per hour. Managers see exactly which hours of the day are overstaffed or understaffed. Historical comparison: "Today vs. same day last week." | P2 |
| **Earned vs. Actual Hours** | Compare "earned hours" (the number of hours the restaurant should have staffed based on actual sales and productivity targets) vs. "actual hours" (hours actually worked). If actual > earned, the shift was overstaffed. If actual < earned, it was understaffed. This is 7shifts' key efficiency metric. Tracked per shift, per day, per week, per department. | P2 |
| **Real-Time Staffing Alerts** | Automated alerts when: labor % exceeds target for >1 hour, a department has no one clocked in (e.g., no bartender during bar hours), actual hours are trending significantly over earned hours, overtime threshold approaching for an employee. Alerts sent via in-app notification and optionally push notification. | P2 |
| **Daily Labor Summary** | Auto-generated end-of-day summary: total sales, total labor cost, labor %, SPLH, earned vs. actual hours variance, overtime hours incurred, no-shows, late arrivals, early departures. Compared to previous day and same-day-last-week. Feeds into the Manager Logbook auto-metrics. | P2 |

### 2.29 GPS Live Tracking & Route History (Inspired by Buddy Punch + Connecteam)

> Buddy Punch provides real-time employee location maps with route histories. Connecteam tracks GPS for mobile/delivery workers. Staff Pro's GPS features are essential for delivery drivers, catering staff, and multi-location employees.

| Feature | Description | Priority |
|---------|-------------|----------|
| **Live Employee Map** | Real-time map showing all currently clocked-in employees at their last-known GPS positions. Useful for: verifying delivery drivers are en route, confirming catering staff arrived at event location, seeing who is at which location in a multi-location setup. Map uses the same react-leaflet/Google Maps integration as geofencing. Updated every 5 minutes while clocked in. | P3 |
| **GPS Trail (Route History)** | For delivery drivers and mobile workers: capture GPS coordinates periodically (every 5 minutes) while clocked in. Display as a route trail on the map for the manager. Useful for: verifying delivery routes, tracking time spent at each stop, calculating mileage for reimbursement. | P3 |
| **Privacy Controls** | GPS tracking is opt-in at the role level (e.g., enabled for Delivery Drivers, disabled for Kitchen staff). Employees are clearly informed when GPS tracking is active. Tracking stops immediately on clock-out. Employees can see their own GPS data. Compliant with GDPR and Swiss data protection laws. GPS data auto-deleted after configurable retention period (default: 90 days). | P3 |
| **Mileage Calculation & Reimbursement** | For roles with GPS tracking enabled, auto-calculate total distance traveled during a shift using GPS waypoints. Apply configurable mileage rate (e.g., CHF 0.70/km). Add mileage reimbursement to the payroll export. | P3 |

---

## 2A. Real-World Workflow Scenarios

> These scenarios describe how a real restaurant ("La Dolce Vita") uses Staff Pro day-to-day. They show how the features connect together and what API calls happen behind the scenes.

### Scenario 1: Manager Builds Next Week's Schedule (Monday Morning)

**Who:** Jean (Manager) on his laptop  
**Goal:** Create the schedule for Feb 17–23

```
Step 1: Jean opens the Schedule Builder page
        → Frontend calls GET /api/schedules (filter: locationId, current week)
        → Backend returns existing draft or Jean creates new: POST /api/schedules
           { locationId: "...", startDate: "2026-02-17", endDate: "2026-02-23" }
        → New SchedulePeriod created with status: Draft

Step 2: Jean clicks "Copy Previous Week"
        → Frontend calls POST /api/schedules/{newId}/copy-from/{lastWeekId}
        → Backend copies all ShiftAssignments, then re-runs ConflictDetectionService
           on EVERY copied assignment against current week's data:
           - Checks availability (Marie submitted an override: unavailable Wed)
           - Checks time-off (Pierre has approved vacation Thu-Fri)
           - Checks certifications (Sophie's food safety cert expires Feb 18!)
        → Returns 48 copied assignments + 5 conflicts:
           [Error] Marie – unavailable Wed 19 (override submitted)
           [Error] Pierre – on approved vacation Thu 20
           [Error] Pierre – on approved vacation Fri 21
           [Warning] Sophie – food safety certificate expires Feb 18
           [Warning] Friday evening – 1 waiter short vs. staffing requirement

Step 3: Jean resolves conflicts in the schedule builder
        → Deletes Marie's Wed shift: DELETE /api/schedules/{id}/assignments/{assignmentId}
        → Deletes Pierre's Thu+Fri shifts
        → Drags "Luca" (available, qualified Waiter) onto Wed:
           POST /api/schedules/{id}/assignments
           { employeeId: "luca-id", date: "2026-02-19", shiftTemplateId: "evening-id",
             stationId: "terrace-id", roleId: "waiter-id" }
        → Backend runs ConflictDetectionService.CheckAssignment() → No conflicts → 201 Created
        → Assigns Sophie's Wed shift but acknowledges cert warning (soft block)

Step 4: Jean checks staffing coverage
        → Frontend calls GET /api/schedules/{id}/staffing-coverage
        → Returns per-day, per-role: assigned vs. required
           { "2026-02-21": { "Waiter": { assigned: 3, required: 4, status: "Understaffed" } } }
        → Jean creates an Open Shift for Friday: POST /api/open-shifts
           { date: "2026-02-21", roleId: "waiter-id", startTime: "17:00", endTime: "23:30" }

Step 5: Jean checks labor cost
        → Frontend calls GET /api/schedules/{id}/labor-cost
        → Returns: { totalHours: 312, totalCostCents: 842500, budgetCents: 900000, 
                      laborCostPercent: 31.2, targetPercent: 33.0, status: "UnderBudget" }

Step 6: Jean publishes the schedule
        → Frontend calls POST /api/schedules/{id}/publish
        → Backend changes status Draft → Published, sets publishedAt timestamp
        → Publishes SchedulePublishedEvent via MediatR
        → Event handler sends notifications to all 14 assigned employees:
           - In-app notification (SignalR push)
           - Email (via Hangfire outbox → SendGrid)
        → Each employee receives: "Schedule for Feb 17–23 is published. You have 4 shifts."
        → Open Shift notification sent to all available, qualified Waiters
```

### Scenario 2: Employee's Full Shift Day (Wednesday)

**Who:** Marie (Waiter, Senior) on her phone  
**Shift:** Morning 09:00–15:00 at Terrace

```
08:50  Marie opens Staff Pro on her phone
       → App shows Employee Dashboard with today's shift:
         "Morning (09:00–15:00) · Terrace · Waiter"
       → Clock widget shows big green "CLOCK IN" button
       → Her current tasks load: GET /api/tasks?date=2026-02-19&employeeId=marie-id
         → 6 Opening Checklist tasks + 2 Shift tasks

08:55  Marie taps "CLOCK IN"
       → Frontend captures GPS: { lat: 47.3769, lng: 8.5417, accuracy: 12m }
       → Calls POST /api/clock/verify-location { latitude, longitude }
       → Backend checks against GeofenceZone (center: 47.3770, 8.5418, radius: 100m)
         → Distance: 14m → INSIDE geofence → ✅ Allowed
       → Calls POST /api/clock/in
         { shiftAssignmentId: "...", latitude: 47.3769, longitude: 8.5417, ipAddress: "192.168.1.50" }
       → Backend applies grace period: 08:55 is within 5-min early window → records as 09:00
       → ClockEntry created: { type: ShiftStart, timestamp: 09:00 UTC }
       → Clock widget changes to: "Clocked in since 09:00 · Duration: 0h 0m"

09:05  Marie completes Opening Checklist tasks
       → Task: "Check walk-in cooler temperature" (requires proof: Temperature + Photo)
       → Marie takes photo of thermometer, enters "3.1"
       → POST /api/tasks/{id}/complete
         { proofValue: "3.1", proofPhotoUrl: "uploaded-blob-url" }
       → Task: "Set up terrace tables" → POST /api/tasks/{id}/complete (no proof required)
       → Progress: 2/6 → 33%

11:30  Marie takes her break
       → Taps "START BREAK"
       → POST /api/clock/break-start
       → ClockEntry: { type: BreakStart, timestamp: 11:30 }
       → Widget: "On break · Started 11:30"

12:00  Marie ends break (30 min — matches shift template)
       → Taps "END BREAK"
       → POST /api/clock/break-end
       → ClockEntry: { type: BreakEnd, timestamp: 12:00 }
       → Break duration: 30 min (matches template → no flag)

12:15  Marie gets a chat message from Jean (manager)
       → SignalR pushes NewChatMessage event
       → Notification: "Jean: Can you stay until 16:00? We're short for lunch rush"
       → Marie replies in-app: "Sure, no problem!"

15:05  Marie clocks out
       → POST /api/clock/out
       → ClockEntry: { type: ShiftEnd, timestamp: 15:05 }
       → Rounding rule: nearest 5 min → 15:05 stays as 15:05
       → Total worked: 6h 5m (scheduled: 6h) → 5 min overtime
       → System records actual vs. scheduled variance

15:10  Post-shift survey pops up (after 5-min delay from clock-out)
       → "How was your shift?" ⭐⭐⭐⭐ (4/5)
       → Optional comment: "Good shift, terrace was busy!"
       → POST /api/engagement/surveys
       → Response stored, feeds into team sentiment trend
```

### Scenario 3: End-of-Week Timesheet Approval (Sunday Night)

**Who:** Jean (Manager) on his laptop  
**Goal:** Approve timesheets for the week Feb 17–23

```
Step 1: Jean opens Timesheets page
        → GET /api/timesheets/periods?locationId=...&status=Open
        → Returns current period: Feb 17–23, Status: Open, 14 employees

Step 2: Background job has already auto-generated timesheet entries
        (TimesheetAutoGeneration ran at 02:00 each night)
        Each entry calculated from clock entries:
        - Marie: scheduled 40h, actual 41h 25m, regular: 40h, overtime: 1h 25m
        - Pierre: scheduled 42h, actual 44h, regular: 42h, overtime: 2h, night: 6h
        - Sophie: scheduled 24h, actual 23h 30m (left 30 min early Wednesday)

Step 3: Jean reviews each entry
        → GET /api/timesheets/periods/{id} (includes all entries)
        → Sophie has a discrepancy: 30 min less than scheduled
        → Jean adds a note: "Left early Wed — approved by shift lead"
        → PUT /api/timesheets/entries/{id} { managerNote: "..." }

Step 4: Jean approves all entries
        → For each: POST /api/timesheets/entries/{id}/approve
        → Status changes: AutoGenerated → ManagerApproved
        → Sophie's entry: Jean adjusts actual hours to match scheduled (she had permission)

Step 5: Jean generates payroll export
        → POST /api/exports/payroll-csv { periodId: "...", format: "abacus" }
        → Returns CSV with columns matching Abacus payroll software:
          EmployeeNumber, Name, RegularHours, OvertimeHours, NightHours,
          WeekendHours, HourlyRate, OvertimeRate, GrossPay, Tips

Step 6: Jean writes logbook entry for the week
        → POST /api/logbook
        { date: "2026-02-23", shiftName: "Weekly Summary", category: "Operations",
          content: "Strong week. Friday event went well. Need to hire 1 more waiter for 
          weekend coverage. Sophie cert expires — remind her to renew." }
```

### Scenario 4: Employee Requests Time Off and Manager Approves

**Who:** Pierre (Chef) and Jean (Manager)  
**Goal:** Pierre wants 3 days off for a family trip

```
Pierre's Phone:
1. Opens "My Time Off" page
   → GET /api/time-off/balances/pierre-id
   → Shows: Vacation: 18 days remaining (of 25), Sick: unlimited

2. Taps "Request Time Off"
   → Selects: Leave Type: Vacation, Dates: March 5-7 (Thu-Sat), Reason: "Family ski trip"
   → POST /api/time-off/requests
     { leaveTypeId: "vacation-id", startDate: "2026-03-05", endDate: "2026-03-07",
       reason: "Family ski trip" }
   → Status: Pending
   → Notification sent to Jean (manager)

Jean's Laptop (next morning):
3. Sees "3 Pending Requests" on Manager Dashboard
   → GET /api/time-off/requests?status=Pending

4. Opens Pierre's request
   → GET /api/time-off/requests/{id}
   → Checks staffing impact: GET /api/time-off/requests/{id}/impact
   → Returns:
     { "2026-03-05": { "Kitchen": { remaining: 2, required: 2, status: "Tight" } },
       "2026-03-06": { "Kitchen": { remaining: 1, required: 3, status: "Understaffed!" } },
       "2026-03-07": { "Kitchen": { remaining: 2, required: 3, status: "Understaffed!" } } }
   → March 6 (Friday) and 7 (Saturday) are problematic — only 1-2 kitchen staff vs. 3 needed

5. Jean checks who else is available for those dates
   → GET /api/availability/summary?startDate=2026-03-05&endDate=2026-03-07&roleId=kitchen-id
   → Shows Luca is available Fri+Sat, trainee cook Marco can cover Thu

6. Jean approves Pierre's request knowing coverage is possible
   → POST /api/time-off/requests/{id}/approve
   → Backend deducts 3 days from Pierre's vacation balance (18 → 15)
   → Pierre receives notification: "Your time-off request (Mar 5-7) has been approved!"
   → Pierre's shifts for those dates auto-removed from the schedule (if already published)
   → System creates staffing alerts for Mar 5-7 on the schedule builder
```

---

## 3. Database Schema

### 3.1 Entity Relationship Summary

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                           ORGANIZATIONAL STRUCTURE                           │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                              │
│  Organization (1) ──── (N) Location                                          │
│  Organization (1) ──── (N) Department                                        │
│  Organization (1) ──── (N) Role                                              │
│  Organization (1) ──── (N) LeaveType                                         │
│  Organization (1) ──── (N) ShiftTemplate                                     │
│  Organization (1) ──── (N) SchedulingRule                                    │
│  Organization (1) ──── (N) PosConnection                                     │
│  Organization (1) ──── (N) ChatChannel                                       │
│  Organization (1) ──── (N) TipPool                                           │
│  Organization (1) ──── (N) TaskTemplate                                      │
│                                                                              │
│  Location (1) ──── (N) Station                                               │
│  Location (1) ──── (N) SchedulePeriod                                        │
│  Location (1) ──── (N) BlackoutDate                                          │
│  Location (1) ──── (N) StaffingRequirement                                   │
│  Location (1) ──── (N) GeofenceZone                                          │
│  Location (1) ──── (N) DemandForecast                                        │
│  Location (1) ──── (N) ManagerLogEntry                                       │
│                                                                              │
│  Department (1) ──── (N) Role                                                │
│  Station (N) ─── StationRole ─── (N) Role    (many-to-many)                 │
│                                                                              │
├─────────────────────────────────────────────────────────────────────────────┤
│                              EMPLOYEE DATA                                   │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                              │
│  Employee (1) ──── (N) EmployeeRole          (multi-role assignment)         │
│  Employee (1) ──── (N) EmployeeSkill         (certifications)                │
│  Employee (1) ──── (N) EmployeeLocation      (multi-location)                │
│  Employee (1) ──── (N) Contract              (only 1 active at a time)       │
│  Employee (1) ──── (N) AvailabilityRule      (recurring weekly patterns)     │
│  Employee (1) ──── (N) AvailabilityOverride  (one-off date overrides)        │
│  Employee (1) ──── (N) TimeOffRequest                                        │
│  Employee (1) ──── (N) LeaveBalance          (per leave type per year)       │
│  Employee (1) ──── (N) ShiftAssignment                                       │
│  Employee (1) ──── (N) ClockEntry                                            │
│  Employee (1) ──── (N) EmployeeDocument                                      │
│  Employee (1) ──── (N) EmployeeNote                                          │
│  Employee (1) ──── (N) OnboardingItem                                        │
│  Employee (1) ──── (N) EngagementSurveyResponse                              │
│  Employee (1) ──── (N) EmployeeRecognition   (as recipient)                  │
│  Employee (1) ──── (N) TipDistribution                                       │
│                                                                              │
├─────────────────────────────────────────────────────────────────────────────┤
│                         SCHEDULING & TIME TRACKING                           │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                              │
│  SchedulePeriod (1) ──── (N) ShiftAssignment                                 │
│  ShiftAssignment (1) ──── (N) ClockEntry                                     │
│  ShiftAssignment (1) ──── (1) TimesheetEntry                                 │
│  ShiftAssignment (1) ──── (N) ShiftTaskInstance  (tasks for this shift)      │
│  ShiftAssignment (1) ──── (N) ScheduleChangePremium  (compliance)            │
│                                                                              │
│  TimesheetPeriod (1) ──── (N) TimesheetEntry                                 │
│                                                                              │
│  ShiftSwapRequest ──── ShiftAssignment (source + target)                     │
│  OpenShift (1) ──── (N) OpenShiftClaim                                       │
│                                                                              │
├─────────────────────────────────────────────────────────────────────────────┤
│                           COMMUNICATION & TASKS                              │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                              │
│  ChatChannel (1) ──── (N) ChatChannelMember                                  │
│  ChatChannel (1) ──── (N) ChatMessage                                        │
│  ChatMessage (1) ──── (N) ChatMessageReadReceipt                             │
│                                                                              │
│  TaskTemplate (1) ──── (N) TaskTemplateItem                                  │
│  TaskTemplateItem (1) ──── (N) ShiftTaskInstance                             │
│                                                                              │
├─────────────────────────────────────────────────────────────────────────────┤
│                        FORECASTING & OPTIMIZATION                            │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                              │
│  DemandForecast ──── Location (per hour per day)                             │
│  StaffingRecommendation ──── Location + Role + ShiftTemplate                 │
│                                                                              │
├─────────────────────────────────────────────────────────────────────────────┤
│                              TIP MANAGEMENT                                  │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                              │
│  TipPool (1) ──── (N) TipPoolRole     (which roles get what %)              │
│  TipPool (1) ──── (N) TipEntry        (daily tip records)                   │
│  TipEntry (1) ──── (N) TipDistribution (per-employee amounts)               │
│                                                                              │
├─────────────────────────────────────────────────────────────────────────────┤
│                          HIRING & RECRUITMENT (ATS)                          │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                              │
│  JobPosting (1) ──── (N) JobApplication   (candidates apply)                │
│  JobApplication (1) ──── (N) ApplicationStageHistory (pipeline stages)      │
│  JobApplication (1) ──── (N) InterviewSlot  (scheduled interviews)          │
│  JobApplication (1) ──── (0..1) TrialShift  (trial shift assignment)        │
│  JobPosting ──── Role + Location                                            │
│  JobApplication ──── Employee (after conversion, nullable until hired)       │
│                                                                              │
├─────────────────────────────────────────────────────────────────────────────┤
│                       TRAINING & KNOWLEDGE BASE (LMS)                        │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                              │
│  TrainingCourse (1) ──── (N) TrainingModule   (ordered sections)            │
│  TrainingModule (1) ──── (N) QuizQuestion     (quiz within module)          │
│  TrainingCourse (1) ──── (N) TrainingAssignment (per employee)              │
│  TrainingAssignment (1) ──── (N) ModuleProgress (per module completion)     │
│  TrainingAssignment (1) ──── (N) QuizAttempt    (quiz results)              │
│  KnowledgeBaseArticle ──── Category (searchable SOPs/policies)              │
│  HandbookSection (1) ──── (N) HandbookAcknowledgement (employee sign-off)   │
│                                                                              │
├─────────────────────────────────────────────────────────────────────────────┤
│                            DIGITAL FORMS ENGINE                              │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                              │
│  FormTemplate (1) ──── (N) FormField         (draggable field definitions)  │
│  FormTemplate (1) ──── (N) FormSubmission    (completed form instances)     │
│  FormSubmission (1) ──── (N) FormFieldResponse (field-level answers)        │
│  FormField ──── conditional logic (show/hide based on other field values)    │
│                                                                              │
├─────────────────────────────────────────────────────────────────────────────┤
│                      NEWSFEED & PERFORMANCE REVIEWS                          │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                              │
│  FeedPost (1) ──── (N) FeedComment     (threaded comments)                  │
│  FeedPost (1) ──── (N) FeedReaction    (emoji reactions per user)           │
│  FeedPost ──── PostTarget (all / department / location / role)              │
│                                                                              │
│  ReviewTemplate (1) ──── (N) ReviewCriteria   (rating sections)             │
│  PerformanceReview ──── Employee + ReviewTemplate + Reviewer                │
│  PerformanceReview (1) ──── (N) ReviewResponse  (per-criteria ratings)      │
│  PerformanceReview (1) ──── (N) PerformanceGoal (goals for next period)     │
│                                                                              │
├─────────────────────────────────────────────────────────────────────────────┤
│                    SCHEDULE TEMPLATES & AUTO-SCHEDULING                       │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                              │
│  WeeklyScheduleTemplate (1) ──── (N) TemplateShiftSlot  (role+station+time) │
│  ShiftBid ──── Employee + ShiftTemplate + Date (preference bids)            │
│  ShiftPreference ──── Employee (preferred days/times/hours)                  │
│  AutoScheduleRun (1) ──── (N) AutoScheduleResult (generated assignments)    │
│                                                                              │
├─────────────────────────────────────────────────────────────────────────────┤
│                          GPS TRACKING (DELIVERY)                             │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                              │
│  GpsWaypoint ──── Employee + ClockEntry (periodic location capture)         │
│  MileageRecord ──── Employee + Date (calculated distance + reimbursement)    │
│                                                                              │
└─────────────────────────────────────────────────────────────────────────────┘
```

### 3.2 Complete Entity Definitions

#### Organization (Multi-Tenant Root)

```csharp
public class Organization
{
    public Guid Id { get; set; }
    public string Name { get; set; }                    // "La Dolce Vita Restaurant"
    public string Slug { get; set; }                    // "la-dolce-vita" (URL-safe unique)
    public string? LogoUrl { get; set; }
    public string TimeZone { get; set; }                // "Europe/Zurich"
    public string Currency { get; set; }                // "CHF"
    public string DefaultLanguage { get; set; }         // "en"
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string BillingPlan { get; set; }             // "free", "pro", "enterprise"
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation
    public ICollection<Location> Locations { get; set; }
    public ICollection<Department> Departments { get; set; }
    public ICollection<Role> Roles { get; set; }
    public ICollection<LeaveType> LeaveTypes { get; set; }
    public ICollection<ShiftTemplate> ShiftTemplates { get; set; }
    public ICollection<SchedulingRule> SchedulingRules { get; set; }
    public ICollection<PosConnection> PosConnections { get; set; }
}
```

#### Location

```csharp
public class Location
{
    public Guid Id { get; set; }
    public Guid OrganizationId { get; set; }
    public string Name { get; set; }                    // "Main Restaurant", "Airport Branch"
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public string TimeZone { get; set; }                // can override org-level
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation
    public Organization Organization { get; set; }
    public ICollection<Station> Stations { get; set; }
    public ICollection<SchedulePeriod> SchedulePeriods { get; set; }
    public ICollection<BlackoutDate> BlackoutDates { get; set; }
    public ICollection<StaffingRequirement> StaffingRequirements { get; set; }
}
```

#### Department

```csharp
public class Department
{
    public Guid Id { get; set; }
    public Guid OrganizationId { get; set; }
    public string Name { get; set; }                    // "Front of House", "Back of House"
    public string? Description { get; set; }
    public string? ColorHex { get; set; }               // "#4CAF50"
    public int SortOrder { get; set; }
    public DateTime CreatedAt { get; set; }

    // Navigation
    public Organization Organization { get; set; }
    public ICollection<Role> Roles { get; set; }
}
```

#### Role

```csharp
public class Role
{
    public Guid Id { get; set; }
    public Guid OrganizationId { get; set; }
    public Guid? DepartmentId { get; set; }
    public string Name { get; set; }                    // "Waiter", "Chef", "Bartender"
    public string? Description { get; set; }
    public decimal? DefaultHourlyRate { get; set; }     // in cents (e.g., 2500 = 25.00 CHF)
    public string ColorHex { get; set; }                // "#FF5722" — for schedule UI
    public int SortOrder { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }

    // Navigation
    public Organization Organization { get; set; }
    public Department? Department { get; set; }
    public ICollection<EmployeeRole> EmployeeRoles { get; set; }
    public ICollection<RolePermission> Permissions { get; set; }
    public ICollection<RoleCertificationRequirement> RequiredCertifications { get; set; }
    public ICollection<StationRole> StationRoles { get; set; }
}
```

#### Station

```csharp
public class Station
{
    public Guid Id { get; set; }
    public Guid LocationId { get; set; }
    public string Name { get; set; }                    // "Bar", "Terrace", "Kitchen A"
    public string? Description { get; set; }
    public int MaxCapacity { get; set; }                // max people who can work here
    public string? ColorHex { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }

    // Navigation
    public Location Location { get; set; }
    public ICollection<StationRole> StationRoles { get; set; }  // which roles can work here
}
```

#### StationRole (Many-to-Many: Station ↔ Role)

```csharp
public class StationRole
{
    public Guid StationId { get; set; }
    public Guid RoleId { get; set; }

    // Navigation
    public Station Station { get; set; }
    public Role Role { get; set; }
}
```

#### Employee

```csharp
public class Employee
{
    public Guid Id { get; set; }
    public Guid OrganizationId { get; set; }
    public string? AppUserId { get; set; }              // links to ASP.NET Identity user (nullable — some employees may not have app access)
    
    // Personal Info
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? ProfilePhotoUrl { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? PostalCode { get; set; }
    public string? Country { get; set; }
    
    // Emergency Contact
    public string? EmergencyContactName { get; set; }
    public string? EmergencyContactPhone { get; set; }
    public string? EmergencyContactRelationship { get; set; }
    
    // Employment
    public string EmployeeNumber { get; set; }          // auto-generated or manual (e.g., "EMP-001")
    public DateTime HireDate { get; set; }
    public DateTime? TerminationDate { get; set; }
    public string? TerminationReason { get; set; }
    public EmployeeStatus Status { get; set; }          // Active, OnLeave, Suspended, Terminated
    
    // Sensitive (encrypted at rest)
    public string? NationalId { get; set; }
    public string? WorkPermitNumber { get; set; }
    public DateTime? WorkPermitExpiry { get; set; }
    public string? BankAccountIban { get; set; }
    
    // Metadata
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }                 // soft delete

    // Navigation
    public Organization Organization { get; set; }
    public AppUser? AppUser { get; set; }
    public ICollection<EmployeeRole> EmployeeRoles { get; set; }
    public ICollection<EmployeeSkill> EmployeeSkills { get; set; }
    public ICollection<EmployeeLocation> EmployeeLocations { get; set; }
    public ICollection<Contract> Contracts { get; set; }
    public ICollection<AvailabilityRule> AvailabilityRules { get; set; }
    public ICollection<AvailabilityOverride> AvailabilityOverrides { get; set; }
    public ICollection<TimeOffRequest> TimeOffRequests { get; set; }
    public ICollection<ShiftAssignment> ShiftAssignments { get; set; }
    public ICollection<ClockEntry> ClockEntries { get; set; }
    public ICollection<LeaveBalance> LeaveBalances { get; set; }
    public ICollection<EmployeeDocument> Documents { get; set; }
    public ICollection<EmployeeNote> EmployeeNotes { get; set; }
    public ICollection<OnboardingItem> OnboardingItems { get; set; }
}
```

#### Contract

```csharp
public class Contract
{
    public Guid Id { get; set; }
    public Guid EmployeeId { get; set; }
    public ContractType Type { get; set; }              // FullTime, PartTime, Hourly, Internship, Seasonal
    public decimal HourlyRateCents { get; set; }        // stored as integer cents (2500 = 25.00)
    public decimal? MonthlySalaryCents { get; set; }    // for salaried employees
    public decimal ContractedHoursPerWeek { get; set; } // e.g., 42.0 for full-time Swiss
    public decimal OvertimeThresholdDaily { get; set; } // hours after which daily OT kicks in
    public decimal OvertimeThresholdWeekly { get; set; }// hours after which weekly OT kicks in
    public decimal OvertimeMultiplier { get; set; }     // 1.25 = 125% pay, 1.5 = 150% pay
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }              // null = permanent
    public DateTime? ProbationEndDate { get; set; }
    public int NoticePeriodDays { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation
    public Employee Employee { get; set; }
}
```

#### EmployeeRole (Many-to-Many: Employee ↔ Role)

```csharp
public class EmployeeRole
{
    public Guid Id { get; set; }
    public Guid EmployeeId { get; set; }
    public Guid RoleId { get; set; }
    public ProficiencyLevel Proficiency { get; set; }   // Trainee, Junior, Senior, Lead
    public bool IsPrimary { get; set; }
    public DateTime AssignedAt { get; set; }

    // Navigation
    public Employee Employee { get; set; }
    public Role Role { get; set; }
}
```

#### EmployeeSkill

```csharp
public class EmployeeSkill
{
    public Guid Id { get; set; }
    public Guid EmployeeId { get; set; }
    public string SkillName { get; set; }               // "Food Safety Certificate", "WSET Level 2"
    public string? CertificateNumber { get; set; }
    public DateTime? IssuedDate { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public string? DocumentUrl { get; set; }            // link to uploaded certificate scan
    public bool IsMandatory { get; set; }               // required for their role?
    public DateTime CreatedAt { get; set; }

    // Navigation
    public Employee Employee { get; set; }
}
```

#### AvailabilityRule (Recurring Weekly Pattern)

```csharp
public class AvailabilityRule
{
    public Guid Id { get; set; }
    public Guid EmployeeId { get; set; }
    public DayOfWeek DayOfWeek { get; set; }            // Monday = 1, etc.
    public TimeOnly StartTime { get; set; }             // 09:00
    public TimeOnly EndTime { get; set; }               // 23:00
    public AvailabilityType Type { get; set; }          // Available, Preferred, Unavailable
    public DateTime EffectiveFrom { get; set; }
    public DateTime? EffectiveUntil { get; set; }
    public DateTime CreatedAt { get; set; }

    // Navigation
    public Employee Employee { get; set; }
}
```

#### AvailabilityOverride (One-Off Date Override)

```csharp
public class AvailabilityOverride
{
    public Guid Id { get; set; }
    public Guid EmployeeId { get; set; }
    public DateOnly Date { get; set; }
    public TimeOnly? StartTime { get; set; }            // null = entire day
    public TimeOnly? EndTime { get; set; }
    public AvailabilityType Type { get; set; }          // Available, Unavailable
    public string? Note { get; set; }
    public DateTime CreatedAt { get; set; }

    // Navigation
    public Employee Employee { get; set; }
}
```

#### ShiftTemplate

```csharp
public class ShiftTemplate
{
    public Guid Id { get; set; }
    public Guid OrganizationId { get; set; }
    public string Name { get; set; }                    // "Morning", "Evening", "Split"
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public int BreakDurationMinutes { get; set; }       // e.g., 30
    public bool BreakIsPaid { get; set; }
    public string ColorHex { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }

    // Navigation
    public Organization Organization { get; set; }
}
```

#### SchedulePeriod

```csharp
public class SchedulePeriod
{
    public Guid Id { get; set; }
    public Guid LocationId { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public ScheduleStatus Status { get; set; }          // Draft, Published, Locked
    public DateTime? PublishedAt { get; set; }
    public string? PublishedByUserId { get; set; }
    public DateTime? LockedAt { get; set; }
    public string? LockedByUserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation
    public Location Location { get; set; }
    public ICollection<ShiftAssignment> ShiftAssignments { get; set; }
}
```

#### ShiftAssignment

```csharp
public class ShiftAssignment
{
    public Guid Id { get; set; }
    public Guid SchedulePeriodId { get; set; }
    public Guid EmployeeId { get; set; }
    public Guid? ShiftTemplateId { get; set; }          // null if custom times
    public Guid? StationId { get; set; }
    public Guid? RoleId { get; set; }
    public DateOnly Date { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public int BreakDurationMinutes { get; set; }
    public bool BreakIsPaid { get; set; }
    public ShiftAssignmentStatus Status { get; set; }   // Scheduled, Confirmed, Completed, NoShow, Cancelled
    public string? Notes { get; set; }
    public decimal? EstimatedCostCents { get; set; }    // pre-calculated based on hourly rate × hours
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation
    public SchedulePeriod SchedulePeriod { get; set; }
    public Employee Employee { get; set; }
    public ShiftTemplate? ShiftTemplate { get; set; }
    public Station? Station { get; set; }
    public Role? Role { get; set; }
    public ICollection<ClockEntry> ClockEntries { get; set; }
    public TimesheetEntry? TimesheetEntry { get; set; }
}
```

#### ClockEntry

```csharp
public class ClockEntry
{
    public Guid Id { get; set; }
    public Guid EmployeeId { get; set; }
    public Guid? ShiftAssignmentId { get; set; }        // nullable — can clock without a scheduled shift
    public ClockEntryType Type { get; set; }            // ShiftStart, BreakStart, BreakEnd, ShiftEnd
    public DateTime Timestamp { get; set; }             // UTC
    public DateTime? OriginalTimestamp { get; set; }    // if manager edited, store original
    public string? EditedByUserId { get; set; }
    public string? EditReason { get; set; }
    public ClockSource Source { get; set; }             // WebApp, PosSync, ManagerManual
    public string? IpAddress { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public DateTime CreatedAt { get; set; }

    // Navigation
    public Employee Employee { get; set; }
    public ShiftAssignment? ShiftAssignment { get; set; }
}
```

#### LeaveType

```csharp
public class LeaveType
{
    public Guid Id { get; set; }
    public Guid OrganizationId { get; set; }
    public string Name { get; set; }                    // "Vacation", "Sick Leave", "Personal Day"
    public bool IsPaid { get; set; }
    public bool RequiresDocument { get; set; }          // e.g., sick note required
    public int? MaxDaysPerYear { get; set; }            // null = unlimited
    public decimal? AccrualRatePerMonth { get; set; }   // e.g., 2.08 (25 days / 12 months)
    public int? MaxCarryOverDays { get; set; }          // how many unused days carry to next year
    public string ColorHex { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }

    // Navigation
    public Organization Organization { get; set; }
}
```

#### TimeOffRequest

```csharp
public class TimeOffRequest
{
    public Guid Id { get; set; }
    public Guid EmployeeId { get; set; }
    public Guid LeaveTypeId { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public TimeOnly? StartTime { get; set; }            // for partial-day requests
    public TimeOnly? EndTime { get; set; }
    public decimal TotalDays { get; set; }              // calculated (can be 0.5 for half-day)
    public string? Reason { get; set; }
    public string? AttachmentUrl { get; set; }
    public TimeOffRequestStatus Status { get; set; }    // Pending, Approved, Denied, Cancelled
    public string? ReviewedByUserId { get; set; }
    public DateTime? ReviewedAt { get; set; }
    public string? DenialReason { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation
    public Employee Employee { get; set; }
    public LeaveType LeaveType { get; set; }
}
```

#### LeaveBalance

```csharp
public class LeaveBalance
{
    public Guid Id { get; set; }
    public Guid EmployeeId { get; set; }
    public Guid LeaveTypeId { get; set; }
    public int Year { get; set; }
    public decimal Entitled { get; set; }               // total days entitled this year
    public decimal CarriedOver { get; set; }            // days carried from previous year
    public decimal Used { get; set; }                   // days used (approved time-off)
    public decimal Pending { get; set; }                // days in pending requests
    public decimal Remaining => Entitled + CarriedOver - Used - Pending;
    public DateTime UpdatedAt { get; set; }

    // Navigation
    public Employee Employee { get; set; }
    public LeaveType LeaveType { get; set; }
}
```

#### ShiftSwapRequest

```csharp
public class ShiftSwapRequest
{
    public Guid Id { get; set; }
    public Guid RequestingEmployeeId { get; set; }
    public Guid TargetEmployeeId { get; set; }
    public Guid RequestingShiftAssignmentId { get; set; }
    public Guid? TargetShiftAssignmentId { get; set; }  // null for cover requests (one-way)
    public SwapType Type { get; set; }                  // Swap, Cover, Drop
    public SwapRequestStatus Status { get; set; }       // Pending, TargetAccepted, ManagerApproved, Denied, Cancelled
    public string? Note { get; set; }
    public string? ManagerNote { get; set; }
    public string? ReviewedByUserId { get; set; }
    public DateTime? ReviewedAt { get; set; }
    public DateTime CreatedAt { get; set; }

    // Navigation
    public Employee RequestingEmployee { get; set; }
    public Employee TargetEmployee { get; set; }
    public ShiftAssignment RequestingShiftAssignment { get; set; }
    public ShiftAssignment? TargetShiftAssignment { get; set; }
}
```

#### OpenShift

```csharp
public class OpenShift
{
    public Guid Id { get; set; }
    public Guid LocationId { get; set; }
    public Guid? StationId { get; set; }
    public Guid? RoleId { get; set; }
    public DateOnly Date { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public int SlotsAvailable { get; set; }
    public int SlotsFilled { get; set; }
    public bool RequiresApproval { get; set; }
    public string? Notes { get; set; }
    public OpenShiftStatus Status { get; set; }         // Open, Filled, Cancelled, Expired
    public DateTime CreatedAt { get; set; }

    // Navigation
    public Location Location { get; set; }
    public Station? Station { get; set; }
    public Role? Role { get; set; }
    public ICollection<OpenShiftClaim> Claims { get; set; }
}
```

#### OpenShiftClaim

```csharp
public class OpenShiftClaim
{
    public Guid Id { get; set; }
    public Guid OpenShiftId { get; set; }
    public Guid EmployeeId { get; set; }
    public ClaimStatus Status { get; set; }             // Pending, Approved, Denied
    public DateTime ClaimedAt { get; set; }
    public string? ReviewedByUserId { get; set; }
    public DateTime? ReviewedAt { get; set; }

    // Navigation
    public OpenShift OpenShift { get; set; }
    public Employee Employee { get; set; }
}
```

#### TimesheetPeriod

```csharp
public class TimesheetPeriod
{
    public Guid Id { get; set; }
    public Guid LocationId { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public TimesheetPeriodType Type { get; set; }       // Weekly, BiWeekly, Monthly
    public TimesheetPeriodStatus Status { get; set; }   // Open, Closed, Locked
    public DateTime CreatedAt { get; set; }

    // Navigation
    public Location Location { get; set; }
    public ICollection<TimesheetEntry> Entries { get; set; }
}
```

#### TimesheetEntry

```csharp
public class TimesheetEntry
{
    public Guid Id { get; set; }
    public Guid TimesheetPeriodId { get; set; }
    public Guid EmployeeId { get; set; }
    public Guid? ShiftAssignmentId { get; set; }
    public DateOnly Date { get; set; }
    public decimal ScheduledHours { get; set; }
    public decimal ActualHours { get; set; }
    public decimal RegularHours { get; set; }
    public decimal OvertimeHours { get; set; }
    public decimal NightHours { get; set; }
    public decimal WeekendHours { get; set; }
    public decimal HolidayHours { get; set; }
    public decimal BreakHours { get; set; }
    public decimal NetPayableHours { get; set; }
    public TimesheetEntryStatus Status { get; set; }    // AutoGenerated, EmployeeSubmitted, ManagerApproved, Disputed, Locked
    public string? EmployeeNote { get; set; }
    public string? ManagerNote { get; set; }
    public string? ApprovedByUserId { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation
    public TimesheetPeriod TimesheetPeriod { get; set; }
    public Employee Employee { get; set; }
    public ShiftAssignment? ShiftAssignment { get; set; }
}
```

#### StaffingRequirement

```csharp
public class StaffingRequirement
{
    public Guid Id { get; set; }
    public Guid LocationId { get; set; }
    public Guid? StationId { get; set; }
    public Guid RoleId { get; set; }
    public Guid ShiftTemplateId { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
    public int MinStaff { get; set; }
    public int MaxStaff { get; set; }
    public DateTime CreatedAt { get; set; }

    // Navigation
    public Location Location { get; set; }
    public Station? Station { get; set; }
    public Role Role { get; set; }
    public ShiftTemplate ShiftTemplate { get; set; }
}
```

#### SchedulingRule

```csharp
public class SchedulingRule
{
    public Guid Id { get; set; }
    public Guid OrganizationId { get; set; }
    public string RuleName { get; set; }                // "MinRestBetweenShifts"
    public string RuleType { get; set; }                // "MinRestHours", "MaxConsecutiveDays", "MaxHoursPerWeek", etc.
    public decimal Value { get; set; }                  // 11 (hours), 6 (days), 48 (hours)
    public bool IsHardConstraint { get; set; }          // true = error (block), false = warning (override)
    public ContractType? ApplicableContractType { get; set; } // null = all contract types
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }

    // Navigation
    public Organization Organization { get; set; }
}
```

#### BlackoutDate

```csharp
public class BlackoutDate
{
    public Guid Id { get; set; }
    public Guid LocationId { get; set; }
    public DateOnly Date { get; set; }
    public string Reason { get; set; }                  // "Christmas Eve", "Valentine's Day"
    public bool IsRecurringYearly { get; set; }
    public DateTime CreatedAt { get; set; }

    // Navigation
    public Location Location { get; set; }
}
```

#### PosConnection

```csharp
public class PosConnection
{
    public Guid Id { get; set; }
    public Guid OrganizationId { get; set; }
    public string PosName { get; set; }                 // "BonApp", "Lightspeed", "Toast"
    public string ApiKeyHash { get; set; }              // hashed API key
    public string? WebhookUrl { get; set; }             // where to send outbound events
    public string? WebhookSecret { get; set; }          // HMAC secret for outbound signatures
    public bool IsActive { get; set; }
    public DateTime? LastSyncAt { get; set; }
    public DateTime CreatedAt { get; set; }

    // Navigation
    public Organization Organization { get; set; }
}
```

#### Notification

```csharp
public class Notification
{
    public Guid Id { get; set; }
    public string RecipientUserId { get; set; }
    public string Type { get; set; }                    // "SchedulePublished", "ShiftAssigned", "TimeOffApproved", etc.
    public string Title { get; set; }
    public string Body { get; set; }
    public string? ActionUrl { get; set; }              // deep link in the app
    public string? EntityType { get; set; }             // "ShiftAssignment", "TimeOffRequest"
    public Guid? EntityId { get; set; }
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ReadAt { get; set; }
}
```

#### AuditLog

```csharp
public class AuditLog
{
    public Guid Id { get; set; }
    public Guid OrganizationId { get; set; }
    public string UserId { get; set; }
    public string UserName { get; set; }
    public string EntityType { get; set; }              // "Employee", "ShiftAssignment", "TimeOffRequest"
    public string EntityId { get; set; }
    public string Action { get; set; }                  // "Created", "Updated", "Deleted", "Approved", "Denied"
    public string? OldValues { get; set; }              // JSON snapshot of changed fields (before)
    public string? NewValues { get; set; }              // JSON snapshot of changed fields (after)
    public string? IpAddress { get; set; }
    public DateTime Timestamp { get; set; }
}
```

#### ChatMessage

```csharp
public class ChatMessage
{
    public Guid Id { get; set; }
    public Guid OrganizationId { get; set; }
    public Guid? ChannelId { get; set; }                // null = direct message
    public string SenderUserId { get; set; }
    public string? RecipientUserId { get; set; }        // for DMs only
    public string Content { get; set; }
    public string? AttachmentUrl { get; set; }
    public string? AttachmentType { get; set; }         // "image", "file"
    public Guid? ShiftAssignmentId { get; set; }        // shift-context messaging
    public bool IsAnnouncement { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? EditedAt { get; set; }
    public bool IsDeleted { get; set; }

    // Navigation
    public ChatChannel? Channel { get; set; }
}
```

#### ChatChannel

```csharp
public class ChatChannel
{
    public Guid Id { get; set; }
    public Guid OrganizationId { get; set; }
    public string Name { get; set; }                    // "Kitchen Team", "All Staff"
    public string? Description { get; set; }
    public ChannelType Type { get; set; }               // Open, Private, Department, Role
    public Guid? DepartmentId { get; set; }             // auto-channel for department
    public Guid? RoleId { get; set; }                   // auto-channel for role
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }

    // Navigation
    public ICollection<ChatChannelMember> Members { get; set; }
    public ICollection<ChatMessage> Messages { get; set; }
}
```

#### ChatMessageReadReceipt

```csharp
public class ChatMessageReadReceipt
{
    public Guid MessageId { get; set; }
    public string UserId { get; set; }
    public DateTime ReadAt { get; set; }
}
```

#### ManagerLogEntry

```csharp
public class ManagerLogEntry
{
    public Guid Id { get; set; }
    public Guid LocationId { get; set; }
    public string AuthorUserId { get; set; }
    public DateOnly Date { get; set; }
    public string ShiftName { get; set; }               // "Morning", "Evening"
    public string Category { get; set; }                // "Operations", "Staff", "Maintenance", "Safety", "CustomerFeedback", "Inventory"
    public string Content { get; set; }
    public decimal? SalesAmount { get; set; }           // auto from POS or manual
    public decimal? LaborCostPercent { get; set; }
    public int? CoversServed { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation
    public Location Location { get; set; }
}
```

#### TaskTemplate

```csharp
public class TaskTemplate
{
    public Guid Id { get; set; }
    public Guid OrganizationId { get; set; }
    public string Name { get; set; }                    // "Opening Checklist", "Closing Checklist"
    public string? Description { get; set; }
    public Guid? RoleId { get; set; }                   // optional: auto-assign to this role
    public Guid? ShiftTemplateId { get; set; }          // optional: auto-assign to this shift
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }

    // Navigation
    public ICollection<TaskTemplateItem> Items { get; set; }
}
```

#### TaskTemplateItem

```csharp
public class TaskTemplateItem
{
    public Guid Id { get; set; }
    public Guid TaskTemplateId { get; set; }
    public string Name { get; set; }                    // "Check walk-in cooler temperature"
    public string? Description { get; set; }
    public int SortOrder { get; set; }
    public bool RequiresProof { get; set; }
    public ProofType? ProofType { get; set; }           // Photo, Temperature, NumericValue, TextNote
    public int? EstimatedMinutes { get; set; }

    // Navigation
    public TaskTemplate TaskTemplate { get; set; }
}
```

#### ShiftTaskInstance

```csharp
public class ShiftTaskInstance
{
    public Guid Id { get; set; }
    public Guid TaskTemplateItemId { get; set; }
    public Guid? ShiftAssignmentId { get; set; }
    public Guid? AssignedEmployeeId { get; set; }
    public Guid LocationId { get; set; }
    public DateOnly Date { get; set; }
    public TaskInstanceStatus Status { get; set; }      // Pending, InProgress, Completed, Overdue, Skipped
    public DateTime? CompletedAt { get; set; }
    public string? CompletedByUserId { get; set; }
    public string? ProofValue { get; set; }             // temperature reading, cash count, note
    public string? ProofPhotoUrl { get; set; }          // photo URL
    public DateTime CreatedAt { get; set; }

    // Navigation
    public TaskTemplateItem TaskTemplateItem { get; set; }
    public Employee? AssignedEmployee { get; set; }
}
```

#### EngagementSurveyResponse

```csharp
public class EngagementSurveyResponse
{
    public Guid Id { get; set; }
    public Guid EmployeeId { get; set; }
    public Guid? ShiftAssignmentId { get; set; }
    public DateOnly Date { get; set; }
    public int Rating { get; set; }                     // 1-5 stars
    public string? Comment { get; set; }
    public bool IsAnonymous { get; set; }
    public DateTime CreatedAt { get; set; }

    // Navigation
    public Employee Employee { get; set; }
}
```

#### EmployeeRecognition

```csharp
public class EmployeeRecognition
{
    public Guid Id { get; set; }
    public Guid OrganizationId { get; set; }
    public string GiverUserId { get; set; }
    public Guid RecipientEmployeeId { get; set; }
    public string Category { get; set; }                // "Teamwork", "CustomerService", "AboveAndBeyond", "Reliability", "Leadership"
    public string Message { get; set; }
    public bool IsPublic { get; set; }
    public DateTime CreatedAt { get; set; }

    // Navigation
    public Employee RecipientEmployee { get; set; }
}
```

#### DemandForecast

```csharp
public class DemandForecast
{
    public Guid Id { get; set; }
    public Guid LocationId { get; set; }
    public DateOnly Date { get; set; }
    public int Hour { get; set; }                       // 0-23
    public decimal ForecastedSales { get; set; }
    public int ForecastedCovers { get; set; }
    public decimal? ActualSales { get; set; }           // filled after the fact
    public int? ActualCovers { get; set; }
    public string? WeatherCondition { get; set; }       // "sunny", "rainy", "snow"
    public decimal? WeatherTemperature { get; set; }
    public string? SpecialEvent { get; set; }           // "Valentine's Day", "Football match"
    public DateTime GeneratedAt { get; set; }

    // Navigation
    public Location Location { get; set; }
}
```

#### StaffingRecommendation

```csharp
public class StaffingRecommendation
{
    public Guid Id { get; set; }
    public Guid LocationId { get; set; }
    public Guid RoleId { get; set; }
    public Guid ShiftTemplateId { get; set; }
    public DateOnly Date { get; set; }
    public int RecommendedStaff { get; set; }
    public decimal RecommendedHours { get; set; }
    public decimal ForecastedSales { get; set; }
    public decimal ProjectedSPLH { get; set; }          // Sales Per Labor Hour
    public DateTime GeneratedAt { get; set; }

    // Navigation
    public Location Location { get; set; }
    public Role Role { get; set; }
    public ShiftTemplate ShiftTemplate { get; set; }
}
```

#### GeofenceZone

```csharp
public class GeofenceZone
{
    public Guid Id { get; set; }
    public Guid LocationId { get; set; }
    public string Name { get; set; }                    // "Main Restaurant", "Parking Lot Entrance"
    public double CenterLatitude { get; set; }
    public double CenterLongitude { get; set; }
    public int RadiusMeters { get; set; }               // default 100, range 50-500
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }

    // Navigation
    public Location Location { get; set; }
}
```

#### TipPool

```csharp
public class TipPool
{
    public Guid Id { get; set; }
    public Guid OrganizationId { get; set; }
    public string Name { get; set; }                    // "Dinner Service Pool", "Bar Pool"
    public TipDistributionMethod Method { get; set; }   // ByHoursWorked, EqualSplit, CustomPercentage
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }

    // Navigation
    public ICollection<TipPoolRole> Roles { get; set; }
    public ICollection<TipEntry> TipEntries { get; set; }
}
```

#### TipPoolRole

```csharp
public class TipPoolRole
{
    public Guid TipPoolId { get; set; }
    public Guid RoleId { get; set; }
    public decimal SharePercentage { get; set; }        // e.g., 60% for waiters, 25% for kitchen, 15% for bar

    // Navigation
    public TipPool TipPool { get; set; }
    public Role Role { get; set; }
}
```

#### TipEntry

```csharp
public class TipEntry
{
    public Guid Id { get; set; }
    public Guid TipPoolId { get; set; }
    public Guid LocationId { get; set; }
    public DateOnly Date { get; set; }
    public string ShiftName { get; set; }
    public decimal TotalTipAmount { get; set; }
    public string EnteredByUserId { get; set; }
    public TipEntrySource Source { get; set; }          // Manual, PosSync
    public bool IsFinalized { get; set; }
    public DateTime CreatedAt { get; set; }

    // Navigation
    public TipPool TipPool { get; set; }
    public ICollection<TipDistribution> Distributions { get; set; }
}
```

#### TipDistribution

```csharp
public class TipDistribution
{
    public Guid Id { get; set; }
    public Guid TipEntryId { get; set; }
    public Guid EmployeeId { get; set; }
    public decimal Amount { get; set; }
    public decimal HoursWorked { get; set; }
    public decimal SharePercentage { get; set; }

    // Navigation
    public TipEntry TipEntry { get; set; }
    public Employee Employee { get; set; }
}
```

#### ScheduleChangePremium

```csharp
public class ScheduleChangePremium
{
    public Guid Id { get; set; }
    public Guid ShiftAssignmentId { get; set; }
    public Guid EmployeeId { get; set; }
    public string ChangeType { get; set; }              // "Added", "Removed", "TimeChanged"
    public int NoticeDaysGiven { get; set; }
    public decimal PremiumAmountCents { get; set; }     // penalty owed
    public string Jurisdiction { get; set; }            // "NYC", "Chicago", "Oregon"
    public bool EmployeeAccepted { get; set; }          // right-to-refuse tracking
    public DateTime ChangedAt { get; set; }
    public string ChangedByUserId { get; set; }

    // Navigation
    public ShiftAssignment ShiftAssignment { get; set; }
    public Employee Employee { get; set; }
}
```

### 3.3 Additional Enums (New Modules)

```csharp
public enum ChannelType { Open, Private, Department, Role }
public enum ProofType { Photo, Temperature, NumericValue, TextNote }
public enum TaskInstanceStatus { Pending, InProgress, Completed, Overdue, Skipped }
public enum TipDistributionMethod { ByHoursWorked, EqualSplit, CustomPercentage }
public enum TipEntrySource { Manual, PosSync }

// --- Hiring & ATS ---
public enum JobPostingStatus { Draft, Active, Paused, Closed, Filled }
public enum ApplicationStage { Applied, Screening, PhoneScreen, Interview, TrialShift, Offer, Hired, Rejected, Withdrawn }
public enum InterviewType { InPerson, Video, Phone }
public enum TrialShiftStatus { Scheduled, Completed, NoShow, Cancelled }
public enum TrialShiftResult { Passed, Failed, Undecided }

// --- Training & LMS ---
public enum CourseStatus { Draft, Published, Archived }
public enum TrainingAssignmentStatus { NotStarted, InProgress, Completed, Failed, Overdue }
public enum QuizQuestionType { MultipleChoice, TrueFalse }
public enum ArticleCategory { FoodSafety, CustomerService, EquipmentGuides, HRPolicies, MenuKnowledge, AllergenInfo, Custom }

// --- Digital Forms ---
public enum FormFieldType { TextInput, NumberInput, DatePicker, Dropdown, MultiSelect, RadioButtons, PhotoCapture, FileUpload, SignaturePad, GpsLocation, TemperatureReading, YesNoToggle, Rating, SectionHeader, InstructionalText }
public enum FormCategory { FoodSafety, HealthAndSafety, Operations, HR, Maintenance, Custom }
public enum FormScheduleType { OneTime, Daily, Weekly, Monthly, EventTriggered }

// --- Newsfeed ---
public enum FeedPostType { General, Recognition, Milestone, Welcome, Promotion }
public enum ReactionType { Like, Love, Celebrate, Funny, Insightful }
public enum PostTargetType { All, Department, Location, Role, Custom }

// --- Performance Reviews ---
public enum ReviewPeriodType { ThirtyDay, NinetyDay, SixMonth, Annual, Custom }
public enum ReviewStatus { Scheduled, SelfAssessmentPending, ManagerReviewPending, MeetingScheduled, Completed, Signed }
public enum RatingScale { ExceedsExpectations, MeetsExpectations, BelowExpectations, NeedsImprovement, Unsatisfactory }
public enum GoalStatus { NotStarted, InProgress, Completed, Missed, Deferred }

// --- Auto-Scheduling ---
public enum AutoScheduleStrategy { CostOptimized, EmployeePreferred, Balanced, DemandDriven }
public enum ShiftBidStatus { Pending, Accepted, Rejected }
```

### 3.4 Enums

```csharp
public enum EmployeeStatus { Active, OnLeave, Suspended, Terminated }
public enum ContractType { FullTime, PartTime, Hourly, Internship, Seasonal }
public enum ProficiencyLevel { Trainee, Junior, Senior, Lead }
public enum AvailabilityType { Available, Preferred, Unavailable }
public enum ScheduleStatus { Draft, Published, Locked }
public enum ShiftAssignmentStatus { Scheduled, Confirmed, Completed, NoShow, Cancelled }
public enum ClockEntryType { ShiftStart, BreakStart, BreakEnd, ShiftEnd }
public enum ClockSource { WebApp, PosSync, ManagerManual, KioskPin, KioskQr, KioskFacial }
public enum TimeOffRequestStatus { Pending, Approved, Denied, Cancelled }
public enum SwapType { Swap, Cover, Drop }
public enum SwapRequestStatus { Pending, TargetAccepted, ManagerApproved, Denied, Cancelled }
public enum OpenShiftStatus { Open, Filled, Cancelled, Expired }
public enum ClaimStatus { Pending, Approved, Denied }
public enum TimesheetPeriodType { Weekly, BiWeekly, Monthly }
public enum TimesheetPeriodStatus { Open, Closed, Locked }
public enum TimesheetEntryStatus { AutoGenerated, EmployeeSubmitted, ManagerApproved, Disputed, Locked }
```

### 3.5 New Entity Definitions (Hiring, Training, Forms, Feed, Reviews, Auto-Scheduling)

#### JobPosting (ATS)

```csharp
public class JobPosting
{
    public Guid Id { get; set; }
    public Guid OrganizationId { get; set; }
    public Guid LocationId { get; set; }
    public Guid RoleId { get; set; }
    public string Title { get; set; }                     // "Experienced Waiter — La Dolce Vita"
    public string Description { get; set; }               // Rich text job description
    public string? Requirements { get; set; }             // Certifications, experience, languages
    public ContractType EmploymentType { get; set; }      // FullTime, PartTime, Seasonal
    public decimal? PayRangeMin { get; set; }             // Stored in cents
    public decimal? PayRangeMax { get; set; }
    public string? ShiftExpectations { get; set; }        // "Must be available weekends"
    public JobPostingStatus Status { get; set; }
    public DateTime? PublishedAt { get; set; }
    public DateTime? ClosedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; }

    // Navigation
    public Organization Organization { get; set; }
    public Location Location { get; set; }
    public Role Role { get; set; }
    public ICollection<JobApplication> Applications { get; set; }
}
```

#### JobApplication (ATS Pipeline)

```csharp
public class JobApplication
{
    public Guid Id { get; set; }
    public Guid JobPostingId { get; set; }
    public string CandidateName { get; set; }
    public string CandidateEmail { get; set; }
    public string? CandidatePhone { get; set; }
    public string? ResumeUrl { get; set; }                // Azure Blob Storage
    public string? CoverLetterUrl { get; set; }
    public string? Source { get; set; }                   // "Indeed", "Glassdoor", "Referral", "Direct"
    public Guid? ReferredByEmployeeId { get; set; }       // Referral tracking
    public ApplicationStage CurrentStage { get; set; }
    public int? AiScreeningScore { get; set; }            // 0-100 AI score (nullable if disabled)
    public string? AiScreeningNotes { get; set; }
    public DateTime AppliedAt { get; set; }
    public Guid? ConvertedEmployeeId { get; set; }        // Set when hired and converted

    // Navigation
    public JobPosting JobPosting { get; set; }
    public Employee? ReferredByEmployee { get; set; }
    public Employee? ConvertedEmployee { get; set; }
    public ICollection<ApplicationStageHistory> StageHistory { get; set; }
    public ICollection<InterviewSlot> Interviews { get; set; }
    public TrialShift? TrialShift { get; set; }
}
```

#### TrainingCourse (LMS)

```csharp
public class TrainingCourse
{
    public Guid Id { get; set; }
    public Guid OrganizationId { get; set; }
    public string Title { get; set; }                     // "Food Safety Level 2"
    public string? Description { get; set; }
    public CourseStatus Status { get; set; }
    public bool IsMandatory { get; set; }                 // Auto-assigned to new hires in target roles
    public int EstimatedMinutes { get; set; }             // Expected completion time
    public Guid? LinkedCertificationId { get; set; }      // Auto-updates cert on completion
    public int? CertValidityDays { get; set; }            // How long cert is valid after completion
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; }

    // Navigation
    public Organization Organization { get; set; }
    public ICollection<TrainingModule> Modules { get; set; }
    public ICollection<TrainingAssignment> Assignments { get; set; }
    public ICollection<Role> TargetRoles { get; set; }    // Many-to-many: which roles need this course
}
```

#### TrainingModule (LMS Section)

```csharp
public class TrainingModule
{
    public Guid Id { get; set; }
    public Guid TrainingCourseId { get; set; }
    public string Title { get; set; }                     // "Module 1: Temperature Control"
    public string? ContentHtml { get; set; }              // Rich text / Markdown content
    public string? VideoUrl { get; set; }                 // YouTube/Vimeo or uploaded MP4
    public string? DocumentUrl { get; set; }              // Downloadable PDF
    public int SortOrder { get; set; }
    public bool HasQuiz { get; set; }
    public int? QuizPassingScore { get; set; }            // Percentage (e.g., 80)
    public int? QuizMaxAttempts { get; set; }
    public int? QuizTimeLimitMinutes { get; set; }

    // Navigation
    public TrainingCourse Course { get; set; }
    public ICollection<QuizQuestion> QuizQuestions { get; set; }
}
```

#### FormTemplate (Digital Forms)

```csharp
public class FormTemplate
{
    public Guid Id { get; set; }
    public Guid OrganizationId { get; set; }
    public string Name { get; set; }                      // "Daily Temperature Log"
    public string? Description { get; set; }
    public FormCategory Category { get; set; }
    public bool IsActive { get; set; }
    public FormScheduleType ScheduleType { get; set; }
    public string? ScheduleCron { get; set; }             // For recurring: cron expression
    public bool RequiresGpsLocation { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; }

    // Navigation
    public Organization Organization { get; set; }
    public ICollection<FormField> Fields { get; set; }
    public ICollection<FormSubmission> Submissions { get; set; }
    public ICollection<Role> AssignedRoles { get; set; }  // Which roles fill out this form
}
```

#### FormField (Form Builder Fields)

```csharp
public class FormField
{
    public Guid Id { get; set; }
    public Guid FormTemplateId { get; set; }
    public FormFieldType FieldType { get; set; }
    public string Label { get; set; }                     // "Walk-in Cooler Temperature"
    public string? Placeholder { get; set; }
    public string? HelpText { get; set; }
    public bool IsRequired { get; set; }
    public int SortOrder { get; set; }
    public string? OptionsJson { get; set; }              // For dropdown/radio: ["Option A", "Option B"]
    public string? ValidationJson { get; set; }           // {"min": 0, "max": 10, "alertAbove": 5}
    public string? ConditionalLogicJson { get; set; }     // {"showWhen": {"fieldId": "...", "equals": "Yes"}}
    public decimal? AlertThresholdMin { get; set; }       // Trigger alert if value below
    public decimal? AlertThresholdMax { get; set; }       // Trigger alert if value above

    // Navigation
    public FormTemplate FormTemplate { get; set; }
}
```

#### FeedPost (Company Newsfeed)

```csharp
public class FeedPost
{
    public Guid Id { get; set; }
    public Guid OrganizationId { get; set; }
    public Guid AuthorId { get; set; }                    // Employee who posted
    public FeedPostType PostType { get; set; }
    public string Content { get; set; }                   // Text content (Markdown supported)
    public string? ImageUrls { get; set; }                // JSON array of image URLs
    public string? AttachmentUrl { get; set; }
    public PostTargetType TargetType { get; set; }
    public Guid? TargetId { get; set; }                   // Department/Location/Role ID (null = All)
    public bool IsPinned { get; set; }
    public bool CommentsEnabled { get; set; }
    public bool RequiresApproval { get; set; }
    public bool IsApproved { get; set; }
    public Guid? LinkedRecognitionId { get; set; }        // If auto-generated from recognition
    public DateTime CreatedAt { get; set; }

    // Navigation
    public Organization Organization { get; set; }
    public Employee Author { get; set; }
    public ICollection<FeedComment> Comments { get; set; }
    public ICollection<FeedReaction> Reactions { get; set; }
}
```

#### ReviewTemplate (Performance Reviews)

```csharp
public class ReviewTemplate
{
    public Guid Id { get; set; }
    public Guid OrganizationId { get; set; }
    public string Name { get; set; }                      // "90-Day Probation Review"
    public ReviewPeriodType PeriodType { get; set; }
    public bool IncludeSelfAssessment { get; set; }
    public bool IncludeGoalSetting { get; set; }
    public bool RequiresDigitalSignature { get; set; }
    public DateTime CreatedAt { get; set; }

    // Navigation
    public Organization Organization { get; set; }
    public ICollection<ReviewCriteria> Criteria { get; set; }
    public ICollection<Role> ApplicableRoles { get; set; }
}
```

#### PerformanceReview (Review Instance)

```csharp
public class PerformanceReview
{
    public Guid Id { get; set; }
    public Guid EmployeeId { get; set; }
    public Guid ReviewTemplateId { get; set; }
    public Guid ReviewerId { get; set; }                  // Manager conducting the review
    public ReviewStatus Status { get; set; }
    public DateTime ReviewPeriodStart { get; set; }
    public DateTime ReviewPeriodEnd { get; set; }
    public DateTime DueDate { get; set; }
    public RatingScale? OverallRating { get; set; }
    public string? ManagerComments { get; set; }
    public string? EmployeeSelfAssessmentComments { get; set; }
    // Auto-populated data-driven metrics:
    public decimal? AttendanceRate { get; set; }          // % shifts without late/no-show
    public decimal? AvgPunctualityMinutes { get; set; }   // +/- avg clock-in vs shift start
    public decimal? HoursWorkedVsContracted { get; set; } // Ratio
    public int? TaskCompletionRate { get; set; }          // % tasks completed on time
    public int? RecognitionsReceived { get; set; }        // Count of shout-outs received
    public decimal? EngagementSurveyAvg { get; set; }     // Average survey score
    public DateTime? EmployeeSignedAt { get; set; }
    public DateTime? ManagerSignedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public DateTime CreatedAt { get; set; }

    // Navigation
    public Employee Employee { get; set; }
    public ReviewTemplate ReviewTemplate { get; set; }
    public Employee Reviewer { get; set; }
    public ICollection<ReviewResponse> Responses { get; set; }
    public ICollection<PerformanceGoal> Goals { get; set; }
}
```

#### WeeklyScheduleTemplate (Auto-Scheduling)

```csharp
public class WeeklyScheduleTemplate
{
    public Guid Id { get; set; }
    public Guid OrganizationId { get; set; }
    public Guid LocationId { get; set; }
    public string Name { get; set; }                      // "Summer Weekday Pattern"
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; }

    // Navigation
    public Organization Organization { get; set; }
    public Location Location { get; set; }
    public ICollection<TemplateShiftSlot> ShiftSlots { get; set; }
}

public class TemplateShiftSlot
{
    public Guid Id { get; set; }
    public Guid WeeklyScheduleTemplateId { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
    public Guid RoleId { get; set; }
    public Guid StationId { get; set; }
    public Guid? ShiftTemplateId { get; set; }            // Optional: use shift template times
    public TimeSpan? CustomStartTime { get; set; }        // Override start
    public TimeSpan? CustomEndTime { get; set; }          // Override end
    public int HeadCount { get; set; }                    // How many people needed

    // Navigation
    public WeeklyScheduleTemplate WeeklyScheduleTemplate { get; set; }
    public Role Role { get; set; }
    public Station Station { get; set; }
    public ShiftTemplate? ShiftTemplate { get; set; }
}
```

#### ShiftBid & ShiftPreference (Employee Preferences)

```csharp
public class ShiftBid
{
    public Guid Id { get; set; }
    public Guid EmployeeId { get; set; }
    public DateOnly ShiftDate { get; set; }
    public Guid ShiftTemplateId { get; set; }
    public Guid? StationId { get; set; }
    public int Priority { get; set; }                     // 1 = top choice, 2 = second, etc.
    public ShiftBidStatus Status { get; set; }
    public DateTime SubmittedAt { get; set; }

    // Navigation
    public Employee Employee { get; set; }
    public ShiftTemplate ShiftTemplate { get; set; }
}

public class ShiftPreference
{
    public Guid Id { get; set; }
    public Guid EmployeeId { get; set; }
    public string? PreferredDaysJson { get; set; }        // ["Monday","Tuesday","Wednesday"]
    public string? PreferredTimesJson { get; set; }       // ["Morning","Evening"]
    public decimal? MinDesiredHoursPerWeek { get; set; }
    public decimal? MaxDesiredHoursPerWeek { get; set; }
    public bool PrefersConsecutiveDays { get; set; }
    public string? Notes { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation
    public Employee Employee { get; set; }
}
```

---

## 4. API Endpoints

### 4.1 Authentication & User Management

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| POST | `/api/auth/register` | Register new organization + admin user | Public |
| POST | `/api/auth/login` | Login with email + password, returns JWT | Public |
| POST | `/api/auth/refresh` | Refresh expired JWT using refresh token | Public |
| POST | `/api/auth/forgot-password` | Send password reset email | Public |
| POST | `/api/auth/reset-password` | Reset password with token | Public |
| GET | `/api/auth/me` | Get current user profile | Authenticated |
| PUT | `/api/auth/me` | Update current user profile | Authenticated |
| PUT | `/api/auth/me/password` | Change current user password | Authenticated |
| POST | `/api/auth/invite` | Invite a user to the organization (send email with link) | Admin |
| POST | `/api/auth/accept-invite/{token}` | Accept invitation and create account | Public |

### 4.2 Organization & Settings

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| GET | `/api/organization` | Get current organization details | Authenticated |
| PUT | `/api/organization` | Update organization settings | Admin |
| GET | `/api/organization/locations` | List all locations | Authenticated |
| POST | `/api/organization/locations` | Create location | Admin |
| PUT | `/api/organization/locations/{id}` | Update location | Admin |
| DELETE | `/api/organization/locations/{id}` | Deactivate location | Admin |
| GET | `/api/organization/departments` | List departments | Authenticated |
| POST | `/api/organization/departments` | Create department | Admin |
| PUT | `/api/organization/departments/{id}` | Update department | Admin |
| DELETE | `/api/organization/departments/{id}` | Delete department | Admin |

### 4.3 Roles & Stations

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| GET | `/api/roles` | List all roles | Authenticated |
| POST | `/api/roles` | Create role | Admin/Manager |
| PUT | `/api/roles/{id}` | Update role | Admin/Manager |
| DELETE | `/api/roles/{id}` | Deactivate role | Admin |
| GET | `/api/roles/{id}/permissions` | Get role permissions | Admin |
| PUT | `/api/roles/{id}/permissions` | Update role permissions | Admin |
| GET | `/api/stations` | List stations (optionally filtered by location) | Authenticated |
| POST | `/api/stations` | Create station | Admin/Manager |
| PUT | `/api/stations/{id}` | Update station | Admin/Manager |
| DELETE | `/api/stations/{id}` | Deactivate station | Admin |

### 4.4 Employees

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| GET | `/api/employees` | List employees (paginated, filterable by status, role, location) | Manager+ |
| GET | `/api/employees/{id}` | Get employee detail | Manager+ or Self |
| POST | `/api/employees` | Create employee | Manager+ |
| PUT | `/api/employees/{id}` | Update employee | Manager+ |
| DELETE | `/api/employees/{id}` | Soft-delete (terminate) employee | Admin |
| GET | `/api/employees/{id}/roles` | Get employee roles | Manager+ |
| PUT | `/api/employees/{id}/roles` | Update employee roles | Manager+ |
| GET | `/api/employees/{id}/skills` | Get employee skills/certifications | Manager+ |
| POST | `/api/employees/{id}/skills` | Add skill/certification | Manager+ |
| PUT | `/api/employees/{id}/skills/{skillId}` | Update skill | Manager+ |
| DELETE | `/api/employees/{id}/skills/{skillId}` | Remove skill | Manager+ |
| GET | `/api/employees/{id}/contracts` | Get contract history | Admin/HR |
| POST | `/api/employees/{id}/contracts` | Create new contract | Admin/HR |
| PUT | `/api/employees/{id}/contracts/{contractId}` | Update contract | Admin/HR |
| GET | `/api/employees/{id}/documents` | List documents | Admin/HR |
| POST | `/api/employees/{id}/documents` | Upload document | Admin/HR |
| DELETE | `/api/employees/{id}/documents/{docId}` | Delete document | Admin/HR |
| GET | `/api/employees/{id}/notes` | Get employee notes | Manager+ |
| POST | `/api/employees/{id}/notes` | Add note | Manager+ |
| GET | `/api/employees/{id}/onboarding` | Get onboarding checklist | Manager+ |
| PUT | `/api/employees/{id}/onboarding/{itemId}` | Update onboarding item status | Manager+ |

### 4.5 Availability

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| GET | `/api/employees/{id}/availability` | Get recurring availability rules | Self or Manager+ |
| PUT | `/api/employees/{id}/availability` | Update recurring availability (bulk replace) | Self |
| GET | `/api/employees/{id}/availability/overrides` | Get one-off overrides (date range filter) | Self or Manager+ |
| POST | `/api/employees/{id}/availability/overrides` | Create one-off override | Self |
| PUT | `/api/employees/{id}/availability/overrides/{overrideId}` | Update override | Self |
| DELETE | `/api/employees/{id}/availability/overrides/{overrideId}` | Delete override | Self |
| GET | `/api/availability/summary` | Get all employees' availability for a date range (for schedule builder) | Manager+ |

### 4.6 Scheduling

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| GET | `/api/schedules` | List schedule periods (filtered by location, date range) | Authenticated |
| POST | `/api/schedules` | Create new schedule period (Draft) | Manager+ |
| GET | `/api/schedules/{id}` | Get schedule period with all assignments | Authenticated |
| PUT | `/api/schedules/{id}` | Update schedule period metadata | Manager+ |
| DELETE | `/api/schedules/{id}` | Delete schedule period (only if Draft) | Manager+ |
| POST | `/api/schedules/{id}/publish` | Publish schedule (Draft → Published) | Manager+ |
| POST | `/api/schedules/{id}/lock` | Lock schedule (Published → Locked) | Admin |
| POST | `/api/schedules/{id}/copy-from/{sourceId}` | Copy assignments from another schedule period | Manager+ |
| GET | `/api/schedules/{id}/assignments` | List all shift assignments in period | Authenticated |
| POST | `/api/schedules/{id}/assignments` | Create shift assignment | Manager+ |
| PUT | `/api/schedules/{id}/assignments/{assignmentId}` | Update shift assignment | Manager+ |
| DELETE | `/api/schedules/{id}/assignments/{assignmentId}` | Delete shift assignment | Manager+ |
| POST | `/api/schedules/{id}/assignments/bulk` | Bulk create/update assignments | Manager+ |
| POST | `/api/schedules/{id}/validate` | Run conflict detection on entire schedule | Manager+ |
| GET | `/api/schedules/{id}/conflicts` | Get all conflicts for schedule period | Manager+ |
| GET | `/api/schedules/{id}/staffing-coverage` | Compare assigned vs. required staffing | Manager+ |
| GET | `/api/schedules/{id}/labor-cost` | Get projected labor cost for schedule | Manager+ |
| GET | `/api/my-schedule` | Get current user's upcoming shifts | Employee |

### 4.7 Shift Templates

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| GET | `/api/shift-templates` | List shift templates | Authenticated |
| POST | `/api/shift-templates` | Create template | Manager+ |
| PUT | `/api/shift-templates/{id}` | Update template | Manager+ |
| DELETE | `/api/shift-templates/{id}` | Deactivate template | Manager+ |

### 4.8 Staffing Requirements

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| GET | `/api/staffing-requirements` | List requirements (filtered by location, day) | Manager+ |
| POST | `/api/staffing-requirements` | Create requirement | Manager+ |
| PUT | `/api/staffing-requirements/{id}` | Update requirement | Manager+ |
| DELETE | `/api/staffing-requirements/{id}` | Delete requirement | Manager+ |
| POST | `/api/staffing-requirements/bulk` | Bulk create/update requirements | Manager+ |

### 4.9 Time-Off & Leave

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| GET | `/api/time-off/requests` | List time-off requests (filtered by status, employee, date) | Manager+ |
| GET | `/api/time-off/requests/{id}` | Get request detail | Self or Manager+ |
| POST | `/api/time-off/requests` | Submit time-off request | Employee |
| PUT | `/api/time-off/requests/{id}` | Update pending request | Self |
| DELETE | `/api/time-off/requests/{id}` | Cancel pending request | Self |
| POST | `/api/time-off/requests/{id}/approve` | Approve request | Manager+ |
| POST | `/api/time-off/requests/{id}/deny` | Deny request (with reason) | Manager+ |
| GET | `/api/time-off/requests/{id}/impact` | Preview staffing impact before approving | Manager+ |
| GET | `/api/time-off/calendar` | Team time-off calendar (all approved leaves for date range) | Authenticated |
| GET | `/api/time-off/balances/{employeeId}` | Get leave balances for employee | Self or Manager+ |
| GET | `/api/leave-types` | List leave types | Authenticated |
| POST | `/api/leave-types` | Create leave type | Admin |
| PUT | `/api/leave-types/{id}` | Update leave type | Admin |
| DELETE | `/api/leave-types/{id}` | Deactivate leave type | Admin |
| GET | `/api/blackout-dates` | List blackout dates | Authenticated |
| POST | `/api/blackout-dates` | Create blackout date | Manager+ |
| DELETE | `/api/blackout-dates/{id}` | Delete blackout date | Manager+ |

### 4.10 Shift Swaps & Open Shifts

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| GET | `/api/shift-swaps` | List swap/cover/drop requests | Manager+ |
| GET | `/api/shift-swaps/{id}` | Get swap request detail | Involved parties or Manager+ |
| POST | `/api/shift-swaps` | Create swap/cover/drop request | Employee |
| POST | `/api/shift-swaps/{id}/accept` | Target employee accepts swap | Target Employee |
| POST | `/api/shift-swaps/{id}/approve` | Manager approves swap | Manager+ |
| POST | `/api/shift-swaps/{id}/deny` | Manager denies swap | Manager+ |
| DELETE | `/api/shift-swaps/{id}` | Cancel swap request | Requesting Employee |
| GET | `/api/open-shifts` | List open shifts (filtered by location, date, role) | Employee |
| POST | `/api/open-shifts` | Create open shift | Manager+ |
| PUT | `/api/open-shifts/{id}` | Update open shift | Manager+ |
| DELETE | `/api/open-shifts/{id}` | Cancel open shift | Manager+ |
| POST | `/api/open-shifts/{id}/claim` | Claim open shift | Employee |
| POST | `/api/open-shifts/{id}/claims/{claimId}/approve` | Approve claim | Manager+ |
| POST | `/api/open-shifts/{id}/claims/{claimId}/deny` | Deny claim | Manager+ |

### 4.11 Clock-In/Out & Timesheets

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| POST | `/api/clock/in` | Clock in (shift start) | Employee |
| POST | `/api/clock/break-start` | Start break | Employee |
| POST | `/api/clock/break-end` | End break | Employee |
| POST | `/api/clock/out` | Clock out (shift end) | Employee |
| GET | `/api/clock/status` | Get current clock status for employee | Employee |
| GET | `/api/clock/entries` | List clock entries (filtered by employee, date) | Manager+ |
| PUT | `/api/clock/entries/{id}` | Manager edit clock entry (with reason) | Manager+ |
| GET | `/api/timesheets/periods` | List timesheet periods | Manager+ |
| POST | `/api/timesheets/periods` | Create/generate timesheet period | Manager+ |
| GET | `/api/timesheets/periods/{id}` | Get timesheet period with all entries | Manager+ |
| POST | `/api/timesheets/periods/{id}/close` | Close timesheet period | Manager+ |
| POST | `/api/timesheets/periods/{id}/lock` | Lock timesheet period (for payroll) | Admin |
| GET | `/api/timesheets/entries` | List timesheet entries (filtered by period, employee) | Manager+ or Self |
| PUT | `/api/timesheets/entries/{id}` | Update entry (adjust hours, add note) | Manager+ |
| POST | `/api/timesheets/entries/{id}/submit` | Employee submits timesheet entry | Employee |
| POST | `/api/timesheets/entries/{id}/approve` | Manager approves entry | Manager+ |
| POST | `/api/timesheets/entries/{id}/dispute` | Employee disputes entry | Employee |
| GET | `/api/my-timesheet` | Get current user's timesheet for current period | Employee |

### 4.12 Reports & Exports

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| GET | `/api/reports/payroll-summary` | Payroll summary for period (all employees) | Admin/HR |
| GET | `/api/reports/payroll-summary/{employeeId}` | Payroll summary for single employee | Admin/HR or Self |
| GET | `/api/reports/labor-cost` | Labor cost report (by department, location, date range) | Manager+ |
| GET | `/api/reports/scheduled-vs-actual` | Scheduled vs. actual hours comparison | Manager+ |
| GET | `/api/reports/overtime` | Overtime report (employees approaching/exceeding limits) | Manager+ |
| GET | `/api/reports/attendance` | Attendance report (late arrivals, no-shows, absences) | Manager+ |
| GET | `/api/reports/leave-usage` | Leave usage report by type and department | Manager+ |
| GET | `/api/reports/certification-expiry` | Upcoming certification expirations | Manager+ |
| GET | `/api/reports/employee-hours` | Employee hours summary (for any date range) | Manager+ |
| POST | `/api/exports/payroll-csv` | Export payroll CSV (configurable columns) | Admin/HR |
| POST | `/api/exports/schedule-pdf` | Export schedule as PDF | Manager+ |
| POST | `/api/exports/timesheet-csv` | Export timesheet data as CSV | Admin/HR |

### 4.13 Notifications

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| GET | `/api/notifications` | Get user's notifications (paginated) | Authenticated |
| GET | `/api/notifications/unread-count` | Get unread notification count | Authenticated |
| POST | `/api/notifications/{id}/read` | Mark notification as read | Authenticated |
| POST | `/api/notifications/read-all` | Mark all as read | Authenticated |
| GET | `/api/notifications/preferences` | Get notification preferences | Authenticated |
| PUT | `/api/notifications/preferences` | Update notification preferences | Authenticated |

### 4.14 POS Integration API

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| POST | `/api/pos/v1/clock` | POS sends clock-in/out event | API Key |
| POST | `/api/pos/v1/sales-summary` | POS sends sales data for labor ratio | API Key |
| GET | `/api/pos/v1/scheduled-staff` | POS queries who is scheduled today | API Key |
| GET | `/api/pos/v1/staff-roles` | POS queries staff roles | API Key |
| POST | `/api/pos/connections` | Register a POS connection | Admin |
| GET | `/api/pos/connections` | List POS connections | Admin |
| PUT | `/api/pos/connections/{id}` | Update POS connection | Admin |
| DELETE | `/api/pos/connections/{id}` | Deactivate POS connection | Admin |
| POST | `/api/pos/connections/{id}/regenerate-key` | Regenerate API key | Admin |

### 4.15 Audit

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| GET | `/api/audit-logs` | Query audit logs (filtered by entity, user, date) | Admin |
| GET | `/api/audit-logs/{entityType}/{entityId}` | Get audit history for specific entity | Admin |

### 4.16 Scheduling Rules & Constraints

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| GET | `/api/scheduling-rules` | List all scheduling rules | Manager+ |
| POST | `/api/scheduling-rules` | Create scheduling rule | Admin |
| PUT | `/api/scheduling-rules/{id}` | Update scheduling rule | Admin |
| DELETE | `/api/scheduling-rules/{id}` | Delete scheduling rule | Admin |

### 4.17 Team Communication

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| GET | `/api/chat/channels` | List channels user belongs to | Authenticated |
| POST | `/api/chat/channels` | Create channel | Manager+ |
| PUT | `/api/chat/channels/{id}` | Update channel | Manager+ |
| POST | `/api/chat/channels/{id}/members` | Add member to channel | Manager+ |
| DELETE | `/api/chat/channels/{id}/members/{userId}` | Remove member | Manager+ |
| GET | `/api/chat/channels/{id}/messages` | Get messages in channel (paginated, cursor-based) | Member |
| POST | `/api/chat/channels/{id}/messages` | Send message to channel | Member |
| GET | `/api/chat/direct/{userId}/messages` | Get direct messages with user | Authenticated |
| POST | `/api/chat/direct/{userId}/messages` | Send direct message | Authenticated |
| POST | `/api/chat/messages/{id}/read` | Mark message as read | Authenticated |
| GET | `/api/chat/messages/{id}/read-receipts` | Get read receipts for message | Sender or Manager+ |
| POST | `/api/announcements` | Create announcement (one-way broadcast) | Manager+ |
| GET | `/api/announcements` | List announcements | Authenticated |
| GET | `/api/announcements/{id}/read-status` | Who has read this announcement | Manager+ |
| GET | `/api/chat/unread-counts` | Get unread counts per channel/DM | Authenticated |

### 4.18 Manager Logbook

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| GET | `/api/logbook` | List log entries (filtered by date, location, category) | Manager+ |
| POST | `/api/logbook` | Create log entry | Manager+ |
| PUT | `/api/logbook/{id}` | Update log entry | Author or Admin |
| GET | `/api/logbook/{id}` | Get log entry detail | Manager+ |
| GET | `/api/logbook/search` | Full-text search across log entries | Manager+ |
| GET | `/api/logbook/summary/{date}` | Get daily summary (all entries + metrics) | Manager+ |

### 4.19 Task Management

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| GET | `/api/task-templates` | List task templates | Manager+ |
| POST | `/api/task-templates` | Create task template with items | Manager+ |
| PUT | `/api/task-templates/{id}` | Update task template | Manager+ |
| DELETE | `/api/task-templates/{id}` | Deactivate template | Manager+ |
| GET | `/api/tasks` | List task instances (filtered by date, status, employee) | Authenticated |
| GET | `/api/tasks/{id}` | Get task instance detail | Authenticated |
| POST | `/api/tasks/{id}/complete` | Mark task as completed (with optional proof) | Assigned Employee |
| POST | `/api/tasks/{id}/skip` | Skip task (with reason) | Assigned Employee |
| POST | `/api/tasks/{id}/proof` | Upload proof (photo, value) | Assigned Employee |
| GET | `/api/tasks/dashboard` | Task completion dashboard (real-time stats) | Manager+ |
| GET | `/api/tasks/reports` | Task completion reports (per employee, per task, trends) | Manager+ |

### 4.20 Employee Engagement

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| POST | `/api/engagement/surveys` | Submit post-shift survey response | Employee |
| GET | `/api/engagement/surveys` | Get survey results (filtered by date, employee, shift) | Manager+ |
| GET | `/api/engagement/surveys/trends` | Survey score trends over time | Manager+ |
| POST | `/api/engagement/recognition` | Send recognition to employee | Authenticated |
| GET | `/api/engagement/recognition` | List recognition feed | Authenticated |
| GET | `/api/engagement/recognition/{employeeId}` | Recognition received by employee | Self or Manager+ |
| GET | `/api/engagement/score/{employeeId}` | Get engagement score for employee | Manager+ |
| GET | `/api/engagement/flight-risk` | List employees flagged as flight risk | Manager+ |
| GET | `/api/engagement/milestones` | Upcoming employee milestones | Manager+ |

### 4.21 Demand Forecasting & Labor Optimization

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| GET | `/api/forecast/{locationId}` | Get demand forecast for date range | Manager+ |
| POST | `/api/forecast/{locationId}/generate` | Generate/refresh forecast | Manager+ |
| GET | `/api/forecast/{locationId}/recommendations` | Get staffing recommendations | Manager+ |
| GET | `/api/forecast/{locationId}/accuracy` | Forecast vs. actual accuracy report | Manager+ |
| GET | `/api/labor/splh` | Get SPLH metrics (filtered by location, date range) | Manager+ |
| GET | `/api/labor/cost-percentage` | Get labor cost % vs. revenue | Manager+ |
| POST | `/api/forecast/events` | Create special event (impacts forecast) | Manager+ |
| GET | `/api/forecast/events` | List special events | Manager+ |

### 4.22 Geofencing

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| GET | `/api/geofences` | List geofence zones | Manager+ |
| POST | `/api/geofences` | Create geofence zone | Admin |
| PUT | `/api/geofences/{id}` | Update geofence zone | Admin |
| DELETE | `/api/geofences/{id}` | Deactivate geofence zone | Admin |
| POST | `/api/clock/verify-location` | Check if coordinates are within geofence | Employee |
| GET | `/api/clock/location-map` | Get map of clock-in locations for date | Manager+ |
| GET | `/api/clock/fraud-alerts` | List anti-fraud alerts | Manager+ |

### 4.23 Tip Management

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| GET | `/api/tip-pools` | List tip pool configurations | Manager+ |
| POST | `/api/tip-pools` | Create tip pool | Admin |
| PUT | `/api/tip-pools/{id}` | Update tip pool rules | Admin |
| POST | `/api/tips` | Enter tip amount for shift | Manager+ |
| GET | `/api/tips` | List tip entries (filtered by date, pool) | Manager+ |
| GET | `/api/tips/{id}/preview` | Preview tip distribution before finalizing | Manager+ |
| POST | `/api/tips/{id}/finalize` | Finalize tip distribution | Manager+ |
| GET | `/api/tips/employee/{employeeId}` | Get tip history for employee | Self or Manager+ |
| GET | `/api/tips/reports` | Tip summary report | Manager+ |

### 4.24 Predictive Scheduling Compliance

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| GET | `/api/compliance/jurisdictions` | List available jurisdiction presets | Admin |
| PUT | `/api/compliance/jurisdiction` | Set active jurisdiction(s) | Admin |
| GET | `/api/compliance/premiums` | List schedule change premiums owed | Manager+ |
| GET | `/api/compliance/premiums/summary` | Summary of premiums by period | Admin |
| GET | `/api/compliance/report` | Generate compliance report for period | Admin |
| GET | `/api/compliance/schedule/{id}/check` | Check schedule for compliance violations | Manager+ |

### 4.25 Auto-Scheduling & Schedule Templates

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| POST | `/api/schedules/{periodId}/auto-schedule` | Generate auto-schedule for period (body: strategy, weights) | Manager+ |
| GET | `/api/schedules/{periodId}/auto-schedule/preview` | Preview auto-schedule results without saving | Manager+ |
| POST | `/api/schedules/{periodId}/auto-schedule/apply` | Apply auto-schedule results to draft | Manager+ |
| POST | `/api/schedules/{periodId}/auto-schedule/compare` | Generate multiple strategy variants for comparison | Manager+ |
| GET | `/api/schedule-templates` | List saved weekly schedule templates | Manager+ |
| POST | `/api/schedule-templates` | Save current schedule as a named template | Manager+ |
| GET | `/api/schedule-templates/{id}` | Get template details with shift slots | Manager+ |
| PUT | `/api/schedule-templates/{id}` | Update template | Manager+ |
| DELETE | `/api/schedule-templates/{id}` | Delete template | Manager+ |
| POST | `/api/schedule-templates/{id}/apply/{periodId}` | Apply template to a schedule period + auto-fill employees | Manager+ |
| GET | `/api/shift-bids` | List shift bids for upcoming period | Manager+ |
| POST | `/api/shift-bids` | Submit shift bid (employee) | Employee+ |
| PUT | `/api/shift-bids/{id}` | Update bid priority | Employee+ |
| DELETE | `/api/shift-bids/{id}` | Cancel bid | Employee+ |
| GET | `/api/employees/me/shift-preferences` | Get own shift preferences | Employee+ |
| PUT | `/api/employees/me/shift-preferences` | Update shift preferences | Employee+ |
| GET | `/api/employees/{id}/fairness-score` | Get employee fairness metrics (rolling period) | Manager+ |

### 4.26 Hiring & Applicant Tracking (ATS)

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| GET | `/api/hiring/postings` | List job postings (filterable by status, role, location) | Manager+ |
| POST | `/api/hiring/postings` | Create job posting | Manager+ |
| GET | `/api/hiring/postings/{id}` | Get posting details + application count | Manager+ |
| PUT | `/api/hiring/postings/{id}` | Update posting | Manager+ |
| PUT | `/api/hiring/postings/{id}/status` | Change posting status (activate/pause/close) | Manager+ |
| POST | `/api/hiring/postings/{id}/distribute` | Publish to external job boards (Indeed, etc.) | Manager+ |
| GET | `/api/hiring/applications` | List all applications (filterable by stage, posting, score) | Manager+ |
| GET | `/api/hiring/applications/{id}` | Get application detail + stage history | Manager+ |
| PUT | `/api/hiring/applications/{id}/stage` | Move application to next/specific stage | Manager+ |
| POST | `/api/hiring/applications/{id}/notes` | Add note to application | Manager+ |
| POST | `/api/hiring/applications/{id}/ai-screen` | Run AI screening on application | Manager+ |
| POST | `/api/hiring/applications/{id}/interview` | Schedule interview | Manager+ |
| PUT | `/api/hiring/interviews/{id}` | Update/reschedule interview | Manager+ |
| POST | `/api/hiring/applications/{id}/trial-shift` | Schedule trial shift | Manager+ |
| PUT | `/api/hiring/trial-shifts/{id}/evaluate` | Submit trial shift evaluation | Manager+ |
| POST | `/api/hiring/applications/{id}/offer` | Generate and send offer letter | Manager+ |
| POST | `/api/hiring/applications/{id}/convert` | Convert hired applicant to employee record | Manager+ |
| GET | `/api/hiring/analytics` | Hiring analytics (time-to-hire, source effectiveness, funnel) | Admin |
| POST | `/api/hiring/applications` | Submit application (public candidate endpoint) | Public |

### 4.27 Training & Knowledge Base (LMS)

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| GET | `/api/training/courses` | List all courses | Manager+ |
| POST | `/api/training/courses` | Create course | Manager+ |
| GET | `/api/training/courses/{id}` | Get course details + modules | Authenticated |
| PUT | `/api/training/courses/{id}` | Update course | Manager+ |
| PUT | `/api/training/courses/{id}/publish` | Publish course (make available) | Manager+ |
| POST | `/api/training/courses/{id}/modules` | Add module to course | Manager+ |
| PUT | `/api/training/modules/{id}` | Update module content | Manager+ |
| DELETE | `/api/training/modules/{id}` | Remove module from course | Manager+ |
| POST | `/api/training/modules/{id}/quiz-questions` | Add quiz question | Manager+ |
| PUT | `/api/training/quiz-questions/{id}` | Update quiz question | Manager+ |
| POST | `/api/training/courses/{id}/assign` | Assign course to employees/roles/departments | Manager+ |
| GET | `/api/training/assignments` | List all training assignments (filterable) | Manager+ |
| GET | `/api/training/assignments/me` | Get own training assignments | Employee+ |
| PUT | `/api/training/assignments/{id}/progress` | Update module progress (mark module complete) | Employee+ |
| POST | `/api/training/assignments/{id}/quiz-attempt` | Submit quiz answers | Employee+ |
| GET | `/api/training/analytics` | Training analytics (completion rates, quiz stats) | Manager+ |
| GET | `/api/knowledge-base/articles` | List knowledge base articles | Authenticated |
| POST | `/api/knowledge-base/articles` | Create article | Manager+ |
| GET | `/api/knowledge-base/articles/{id}` | Get article detail | Authenticated |
| PUT | `/api/knowledge-base/articles/{id}` | Update article | Manager+ |
| GET | `/api/knowledge-base/search` | Search articles by keyword | Authenticated |
| GET | `/api/handbook/sections` | List handbook sections | Authenticated |
| POST | `/api/handbook/sections` | Create/update handbook section | Admin |
| POST | `/api/handbook/sections/{id}/acknowledge` | Employee acknowledges reading section | Employee+ |
| GET | `/api/handbook/acknowledgements` | List acknowledgement status per employee | Manager+ |

### 4.28 Digital Forms

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| GET | `/api/forms/templates` | List form templates | Manager+ |
| POST | `/api/forms/templates` | Create form template | Manager+ |
| GET | `/api/forms/templates/{id}` | Get template with fields | Authenticated |
| PUT | `/api/forms/templates/{id}` | Update template | Manager+ |
| POST | `/api/forms/templates/{id}/fields` | Add field to template | Manager+ |
| PUT | `/api/forms/fields/{id}` | Update field | Manager+ |
| DELETE | `/api/forms/fields/{id}` | Remove field | Manager+ |
| PUT | `/api/forms/fields/reorder` | Reorder fields | Manager+ |
| GET | `/api/forms/submissions` | List submissions (filterable by template, date, employee) | Manager+ |
| POST | `/api/forms/templates/{id}/submit` | Submit completed form | Employee+ |
| GET | `/api/forms/submissions/{id}` | Get submission detail with responses | Manager+ |
| GET | `/api/forms/submissions/{id}/pdf` | Export submission as PDF | Manager+ |
| GET | `/api/forms/analytics/{templateId}` | Form analytics (completion rates, common responses, alerts) | Manager+ |
| GET | `/api/forms/my-pending` | List forms pending completion for current user | Employee+ |
| GET | `/api/forms/library` | List pre-built form templates (read-only) | Manager+ |
| POST | `/api/forms/library/{id}/clone` | Clone pre-built template to organization | Manager+ |

### 4.29 Company Newsfeed

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| GET | `/api/feed` | Get feed posts (paginated, filtered by target groups user belongs to) | Authenticated |
| POST | `/api/feed` | Create feed post | Manager+ (or All if open posting enabled) |
| GET | `/api/feed/{id}` | Get post detail with comments and reactions | Authenticated |
| PUT | `/api/feed/{id}` | Update post | Author or Manager+ |
| DELETE | `/api/feed/{id}` | Delete post | Author or Manager+ |
| PUT | `/api/feed/{id}/pin` | Pin/unpin post | Manager+ |
| POST | `/api/feed/{id}/reactions` | Add reaction to post | Authenticated |
| DELETE | `/api/feed/{id}/reactions` | Remove reaction | Authenticated |
| POST | `/api/feed/{id}/comments` | Add comment to post | Authenticated |
| DELETE | `/api/feed/comments/{id}` | Delete comment | Author or Manager+ |
| PUT | `/api/feed/{id}/approve` | Approve pending post (if moderation enabled) | Manager+ |

### 4.30 Performance Reviews

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| GET | `/api/reviews/templates` | List review templates | Manager+ |
| POST | `/api/reviews/templates` | Create review template | Admin |
| PUT | `/api/reviews/templates/{id}` | Update template + criteria | Admin |
| GET | `/api/reviews` | List all reviews (filterable by status, employee, period) | Manager+ |
| POST | `/api/reviews` | Create/schedule a review for an employee | Manager+ |
| GET | `/api/reviews/{id}` | Get review detail (with self-assessment + manager rating) | Manager+ or Self |
| PUT | `/api/reviews/{id}/self-assessment` | Submit employee self-assessment | Employee (Self) |
| PUT | `/api/reviews/{id}/manager-review` | Submit manager review + ratings | Manager+ |
| PUT | `/api/reviews/{id}/sign` | Digitally sign the review (employee or manager) | Authenticated |
| POST | `/api/reviews/{id}/goals` | Add goal for next review period | Manager+ |
| PUT | `/api/reviews/goals/{id}` | Update goal status | Manager+ or Self |
| GET | `/api/reviews/analytics` | Performance analytics (rating distribution, trends) | Admin |
| GET | `/api/reviews/due` | List upcoming/overdue reviews | Manager+ |
| GET | `/api/employees/me/reviews` | Get own review history | Employee+ |

### 4.31 Employee Self-Service

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| GET | `/api/employees/me/earnings/estimate` | Get projected earnings for current/next schedule period | Employee+ |
| GET | `/api/employees/me/earnings/history` | Get earnings history by pay period | Employee+ |
| GET | `/api/employees/me/hours-tracker` | Get live hours worked this period, overtime status | Employee+ |
| GET | `/api/employees/me/leave-balances` | Get all leave type balances with accrual schedule | Employee+ |
| GET | `/api/employees/me/documents` | List own accessible documents | Employee+ |
| POST | `/api/employees/me/documents` | Upload document (cert, ID) — pending HR review | Employee+ |
| PUT | `/api/employees/me/profile` | Update own profile (phone, address, emergency contact) | Employee+ |
| GET | `/api/employees/me/onboarding` | Get onboarding progress (steps, completion %) | Employee+ |
| PUT | `/api/employees/me/onboarding/{stepId}` | Complete onboarding step (upload doc, acknowledge policy) | Employee+ |

### 4.32 GPS Tracking & Intraday Dashboard

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| GET | `/api/dashboard/intraday` | Real-time intraday labor vs. sales data | Manager+ |
| GET | `/api/dashboard/intraday/hourly` | Hour-by-hour breakdown for today | Manager+ |
| GET | `/api/dashboard/earned-vs-actual` | Earned hours vs actual hours by period | Manager+ |
| GET | `/api/dashboard/daily-summary` | End-of-day labor summary | Manager+ |
| GET | `/api/gps/live-map` | Get current positions of clocked-in employees | Manager+ |
| GET | `/api/gps/trail/{employeeId}/{date}` | Get GPS route history for employee on date | Manager+ |
| GET | `/api/gps/mileage/{employeeId}` | Get mileage records for employee | Manager+ |
| GET | `/api/employees/me/gps-trail/{date}` | View own GPS trail for a date | Employee+ |

---

## 5. Solution Structure

```
StaffPro/
├── StaffPro.sln
├── docker-compose.yml
├── docker-compose.override.yml
├── .github/
│   └── workflows/
│       ├── ci.yml
│       └── cd.yml
│
├── src/
│   ├── StaffPro.Api/                          # Presentation Layer
│   │   ├── Controllers/
│   │   │   ├── AuthController.cs
│   │   │   ├── OrganizationController.cs
│   │   │   ├── EmployeesController.cs
│   │   │   ├── RolesController.cs
│   │   │   ├── StationsController.cs
│   │   │   ├── AvailabilityController.cs
│   │   │   ├── SchedulesController.cs
│   │   │   ├── ShiftTemplatesController.cs
│   │   │   ├── StaffingRequirementsController.cs
│   │   │   ├── TimeOffController.cs
│   │   │   ├── ShiftSwapsController.cs
│   │   │   ├── OpenShiftsController.cs
│   │   │   ├── ClockController.cs
│   │   │   ├── TimesheetsController.cs
│   │   │   ├── ReportsController.cs
│   │   │   ├── ExportsController.cs
│   │   │   ├── NotificationsController.cs
│   │   │   ├── PosIntegrationController.cs
│   │   │   ├── AuditLogsController.cs
│   │   │   ├── SchedulingRulesController.cs
│   │   │   ├── ChatController.cs                 # Team messaging + announcements
│   │   │   ├── AnnouncementsController.cs        # One-way broadcasts
│   │   │   ├── LogbookController.cs              # Manager shift logbook
│   │   │   ├── TasksController.cs                # Task management
│   │   │   ├── EngagementController.cs           # Surveys + recognition
│   │   │   ├── ForecastController.cs             # AI demand forecasting
│   │   │   ├── GeofencesController.cs            # Geofence zone management
│   │   │   ├── TipsController.cs                 # Tip management
│   │   │   ├── ComplianceController.cs           # Predictive scheduling compliance
│   │   │   ├── AutoScheduleController.cs         # Auto-scheduling + schedule templates
│   │   │   ├── HiringController.cs               # Hiring & ATS pipeline
│   │   │   ├── TrainingController.cs             # LMS courses + modules + quizzes
│   │   │   ├── KnowledgeBaseController.cs        # Knowledge base articles + handbook
│   │   │   ├── FormsController.cs                # Digital forms builder + submissions
│   │   │   ├── FeedController.cs                 # Company newsfeed + reactions + comments
│   │   │   ├── ReviewsController.cs              # Performance reviews + goals
│   │   │   ├── SelfServiceController.cs          # Employee self-service (earnings, onboarding)
│   │   │   └── GpsTrackingController.cs          # GPS live map + route history + mileage
│   │   ├── Middleware/
│   │   │   ├── ExceptionHandlingMiddleware.cs
│   │   │   ├── TenantResolutionMiddleware.cs   # resolve OrganizationId from JWT
│   │   │   ├── AuditLoggingMiddleware.cs
│   │   │   └── RequestLoggingMiddleware.cs
│   │   ├── Filters/
│   │   │   ├── ValidationFilter.cs
│   │   │   └── TenantAuthorizationFilter.cs
│   │   ├── Hubs/
│   │   │   ├── NotificationHub.cs              # SignalR hub for real-time notifications
│   │   │   └── ChatHub.cs                      # SignalR hub for real-time messaging
│   │   ├── Program.cs
│   │   ├── appsettings.json
│   │   └── appsettings.Development.json
│   │
│   ├── StaffPro.Application/                   # Application Layer (Business Logic Orchestration)
│   │   ├── Commands/                           # CQRS Write operations
│   │   │   ├── Employees/
│   │   │   │   ├── CreateEmployeeCommand.cs
│   │   │   │   ├── UpdateEmployeeCommand.cs
│   │   │   │   └── TerminateEmployeeCommand.cs
│   │   │   ├── Schedules/
│   │   │   │   ├── CreateShiftAssignmentCommand.cs
│   │   │   │   ├── PublishScheduleCommand.cs
│   │   │   │   └── CopyScheduleCommand.cs
│   │   │   ├── TimeOff/
│   │   │   ├── Clock/
│   │   │   ├── Timesheets/
│   │   │   ├── Chat/
│   │   │   ├── Tasks/
│   │   │   ├── Engagement/
│   │   │   ├── Tips/
│   │   │   ├── AutoSchedule/
│   │   │   │   ├── GenerateAutoScheduleCommand.cs
│   │   │   │   ├── ApplyAutoScheduleCommand.cs
│   │   │   │   ├── SaveScheduleTemplateCommand.cs
│   │   │   │   └── SubmitShiftBidCommand.cs
│   │   │   ├── Hiring/
│   │   │   │   ├── CreateJobPostingCommand.cs
│   │   │   │   ├── AdvanceApplicationStageCommand.cs
│   │   │   │   ├── ScheduleInterviewCommand.cs
│   │   │   │   ├── ConvertApplicantCommand.cs
│   │   │   │   └── SubmitTrialEvaluationCommand.cs
│   │   │   ├── Training/
│   │   │   │   ├── CreateCourseCommand.cs
│   │   │   │   ├── AssignCourseCommand.cs
│   │   │   │   ├── UpdateModuleProgressCommand.cs
│   │   │   │   └── SubmitQuizAttemptCommand.cs
│   │   │   ├── Forms/
│   │   │   │   ├── CreateFormTemplateCommand.cs
│   │   │   │   └── SubmitFormCommand.cs
│   │   │   ├── Feed/
│   │   │   │   ├── CreateFeedPostCommand.cs
│   │   │   │   ├── AddReactionCommand.cs
│   │   │   │   └── AddCommentCommand.cs
│   │   │   └── Reviews/
│   │   │       ├── CreateReviewCommand.cs
│   │   │       ├── SubmitSelfAssessmentCommand.cs
│   │   │       ├── SubmitManagerReviewCommand.cs
│   │   │       └── SignReviewCommand.cs
│   │   ├── Queries/                            # CQRS Read operations
│   │   │   ├── Employees/
│   │   │   ├── Schedules/
│   │   │   ├── TimeOff/
│   │   │   ├── Timesheets/
│   │   │   ├── Reports/
│   │   │   ├── Forecast/
│   │   │   ├── Chat/
│   │   │   ├── Hiring/
│   │   │   ├── Training/
│   │   │   ├── Forms/
│   │   │   ├── Feed/
│   │   │   └── Reviews/
│   │   ├── Behaviors/                          # MediatR pipeline behaviors
│   │   │   ├── ValidationBehavior.cs           # Auto-validate before handler
│   │   │   ├── LoggingBehavior.cs              # Log all commands/queries
│   │   │   ├── TransactionBehavior.cs          # Wrap commands in DB transaction
│   │   │   └── AuditBehavior.cs                # Auto-audit write operations
│   │   ├── Events/                             # Domain events (published via MediatR)
│   │   │   ├── ShiftAssignedEvent.cs
│   │   │   ├── SchedulePublishedEvent.cs
│   │   │   ├── TimeOffApprovedEvent.cs
│   │   │   ├── ClockEntryCreatedEvent.cs
│   │   │   ├── MessageSentEvent.cs
│   │   │   └── TaskCompletedEvent.cs
│   │   ├── Interfaces/
│   │   │   ├── IConflictDetectionService.cs
│   │   │   ├── INotificationService.cs
│   │   │   ├── IPayrollReportService.cs
│   │   │   ├── IExportService.cs
│   │   │   ├── IAuditService.cs
│   │   │   ├── IPosIntegrationService.cs
│   │   │   ├── IDemandForecastService.cs       # AI forecasting
│   │   │   ├── IGeofenceService.cs             # GPS verification
│   │   │   ├── ITipDistributionService.cs      # Tip calculation
│   │   │   ├── IComplianceService.cs           # Predictive scheduling compliance
│   │   │   ├── IAutoScheduleService.cs         # Constraint solver algorithm
│   │   │   ├── IHiringService.cs               # ATS pipeline + AI screening
│   │   │   ├── ITrainingService.cs             # Course management + quiz grading
│   │   │   ├── IFormService.cs                 # Form builder + submission processing
│   │   │   ├── IFeedService.cs                 # Newsfeed + moderation
│   │   │   ├── IReviewService.cs               # Performance review automation
│   │   │   ├── IEarningsService.cs             # Pay estimate calculation
│   │   │   ├── IGpsTrackingService.cs          # Route tracking + mileage
│   │   │   └── IIntradayDashboardService.cs    # Real-time labor vs. sales
│   │   ├── Services/
│   │   │   ├── ConflictDetectionService.cs     # THE key business logic service
│   │   │   ├── PayrollReportService.cs
│   │   │   ├── AutoScheduleService.cs          # Constraint-based schedule generation
│   │   │   ├── DemandForecastService.cs        # Sales-based forecasting engine
│   │   │   ├── GeofenceService.cs              # GPS boundary checking
│   │   │   ├── TipDistributionService.cs       # Pool calculation engine
│   │   │   ├── ComplianceService.cs            # Premium calculation + checks
│   │   │   ├── FlightRiskService.cs            # Engagement-based turnover prediction
│   │   │   └── PosIntegrationService.cs
│   │   ├── DTOs/
│   │   │   ├── Auth/
│   │   │   ├── Employees/
│   │   │   ├── Schedules/
│   │   │   ├── TimeOff/
│   │   │   ├── Timesheets/
│   │   │   ├── Reports/
│   │   │   ├── Notifications/
│   │   │   └── Pos/
│   │   ├── Validators/
│   │   │   ├── CreateEmployeeValidator.cs
│   │   │   ├── CreateShiftAssignmentValidator.cs
│   │   │   ├── TimeOffRequestValidator.cs
│   │   │   ├── ClockEntryValidator.cs
│   │   │   └── ... (FluentValidation for all DTOs)
│   │   ├── Mappings/
│   │   │   └── AutoMapperProfile.cs
│   │   └── Options/
│   │       ├── JwtOptions.cs
│   │       ├── ClockRulesOptions.cs
│   │       ├── OvertimeOptions.cs
│   │       └── NotificationOptions.cs
│   │
│   ├── StaffPro.Domain/                        # Domain Layer (Pure Business Model)
│   │   ├── Entities/
│   │   │   ├── Organization.cs
│   │   │   ├── Location.cs
│   │   │   ├── Department.cs
│   │   │   ├── Role.cs
│   │   │   ├── Station.cs
│   │   │   ├── StationRole.cs
│   │   │   ├── Employee.cs
│   │   │   ├── EmployeeRole.cs
│   │   │   ├── EmployeeSkill.cs
│   │   │   ├── EmployeeLocation.cs
│   │   │   ├── Contract.cs
│   │   │   ├── AvailabilityRule.cs
│   │   │   ├── AvailabilityOverride.cs
│   │   │   ├── ShiftTemplate.cs
│   │   │   ├── SchedulePeriod.cs
│   │   │   ├── ShiftAssignment.cs
│   │   │   ├── ClockEntry.cs
│   │   │   ├── LeaveType.cs
│   │   │   ├── TimeOffRequest.cs
│   │   │   ├── LeaveBalance.cs
│   │   │   ├── ShiftSwapRequest.cs
│   │   │   ├── OpenShift.cs
│   │   │   ├── OpenShiftClaim.cs
│   │   │   ├── TimesheetPeriod.cs
│   │   │   ├── TimesheetEntry.cs
│   │   │   ├── StaffingRequirement.cs
│   │   │   ├── SchedulingRule.cs
│   │   │   ├── BlackoutDate.cs
│   │   │   ├── PosConnection.cs
│   │   │   ├── Notification.cs
│   │   │   ├── NotificationPreference.cs
│   │   │   ├── EmployeeDocument.cs
│   │   │   ├── EmployeeNote.cs
│   │   │   ├── OnboardingItem.cs
│   │   │   └── AuditLog.cs
│   │   ├── Enums/
│   │   │   ├── EmployeeStatus.cs
│   │   │   ├── ContractType.cs
│   │   │   ├── ProficiencyLevel.cs
│   │   │   ├── AvailabilityType.cs
│   │   │   ├── ScheduleStatus.cs
│   │   │   ├── ShiftAssignmentStatus.cs
│   │   │   ├── ClockEntryType.cs
│   │   │   ├── ClockSource.cs
│   │   │   ├── TimeOffRequestStatus.cs
│   │   │   ├── SwapType.cs
│   │   │   ├── SwapRequestStatus.cs
│   │   │   ├── OpenShiftStatus.cs
│   │   │   ├── ClaimStatus.cs
│   │   │   ├── TimesheetPeriodType.cs
│   │   │   ├── TimesheetPeriodStatus.cs
│   │   │   └── TimesheetEntryStatus.cs
│   │   ├── Interfaces/
│   │   │   ├── IEmployeeRepository.cs
│   │   │   ├── IScheduleRepository.cs
│   │   │   ├── ITimeOffRepository.cs
│   │   │   ├── ITimesheetRepository.cs
│   │   │   ├── IClockEntryRepository.cs
│   │   │   ├── IAvailabilityRepository.cs
│   │   │   ├── IShiftSwapRepository.cs
│   │   │   ├── IOpenShiftRepository.cs
│   │   │   ├── INotificationRepository.cs
│   │   │   ├── IAuditLogRepository.cs
│   │   │   └── IGenericRepository.cs
│   │   └── Constants/
│   │       ├── Permissions.cs
│   │       └── SystemRoles.cs
│   │
│   ├── StaffPro.DataAccess/                    # Data Access Layer
│   │   ├── ApplicationDbContext.cs
│   │   ├── Configurations/                     # EF Core Fluent API configurations
│   │   │   ├── OrganizationConfiguration.cs
│   │   │   ├── EmployeeConfiguration.cs
│   │   │   ├── ShiftAssignmentConfiguration.cs
│   │   │   ├── ClockEntryConfiguration.cs
│   │   │   ├── TimesheetEntryConfiguration.cs
│   │   │   └── ... (one per entity)
│   │   ├── Repositories/
│   │   │   ├── GenericRepository.cs
│   │   │   ├── EmployeeRepository.cs
│   │   │   ├── ScheduleRepository.cs
│   │   │   ├── TimeOffRepository.cs
│   │   │   ├── TimesheetRepository.cs
│   │   │   ├── ClockEntryRepository.cs
│   │   │   ├── AvailabilityRepository.cs
│   │   │   ├── ShiftSwapRepository.cs
│   │   │   ├── OpenShiftRepository.cs
│   │   │   ├── NotificationRepository.cs
│   │   │   └── AuditLogRepository.cs
│   │   ├── Migrations/
│   │   ├── Seeders/
│   │   │   ├── DbInitializer.cs
│   │   │   ├── RoleSeeder.cs
│   │   │   └── DemoDataSeeder.cs               # optional: seed demo data for development
│   │   └── Interceptors/
│   │       ├── AuditInterceptor.cs             # EF Core SaveChanges interceptor for audit logs
│   │       └── SoftDeleteInterceptor.cs
│   │
│   └── StaffPro.Infrastructure/                # Infrastructure Layer (External Integrations)
│       ├── Email/
│       │   ├── IEmailService.cs
│       │   └── SendGridEmailService.cs
│       ├── Push/
│       │   ├── IPushNotificationService.cs
│       │   └── FcmPushService.cs
│       ├── Storage/
│       │   ├── IFileStorageService.cs
│       │   └── AzureBlobStorageService.cs
│       ├── Export/
│       │   ├── CsvExportService.cs
│       │   └── PdfExportService.cs
│       └── Webhooks/
│           ├── IWebhookSender.cs
│           └── HmacWebhookSender.cs
│
└── tests/
    ├── StaffPro.UnitTests/
    │   ├── Services/
    │   │   ├── ConflictDetectionServiceTests.cs    # most critical tests
    │   │   ├── ScheduleServiceTests.cs
    │   │   ├── TimesheetServiceTests.cs
    │   │   ├── ClockServiceTests.cs
    │   │   ├── TimeOffServiceTests.cs
    │   │   └── PayrollReportServiceTests.cs
    │   ├── Validators/
    │   └── Mappings/
    │
    └── StaffPro.IntegrationTests/
        ├── Api/
        │   ├── EmployeesApiTests.cs
        │   ├── SchedulesApiTests.cs
        │   ├── ClockApiTests.cs
        │   ├── TimeOffApiTests.cs
        │   └── PosIntegrationApiTests.cs
        └── Helpers/
            └── TestWebApplicationFactory.cs
```

---

## 6. Key Services Detail

### 6.1 ConflictDetectionService (Most Critical Service)

This is the heart of the scheduling system. Every time a manager creates or modifies a shift assignment, this service runs ALL constraint checks and returns a result object.

```csharp
public interface IConflictDetectionService
{
    Task<ConflictCheckResult> CheckAssignment(ShiftAssignmentDto assignment);
    Task<ScheduleValidationResult> ValidateEntireSchedule(Guid schedulePeriodId);
}

public class ConflictCheckResult
{
    public bool HasErrors { get; set; }                 // hard blocks
    public bool HasWarnings { get; set; }               // soft warnings
    public List<ScheduleConflict> Conflicts { get; set; }
}

public class ScheduleConflict
{
    public ConflictSeverity Severity { get; set; }      // Error, Warning
    public ConflictType Type { get; set; }              // Overlap, InsufficientRest, Overtime, etc.
    public string Message { get; set; }                 // human-readable
    public Guid? RelatedShiftAssignmentId { get; set; } // the conflicting shift
}
```

**Checks performed (in order):**
1. Employee exists and is Active
2. Employee is assigned to this location
3. Employee has the required role for the station
4. Employee has all mandatory certifications (not expired)
5. Employee is available (recurring rules + overrides)
6. Employee is not on approved leave
7. No overlapping shift on the same date
8. Minimum rest between shifts (e.g., 11 hours from end of previous shift)
9. Maximum hours per day not exceeded
10. Maximum hours per week not exceeded
11. Maximum consecutive working days not exceeded
12. Shift does not fall on a blackout date (warning only)
13. Station maximum capacity not exceeded
14. Staffing maximum for this role/shift not exceeded

### 6.2 TimesheetService

Generates timesheet entries from clock data and scheduling rules. This is the bridge between raw clock punches and payroll-ready summaries.

**How timesheet auto-generation works step-by-step:**

```csharp
public class TimesheetService : ITimesheetService
{
    /// <summary>
    /// Called by Hangfire job "TimesheetAutoGeneration" every night at 02:00.
    /// For each employee who worked yesterday, creates or updates their TimesheetEntry.
    /// </summary>
    public async Task GenerateEntries(DateOnly date, Guid locationId)
    {
        // 1. Get all shift assignments for this date at this location
        var assignments = await _scheduleRepo.GetAssignmentsByDate(date, locationId);
        
        foreach (var assignment in assignments)
        {
            // 2. Get clock entries for this shift assignment
            var clockEntries = await _clockRepo.GetByShiftAssignment(assignment.Id);
            
            if (clockEntries.Count == 0)
            {
                // No clock entries → employee was a No-Show
                // Create entry with 0 actual hours, flag for manager review
                continue;
            }
            
            // 3. Calculate raw worked time
            var shiftStart = clockEntries.First(c => c.Type == ClockEntryType.ShiftStart);
            var shiftEnd = clockEntries.FirstOrDefault(c => c.Type == ClockEntryType.ShiftEnd);
            
            if (shiftEnd == null)
            {
                // Employee forgot to clock out → flag for manager correction
                // Use scheduled end time as fallback, with "NeedsReview" flag
                continue;
            }
            
            // 4. Apply rounding rules (from organization settings)
            var roundedStart = ApplyRounding(shiftStart.Timestamp, _settings.RoundingMinutes);
            var roundedEnd = ApplyRounding(shiftEnd.Timestamp, _settings.RoundingMinutes);
            var rawHours = (roundedEnd - roundedStart).TotalHours;
            
            // 5. Calculate break time
            var breakEntries = clockEntries.Where(c => 
                c.Type == ClockEntryType.BreakStart || c.Type == ClockEntryType.BreakEnd);
            var actualBreakMinutes = CalculateBreakTime(breakEntries);
            
            // If no break was clocked but shift > 6 hours → auto-deduct
            if (actualBreakMinutes == 0 && rawHours > 6 && _settings.AutoDeductBreak)
                actualBreakMinutes = assignment.BreakDurationMinutes;
            
            // 6. Net hours (raw - unpaid breaks)
            var unpaidBreakHours = assignment.BreakIsPaid ? 0 : actualBreakMinutes / 60.0;
            var netHours = rawHours - unpaidBreakHours;
            
            // 7. Classify hours by type
            var contract = await _contractRepo.GetActiveByEmployee(assignment.EmployeeId);
            var hourBreakdown = ClassifyHours(
                roundedStart, roundedEnd, contract,
                isWeekend: date.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday,
                isHoliday: await IsPublicHoliday(date, locationId)
            );
            
            // hourBreakdown returns:
            // { RegularHours: 7.5, OvertimeHours: 0.5, NightHours: 2.0, 
            //   WeekendHours: 0.0, HolidayHours: 0.0 }
            
            // 8. Check for WEEKLY overtime (requires summing the whole week)
            var weeklyTotal = await GetWeeklyHoursToDate(assignment.EmployeeId, date);
            if (weeklyTotal + netHours > contract.OvertimeThresholdWeekly)
            {
                var weeklyOvertimeHours = (weeklyTotal + netHours) - contract.OvertimeThresholdWeekly;
                // Reclassify: move hours from regular to overtime
                hourBreakdown.OvertimeHours += weeklyOvertimeHours;
                hourBreakdown.RegularHours -= weeklyOvertimeHours;
            }
            
            // 9. Save TimesheetEntry
            var entry = new TimesheetEntry
            {
                EmployeeId = assignment.EmployeeId,
                ShiftAssignmentId = assignment.Id,
                Date = date,
                ScheduledHours = assignment.ScheduledDuration,
                ActualHours = (decimal)netHours,
                RegularHours = hourBreakdown.RegularHours,
                OvertimeHours = hourBreakdown.OvertimeHours,
                NightHours = hourBreakdown.NightHours,
                WeekendHours = hourBreakdown.WeekendHours,
                HolidayHours = hourBreakdown.HolidayHours,
                BreakHours = (decimal)(actualBreakMinutes / 60.0),
                NetPayableHours = (decimal)netHours,
                Status = TimesheetEntryStatus.AutoGenerated
            };
            
            await _timesheetRepo.Add(entry);
        }
    }
    
    /// <summary>
    /// Rounding rules: nearest 5, 10, or 15 minutes.
    /// Example (round to 15): 09:07 → 09:00, 09:08 → 09:15, 09:22 → 09:15, 09:23 → 09:30
    /// </summary>
    private DateTime ApplyRounding(DateTime timestamp, int roundingMinutes)
    {
        var totalMinutes = timestamp.Minute;
        var remainder = totalMinutes % roundingMinutes;
        var roundedMinutes = remainder < (roundingMinutes / 2.0)
            ? totalMinutes - remainder           // round down
            : totalMinutes + (roundingMinutes - remainder); // round up
        return new DateTime(timestamp.Year, timestamp.Month, timestamp.Day, 
                            timestamp.Hour, 0, 0).AddMinutes(roundedMinutes);
    }
}
```

**Night hours classification logic:**
```
Night hours = time worked between 23:00 and 06:00 (configurable per organization)

Example: Employee works 20:00–02:00
  → Regular hours: 20:00–23:00 = 3 hours
  → Night hours: 23:00–02:00 = 3 hours
  → Night premium multiplier: 1.25× (Swiss default)
  → Pay calculation: 3h × CHF 25.00 + 3h × CHF 31.25 = CHF 168.75
```

### 6.3 PayrollReportService

Aggregates timesheet data into payroll-ready summaries. Designed to produce CSV/Excel/PDF exports that any payroll software can import.

**Per employee per period — detailed calculation example:**

```
Employee: Marie Dubois (Waiter, Senior)
Contract: Part-Time, 30h/week, CHF 26.00/h, OT threshold: 30h/week
Period: Feb 17–23, 2026

┌──────────────┬──────────┬──────────┬────────┬────────┬────────┬────────┐
│ Date         │ Scheduled│ Actual   │ Regular│ OT     │ Night  │ Weekend│
├──────────────┼──────────┼──────────┼────────┼────────┼────────┼────────┤
│ Mon Feb 17   │ 6.0h     │ 6.0h     │ 6.0h   │ 0.0h   │ 0.0h   │ 0.0h   │
│ Tue Feb 18   │ 6.0h     │ 6.5h     │ 6.5h   │ 0.0h   │ 0.0h   │ 0.0h   │
│ Wed Feb 19   │ 6.0h     │ 6.0h     │ 6.0h   │ 0.0h   │ 0.0h   │ 0.0h   │
│ Thu Feb 20   │ 6.0h     │ 6.0h     │ 6.0h   │ 0.0h   │ 0.0h   │ 0.0h   │
│ Fri Feb 21   │ 8.0h     │ 8.5h     │ 5.5h   │ 3.0h   │ 2.0h   │ 0.0h   │
│ Sat Feb 22   │ 6.0h     │ 6.0h     │ 0.0h   │ 6.0h   │ 0.0h   │ 6.0h   │
│ Sun Feb 23   │ OFF      │ 0.0h     │ 0.0h   │ 0.0h   │ 0.0h   │ 0.0h   │
├──────────────┼──────────┼──────────┼────────┼────────┼────────┼────────┤
│ TOTALS       │ 38.0h    │ 39.0h    │ 30.0h  │ 9.0h   │ 2.0h   │ 6.0h   │
└──────────────┴──────────┴──────────┴────────┴────────┴────────┴────────┘

Pay Breakdown:
  Regular:    30.0h × CHF 26.00          = CHF 780.00
  Overtime:    9.0h × CHF 32.50 (1.25×)  = CHF 292.50
  Night:       2.0h × CHF  6.50 (0.25×)  = CHF  13.00  (premium on top)
  Weekend:     6.0h × CHF  6.50 (0.25×)  = CHF  39.00  (premium on top)
  Tips:        (from TipDistribution)     = CHF 185.00
  ─────────────────────────────────────────────────────
  Gross Total:                            = CHF 1,309.50

  Deductions:
    Meal allowance (3 meals × CHF 3.50)   = CHF -10.50
  ─────────────────────────────────────────────────────
  Net Payable:                            = CHF 1,299.00
```

**CSV export columns (configurable per payroll software):**

| Column | Example Value | Notes |
|--------|--------------|-------|
| EmployeeNumber | EMP-012 | Auto-generated or manual |
| FirstName | Marie | |
| LastName | Dubois | |
| Department | FOH | Front of House |
| CostCenter | 100-FOH | Organization-specific |
| RegularHours | 30.00 | |
| OvertimeHours | 9.00 | |
| NightHours | 2.00 | Premium hours (overlap with regular) |
| WeekendHours | 6.00 | Premium hours (overlap with OT) |
| HolidayHours | 0.00 | |
| TotalHours | 39.00 | |
| HourlyRate | 26.00 | In configured currency |
| OvertimeRate | 32.50 | |
| GrossPay | 1309.50 | |
| TipAmount | 185.00 | |
| Deductions | 10.50 | |
| NetPay | 1299.00 | |
| IBAN | CH93 0076... | Only if included in export config |
| PeriodStart | 2026-02-17 | |
| PeriodEnd | 2026-02-23 | |

### 6.4 DemandForecastService (AI Forecasting Engine)

Predicts expected sales and covers for future dates, then calculates recommended staffing levels. The algorithm improves over time as more POS data accumulates.

```csharp
public class DemandForecastService : IDemandForecastService
{
    /// <summary>
    /// Generates demand forecasts for the next 14 days at a specific location.
    /// Requires minimum 4 weeks of historical POS sales data.
    /// </summary>
    public async Task<List<DemandForecast>> GenerateForecast(Guid locationId)
    {
        // 1. Check data availability
        var historicalDays = await _salesRepo.GetDayCount(locationId);
        if (historicalDays < 28)
            throw new InsufficientDataException(
                $"Need 28+ days of sales data, have {historicalDays}. " +
                "Keep syncing POS data — forecasting activates automatically.");

        var forecasts = new List<DemandForecast>();
        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        for (int dayOffset = 1; dayOffset <= 14; dayOffset++)
        {
            var forecastDate = today.AddDays(dayOffset);
            var dayOfWeek = forecastDate.DayOfWeek;

            for (int hour = 0; hour < 24; hour++)
            {
                // 2. Weighted Moving Average — last 8 same-day occurrences
                //    (e.g., last 8 Fridays for forecasting a Friday)
                //    Recent weeks weighted more heavily
                var sameDaySales = await _salesRepo.GetHourlySales(
                    locationId, dayOfWeek, hour, weeksBack: 8);
                
                // Weights: [0.25, 0.20, 0.15, 0.12, 0.10, 0.08, 0.06, 0.04]
                // Most recent week gets 25%, oldest gets 4%
                var weights = new[] { 0.25, 0.20, 0.15, 0.12, 0.10, 0.08, 0.06, 0.04 };
                var baseForecast = sameDaySales
                    .Zip(weights)
                    .Sum(x => x.First.Sales * (decimal)x.Second);

                // 3. Seasonal adjustment (monthly trend)
                //    Compare this month's average vs. overall average
                var seasonalFactor = await CalculateSeasonalFactor(
                    locationId, forecastDate.Month);
                baseForecast *= seasonalFactor;

                // 4. Weather adjustment (optional — if OpenWeatherMap configured)
                var weather = await _weatherService.GetForecast(locationId, forecastDate);
                if (weather != null)
                {
                    var weatherFactor = GetWeatherFactor(weather);
                    // Rainy day → -15% for terrace restaurants
                    // Sunny day → +10% for terrace restaurants
                    // Snow day → -25% (people stay home)
                    // Temperature > 25°C → +8% (more outdoor dining)
                    baseForecast *= weatherFactor;
                }

                // 5. Special event adjustment (manually entered events)
                var events = await _eventRepo.GetByDate(locationId, forecastDate);
                foreach (var evt in events)
                {
                    // "Football match nearby" → +30%
                    // "Public holiday" → varies (+20% for dinner, -40% for lunch)
                    // "Festival week" → +25%
                    baseForecast *= evt.SalesMultiplier;
                }

                // 6. Calculate expected covers from sales
                var avgTicket = await _salesRepo.GetAverageTicketSize(locationId);
                var forecastedCovers = (int)(baseForecast / avgTicket);

                forecasts.Add(new DemandForecast
                {
                    LocationId = locationId,
                    Date = forecastDate,
                    Hour = hour,
                    ForecastedSales = Math.Round(baseForecast, 2),
                    ForecastedCovers = forecastedCovers,
                    WeatherCondition = weather?.Condition,
                    WeatherTemperature = weather?.Temperature,
                    SpecialEvent = events.FirstOrDefault()?.Name,
                    GeneratedAt = DateTime.UtcNow
                });
            }
        }

        // 7. Generate staffing recommendations based on forecast
        await GenerateStaffingRecommendations(locationId, forecasts);

        return forecasts;
    }

    /// <summary>
    /// Converts demand forecast into staffing recommendations per role per shift.
    /// Uses configurable staff-to-demand ratios.
    /// </summary>
    private async Task GenerateStaffingRecommendations(
        Guid locationId, List<DemandForecast> forecasts)
    {
        // Staff-to-demand ratios (configurable per organization):
        // - 1 Waiter per 4-5 tables (4 tables × 2.5 covers/table = 10 covers)
        // - 1 Kitchen staff per 15-20 covers/hour
        // - 1 Bartender per 25-30 covers
        // - 1 Host per 40-50 covers

        var ratios = await _ratioRepo.GetByLocation(locationId);
        var shiftTemplates = await _shiftTemplateRepo.GetByOrganization(/* ... */);

        foreach (var shiftTemplate in shiftTemplates)
        {
            var shiftHours = GetHourRange(shiftTemplate.StartTime, shiftTemplate.EndTime);

            foreach (var date in forecasts.Select(f => f.Date).Distinct())
            {
                var shiftForecasts = forecasts
                    .Where(f => f.Date == date && shiftHours.Contains(f.Hour));
                var peakCovers = shiftForecasts.Max(f => f.ForecastedCovers);
                var totalSales = shiftForecasts.Sum(f => f.ForecastedSales);

                foreach (var ratio in ratios)
                {
                    var recommendedStaff = (int)Math.Ceiling(
                        (double)peakCovers / ratio.CoversPerStaff);
                    
                    recommendedStaff = Math.Max(recommendedStaff, ratio.MinimumStaff);
                    recommendedStaff = Math.Min(recommendedStaff, ratio.MaximumStaff);

                    var recommendedHours = recommendedStaff * shiftTemplate.DurationHours;
                    var projectedSPLH = recommendedHours > 0
                        ? totalSales / recommendedHours
                        : 0;

                    await _recRepo.Save(new StaffingRecommendation
                    {
                        LocationId = locationId,
                        RoleId = ratio.RoleId,
                        ShiftTemplateId = shiftTemplate.Id,
                        Date = date,
                        RecommendedStaff = recommendedStaff,
                        RecommendedHours = recommendedHours,
                        ForecastedSales = totalSales,
                        ProjectedSPLH = projectedSPLH,
                        GeneratedAt = DateTime.UtcNow
                    });
                }
            }
        }
    }
}
```

**Forecast accuracy tracking:**
```
Every day at 04:00, the ForecastAccuracyUpdate job compares yesterday's forecast vs. actual:

  Forecast for Feb 19, 12:00-13:00: CHF 2,400 / 48 covers
  Actual for Feb 19, 12:00-13:00:   CHF 2,550 / 52 covers
  Accuracy: 94.1% (within CHF 150)

  Overall Feb accuracy: 91.3% (target: 85%+)
  SPLH target: CHF 45.00 — Actual SPLH: CHF 47.20 → Above target ✅
```

### 6.5 GeofenceService (GPS Boundary Verification)

Validates that an employee is physically at the restaurant before allowing clock-in. Uses the Haversine formula to calculate distance between two GPS coordinates.

```csharp
public class GeofenceService : IGeofenceService
{
    /// <summary>
    /// Checks if the given coordinates fall within any active geofence zone at the location.
    /// Returns verification result with distance and zone details.
    /// </summary>
    public async Task<GeofenceVerificationResult> VerifyLocation(
        Guid locationId, double latitude, double longitude, double accuracyMeters)
    {
        var zones = await _geofenceRepo.GetActiveByLocation(locationId);
        
        if (zones.Count == 0)
        {
            // No geofences configured → always allow (geofencing is opt-in)
            return new GeofenceVerificationResult 
            { 
                IsWithinGeofence = true, 
                Reason = "No geofence configured for this location" 
            };
        }

        // Check against low GPS accuracy (possible spoofing)
        if (accuracyMeters > 100)
        {
            // Create anti-fraud alert
            await CreateFraudAlert(locationId, FraudAlertType.LowGpsAccuracy,
                $"GPS accuracy: {accuracyMeters}m (threshold: 100m). Possible GPS spoofing.");
            
            // Still allow clock-in but flag it (soft block — manager decides)
            return new GeofenceVerificationResult
            {
                IsWithinGeofence = false,
                Reason = $"GPS accuracy too low ({accuracyMeters:F0}m). " +
                         "Move to an open area or connect to WiFi for better accuracy.",
                IsSoftBlock = true  // manager can override
            };
        }

        foreach (var zone in zones)
        {
            var distanceMeters = CalculateHaversineDistance(
                latitude, longitude, 
                zone.CenterLatitude, zone.CenterLongitude);

            if (distanceMeters <= zone.RadiusMeters)
            {
                return new GeofenceVerificationResult
                {
                    IsWithinGeofence = true,
                    ZoneName = zone.Name,
                    DistanceMeters = distanceMeters,
                    ZoneRadiusMeters = zone.RadiusMeters
                };
            }
        }

        // Employee is outside ALL geofence zones
        var closestZone = zones.MinBy(z => CalculateHaversineDistance(
            latitude, longitude, z.CenterLatitude, z.CenterLongitude));
        var closestDistance = CalculateHaversineDistance(
            latitude, longitude, closestZone.CenterLatitude, closestZone.CenterLongitude);

        return new GeofenceVerificationResult
        {
            IsWithinGeofence = false,
            Reason = $"You are {closestDistance:F0}m from {closestZone.Name} " +
                     $"(must be within {closestZone.RadiusMeters}m).",
            ClosestZoneName = closestZone.Name,
            DistanceMeters = closestDistance
        };
    }

    /// <summary>
    /// Haversine formula — calculates the great-circle distance between two GPS points.
    /// Returns distance in meters. Accurate to ~0.5% for distances under 10km.
    /// </summary>
    private static double CalculateHaversineDistance(
        double lat1, double lon1, double lat2, double lon2)
    {
        const double R = 6371000; // Earth's radius in meters
        
        var dLat = DegreesToRadians(lat2 - lat1);
        var dLon = DegreesToRadians(lon2 - lon1);
        
        var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(DegreesToRadians(lat1)) * Math.Cos(DegreesToRadians(lat2)) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
        
        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        
        return R * c; // distance in meters
    }
}
```

### 6.6 TipDistributionService (Pool Calculation Engine)

Distributes tips among employees based on configurable pool rules. Supports three distribution methods: by hours worked, equal split, and custom percentage per role.

```csharp
public class TipDistributionService : ITipDistributionService
{
    /// <summary>
    /// Calculates tip distribution for a tip entry. Does NOT save — returns preview.
    /// Manager reviews the preview, makes adjustments, then calls Finalize().
    /// </summary>
    public async Task<TipDistributionPreview> CalculateDistribution(Guid tipEntryId)
    {
        var tipEntry = await _tipRepo.GetById(tipEntryId);
        var tipPool = await _poolRepo.GetById(tipEntry.TipPoolId);
        var poolRoles = await _poolRoleRepo.GetByPool(tipPool.Id);

        // Get all employees who worked this shift at this location on this date
        var assignments = await _scheduleRepo.GetAssignmentsByDateAndLocation(
            tipEntry.Date, tipEntry.LocationId, tipEntry.ShiftName);

        // Filter to only employees whose role is in the tip pool
        var eligibleRoleIds = poolRoles.Select(r => r.RoleId).ToHashSet();
        var eligibleAssignments = assignments
            .Where(a => a.RoleId.HasValue && eligibleRoleIds.Contains(a.RoleId.Value))
            .ToList();

        if (eligibleAssignments.Count == 0)
            throw new NoEligibleEmployeesException("No eligible employees found for tip distribution.");

        var distributions = new List<TipDistributionLine>();

        switch (tipPool.Method)
        {
            case TipDistributionMethod.ByHoursWorked:
                // Each role gets its configured percentage of the total
                // Within a role, tips are split proportionally by hours worked
                foreach (var poolRole in poolRoles)
                {
                    var roleAssignments = eligibleAssignments
                        .Where(a => a.RoleId == poolRole.RoleId).ToList();
                    
                    if (roleAssignments.Count == 0) continue;

                    var rolePoolAmount = tipEntry.TotalTipAmount * 
                        (poolRole.SharePercentage / 100m);
                    var totalRoleHours = roleAssignments.Sum(a => 
                        CalculateActualHours(a));

                    foreach (var assignment in roleAssignments)
                    {
                        var hours = CalculateActualHours(assignment);
                        var share = totalRoleHours > 0
                            ? (hours / totalRoleHours) * rolePoolAmount
                            : rolePoolAmount / roleAssignments.Count;

                        distributions.Add(new TipDistributionLine
                        {
                            EmployeeId = assignment.EmployeeId,
                            EmployeeName = assignment.Employee.FullName,
                            RoleName = assignment.Role.Name,
                            HoursWorked = hours,
                            SharePercentage = poolRole.SharePercentage,
                            Amount = Math.Round(share, 2)
                        });
                    }
                }
                break;

            case TipDistributionMethod.EqualSplit:
                // Every eligible employee gets an equal share
                var equalAmount = tipEntry.TotalTipAmount / eligibleAssignments.Count;
                foreach (var assignment in eligibleAssignments)
                {
                    distributions.Add(new TipDistributionLine
                    {
                        EmployeeId = assignment.EmployeeId,
                        Amount = Math.Round(equalAmount, 2)
                    });
                }
                break;

            case TipDistributionMethod.CustomPercentage:
                // Same as ByHoursWorked but within a role, split equally
                // (not by hours)
                foreach (var poolRole in poolRoles)
                {
                    var roleAssignments = eligibleAssignments
                        .Where(a => a.RoleId == poolRole.RoleId).ToList();
                    
                    if (roleAssignments.Count == 0) continue;

                    var rolePoolAmount = tipEntry.TotalTipAmount * 
                        (poolRole.SharePercentage / 100m);
                    var perPerson = rolePoolAmount / roleAssignments.Count;

                    foreach (var assignment in roleAssignments)
                    {
                        distributions.Add(new TipDistributionLine
                        {
                            EmployeeId = assignment.EmployeeId,
                            Amount = Math.Round(perPerson, 2)
                        });
                    }
                }
                break;
        }

        // Handle rounding: ensure distributions sum to exactly TotalTipAmount
        var distributedTotal = distributions.Sum(d => d.Amount);
        var roundingDiff = tipEntry.TotalTipAmount - distributedTotal;
        if (roundingDiff != 0)
            distributions.First().Amount += roundingDiff; // add pennies to first person

        return new TipDistributionPreview
        {
            TipEntryId = tipEntryId,
            TotalAmount = tipEntry.TotalTipAmount,
            Lines = distributions
        };
    }
}
```

**Example tip distribution (ByHoursWorked method):**
```
Friday Evening, Feb 21 — Total Tips: CHF 450.00

Pool Rules: Waiters 60%, Kitchen 25%, Bar 15%

Waiter Pool (60% = CHF 270.00):
  Marie:  6.0h of 18.0h total → 33.3% → CHF 90.00
  Luca:   6.0h of 18.0h total → 33.3% → CHF 90.00
  Sophie: 6.0h of 18.0h total → 33.3% → CHF 90.00

Kitchen Pool (25% = CHF 112.50):
  Pierre: 8.0h of 16.0h total → 50.0% → CHF 56.25
  Marco:  8.0h of 16.0h total → 50.0% → CHF 56.25

Bar Pool (15% = CHF 67.50):
  Alex:   6.0h of 6.0h total  → 100%  → CHF 67.50

Verification: 270.00 + 112.50 + 67.50 = CHF 450.00 ✅
```

### 6.7 ComplianceService (Predictive Scheduling)

Enforces Fair Workweek laws and Swiss labor regulations. Calculates schedule change premiums and generates compliance documentation.

```csharp
public class ComplianceService : IComplianceService
{
    /// <summary>
    /// Called every time a published schedule is modified. Checks if the change
    /// falls within the advance notice window and calculates any premium owed.
    /// </summary>
    public async Task<ComplianceCheckResult> CheckScheduleChange(
        ShiftAssignment originalAssignment,
        ShiftAssignment? modifiedAssignment,  // null if shift was deleted
        Guid organizationId)
    {
        var jurisdiction = await _settingsRepo.GetActiveJurisdiction(organizationId);
        if (jurisdiction == null)
            return ComplianceCheckResult.NoActiveJurisdiction();

        var schedule = await _scheduleRepo.GetById(originalAssignment.SchedulePeriodId);
        
        // Only applies to PUBLISHED schedules
        if (schedule.Status != ScheduleStatus.Published)
            return ComplianceCheckResult.NotPublished();

        var now = DateTime.UtcNow;
        var shiftDate = originalAssignment.Date.ToDateTime(originalAssignment.StartTime);
        var noticeDays = (shiftDate - now).TotalDays;

        // Get jurisdiction-specific rules
        var rules = GetJurisdictionRules(jurisdiction);
        
        // Example jurisdiction rules:
        // NYC:     14 days notice, $10-$75 penalty depending on notice given
        // Chicago: 10 days notice, $15-$75 penalty
        // Oregon:  14 days notice, 1 hour of pay for each change
        // Swiss:   No Fair Workweek law, but 11h min rest + max 45h/week enforced

        string changeType;
        if (modifiedAssignment == null)
            changeType = "Removed";
        else if (originalAssignment.StartTime != modifiedAssignment.StartTime ||
                 originalAssignment.EndTime != modifiedAssignment.EndTime)
            changeType = "TimeChanged";
        else
            changeType = "Other";

        decimal premiumAmountCents = 0;

        if (noticeDays < rules.AdvanceNoticeDays)
        {
            // Schedule changed within notice window → premium applies
            premiumAmountCents = CalculatePremium(
                noticeDays, changeType, rules, originalAssignment);

            // Record the premium
            var premium = new ScheduleChangePremium
            {
                ShiftAssignmentId = originalAssignment.Id,
                EmployeeId = originalAssignment.EmployeeId,
                ChangeType = changeType,
                NoticeDaysGiven = (int)Math.Floor(noticeDays),
                PremiumAmountCents = premiumAmountCents,
                Jurisdiction = jurisdiction.Name,
                EmployeeAccepted = false,  // will be updated when employee responds
                ChangedAt = DateTime.UtcNow,
                ChangedByUserId = _currentUser.Id
            };

            await _premiumRepo.Add(premium);

            // Notify the employee about the change + their right to refuse
            await _mediator.Publish(new ScheduleChangeNotification(
                originalAssignment.EmployeeId,
                changeType,
                premiumAmountCents,
                jurisdiction.RequiresRightToRefuse));
        }

        // Check clopening (closing shift → opening shift < 11 hours rest)
        var clopen = await CheckClopeningViolation(
            originalAssignment.EmployeeId,
            modifiedAssignment ?? originalAssignment,
            jurisdiction);

        return new ComplianceCheckResult
        {
            HasViolation = premiumAmountCents > 0 || clopen.IsViolation,
            PremiumAmountCents = premiumAmountCents,
            NoticeDaysGiven = (int)Math.Floor(noticeDays),
            RequiredNoticeDays = rules.AdvanceNoticeDays,
            ClopeningViolation = clopen,
            RightToRefuseRequired = jurisdiction.RequiresRightToRefuse
        };
    }
}
```

**Jurisdiction preset values (built-in):**

| Jurisdiction | Advance Notice | Premium Tiers | Clopening Rule | Right to Refuse |
|-------------|---------------|---------------|----------------|-----------------|
| **NYC Fair Workweek** | 14 days | >14d: $0, 7-14d: $10-20, <7d: $15-45, <24h: $15-75 | 11h min rest | Yes |
| **Chicago Fair Workweek** | 10 days | >10d: $0, 3-10d: $15, <3d: $45, <24h: $75 | 10h min rest | Yes |
| **Oregon Predictive** | 14 days | 1 hour of pay per change | 10h min rest | Yes |
| **Seattle Secure** | 14 days | 50% of scheduled pay for <14d changes | 10h min rest | Yes |
| **Swiss Labor Law** | No law | N/A | 11h min rest (ArG Art. 15a) | N/A |
| **EU Working Time Directive** | No law | N/A | 11h min rest (Art. 3) | N/A |
| **SF Formula Retail** | 14 days | 1-4 hours of pay per change | No specific rule | Yes |

### 6.8 FlightRiskService (Employee Retention Analytics)

Analyzes engagement metrics to identify employees who may be at risk of quitting. Runs weekly and alerts managers to take preventive action.

```csharp
public class FlightRiskService
{
    /// <summary>
    /// Calculates a flight risk score (0-100) for each active employee.
    /// Higher score = higher risk of quitting. Score > 70 triggers manager alert.
    /// </summary>
    public async Task<FlightRiskAssessment> AnalyzeEmployee(Guid employeeId)
    {
        var employee = await _employeeRepo.GetById(employeeId);
        var score = 0.0;
        var factors = new List<FlightRiskFactor>();

        // Factor 1: Survey score trend (weight: 25%)
        // If average survey score declined by >1 star over last 4 weeks → +25 points
        var recentSurveys = await _surveyRepo.GetByEmployee(employeeId, weeksBack: 8);
        if (recentSurveys.Count >= 4)
        {
            var firstHalfAvg = recentSurveys.Take(recentSurveys.Count / 2).Average(s => s.Rating);
            var secondHalfAvg = recentSurveys.Skip(recentSurveys.Count / 2).Average(s => s.Rating);
            var decline = firstHalfAvg - secondHalfAvg;
            
            if (decline > 1.0)
            {
                score += 25;
                factors.Add(new FlightRiskFactor("Survey scores declining", 
                    $"Average dropped from {firstHalfAvg:F1} to {secondHalfAvg:F1} stars"));
            }
            else if (decline > 0.5)
            {
                score += 12;
                factors.Add(new FlightRiskFactor("Survey scores slightly declining", 
                    $"Average dropped by {decline:F1} stars"));
            }
        }

        // Factor 2: Attendance reliability (weight: 20%)
        // Late arrivals, no-shows, early departures over last 30 days
        var clockEntries = await _clockRepo.GetByEmployee(employeeId, daysBack: 30);
        var lateArrivals = CountLateArrivals(clockEntries);
        var noShows = CountNoShows(employeeId, daysBack: 30);
        
        if (noShows >= 2 || lateArrivals >= 5)
        {
            score += 20;
            factors.Add(new FlightRiskFactor("Attendance issues", 
                $"{noShows} no-shows, {lateArrivals} late arrivals in last 30 days"));
        }

        // Factor 3: Time-off request pattern (weight: 15%)
        // Sudden increase in time-off requests (3+ in last month vs. usual 1 or less)
        var recentRequests = await _timeOffRepo.GetByEmployee(employeeId, daysBack: 30);
        var historicalMonthlyAvg = await _timeOffRepo.GetMonthlyAverage(employeeId);
        
        if (recentRequests.Count > historicalMonthlyAvg * 2 && recentRequests.Count >= 3)
        {
            score += 15;
            factors.Add(new FlightRiskFactor("Increased time-off requests", 
                $"{recentRequests.Count} requests this month (average: {historicalMonthlyAvg:F1})"));
        }

        // Factor 4: Availability reduction (weight: 15%)
        // Employee reduced their available hours significantly
        var currentAvailability = await _availabilityRepo.GetTotalAvailableHours(employeeId);
        var previousAvailability = await _availabilityRepo.GetTotalAvailableHours(
            employeeId, weeksAgo: 8);
        
        if (previousAvailability > 0 && 
            currentAvailability < previousAvailability * 0.7) // >30% reduction
        {
            score += 15;
            factors.Add(new FlightRiskFactor("Reduced availability", 
                $"From {previousAvailability:F0}h to {currentAvailability:F0}h per week"));
        }

        // Factor 5: Tenure risk (weight: 10%)
        // Employees in first 90 days have highest turnover risk
        var tenure = (DateTime.UtcNow - employee.HireDate).TotalDays;
        if (tenure < 30) { score += 10; factors.Add(new("New hire (< 30 days)", "")); }
        else if (tenure < 90) { score += 7; factors.Add(new("In probation (< 90 days)", "")); }

        // Factor 6: Open shift acceptance rate (weight: 10%)
        // If employee stopped accepting open shifts (was previously active)
        var openShiftResponse = await _openShiftRepo.GetResponseRate(employeeId, daysBack: 60);
        if (openShiftResponse.PreviousRate > 0.5 && openShiftResponse.CurrentRate < 0.1)
        {
            score += 10;
            factors.Add(new FlightRiskFactor("Stopped accepting open shifts", 
                $"Rate dropped from {openShiftResponse.PreviousRate:P0} to {openShiftResponse.CurrentRate:P0}"));
        }

        // Factor 7: Recognition received (weight: 5%)
        // Employees who never receive recognition are more likely to leave
        var recognitionCount = await _recognitionRepo.CountByRecipient(employeeId, daysBack: 90);
        if (recognitionCount == 0 && tenure > 30)
        {
            score += 5;
            factors.Add(new FlightRiskFactor("No recognition received in 90 days", 
                "Consider acknowledging their contributions"));
        }

        // Normalize score to 0-100
        score = Math.Min(score, 100);

        var riskLevel = score switch
        {
            >= 70 => RiskLevel.High,     // Alert manager immediately
            >= 40 => RiskLevel.Medium,   // Include in weekly report
            _ => RiskLevel.Low            // No action needed
        };

        return new FlightRiskAssessment
        {
            EmployeeId = employeeId,
            EmployeeName = employee.FullName,
            Score = score,
            RiskLevel = riskLevel,
            Factors = factors,
            SuggestedActions = GenerateSuggestedActions(factors, riskLevel),
            AssessedAt = DateTime.UtcNow
        };
    }

    private List<string> GenerateSuggestedActions(
        List<FlightRiskFactor> factors, RiskLevel level)
    {
        var actions = new List<string>();
        
        if (level >= RiskLevel.Medium)
            actions.Add("Schedule a 1-on-1 conversation with this employee");
        
        if (factors.Any(f => f.Name.Contains("recognition")))
            actions.Add("Send a recognition shout-out to acknowledge their work");
        
        if (factors.Any(f => f.Name.Contains("Survey")))
            actions.Add("Review recent survey comments for specific concerns");
        
        if (factors.Any(f => f.Name.Contains("New hire")))
            actions.Add("Check onboarding progress — ensure they feel supported");
        
        if (factors.Any(f => f.Name.Contains("Attendance")))
            actions.Add("Discuss schedule flexibility — might have external conflicts");

        return actions;
    }
}
```

---

## 6A. Database Indexing Strategy

> Without proper indexes, schedule queries that join ShiftAssignment + Employee + Role + Station + ClockEntry will become progressively slower as data grows. This section defines the critical indexes that must exist from day one.

### Critical Performance Indexes

```sql
-- ═══════════════════════════════════════════════════════════════
-- SCHEDULING QUERIES (most frequent — every schedule builder load)
-- ═══════════════════════════════════════════════════════════════

-- "Get all shift assignments for a schedule period" (every time schedule page loads)
-- This is the #1 most executed query in the entire system
CREATE INDEX IX_ShiftAssignment_SchedulePeriodId_Date 
ON ShiftAssignments (SchedulePeriodId, Date)
INCLUDE (EmployeeId, RoleId, StationId, StartTime, EndTime, Status);

-- "Get all assignments for an employee in a date range" (conflict detection, my-schedule)
CREATE INDEX IX_ShiftAssignment_EmployeeId_Date 
ON ShiftAssignments (EmployeeId, Date)
INCLUDE (SchedulePeriodId, StartTime, EndTime, Status);

-- "Get schedule periods by location and date range" (schedule list page)
CREATE INDEX IX_SchedulePeriod_LocationId_StartDate 
ON SchedulePeriods (LocationId, StartDate DESC)
INCLUDE (EndDate, Status, PublishedAt);

-- ═══════════════════════════════════════════════════════════════
-- CLOCK & TIMESHEET QUERIES (real-time during shifts)
-- ═══════════════════════════════════════════════════════════════

-- "Get clock entries for an employee today" (clock status widget)
CREATE INDEX IX_ClockEntry_EmployeeId_Timestamp 
ON ClockEntries (EmployeeId, Timestamp DESC)
INCLUDE (Type, ShiftAssignmentId, Source);

-- "Get clock entries for a shift assignment" (timesheet generation)
CREATE INDEX IX_ClockEntry_ShiftAssignmentId 
ON ClockEntries (ShiftAssignmentId)
INCLUDE (Type, Timestamp, Latitude, Longitude);

-- "Get timesheet entries for a period" (timesheet approval page)
CREATE INDEX IX_TimesheetEntry_PeriodId_EmployeeId 
ON TimesheetEntries (TimesheetPeriodId, EmployeeId)
INCLUDE (Date, ActualHours, RegularHours, OvertimeHours, Status);

-- ═══════════════════════════════════════════════════════════════
-- AVAILABILITY QUERIES (schedule builder — checked per assignment)
-- ═══════════════════════════════════════════════════════════════

-- "Get recurring availability for an employee" (conflict detection)
CREATE INDEX IX_AvailabilityRule_EmployeeId_DayOfWeek 
ON AvailabilityRules (EmployeeId, DayOfWeek)
INCLUDE (StartTime, EndTime, Type, EffectiveFrom, EffectiveUntil);

-- "Get availability overrides for date range" (schedule builder)
CREATE INDEX IX_AvailabilityOverride_EmployeeId_Date 
ON AvailabilityOverrides (EmployeeId, Date)
INCLUDE (StartTime, EndTime, Type);

-- ═══════════════════════════════════════════════════════════════
-- TIME-OFF QUERIES (leave calendar, conflict detection)
-- ═══════════════════════════════════════════════════════════════

-- "Get approved time-off for date range" (team calendar, conflict check)
CREATE INDEX IX_TimeOffRequest_EmployeeId_Status_Dates 
ON TimeOffRequests (EmployeeId, Status, StartDate, EndDate)
INCLUDE (LeaveTypeId, TotalDays);

-- "Get pending requests for manager approval"
CREATE INDEX IX_TimeOffRequest_Status_CreatedAt 
ON TimeOffRequests (Status, CreatedAt DESC)
WHERE Status = 0; -- Pending status only (filtered index)

-- ═══════════════════════════════════════════════════════════════
-- EMPLOYEE QUERIES (directory, search, dropdowns)
-- ═══════════════════════════════════════════════════════════════

-- "List active employees for organization" (employee dropdown, directory)
CREATE INDEX IX_Employee_OrganizationId_Status 
ON Employees (OrganizationId, Status)
WHERE IsDeleted = 0
INCLUDE (FirstName, LastName, Email, EmployeeNumber);

-- "Find employee by AppUserId" (JWT → employee resolution, every authenticated request)
CREATE UNIQUE INDEX IX_Employee_AppUserId 
ON Employees (AppUserId)
WHERE AppUserId IS NOT NULL;

-- ═══════════════════════════════════════════════════════════════
-- MULTI-TENANCY (applied to EVERY query via EF Core global filter)
-- ═══════════════════════════════════════════════════════════════

-- Every entity with OrganizationId needs this index
-- These are already covered by composite indexes above, but for smaller tables:
CREATE INDEX IX_Role_OrganizationId ON Roles (OrganizationId);
CREATE INDEX IX_Department_OrganizationId ON Departments (OrganizationId);
CREATE INDEX IX_ShiftTemplate_OrganizationId ON ShiftTemplates (OrganizationId);
CREATE INDEX IX_LeaveType_OrganizationId ON LeaveTypes (OrganizationId);
CREATE INDEX IX_ChatChannel_OrganizationId ON ChatChannels (OrganizationId);

-- ═══════════════════════════════════════════════════════════════
-- COMMUNICATION QUERIES (chat, announcements)
-- ═══════════════════════════════════════════════════════════════

-- "Get messages in channel, newest first" (chat page scroll)
CREATE INDEX IX_ChatMessage_ChannelId_CreatedAt 
ON ChatMessages (ChannelId, CreatedAt DESC)
INCLUDE (SenderUserId, Content, IsDeleted);

-- "Get direct messages between two users"
CREATE INDEX IX_ChatMessage_DM 
ON ChatMessages (SenderUserId, RecipientUserId, CreatedAt DESC)
WHERE ChannelId IS NULL;

-- "Unread message count" (notification badge)
CREATE INDEX IX_ChatMessageReadReceipt_UserId 
ON ChatMessageReadReceipts (UserId, MessageId);

-- ═══════════════════════════════════════════════════════════════
-- NOTIFICATION QUERIES (bell icon, unread count)
-- ═══════════════════════════════════════════════════════════════

-- "Get unread notifications for user" (every page load)
CREATE INDEX IX_Notification_RecipientUserId_IsRead 
ON Notifications (RecipientUserId, IsRead, CreatedAt DESC);

-- ═══════════════════════════════════════════════════════════════
-- AUDIT LOG QUERIES (compliance, investigation)
-- ═══════════════════════════════════════════════════════════════

-- "Get audit history for specific entity"
CREATE INDEX IX_AuditLog_EntityType_EntityId 
ON AuditLogs (EntityType, EntityId, Timestamp DESC);

-- "Search audit by user and date"
CREATE INDEX IX_AuditLog_OrganizationId_Timestamp 
ON AuditLogs (OrganizationId, Timestamp DESC)
INCLUDE (UserId, EntityType, Action);

-- ═══════════════════════════════════════════════════════════════
-- TASK MANAGEMENT QUERIES
-- ═══════════════════════════════════════════════════════════════

-- "Get tasks for today by location" (task dashboard)
CREATE INDEX IX_ShiftTaskInstance_LocationId_Date_Status 
ON ShiftTaskInstances (LocationId, Date, Status)
INCLUDE (TaskTemplateItemId, AssignedEmployeeId, CompletedAt);

-- ═══════════════════════════════════════════════════════════════
-- FORECAST & OPTIMIZATION QUERIES
-- ═══════════════════════════════════════════════════════════════

-- "Get forecast for date range" (schedule builder overlay)
CREATE INDEX IX_DemandForecast_LocationId_Date 
ON DemandForecasts (LocationId, Date, Hour)
INCLUDE (ForecastedSales, ForecastedCovers, ActualSales);
```

### Query Performance Targets

| Query | Target Response Time | Expected Frequency |
|-------|--------------------|--------------------|
| Load schedule builder (1 week, 1 location, ~50 assignments) | < 200ms | Every schedule page load |
| Conflict detection (single assignment) | < 100ms | Every drag-drop in schedule builder |
| Validate entire schedule (50+ assignments) | < 2 seconds | On "Validate" button click |
| Clock-in (verify geofence + create entry) | < 300ms | Every employee clock-in |
| Employee dashboard (my shifts + clock status) | < 150ms | Every employee app open |
| Unread notification count | < 50ms | Every page load (every user) |
| Chat messages (paginated, 25 messages) | < 100ms | Every chat page scroll |
| Demand forecast (14 days × 24 hours) | < 500ms | Schedule builder with overlay |
| Payroll report (20 employees × 1 month) | < 3 seconds | End of each pay period |

### Data Growth Estimates (First Year)

| Table | Expected Rows (Year 1, 50-employee restaurant) | Growth Pattern |
|-------|-------------------------------------------------|----------------|
| ShiftAssignment | ~13,000 (50 employees × 5 shifts/week × 52 weeks) | Linear |
| ClockEntry | ~52,000 (4 entries per shift × 13,000 shifts) | Linear |
| TimesheetEntry | ~13,000 (1 per shift assignment) | Linear |
| ChatMessage | ~50,000–200,000 (depends on team activity) | Exponential early, then steady |
| Notification | ~100,000+ (multiple per user per day) | Linear — cleaned up weekly |
| AuditLog | ~200,000+ (every data change) | Linear — archived monthly |
| DemandForecast | ~122,640 (1 per hour × 24h × 365 days × 14-day look-ahead) | Steady after first 14 days |
| ShiftTaskInstance | ~52,000 (4 tasks per shift × 13,000 shifts) | Linear |

---

## 7. Multi-Tenancy Strategy

Staff Pro is a **multi-tenant application** where each Organization is a tenant.

| Aspect | Strategy |
|--------|----------|
| **Isolation** | Shared database, schema-level isolation (every query filtered by OrganizationId) |
| **Tenant Resolution** | JWT token contains OrganizationId claim; middleware extracts it and sets `ITenantContext` |
| **Query Filtering** | EF Core global query filter: `builder.HasQueryFilter(e => e.OrganizationId == _tenantContext.OrganizationId)` on all tenant-scoped entities |
| **Data Safety** | Impossible to accidentally query another tenant's data — filter is automatic |
| **Cross-Tenant** | System admin role can bypass tenant filter for support operations |

---

## 8. Security Checklist

| Item | Implementation |
|------|---------------|
| **Secrets** | Azure Key Vault in production; user-secrets in development. NEVER in appsettings.json. |
| **Passwords** | ASP.NET Identity default (bcrypt/PBKDF2). Password complexity rules enforced. |
| **JWT** | Short-lived access tokens (15 min) + refresh tokens (7 days). Stored in httpOnly cookies. |
| **Sensitive Fields** | National ID, work permit, bank IBAN encrypted at rest (AES-256 via EF Core value converter). |
| **Rate Limiting** | ASP.NET Core rate limiting middleware on all public endpoints. |
| **CORS** | Whitelist specific frontend origins only. |
| **API Keys** | POS integration API keys hashed with SHA-256 before storage. |
| **Webhooks** | HMAC-SHA256 signature on all outbound webhook payloads. Inbound POS webhooks verified. |
| **Audit** | All data mutations logged. Audit logs are append-only. |
| **GDPR** | Data export endpoint, deletion request workflow, encrypted sensitive fields, access logging. |
| **Input Validation** | FluentValidation on all incoming DTOs. No raw user input in SQL queries. |
| **Authorization** | Role-based + permission-based. Every endpoint has explicit authorization attribute. |

---

## 9. Background Jobs

| Job | Schedule | Purpose |
|-----|----------|---------|
| **TimesheetAutoGeneration** | Daily at 02:00 | Generate timesheet entries for completed shifts |
| **LeaveBalanceAccrual** | Monthly on 1st | Add monthly accrual to leave balances |
| **LeaveBalanceCarryOver** | Yearly on Jan 1st | Carry over unused leave to new year |
| **CertificationExpiryChecker** | Daily at 08:00 | Check for expiring certifications, send alerts |
| **NoShowDetection** | Every 30 minutes | Detect employees who haven't clocked in for scheduled shifts |
| **OpenShiftExpiration** | Hourly | Mark unfilled open shifts as expired after their start time |
| **NotificationCleanup** | Weekly | Delete read notifications older than 90 days |
| **AuditLogArchival** | Monthly | Archive old audit logs to cold storage |
| **DataRetentionCleanup** | Monthly | Apply data retention policies (anonymize terminated employees after X years) |
| **DemandForecastGeneration** | Daily at 03:00 | Generate demand forecasts for next 14 days based on historical sales |
| **WeatherDataSync** | Every 6 hours | Fetch weather forecast for all locations (OpenWeatherMap) |
| **EngagementSurveyTrigger** | After each shift ends | Send post-shift survey to employees who just finished a shift |
| **FlightRiskAnalysis** | Weekly (Monday) | Analyze engagement metrics and flag at-risk employees |
| **MilestoneChecker** | Daily at 09:00 | Check for upcoming employee milestones (30/90/180/365 days) |
| **TaskAutoGeneration** | On schedule publish | Generate task instances from templates for all scheduled shifts |
| **OverdueTaskDetection** | Every 30 minutes | Flag tasks that passed their deadline without completion |
| **CompliancePremiumCalculation** | On schedule change | Calculate predictive scheduling premiums when published schedules change |
| **LogbookDigestEmail** | Daily at 23:00 | Send daily logbook digest to subscribed managers |
| **SPLHCalculation** | Daily at 02:30 | Calculate SPLH metrics for previous day once POS sales data is received |
| **ForecastAccuracyUpdate** | Daily at 04:00 | Compare yesterday's forecast with actual sales data |

**Implementation:** Hangfire with Redis storage (production) or SQL Server storage (development). Dashboard accessible at `/hangfire` (Admin only).

---

## 9A. Webhook Payload Schemas (POS Integration)

> Every outbound webhook is signed with HMAC-SHA256. The POS verifies the signature using the shared `WebhookSecret`. This prevents tampering — if the signature doesn't match, the POS should reject the payload.

### Signature Verification (POS side)

```
Header: X-StaffPro-Signature: sha256=abc123def456...
Header: X-StaffPro-Timestamp: 2026-02-19T14:30:00Z

Verification:
  1. Concatenate: timestamp + "." + raw JSON body
  2. Compute HMAC-SHA256 using shared secret
  3. Compare with X-StaffPro-Signature header
  4. Reject if timestamp is older than 5 minutes (replay attack prevention)
```

### Event: `schedule.published`

Sent when a schedule period is published. POS uses this to know who's working each day.

```json
{
  "event": "schedule.published",
  "timestamp": "2026-02-19T14:30:00Z",
  "organization_id": "f47ac10b-58cc-4372-a567-0e02b2c3d479",
  "location_id": "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
  "data": {
    "schedule_period_id": "550e8400-e29b-41d4-a716-446655440000",
    "start_date": "2026-02-17",
    "end_date": "2026-02-23",
    "published_by": "jean.manager@restaurant.ch",
    "total_assignments": 48,
    "assignments": [
      {
        "assignment_id": "6ba7b810-9dad-11d1-80b4-00c04fd430c8",
        "employee_id": "7c9e6679-7425-40de-944b-e07fc1f90ae7",
        "employee_name": "Marie Dubois",
        "employee_number": "EMP-012",
        "date": "2026-02-17",
        "start_time": "09:00",
        "end_time": "15:00",
        "role": "Waiter",
        "station": "Terrace",
        "break_minutes": 30
      }
    ]
  }
}
```

### Event: `employee.role_changed`

Sent when an employee's role changes. POS updates its permissions accordingly.

```json
{
  "event": "employee.role_changed",
  "timestamp": "2026-02-19T10:00:00Z",
  "organization_id": "f47ac10b-58cc-4372-a567-0e02b2c3d479",
  "data": {
    "employee_id": "7c9e6679-7425-40de-944b-e07fc1f90ae7",
    "employee_number": "EMP-012",
    "employee_name": "Marie Dubois",
    "previous_roles": ["Waiter"],
    "current_roles": ["Waiter", "Shift Leader"],
    "primary_role": "Shift Leader",
    "effective_date": "2026-02-19"
  }
}
```

### Event: `employee.terminated`

Sent when an employee is terminated. POS should immediately revoke access.

```json
{
  "event": "employee.terminated",
  "timestamp": "2026-02-19T16:00:00Z",
  "organization_id": "f47ac10b-58cc-4372-a567-0e02b2c3d479",
  "data": {
    "employee_id": "7c9e6679-7425-40de-944b-e07fc1f90ae7",
    "employee_number": "EMP-012",
    "employee_name": "Marie Dubois",
    "termination_date": "2026-02-19",
    "action_required": "REVOKE_POS_ACCESS"
  }
}
```

### Event: `shift_swap.approved`

Sent when a shift swap is approved, so POS knows the schedule has changed.

```json
{
  "event": "shift_swap.approved",
  "timestamp": "2026-02-19T11:30:00Z",
  "organization_id": "f47ac10b-58cc-4372-a567-0e02b2c3d479",
  "data": {
    "swap_id": "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
    "swap_type": "Swap",
    "date": "2026-02-21",
    "original_employee": {
      "id": "7c9e6679-7425-40de-944b-e07fc1f90ae7",
      "name": "Marie Dubois",
      "shift": "17:00-23:30"
    },
    "replacement_employee": {
      "id": "8d0f7680-8536-51e2-91c5-01d15ge2a1b8",
      "name": "Luca Rossi",
      "shift": "17:00-23:30"
    }
  }
}
```

### Inbound POS Clock Event: `POST /api/pos/v1/clock`

```json
{
  "employee_identifier": "EMP-012",
  "event_type": "clock_in",
  "timestamp": "2026-02-19T08:55:00Z",
  "pos_terminal_id": "terminal-01",
  "pos_terminal_name": "Front Register",
  "metadata": {
    "pos_name": "BonApp",
    "pos_version": "2.3.1"
  }
}
```

### Inbound POS Sales Summary: `POST /api/pos/v1/sales-summary`

```json
{
  "location_id": "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
  "date": "2026-02-19",
  "hour": 12,
  "sales_amount": 2550.00,
  "currency": "CHF",
  "covers": 52,
  "average_ticket": 49.04,
  "tips_collected": 180.00,
  "breakdown": {
    "food_sales": 1800.00,
    "beverage_sales": 550.00,
    "other_sales": 200.00
  }
}
```

---

## 9B. Swiss Labor Law Compliance Details

> Staff Pro is designed with Swiss labor law (ArG — Arbeitsgesetz / Loi sur le travail) as the default jurisdiction. This section documents the specific Swiss rules that the system enforces.

### Swiss Working Time Regulations (ArG)

| Rule | Legal Reference | Staff Pro Implementation |
|------|----------------|------------------------|
| **Maximum weekly hours** | ArG Art. 9: 45h for industrial workers, office staff, technical staff; 50h for all others | `SchedulingRule: MaxHoursPerWeek = 45 or 50` (configurable per contract type) |
| **Maximum daily hours** | ArG Art. 10: Generally 14 hours including breaks | `SchedulingRule: MaxHoursPerDay = 14` (hard constraint) |
| **Minimum rest between shifts** | ArG Art. 15a: 11 consecutive hours between two work periods | `SchedulingRule: MinRestBetweenShifts = 11` (hard constraint — Error level) |
| **Reduced rest (exception)** | ArG Art. 15a: Can be reduced to 8h once per week if 11h average maintained | `SchedulingRule: MinRestReduced = 8, MaxReducedRestPerWeek = 1` (Warning level) |
| **Weekly rest day** | ArG Art. 21: At least 1 full rest day per week (Sunday preferred) | `SchedulingRule: MaxConsecutiveDays = 6` (hard constraint) |
| **Night work** | ArG Art. 17: Work between 23:00–06:00 requires compensation (time or money) | Night hours tracked, 25% time supplement or premium pay |
| **Sunday work** | ArG Art. 19: Generally prohibited; hospitality sector has permanent exemption | Weekend hours tracked with premium multiplier |
| **Overtime** | ArG Art. 12-13: Max 170h/year (45h contracts) or 140h/year (50h contracts) | Yearly overtime cap tracked per employee |
| **Overtime compensation** | OR Art. 321c: 125% pay or equivalent time off | `Contract.OvertimeMultiplier = 1.25` (default) |
| **Break requirements** | ArG Art. 15: 15 min break if shift > 5.5h, 30 min if > 7h, 1h if > 9h | Auto-enforced in shift template validation |
| **Record keeping** | ArG Art. 46: Employer must record working hours for at least 5 years | `DataRetention: ClockEntries = 5 years` (minimum) |
| **Young workers (< 18)** | ArG Art. 29-32: Max 9h/day, no night work, no Sunday work | Additional scheduling rules auto-applied when DOB indicates < 18 |
| **Pregnant employees** | ArG Art. 35-35b: No night work, max 9h/day, no dangerous work | Special scheduling constraints when employee status = "Pregnant" |

### Swiss-Specific Break Rules (Automatically Enforced)

```
Shift Duration    → Required Break    → Staff Pro Enforcement
─────────────────────────────────────────────────────────────
≤ 5.5 hours       → No mandatory break → Break optional in ShiftTemplate
> 5.5 hours        → 15 minutes minimum → Warning if shift template has < 15 min break
> 7.0 hours        → 30 minutes minimum → Error if shift template has < 30 min break
> 9.0 hours        → 60 minutes minimum → Error if shift template has < 60 min break

Break placement: Must be roughly in the middle of the shift.
Staff Pro checks: if break starts before 1/3 of shift or after 2/3 → Warning.

Examples:
  Shift 09:00–14:30 (5.5h) → No break required
  Shift 09:00–17:00 (8.0h) → Minimum 30 min break → Template: 30 min at ~12:30
  Shift 09:00–20:00 (11.0h) → Minimum 60 min break → Template: 60 min at ~14:00
```

### Swiss Public Holidays (Canton-Specific)

```
Staff Pro ships with a holiday calendar per Swiss canton. Example (Zurich):

  National holidays (all cantons):
    - January 1 (New Year)
    - August 1 (Swiss National Day)
    - December 25 (Christmas Day)

  Zurich cantonal holidays:
    - January 2 (Berchtoldstag)
    - Good Friday
    - Easter Monday
    - May 1 (Labor Day)
    - Ascension Day
    - Whit Monday
    - December 26 (St. Stephen's Day)

  These are auto-loaded when organization selects "Switzerland" + canton.
  Holiday shifts trigger premium pay multiplier (configurable: 1.0×, 1.25×, 1.5×, 2.0×).
  Staffing requirements can be different on holidays (separate template).
```

---

## 10. Technology Stack Summary

| Category | Technology | Why |
|----------|-----------|-----|
| **Runtime** | .NET 8.0 | LTS, high performance |
| **Web Framework** | ASP.NET Core 8.0 (Controllers) | Battle-tested REST API |
| **Architecture Pattern** | CQRS + MediatR 12.x | Separates read/write, clean handlers, pipeline behaviors for validation/logging |
| **ORM** | Entity Framework Core 8.0 | Type-safe queries, migrations, interceptors |
| **Database** | SQL Server 2022 (Azure SQL in production) | Reliable, good EF Core support |
| **Cache** | Redis (StackExchange.Redis) | Distributed cache, SignalR backplane, rate limiting store |
| **Authentication** | ASP.NET Identity + JWT Bearer | Industry standard |
| **Validation** | FluentValidation (via MediatR pipeline behavior) | Declarative validation, auto-runs before handlers |
| **Mapping** | AutoMapper | Entity ↔ DTO conversion |
| **Real-Time** | SignalR (WebSocket) with Redis backplane | Notifications + chat + live schedule updates |
| **Background Jobs** | Hangfire + Dashboard | Recurring jobs, delayed jobs, retry logic, monitoring UI |
| **Reliable Events** | Outbox Pattern (Hangfire + MediatR) | Ensures external calls (email, webhook) complete even if request fails |
| **Email** | SendGrid | Transactional emails, announcement digests |
| **Push Notifications** | Firebase Cloud Messaging (FCM) | Web push for mobile browsers |
| **File Storage** | Azure Blob Storage | Employee docs, task proof photos, chat attachments |
| **PDF Generation** | QuestPDF | Modern, fluent API, no legacy dependencies |
| **CSV Export** | CsvHelper | Configurable column mapping for payroll export |
| **Weather API** | OpenWeatherMap (optional) | Demand forecasting input |
| **Logging** | Serilog + Application Insights | Structured logging, Azure monitoring |
| **Health Checks** | ASP.NET Core Health Checks (DB + Redis + external services) | Readiness/liveness probes |
| **API Documentation** | Swagger / OpenAPI (Swashbuckle) | Interactive API explorer |
| **Rate Limiting** | ASP.NET Core Rate Limiting + Redis | Protect public endpoints |
| **Testing** | xUnit + Moq + FluentAssertions + Testcontainers | Unit + integration tests |
| **Containerization** | Docker + docker-compose | Local dev + production |
| **CI/CD** | GitHub Actions | Automated build/test/deploy |

### Why CQRS + MediatR (Architecture Decision)

The original report used a traditional service-based pattern. Based on industry best practices for .NET scheduling systems, we've upgraded to CQRS + MediatR because:

1. **Separation of concerns** — Read operations (get schedule, get employees) are fundamentally different from write operations (create shift, approve leave). CQRS separates them into distinct handlers.
2. **Pipeline behaviors** — MediatR pipeline behaviors enable cross-cutting concerns (validation, logging, transaction management, audit logging) without cluttering business logic.
3. **Testability** — Each handler is a small, focused class that's easy to unit test.
4. **Outbox pattern** — MediatR + Hangfire outbox ensures reliable delivery of notifications, emails, and webhooks even when the external service is temporarily down.
5. **Real-world validation** — The HR Management System reference implementation on GitHub uses this exact pattern for leave management in ASP.NET Core.

```csharp
// Example: CQRS handler for creating a shift assignment
public record CreateShiftAssignmentCommand(Guid SchedulePeriodId, CreateAssignmentDto Dto) : IRequest<ShiftAssignmentDto>;

public class CreateShiftAssignmentHandler : IRequestHandler<CreateShiftAssignmentCommand, ShiftAssignmentDto>
{
    private readonly IConflictDetectionService _conflicts;
    private readonly IScheduleRepository _scheduleRepo;
    private readonly IMediator _mediator;

    public async Task<ShiftAssignmentDto> Handle(CreateShiftAssignmentCommand request, CancellationToken ct)
    {
        // 1. Run conflict detection
        var conflicts = await _conflicts.CheckAssignment(request.Dto);
        if (conflicts.HasErrors) throw new ConflictException(conflicts);

        // 2. Create assignment
        var assignment = _mapper.Map<ShiftAssignment>(request.Dto);
        await _scheduleRepo.AddAssignment(assignment);

        // 3. Publish domain event (handled by outbox → notifications, webhooks)
        await _mediator.Publish(new ShiftAssignedEvent(assignment.Id, assignment.EmployeeId));

        return _mapper.Map<ShiftAssignmentDto>(assignment);
    }
}
```

---

## 11. Priority Matrix — Core vs. Nice-to-Have

> **IMPORTANT:** Before looking at phase tables, understand which features are CORE (every competitor has them) vs. EXTENDED (only some competitors have them) vs. PREMIUM (separate product categories bolted on). This prevents building flashy features while core functionality is weak.

### Tier 1 — CORE (Every scheduling app has these. Without them, you don't have a product.)

| Feature | In Our App | Notes |
|---------|-----------|-------|
| Employee directory (CRUD) | ✅ 2.1 (P0) | Names, roles, contacts, status |
| Roles & stations | ✅ 2.2 (P0) | Define who can work where |
| Weekly availability | ✅ 2.3 (P0) | Employees set when they can work |
| Shift templates | ✅ 2.4 (P0) | Morning/Afternoon/Evening patterns |
| Schedule builder (drag-drop) | ✅ 2.4 (P0) | **THE core feature** — build weekly schedules |
| Conflict detection | ✅ 2.4 (P0) | Prevent double-booking, overtime, rest violations |
| Schedule publish + notifications | ✅ 2.4 (P0) | Employees see their shifts |
| Copy previous schedule | ✅ 2.4 (P0) | Saves time every week |
| Time-off requests + approval | ✅ 2.5 (P0) | Employees request, managers approve/deny |
| Leave balance tracking | ✅ 2.5 (P1) | How many vacation days left |
| Clock-in / clock-out | ✅ 2.7 (P0) | Record worked hours |
| Break tracking | ✅ 2.7 (P1) | Track meal/rest breaks |
| Timesheet summary + approval | ✅ 2.7 (P0) | Manager approves worked hours |
| Overtime calculation | ✅ 2.7 (P1) | Automatic daily/weekly OT |
| Payroll CSV export | ✅ 2.8 (P0) | Export hours for payroll software |
| In-app notifications | ✅ 2.10 (P0) | Bell icon, unread count |
| Audit trail | ✅ 2.11 (P0) | Who changed what, when |
| Staffing requirements | ✅ 2.3 (P0) | Min/max staff per shift |
| Permission system (RBAC) | ✅ 2.2 (P0) | Admin / Manager / Employee access |
| Announcements (one-way) | ✅ 2.12 (P0) | Manager broadcasts to team |

### Tier 2 — STANDARD (Most competitors have these. Expected by customers within 6 months of launch.)

| Feature | In Our App | Notes |
|---------|-----------|-------|
| Open shifts / shift marketplace | ✅ 2.4 (P1) | Post unfilled shifts, employees claim |
| Shift swaps | ✅ 2.6 (P1) | Employees swap shifts with each other |
| Team messaging (DMs + channels) | ✅ 2.12 (P1) | In-app chat (replaces WhatsApp groups) |
| Email notifications | ✅ 2.10 (P1) | Schedule published, shift changes |
| Geofencing clock-in | ✅ 2.17 (P1) | Verify employee is at restaurant |
| Kiosk mode (shared tablet) | ✅ 2.17 (P1) | PIN/QR clock at entrance |
| Task management (checklists) | ✅ 2.14 (P1) | Opening/closing checklists |
| Skill/certification tracking | ✅ 2.1 (P1) | Food safety cert, expiry alerts |
| Multi-location | ✅ 2.2 (P1) | Multiple restaurants |
| POS integration | ✅ 2.9 (P1) | Connect to any POS system |
| Schedule templates (full week) | ✅ 2.20 (P1) | Save/reuse weekly patterns |
| Labor cost reports | ✅ 2.8 (P1) | Cost by department, day, shift |
| Employee earnings estimate | ✅ 2.23 (P1) | Employees see projected pay |
| Push notifications | ✅ 2.10 (P2) | Alerts even when app is closed |
| Blackout dates | ✅ 2.5 (P1) | No leave allowed on certain dates |
| Scheduled vs. actual hours report | ✅ 2.8 (P1) | Variance analysis |

### Tier 3 — DIFFERENTIATOR (Some competitors have these. Gives competitive advantage.)

| Feature | In Our App | Notes |
|---------|-----------|-------|
| AI demand forecasting | ✅ 2.16 (P2) | Predict staffing needs from POS sales data |
| Auto-scheduling engine | ✅ 2.20 (P2) | One-click schedule generation |
| Tip management | ✅ 2.19 (P2) | Tip pool distribution |
| Predictive scheduling compliance | ✅ 2.18 (P2) | Fair Workweek law compliance |
| Manager logbook | ✅ 2.13 (P1) | Shift handover notes |
| Employee engagement (surveys) | ✅ 2.15 (P2) | Post-shift mood surveys |
| Photo verification clock | ✅ 2.17 (P2) | Selfie at clock-in |
| Labor cost % live tracker | ✅ 2.16 (P1) | Real-time labor % during scheduling |
| Employee recognition (shout-outs) | ✅ 2.15 (P2) | Public praise feed |
| Onboarding checklist | ✅ 2.1 (P2) | New hire setup steps |
| GDPR compliance tools | ✅ 2.11 (P1) | Data export/deletion |
| Minor work rules | ✅ 2.27 (P2) | Under-18 restrictions |
| Intraday labor dashboard | ✅ 2.28 (P2) | Live sales vs. labor |
| Earned vs. actual hours | ✅ 2.28 (P2) | Staffing efficiency metric |

### Tier 4 — PLATFORM EXTENSION (Only 1-2 competitors have these. Essentially separate products. Build ONLY after Tiers 1-3 are rock-solid.)

| Feature | In Our App | Risk if built too early |
|---------|-----------|----------------------|
| Hiring & ATS (2.21) | ✅ | **HIGH** — ATS is a full product (Greenhouse, Lever). Only Homebase has it. Building it takes focus away from core scheduling. |
| Training & LMS (2.22) | ✅ | **HIGH** — LMS is a full product (TalentLMS, Connecteam). Only Connecteam has it. 10 dev days is an underestimate. |
| Digital forms builder (2.24) | ✅ | **HIGH** — Form builders are low-code platforms (Typeform, JotForm). Only Connecteam has it. Very complex to build well. |
| Performance reviews (2.26) | ✅ | **MEDIUM** — HR software territory (Lattice, BambooHR). Only Push Operations has it among scheduling apps. |
| Company newsfeed (2.25) | ✅ | **LOW** — Simpler to build, but not expected by scheduling app users. Only Connecteam/Sling have it. |
| Knowledge base & handbook (2.22) | ✅ | **LOW** — Relatively simple (CRUD articles), but not expected. |
| Facial recognition clock | ✅ | **HIGH** — ML model integration is a specialized skill. |
| GPS live tracking & mileage (2.29) | ✅ | **MEDIUM** — Only relevant for delivery/catering, not core restaurant scheduling. |
| Shift bidding (2.20) | ✅ | **LOW** — Nice evolution of open shifts, builds on existing features. |
| What-if schedule scenarios (2.20) | ✅ | **MEDIUM** — Requires auto-scheduling to work first. |

---

## 12. Phased Delivery Plan

### Phase 1 — MVP (Core Scheduling)

> **Goal:** A restaurant can manage staff, build schedules, track time, and export payroll. This alone replaces spreadsheets and paper schedules.

| Module | Included in MVP |
|--------|----------------|
| Authentication & User Management | Yes |
| Organization & Location Setup | Yes (single location) |
| Roles & Stations | Yes |
| Employee Directory (CRUD) | Yes |
| Contracts | Yes |
| Multi-Role Assignment | Yes |
| Recurring Availability | Yes |
| Availability Overrides | Yes |
| Shift Templates | Yes |
| Schedule Period (Draft/Publish/Lock) | Yes |
| Shift Assignment with Conflict Detection | Yes |
| Copy Previous Schedule | Yes |
| Staffing Requirements | Yes |
| Time-Off Requests & Approvals | Yes |
| Leave Types & Balance Tracking | Yes |
| Clock-In / Clock-Out | Yes |
| Break Tracking | Yes |
| Timesheet Summary | Yes |
| Timesheet Approval | Yes |
| Overtime Calculation | Yes |
| Payroll CSV Export | Yes |
| In-App Notifications | Yes |
| Audit Trail | Yes |
| Announcements (one-way broadcast with read receipts) | Yes |
| **NOT in MVP** | |
| Team Messaging (DMs + Channels) | Phase 2 *(Announcements are sufficient for MVP)* |

### Phase 2 — Full Product (Features Every Competitor Has)

> **Goal:** Match feature parity with 7shifts, Deputy, When I Work. A customer choosing between Staff Pro and competitors should not find a missing feature.

| Module | Notes |
|--------|-------|
| Team Messaging (DMs + Channels) | Moved from MVP — build on existing SignalR infrastructure |
| Open Shifts / Shift Marketplace | Core feature — every competitor has this |
| Shift Swaps (swap, cover, drop) | Core feature — every competitor has this |
| Geofencing Clock-In | Industry standard — Deputy, Homebase, Connecteam |
| Kiosk Mode (shared tablet, PIN/QR) | Homebase, When I Work, Connecteam |
| Task Management (checklists + proof) | 7shifts, Deputy, Connecteam |
| Manager Logbook | 7shifts — highly requested |
| Multi-Location | Required for chains |
| POS Integration (clock sync, sales data, webhooks) | Connects to BonApp, Toast, Square, etc. |
| Email + Push Notifications | Expected in any modern app |
| Skill/Certification Tracking | Food safety cert expiry alerts |
| Schedule Templates (full week save/load) | When I Work, 7shifts |
| Employee Earnings Estimate + Hours Tracker | 7shifts — employees see projected pay |
| Advanced Reports (labor cost, attendance, scheduled vs. actual) | Basic analytics |
| Blackout Dates | Standard leave management feature |
| Document Storage + Onboarding Checklist | Basic HR features |
| Offline Support (PWA) | Deputy's key differentiator |

### Phase 3 — Advanced Features (Competitive Differentiators)

> **Goal:** Features that set Staff Pro apart from competitors. Build only after Phase 2 is stable and customers are happy.

| Module | Notes |
|--------|-------|
| AI Demand Forecasting + SPLH Tracking | CrunchTime/Lineup.ai — requires 4+ weeks of POS data |
| Auto-Scheduling Engine (constraint solver) | 7shifts/Deputy — one-click schedule generation |
| Labor Cost % Live Tracker | Real-time % during schedule building |
| Tip Management (pool config, distribution) | 7shifts — eliminates spreadsheet tips |
| Predictive Scheduling Compliance | Fair Workweek laws (NYC, Chicago, Swiss, EU) |
| Employee Engagement (surveys + recognition + milestones) | 7shifts + Harri — retention tools |
| Intraday Labor Dashboard (live sales vs. labor) | Restaurant365 — requires stable POS integration |
| Earned vs. Actual Hours | 7shifts efficiency metric |
| Photo Verification Clock (selfie) | Deputy — anti-fraud |
| GDPR Compliance Tools | Data export + deletion |
| Minor Work Rules & Age Restrictions | Under-18 compliance |
| Custom CSV Export Mapping | Payroll software compatibility |

### Phase 4 — Platform Extensions (Separate Product Categories)

> **Goal:** Transform Staff Pro from a scheduling app into a full workforce management platform. Build ONLY when the core product is mature, profitable, and customers are requesting these specific features. Each of these is essentially a mini-product.

| Module | Complexity | Build only if... |
|--------|-----------|-----------------|
| Hiring & Applicant Tracking (ATS) | HIGH | Customers ask "can I post jobs from here?" — until then, they use Indeed/LinkedIn |
| Training & LMS (courses, quizzes) | HIGH | Customers ask "can I train staff here?" — until then, they use separate training tools |
| Digital Forms Builder | HIGH | Customers ask "can I replace our paper forms?" — task management covers 80% of this need |
| Performance Reviews | MEDIUM | Customers ask "can I do reviews here?" — employee notes cover 80% of this need |
| Knowledge Base & Employee Handbook | LOW | Customers ask "where do staff find our policies?" — announcements cover basic needs |
| Company Newsfeed / Social Feed | LOW | Customers ask "can we have a team feed?" — chat + announcements cover basic needs |
| Shift Bidding & Preferences | LOW | Natural evolution of open shifts — build when auto-scheduling is ready |
| Facial Recognition Clock (AI) | HIGH | Only build if photo verification clock shows demand |
| GPS Live Map & Route History | MEDIUM | Only relevant for delivery/catering businesses |
| Mileage Tracking & Reimbursement | LOW | Extension of GPS tracking |
| AI Candidate Screening | MEDIUM | Extension of hiring module |
| What-If Schedule Scenarios | LOW | Extension of auto-scheduling |
| Base & Flex Scheduling | MEDIUM | Advanced scheduling pattern — needs forecasting first |

---

## 13. Estimated Effort

### Phase 1 — MVP

| Module | Estimated Days | Notes |
|--------|---------------|-------|
| Project Setup (solution, Docker, CI/CD, DB, Redis) | 4 | Foundation + CQRS/MediatR setup |
| Auth + User Management | 5 | Identity, JWT, invite flow |
| Organization + Location + Department | 3 | CRUD + tenant setup |
| Roles + Stations + Permissions | 4 | RBAC implementation |
| Employee CRUD + Contracts | 5 | Core HR module |
| Availability (recurring + overrides) | 4 | Calendar logic |
| Shift Templates + Staffing Requirements | 3 | Schedule setup |
| Schedule Period + Shift Assignments | 6 | Core scheduling engine |
| Conflict Detection Engine | 8 | **Most complex** — 14 checks, extensive testing |
| Copy Previous Schedule | 2 | Clone + re-validate |
| Time-Off Requests + Approvals | 5 | Leave management |
| Leave Balance Tracking | 3 | Accrual + deductions |
| Clock-In/Out + Break Tracking | 5 | With rounding + grace rules + breaks |
| Timesheet Generation + Approval | 6 | Overtime calculation, approval flow |
| Payroll Summary + CSV Export | 4 | Report generation |
| In-App Notifications (SignalR) | 4 | Real-time delivery |
| Announcements (broadcast + read receipts) | 3 | One-way messaging |
| Audit Trail | 3 | EF interceptor |
| Background Jobs (Hangfire) | 3 | 5-6 scheduled jobs |
| Unit Tests | 8 | Especially conflict detection |
| Integration Tests | 5 | API-level tests |
| **Total MVP** | **~93 days** | **~4 months with 1 developer** |

### Phase 2 — Full Product

| Module | Estimated Days | Notes |
|--------|---------------|-------|
| Team Messaging (DMs + Channels) | 6 | Chat infrastructure, SignalR |
| Open Shifts + Shift Swaps | 5 | Marketplace features |
| Geofencing + Anti-Fraud + Kiosk | 8 | Zones, GPS, PIN, QR, alerts |
| Task Management | 8 | Templates, instances, proof upload, dashboard |
| Manager Logbook | 4 | CRUD + search + auto-metrics |
| Multi-Location | 5 | Cross-location scheduling |
| POS Integration (full) | 6 | Webhooks, clock sync, sales data |
| Email + Push Notifications | 4 | SendGrid + FCM |
| Skill/Certification Tracking | 3 | CRUD + expiry alerts |
| Schedule Templates (full week) | 4 | Template CRUD + apply-to-period |
| Employee Earnings + Self-Service | 4 | Estimate, hours tracker, profile |
| Advanced Reports | 5 | Labor cost, attendance, scheduled vs. actual |
| Blackout Dates + Document Storage + Onboarding | 5 | Basic HR features |
| Offline Support (PWA) | 4 | Workbox + IndexedDB queue |
| **Total Phase 2** | **~71 days** | **~3 months with 1 developer** |

### Phase 3 — Advanced Features

| Module | Estimated Days | Notes |
|--------|---------------|-------|
| AI Demand Forecasting + SPLH | 10 | ML model + weather + events |
| Auto-Scheduling Engine | 10 | Constraint solver, fairness, demand-driven |
| Labor Cost % Live Tracker | 3 | Real-time gauge on schedule builder |
| Tip Management | 5 | Pool config, distribution, reports |
| Predictive Scheduling Compliance | 6 | Jurisdiction presets, premium calculator |
| Employee Engagement (Surveys + Recognition) | 6 | Survey system + recognition feed |
| Intraday Labor Dashboard | 5 | Real-time labor vs. sales, earned vs. actual |
| Photo Verification (Selfie Clock) | 4 | Camera capture + storage |
| GDPR Compliance Tools | 3 | Export + deletion |
| Minor Work Rules | 3 | Age-based constraints |
| Earned vs. Actual Hours | 3 | Efficiency analytics |
| **Total Phase 3** | **~58 days** | **~2.5 months with 1 developer** |

### Phase 4 — Platform Extensions (build on demand)

| Module | Estimated Days | Notes |
|--------|---------------|-------|
| Hiring & ATS | 10 | Full pipeline — only if customers request |
| Training & LMS | 10 | Course builder + quiz engine — only if customers request |
| Digital Forms Builder | 8 | Complex — only if task management isn't enough |
| Performance Reviews | 8 | HR territory — only if customers request |
| Knowledge Base & Handbook | 5 | Articles + acknowledgements |
| Company Newsfeed | 5 | Social feed — only if chat + announcements aren't enough |
| Enhanced Onboarding (self-service) | 5 | Upgrade from basic checklist |
| Shift Bidding & Preferences | 4 | After auto-scheduling is built |
| Facial Recognition Clock | 6 | After photo verification proves demand |
| GPS Live Map & Route History | 5 | Only for delivery/catering |
| Mileage Tracking | 3 | Extension of GPS |
| AI Candidate Screening | 4 | Extension of hiring |
| What-If Schedule Scenarios | 4 | Extension of auto-scheduling |
| Base & Flex Scheduling | 5 | After forecasting is built |
| **Total Phase 4** | **~87 days** | **~4 months with 1 developer (not all needed)** |

---

*This report serves as the complete backend specification for Staff Pro. It should be used as the primary reference during development.*
