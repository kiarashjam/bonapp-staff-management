# Staff Pro - Frontend Development Report

**Generated:** February 9, 2026 (Updated with Industry UX Research)  
**Project:** Staff Pro — Standalone Restaurant Staff Management & Scheduling System  
**Framework:** React 18 | Vite 5 | TypeScript  
**Architecture:** Standalone React application (NOT part of BonApp monorepo)  
**Repository:** `staff-frontend` (separate Git repository)  
**Status:** Planning Phase

---

## 0. UX Research & Design Inspiration

> This section is based on research of leading scheduling UIs as of early 2026: **7shifts** (rated 4.7/5 for usability), **Deputy** (mobile-first pioneer), **When I Work** (simplicity benchmark), **Shyft** (drag-drop UX research), and **Eleken** (calendar UI best practices).

### Key UX Findings

| Principle | Research Finding | Staff Pro Implementation |
|-----------|-----------------|------------------------|
| **Mobile-first is non-negotiable** | 27% of the workforce is Gen Z; they expect schedule management from their phones. Decentralized workforces demand interfaces that work on any device. | Every feature has a mobile view. Clock page is designed phone-first. |
| **Minimize taps for common actions** | Shyft research shows top scheduling UIs require <3 taps for the most common action (viewing "my next shift"). | Employee dashboard shows next shift immediately. Clock-in is 1 tap. |
| **Visual clarity over information density** | Role-appropriate views are essential — managers see staffing patterns, employees see personal schedules (Shyft, 2025). | Separate dashboard for Manager vs. Employee. Schedule builder shows role-coded colors. |
| **Drag-and-drop must feel native** | Touch-optimized drag with immediate visual feedback, consistent behavior, and natural interactions (Shyft drag-drop research). | @dnd-kit with custom touch sensors, haptic feedback on mobile, visual drag preview. |
| **Keyboard shortcuts for power users** | Expert managers build schedules faster with keyboard shortcuts than drag-drop (Eleken calendar research). | Full keyboard navigation: arrow keys, Enter to assign, Esc to cancel, Ctrl+C/V for copy-paste shifts. |
| **Real-time feedback prevents errors** | Users need instant conflict feedback during drag operations, not after save (Shyft). | Conflict detection runs on drag-hover (before drop), showing red/yellow borders in real-time. |
| **Offline must be invisible** | Deputy's offline clock shows "Pending sync" with the original timestamp, so employees trust the system even without WiFi. | Offline actions queued with local timestamp, subtle banner, auto-sync when online. |

### Competitive UI Features Staff Pro Must Match

| Feature | Who does it best | Staff Pro approach |
|---------|-----------------|-------------------|
| Schedule builder grid | 7shifts | Weekly grid + employee sidebar, drag-drop, staffing coverage bars |
| **Auto-scheduling wizard** | 7shifts + Deputy | One-click generate → preview → adjust → apply, strategy picker |
| **Schedule template library** | When I Work | Save/load full weekly templates with named patterns |
| Mobile schedule view | When I Work | Day-by-day cards, swipe between days, pull-to-refresh |
| Team messaging | 7shifts | In-app DMs + channels + announcements with read receipts |
| **Company newsfeed** | Connecteam | Social feed with reactions, comments, pins, milestones |
| Manager logbook | 7shifts | Daily log form + auto-populated metrics from POS |
| Task management | 7shifts | Shift-linked checklists with photo proof |
| **Digital forms** | Connecteam | Drag-and-drop form builder with photo, signature, GPS fields |
| Recognition feed | 7shifts | Social feed with categories + emoji reactions |
| Geofencing map | Deputy | Interactive map for setting geofence radius around locations |
| **GPS live employee map** | Connecteam | Real-time map of clocked-in employees (delivery/catering) |
| Demand forecast overlay | CrunchTime | Forecast bar chart overlaid on schedule builder |
| Labor cost % tracker | HotSchedules | Real-time % gauge during schedule building |
| **Intraday labor dashboard** | Restaurant365 | Live sales vs. labor cost, hourly breakdown, earned vs. actual |
| Kiosk clock mode | Homebase | Full-screen, locked-down, PIN/QR entry |
| **Hiring pipeline** | Homebase | Kanban board for applicant tracking + interview scheduling |
| **Training / LMS** | Connecteam | Course viewer, quiz pages, progress tracking, knowledge base |
| **Employee earnings** | 7shifts | Projected pay for upcoming schedule + historical earnings |
| **Performance reviews** | Push Operations | Self-assessment + manager review + data-driven metrics |
| **Employee onboarding** | 7shifts | Step-by-step self-service onboarding with progress tracker |

---

## 1. Executive Summary

This report defines the complete frontend development plan for **Staff Pro**, a fully **autonomous, standalone** restaurant staff management and scheduling frontend. This is a **completely independent application** — it has its own codebase, dependencies, design system, authentication UI, and deployment. It does NOT live inside the BonApp frontend monorepo.

The application is designed for **any restaurant** regardless of their POS system:
- Restaurants using **BonApp POS** (future integration via API)
- Restaurants using **Lightspeed, Toast, Square, Orderbird**
- Restaurants using **any POS system** with API/webhook support
- Restaurants with **no POS system** at all (standalone staff management)

### Architectural Independence

- **Own Git repository** — `staff-frontend/` is a standalone project
- **Own design system** — Based on Ant Design 5, NOT `@repo/ui`
- **Own authentication** — Login, register, forgot password (talks to own Staff Pro backend)
- **Own deployment** — Separate Docker build, separate CDN, separate CI/CD pipeline
- **No shared code** — No `@repo/ui`, `@repo/configs`, or `@repo/eslint-config` dependencies
- **No cross-app navigation** — No links to/from BonApp Manager, Waiter, or Customer apps

### Design Principles

- **Mobile-first responsive design** — Managers often check schedules on phones; employees clock in/out and check shifts on mobile devices in the restaurant. 27% of the workforce is Gen Z and expects full mobile access.
- **Offline-capable** — Clock-in/out must work even with spotty WiFi (queue actions and sync when online). Actions are queued with local timestamps and auto-synced.
- **Fast interaction patterns** — Drag-and-drop scheduling, quick shift templates, keyboard shortcuts for power users. Common actions take <3 taps on mobile.
- **Visual clarity** — Color-coded roles, shifts, and departments make the schedule readable at a glance. Role-appropriate views (managers see patterns, employees see personal schedules).
- **Progressive disclosure** — Simple views by default, detailed views on demand (don't overwhelm casual users)
- **Real-time updates** — WebSocket connection for live schedule changes, notifications, chat, and clock status
- **Instant conflict feedback** — Conflict detection runs during drag (before drop), not after save
- **Own design system** — Ant Design 5 as base, with custom theme tokens for scheduling-specific colors and components

---

## 2. Application Configuration

### Standalone Project Setup

| Setting | Value |
|---------|-------|
| **App Name** | `staff-pro` |
| **Location** | `staff-frontend/` (own Git repository) |
| **Dev Port** | 3100 |
| **Base Path** | `/` |
| **Language** | TypeScript (.tsx) |
| **React Version** | ^18.3.1 |
| **Vite Version** | ^5.3.1 |
| **UI Library** | Ant Design 5 (own theme — NOT @repo/ui) |
| **State Management** | Redux Toolkit + RTK Query |
| **Redux Persist** | Yes (auth, settings, ui, offline-queue) |
| **Router** | React Router ^7.x |
| **Forms** | React Hook Form + Zod (modern alternative to Formik + Yup) |
| **i18n Fallback** | English ("en") |
| **Charts** | Chart.js ^4.x + react-chartjs-2 (for dashboards/reports) |
| **Tables** | Ant Design Table + @tanstack/react-table for complex data grids |
| **Date Handling** | dayjs (Ant Design 5 uses dayjs natively) |
| **Animation** | Framer Motion |
| **Drag & Drop** | @dnd-kit/core + @dnd-kit/sortable (for schedule builder) |
| **Calendar** | @schedule-x/react (schedule calendar) or FullCalendar (alternative) |
| **PDF Export** | html2pdf.js |
| **CSV/Excel** | SheetJS (xlsx) for import/export |
| **Notifications Toast** | react-hot-toast |
| **Real-Time** | SignalR client (@microsoft/signalr) — notifications + chat |
| **PWA** | Workbox (for offline clock-in/out) |
| **Maps** | react-leaflet + leaflet (for geofence map) or @react-google-maps/api |
| **Camera** | react-webcam (for task proof photos + selfie clock) |
| **QR Code** | react-qr-reader (for kiosk QR scan) |
| **Haptics** | Navigator.vibrate API (for mobile drag-drop feedback) |

### Package Manager

```bash
# Standalone project — uses pnpm directly (no monorepo)
pnpm create vite@latest staff-frontend -- --template react-ts
cd staff-frontend
pnpm install
```

### Environment Variables

```env
VITE_API_BASE_URL=http://localhost:5200/api
VITE_SIGNALR_HUB_URL=http://localhost:5200/hubs/notifications
VITE_APP_NAME=Staff Pro
VITE_APP_VERSION=1.0.0
VITE_ENABLE_MOCK=false
VITE_SENTRY_DSN=
VITE_GOOGLE_MAPS_KEY=                     # optional: for GPS-based clock verification
VITE_FCM_VAPID_KEY=                       # for push notifications
VITE_SIGNALR_CHAT_HUB_URL=http://localhost:5200/hubs/chat
VITE_GEOFENCE_DEFAULT_RADIUS=100          # meters
VITE_KIOSK_TIMEOUT=10000                  # ms before kiosk returns to PIN screen
```

### TypeScript Configuration

```json
{
  "compilerOptions": {
    "target": "ES2020",
    "lib": ["ES2020", "DOM", "DOM.Iterable"],
    "module": "ESNext",
    "strict": true,
    "noUnusedLocals": true,
    "noUnusedParameters": true,
    "noFallthroughCasesInSwitch": true,
    "jsx": "react-jsx",
    "moduleResolution": "bundler",
    "allowImportingTsExtensions": true,
    "resolveJsonModule": true,
    "isolatedModules": true,
    "noEmit": true,
    "baseUrl": ".",
    "paths": {
      "@/*": ["src/*"],
      "@components/*": ["src/components/*"],
      "@pages/*": ["src/pages/*"],
      "@hooks/*": ["src/hooks/*"],
      "@store/*": ["src/store/*"],
      "@api/*": ["src/api/*"],
      "@utils/*": ["src/utils/*"],
      "@types/*": ["src/types/*"],
      "@assets/*": ["src/assets/*"]
    }
  }
}
```

---

## 3. Project Structure

```
staff-frontend/
├── public/
│   ├── favicon.ico
│   ├── logo192.png
│   ├── logo512.png
│   ├── manifest.json                          # PWA manifest
│   └── service-worker.js                      # Workbox service worker
│
├── src/
│   ├── main.tsx                               # App entry point
│   ├── App.tsx                                # Root component with providers
│   ├── vite-env.d.ts
│   │
│   ├── api/                                   # RTK Query API definitions
│   │   ├── baseApi.ts                         # createApi with baseQuery (JWT auto-attach)
│   │   ├── authApi.ts                         # login, register, refresh, invite
│   │   ├── employeesApi.ts                    # employee CRUD
│   │   ├── schedulesApi.ts                    # schedules + shift assignments
│   │   ├── availabilityApi.ts                 # availability rules + overrides
│   │   ├── timeOffApi.ts                      # time-off requests + leave types
│   │   ├── clockApi.ts                        # clock-in/out
│   │   ├── timesheetsApi.ts                   # timesheet periods + entries
│   │   ├── shiftSwapsApi.ts                   # swap, cover, drop requests
│   │   ├── openShiftsApi.ts                   # open shifts + claims
│   │   ├── rolesApi.ts                        # roles CRUD
│   │   ├── stationsApi.ts                     # stations CRUD
│   │   ├── organizationApi.ts                 # org settings + locations
│   │   ├── reportsApi.ts                      # report endpoints
│   │   ├── notificationsApi.ts                # notifications
│   │   ├── auditApi.ts                        # audit logs
│   │   ├── posApi.ts                          # POS connections
│   │   ├── chatApi.ts                         # team messaging + announcements
│   │   ├── logbookApi.ts                      # manager logbook entries
│   │   ├── tasksApi.ts                        # task templates + instances
│   │   ├── engagementApi.ts                   # surveys + recognition
│   │   ├── forecastApi.ts                     # demand forecasting + SPLH
│   │   ├── geofenceApi.ts                     # geofence zones
│   │   ├── tipsApi.ts                         # tip management
│   │   ├── complianceApi.ts                   # predictive scheduling compliance
│   │   ├── hiringApi.ts                       # job postings, applications, interviews, ATS
│   │   ├── trainingApi.ts                     # courses, modules, quizzes, assignments
│   │   ├── knowledgeBaseApi.ts                # articles, handbook, acknowledgements
│   │   ├── formsApi.ts                        # form templates, fields, submissions
│   │   ├── feedApi.ts                         # newsfeed posts, reactions, comments
│   │   ├── reviewsApi.ts                      # performance reviews, templates, goals
│   │   ├── autoScheduleApi.ts                 # auto-scheduling, schedule templates, bids
│   │   ├── selfServiceApi.ts                  # earnings estimate, hours tracker, onboarding
│   │   ├── intradayApi.ts                     # real-time labor dashboard data
│   │   └── gpsApi.ts                          # live map, route trails, mileage
│   │
│   ├── store/                                 # Redux Toolkit store
│   │   ├── index.ts                           # configureStore
│   │   ├── rootReducer.ts
│   │   ├── slices/
│   │   │   ├── authSlice.ts                   # user session, tokens, org context
│   │   │   ├── uiSlice.ts                     # sidebar collapsed, theme, locale
│   │   │   ├── scheduleBuilderSlice.ts        # drag state, selected period, filters
│   │   │   ├── clockSlice.ts                  # current clock status
│   │   │   ├── notificationSlice.ts           # unread count, toast queue
│   │   │   └── offlineQueueSlice.ts           # queued actions for offline mode
│   │   └── middleware/
│   │       ├── signalrMiddleware.ts           # SignalR connection + event handling
│   │       └── offlineMiddleware.ts           # queue actions when offline, replay when online
│   │
│   ├── hooks/                                 # Custom React hooks
│   │   ├── useAuth.ts                         # login, logout, isAuthenticated, hasPermission
│   │   ├── useCurrentUser.ts                  # current user data + employee info
│   │   ├── usePermissions.ts                  # permission checks: can('CreateSchedule')
│   │   ├── useSignalR.ts                      # SignalR connection hook
│   │   ├── useNotifications.ts                # notification subscription + actions
│   │   ├── useScheduleBuilder.ts              # schedule drag-drop state management
│   │   ├── useClock.ts                        # clock-in/out actions + current status
│   │   ├── useOffline.ts                      # offline detection + queue status
│   │   ├── useDebounce.ts                     # debounce for search inputs
│   │   ├── useMediaQuery.ts                   # responsive breakpoint detection
│   │   ├── useKeyboardShortcut.ts             # keyboard shortcut registration
│   │   ├── useLocalStorage.ts                 # typed localStorage hook
│   │   ├── useAutoSchedule.ts                 # auto-schedule wizard state management
│   │   ├── useHiringPipeline.ts               # ATS drag-drop pipeline state
│   │   ├── useTrainingProgress.ts             # course progress + quiz state
│   │   ├── useFormBuilder.ts                  # form builder drag-drop + field state
│   │   ├── useFeed.ts                         # newsfeed infinite scroll + reactions
│   │   ├── useEarnings.ts                     # earnings calculation + trends
│   │   └── useIntradayDashboard.ts            # real-time polling for labor data
│   │
│   ├── components/                            # Shared/reusable components
│   │   ├── layout/
│   │   │   ├── AppLayout.tsx                  # Main layout: sidebar + header + content
│   │   │   ├── Sidebar.tsx                    # Navigation sidebar (collapsible)
│   │   │   ├── Header.tsx                     # Top bar: user menu, notifications, clock status
│   │   │   ├── Breadcrumb.tsx
│   │   │   ├── MobileBottomNav.tsx            # Bottom navigation for mobile
│   │   │   └── PageHeader.tsx                 # Page title + action buttons
│   │   │
│   │   ├── auth/
│   │   │   ├── ProtectedRoute.tsx             # Route guard (redirect to login if unauthenticated)
│   │   │   ├── PermissionGate.tsx             # Show/hide based on permission
│   │   │   └── RoleGate.tsx                   # Show/hide based on role
│   │   │
│   │   ├── common/
│   │   │   ├── DataTable.tsx                  # Generic data table with sorting, filtering, pagination
│   │   │   ├── SearchInput.tsx                # Debounced search input
│   │   │   ├── StatusBadge.tsx                # Colored status badges (Active, Pending, etc.)
│   │   │   ├── ConfirmModal.tsx               # Reusable confirmation dialog
│   │   │   ├── EmptyState.tsx                 # Empty state illustration + CTA
│   │   │   ├── LoadingSkeleton.tsx            # Skeleton loading states
│   │   │   ├── ErrorBoundary.tsx              # React error boundary
│   │   │   ├── OfflineBanner.tsx              # "You are offline" banner
│   │   │   ├── AvatarGroup.tsx                # Overlapping avatar group
│   │   │   ├── ColorPicker.tsx                # For role/shift template colors
│   │   │   ├── DateRangePicker.tsx            # Unified date range picker
│   │   │   └── FileUpload.tsx                 # Drag-drop file upload
│   │   │
│   │   ├── schedule/
│   │   │   ├── ScheduleCalendar.tsx           # Main schedule grid (weekly view)
│   │   │   ├── ScheduleDayColumn.tsx          # Single day column in grid
│   │   │   ├── ShiftCard.tsx                  # Draggable shift card (employee + times + role)
│   │   │   ├── ShiftAssignmentModal.tsx       # Create/edit shift assignment form
│   │   │   ├── ConflictAlert.tsx              # Display conflict errors/warnings
│   │   │   ├── StaffingCoverageBar.tsx        # Visual bar showing filled vs. required
│   │   │   ├── ShiftTemplateSelector.tsx      # Quick template selection dropdown
│   │   │   ├── EmployeeSidebar.tsx            # Left sidebar with employee list (drag source)
│   │   │   ├── ScheduleToolbar.tsx            # Week navigation, view toggle, publish button
│   │   │   ├── ScheduleFilterBar.tsx          # Filter by role, department, station
│   │   │   ├── LaborCostSummary.tsx           # Projected cost footer
│   │   │   ├── PrintScheduleView.tsx          # Print-friendly layout
│   │   │   └── MobileScheduleView.tsx         # Mobile-optimized list view
│   │   │
│   │   ├── timeoff/
│   │   │   ├── TimeOffRequestForm.tsx         # Request submission form
│   │   │   ├── TimeOffRequestCard.tsx         # Request card with status
│   │   │   ├── LeaveBalanceWidget.tsx         # Balance display (entitled/used/remaining)
│   │   │   ├── TeamCalendar.tsx               # Calendar showing team leave
│   │   │   ├── StaffingImpactPreview.tsx      # Impact analysis before approval
│   │   │   └── BlackoutDatesBadge.tsx         # Visual indicator for blackout dates
│   │   │
│   │   ├── clock/
│   │   │   ├── ClockWidget.tsx                # Big clock-in/out button (mobile-optimized)
│   │   │   ├── ClockStatusIndicator.tsx       # Header indicator (clocked in, on break, etc.)
│   │   │   ├── ClockHistory.tsx               # Today's clock entries
│   │   │   └── ClockPinPad.tsx                # Optional PIN entry for shared devices
│   │   │
│   │   ├── timesheet/
│   │   │   ├── TimesheetTable.tsx             # Weekly timesheet grid
│   │   │   ├── TimesheetEntryRow.tsx          # Single entry with editable hours
│   │   │   ├── TimesheetApprovalActions.tsx   # Approve/dispute buttons
│   │   │   ├── TimesheetSummaryCard.tsx       # Period totals summary
│   │   │   └── OvertimeHighlight.tsx          # Visual overtime indicator
│   │   │
│   │   ├── employee/
│   │   │   ├── EmployeeCard.tsx               # Employee summary card (photo, roles, status)
│   │   │   ├── EmployeeForm.tsx               # Full employee create/edit form
│   │   │   ├── ContractForm.tsx               # Contract creation/edit form
│   │   │   ├── RoleAssignmentForm.tsx         # Assign/remove roles with proficiency
│   │   │   ├── SkillCertificationForm.tsx     # Add/edit skills and certs
│   │   │   ├── AvailabilityEditor.tsx         # Weekly availability pattern editor
│   │   │   ├── AvailabilityGrid.tsx           # Visual weekly grid (time slots)
│   │   │   ├── EmployeeTimeline.tsx           # History timeline (notes, events)
│   │   │   └── OnboardingChecklist.tsx        # Checklist with progress
│   │   │
│   │   ├── notifications/
│   │   │   ├── NotificationBell.tsx           # Header bell with unread count
│   │   │   ├── NotificationDropdown.tsx       # Dropdown list of recent notifications
│   │   │   ├── NotificationItem.tsx           # Single notification row
│   │   │   └── NotificationPreferencesForm.tsx
│   │   │
│   │   ├── reports/
│   │   │   ├── PayrollSummaryTable.tsx        # Payroll data grid
│   │   │   ├── LaborCostChart.tsx             # Chart.js labor cost visualization
│   │   │   ├── AttendanceChart.tsx            # Attendance/no-show chart
│   │   │   ├── OvertimeChart.tsx              # Overtime trends chart
│   │   │   ├── ScheduledVsActualChart.tsx     # Comparison chart
│   │   │   ├── LeaveUsageChart.tsx            # Leave type breakdown
│   │   │   ├── CertificationExpiryList.tsx    # Upcoming expirations
│   │   │   └── ExportButton.tsx               # CSV/PDF export trigger
│   │   │
│   │   ├── chat/                               # Team Communication (inspired by 7shifts)
│   │   │   ├── ChatSidebar.tsx                # Channel/DM list with unread counts
│   │   │   ├── ChatWindow.tsx                 # Message thread with auto-scroll
│   │   │   ├── ChatMessage.tsx                # Single message bubble with timestamp
│   │   │   ├── ChatInput.tsx                  # Message input with file attachment
│   │   │   ├── ChannelHeader.tsx              # Channel name, members count, settings
│   │   │   ├── ChannelCreateModal.tsx         # Create new channel form
│   │   │   ├── AnnouncementCard.tsx           # One-way announcement with read status
│   │   │   ├── AnnouncementCreateForm.tsx     # Broadcast to all/dept/role
│   │   │   ├── ReadReceiptList.tsx            # Who has read (for managers)
│   │   │   └── UnreadBadge.tsx                # Unread count badge component
│   │   │
│   │   ├── logbook/                            # Manager Logbook (inspired by 7shifts)
│   │   │   ├── LogEntryForm.tsx               # Create/edit log entry form
│   │   │   ├── LogEntryCard.tsx               # Single log entry display
│   │   │   ├── LogbookTimeline.tsx            # Daily timeline view
│   │   │   ├── LogbookMetrics.tsx             # Auto-populated shift metrics
│   │   │   ├── LogbookSearch.tsx              # Full-text search across entries
│   │   │   └── DailyDigestPreview.tsx         # Preview of daily email digest
│   │   │
│   │   ├── tasks/                              # Task Management (inspired by 7shifts)
│   │   │   ├── TaskTemplateEditor.tsx         # Create/edit task template with items
│   │   │   ├── TaskChecklist.tsx              # Employee task checklist (mobile-optimized)
│   │   │   ├── TaskChecklistItem.tsx          # Single task with proof upload
│   │   │   ├── TaskProofCapture.tsx           # Photo/temperature/value capture
│   │   │   ├── TaskDashboard.tsx              # Real-time completion dashboard (manager)
│   │   │   ├── TaskCompletionChart.tsx        # Completion rate chart
│   │   │   └── TaskProgressBar.tsx            # Shift task progress indicator
│   │   │
│   │   ├── engagement/                         # Employee Engagement (inspired by 7shifts + Harri)
│   │   │   ├── PostShiftSurvey.tsx            # 1-3 question quick survey (mobile modal)
│   │   │   ├── SurveyResultsChart.tsx         # Trend chart of survey scores
│   │   │   ├── RecognitionFeed.tsx            # Social feed of shout-outs
│   │   │   ├── RecognitionCard.tsx            # Single recognition entry
│   │   │   ├── GiveRecognitionModal.tsx       # Send recognition form
│   │   │   ├── EngagementScoreCard.tsx        # Employee engagement score display
│   │   │   ├── FlightRiskAlert.tsx            # Flight risk warning card
│   │   │   ├── MilestoneCard.tsx              # Employee milestone celebration
│   │   │   └── SentimentTrendWidget.tsx       # Team sentiment dashboard widget
│   │   │
│   │   ├── forecast/                           # AI Demand Forecasting (inspired by CrunchTime)
│   │   │   ├── ForecastChart.tsx              # Forecast vs. actual sales chart
│   │   │   ├── StaffingRecommendation.tsx     # Recommended staff per role per shift
│   │   │   ├── SPLHGauge.tsx                  # Sales Per Labor Hour gauge/trend
│   │   │   ├── LaborCostPercentGauge.tsx      # Real-time labor cost % vs. target
│   │   │   ├── DemandOverlay.tsx              # Overlay on schedule builder showing forecast
│   │   │   ├── WeatherBadge.tsx               # Weather icon + temp for scheduled days
│   │   │   ├── SpecialEventTag.tsx            # Event indicator on calendar
│   │   │   └── ForecastAccuracyChart.tsx      # Forecast accuracy over time
│   │   │
│   │   ├── geofencing/                         # Geofencing (inspired by Deputy)
│   │   │   ├── GeofenceMap.tsx                # Interactive map for setting geofence zones
│   │   │   ├── GeofenceZoneEditor.tsx         # Set center + radius on map
│   │   │   ├── ClockLocationMap.tsx           # Map showing where employees clocked in
│   │   │   ├── GeofenceStatus.tsx             # "Inside/Outside zone" indicator on clock page
│   │   │   ├── FraudAlertList.tsx             # List of suspicious clock-in attempts
│   │   │   └── LocationVerification.tsx       # GPS check before clock-in allowed
│   │   │
│   │   ├── tips/                               # Tip Management (inspired by 7shifts)
│   │   │   ├── TipEntryForm.tsx               # Enter total tips for shift
│   │   │   ├── TipDistributionPreview.tsx     # Preview distribution before finalizing
│   │   │   ├── TipPoolConfig.tsx              # Configure tip pool rules
│   │   │   ├── TipHistoryTable.tsx            # Employee tip history
│   │   │   └── TipReportChart.tsx             # Tip trends over time
│   │   │
│   │   ├── kiosk/                              # Kiosk Mode (inspired by Homebase)
│   │   │   ├── KioskClockScreen.tsx           # Full-screen clock for shared tablet
│   │   │   ├── KioskPinEntry.tsx              # PIN pad for employee identification
│   │   │   ├── KioskQRScanner.tsx             # QR code scanner for quick ID
│   │   │   └── KioskAdminLock.tsx             # Lock/unlock kiosk mode (admin PIN)
│   │   │
│   │   ├── hiring/                             # Hiring & ATS (inspired by Homebase)
│   │   │   ├── JobPostingList.tsx             # Active/closed postings list
│   │   │   ├── JobPostingForm.tsx             # Create/edit job posting
│   │   │   ├── HiringPipeline.tsx            # Kanban board for applicant stages
│   │   │   ├── ApplicantCard.tsx             # Card in pipeline with AI score
│   │   │   ├── ApplicantDetailModal.tsx      # Full applicant view with resume, notes
│   │   │   ├── InterviewScheduler.tsx        # Interview slot picker + confirmation
│   │   │   ├── TrialShiftEvaluation.tsx      # Trial shift evaluation form
│   │   │   ├── OfferLetterPreview.tsx        # Generate + preview offer letter
│   │   │   └── HiringAnalyticsDashboard.tsx  # Time-to-hire, source effectiveness
│   │   │
│   │   ├── training/                          # Training & LMS (inspired by Connecteam)
│   │   │   ├── CourseList.tsx                # Manager: all courses list
│   │   │   ├── CourseForm.tsx                # Create/edit course
│   │   │   ├── ModuleEditor.tsx              # Edit module content + video + quiz
│   │   │   ├── QuizBuilder.tsx               # Add/edit quiz questions
│   │   │   ├── MyCourses.tsx                 # Employee: assigned courses + progress
│   │   │   ├── CourseViewer.tsx              # Course content viewer (module by module)
│   │   │   ├── QuizPlayer.tsx               # Take a quiz (timer, questions, submit)
│   │   │   ├── QuizResults.tsx              # Quiz score + correct answers
│   │   │   ├── KnowledgeBaseSearch.tsx       # Searchable article library
│   │   │   ├── ArticleViewer.tsx            # Read knowledge base article
│   │   │   ├── HandbookViewer.tsx           # Employee handbook with acknowledge buttons
│   │   │   └── TrainingAnalytics.tsx        # Completion rates, quiz stats
│   │   │
│   │   ├── forms/                             # Digital Forms (inspired by Connecteam)
│   │   │   ├── FormTemplateList.tsx          # Manager: all form templates
│   │   │   ├── FormBuilder.tsx              # Drag-and-drop form builder
│   │   │   ├── FormFieldPalette.tsx         # Field type picker (text, photo, sig, etc.)
│   │   │   ├── FormFieldEditor.tsx          # Configure individual field (validation, alerts)
│   │   │   ├── FormFiller.tsx               # Employee: fill out a form instance
│   │   │   ├── MyPendingForms.tsx           # Employee: list of pending forms
│   │   │   ├── FormSubmissionViewer.tsx     # View completed submission + photos + sigs
│   │   │   ├── FormSubmissionPDF.tsx        # PDF export of submission
│   │   │   └── FormAnalytics.tsx            # Completion rates, alert summary
│   │   │
│   │   ├── feed/                              # Company Newsfeed (inspired by Connecteam + Sling)
│   │   │   ├── FeedPage.tsx                 # Main feed with infinite scroll
│   │   │   ├── FeedPostCard.tsx             # Individual post card (text, images, reactions)
│   │   │   ├── FeedPostForm.tsx             # Create/edit post (rich text, image upload)
│   │   │   ├── FeedReactionBar.tsx          # Reaction picker (like, love, celebrate...)
│   │   │   ├── FeedCommentThread.tsx        # Threaded comments under a post
│   │   │   └── FeedModerationPanel.tsx      # Manager: approve/remove pending posts
│   │   │
│   │   ├── reviews/                           # Performance Reviews (inspired by Push Operations)
│   │   │   ├── ReviewList.tsx               # Manager: all reviews by status
│   │   │   ├── ReviewTemplateForm.tsx       # Admin: create/edit review template + criteria
│   │   │   ├── ReviewDetail.tsx             # Full review: data metrics + ratings + comments
│   │   │   ├── SelfAssessmentForm.tsx       # Employee: complete self-assessment
│   │   │   ├── ManagerReviewForm.tsx        # Manager: complete review + ratings
│   │   │   ├── ReviewSignaturePanel.tsx     # Digital signature for review sign-off
│   │   │   ├── GoalTracker.tsx              # Track goals from reviews
│   │   │   ├── MyReviews.tsx               # Employee: own review history + goals
│   │   │   └── ReviewAnalytics.tsx          # Rating distribution, trends
│   │   │
│   │   ├── autoSchedule/                     # Auto-Scheduling (inspired by 7shifts + Deputy)
│   │   │   ├── AutoScheduleWizard.tsx       # Step 1-3 wizard for generating schedule
│   │   │   ├── StrategyPicker.tsx           # Cost-optimized / Employee-preferred / Balanced
│   │   │   ├── WeightSliders.tsx            # Configurable constraint weights
│   │   │   ├── AutoSchedulePreview.tsx      # Preview results before applying
│   │   │   ├── ScheduleTemplateList.tsx     # Saved weekly schedule templates
│   │   │   ├── ScheduleTemplateForm.tsx     # Save/edit schedule template
│   │   │   ├── ShiftBidList.tsx             # Manager: view employee shift bids
│   │   │   ├── ShiftBidForm.tsx             # Employee: submit shift bids
│   │   │   ├── ShiftPreferenceForm.tsx      # Employee: set preferred shifts/hours
│   │   │   └── FairnessScoreCard.tsx        # Employee fairness metrics
│   │   │
│   │   ├── selfService/                       # Employee Self-Service (inspired by 7shifts)
│   │   │   ├── EarningsPage.tsx             # Estimated + historical earnings
│   │   │   ├── EarningsBreakdown.tsx        # Detailed pay calculation
│   │   │   ├── EarningsTrendChart.tsx       # 6-month earnings trend
│   │   │   ├── HoursTracker.tsx             # Live hours + overtime tracker
│   │   │   ├── MyDocuments.tsx              # View + upload employee documents
│   │   │   ├── OnboardingPortal.tsx         # New hire onboarding steps
│   │   │   └── OnboardingStepCard.tsx       # Individual onboarding step
│   │   │
│   │   ├── intraday/                         # Intraday Dashboard (inspired by Restaurant365)
│   │   │   ├── IntradayDashboard.tsx        # Main real-time labor dashboard
│   │   │   ├── SalesVsLaborChart.tsx        # Hourly sales vs labor chart
│   │   │   ├── EarnedVsActualCard.tsx       # Earned hours vs actual hours
│   │   │   ├── LiveStaffList.tsx            # Who's clocked in right now
│   │   │   ├── LaborPercentGauge.tsx        # Real-time labor % gauge
│   │   │   └── IntradayAlerts.tsx           # Live staffing alerts
│   │   │
│   │   ├── gps/                              # GPS Tracking (inspired by Buddy Punch)
│   │   │   ├── LiveEmployeeMap.tsx          # Map with employee markers
│   │   │   ├── RouteTrailMap.tsx            # Employee route history on map
│   │   │   └── MileageTable.tsx             # Mileage records table
│   │   │
│   │   └── dashboard/
│   │       ├── DashboardWidget.tsx            # Base widget container
│   │       ├── TodayShiftsWidget.tsx          # Who's working today
│   │       ├── PendingRequestsWidget.tsx      # Pending approvals count
│   │       ├── WeeklyHoursWidget.tsx          # Hours summary this week
│   │       ├── StaffingAlertWidget.tsx        # Understaffed shift warnings
│   │       ├── UpcomingLeavesWidget.tsx       # Upcoming team leaves
│   │       ├── ClockStatusWidget.tsx          # Who's clocked in right now
│   │       ├── LaborCostWidget.tsx            # Current period labor cost
│   │       ├── CertExpiryWidget.tsx           # Expiring certifications
│   │       ├── QuickActionsWidget.tsx         # Common actions shortcuts
│   │       ├── SPLHWidget.tsx                 # Sales Per Labor Hour (Phase 3)
│   │       ├── ForecastWidget.tsx             # Today's demand forecast (Phase 3)
│   │       ├── TaskCompletionWidget.tsx       # Today's task completion rate
│   │       ├── TeamSentimentWidget.tsx        # Recent survey average
│   │       ├── EarningsWidget.tsx             # Employee: this week's estimated earnings
│   │       ├── OnboardingProgressWidget.tsx   # New hire: onboarding completion %
│   │       ├── PendingFormsWidget.tsx         # Pending digital forms count
│   │       ├── TrainingDueWidget.tsx          # Overdue training courses
│   │       └── UnreadMessagesWidget.tsx       # Unread chat messages
│   │
│   ├── pages/                                 # Route-level page components
│   │   ├── auth/
│   │   │   ├── LoginPage.tsx
│   │   │   ├── RegisterPage.tsx               # Organization registration
│   │   │   ├── ForgotPasswordPage.tsx
│   │   │   ├── ResetPasswordPage.tsx
│   │   │   └── AcceptInvitePage.tsx           # Accept team invitation
│   │   │
│   │   ├── dashboard/
│   │   │   ├── ManagerDashboardPage.tsx       # Manager/admin dashboard
│   │   │   └── EmployeeDashboardPage.tsx      # Employee self-service dashboard
│   │   │
│   │   ├── employees/
│   │   │   ├── EmployeeListPage.tsx           # Paginated employee directory
│   │   │   ├── EmployeeDetailPage.tsx         # Full employee profile (tabbed)
│   │   │   └── EmployeeCreatePage.tsx         # New employee form
│   │   │
│   │   ├── schedule/
│   │   │   ├── ScheduleBuilderPage.tsx        # THE main schedule builder (drag-drop)
│   │   │   ├── ScheduleListPage.tsx           # List of schedule periods
│   │   │   └── MySchedulePage.tsx             # Employee's own schedule view
│   │   │
│   │   ├── availability/
│   │   │   ├── MyAvailabilityPage.tsx         # Employee sets their availability
│   │   │   └── TeamAvailabilityPage.tsx       # Manager views team availability
│   │   │
│   │   ├── timeoff/
│   │   │   ├── TimeOffRequestsPage.tsx        # List + approve/deny requests
│   │   │   ├── MyTimeOffPage.tsx              # Employee's own requests + balances
│   │   │   └── TeamCalendarPage.tsx           # Team leave calendar
│   │   │
│   │   ├── swaps/
│   │   │   ├── ShiftSwapsPage.tsx             # List swap/cover/drop requests
│   │   │   └── OpenShiftsPage.tsx             # Open shifts marketplace
│   │   │
│   │   ├── clock/
│   │   │   ├── ClockPage.tsx                  # Full-screen clock-in/out
│   │   │   └── ClockHistoryPage.tsx           # Clock entry history + corrections
│   │   │
│   │   ├── timesheets/
│   │   │   ├── TimesheetPeriodsPage.tsx       # List timesheet periods
│   │   │   ├── TimesheetDetailPage.tsx        # Single period detail with all entries
│   │   │   └── MyTimesheetPage.tsx            # Employee's own timesheet
│   │   │
│   │   ├── reports/
│   │   │   ├── ReportsDashboardPage.tsx       # Reports landing page
│   │   │   ├── PayrollReportPage.tsx          # Payroll summary with export
│   │   │   ├── LaborCostReportPage.tsx        # Labor cost analysis
│   │   │   ├── AttendanceReportPage.tsx       # Attendance/absence report
│   │   │   └── OvertimeReportPage.tsx         # Overtime report
│   │   │
│   │   ├── chat/                              # Team Communication (NEW)
│   │   │   ├── ChatPage.tsx                   # Full messaging interface
│   │   │   └── AnnouncementsPage.tsx          # Announcement feed + create
│   │   │
│   │   ├── logbook/                           # Manager Logbook (NEW)
│   │   │   └── LogbookPage.tsx                # Daily logbook view + entry form
│   │   │
│   │   ├── tasks/                             # Task Management (NEW)
│   │   │   ├── TaskDashboardPage.tsx          # Manager task overview
│   │   │   ├── MyTasksPage.tsx                # Employee's current shift tasks
│   │   │   └── TaskTemplatesPage.tsx          # Task template configuration
│   │   │
│   │   ├── engagement/                        # Employee Engagement (NEW)
│   │   │   ├── EngagementDashboardPage.tsx    # Survey results + sentiment trends
│   │   │   ├── RecognitionPage.tsx            # Recognition feed + give shout-out
│   │   │   └── MilestonesPage.tsx             # Upcoming milestones
│   │   │
│   │   ├── forecast/                          # Demand Forecasting (NEW)
│   │   │   └── ForecastDashboardPage.tsx      # Forecast + SPLH + labor % dashboard
│   │   │
│   │   ├── geofencing/                        # Geofencing (NEW)
│   │   │   └── GeofenceSettingsPage.tsx       # Geofence zone management + map
│   │   │
│   │   ├── tips/                              # Tip Management (NEW)
│   │   │   ├── TipEntryPage.tsx               # Enter + distribute tips
│   │   │   └── TipReportsPage.tsx             # Tip reporting + history
│   │   │
│   │   ├── kiosk/                             # Kiosk Mode (NEW)
│   │   │   └── KioskPage.tsx                  # Full-screen kiosk clock mode
│   │   │
│   │   ├── settings/
│   │   │   ├── OrganizationSettingsPage.tsx   # Org profile, locations, departments
│   │   │   ├── RolesSettingsPage.tsx          # Roles + permissions management
│   │   │   ├── StationsSettingsPage.tsx       # Station management
│   │   │   ├── ShiftTemplatesPage.tsx         # Shift template management
│   │   │   ├── StaffingRequirementsPage.tsx   # Staffing requirement config
│   │   │   ├── SchedulingRulesPage.tsx        # Constraint rules config
│   │   │   ├── LeaveTypesPage.tsx             # Leave type management
│   │   │   ├── BlackoutDatesPage.tsx          # Blackout date management
│   │   │   ├── PosConnectionsPage.tsx         # POS integration management
│   │   │   ├── NotificationSettingsPage.tsx   # Notification preferences
│   │   │   ├── GeofenceSettingsPage.tsx       # Geofence zone management
│   │   │   ├── TipPoolSettingsPage.tsx        # Tip pool configuration
│   │   │   ├── ComplianceSettingsPage.tsx     # Jurisdiction + compliance rules
│   │   │   └── ProfileSettingsPage.tsx        # User profile + password
│   │   │
│   │   └── audit/
│   │       └── AuditLogPage.tsx               # Audit trail viewer
│   │
│   ├── router/
│   │   ├── index.tsx                          # createBrowserRouter configuration
│   │   ├── routes.ts                          # Route definitions with metadata
│   │   └── guards.tsx                         # Auth + permission route guards
│   │
│   ├── types/                                 # TypeScript type definitions
│   │   ├── employee.ts
│   │   ├── schedule.ts
│   │   ├── timeoff.ts
│   │   ├── timesheet.ts
│   │   ├── clock.ts
│   │   ├── role.ts
│   │   ├── station.ts
│   │   ├── organization.ts
│   │   ├── notification.ts
│   │   ├── report.ts
│   │   ├── auth.ts
│   │   ├── api.ts                             # Generic API response types
│   │   └── enums.ts                           # All enum types matching backend
│   │
│   ├── utils/
│   │   ├── dateUtils.ts                       # Date formatting, time zone helpers
│   │   ├── timeUtils.ts                       # Duration calculations, time formatting
│   │   ├── currencyUtils.ts                   # Format cents to currency display
│   │   ├── colorUtils.ts                      # Role/shift color helpers
│   │   ├── permissionUtils.ts                 # Permission check utilities
│   │   ├── exportUtils.ts                     # CSV/PDF generation helpers
│   │   ├── validationUtils.ts                 # Common validation patterns
│   │   ├── scheduleUtils.ts                   # Schedule calculation helpers
│   │   └── constants.ts                       # App-wide constants
│   │
│   ├── i18n/
│   │   ├── index.ts                           # i18next configuration
│   │   ├── en/
│   │   │   ├── common.json
│   │   │   ├── auth.json
│   │   │   ├── employees.json
│   │   │   ├── schedule.json
│   │   │   ├── timeoff.json
│   │   │   ├── timesheets.json
│   │   │   ├── clock.json
│   │   │   ├── reports.json
│   │   │   ├── settings.json
│   │   │   └── notifications.json
│   │   ├── fr/
│   │   │   └── ... (same structure)
│   │   └── de/
│   │       └── ... (same structure)
│   │
│   ├── theme/
│   │   ├── antdTheme.ts                       # Ant Design 5 theme tokens
│   │   ├── colors.ts                          # App color palette
│   │   ├── breakpoints.ts                     # Responsive breakpoints
│   │   └── global.css                         # Global styles
│   │
│   └── assets/
│       ├── images/
│       ├── icons/
│       └── illustrations/                     # Empty states, onboarding, etc.
│
├── .env
├── .env.development
├── .env.production
├── .eslintrc.cjs
├── .prettierrc
├── tsconfig.json
├── tsconfig.node.json
├── vite.config.ts
├── index.html
├── package.json
├── pnpm-lock.yaml
├── Dockerfile
├── .dockerignore
└── .github/
    └── workflows/
        ├── ci.yml
        └── cd.yml
```

---

## 4. Routing Configuration

### 4.1 Route Map

```typescript
// src/router/routes.ts

export const routes = {
  // --- Public Routes (no auth required) ---
  auth: {
    login:           '/login',
    register:        '/register',
    forgotPassword:  '/forgot-password',
    resetPassword:   '/reset-password/:token',
    acceptInvite:    '/invite/:token',
  },

  // --- Authenticated Routes ---
  dashboard:         '/',

  // Employees
  employees: {
    list:            '/employees',
    create:          '/employees/new',
    detail:          '/employees/:id',
    edit:            '/employees/:id/edit',
  },

  // Scheduling
  schedule: {
    list:            '/schedules',
    builder:         '/schedules/:id',
    mySchedule:      '/my-schedule',
  },

  // Availability
  availability: {
    mine:            '/my-availability',
    team:            '/team-availability',
  },

  // Time Off
  timeoff: {
    requests:        '/time-off',
    mine:            '/my-time-off',
    calendar:        '/team-calendar',
  },

  // Shift Swaps & Open Shifts
  swaps: {
    list:            '/shift-swaps',
    openShifts:      '/open-shifts',
  },

  // Clock
  clock: {
    main:            '/clock',
    history:         '/clock/history',
  },

  // Timesheets
  timesheets: {
    periods:         '/timesheets',
    detail:          '/timesheets/:id',
    mine:            '/my-timesheet',
  },

  // Reports
  reports: {
    dashboard:       '/reports',
    payroll:         '/reports/payroll',
    laborCost:       '/reports/labor-cost',
    attendance:      '/reports/attendance',
    overtime:        '/reports/overtime',
  },

  // Settings
  settings: {
    organization:    '/settings/organization',
    roles:           '/settings/roles',
    stations:        '/settings/stations',
    shiftTemplates:  '/settings/shift-templates',
    staffing:        '/settings/staffing-requirements',
    rules:           '/settings/scheduling-rules',
    leaveTypes:      '/settings/leave-types',
    blackoutDates:   '/settings/blackout-dates',
    pos:             '/settings/pos-connections',
    notifications:   '/settings/notifications',
    profile:         '/settings/profile',
  },

  // Team Communication
  chat: {
    main:            '/chat',
    announcements:   '/announcements',
  },

  // Manager Logbook
  logbook: {
    main:            '/logbook',
  },

  // Task Management
  tasks: {
    dashboard:       '/tasks',
    myTasks:         '/my-tasks',
    templates:       '/tasks/templates',
  },

  // Employee Engagement
  engagement: {
    dashboard:       '/engagement',
    recognition:     '/engagement/recognition',
    milestones:      '/engagement/milestones',
  },

  // Demand Forecasting
  forecast: {
    dashboard:       '/forecast',
  },

  // Tips
  tips: {
    entry:           '/tips',
    reports:         '/tips/reports',
  },

  // Kiosk
  kiosk: {
    main:            '/kiosk',
  },

  // Audit
  audit: {
    logs:            '/audit',
  },

  // --- New Modules ---

  // Hiring & ATS
  hiring: {
    postings:        '/hiring',
    postingDetail:   '/hiring/postings/:id',
    createPosting:   '/hiring/postings/new',
    pipeline:        '/hiring/postings/:id/pipeline',
    applicant:       '/hiring/applications/:id',
    analytics:       '/hiring/analytics',
  },

  // Training & LMS
  training: {
    courses:         '/training',
    courseDetail:     '/training/courses/:id',
    myCourses:       '/my-training',
    courseViewer:     '/training/courses/:id/learn',
    quiz:            '/training/courses/:id/modules/:moduleId/quiz',
    knowledgeBase:   '/knowledge-base',
    articleDetail:   '/knowledge-base/:id',
    handbook:        '/handbook',
  },

  // Digital Forms
  forms: {
    templates:       '/forms',
    builder:         '/forms/templates/:id/edit',
    createTemplate:  '/forms/templates/new',
    myForms:         '/my-forms',
    fillForm:        '/forms/templates/:id/fill',
    submission:      '/forms/submissions/:id',
    analytics:       '/forms/analytics',
  },

  // Company Newsfeed
  feed: {
    main:            '/feed',
    post:            '/feed/:id',
  },

  // Performance Reviews
  reviews: {
    list:            '/reviews',
    detail:          '/reviews/:id',
    myReviews:       '/my-reviews',
    selfAssessment:  '/reviews/:id/self-assessment',
    templates:       '/reviews/templates',
  },

  // Auto-Scheduling
  autoSchedule: {
    wizard:          '/schedules/:periodId/auto-schedule',
    templates:       '/schedule-templates',
    templateDetail:  '/schedule-templates/:id',
    bids:            '/shift-bids',
    myPreferences:   '/my-preferences',
  },

  // Employee Self-Service
  selfService: {
    earnings:        '/my-earnings',
    onboarding:      '/my-onboarding',
    documents:       '/my-documents',
    hoursTracker:    '/my-hours',
  },

  // Intraday Dashboard
  intraday: {
    dashboard:       '/dashboard/intraday',
  },

  // GPS Tracking
  gps: {
    liveMap:         '/gps/live-map',
    trail:           '/gps/trail/:employeeId/:date',
  },
};
```

### 4.2 Navigation Structure

```
├── Dashboard                    (/)
├── Scheduling
│   ├── Schedule Builder         (/schedules/:id)
│   ├── Schedule Periods         (/schedules)
│   └── My Schedule              (/my-schedule)        [Employee]
├── Employees                    (/employees)           [Manager+]
├── Availability
│   ├── My Availability          (/my-availability)
│   └── Team Availability        (/team-availability)  [Manager+]
├── Time Off
│   ├── Requests                 (/time-off)           [Manager+]
│   ├── My Time Off              (/my-time-off)
│   └── Team Calendar            (/team-calendar)
├── Shift Market
│   ├── Swap Requests            (/shift-swaps)
│   └── Open Shifts              (/open-shifts)
├── Clock                        (/clock)
├── Timesheets
│   ├── Periods                  (/timesheets)         [Manager+]
│   ├── Period Detail            (/timesheets/:id)     [Manager+]
│   └── My Timesheet             (/my-timesheet)
├── Reports                                             [Manager+]
│   ├── Dashboard                (/reports)
│   ├── Payroll                  (/reports/payroll)
│   ├── Labor Cost               (/reports/labor-cost)
│   ├── Attendance               (/reports/attendance)
│   └── Overtime                 (/reports/overtime)
├── Settings                                            [Admin]
│   ├── Organization             (/settings/organization)
│   ├── Roles & Permissions      (/settings/roles)
│   ├── Stations                 (/settings/stations)
│   ├── Shift Templates          (/settings/shift-templates)
│   ├── Staffing Requirements    (/settings/staffing-requirements)
│   ├── Scheduling Rules         (/settings/scheduling-rules)
│   ├── Leave Types              (/settings/leave-types)
│   ├── Blackout Dates           (/settings/blackout-dates)
│   ├── POS Connections          (/settings/pos-connections)
│   └── Notifications            (/settings/notifications)
├── Team Chat                    (/chat)
│   ├── Messages                 (/chat)
│   └── Announcements            (/announcements)
├── Manager Logbook              (/logbook)              [Manager+]
├── Tasks
│   ├── Task Dashboard           (/tasks)                [Manager+]
│   ├── My Tasks                 (/my-tasks)
│   └── Task Templates           (/tasks/templates)      [Manager+]
├── Engagement                                            [Manager+]
│   ├── Dashboard                (/engagement)
│   ├── Recognition              (/engagement/recognition)
│   └── Milestones               (/engagement/milestones)
├── Forecasting                  (/forecast)             [Manager+]
├── Tips                                                  [Manager+]
│   ├── Tip Entry                (/tips)
│   └── Tip Reports              (/tips/reports)
├── Hiring                                                [Manager+]
│   ├── Job Postings             (/hiring)
│   ├── Pipeline                 (/hiring/postings/:id/pipeline)
│   └── Analytics                (/hiring/analytics)
├── Training                                               [All]
│   ├── My Training              (/my-training)            [Employee]
│   ├── Course Library           (/training)               [Manager+]
│   ├── Knowledge Base           (/knowledge-base)
│   └── Employee Handbook        (/handbook)
├── Forms                                                  [All]
│   ├── My Pending Forms         (/my-forms)               [Employee]
│   ├── Form Templates           (/forms)                  [Manager+]
│   ├── Form Builder             (/forms/templates/:id/edit) [Manager+]
│   └── Submissions & Analytics  (/forms/analytics)        [Manager+]
├── Company Feed                 (/feed)                   [All]
├── Performance Reviews                                    [All]
│   ├── My Reviews               (/my-reviews)             [Employee]
│   ├── All Reviews              (/reviews)                [Manager+]
│   └── Review Templates         (/reviews/templates)      [Admin]
├── Self-Service
│   ├── My Earnings              (/my-earnings)            [Employee]
│   ├── My Hours                 (/my-hours)               [Employee]
│   ├── My Documents             (/my-documents)           [Employee]
│   └── Onboarding               (/my-onboarding)         [New Hire]
├── Intraday Dashboard           (/dashboard/intraday)     [Manager+]
├── GPS Live Map                 (/gps/live-map)           [Manager+]
├── Auto-Scheduling                                        [Manager+]
│   ├── Schedule Templates       (/schedule-templates)
│   ├── Shift Bids               (/shift-bids)
│   └── My Preferences           (/my-preferences)        [Employee]
├── Profile                      (/settings/profile)
├── Kiosk Mode                   (/kiosk)                [Admin]
└── Audit Log                    (/audit)               [Admin]
```

### 4.3 Role-Based Navigation Visibility

| Nav Item | Employee | Manager | Admin |
|----------|----------|---------|-------|
| Dashboard | Own stats | Team overview | Full overview |
| Schedule Builder | View only | Full edit | Full edit |
| My Schedule | Yes | Yes | Yes |
| Employees | No | Yes | Yes |
| My Availability | Yes | Yes | Yes |
| Team Availability | No | Yes | Yes |
| My Time Off | Yes | Yes | Yes |
| Time-Off Requests | No | Yes (approve) | Yes (approve) |
| Team Calendar | View only | Yes | Yes |
| Shift Swaps | Own swaps | All swaps | All swaps |
| Open Shifts | Claim | Create + manage | Create + manage |
| Clock | Yes | Yes | Yes |
| My Timesheet | Yes | Yes | Yes |
| Timesheet Periods | No | Yes | Yes |
| Reports | No | Yes | Yes |
| Team Chat | Yes | Yes | Yes |
| Announcements | Read only | Create + read | Create + read |
| Manager Logbook | No | Yes | Yes |
| My Tasks | Yes | Yes | Yes |
| Task Dashboard | No | Yes | Yes |
| Recognition Feed | Yes (give/receive) | Yes | Yes |
| Engagement Dashboard | No | Yes | Yes |
| Demand Forecast | No | Yes | Yes |
| Tips | No | Yes | Yes |
| Kiosk Mode | No | No | Yes (setup) |
| **Hiring Pipeline** | No | Yes | Yes |
| **My Training** | Yes (own courses) | Yes | Yes |
| **Course Management** | No | Yes (create) | Yes (create) |
| **Knowledge Base** | Yes (read) | Yes (read + write) | Yes (full) |
| **Employee Handbook** | Yes (read + acknowledge) | Yes | Yes |
| **My Pending Forms** | Yes (fill out) | Yes | Yes |
| **Form Templates** | No | Yes (create) | Yes (create) |
| **Form Builder** | No | Yes | Yes |
| **Company Feed** | Yes (read + react + post*) | Yes (all + moderate) | Yes (all + moderate) |
| **My Earnings** | Yes | Yes | Yes |
| **My Hours Tracker** | Yes | Yes | Yes |
| **Onboarding Portal** | Yes (new hires) | No | No |
| **My Reviews** | Yes (own reviews) | Yes | Yes |
| **All Reviews** | No | Yes (team) | Yes (all) |
| **Review Templates** | No | No | Yes |
| **Intraday Dashboard** | No | Yes | Yes |
| **Schedule Templates** | No | Yes | Yes |
| **Auto-Schedule Wizard** | No | Yes | Yes |
| **Shift Bids** | Yes (submit bids) | Yes (view all) | Yes (view all) |
| **My Preferences** | Yes | Yes | Yes |
| **GPS Live Map** | No | Yes | Yes |
| Settings | Profile only | Limited | Full access |
| Audit Log | No | No | Yes |

*Company Feed posting by employees requires "open posting" to be enabled in settings. Otherwise, employees can only read, react, and comment.

---

## 5. Page Specifications

### 5.1 Schedule Builder Page (Most Complex Page)

This is the **core feature** of the entire application — the drag-and-drop schedule builder.

#### Layout

```
┌─────────────────────────────────────────────────────────────────────┐
│  ScheduleToolbar                                                     │
│  [< Prev Week] [Feb 10–16, 2026] [Next Week >]  [Filter ▾] [Publish]│
├──────────┬──────────────────────────────────────────────────────────┤
│          │                  Schedule Grid                            │
│ Employee │  Mon 10  │  Tue 11  │  Wed 12  │  Thu 13  │  Fri 14  │  │
│ Sidebar  ├──────────┼──────────┼──────────┼──────────┼──────────┤  │
│          │          │          │          │          │          │  │
│ ┌──────┐ │ ┌──────┐ │          │ ┌──────┐ │ ┌──────┐ │          │  │
│ │Marie │ │ │09-15 │ │          │ │09-15 │ │ │09-15 │ │          │  │
│ │Waiter│ │ │Terrace│ │          │ │Bar   │ │ │Terrace│ │          │  │
│ │██████│ │ └──────┘ │          │ └──────┘ │ └──────┘ │          │  │
│ └──────┘ │          │          │          │          │          │  │
│          │          │          │          │          │          │  │
│ ┌──────┐ │          │ ┌──────┐ │ ┌──────┐ │          │ ┌──────┐ │  │
│ │Pierre│ │          │ │17-23 │ │ │17-23 │ │          │ │17-23 │ │  │
│ │Chef  │ │          │ │Kitchen│ │ │Kitchen│ │          │ │Kitchen│ │  │
│ │██████│ │          │ └──────┘ │ └──────┘ │          │ └──────┘ │  │
│ └──────┘ │          │          │          │          │          │  │
│          │          │          │          │          │          │  │
│ [+ Add]  │          │          │          │          │          │  │
│          ├──────────┴──────────┴──────────┴──────────┴──────────┤  │
│          │  StaffingCoverageBar                                  │  │
│          │  Waiters: ████████░░  3/4    Kitchen: ██████████  4/4 │  │
│          ├───────────────────────────────────────────────────────┤  │
│          │  LaborCostSummary                                     │  │
│          │  Projected: CHF 4,250  │  Budget: CHF 4,500  │ ✅ OK │  │
└──────────┴───────────────────────────────────────────────────────┘
```

#### Interactions

| Action | Behavior |
|--------|----------|
| **Drag employee from sidebar to day cell** | Create new shift assignment. Opens ShiftAssignmentModal pre-filled with default template. |
| **Drag existing shift card to different day** | Move shift to new date. Runs conflict detection. |
| **Drag existing shift card to different employee row** | Reassign shift to different employee. Runs conflict detection. |
| **Click shift card** | Open ShiftAssignmentModal for editing. |
| **Right-click shift card** | Context menu: Edit, Delete, Copy, Duplicate to other days. |
| **Double-click empty cell** | Quick-create shift for that employee + day. |
| **Click "Copy Previous Week"** | Clone all assignments from previous week, show conflicts. |
| **Click "Publish"** | Confirm dialog → change status to Published → trigger notifications. |
| **Hover shift card** | Tooltip: full details (times, station, role, cost). |
| **Filter by role** | Only show employees with selected role. Dim non-matching shifts. |
| **Filter by department** | Only show employees in selected department. |

#### Drag-and-Drop Interaction Details

| Phase | Desktop Behavior | Mobile (Touch) Behavior | Animation Timing |
|-------|-----------------|------------------------|-----------------|
| **Drag Start** | Mouse down + 5px movement threshold | Long-press (200ms) + haptic vibration (`Navigator.vibrate(50)`) | Card lifts with `scale(1.05)` + shadow elevation in 150ms |
| **Drag Over Cell** | Cell highlights with dashed border. Conflict check runs on hover (before drop). | Cell highlights on touch-enter. Subtle haptic pulse on each new cell. | 150ms transition on cell highlight color |
| **Conflict Preview** | Red/yellow border on cell BEFORE drop. Tooltip shows conflict message. | Red/yellow cell glow. Conflict detail in bottom sheet. | 200ms border color transition |
| **Drop (Success)** | Card snaps to grid cell with spring animation. API call fires. | Card snaps with haptic success (`vibrate([50, 30, 50])`). | Spring: stiffness 500, damping 30 |
| **Drop (Rejected)** | Card bounces back to original position. Red flash on drop zone. | Card returns to origin with haptic error (`vibrate(200)`). | 300ms ease-out return animation |
| **Cancel** | Press `Escape` or drop outside grid | Lift finger outside grid area | 200ms fade back to original position |

#### Complete Keyboard Shortcuts Reference

| Shortcut | Action | Context |
|----------|--------|---------|
| `←` `→` | Navigate between days | Schedule grid focused |
| `↑` `↓` | Navigate between employees | Schedule grid focused |
| `Enter` | Open ShiftAssignmentModal for selected cell | Cell selected (edit existing or create new) |
| `Delete` / `Backspace` | Delete selected shift assignment | Shift card focused (shows confirm first) |
| `Escape` | Cancel current operation | Modal open / dragging |
| `Ctrl+C` | Copy selected shift card | Shift card focused |
| `Ctrl+V` | Paste shift to selected cell | Empty cell selected (runs conflict check) |
| `Ctrl+D` | Duplicate shift to next day | Shift card focused |
| `Ctrl+Shift+D` | Duplicate shift to rest of week | Shift card focused |
| `Ctrl+Z` | Undo last action | Schedule builder |
| `Ctrl+Shift+Z` | Redo | Schedule builder |
| `Ctrl+S` | Save all pending changes | Schedule builder |
| `Ctrl+P` | Open print view | Schedule builder |
| `Space` | Toggle multi-select | Shift card focused (enables bulk operations) |
| `F` | Open filter panel | Schedule builder (focus filter dropdown) |
| `Ctrl+Enter` | Publish schedule | Schedule builder (Draft status) |
| `/` | Focus search input | Employee sidebar (quick-search employees) |
| `?` | Show keyboard shortcuts help | Anywhere (opens shortcuts modal) |

#### Conflict Display

When a conflict is detected during drag or after assignment, the ShiftCard shows:
- **Red border (2px solid) + ⛔ icon** for errors (hard blocks — cannot save)
- **Yellow border (2px solid) + ⚠️ icon** for warnings (overridable with reason)
- Clicking the icon shows a popover with conflict details and an "Override" button (warnings only)
- Multiple conflicts stack as a count badge: "⚠️ 3 issues"

**Conflict popover content example:**
```
┌──────────────────────────────────────────┐
│ ⛔ 1 Error  ⚠️ 2 Warnings               │
│──────────────────────────────────────────│
│ ⛔ Insufficient rest: Marie has only     │
│    9h rest since yesterday (min: 11h).   │
│    Previous shift ended at 23:30.        │
│                                          │
│ ⚠️ Overtime: Marie would reach 44h this  │
│    week (contract limit: 42h).           │
│    [Override with reason ▾]              │
│                                          │
│ ⚠️ Sophie's food safety cert expires     │
│    in 3 days (Feb 18). [Acknowledge ▾]   │
└──────────────────────────────────────────┘
```

#### Responsive Behavior

| Viewport | Layout | Interaction Model |
|----------|--------|--------------------|
| Desktop (>=1200px) | Full grid with employee sidebar + 7-day columns | Drag-drop + keyboard shortcuts + right-click context menu |
| Tablet (768-1199px) | Grid with collapsible sidebar, 5-day columns with horizontal scroll | Touch drag-drop (long-press to start) + tap to edit |
| Mobile (<768px) | Day-by-day card list (see wireframe below) | No drag-drop. Tap day to navigate. Tap "+" to assign via modal. |

#### Mobile Schedule View Wireframe

```
┌─────────────────────────────────┐
│  Schedule  Feb 10-16            │
│  [< Mon] [ Tue Feb 11 ] [Wed >]│
├─────────────────────────────────┤
│                                  │
│  Morning (09:00-15:00):          │
│  ┌─────────────────────────────┐│
│  │ Marie D.  Waiter            ││
│  │ Terrace  6h                  ││
│  │ [Edit] [Delete]              ││
│  └─────────────────────────────┘│
│  ┌─────────────────────────────┐│
│  │ Pierre L.  Chef             ││
│  │ Kitchen A  6h                ││
│  └─────────────────────────────┘│
│                                  │
│  Evening (17:00-23:30):          │
│  ┌─────────────────────────────┐│
│  │ Sophie M.  Waiter           ││
│  │ Main Dining  6.5h            ││
│  │ ⚠️ Cert expiring Feb 18      ││
│  └─────────────────────────────┘│
│  ┌─────────────────────────────┐│
│  │ Alex R.  Bartender          ││
│  │ Bar  6.5h                    ││
│  └─────────────────────────────┘│
│                                  │
│  Coverage:                       │
│  Waiters: ██████░░ 2/3 ⚠️      │
│  Kitchen: ████████ 2/2 ✅       │
│  Bar: ████████ 1/1 ✅           │
│                                  │
│  [+ Add Shift]                  │
│                                  │
│  Total: 4 shifts  CHF 650      │
│  Budget: CHF 700  ✅ OK        │
│                                  │
└─────────────────────────────────┘
```

#### Mobile Clock Page (Detailed Interactions)

```
┌─────────────────────────────────┐
│  Online                          │
│                                  │
│      Tue, Feb 10, 2026          │
│         08:53:27                │
│                                  │
│   Today's Shift:                 │
│   ┌─────────────────────────┐   │
│   │ Morning (09:00 - 15:00) │   │
│   │ Terrace  Waiter         │   │
│   │ Break: 30 min (unpaid)  │   │
│   └─────────────────────────┘   │
│                                  │
│   Location: ✅ Inside zone      │
│   La Dolce Vita (12m away)      │
│                                  │
│   ┌─────────────────────────┐   │
│   │                         │   │
│   │     CLOCK IN            │   │
│   │                         │   │
│   └─────────────────────────┘   │
│                                  │
│   Shift starts in 7 min         │
│                                  │
│   My Tasks (6 tasks):            │
│   Opening Checklist (4 items)   │
│   Shift Tasks (2 items)         │
│                                  │
│   Today's Entries:               │
│   (none yet)                     │
│                                  │
└─────────────────────────────────┘

After Clock-In:

┌─────────────────────────────────┐
│  Clocked In  4h 32m              │
│                                  │
│   ┌─────────────────────────┐   │
│   │ Clocked in at 08:55     │   │
│   │ Duration: 4h 32m        │   │
│   │ Scheduled end: 15:00    │   │
│   │ Remaining: 1h 28m       │   │
│   └─────────────────────────┘   │
│                                  │
│   ┌────────────┐ ┌────────────┐ │
│   │  START      │ │  CLOCK     │ │
│   │  BREAK      │ │  OUT       │ │
│   └────────────┘ └────────────┘ │
│                                  │
│   Tasks: ████████░░ 4/6 done   │
│   [View My Tasks]               │
│                                  │
│   Today's Entries:               │
│   ✅ 08:55 - Clocked In        │
│      Terrace Zone (12m)         │
│   ☕ 11:30 - Break Started     │
│   ☕ 12:00 - Break Ended       │
│      (30 min)                   │
│                                  │
└─────────────────────────────────┘
```

### 5.2 Employee Detail Page (Tabbed)

```
┌─────────────────────────────────────────────────────────────────────┐
│  PageHeader: [← Back]  Marie Dubois  [Edit] [Deactivate]           │
├─────────────────────────────────────────────────────────────────────┤
│  ┌──────┐  Marie Dubois                                             │
│  │ Photo│  Waiter (Senior) • Bartender (Junior)                     │
│  └──────┘  marie.dubois@email.ch  •  +41 79 123 4567               │
│            Status: ● Active  •  Hired: 15 Jan 2024                  │
├─────────────────────────────────────────────────────────────────────┤
│  [Overview] [Contract] [Roles & Skills] [Availability] [Schedule]   │
│  [Time Off] [Timesheets] [Documents] [Notes] [Onboarding]          │
├─────────────────────────────────────────────────────────────────────┤
│                                                                      │
│  Tab Content Area                                                    │
│                                                                      │
│  Overview Tab:                                                       │
│  ┌─────────────┐ ┌─────────────┐ ┌─────────────┐ ┌──────────────┐  │
│  │ Hours This  │ │ Leave      │ │ Next Shift  │ │ Overtime     │  │
│  │ Week: 32h   │ │ Balance:   │ │ Tomorrow    │ │ This Month:  │  │
│  │ / 42h       │ │ 18/25 days │ │ 09:00-15:00 │ │ 4.5h         │  │
│  └─────────────┘ └─────────────┘ └─────────────┘ └──────────────┘  │
│                                                                      │
│  Recent Activity Timeline                                            │
│  • Feb 8 — Worked 8h (Morning, Terrace)                            │
│  • Feb 7 — Time-off request approved (Feb 20-22)                    │
│  • Feb 5 — Shift swapped with Pierre (Feb 12 Evening)              │
│                                                                      │
└─────────────────────────────────────────────────────────────────────┘
```

#### Employee Detail Tabs

| Tab | Content | Access |
|-----|---------|--------|
| **Overview** | Summary stats, recent activity timeline, quick actions | Manager+ |
| **Contract** | Current + historical contracts, hourly rate, hours/week | Admin/HR |
| **Roles & Skills** | Assigned roles (with proficiency), skills/certifications (with expiry alerts) | Manager+ |
| **Availability** | Visual weekly grid showing recurring availability + overrides | Manager+ (read), Self (edit) |
| **Schedule** | Employee's scheduled shifts calendar (mini schedule view) | Manager+ or Self |
| **Time Off** | Leave balances + request history | Manager+ or Self |
| **Timesheets** | Timesheet history with hours breakdown | Manager+ or Self |
| **Documents** | Uploaded documents (contract scans, certificates, IDs) | Admin/HR |
| **Notes** | Internal manager notes (not visible to employee) | Manager+ |
| **Onboarding** | Onboarding checklist with completion status | Manager+ |

### 5.3 Clock Page (Mobile-Optimized)

This page is designed for mobile use — employees open it on their phone when arriving/leaving.

```
┌─────────────────────────────────┐
│                                  │
│         Staff Pro                │
│                                  │
│      Mon, Feb 9, 2026           │
│         14:32:05                │
│                                  │
│   ┌─────────────────────────┐   │
│   │                         │   │
│   │     🟢 CLOCK IN        │   │
│   │                         │   │
│   └─────────────────────────┘   │
│                                  │
│   Today's Shift:                 │
│   Morning (09:00 – 15:00)       │
│   Station: Terrace               │
│   Role: Waiter                   │
│                                  │
│   ─────────────────────────     │
│                                  │
│   Today's Clock Entries:         │
│   (none yet)                     │
│                                  │
└─────────────────────────────────┘
```

After clocking in:

```
┌─────────────────────────────────┐
│                                  │
│   Clocked in since 09:03        │
│   Duration: 5h 29m              │
│                                  │
│   ┌────────────┐ ┌────────────┐ │
│   │  ☕ START   │ │  🔴 CLOCK  │ │
│   │   BREAK    │ │    OUT     │ │
│   └────────────┘ └────────────┘ │
│                                  │
│   Today's Clock Entries:         │
│   ✅ 09:03 — Clocked In        │
│                                  │
└─────────────────────────────────┘
```

**Clock page features:**
- Large, touch-friendly buttons (minimum 60px height)
- Current time displayed prominently
- Today's scheduled shift info
- Clock entry history for today
- Offline support: if no internet, queue the clock action and show "Pending sync"
- Optional: GPS location capture on clock-in (with user permission)
- Optional: PIN pad for shared tablet/kiosk mode

### 5.4 Manager Dashboard Page

```
┌─────────────────────────────────────────────────────────────────────┐
│  Dashboard                                          Mon, Feb 9 2026 │
├─────────────────────────────────────────────────────────────────────┤
│                                                                      │
│  ┌──────────────────┐  ┌──────────────────┐  ┌──────────────────┐  │
│  │ 📋 Today's Staff │  │ ⏳ Pending        │  │ 💰 Labor Cost    │  │
│  │                   │  │ Requests          │  │ This Week        │  │
│  │  12 scheduled     │  │                   │  │                   │  │
│  │  10 clocked in    │  │  3 time-off       │  │  CHF 8,420       │  │
│  │   2 not yet       │  │  1 shift swap     │  │  Budget: 9,000   │  │
│  │                   │  │  [Review →]       │  │  ████████░░ 94%  │  │
│  └──────────────────┘  └──────────────────┘  └──────────────────┘  │
│                                                                      │
│  ┌──────────────────┐  ┌──────────────────┐  ┌──────────────────┐  │
│  │ ⚠️ Staffing      │  │ 📅 Upcoming      │  │ 🏥 Cert Expiry   │  │
│  │ Alerts            │  │ Leaves            │  │                   │  │
│  │                   │  │                   │  │  2 expiring       │  │
│  │  Fri Evening:     │  │  Marie: Feb 20-22│  │  within 30 days   │  │
│  │  Need 1 more      │  │  Pierre: Feb 25  │  │                   │  │
│  │  waiter            │  │                   │  │  [View →]        │  │
│  │  [Fix →]          │  │                   │  │                   │  │
│  └──────────────────┘  └──────────────────┘  └──────────────────┘  │
│                                                                      │
│  ┌──────────────────────────────────────────────────────────────┐   │
│  │  Weekly Hours Overview                                        │   │
│  │  ┌─────────────────────────────────────────────────────────┐ │   │
│  │  │  [Chart: stacked bar per day — regular vs. overtime]     │ │   │
│  │  └─────────────────────────────────────────────────────────┘ │   │
│  └──────────────────────────────────────────────────────────────┘   │
│                                                                      │
│  Quick Actions: [+ New Shift] [+ New Employee] [View Schedule]      │
│                                                                      │
└─────────────────────────────────────────────────────────────────────┘
```

### 5.5 Employee Dashboard Page

A simplified dashboard for employees (not managers).

```
┌─────────────────────────────────────────────────────────────────────┐
│  My Dashboard                                       Mon, Feb 9 2026 │
├─────────────────────────────────────────────────────────────────────┤
│                                                                      │
│  ┌──────────────────────────────────────────────────────────────┐   │
│  │  Clock Widget                                                 │   │
│  │  ┌─────────────────────────────────────────────────────────┐ │   │
│  │  │          [🟢 CLOCK IN]     Today: Morning 09:00–15:00   │ │   │
│  │  └─────────────────────────────────────────────────────────┘ │   │
│  └──────────────────────────────────────────────────────────────┘   │
│                                                                      │
│  ┌──────────────────┐  ┌──────────────────┐  ┌──────────────────┐  │
│  │ 📅 My Upcoming   │  │ ⏱️ Hours This    │  │ 🏖️ Leave         │  │
│  │ Shifts            │  │ Week              │  │ Balance           │  │
│  │                   │  │                   │  │                   │  │
│  │  Today: 09-15     │  │  32h / 42h       │  │  Vacation: 18d   │  │
│  │  Wed: 09-15       │  │  OT: 0h          │  │  Sick: unlimited │  │
│  │  Thu: 17-23       │  │                   │  │                   │  │
│  │  [Full schedule→] │  │                   │  │  [Request →]     │  │
│  └──────────────────┘  └──────────────────┘  └──────────────────┘  │
│                                                                      │
│  ┌──────────────────────────────────────────────────────────────┐   │
│  │  Open Shifts Available:                                       │   │
│  │  Sat Feb 15, Evening (17-23), Bar — 1 spot [Claim →]        │   │
│  │  Sun Feb 16, Morning (09-15), Terrace — 2 spots [Claim →]   │   │
│  └──────────────────────────────────────────────────────────────┘   │
│                                                                      │
│  Notifications:                                                      │
│  • Schedule for Feb 10–16 has been published                        │
│  • Your time-off request (Feb 20-22) was approved                   │
│                                                                      │
└─────────────────────────────────────────────────────────────────────┘
```

### 5.6 Timesheet Period Detail Page

```
┌─────────────────────────────────────────────────────────────────────┐
│  Timesheet: Feb 3–9, 2026  •  Status: Open  •  [Close Period]      │
├─────────────────────────────────────────────────────────────────────┤
│  Summary: 156 total hours | 12h overtime | CHF 5,840 estimated      │
├─────────────────────────────────────────────────────────────────────┤
│                                                                      │
│  ┌──────────────────────────────────────────────────────────────┐   │
│  │ Employee    │ Sched │ Actual │ Reg   │ OT   │ Night │ Status│   │
│  ├─────────────┼───────┼────────┼───────┼──────┼───────┼───────┤   │
│  │ Marie D.    │ 40.0  │ 41.5   │ 40.0  │ 1.5  │ 0.0   │ ✅   │   │
│  │ Pierre L.   │ 42.0  │ 44.0   │ 42.0  │ 2.0  │ 6.0   │ ⏳   │   │
│  │ Sophie M.   │ 24.0  │ 23.5   │ 23.5  │ 0.0  │ 0.0   │ ⚠️   │   │
│  │ Jean R.     │ 35.0  │ 38.0   │ 35.0  │ 3.0  │ 8.0   │ ✅   │   │
│  │ ...         │       │        │       │      │       │       │   │
│  └──────────────────────────────────────────────────────────────┘   │
│                                                                      │
│  Status Legend: ✅ Approved  ⏳ Pending  ⚠️ Disputed  🔒 Locked     │
│                                                                      │
│  [Export CSV] [Export PDF] [Approve All] [Lock Period]               │
│                                                                      │
└─────────────────────────────────────────────────────────────────────┘
```

### 5.7 Payroll Report Page

```
┌─────────────────────────────────────────────────────────────────────┐
│  Payroll Report                    Period: [Feb 2026 ▾] [Generate]  │
├─────────────────────────────────────────────────────────────────────┤
│  Totals: 14 employees | 2,184 hours | CHF 52,100 gross              │
├─────────────────────────────────────────────────────────────────────┤
│                                                                      │
│  ┌──────────────────────────────────────────────────────────────┐   │
│  │ Employee     │ Reg H │ OT H │ Night │ Wknd │ Rate │ Gross  │   │
│  ├──────────────┼───────┼──────┼───────┼──────┼──────┼────────┤   │
│  │ Marie D.     │ 168.0 │  6.5 │  0.0  │  0.0 │25.00 │4,406.25│   │
│  │ Pierre L.    │ 168.0 │ 12.0 │ 24.0  │  8.0 │30.00 │5,820.00│   │
│  │ Sophie M.    │  96.0 │  0.0 │  0.0  │ 16.0 │22.00 │2,464.00│   │
│  │ ...          │       │      │       │      │      │        │   │
│  ├──────────────┼───────┼──────┼───────┼──────┼──────┼────────┤   │
│  │ TOTALS       │1,890  │ 68.0 │ 96.0  │130.0 │  —   │52,100  │   │
│  └──────────────────────────────────────────────────────────────┘   │
│                                                                      │
│  ┌───────────────────────────┐  ┌────────────────────────────────┐  │
│  │  Cost by Department       │  │  Cost Trend (6 months)         │  │
│  │  [Pie Chart]              │  │  [Line Chart]                  │  │
│  │  FOH: 55%                 │  │                                │  │
│  │  BOH: 38%                 │  │                                │  │
│  │  Mgmt: 7%                 │  │                                │  │
│  └───────────────────────────┘  └────────────────────────────────┘  │
│                                                                      │
│  [Download CSV] [Download PDF] [Send to Accountant]                 │
│                                                                      │
└─────────────────────────────────────────────────────────────────────┘
```

### 5.8 Availability Editor (Employee Self-Service)

```
┌─────────────────────────────────────────────────────────────────────┐
│  My Availability                                        [Save All]  │
├─────────────────────────────────────────────────────────────────────┤
│                                                                      │
│  Weekly Pattern:                                                     │
│                                                                      │
│        │ Mon  │ Tue  │ Wed  │ Thu  │ Fri  │ Sat  │ Sun  │           │
│  ──────┼──────┼──────┼──────┼──────┼──────┼──────┼──────┤           │
│  06:00 │      │      │      │      │      │      │      │           │
│  07:00 │ ░░░░ │ ░░░░ │ ░░░░ │ ░░░░ │ ░░░░ │      │      │           │
│  08:00 │ ░░░░ │ ░░░░ │ ░░░░ │ ░░░░ │ ░░░░ │      │      │           │
│  09:00 │ ████ │ ████ │ ████ │ ████ │ ████ │ ████ │      │           │
│  10:00 │ ████ │ ████ │ ████ │ ████ │ ████ │ ████ │      │           │
│  ...   │ ████ │ ████ │ ████ │ ████ │ ████ │ ████ │      │           │
│  22:00 │ ████ │ ████ │ ████ │ ████ │ ████ │ ████ │      │           │
│  23:00 │      │      │      │      │      │      │      │           │
│                                                                      │
│  Legend: ████ Available  ░░░░ Preferred  ░░░░ Unavailable            │
│                                                                      │
│  Click and drag to set availability blocks.                          │
│                                                                      │
│  ─────────────────────────────────────────────────────────          │
│                                                                      │
│  One-off Overrides (upcoming):                                       │
│  ┌──────────────────────────────────────────────────────────────┐   │
│  │  Feb 15 (Sat) — Unavailable — "Family event"    [Delete]    │   │
│  │  Feb 22 (Sat) — Available 09-15 — "Can work this Saturday"  │   │
│  └──────────────────────────────────────────────────────────────┘   │
│  [+ Add Override]                                                    │
│                                                                      │
└─────────────────────────────────────────────────────────────────────┘
```

**Interaction:** The weekly grid is click-and-drag — click a cell to toggle, drag to paint a range. Similar to when2meet or Doodle, but for a permanent weekly pattern.

### 5.9 Time-Off Request Page (Manager View)

```
┌─────────────────────────────────────────────────────────────────────┐
│  Time-Off Requests               [Pending (5)] [Approved] [Denied]  │
├─────────────────────────────────────────────────────────────────────┤
│                                                                      │
│  ┌──────────────────────────────────────────────────────────────┐   │
│  │ ┌──────┐  Marie Dubois                                        │   │
│  │ │ Photo│  Vacation • Feb 20–22 (3 days)                      │   │
│  │ └──────┘  "Family ski trip"                                   │   │
│  │                                                                │   │
│  │  Staffing Impact:                                              │   │
│  │  • Feb 20 (Thu): 3 waiters remaining (need 3) ✅              │   │
│  │  • Feb 21 (Fri): 2 waiters remaining (need 4) ⚠️             │   │
│  │  • Feb 22 (Sat): 3 waiters remaining (need 4) ⚠️             │   │
│  │                                                                │   │
│  │  Leave Balance: 18 days remaining (of 25)                     │   │
│  │                                                                │   │
│  │  [✅ Approve]  [❌ Deny]                                      │   │
│  └──────────────────────────────────────────────────────────────┘   │
│                                                                      │
│  ┌──────────────────────────────────────────────────────────────┐   │
│  │  Pierre Laurent                                               │   │
│  │  Sick Leave • Feb 9 (1 day)                                   │   │
│  │  ...                                                          │   │
│  └──────────────────────────────────────────────────────────────┘   │
│                                                                      │
└─────────────────────────────────────────────────────────────────────┘
```

### 5.10 Settings Pages

#### Roles & Permissions Settings

```
┌─────────────────────────────────────────────────────────────────────┐
│  Roles & Permissions                                    [+ Add Role]│
├─────────────────────────────────────────────────────────────────────┤
│                                                                      │
│  ┌──────────────────────────────────────────────────────────────┐   │
│  │ Role        │ Dept │ Rate    │ Color │ Staff │ Actions       │   │
│  ├─────────────┼──────┼─────────┼───────┼───────┼───────────────┤   │
│  │ 🟠 Waiter   │ FOH  │ 25.00/h │ ██    │ 8     │ [Edit] [Perm]│   │
│  │ 🔵 Bartender│ FOH  │ 27.00/h │ ██    │ 3     │ [Edit] [Perm]│   │
│  │ 🟢 Chef     │ BOH  │ 32.00/h │ ██    │ 4     │ [Edit] [Perm]│   │
│  │ 🟡 Host     │ FOH  │ 23.00/h │ ██    │ 2     │ [Edit] [Perm]│   │
│  │ 🔴 Manager  │ Mgmt │ 38.00/h │ ██    │ 2     │ [Edit] [Perm]│   │
│  └──────────────────────────────────────────────────────────────┘   │
│                                                                      │
│  Permission Matrix (click [Perm] to edit):                          │
│  ┌──────────────────────────────────────────────────────────────┐   │
│  │ Permission          │ Waiter │ Chef │ Manager │ Admin        │   │
│  ├─────────────────────┼────────┼──────┼─────────┼──────────────┤   │
│  │ View Own Schedule   │   ✅   │  ✅  │   ✅    │   ✅         │   │
│  │ View All Schedules  │   ❌   │  ❌  │   ✅    │   ✅         │   │
│  │ Create Schedule     │   ❌   │  ❌  │   ✅    │   ✅         │   │
│  │ Approve Time-Off    │   ❌   │  ❌  │   ✅    │   ✅         │   │
│  │ View Payroll Data   │   ❌   │  ❌  │   ❌    │   ✅         │   │
│  │ Manage Employees    │   ❌   │  ❌  │   ✅    │   ✅         │   │
│  │ Manage Settings     │   ❌   │  ❌  │   ❌    │   ✅         │   │
│  │ View Audit Log      │   ❌   │  ❌  │   ❌    │   ✅         │   │
│  └──────────────────────────────────────────────────────────────┘   │
│                                                                      │
└─────────────────────────────────────────────────────────────────────┘
```

### 5.11 Team Chat Page (Inspired by 7shifts)

Full messaging interface with channel sidebar, message thread, and announcement feed.

```
┌─────────────────────────────────────────────────────────────────────┐
│  Team Chat                                              [+ Channel] │
├──────────┬──────────────────────────────────────────────────────────┤
│          │                                                           │
│ Channels │  # Kitchen Team                          3 members       │
│          │  ────────────────────────────────────────────────         │
│ ┌──────┐ │                                                           │
│ │# All │ │  Pierre L.                              10:15 AM         │
│ │Staff │ │  Can someone cover my prep station for 15 min?            │
│ │      │ │  I need to take a delivery.                               │
│ │# Kit-│ │                                                           │
│ │chen  │ │  Marie D.                               10:17 AM         │
│ │      │ │  I got it! Be back by 10:30.                             │
│ │# FOH │ │                                                           │
│ │      │ │  Pierre L.                              10:18 AM         │
│ │# Bar │ │  Thanks Marie! 🙏                                        │
│ └──────┘ │                                                           │
│          │                                                           │
│ Direct   │  ────────────────────────────────────────────────         │
│ Messages │                                                           │
│ ┌──────┐ │                                                           │
│ │Marie │ │  ┌──────────────────────────────────────────────────┐    │
│ │ (2)  │ │  │ Type a message...                    📎  📷  ➤  │    │
│ │Jean  │ │  └──────────────────────────────────────────────────┘    │
│ └──────┘ │                                                           │
└──────────┴──────────────────────────────────────────────────────────┘
```

**Mobile layout:** Channels list → tap → full-screen message thread → swipe back.

#### Announcement View

```
┌─────────────────────────────────────────────────────────────────────┐
│  Announcements                                    [+ New Announce]  │
├─────────────────────────────────────────────────────────────────────┤
│                                                                      │
│  ┌──────────────────────────────────────────────────────────────┐   │
│  │ 📢 Manager Jean R.                         Feb 9, 11:00 AM  │   │
│  │                                                                │   │
│  │ "Friday evening: Private event for 30 guests.                 │   │
│  │  Please wear black. Kitchen needs extra prep by 16:00."       │   │
│  │                                                                │   │
│  │ To: All Staff                                                  │   │
│  │ Read: 8/12 employees  [View who hasn't read →]                │   │
│  └──────────────────────────────────────────────────────────────┘   │
│                                                                      │
│  ┌──────────────────────────────────────────────────────────────┐   │
│  │ 📢 Admin Sophie M.                          Feb 7, 09:00 AM  │   │
│  │                                                                │   │
│  │ "New allergen policy effective immediately. All kitchen staff  │   │
│  │  must complete the online training by Feb 14."                 │   │
│  │                                                                │   │
│  │ To: Kitchen Department                                         │   │
│  │ Read: 4/4 employees  ✅                                       │   │
│  └──────────────────────────────────────────────────────────────┘   │
│                                                                      │
└─────────────────────────────────────────────────────────────────────┘
```

### 5.12 Manager Logbook Page (Inspired by 7shifts)

```
┌─────────────────────────────────────────────────────────────────────┐
│  Manager Logbook          [< Feb 8] [Feb 9, 2026] [Feb 10 >]       │
├─────────────────────────────────────────────────────────────────────┤
│                                                                      │
│  Auto Metrics (from POS):                                            │
│  ┌──────────────┐ ┌──────────────┐ ┌──────────────┐ ┌────────────┐ │
│  │ Sales: CHF   │ │ Covers:      │ │ Labor %:     │ │ SPLH:      │ │
│  │ 4,250        │ │ 85           │ │ 31.2%        │ │ CHF 48.50  │ │
│  │ ▲ 8% vs avg  │ │ ▲ 5%         │ │ ✅ under 33% │ │ ✅ target  │ │
│  └──────────────┘ └──────────────┘ └──────────────┘ └────────────┘ │
│                                                                      │
│  Shift Notes:                                                        │
│  ┌──────────────────────────────────────────────────────────────┐   │
│  │ Morning Shift — Jean R.                         09:15        │   │
│  │ Category: Operations                                          │   │
│  │ "Quiet morning. Terrace opened at 10:00, weather was good.   │   │
│  │  Walk-in cooler temp logged at 3.1°C. All tasks completed."  │   │
│  └──────────────────────────────────────────────────────────────┘   │
│                                                                      │
│  ┌──────────────────────────────────────────────────────────────┐   │
│  │ Evening Shift — Marie D.                        23:30        │   │
│  │ Category: Staff                                               │   │
│  │ "Busy evening, private event went well. Pierre left 30 min   │   │
│  │  early (sick). Sophie covered his station. Need to follow up."│   │
│  └──────────────────────────────────────────────────────────────┘   │
│                                                                      │
│  Tasks Status:                                                       │
│  Morning: ████████████ 12/12 (100%)                                 │
│  Evening: ████████░░░░  8/12 (67%)  — 4 overdue [View →]           │
│                                                                      │
│  [+ Add Entry]  [Search Past Logs]  [Email Digest →]                │
│                                                                      │
└─────────────────────────────────────────────────────────────────────┘
```

### 5.13 Task Management Page (Employee View — Mobile Optimized)

```
┌─────────────────────────────────┐
│  My Tasks — Evening Shift       │
│  Feb 9, 2026  •  4/8 done      │
├─────────────────────────────────┤
│                                  │
│  ████████░░░░  50%              │
│                                  │
│  Opening Tasks:                  │
│  ✅ Set up terrace tables       │
│  ✅ Check reservation list      │
│  ✅ Stock napkins/cutlery       │
│  ⬜ Check restroom supplies     │
│                                  │
│  Shift Tasks:                    │
│  ✅ Log walk-in cooler temp     │
│     → 3.1°C ✓ (photo attached) │
│  ⬜ Restock bar garnishes       │
│  ⬜ Wipe down menus             │
│                                  │
│  Closing Tasks:                  │
│  ⬜ Count cash drawer            │
│     📸 Requires: Photo + Value  │
│                                  │
│  [Mark All Done]                │
│                                  │
└─────────────────────────────────┘
```

**Task proof capture:**
```
┌─────────────────────────────────┐
│  Log walk-in cooler temp        │
│                                  │
│  📸 Take Photo (required)       │
│  ┌─────────────────────────┐   │
│  │                         │   │
│  │     [📷 Open Camera]    │   │
│  │                         │   │
│  └─────────────────────────┘   │
│                                  │
│  Temperature: [____] °C         │
│                                  │
│  ⚠️ Alert if > 5°C              │
│                                  │
│  [Submit Proof]                  │
│                                  │
└─────────────────────────────────┘
```

### 5.14 Employee Engagement Dashboard (Manager View)

```
┌─────────────────────────────────────────────────────────────────────┐
│  Employee Engagement                        Period: [Last 30 Days ▾]│
├─────────────────────────────────────────────────────────────────────┤
│                                                                      │
│  ┌──────────────────┐  ┌──────────────────┐  ┌──────────────────┐  │
│  │ 😊 Team Mood     │  │ 📊 Survey Rate   │  │ ⚠️ Flight Risks  │  │
│  │                   │  │                   │  │                   │  │
│  │  4.2 / 5.0       │  │  78% response     │  │  2 employees     │  │
│  │  ▲ 0.3 vs last   │  │  rate             │  │  flagged          │  │
│  │  month            │  │                   │  │  [Review →]       │  │
│  └──────────────────┘  └──────────────────┘  └──────────────────┘  │
│                                                                      │
│  ┌──────────────────────────────────────────────────────────────┐   │
│  │  Sentiment Trend (30 days)                                    │   │
│  │  ┌─────────────────────────────────────────────────────────┐ │   │
│  │  │  5 ─ · · · · · · · · · · · · · · · · · · · · · · · · · │ │   │
│  │  │  4 ─ ●───●───●───●───●───●───●───●───●   avg: 4.2    │ │   │
│  │  │  3 ─                                                     │ │   │
│  │  │  2 ─                                                     │ │   │
│  │  │  1 ─                                                     │ │   │
│  │  │    Jan 10   Jan 17   Jan 24   Jan 31   Feb 7            │ │   │
│  │  └─────────────────────────────────────────────────────────┘ │   │
│  └──────────────────────────────────────────────────────────────┘   │
│                                                                      │
│  ┌───────────────────────────────┐  ┌────────────────────────────┐  │
│  │  🏆 Recent Recognition        │  │  🎂 Upcoming Milestones    │  │
│  │                               │  │                            │  │
│  │  Marie → Pierre               │  │  Sophie M. — 90 days      │  │
│  │  "Amazing teamwork tonight!"  │  │  (Feb 12) [Celebrate →]   │  │
│  │  Category: Teamwork           │  │                            │  │
│  │                               │  │  Jean R. — 1 year         │  │
│  │  Jean → Sophie                │  │  (Feb 18) [Celebrate →]   │  │
│  │  "Great with the customers!"  │  │                            │  │
│  │  Category: Customer Service   │  │                            │  │
│  │                               │  │                            │  │
│  │  [See all →]                  │  │                            │  │
│  └───────────────────────────────┘  └────────────────────────────┘  │
│                                                                      │
│  ⚠️ Flight Risk Employees:                                          │
│  ┌──────────────────────────────────────────────────────────────┐   │
│  │  Pierre L. — Score: 2.8/5  ▼ declining 3 weeks              │   │
│  │  Signals: Survey scores dropped, 2 missed shifts, reduced    │   │
│  │  availability. Suggested: Schedule 1:1 meeting.              │   │
│  └──────────────────────────────────────────────────────────────┘   │
│                                                                      │
└─────────────────────────────────────────────────────────────────────┘
```

### 5.15 Demand Forecast Dashboard (Manager View — Phase 3)

```
┌─────────────────────────────────────────────────────────────────────┐
│  Demand Forecast & Labor Optimization     Location: [Main ▾]        │
├─────────────────────────────────────────────────────────────────────┤
│                                                                      │
│  ┌──────────────────┐  ┌──────────────────┐  ┌──────────────────┐  │
│  │ 📈 SPLH Today    │  │ 💰 Labor % Today │  │ 🌤️ Weather       │  │
│  │                   │  │                   │  │                   │  │
│  │  CHF 52.30       │  │  29.8%            │  │  ☀️ 12°C          │  │
│  │  Target: 48.00   │  │  Target: 30%      │  │  Good for terrace │  │
│  │  ✅ Above target │  │  ✅ Under target  │  │                   │  │
│  └──────────────────┘  └──────────────────┘  └──────────────────┘  │
│                                                                      │
│  ┌──────────────────────────────────────────────────────────────┐   │
│  │  Forecast vs. Actual (This Week)                              │   │
│  │  ┌─────────────────────────────────────────────────────────┐ │   │
│  │  │  CHF                                                     │ │   │
│  │  │  6k ─    ┌──┐                          ▓ Forecast       │ │   │
│  │  │  5k ─ ┌──┤░░├──┐    ┌──┐               ░ Actual        │ │   │
│  │  │  4k ─ │▓▓│░░│▓▓│ ┌──┤░░├──┐                            │ │   │
│  │  │  3k ─ │▓▓│░░│▓▓│ │▓▓│░░│▓▓│ ┌──┐                      │ │   │
│  │  │  2k ─ │▓▓│░░│▓▓│ │▓▓│░░│▓▓│ │▓▓│                      │ │   │
│  │  │       Mon      Tue      Wed   Thu   Fri   Sat   Sun     │ │   │
│  │  └─────────────────────────────────────────────────────────┘ │   │
│  │  Accuracy: 94.2%  (▲ 2.1% vs. last week)                    │   │
│  └──────────────────────────────────────────────────────────────┘   │
│                                                                      │
│  Staffing Recommendations (Next Week):                               │
│  ┌──────────────────────────────────────────────────────────────┐   │
│  │ Day       │ Forecast  │ Waiters │ Kitchen │ Bar │ Status     │   │
│  ├───────────┼───────────┼─────────┼─────────┼─────┼────────────┤   │
│  │ Mon Feb16 │ CHF 3,200 │ 3       │ 2       │ 1   │ ✅ Matched│   │
│  │ Tue Feb17 │ CHF 3,500 │ 3       │ 2       │ 1   │ ✅ Matched│   │
│  │ Fri Feb20 │ CHF 5,800 │ 4       │ 3       │ 2   │ ⚠️ Need 1 │   │
│  │ Sat Feb21 │ CHF 6,200 │ 5       │ 3       │ 2   │ ⚠️ Need 2 │   │
│  │           │ 🎪 Local  │ +1      │ +1      │ —   │ Event adj. │   │
│  └──────────────────────────────────────────────────────────────┘   │
│                                                                      │
│  [Apply Recommendations to Schedule →]                              │
│                                                                      │
└─────────────────────────────────────────────────────────────────────┘
```

### 5.16 Geofence Settings Page (Admin View)

```
┌─────────────────────────────────────────────────────────────────────┐
│  Geofence Settings                              Location: [Main ▾]  │
├─────────────────────────────────────────────────────────────────────┤
│                                                                      │
│  ┌──────────────────────────────────────────────────────────────┐   │
│  │                                                                │   │
│  │       ┌───────────────────────────────────────────────┐      │   │
│  │       │                                               │      │   │
│  │       │              [Interactive Map]                 │      │   │
│  │       │                                               │      │   │
│  │       │        ╭─────────────╮                        │      │   │
│  │       │       ╱    100m      ╲                       │      │   │
│  │       │      │    radius     │                       │      │   │
│  │       │      │   📍 Center   │                       │      │   │
│  │       │       ╲              ╱                        │      │   │
│  │       │        ╰─────────────╯                        │      │   │
│  │       │                                               │      │   │
│  │       └───────────────────────────────────────────────┘      │   │
│  │                                                                │   │
│  │  Drag the pin to set center. Drag the edge to adjust radius. │   │
│  └──────────────────────────────────────────────────────────────┘   │
│                                                                      │
│  Zone Settings:                                                      │
│  ┌──────────────────────────────────────────────────────────────┐   │
│  │  Name:    [Main Restaurant Zone              ]                │   │
│  │  Center:  47.3769° N, 8.5417° E   [Set from map]            │   │
│  │  Radius:  [100] meters  (range: 50-500m)                    │   │
│  │  Status:  ● Active                                            │   │
│  └──────────────────────────────────────────────────────────────┘   │
│                                                                      │
│  Clock-In Options:                                                   │
│  ☑ Require GPS within geofence for clock-in                        │
│  ☑ Allow clock-in from restaurant WiFi IP (fallback)               │
│  ☐ Require photo verification (selfie)                              │
│  ☑ Alert manager for out-of-zone attempts                          │
│                                                                      │
│  Recent Clock Locations:  [View Map →]                              │
│  ✅ 12 clock-ins inside zone today                                  │
│  ⚠️ 1 attempt outside zone (Pierre, 09:12 — 150m away)            │
│                                                                      │
│  [Save Zone]                                                        │
│                                                                      │
└─────────────────────────────────────────────────────────────────────┘
```

### 5.17 Kiosk Clock Mode (Shared Tablet)

```
┌─────────────────────────────────────────────────────────────────────┐
│                                                                      │
│                         STAFF PRO                                    │
│                    La Dolce Vita Restaurant                          │
│                                                                      │
│                    Monday, Feb 9, 2026                               │
│                       14:32:05                                       │
│                                                                      │
│                                                                      │
│              ┌─────────────────────────────┐                        │
│              │                             │                        │
│              │   Enter your PIN to         │                        │
│              │   clock in / out            │                        │
│              │                             │                        │
│              │   ┌───┐ ┌───┐ ┌───┐       │                        │
│              │   │ 1 │ │ 2 │ │ 3 │       │                        │
│              │   └───┘ └───┘ └───┘       │                        │
│              │   ┌───┐ ┌───┐ ┌───┐       │                        │
│              │   │ 4 │ │ 5 │ │ 6 │       │                        │
│              │   └───┘ └───┘ └───┘       │                        │
│              │   ┌───┐ ┌───┐ ┌───┐       │                        │
│              │   │ 7 │ │ 8 │ │ 9 │       │                        │
│              │   └───┘ └───┘ └───┘       │                        │
│              │         ┌───┐              │                        │
│              │         │ 0 │              │                        │
│              │         └───┘              │                        │
│              │                             │                        │
│              │   ── or scan QR code ──    │                        │
│              │                             │                        │
│              └─────────────────────────────┘                        │
│                                                                      │
│  Currently clocked in: Marie D., Pierre L., Sophie M., Jean R.     │
│                                                                      │
└─────────────────────────────────────────────────────────────────────┘
```

After PIN entry:
```
┌─────────────────────────────────────────────────────────────────────┐
│                                                                      │
│                    Welcome, Marie Dubois                             │
│                                                                      │
│              Today's Shift: Morning (09:00 – 15:00)                 │
│              Station: Terrace  •  Role: Waiter                      │
│                                                                      │
│              ┌─────────────────────────────┐                        │
│              │                             │                        │
│              │       🟢 CLOCK IN          │                        │
│              │                             │                        │
│              └─────────────────────────────┘                        │
│                                                                      │
│              (Screen returns to PIN entry after 10 seconds)         │
│                                                                      │
└─────────────────────────────────────────────────────────────────────┘
```

### 5.18 Enhanced Schedule Builder (with Forecast Overlay — Phase 3)

The existing schedule builder (5.1) gains additional overlays when POS data and forecasting are enabled:

```
┌─────────────────────────────────────────────────────────────────────┐
│  ScheduleToolbar                                                     │
│  [< Prev Week] [Feb 10–16, 2026] [Next Week >]  [Filter ▾] [Publish]│
│  [👁 Overlays: ☑ Forecast  ☑ Weather  ☑ Labor%  ☐ Events]          │
├──────────┬──────────────────────────────────────────────────────────┤
│          │  Forecast │  Mon 10  │  Tue 11  │  Wed 12  │ ...        │
│          │  Bar      │  CHF 3.2k│  CHF 3.5k│  CHF 4.1k│           │
│          │  Weather  │  ☀️ 14°  │  🌧️ 8°  │  ☀️ 12°  │           │
│ Employee ├──────────┼──────────┼──────────┼──────────┤             │
│ Sidebar  │          │          │          │          │             │
│          │  (normal schedule grid with shift cards)  │             │
│          │                                                          │
│          ├──────────┴──────────┴──────────┴──────────┤             │
│          │  StaffingCoverageBar                       │             │
│          │  Waiters: ████████░░  3/4 (Forecast: 4)   │             │
│          ├───────────────────────────────────────────┤             │
│          │  LaborCostSummary                         │             │
│          │  Projected: CHF 4,250 │ Revenue: CHF 14k  │             │
│          │  Labor %: 30.4%  │  Target: 30%  │  ✅    │             │
│          │  SPLH: CHF 48.20 │  Target: 48   │  ✅    │             │
└──────────┴───────────────────────────────────────────┘
```

### 5.19 Auto-Scheduling Wizard (Manager View)

```
┌─────────────────────────────────────────────────────────────────────┐
│  Auto-Schedule — Week of Feb 16-22           [Cancel]  [Generate →]│
├─────────────────────────────────────────────────────────────────────┤
│                                                                      │
│  Step 1: Choose Strategy                                            │
│  ┌────────────────┐  ┌────────────────┐  ┌────────────────┐        │
│  │ 💰 Cost-       │  │ 😊 Employee-   │  │ ⚖️ Balanced    │        │
│  │  Optimized     │  │  Preferred     │  │  (Default)     │        │
│  │                │  │                │  │  ▶ SELECTED    │        │
│  │ Minimize labor │  │ Maximize       │  │  Balance cost  │        │
│  │ cost while     │  │ preference     │  │  + preferences │        │
│  │ meeting min    │  │ satisfaction   │  │  + fairness    │        │
│  │ staffing       │  │ + fairness    │  │                │        │
│  └────────────────┘  └────────────────┘  └────────────────┘        │
│                                                                      │
│  Step 2: Configure Weights (Advanced)                  [▼ Expand]  │
│  ┌──────────────────────────────────────────────────────────────┐   │
│  │ Availability match ████████████████████ 100 (locked)        │   │
│  │ Role/skill match   ████████████████████ 100 (locked)        │   │
│  │ Hours fairness     ████████████████░░░░  80                 │   │
│  │ Cost optimization  █████████████░░░░░░░  65                 │   │
│  │ Preference honor   ████████████░░░░░░░░  60                 │   │
│  │ Seniority weight   ██████░░░░░░░░░░░░░░  30                 │   │
│  └──────────────────────────────────────────────────────────────┘   │
│                                                                      │
│  Step 3: Data Sources                                               │
│  ☑ Use demand forecast (POS data available: 12 weeks)              │
│  ☑ Include shift bids (7 bids received from 5 employees)           │
│  ☑ Apply shift preferences (14 employees have set preferences)     │
│  ☐ Use schedule template: [Select template ▾]                      │
│                                                                      │
│  [Generate Preview →]                                               │
│                                                                      │
├─────────────────────────────────────────────────────────────────────┤
│  Preview Results:                                                    │
│  ┌──────────────────────────────────────────────────────────────┐   │
│  │  ✅ All staffing requirements met                            │   │
│  │  📊 Projected labor cost: CHF 12,450 (30.2% of forecast)    │   │
│  │  😊 Preference satisfaction: 78% (11/14 honored)            │   │
│  │  ⚖️  Fairness score: 92/100 (max variance: 2.5 hours)       │   │
│  │  ⚠️  2 warnings: Pierre L. gets 3 closing shifts            │   │
│  │  🚫 0 hard conflicts                                        │   │
│  └──────────────────────────────────────────────────────────────┘   │
│                                                                      │
│  (Full schedule grid preview shown below with generated assignments) │
│                                                                      │
│  [← Back]  [Adjust & Regenerate]  [Apply to Draft →]               │
│                                                                      │
└─────────────────────────────────────────────────────────────────────┘
```

**Mobile:** Steps collapse into a stepper (1→2→3→Preview). Grid preview is swipeable day-by-day.

### 5.20 Hiring Pipeline Page (Manager View — ATS)

```
┌─────────────────────────────────────────────────────────────────────┐
│  Hiring                    [+ Create Job Posting]  [Analytics →]    │
├─────────────────────────────────────────────────────────────────────┤
│                                                                      │
│  Active Postings:                                                    │
│  ┌──────────────────────────────────────────────────────────────┐   │
│  │ 🟢 Experienced Waiter — Main Location       12 applicants   │   │
│  │    Posted: Feb 1  •  Source: Indeed, Glassdoor              │   │
│  │    [View Pipeline →]  [Pause]  [Close]                      │   │
│  ├──────────────────────────────────────────────────────────────┤   │
│  │ 🟢 Line Cook — Main Location                 5 applicants   │   │
│  │    Posted: Feb 5  •  Source: Jobs.ch, Direct               │   │
│  │    [View Pipeline →]  [Pause]  [Close]                      │   │
│  └──────────────────────────────────────────────────────────────┘   │
│                                                                      │
│  Pipeline (Waiter — 12 applicants):                   [Kanban|List] │
│  ┌──────────┬──────────┬──────────┬──────────┬──────────┐          │
│  │ Applied  │ Screening│ Interview│ Trial    │ Offer    │          │
│  │   (5)    │   (3)    │   (2)    │   (1)    │   (1)    │          │
│  ├──────────┼──────────┼──────────┼──────────┼──────────┤          │
│  │ ┌──────┐│ ┌──────┐ │ ┌──────┐ │ ┌──────┐ │ ┌──────┐ │          │
│  │ │Anna K││ │Marc L│ │ │Julie │ │ │Sophie│ │ │Pierre│ │          │
│  │ │AI: 85││ │AI: 72│ │ │Feb 12│ │ │Feb 14│ │ │Sent  │ │          │
│  │ │Indeed ││ │Glass.│ │ │2:00pm│ │ │Eve.  │ │ │Feb 8 │ │          │
│  │ └──────┘│ └──────┘ │ └──────┘ │ └──────┘ │ └──────┘ │          │
│  │ ┌──────┐│ ┌──────┐ │ ┌──────┐ │          │          │          │
│  │ │Tom R ││ │Lea S │ │ │Marc P│ │          │          │          │
│  │ │AI: 78││ │AI: 68│ │ │Feb 13│ │          │          │          │
│  │ └──────┘│ └──────┘ │ └──────┘ │          │          │          │
│  └──────────┴──────────┴──────────┴──────────┴──────────┘          │
│                                                                      │
│  (Drag cards between columns to advance stage)                      │
│                                                                      │
└─────────────────────────────────────────────────────────────────────┘
```

**Applicant Detail Modal:**
```
┌─────────────────────────────────────────────────────────────────────┐
│  Anna Kovacs — Waiter Application                         [× Close]│
├─────────────────────────────────────────────────────────────────────┤
│                                                                      │
│  AI Score: 85/100 ████████████████░░░░                              │
│  ├─ Availability match: 92%  (weekends ✅, evenings ✅)             │
│  ├─ Certifications: Food Safety ✅, Alcohol ✅                      │
│  ├─ Experience: 3 years restaurant (keywords matched)               │
│  └─ Location: 2.3 km from restaurant                               │
│                                                                      │
│  📄 Resume: [View PDF]     📧 anna.k@email.com                    │
│  📞 +41 79 123 4567        📍 Zürich                              │
│  Source: Indeed  •  Applied: Feb 3, 2026                           │
│                                                                      │
│  Stage History:                                                      │
│  ● Applied — Feb 3  ● Screening — Feb 5  ○ Interview  ○ Offer     │
│                                                                      │
│  Notes:                                                              │
│  Jean R. (Feb 5): "Strong experience, available weekends. Schedule  │
│  interview."                                                         │
│                                                                      │
│  [Schedule Interview]  [Move to →]  [Reject]  [Add Note]           │
│                                                                      │
└─────────────────────────────────────────────────────────────────────┘
```

### 5.21 Training & LMS Page (Employee View — Mobile Optimized)

```
┌─────────────────────────────────┐
│  My Training                    │
│  3 courses assigned  •  1 due   │
├─────────────────────────────────┤
│                                  │
│  🔴 OVERDUE                     │
│  ┌─────────────────────────────┐│
│  │ 🍳 Food Safety Level 2     ││
│  │ Due: Feb 7 (2 days overdue) ││
│  │ Progress: ████████░░ 80%   ││
│  │ 3/4 modules  •  Quiz next  ││
│  │ [Continue →]                ││
│  └─────────────────────────────┘│
│                                  │
│  📚 IN PROGRESS                 │
│  ┌─────────────────────────────┐│
│  │ 🍷 Wine & Spirits Basics   ││
│  │ Due: Feb 28                 ││
│  │ Progress: ██████░░░░ 50%   ││
│  │ 2/4 modules                 ││
│  │ [Continue →]                ││
│  └─────────────────────────────┘│
│                                  │
│  ✅ COMPLETED                   │
│  ┌─────────────────────────────┐│
│  │ 🧹 Hygiene & Allergens     ││
│  │ Completed: Jan 15           ││
│  │ Quiz: 95% ✅               ││
│  │ [View Certificate →]       ││
│  └─────────────────────────────┘│
│                                  │
│  📖 Knowledge Base             │
│  [Search articles...]          │
│  • Equipment Guides (12)       │
│  • Menu Knowledge (8)          │
│  • HR Policies (5)             │
│                                  │
└─────────────────────────────────┘
```

**Course Viewer:**
```
┌─────────────────────────────────┐
│  Food Safety Level 2            │
│  Module 3: Temperature Control  │
├─────────────────────────────────┤
│                                  │
│  Progress: Module 3 of 4        │
│  ○ ● ● ◉ ○                     │
│                                  │
│  📹 [Video: Proper Temperature  │
│       Storage — 5:32]          │
│                                  │
│  📝 Content:                    │
│  Cold food must be stored below │
│  5°C at all times. The danger   │
│  zone is 5°C to 60°C where     │
│  bacteria multiply rapidly...   │
│                                  │
│  📥 Download: Temperature Log   │
│     Reference Card (PDF)       │
│                                  │
│  ────────────────────────────   │
│  ✅ Quiz: Temperature Control  │
│  5 questions • Pass: 80%       │
│  Time limit: 10 minutes        │
│  [Start Quiz →]                │
│                                  │
│  [← Previous Module]           │
│  [Next Module →]               │
│                                  │
└─────────────────────────────────┘
```

### 5.22 Digital Forms Page (Employee View — Mobile Optimized)

```
┌─────────────────────────────────┐
│  My Forms                       │
│  2 pending  •  1 overdue       │
├─────────────────────────────────┤
│                                  │
│  🔴 OVERDUE                     │
│  ┌─────────────────────────────┐│
│  │ 🌡️ Daily Temperature Log    ││
│  │ Due: Today 10:00 AM        ││
│  │ (45 min overdue)           ││
│  │ [Fill Out Now →]           ││
│  └─────────────────────────────┘│
│                                  │
│  📋 PENDING                     │
│  ┌─────────────────────────────┐│
│  │ 🔒 Closing Checklist       ││
│  │ Due: Today 23:30            ││
│  │ [Fill Out →]               ││
│  └─────────────────────────────┘│
│                                  │
│  ✅ COMPLETED TODAY             │
│  ┌─────────────────────────────┐│
│  │ ☀️ Opening Checklist        ││
│  │ Submitted: 09:05 AM        ││
│  │ [View Submission →]        ││
│  └─────────────────────────────┘│
│                                  │
└─────────────────────────────────┘
```

**Form Filling (Example: Temperature Log):**
```
┌─────────────────────────────────┐
│  Daily Temperature Log          │
│  📍 Location captured          │
├─────────────────────────────────┤
│                                  │
│  Walk-in Cooler:                │
│  Temperature: [3.2] °C         │
│  ✅ Within range (0-5°C)       │
│                                  │
│  📸 Take Photo (required)      │
│  ┌─────────────────────────┐   │
│  │  [photo of thermometer] │   │
│  │  ✅ Captured 09:15 AM   │   │
│  └─────────────────────────┘   │
│                                  │
│  Walk-in Freezer:               │
│  Temperature: [-18.5] °C       │
│  ✅ Within range (-25 to -15°C)│
│                                  │
│  Prep Station Fridge:           │
│  Temperature: [6.8] °C         │
│  ⚠️ ABOVE THRESHOLD (>5°C)     │
│  ⚠️ Alert will be sent to mgr  │
│                                  │
│  Notes: [Fridge needs service, │
│  temp rising since yesterday]  │
│                                  │
│  ✍️ Signature:                  │
│  ┌─────────────────────────┐   │
│  │  [signature pad]        │   │
│  └─────────────────────────┘   │
│                                  │
│  [Submit Form]                  │
│                                  │
└─────────────────────────────────┘
```

### 5.23 Company Newsfeed Page

```
┌─────────────────────────────────────────────────────────────────────┐
│  Company Feed                                   [+ New Post]        │
├─────────────────────────────────────────────────────────────────────┤
│                                                                      │
│  📌 PINNED                                                          │
│  ┌──────────────────────────────────────────────────────────────┐   │
│  │ 📢 Manager Jean R.                              Feb 7       │   │
│  │ "This week's specials: Truffle risotto (CHF 32), Catch of   │   │
│  │  the day (market price). Please memorize descriptions!"      │   │
│  │                                                                │   │
│  │  ❤️ 8   🎉 3   💬 2 comments                                 │   │
│  └──────────────────────────────────────────────────────────────┘   │
│                                                                      │
│  🎂 MILESTONE                                                       │
│  ┌──────────────────────────────────────────────────────────────┐   │
│  │ 🎉 Sophie M. just reached 90 days!                           │   │
│  │ Congratulations on completing your probation period! 🥳      │   │
│  │                                                                │   │
│  │  ❤️ 12   🎉 8   🎊 5   💬 6 comments                        │   │
│  │  ├─ "Welcome to the team officially!" — Jean R.              │   │
│  │  ├─ "You're doing amazing!" — Marie D.                       │   │
│  │  └─ [View all 6 comments]                                    │   │
│  └──────────────────────────────────────────────────────────────┘   │
│                                                                      │
│  🏆 RECOGNITION                                                     │
│  ┌──────────────────────────────────────────────────────────────┐   │
│  │ Marie D. recognized Pierre L. — Teamwork                     │   │
│  │ "Covered my station without even being asked when I was      │   │
│  │  overwhelmed during the lunch rush. Real team player!"       │   │
│  │                                                                │   │
│  │  ❤️ 6   🎉 4   💬 1 comment                                  │   │
│  └──────────────────────────────────────────────────────────────┘   │
│                                                                      │
│  📝 GENERAL                                                         │
│  ┌──────────────────────────────────────────────────────────────┐   │
│  │ Admin Sophie M.                                  Feb 9       │   │
│  │ "Team photo from Saturday's private event! Great job         │   │
│  │  everyone." 📸 [3 photos]                                    │   │
│  │                                                                │   │
│  │  ❤️ 15   😄 3   💬 4 comments                                │   │
│  └──────────────────────────────────────────────────────────────┘   │
│                                                                      │
│  [Load more...]                                                      │
│                                                                      │
└─────────────────────────────────────────────────────────────────────┘
```

**Mobile:** Full-width cards, reactions at bottom of each card, pull-to-refresh, infinite scroll.

### 5.24 Employee Earnings & Self-Service Page

```
┌─────────────────────────────────┐
│  My Earnings                    │
├─────────────────────────────────┤
│                                  │
│  This Week (Feb 10-16):         │
│  ┌─────────────────────────────┐│
│  │  Estimated Gross Pay:       ││
│  │  CHF 1,312.50               ││
│  │                              ││
│  │  Regular: 35h × CHF 25.00   ││
│  │         = CHF 875.00        ││
│  │  Overtime: 5h × CHF 31.25   ││
│  │         = CHF 156.25        ││
│  │  Night:   4h × CHF 28.75    ││
│  │         = CHF 115.00        ││
│  │  Est. Tips: ~CHF 166.25     ││
│  │  (avg CHF 4.75/h worked)    ││
│  └─────────────────────────────┘│
│                                  │
│  Hours This Period:              │
│  ┌─────────────────────────────┐│
│  │  Worked: 28h / 44h sched.  ││
│  │  ████████████░░░░░░ 64%    ││
│  │                              ││
│  │  Overtime threshold: 42h    ││
│  │  ██████████████████░░       ││
│  │  "14h until overtime"       ││
│  └─────────────────────────────┘│
│                                  │
│  Earnings History:               │
│  ┌─────────────────────────────┐│
│  │  📊 [6-month trend chart]  ││
│  │  Jan:  CHF 4,850            ││
│  │  Dec:  CHF 5,120 (holiday)  ││
│  │  Nov:  CHF 4,680            ││
│  │  Oct:  CHF 4,790            ││
│  │  [View full history →]      ││
│  └─────────────────────────────┘│
│                                  │
│  Leave Balances:                 │
│  Vacation: 8.5 / 25 days used  │
│  Sick: 2 / unlimited           │
│  Next accrual: Mar 1 (+2.08d)  │
│                                  │
│  [My Documents] [My Profile]    │
│                                  │
└─────────────────────────────────┘
```

### 5.25 Performance Review Page (Employee View)

```
┌─────────────────────────────────────────────────────────────────────┐
│  My Performance Reviews                                              │
├─────────────────────────────────────────────────────────────────────┤
│                                                                      │
│  🔵 SELF-ASSESSMENT DUE                                             │
│  ┌──────────────────────────────────────────────────────────────┐   │
│  │  90-Day Probation Review                                      │   │
│  │  Period: Nov 15, 2025 – Feb 15, 2026                         │   │
│  │  Reviewer: Manager Jean R.                                    │   │
│  │  Self-assessment due: Feb 12, 2026                            │   │
│  │                                                                │   │
│  │  Your Data This Period (auto-populated):                      │   │
│  │  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐       │   │
│  │  │ Attendance   │  │ Punctuality  │  │ Tasks Done   │       │   │
│  │  │ 97% ✅       │  │ Avg +1 min   │  │ 94% ✅       │       │   │
│  │  └──────────────┘  └──────────────┘  └──────────────┘       │   │
│  │  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐       │   │
│  │  │ Recognitions │  │ Survey Avg   │  │ Hours vs     │       │   │
│  │  │ 5 received   │  │ 4.3/5.0      │  │ Contract     │       │   │
│  │  │              │  │              │  │ 102% ✅       │       │   │
│  │  └──────────────┘  └──────────────┘  └──────────────┘       │   │
│  │                                                                │   │
│  │  Self-Assessment:                                              │   │
│  │  Punctuality:     [⭐⭐⭐⭐⭐] 5/5                            │   │
│  │  Customer Service: [⭐⭐⭐⭐☆] 4/5                            │   │
│  │  Teamwork:         [⭐⭐⭐⭐⭐] 5/5                            │   │
│  │  Technical Skills: [⭐⭐⭐⭐☆] 4/5                            │   │
│  │  Initiative:       [⭐⭐⭐☆☆] 3/5                            │   │
│  │  Comments: [I've improved a lot in wine service this         │   │
│  │            period and would like to pursue sommelier...]      │   │
│  │                                                                │   │
│  │  [Save Draft]  [Submit Self-Assessment →]                     │   │
│  └──────────────────────────────────────────────────────────────┘   │
│                                                                      │
│  ✅ COMPLETED REVIEWS                                                │
│  ┌──────────────────────────────────────────────────────────────┐   │
│  │  30-Day Check-In — Completed Dec 18, 2025                    │   │
│  │  Overall: Meets Expectations                                  │   │
│  │  [View Review]  [View Goals]                                  │   │
│  └──────────────────────────────────────────────────────────────┘   │
│                                                                      │
│  My Goals:                                                           │
│  ✅ Complete Food Safety Level 2 by Jan 31 — Done ✓               │
│  🔵 Take on 2 closing shifts per week — In Progress (1.5 avg)     │
│  ⬜ Begin sommelier certification — Not Started                    │
│                                                                      │
└─────────────────────────────────────────────────────────────────────┘
```

### 5.26 Intraday Labor Dashboard (Manager View — Real-Time)

```
┌─────────────────────────────────────────────────────────────────────┐
│  Live Labor Dashboard          Today: Feb 9, 2026  •  Updated: Now │
├─────────────────────────────────────────────────────────────────────┤
│                                                                      │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐  ┌────────┐ │
│  │ 💰 Sales     │  │ 👥 Labor     │  │ 📊 Labor %   │  │ SPLH   │ │
│  │ CHF 2,840    │  │ CHF 892      │  │   31.4%      │  │ 45.80  │ │
│  │ (so far)     │  │ (so far)     │  │ ⚠️ target 30%│  │ target │ │
│  │ ▲ 12% vs avg │  │ 8 clocked in │  │              │  │ 48.00  │ │
│  └──────────────┘  └──────────────┘  └──────────────┘  └────────┘ │
│                                                                      │
│  ┌──────────────────────────────────────────────────────────────┐   │
│  │  Hourly Breakdown (Sales vs. Labor)                          │   │
│  │                                                                │   │
│  │  500 ─ █                                                      │   │
│  │  400 ─ █ █   █                                    sales ■    │   │
│  │  300 ─ █ █ █ █ █                                  labor □    │   │
│  │  200 ─ █□█□█□█□█□                                            │   │
│  │  100 ─ █□█□█□█□█□█□█□                                        │   │
│  │    0 ─ 09 10 11 12 13 14 15 16 17 18 19 20 21 22            │   │
│  │        ─────── actual ─────── │ ── forecast ──               │   │
│  └──────────────────────────────────────────────────────────────┘   │
│                                                                      │
│  ┌─────────────────────────────────┐ ┌───────────────────────────┐ │
│  │  Earned vs. Actual Hours        │ │  Currently Clocked In     │ │
│  │                                  │ │                           │ │
│  │  Earned (from sales): 58.2h     │ │  🟢 Marie D. — Waiter    │ │
│  │  Actual (worked):     62.0h     │ │  🟢 Pierre L. — Waiter   │ │
│  │  Variance: +3.8h (overstaffed)  │ │  🟢 Jean R. — Manager    │ │
│  │                                  │ │  🟢 Sophie M. — Chef     │ │
│  │  By Department:                  │ │  🟢 Lea S. — Bartender   │ │
│  │  FOH: +2.1h over    🔴         │ │  🟢 Tom R. — Line Cook   │ │
│  │  BOH: +0.5h over    🟡         │ │  🟢 Anna K. — Host       │ │
│  │  Mgmt: +1.2h over   🟡         │ │  🟡 Marc L. — Prep Cook  │ │
│  │                                  │ │     (overtime in 1.5h)   │ │
│  └─────────────────────────────────┘ └───────────────────────────┘ │
│                                                                      │
│  ⚠️ Alerts:                                                         │
│  • Labor % exceeded target for 2 consecutive hours (12:00-14:00)   │
│  • Marc L. approaching overtime threshold (40h) — 1.5h remaining   │
│  • No bartender scheduled after 22:00 (bar closes at 23:00)        │
│                                                                      │
└─────────────────────────────────────────────────────────────────────┘
```

### 5.27 Employee Onboarding Portal (New Hire Self-Service)

```
┌─────────────────────────────────┐
│  Welcome to La Dolce Vita! 🎉   │
│  Your Onboarding Progress       │
│  ████████████░░░░ 60%           │
├─────────────────────────────────┤
│                                  │
│  Step 1: Personal Details   ✅  │
│  Completed Feb 8                │
│                                  │
│  Step 2: Documents          ✅  │
│  ├─ ID Copy: Uploaded ✅        │
│  ├─ Work Permit: Uploaded ✅    │
│  └─ Food Safety Cert: ⬜       │
│     [Upload Certificate]       │
│                                  │
│  Step 3: Policies           🔵  │
│  ├─ Employee Handbook: ✅ Read  │
│  ├─ Dress Code: ✅ Acknowledged │
│  ├─ Data Privacy: ⬜ Pending   │
│  │   [Read & Acknowledge →]    │
│  └─ Health & Safety: ⬜ Pending│
│     [Read & Acknowledge →]     │
│                                  │
│  Step 4: Training           ⬜  │
│  ├─ Food Safety Level 2        │
│  │   [Start Course →]         │
│  ├─ Allergen Awareness         │
│  │   [Start Course →]         │
│  └─ Customer Service Basics    │
│     [Start Course →]          │
│                                  │
│  Step 5: Manager Checklist  ⬜  │
│  (Completed by your manager)   │
│  ├─ ⬜ Uniform issued          │
│  ├─ ⬜ Locker assigned         │
│  ├─ ⬜ Systems access granted  │
│  └─ ⬜ Buddy assigned          │
│                                  │
│  Questions? Chat with Jean R.  │
│  [Open Chat →]                  │
│                                  │
└─────────────────────────────────┘
```

### 5.28 Form Builder Page (Manager/Admin View)

```
┌─────────────────────────────────────────────────────────────────────┐
│  Form Builder — Daily Temperature Log                    [Preview] │
├─────────────────────────────────────────────────────────────────────┤
│                                                                      │
│  ┌── Field Palette ──┐  ┌── Form Canvas ──────────────────────┐    │
│  │                    │  │                                      │    │
│  │ 📝 Text Input     │  │  ┌── Section: Walk-in Cooler ─────┐ │    │
│  │ 🔢 Number Input   │  │  │                                  │ │    │
│  │ 📅 Date Picker    │  │  │  🌡️ Temperature (°C)            │ │    │
│  │ ▾  Dropdown       │  │  │  Type: Number │ Required: ✅    │ │    │
│  │ ☐  Checkboxes     │  │  │  Alert if > 5°C  ⚠️            │ │    │
│  │ ◉  Radio Buttons  │  │  │  [Edit] [Delete] [⬆⬇ Move]    │ │    │
│  │ 📸 Photo Capture  │  │  │                                  │ │    │
│  │ 📎 File Upload    │  │  │  📸 Photo of Thermometer        │ │    │
│  │ ✍️ Signature      │  │  │  Type: Photo │ Required: ✅     │ │    │
│  │ 📍 GPS Location   │  │  │  [Edit] [Delete] [⬆⬇ Move]    │ │    │
│  │ 🌡️ Temperature    │  │  │                                  │ │    │
│  │ ✅ Yes/No Toggle  │  │  └──────────────────────────────────┘ │    │
│  │ ⭐ Rating (1-5)   │  │                                      │    │
│  │ ── Section Header │  │  ┌── Section: Walk-in Freezer ────┐ │    │
│  │ 📄 Instructions   │  │  │  (similar fields...)            │ │    │
│  │                    │  │  └──────────────────────────────────┘ │    │
│  │ [Drag fields to   │  │                                      │    │
│  │  canvas →]        │  │  [+ Add Section]                    │    │
│  └────────────────────┘  └──────────────────────────────────────┘    │
│                                                                      │
│  Settings:                                                           │
│  Category: [Food Safety ▾]  Schedule: [Daily ▾]                    │
│  Assign to: [Kitchen Staff ▾]  GPS Required: [✅]                 │
│                                                                      │
│  [Save Draft]  [Publish Form]                                       │
│                                                                      │
└─────────────────────────────────────────────────────────────────────┘
```

---

## 6. State Management Architecture

### 6.1 RTK Query API Layer

Instead of manually writing thunks and reducers for every API call, Staff Pro uses **RTK Query** — Redux Toolkit's built-in data fetching and caching solution. This eliminates boilerplate and provides automatic cache invalidation, loading states, and error handling.

```typescript
// src/api/baseApi.ts
import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';

export const baseApi = createApi({
  reducerPath: 'api',
  baseQuery: fetchBaseQuery({
    baseUrl: import.meta.env.VITE_API_BASE_URL,
    prepareHeaders: (headers, { getState }) => {
      const token = (getState() as RootState).auth.accessToken;
      if (token) headers.set('Authorization', `Bearer ${token}`);
      return headers;
    },
  }),
  tagTypes: [
    'Employee', 'Employees',
    'Schedule', 'Schedules',
    'ShiftAssignment', 'ShiftAssignments',
    'TimeOffRequest', 'TimeOffRequests',
    'TimesheetPeriod', 'TimesheetEntry',
    'Notification', 'Notifications',
    'Role', 'Roles',
    'Station', 'Stations',
    'ShiftTemplate',
    'LeaveType',
    'AvailabilityRule',
    'OpenShift',
    'ShiftSwap',
    'ClockEntry',
    // --- New modules ---
    'JobPosting', 'JobApplication', 'Interview',     // Hiring / ATS
    'TrainingCourse', 'TrainingAssignment', 'QuizAttempt', // Training / LMS
    'KnowledgeBaseArticle', 'HandbookSection',        // Knowledge Base
    'FormTemplate', 'FormSubmission',                  // Digital Forms
    'FeedPost',                                        // Company Newsfeed
    'PerformanceReview', 'ReviewTemplate', 'Goal',     // Performance Reviews
    'ScheduleTemplate', 'ShiftBid', 'ShiftPreference', // Auto-Scheduling
    'EarningsEstimate',                                 // Self-Service
    'IntradayDashboard',                                // Labor Dashboard
  ],
  endpoints: () => ({}),
});
```

```typescript
// src/api/schedulesApi.ts
import { baseApi } from './baseApi';

export const schedulesApi = baseApi.injectEndpoints({
  endpoints: (builder) => ({
    getSchedulePeriods: builder.query<SchedulePeriod[], ScheduleFilter>({
      query: (filter) => ({ url: '/schedules', params: filter }),
      providesTags: ['Schedules'],
    }),
    getSchedulePeriod: builder.query<SchedulePeriodDetail, string>({
      query: (id) => `/schedules/${id}`,
      providesTags: (result, error, id) => [{ type: 'Schedule', id }],
    }),
    createShiftAssignment: builder.mutation<ShiftAssignment, CreateAssignmentDto>({
      query: (body) => ({ url: `/schedules/${body.schedulePeriodId}/assignments`, method: 'POST', body }),
      invalidatesTags: (result, error, arg) => [
        { type: 'Schedule', id: arg.schedulePeriodId },
        'ShiftAssignments',
      ],
    }),
    validateSchedule: builder.mutation<ConflictCheckResult, string>({
      query: (scheduleId) => ({ url: `/schedules/${scheduleId}/validate`, method: 'POST' }),
    }),
    publishSchedule: builder.mutation<void, string>({
      query: (id) => ({ url: `/schedules/${id}/publish`, method: 'POST' }),
      invalidatesTags: (result, error, id) => [{ type: 'Schedule', id }, 'Schedules'],
    }),
    // ... more endpoints
  }),
});

export const {
  useGetSchedulePeriodsQuery,
  useGetSchedulePeriodQuery,
  useCreateShiftAssignmentMutation,
  useValidateScheduleMutation,
  usePublishScheduleMutation,
} = schedulesApi;
```

### 6.2 Redux Store Slices

| Slice | Persisted | Content |
|-------|-----------|---------|
| `authSlice` | Yes | accessToken, refreshToken, user profile, organizationId, permissions |
| `uiSlice` | Yes | sidebarCollapsed, theme (light/dark), locale, lastVisitedPage |
| `scheduleBuilderSlice` | No | selectedPeriodId, dragState, activeFilters, selectedEmployees, conflictHighlights |
| `clockSlice` | No | currentClockStatus (NotClockedIn, ClockedIn, OnBreak), lastClockEntry |
| `notificationSlice` | No | unreadCount, recentNotifications, toastQueue |
| `offlineQueueSlice` | Yes | pendingActions (clock entries, availability changes queued while offline) |
| `chatSlice` | No | activeChannelId, unreadCounts per channel/DM, typing indicators |
| `taskSlice` | No | myCurrentTasks, completionProgress, proofUploadState |
| `forecastSlice` | No | currentForecast, splhData, laborCostPercent |
| `geofenceSlice` | No | currentLocation, isInsideGeofence, locationAccuracy |

### 6.3 SignalR Real-Time Events

```typescript
// Events received from server via SignalR
interface SignalREvents {
  // --- Scheduling Events ---
  'SchedulePublished':     { schedulePeriodId: string; locationId: string };
  'ShiftAssigned':         { assignmentId: string; employeeId: string };
  'ShiftChanged':          { assignmentId: string; changes: string[] };
  'ShiftCancelled':        { assignmentId: string; employeeId: string };
  'TimeOffRequestUpdate':  { requestId: string; status: string };
  'ShiftSwapUpdate':       { swapId: string; status: string };
  'OpenShiftAvailable':    { openShiftId: string; roleId: string };
  'NewNotification':       { notification: Notification };
  'ClockStatusUpdate':     { employeeId: string; status: string };

  // --- Chat Events (NEW) ---
  'NewChatMessage':        { channelId?: string; senderId: string; message: ChatMessage };
  'MessageRead':           { messageId: string; userId: string };
  'UserTyping':            { channelId?: string; userId: string; isTyping: boolean };
  'NewAnnouncement':       { announcement: Announcement };

  // --- Task Events (NEW) ---
  'TaskCompleted':         { taskId: string; employeeId: string; proof?: string };
  'TaskOverdue':           { taskId: string; employeeName: string };

  // --- Engagement Events (NEW) ---
  'NewRecognition':        { recognition: Recognition };
  'PostShiftSurveyReady':  { shiftAssignmentId: string };

  // --- Forecast Events (NEW) ---
  'ForecastUpdated':       { locationId: string; date: string };
  'LaborCostAlert':        { locationId: string; currentPercent: number; target: number };
}
```

### 6.4 SignalR Middleware (Detailed Implementation)

```typescript
// src/store/middleware/signalrMiddleware.ts
import { HubConnectionBuilder, HubConnectionState, LogLevel } from '@microsoft/signalr';
import type { Middleware } from '@reduxjs/toolkit';
import { baseApi } from '@api/baseApi';

let notificationConnection: HubConnection | null = null;
let chatConnection: HubConnection | null = null;

export const signalrMiddleware: Middleware = (store) => (next) => (action) => {
  const result = next(action);

  // === Connect on successful login ===
  if (action.type === 'auth/setCredentials') {
    const { accessToken } = action.payload;
    connectToHubs(accessToken, store);
  }

  // === Disconnect on logout ===
  if (action.type === 'auth/logout') {
    disconnectFromHubs();
  }

  // === Send typing indicator when user types in chat ===
  if (action.type === 'chat/setTyping') {
    chatConnection?.invoke('SendTypingIndicator', action.payload.channelId);
  }

  return result;
};

async function connectToHubs(token: string, store: MiddlewareAPI) {
  // --- Notification Hub ---
  notificationConnection = new HubConnectionBuilder()
    .withUrl(import.meta.env.VITE_SIGNALR_HUB_URL, {
      accessTokenFactory: () => token,
    })
    .withAutomaticReconnect({
      // Exponential backoff: 0s, 2s, 5s, 10s, 30s, then stop
      nextRetryDelayInMilliseconds: (context) => {
        const delays = [0, 2000, 5000, 10000, 30000];
        return context.previousRetryCount < delays.length
          ? delays[context.previousRetryCount]
          : null; // stop retrying after 5 attempts
      },
    })
    .configureLogging(LogLevel.Warning)
    .build();

  // Subscribe to events → dispatch Redux actions
  notificationConnection.on('NewNotification', (notification) => {
    store.dispatch({ type: 'notifications/addNotification', payload: notification });
    store.dispatch({ type: 'notifications/incrementUnread' });
    // Also show a toast
    toast(notification.title, { icon: getNotificationIcon(notification.type) });
  });

  notificationConnection.on('SchedulePublished', (data) => {
    // Invalidate RTK Query cache so schedule page refreshes
    store.dispatch(baseApi.util.invalidateTags([
      { type: 'Schedule', id: data.schedulePeriodId },
      'Schedules',
    ]));
    toast('Schedule has been published!', { icon: '📅' });
  });

  notificationConnection.on('ShiftAssigned', (data) => {
    store.dispatch(baseApi.util.invalidateTags(['ShiftAssignments']));
  });

  notificationConnection.on('ClockStatusUpdate', (data) => {
    // Update who's clocked in (for manager dashboard "today's staff" widget)
    store.dispatch(baseApi.util.invalidateTags(['ClockEntry']));
  });

  notificationConnection.on('TimeOffRequestUpdate', (data) => {
    store.dispatch(baseApi.util.invalidateTags(['TimeOffRequests']));
  });

  // Handle reconnection state
  notificationConnection.onreconnecting(() => {
    store.dispatch({ type: 'ui/setConnectionStatus', payload: 'reconnecting' });
  });
  notificationConnection.onreconnected(() => {
    store.dispatch({ type: 'ui/setConnectionStatus', payload: 'connected' });
  });
  notificationConnection.onclose(() => {
    store.dispatch({ type: 'ui/setConnectionStatus', payload: 'disconnected' });
  });

  await notificationConnection.start();

  // --- Chat Hub (separate connection for messaging) ---
  chatConnection = new HubConnectionBuilder()
    .withUrl(import.meta.env.VITE_SIGNALR_CHAT_HUB_URL, {
      accessTokenFactory: () => token,
    })
    .withAutomaticReconnect()
    .build();

  chatConnection.on('NewChatMessage', (message) => {
    store.dispatch({ type: 'chat/receiveMessage', payload: message });
    // Update unread count for the channel
    if (message.channelId !== store.getState().chat.activeChannelId) {
      store.dispatch({ type: 'chat/incrementUnread', payload: message.channelId });
    }
  });

  chatConnection.on('UserTyping', (data) => {
    store.dispatch({ type: 'chat/setUserTyping', payload: data });
  });

  chatConnection.on('NewAnnouncement', (announcement) => {
    store.dispatch(baseApi.util.invalidateTags(['Announcements']));
    toast(announcement.title, { icon: '📢', duration: 6000 });
  });

  chatConnection.on('TaskCompleted', (data) => {
    store.dispatch(baseApi.util.invalidateTags(['Tasks']));
  });

  chatConnection.on('PostShiftSurveyReady', (data) => {
    // Show the post-shift survey modal
    store.dispatch({ type: 'engagement/showSurvey', payload: data });
  });

  await chatConnection.start();
}

function disconnectFromHubs() {
  notificationConnection?.stop();
  chatConnection?.stop();
  notificationConnection = null;
  chatConnection = null;
}
```

### 6.5 Custom Hooks (Detailed Implementations)

#### useScheduleBuilder — the most complex hook

```typescript
// src/hooks/useScheduleBuilder.ts
import { useMemo, useCallback, useState } from 'react';
import { useAppDispatch, useAppSelector } from '@store/index';
import { 
  useGetSchedulePeriodQuery, 
  useCreateShiftAssignmentMutation,
  useValidateScheduleMutation 
} from '@api/schedulesApi';

interface UseScheduleBuilderReturn {
  // Data
  schedule: SchedulePeriodDetail | undefined;
  assignments: ShiftAssignment[];
  conflicts: ScheduleConflict[];
  isLoading: boolean;
  
  // Computed
  assignmentsByEmployeeAndDay: Map<string, Map<string, ShiftAssignment[]>>;
  staffingCoverage: StaffingCoverage[];
  totalLaborCost: number;
  
  // Actions
  createAssignment: (dto: CreateAssignmentDto) => Promise<void>;
  moveAssignment: (id: string, newDate: string, newEmployeeId?: string) => Promise<void>;
  deleteAssignment: (id: string) => Promise<void>;
  validateAll: () => Promise<ConflictCheckResult>;
  publishSchedule: () => Promise<void>;
  
  // Undo/Redo
  canUndo: boolean;
  canRedo: boolean;
  undo: () => void;
  redo: () => void;
}

export function useScheduleBuilder(periodId: string): UseScheduleBuilderReturn {
  const dispatch = useAppDispatch();
  const [undoStack, setUndoStack] = useState<UndoAction[]>([]);
  const [redoStack, setRedoStack] = useState<UndoAction[]>([]);

  const { data: schedule, isLoading } = useGetSchedulePeriodQuery(periodId);
  const [createMutation] = useCreateShiftAssignmentMutation();
  const [validateMutation] = useValidateScheduleMutation();

  // Pre-compute assignment grid: employee → day → assignments[]
  // This powers the O(1) lookup for the schedule grid rendering
  const assignmentsByEmployeeAndDay = useMemo(() => {
    if (!schedule?.assignments) return new Map();
    
    const grid = new Map<string, Map<string, ShiftAssignment[]>>();
    
    for (const assignment of schedule.assignments) {
      if (!grid.has(assignment.employeeId)) {
        grid.set(assignment.employeeId, new Map());
      }
      const employeeMap = grid.get(assignment.employeeId)!;
      const dayKey = assignment.date; // "2026-02-17"
      
      if (!employeeMap.has(dayKey)) {
        employeeMap.set(dayKey, []);
      }
      employeeMap.get(dayKey)!.push(assignment);
    }
    
    return grid;
  }, [schedule?.assignments]);

  // Calculate total labor cost from assignments
  const totalLaborCost = useMemo(() => {
    if (!schedule?.assignments) return 0;
    return schedule.assignments.reduce(
      (sum, a) => sum + (a.estimatedCostCents || 0), 0
    );
  }, [schedule?.assignments]);

  const createAssignment = useCallback(async (dto: CreateAssignmentDto) => {
    // Push to undo stack before making the change
    setUndoStack(prev => [...prev, { type: 'create', data: dto }]);
    setRedoStack([]); // clear redo on new action
    
    await createMutation({ ...dto, schedulePeriodId: periodId }).unwrap();
  }, [createMutation, periodId]);

  const undo = useCallback(() => {
    const lastAction = undoStack[undoStack.length - 1];
    if (!lastAction) return;
    
    // Reverse the action
    if (lastAction.type === 'create') {
      // Delete the assignment that was created
      dispatch(/* delete mutation */);
    } else if (lastAction.type === 'delete') {
      // Re-create the assignment that was deleted
      dispatch(/* create mutation with original data */);
    }
    
    setUndoStack(prev => prev.slice(0, -1));
    setRedoStack(prev => [...prev, lastAction]);
  }, [undoStack, dispatch]);

  // ... more implementation

  return {
    schedule,
    assignments: schedule?.assignments || [],
    conflicts: [],
    isLoading,
    assignmentsByEmployeeAndDay,
    staffingCoverage: [],
    totalLaborCost,
    createAssignment,
    moveAssignment: async () => {},
    deleteAssignment: async () => {},
    validateAll: async () => validateMutation(periodId).unwrap(),
    publishSchedule: async () => {},
    canUndo: undoStack.length > 0,
    canRedo: redoStack.length > 0,
    undo,
    redo: () => {},
  };
}
```

#### useClock — clock-in/out with offline support and geofencing

```typescript
// src/hooks/useClock.ts
import { useCallback, useEffect, useState } from 'react';
import { useClockInMutation, useClockOutMutation, useGetClockStatusQuery, 
         useVerifyLocationMutation } from '@api/clockApi';
import { useAppDispatch } from '@store/index';
import { addToOfflineQueue } from '@store/slices/offlineQueueSlice';

interface UseClockReturn {
  status: 'not_clocked_in' | 'clocked_in' | 'on_break';
  lastEntry: ClockEntry | null;
  todayShift: ShiftAssignment | null;
  duration: string;  // "4h 32m"
  isInsideGeofence: boolean | null;  // null = checking
  geoError: string | null;
  clockIn: () => Promise<void>;
  clockOut: () => Promise<void>;
  startBreak: () => Promise<void>;
  endBreak: () => Promise<void>;
  isLoading: boolean;
}

export function useClock(): UseClockReturn {
  const { data: clockData, isLoading } = useGetClockStatusQuery(undefined, {
    pollingInterval: 60_000, // refresh every minute to keep duration accurate
  });
  const [clockInMutation] = useClockInMutation();
  const [clockOutMutation] = useClockOutMutation();
  const [verifyLocation] = useVerifyLocationMutation();
  const dispatch = useAppDispatch();

  const [geoState, setGeoState] = useState<{
    isInside: boolean | null;
    error: string | null;
  }>({ isInside: null, error: null });

  // Running duration timer
  const [duration, setDuration] = useState('0h 0m');
  useEffect(() => {
    if (clockData?.status !== 'clocked_in') return;
    
    const updateDuration = () => {
      const start = new Date(clockData.lastClockIn).getTime();
      const now = Date.now();
      const diffMinutes = Math.floor((now - start) / 60000);
      const hours = Math.floor(diffMinutes / 60);
      const minutes = diffMinutes % 60;
      setDuration(`${hours}h ${minutes}m`);
    };
    
    updateDuration();
    const interval = setInterval(updateDuration, 60_000); // update every minute
    return () => clearInterval(interval);
  }, [clockData]);

  // Check geofence on mount
  useEffect(() => {
    checkGeofence();
  }, []);

  const checkGeofence = useCallback(async () => {
    try {
      const position = await new Promise<GeolocationPosition>((resolve, reject) => {
        navigator.geolocation.getCurrentPosition(resolve, reject, {
          enableHighAccuracy: true,
          timeout: 10000,
          maximumAge: 30000,
        });
      });

      const result = await verifyLocation({
        latitude: position.coords.latitude,
        longitude: position.coords.longitude,
        accuracy: position.coords.accuracy,
      }).unwrap();

      setGeoState({ isInside: result.isWithinGeofence, error: null });
    } catch (err) {
      setGeoState({ 
        isInside: null, 
        error: 'Unable to determine location. Please enable GPS.' 
      });
    }
  }, [verifyLocation]);

  const clockIn = useCallback(async () => {
    // Get current GPS position
    let gpsData: { latitude: number; longitude: number } | undefined;
    try {
      const pos = await new Promise<GeolocationPosition>((resolve, reject) => {
        navigator.geolocation.getCurrentPosition(resolve, reject, {
          enableHighAccuracy: true,
          timeout: 5000,
        });
      });
      gpsData = { latitude: pos.coords.latitude, longitude: pos.coords.longitude };
    } catch {
      // GPS not available — continue without it (unless geofencing is required)
    }

    const clockPayload = {
      timestamp: new Date().toISOString(),
      latitude: gpsData?.latitude,
      longitude: gpsData?.longitude,
      ipAddress: undefined, // server captures this
    };

    if (!navigator.onLine) {
      // === OFFLINE CLOCK-IN ===
      // Queue the action with local timestamp and GPS
      dispatch(addToOfflineQueue({
        type: 'clock/in',
        payload: clockPayload,
        queuedAt: new Date().toISOString(),
      }));
      toast('Clock-in saved. Will sync when online.', { icon: '📡' });
      return;
    }

    await clockInMutation(clockPayload).unwrap();
    // Haptic feedback on success
    if (navigator.vibrate) navigator.vibrate([50, 30, 50]);
    toast.success('Clocked in!');
  }, [clockInMutation, dispatch]);

  // Similar implementation for clockOut, startBreak, endBreak...

  return {
    status: clockData?.status || 'not_clocked_in',
    lastEntry: clockData?.lastEntry || null,
    todayShift: clockData?.todayShift || null,
    duration,
    isInsideGeofence: geoState.isInside,
    geoError: geoState.error,
    clockIn,
    clockOut: async () => { /* similar to clockIn */ },
    startBreak: async () => { /* similar */ },
    endBreak: async () => { /* similar */ },
    isLoading,
  };
}
```

#### usePermissions — role-based access control

```typescript
// src/hooks/usePermissions.ts
import { useMemo } from 'react';
import { useAppSelector } from '@store/index';

type Permission =
  | 'ViewOwnSchedule' | 'ViewAllSchedules' | 'CreateSchedule' | 'PublishSchedule'
  | 'ViewEmployees' | 'ManageEmployees'
  | 'ApproveTimeOff' | 'ViewPayroll' | 'ManageSettings'
  | 'ViewAuditLog' | 'ApproveTimesheets' | 'ManageRoles'
  | 'CreateAnnouncement' | 'ManageTasks' | 'ViewEngagement'
  | 'ManageGeofence' | 'ManageTips' | 'ManageCompliance';

type Role = 'Employee' | 'Manager' | 'Admin';

export function usePermissions() {
  const { permissions, roles } = useAppSelector((state) => state.auth);

  const can = useMemo(() => {
    const permissionSet = new Set(permissions);
    return (permission: Permission): boolean => permissionSet.has(permission);
  }, [permissions]);

  const hasRole = useMemo(() => {
    const roleSet = new Set(roles);
    return (role: Role): boolean => roleSet.has(role);
  }, [roles]);

  const isManager = useMemo(() => hasRole('Manager') || hasRole('Admin'), [hasRole]);
  const isAdmin = useMemo(() => hasRole('Admin'), [hasRole]);

  return { can, hasRole, isManager, isAdmin };
}

// Usage in components:
// const { can, isManager } = usePermissions();
// if (can('ApproveTimeOff')) { /* show approve button */ }
// <PermissionGate permission="ManageEmployees"><EmployeeEditButton /></PermissionGate>
```

---

## 7. Offline Support Strategy

### What Works Offline

| Feature | Offline Behavior |
|---------|------------------|
| **Clock In/Out** | Queued in `offlineQueueSlice` with local timestamp. Synced when online. Banner shows "1 pending action." |
| **View My Schedule** | Cached via RTK Query. Last-viewed schedule available offline. |
| **View My Timesheet** | Cached via RTK Query. |
| **Submit Availability Override** | Queued. Synced when online. |
| **All other features** | Read from cache if available. Write operations blocked with "You are offline" message. |

### Implementation

```typescript
// src/store/middleware/offlineMiddleware.ts
const offlineMiddleware: Middleware = (store) => (next) => (action) => {
  if (!navigator.onLine && isOfflineableAction(action)) {
    // Queue action for later
    store.dispatch(addToOfflineQueue(action));
    // Show toast
    toast('Action saved. Will sync when back online.', { icon: '📡' });
    return;
  }
  return next(action);
};
```

### Offline Queue Replay Strategy (Detailed)

```typescript
// src/store/middleware/offlineMiddleware.ts
import type { Middleware } from '@reduxjs/toolkit';
import { addToOfflineQueue, removeFromQueue, clearQueue } from '@store/slices/offlineQueueSlice';

// Actions that can be queued offline
const OFFLINEABLE_ACTIONS = [
  'clockApi/clockIn',
  'clockApi/clockOut',
  'clockApi/breakStart',
  'clockApi/breakEnd',
  'availabilityApi/createOverride',
];

export const offlineMiddleware: Middleware = (store) => (next) => (action) => {
  // Check if this is an RTK Query mutation that should work offline
  if (!navigator.onLine && isOfflineableAction(action)) {
    const offlineEntry = {
      id: crypto.randomUUID(),
      actionType: action.type,
      payload: action.meta?.arg?.originalArgs,
      timestamp: new Date().toISOString(),
      gpsData: action.meta?.gps, // preserve GPS from time of action
      retryCount: 0,
    };

    store.dispatch(addToOfflineQueue(offlineEntry));
    
    // Return a "fake success" so the UI updates optimistically
    return Promise.resolve({ data: { ...offlineEntry.payload, isPending: true } });
  }

  return next(action);
};

function isOfflineableAction(action: any): boolean {
  return OFFLINEABLE_ACTIONS.some(type => action.type?.startsWith(type));
}

// === Replay engine: runs when connection is restored ===

export function setupOnlineListener(store: Store) {
  window.addEventListener('online', async () => {
    const queue = store.getState().offlineQueue.pendingActions;
    
    if (queue.length === 0) return;

    toast(`Syncing ${queue.length} pending action(s)...`, { icon: '🔄' });

    let successCount = 0;
    let failCount = 0;

    for (const entry of queue) {
      try {
        // Replay in order (FIFO — oldest first)
        switch (entry.actionType) {
          case 'clockApi/clockIn':
            await store.dispatch(clockApi.endpoints.clockIn.initiate({
              ...entry.payload,
              // Use ORIGINAL timestamp, not current time!
              // This ensures the clock entry reflects when the employee actually clocked in
              timestamp: entry.timestamp,
              isOfflineSync: true,
            }));
            break;
          
          case 'clockApi/clockOut':
            await store.dispatch(clockApi.endpoints.clockOut.initiate({
              ...entry.payload,
              timestamp: entry.timestamp,
              isOfflineSync: true,
            }));
            break;
          
          case 'availabilityApi/createOverride':
            await store.dispatch(
              availabilityApi.endpoints.createOverride.initiate(entry.payload)
            );
            break;
        }

        successCount++;
        store.dispatch(removeFromQueue(entry.id));
      } catch (error) {
        failCount++;
        // Retry up to 3 times
        if (entry.retryCount < 3) {
          store.dispatch(incrementRetry(entry.id));
        } else {
          // After 3 failures, alert the user
          toast.error(`Failed to sync: ${entry.actionType}. Please check manually.`);
          store.dispatch(markFailed(entry.id));
        }
      }
    }

    if (successCount > 0) {
      toast.success(`${successCount} action(s) synced successfully!`);
    }
    if (failCount > 0) {
      toast.error(`${failCount} action(s) failed to sync.`);
    }
  });

  // Also detect offline state
  window.addEventListener('offline', () => {
    toast('You are offline. Clock actions will be saved locally.', { 
      icon: '📡',
      duration: 5000,
    });
  });
}
```

### Offline UI Indicators

```
Online state: No banner. Green dot in header status area.

Offline state:
┌─────────────────────────────────────────────┐
│ 📡 You are offline. Actions saved locally.  │  ← Yellow banner, persistent
│ 1 pending action queued.                     │     Shows count of pending actions
└─────────────────────────────────────────────┘

Syncing state (just came back online):
┌─────────────────────────────────────────────┐
│ 🔄 Syncing 3 pending actions...             │  ← Blue banner, animated progress
│ ██████░░░░░░ 1/3                            │
└─────────────────────────────────────────────┘

Sync complete:
┌─────────────────────────────────────────────┐
│ ✅ All actions synced successfully!          │  ← Green banner, auto-dismiss 3s
└─────────────────────────────────────────────┘

Sync failed:
┌─────────────────────────────────────────────┐
│ ⚠️ 1 action failed to sync. [Retry] [View] │  ← Yellow banner, persistent
└─────────────────────────────────────────────┘
```

---

## 8. Theming & Design System

### 8.1 Ant Design 5 Theme Tokens

```typescript
// src/theme/antdTheme.ts
import { ThemeConfig } from 'antd';

export const lightTheme: ThemeConfig = {
  token: {
    colorPrimary: '#2563EB',           // Blue-600 (professional, trustworthy)
    colorSuccess: '#16A34A',           // Green-600
    colorWarning: '#D97706',           // Amber-600
    colorError: '#DC2626',             // Red-600
    colorInfo: '#0891B2',              // Cyan-600
    borderRadius: 8,
    fontFamily: "'Inter', -apple-system, BlinkMacSystemFont, sans-serif",
    fontSize: 14,
    colorBgContainer: '#FFFFFF',
    colorBgLayout: '#F8FAFC',          // Slate-50
  },
  components: {
    Layout: {
      siderBg: '#0F172A',             // Slate-900 (dark sidebar)
      headerBg: '#FFFFFF',
    },
    Menu: {
      darkItemBg: '#0F172A',
      darkItemSelectedBg: '#1E40AF',   // Blue-800
    },
    Card: {
      borderRadiusLG: 12,
    },
  },
};

export const darkTheme: ThemeConfig = {
  token: {
    colorPrimary: '#3B82F6',           // Blue-500
    colorBgContainer: '#1E293B',       // Slate-800
    colorBgLayout: '#0F172A',          // Slate-900
    colorText: '#F1F5F9',             // Slate-100
    colorBorder: '#334155',            // Slate-700
  },
};
```

### 8.2 Schedule-Specific Colors

```typescript
// src/theme/colors.ts

// Role colors (used on shift cards and calendar)
export const roleColors = {
  waiter:    { bg: '#FEF3C7', border: '#D97706', text: '#92400E' },   // Amber
  bartender: { bg: '#DBEAFE', border: '#2563EB', text: '#1E3A8A' },   // Blue
  chef:      { bg: '#DCFCE7', border: '#16A34A', text: '#14532D' },   // Green
  host:      { bg: '#F3E8FF', border: '#9333EA', text: '#581C87' },   // Purple
  manager:   { bg: '#FFE4E6', border: '#E11D48', text: '#881337' },   // Rose
  dishwasher:{ bg: '#E0E7FF', border: '#4F46E5', text: '#312E81' },   // Indigo
  delivery:  { bg: '#CCFBF1', border: '#0D9488', text: '#134E4A' },   // Teal
};

// Shift status colors
export const shiftStatusColors = {
  scheduled: '#3B82F6',   // Blue
  confirmed: '#16A34A',   // Green
  completed: '#6B7280',   // Gray
  noShow:    '#DC2626',   // Red
  cancelled: '#9CA3AF',   // Light gray
};

// Leave type colors
export const leaveColors = {
  vacation:   '#3B82F6',   // Blue
  sick:       '#EF4444',   // Red
  personal:   '#8B5CF6',   // Violet
  unpaid:     '#6B7280',   // Gray
  training:   '#F59E0B',   // Amber
};
```

---

## 9. Internationalization (i18n)

### Supported Languages

| Language | Code | Priority |
|----------|------|----------|
| English | `en` | P0 (default) |
| French | `fr` | P0 |
| German | `de` | P1 |

### Translation Structure

```json
// src/i18n/en/schedule.json
{
  "schedule": {
    "title": "Schedule",
    "builder": {
      "title": "Schedule Builder",
      "weekOf": "Week of {{date}}",
      "publish": "Publish Schedule",
      "publishConfirm": "This will notify all {{count}} assigned employees. Continue?",
      "copyPrevious": "Copy Previous Week",
      "conflicts": {
        "title": "{{count}} conflict(s) found",
        "overlap": "{{employee}} has an overlapping shift on {{date}}",
        "insufficientRest": "{{employee}} has only {{hours}}h rest (minimum: {{min}}h)",
        "overtimeWeekly": "{{employee}} would exceed {{max}}h weekly limit (currently: {{current}}h)",
        "notAvailable": "{{employee}} is not available on {{date}} {{time}}",
        "onLeave": "{{employee}} is on approved {{leaveType}} on {{date}}",
        "missingSkill": "{{employee}} does not have required certification: {{skill}}"
      }
    },
    "statuses": {
      "draft": "Draft",
      "published": "Published",
      "locked": "Locked"
    }
  }
}
```

---

## 10. Performance Optimization

### 10.1 Performance Budgets (Concrete Targets)

| Metric | Target | Measurement | Why This Target |
|--------|--------|-------------|-----------------|
| **Largest Contentful Paint (LCP)** | < 1.5s (desktop), < 2.5s (mobile 4G) | Schedule Builder page fully loaded | Manager needs to see schedule quickly to start editing |
| **First Input Delay (FID)** | < 100ms | Time from first click/tap to response | Drag-drop must feel instant |
| **Cumulative Layout Shift (CLS)** | < 0.1 | Page visual stability | Cards shifting during load would be confusing |
| **Time to Interactive (TTI)** | < 3s (desktop), < 5s (mobile 4G) | Employee Dashboard page | Employees check shifts on phone — must be fast |
| **Initial JS bundle** | < 200KB gzipped | Main bundle (not lazy chunks) | Fast first load for restaurant WiFi |
| **Schedule Builder chunk** | < 150KB gzipped | Lazy-loaded on route entry | Largest page — includes DnD library |
| **Chat chunk** | < 80KB gzipped | Lazy-loaded on route entry | Chat UI with SignalR |
| **Total page weight** | < 1MB per page (including images) | All resources loaded | Restaurant WiFi can be slow |
| **API response render** | < 200ms from API response to UI update | Schedule grid re-render after fetch | Users see stale data as broken |
| **Drag-drop frame rate** | 60fps during drag operations | No frame drops during active drag | Janky drag = unusable |
| **Clock-in action** | < 500ms end-to-end (including GPS) | Tap to confirmation | Employees won't wait — they'll bypass the system |

### 10.2 Optimization Strategies

| Strategy | Implementation | Expected Impact |
|----------|---------------|----------------|
| **Code Splitting** | `React.lazy()` for ALL page components. Each route loads only its own code. Schedule Builder, Chat, Reports, and Forecast are the largest chunks — loaded on demand. | -60% initial bundle size |
| **RTK Query Caching** | API responses cached with configurable TTL. `keepUnusedDataFor: 300` (5 minutes). Schedule data cached per period. Automatic cache invalidation on mutations via tag system. | Eliminates redundant API calls |
| **Virtual Scrolling** | Employee list and timesheet tables use `@tanstack/react-virtual` when > 50 rows. Only renders visible rows + 5 overscan. | Handle 200+ employees without lag |
| **Debounced Search** | All search inputs debounced (300ms) to prevent API spam. Employee search, audit log search, chat message search. | -80% search API calls |
| **Image Optimization** | Employee photos: max 200x200px, WebP format, lazy loading with `loading="lazy"`. Placeholder avatar SVG (no network request). | -90% image bytes |
| **Bundle Size** | Tree-shake Ant Design: `import { Button } from 'antd'` (not `import antd`). Import only used dayjs plugins (`dayjs/plugin/relativeTime`). | -40% from Ant Design alone |
| **PWA Caching** | Workbox strategies: `CacheFirst` for static assets (60 day TTL), `StaleWhileRevalidate` for API responses, `NetworkOnly` for mutations. | Offline-ready + instant revisit |
| **Memoization** | `useMemo` for `assignmentsByEmployeeAndDay` grid computation (O(n) → O(1) lookups). `React.memo` on `ShiftCard` (re-renders only when its props change, not when sibling cards change). | Schedule grid renders in <16ms |
| **WebSocket** | Single SignalR connection per hub (not polling). Events update SPECIFIC cache entries via RTK Query tag invalidation — not full page re-fetches. | Real-time with minimal data transfer |
| **Preloading** | Preload schedule data on sidebar hover: `router.prefetch('/schedules/...')`. Preload employee detail on employee card hover. | Perceived instant navigation |
| **Font Loading** | Inter font loaded via `font-display: swap`. System font stack as fallback. Only load `400`, `500`, `600` weights. | No FOIT (Flash of Invisible Text) |

### 10.3 Bundle Size Budget Breakdown

```
Target: < 200KB gzipped initial bundle

  react + react-dom:               ~45KB
  @reduxjs/toolkit + react-redux:  ~15KB
  react-router-dom:                ~12KB
  antd (tree-shaken):              ~60KB  (only Button, Form, Table, Modal, Menu, Layout)
  @microsoft/signalr:              ~15KB
  dayjs:                           ~5KB
  framer-motion:                   ~20KB
  our app code (auth, layout):     ~25KB
  ─────────────────────────────────────
  Total initial:                   ~197KB gzipped ✅

Lazy chunks (loaded on demand):
  Schedule Builder + @dnd-kit:     ~140KB
  Chat UI:                         ~75KB
  Reports + Chart.js:              ~90KB
  Forecast dashboard:              ~60KB
  Geofence + leaflet:              ~120KB
  Task management:                 ~40KB
  Employee forms + validation:     ~50KB
```

---

## 11. Accessibility (a11y)

### 11.1 General Accessibility Standards

| Area | Implementation |
|------|---------------|
| **WCAG Level** | WCAG 2.1 AA compliance targeted for all pages |
| **Keyboard Navigation** | Full keyboard support for schedule builder (arrow keys, Enter, Escape, Tab) |
| **Screen Reader** | ARIA labels on all interactive elements. Meaningful alt text. |
| **Color Contrast** | All role colors meet WCAG 2.1 AA contrast ratio (4.5:1 minimum for text) |
| **Focus Management** | Visible focus indicators (2px solid outline). Modal focus trapping. Return focus on close. |
| **Responsive Text** | rem-based sizing. Respects user font size preferences up to 200% zoom. |
| **Reduced Motion** | `prefers-reduced-motion` media query respected. All Framer Motion animations skippable. |
| **Touch Targets** | Minimum 44x44px for all interactive elements (WCAG 2.5.5) |

### 11.2 Schedule Grid ARIA Pattern (Detailed)

The schedule builder uses the `grid` ARIA role to enable screen reader navigation of the complex table structure.

```tsx
// ScheduleCalendar.tsx — ARIA grid implementation
<div
  role="grid"
  aria-label={`Schedule for ${weekRange}. ${assignments.length} shift assignments.`}
  aria-rowcount={employees.length + 1}  // +1 for header row
  aria-colcount={8}                      // employee column + 7 days
  tabIndex={0}
  onKeyDown={handleGridKeyDown}
>
  {/* Header row with day names */}
  <div role="row" aria-rowindex={1}>
    <div role="columnheader" aria-colindex={1}>Employee</div>
    <div role="columnheader" aria-colindex={2}>
      Mon Feb 10
      <span className="sr-only">, 3 shifts scheduled</span>
    </div>
    {/* ... other days */}
  </div>

  {/* Employee rows */}
  {employees.map((employee, rowIdx) => (
    <div 
      key={employee.id} 
      role="row" 
      aria-rowindex={rowIdx + 2}
      aria-label={`${employee.name}, ${employee.primaryRole}`}
    >
      {/* Employee name cell */}
      <div role="rowheader" aria-colindex={1}>
        {employee.name}
        <span className="sr-only">
          {employee.roles.join(', ')}. 
          {getWeeklyHours(employee.id)} hours this week.
        </span>
      </div>

      {/* Day cells */}
      {days.map((day, colIdx) => {
        const cellAssignments = getAssignments(employee.id, day);
        const hasConflict = cellAssignments.some(a => a.conflicts.length > 0);
        
        return (
          <div
            key={day}
            role="gridcell"
            aria-colindex={colIdx + 2}
            aria-selected={isSelected(employee.id, day)}
            aria-describedby={hasConflict ? `conflict-${employee.id}-${day}` : undefined}
            tabIndex={isFocused(employee.id, day) ? 0 : -1}
            data-employee={employee.id}
            data-day={day}
          >
            {cellAssignments.map(assignment => (
              <ShiftCard
                key={assignment.id}
                assignment={assignment}
                aria-label={`
                  ${employee.name}, 
                  ${assignment.startTime} to ${assignment.endTime}, 
                  ${assignment.roleName} at ${assignment.stationName}.
                  ${assignment.conflicts.length > 0 
                    ? `${assignment.conflicts.length} conflicts: ${assignment.conflicts.map(c => c.message).join('. ')}`
                    : 'No conflicts.'}
                `}
                role="button"
                aria-pressed={isCardSelected(assignment.id)}
              />
            ))}
            
            {cellAssignments.length === 0 && (
              <span className="sr-only">
                No shift assigned for {employee.name} on {formatDate(day)}.
                Press Enter to assign a shift.
              </span>
            )}

            {/* Conflict description for screen readers */}
            {hasConflict && (
              <div id={`conflict-${employee.id}-${day}`} className="sr-only">
                Warning: {cellAssignments.flatMap(a => a.conflicts).map(c => c.message).join('. ')}
              </div>
            )}
          </div>
        );
      })}
    </div>
  ))}
</div>
```

**Screen reader navigation experience:**
```
User presses Tab → focus enters schedule grid
Screen reader: "Schedule for February 10 to 16. 48 shift assignments. Grid."

User presses → arrow key
Screen reader: "Monday February 10, 3 shifts scheduled."

User presses ↓ arrow key
Screen reader: "Marie Dubois, Waiter Senior. 32 hours this week."

User presses → arrow key
Screen reader: "Marie Dubois, 9:00 to 15:00, Waiter at Terrace. No conflicts."

User presses → arrow key to Wednesday cell
Screen reader: "No shift assigned for Marie Dubois on Wednesday February 12. 
               Press Enter to assign a shift."

User presses Enter
Screen reader: "Shift Assignment dialog opened. Shift template dropdown, Morning selected."
```

### 11.3 Form Validation Accessibility

```tsx
// All forms use react-hook-form + Zod with accessible error messages

<Form.Item
  label="First Name"
  validateStatus={errors.firstName ? 'error' : undefined}
  help={errors.firstName?.message}
>
  <Input
    {...register('firstName')}
    aria-invalid={!!errors.firstName}
    aria-describedby={errors.firstName ? 'firstName-error' : undefined}
    aria-required="true"
  />
  {errors.firstName && (
    <span id="firstName-error" role="alert">
      {errors.firstName.message}
    </span>
  )}
</Form.Item>
```

### 11.4 Color-Blind Safe Design

Role colors are designed to be distinguishable even with common color vision deficiencies:

| Role | Color | Pattern (for color-blind users) | Contrast Ratio |
|------|-------|--------------------------------|----------------|
| Waiter | Amber (#D97706) | Solid fill | 4.8:1 on white ✅ |
| Bartender | Blue (#2563EB) | Diagonal stripes (optional) | 5.2:1 on white ✅ |
| Chef | Green (#16A34A) | Cross-hatch (optional) | 4.6:1 on white ✅ |
| Host | Purple (#9333EA) | Dots (optional) | 5.5:1 on white ✅ |
| Manager | Rose (#E11D48) | Horizontal stripes (optional) | 5.8:1 on white ✅ |

Pattern fills are enabled by default in high-contrast mode and can be toggled in accessibility settings.

---

## 12. Testing Strategy

| Layer | Tool | What to Test |
|-------|------|-------------|
| **Unit Tests** | Vitest + React Testing Library | Utility functions, hooks, individual components in isolation |
| **Component Tests** | Vitest + React Testing Library | ShiftCard rendering, ConflictAlert display, form validation, StatusBadge variants |
| **Integration Tests** | Vitest + MSW (Mock Service Worker) | Full page renders with mocked API, user flows (login → view schedule → assign shift) |
| **E2E Tests** | Playwright | Critical paths: login, create employee, build schedule, clock in/out, approve time-off, export payroll |
| **Visual Regression** | Chromatic (Storybook) | Catch unintended UI changes in ShiftCard, ScheduleCalendar, Dashboard widgets |

### Critical Test Scenarios

1. **Schedule Builder Drag-Drop** — Drag employee to day → shift created, card appears, conflict check runs
2. **Conflict Display** — Assign overlapping shift → red border shown, error message displayed
3. **Clock In/Out Flow** — Clock in → status changes → break → clock out → entries recorded
4. **Time-Off Approval** — Manager views request → sees staffing impact → approves → balance updated
5. **Offline Clock** — Go offline → clock in → "pending" shown → come online → synced
6. **Publish Schedule** — Click publish → confirm dialog → notification sent → status changes
7. **Payroll Export** — Generate report → verify totals → download CSV → validate format

---

## 13. Priority Matrix — Core vs. Nice-to-Have

> **See Backend Report Section 11 for the full Priority Matrix.** The same Tier 1-4 classification applies to the frontend. Build Tier 1 (core scheduling) first, Tier 2 (standard features) second, Tier 3 (differentiators) third, and Tier 4 (platform extensions) only when customers ask for them.

---

## 14. Phased Delivery Plan

### Phase 1 — MVP (Core Scheduling UI)

> **Goal:** A manager can build schedules, employees can view shifts and clock in/out, payroll can be exported.

| Module | Included in MVP |
|--------|----------------|
| Authentication (Login, Register, Invite) | Yes |
| App Layout (Sidebar, Header, Mobile Nav) | Yes |
| Manager Dashboard | Yes |
| Employee Dashboard | Yes |
| Employee Directory (List + Detail + Create/Edit) | Yes |
| Contract Management | Yes |
| Multi-Role Assignment | Yes |
| Availability Editor (Recurring + Overrides) | Yes |
| Schedule Builder (Drag-Drop) with Conflict Detection | Yes |
| Shift Templates | Yes |
| Staffing Requirements Display | Yes |
| Copy Previous Schedule | Yes |
| Schedule Publish + Status Flow | Yes |
| Time-Off Request Form + Approval | Yes |
| Leave Balance Display | Yes |
| Clock-In / Clock-Out Page | Yes |
| Timesheet Summary View | Yes |
| Timesheet Approval Flow | Yes |
| Payroll Report + CSV Export | Yes |
| In-App Notifications (Bell + Dropdown) | Yes |
| Settings: Roles, Stations, Shift Templates | Yes |
| Settings: Leave Types, Scheduling Rules | Yes |
| Profile Settings | Yes |
| Light/Dark Theme | Yes |
| English + French i18n | Yes |
| Responsive (Desktop + Mobile) | Yes |
| Announcements (broadcast + read receipts) | Yes |
| **NOT in MVP** | |
| Team Chat (DMs + Channels) | Phase 2 *(Announcements sufficient for MVP)* |

### Phase 2 — Full Product (Match Competitor Feature Parity)

> **Goal:** Feature parity with 7shifts, Deputy, When I Work. No reason for a customer to choose a competitor.

| Module | Phase |
|--------|-------|
| Team Chat (DMs + Channels via SignalR) | Phase 2 |
| Open Shifts / Shift Marketplace | Phase 2 |
| Shift Swaps | Phase 2 |
| Kiosk Mode (shared tablet clock) | Phase 2 |
| Geofence Settings + Map | Phase 2 |
| Task Management (checklists + proof upload) | Phase 2 |
| Manager Logbook | Phase 2 |
| Offline Support (PWA) | Phase 2 |
| Push Notifications | Phase 2 |
| Email Notification Preferences | Phase 2 |
| Skill/Certification Tracking UI | Phase 2 |
| Schedule Templates (Save/Load Full Weekly Patterns) | Phase 2 |
| Employee Earnings Page (Estimate + Hours Tracker) | Phase 2 |
| Advanced Reports (Charts) | Phase 2 |
| POS Connection Settings | Phase 2 |
| Document Upload + Onboarding Checklist UI | Phase 2 |
| Team Calendar (Full Calendar View) | Phase 2 |
| Blackout Dates Management | Phase 2 |
| Audit Log Viewer | Phase 2 |
| German i18n | Phase 2 |

### Phase 3 — Advanced Features (Competitive Differentiators)

| Module | Phase |
|--------|-------|
| Demand Forecast Dashboard + SPLH | Phase 3 |
| Auto-Scheduling Wizard (Strategy + Preview + Apply) | Phase 3 |
| Schedule Builder Forecast/Weather Overlay | Phase 3 |
| Labor Cost % Live Tracker | Phase 3 |
| Tip Management UI | Phase 3 |
| Compliance Settings (Predictive Scheduling) | Phase 3 |
| Employee Engagement (Surveys + Recognition Feed) | Phase 3 |
| Flight Risk Alerts | Phase 3 |
| Photo Verification Clock | Phase 3 |
| Intraday Labor Dashboard (Real-time Sales vs. Labor) | Phase 3 |
| Earned vs. Actual Hours Dashboard | Phase 3 |
| Minor Work Rules UI | Phase 3 |

### Phase 4 — Platform Extensions (Build on Customer Demand)

| Module | Phase |
|--------|-------|
| Hiring Pipeline (Kanban + Job Postings + Interview Scheduling) | Phase 4 |
| Training / LMS (Course Viewer + Quiz Player + Progress) | Phase 4 |
| Knowledge Base & Employee Handbook | Phase 4 |
| Digital Forms (Form Builder + Form Filler + Submissions) | Phase 4 |
| Company Newsfeed (Feed + Reactions + Comments + Pins) | Phase 4 |
| Performance Reviews (Self-Assessment + Manager Review + Goals) | Phase 4 |
| Employee Onboarding Portal (Self-Service Steps) | Phase 4 |
| Shift Bidding + Preferences UI | Phase 4 |
| Facial Recognition Clock | Phase 4 |
| GPS Live Employee Map + Route History | Phase 4 |
| Mileage Tracking Table | Phase 4 |
| AI Candidate Screening UI | Phase 4 |
| What-If Schedule Comparison | Phase 4 |
| Hiring Analytics Dashboard | Phase 4 |
| Training Analytics Dashboard | Phase 4 |

---

## 15. Estimated Effort

### Phase 1 — MVP

| Module | Estimated Days | Notes |
|--------|---------------|-------|
| Project Setup (Vite, TS, Ant Design, RTK Query, Router) | 3 | Foundation |
| Theme + Design System + Global Styles | 2 | Color tokens, typography, dark mode |
| Auth Pages (Login, Register, Forgot Password, Invite) | 4 | + JWT token management |
| App Layout (Sidebar, Header, Responsive, Mobile Nav) | 4 | Core navigation shell |
| Employee List + Search + Filters | 3 | DataTable + paginated API |
| Employee Detail Page (Tabbed) | 5 | 5 tabs in MVP |
| Employee Create/Edit Form | 3 | Multi-step form, validation |
| Contract Form | 2 | CRUD within employee detail |
| Role Assignment Form | 2 | Multi-role with proficiency |
| Availability Editor (Grid + Overrides) | 5 | **Complex** — interactive weekly grid |
| Schedule Builder Page (Drag-Drop) | 10 | **Most complex** — DnD, grid, filters, toolbar |
| ShiftCard + ConflictAlert Components | 3 | Visual shift representations |
| ShiftAssignmentModal | 3 | Create/edit form with validation |
| Copy Previous Schedule | 2 | Clone + conflict display |
| Schedule Publish Flow | 2 | Status transitions + confirmation |
| Staffing Coverage Bar | 2 | Visual staffing comparison |
| Time-Off Request Form | 3 | Form + date range + balance check |
| Time-Off Approval List (Manager) | 3 | List + impact preview + actions |
| Leave Balance Widget | 1 | Display component |
| Clock Page (Mobile-Optimized) | 4 | Big buttons, status, history |
| Timesheet Period List | 2 | Table view |
| Timesheet Detail (All Entries) | 4 | Grid with approval actions |
| My Timesheet (Employee) | 2 | Simplified view |
| Payroll Report Page | 4 | Table + charts + export |
| Manager Dashboard | 4 | 6-8 widgets |
| Employee Dashboard | 3 | Clock + shifts + balance widgets |
| Notification Bell + Dropdown | 2 | Header component |
| Announcements (Create + Feed + Read Status) | 3 | One-way broadcasts |
| Settings: Roles + Permissions | 3 | CRUD + permission matrix |
| Settings: Stations | 2 | CRUD |
| Settings: Shift Templates | 2 | CRUD |
| Settings: Leave Types | 2 | CRUD |
| Settings: Scheduling Rules | 2 | CRUD |
| Settings: Organization Profile | 2 | Form |
| Profile Settings | 2 | Password change, preferences |
| i18n Setup (English + French) | 3 | All translation files |
| Responsive Polish (Mobile/Tablet) | 4 | Cross-device testing |
| Unit + Component Tests | 6 | Critical components |
| E2E Tests (Playwright) | 4 | Critical user flows |
| **Total MVP** | **~117 days** | **~5 months with 1 developer** |

### Phase 2 — Full Product

| Module | Estimated Days | Notes |
|--------|---------------|-------|
| Team Chat (Sidebar + Thread + SignalR) | 8 | Channels, DMs, real-time |
| Open Shifts + Shift Swaps | 5 | Marketplace features |
| Kiosk Mode (Full-screen + PIN pad + QR scanner) | 5 | Shared tablet lock |
| Geofence Settings (Interactive Map + Zone Editor) | 5 | Map integration (Leaflet) |
| Task Management (Templates + Checklist + Proof + Dashboard) | 8 | Mobile-first with camera |
| Manager Logbook (Timeline + Form + Metrics + Search) | 5 | Daily shift log |
| Offline Support (PWA + Service Worker) | 5 | Workbox + IndexedDB queue |
| Schedule Templates (Save/Load) | 3 | Template CRUD + apply flow |
| Employee Earnings + Hours Tracker | 3 | Charts + live calculations |
| Advanced Reports (Charts + Exports) | 5 | Chart.js visualizations |
| Remaining Settings (Blackout, POS, Audit, Certs, Documents) | 8 | Multiple CRUD pages |
| German i18n | 2 | Translation files |
| **Total Phase 2** | **~62 days** | **~3 months with 1 developer** |

### Phase 3 — Advanced Features

| Module | Estimated Days | Notes |
|--------|---------------|-------|
| Demand Forecast Dashboard (Charts + Recommendations) | 6 | Data visualization |
| Auto-Scheduling Wizard (Strategy Picker + Weights + Preview) | 6 | 3-step wizard + grid preview |
| SPLH Gauge + Weather Badge | 3 | Dashboard widgets |
| Schedule Builder Forecast Overlay | 4 | Overlay on existing builder |
| Labor Cost % Live Tracker | 2 | Gauge component |
| Tip Management (Entry + Distribution Preview + Reports) | 4 | Pool config + auto-distribution |
| Compliance Settings (Jurisdiction picker + Premium display) | 3 | Predictive scheduling |
| Employee Engagement (Surveys + Recognition Feed + Milestones) | 8 | Social features |
| Engagement Dashboard (Sentiment Trend + Flight Risk) | 5 | Analytics UI |
| Intraday Labor Dashboard (Live Charts + Alerts) | 5 | Real-time polling + Chart.js |
| Photo Verification Clock | 3 | Camera capture component |
| Earned vs. Actual Hours View | 2 | Dashboard extension |
| **Total Phase 3** | **~51 days** | **~2.5 months with 1 developer** |

### Phase 4 — Platform Extensions (build on customer demand)

| Module | Estimated Days | Notes |
|--------|---------------|-------|
| Hiring Pipeline (Kanban + Job Postings + Applicant Modal) | 8 | DnD kanban + pipeline |
| Interview Scheduling + Trial Shift Evaluation | 3 | Calendar integration |
| Training / LMS (Course Viewer + Quiz Player + Progress) | 8 | Video player, quiz engine |
| Knowledge Base + Handbook (Article Browser + Acknowledgements) | 4 | Search + rich text |
| Digital Forms Builder (Drag-Drop + Field Types + Preview) | 8 | DnD form builder |
| Digital Forms Filler + Submissions Viewer | 4 | Camera/signature/PDF |
| Company Newsfeed (Feed + Reactions + Comments + Moderation) | 5 | Social feed |
| Performance Reviews (Self-Assessment + Manager Review + Goals) | 6 | Multi-step flow |
| Onboarding Portal (Self-Service Steps) | 4 | Progress tracking |
| Shift Bidding + Preferences UI | 3 | After auto-scheduling |
| Facial Recognition Clock | 4 | ML model integration |
| GPS Live Map + Route History | 5 | Map integration |
| Mileage Tracking Table + Reimbursement | 2 | Table + export |
| Hiring + Training Analytics | 6 | Chart.js analytics |
| What-If Schedule Comparison | 3 | Side-by-side variant view |
| **Total Phase 4** | **~73 days** | **~3.5 months (not all needed)** |

---

## 15. Deployment

### Docker Build

```dockerfile
# Dockerfile
FROM node:20-alpine AS build
WORKDIR /app
COPY package.json pnpm-lock.yaml ./
RUN corepack enable pnpm && pnpm install --frozen-lockfile
COPY . .
RUN pnpm build

FROM nginx:alpine
COPY --from=build /app/dist /usr/share/nginx/html
COPY nginx.conf /etc/nginx/conf.d/default.conf
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
```

### Nginx Config

```nginx
server {
    listen 80;
    root /usr/share/nginx/html;
    index index.html;

    # SPA fallback — all routes serve index.html
    location / {
        try_files $uri $uri/ /index.html;
    }

    # API proxy (if same domain)
    location /api/ {
        proxy_pass http://staff-backend:5200;
        proxy_http_version 1.1;
        proxy_set_header Upgrade $http_upgrade;
        proxy_set_header Connection "upgrade";
    }

    # SignalR proxy
    location /hubs/ {
        proxy_pass http://staff-backend:5200;
        proxy_http_version 1.1;
        proxy_set_header Upgrade $http_upgrade;
        proxy_set_header Connection "upgrade";
    }

    # Cache static assets
    location ~* \.(js|css|png|jpg|jpeg|gif|ico|svg|woff2)$ {
        expires 1y;
        add_header Cache-Control "public, immutable";
    }

    # Security headers
    add_header X-Frame-Options "SAMEORIGIN" always;
    add_header X-Content-Type-Options "nosniff" always;
    add_header X-XSS-Protection "1; mode=block" always;
    add_header Referrer-Policy "strict-origin-when-cross-origin" always;
}
```

---

*This report serves as the complete frontend specification for Staff Pro. It should be used as the primary reference during development alongside the Backend Development Report.*
