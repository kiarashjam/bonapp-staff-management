import { Routes, Route, Navigate } from 'react-router-dom';
import { useAppSelector } from './store/store';
import AppLayout from './components/layout/AppLayout';
import LoginPage from './pages/LoginPage';
import RegisterPage from './pages/RegisterPage';
import DashboardPage from './pages/DashboardPage';
import EmployeeListPage from './pages/EmployeeListPage';
import EmployeeDetailPage from './pages/EmployeeDetailPage';
import EmployeeCreatePage from './pages/EmployeeCreatePage';
import ScheduleListPage from './pages/ScheduleListPage';
import ScheduleBuilderPage from './pages/ScheduleBuilderPage';
import TimeOffPage from './pages/TimeOffPage';
import ClockPage from './pages/ClockPage';
import TimesheetPage from './pages/TimesheetPage';
import PayrollPage from './pages/PayrollPage';
import SettingsPage from './pages/SettingsPage';
import AnnouncementsPage from './pages/AnnouncementsPage';
import PosIntegrationPage from './pages/PosIntegrationPage';

function ProtectedRoute({ children }: { children: React.ReactNode }) {
  const { isAuthenticated } = useAppSelector((state) => state.auth);
  if (!isAuthenticated) return <Navigate to="/login" replace />;
  return <>{children}</>;
}

function App() {
  return (
    <Routes>
      {/* Public Routes */}
      <Route path="/login" element={<LoginPage />} />
      <Route path="/register" element={<RegisterPage />} />

      {/* Protected Routes */}
      <Route path="/" element={<ProtectedRoute><AppLayout /></ProtectedRoute>}>
        <Route index element={<Navigate to="/dashboard" replace />} />
        <Route path="dashboard" element={<DashboardPage />} />
        <Route path="employees" element={<EmployeeListPage />} />
        <Route path="employees/new" element={<EmployeeCreatePage />} />
        <Route path="employees/:id" element={<EmployeeDetailPage />} />
        <Route path="schedules" element={<ScheduleListPage />} />
        <Route path="schedules/:id" element={<ScheduleBuilderPage />} />
        <Route path="time-off" element={<TimeOffPage />} />
        <Route path="clock" element={<ClockPage />} />
        <Route path="timesheets" element={<TimesheetPage />} />
        <Route path="payroll" element={<PayrollPage />} />
        <Route path="announcements" element={<AnnouncementsPage />} />
        <Route path="pos-integration" element={<PosIntegrationPage />} />
        <Route path="settings" element={<SettingsPage />} />
      </Route>

      <Route path="*" element={<Navigate to="/dashboard" replace />} />
    </Routes>
  );
}

export default App;
