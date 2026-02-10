import { Card, Col, Row, Statistic, Typography, List, Tag, Space } from 'antd';
import { TeamOutlined, ClockCircleOutlined, CalendarOutlined, FileTextOutlined, ScheduleOutlined } from '@ant-design/icons';
import { useAppSelector } from '../store/store';
import { useGetManagerDashboardQuery, useGetEmployeeDashboardQuery } from '../api/settingsApi';
import type { ManagerDashboard, EmployeeDashboard, ShiftAssignment } from '../types';

const { Title, Text } = Typography;

export default function DashboardPage() {
  const { user } = useAppSelector((s) => s.auth);
  const isManager = user?.role === 'Admin' || user?.role === 'Manager' || user?.role === 'SuperAdmin';

  return isManager ? <ManagerDashboardView /> : <EmployeeDashboardView />;
}

function ManagerDashboardView() {
  const { data, isLoading } = useGetManagerDashboardQuery();
  const dashboard = data as unknown as ManagerDashboard | undefined;

  return (
    <>
      <Title level={3}>Manager Dashboard</Title>
      <Row gutter={[16, 16]}>
        <Col xs={12} sm={6}><Card><Statistic title="Total Employees" value={dashboard?.totalEmployees || 0} prefix={<TeamOutlined />} /></Card></Col>
        <Col xs={12} sm={6}><Card><Statistic title="Active Today" value={dashboard?.activeToday || 0} prefix={<ClockCircleOutlined />} valueStyle={{ color: '#52c41a' }} /></Card></Col>
        <Col xs={12} sm={6}><Card><Statistic title="On Leave Today" value={dashboard?.onLeaveToday || 0} prefix={<CalendarOutlined />} valueStyle={{ color: '#faad14' }} /></Card></Col>
        <Col xs={12} sm={6}><Card><Statistic title="Pending Requests" value={dashboard?.pendingTimeOffRequests || 0} prefix={<FileTextOutlined />} valueStyle={{ color: '#ff4d4f' }} /></Card></Col>
      </Row>

      <Row gutter={[16, 16]} style={{ marginTop: 16 }}>
        <Col xs={12} sm={6}><Card><Statistic title="Pending Timesheets" value={dashboard?.pendingTimesheets || 0} prefix={<ScheduleOutlined />} /></Card></Col>
        <Col xs={12} sm={6}><Card><Statistic title="Weekly Labor Hours" value={dashboard?.weeklyLaborHours || 0} suffix="hrs" precision={1} /></Card></Col>
      </Row>

      <Card title="Today's Shifts" style={{ marginTop: 16 }} loading={isLoading}>
        <List
          dataSource={dashboard?.todayShifts || []}
          locale={{ emptyText: 'No shifts scheduled for today' }}
          renderItem={(shift: ShiftAssignment) => (
            <List.Item>
              <Space>
                <Text strong>{shift.employeeName}</Text>
                <Tag color={shift.roleColor}>{shift.roleName}</Tag>
                <Text type="secondary">{shift.startTime} - {shift.endTime}</Text>
                {shift.stationName && <Tag>{shift.stationName}</Tag>}
              </Space>
            </List.Item>
          )}
        />
      </Card>
    </>
  );
}

function EmployeeDashboardView() {
  const { data, isLoading } = useGetEmployeeDashboardQuery();
  const dashboard = data as unknown as EmployeeDashboard | undefined;

  return (
    <>
      <Title level={3}>My Dashboard</Title>
      <Row gutter={[16, 16]}>
        <Col xs={12} sm={8}>
          <Card>
            <Statistic
              title="Clock Status"
              value={dashboard?.clockStatus?.isClockedIn ? 'Clocked In' : 'Clocked Out'}
              valueStyle={{ color: dashboard?.clockStatus?.isClockedIn ? '#52c41a' : '#999' }}
            />
          </Card>
        </Col>
        <Col xs={12} sm={8}><Card><Statistic title="Hours This Week" value={dashboard?.hoursThisWeek || 0} suffix="hrs" precision={1} /></Card></Col>
        <Col xs={12} sm={8}><Card><Statistic title="Pending Time-Off" value={dashboard?.pendingTimeOffRequests || 0} /></Card></Col>
      </Row>

      <Card title="Upcoming Shifts" style={{ marginTop: 16 }} loading={isLoading}>
        <List
          dataSource={dashboard?.upcomingShifts || []}
          locale={{ emptyText: 'No upcoming shifts' }}
          renderItem={(shift: ShiftAssignment) => (
            <List.Item>
              <Space>
                <Text strong>{shift.date}</Text>
                <Text>{shift.startTime} - {shift.endTime}</Text>
                {shift.roleName && <Tag color={shift.roleColor}>{shift.roleName}</Tag>}
                {shift.stationName && <Tag>{shift.stationName}</Tag>}
                <Text type="secondary">{shift.netHours}h</Text>
              </Space>
            </List.Item>
          )}
        />
      </Card>

      <Card title="Leave Balances" style={{ marginTop: 16 }}>
        <Row gutter={[16, 16]}>
          {(dashboard?.leaveBalances || []).map((lb) => (
            <Col xs={12} sm={6} key={lb.leaveTypeId}>
              <Statistic title={lb.leaveTypeName} value={lb.remaining} suffix={`/ ${lb.entitled}`} />
            </Col>
          ))}
        </Row>
      </Card>
    </>
  );
}
