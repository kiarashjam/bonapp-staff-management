import { useState } from 'react';
import { Outlet, useNavigate, useLocation } from 'react-router-dom';
import { Layout, Menu, Avatar, Badge, Dropdown, Button, Typography, Space } from 'antd';
import {
  DashboardOutlined, TeamOutlined, CalendarOutlined, ClockCircleOutlined,
  FileTextOutlined, DollarOutlined, SettingOutlined, BellOutlined,
  LogoutOutlined, UserOutlined, MenuFoldOutlined, MenuUnfoldOutlined,
  SoundOutlined, ScheduleOutlined, ApiOutlined,
} from '@ant-design/icons';
import { useAppSelector, useAppDispatch } from '../../store/store';
import { logout } from '../../store/authSlice';
import { useGetNotificationsQuery } from '../../api/settingsApi';

const { Header, Sider, Content } = Layout;
const { Text } = Typography;

export default function AppLayout() {
  const [collapsed, setCollapsed] = useState(false);
  const navigate = useNavigate();
  const location = useLocation();
  const dispatch = useAppDispatch();
  const { user } = useAppSelector((s) => s.auth);
  const { data: notifData } = useGetNotificationsQuery({ page: 1, pageSize: 5 });

  const isManager = user?.role === 'Admin' || user?.role === 'Manager' || user?.role === 'SuperAdmin';

  const menuItems = [
    { key: '/dashboard', icon: <DashboardOutlined />, label: 'Dashboard' },
    ...(isManager ? [{ key: '/employees', icon: <TeamOutlined />, label: 'Employees' }] : []),
    { key: '/schedules', icon: <CalendarOutlined />, label: 'Schedules' },
    { key: '/time-off', icon: <ScheduleOutlined />, label: 'Time Off' },
    { key: '/clock', icon: <ClockCircleOutlined />, label: 'Clock In/Out' },
    ...(isManager ? [{ key: '/timesheets', icon: <FileTextOutlined />, label: 'Timesheets' }] : []),
    ...(isManager ? [{ key: '/payroll', icon: <DollarOutlined />, label: 'Payroll' }] : []),
    { key: '/announcements', icon: <SoundOutlined />, label: 'Announcements' },
    ...(isManager ? [{ key: '/pos-integration', icon: <ApiOutlined />, label: 'POS Integration' }] : []),
    ...(isManager ? [{ key: '/settings', icon: <SettingOutlined />, label: 'Settings' }] : []),
  ];

  const profileMenuItems = [
    { key: 'profile', icon: <UserOutlined />, label: 'Profile Settings' },
    { key: 'logout', icon: <LogoutOutlined />, label: 'Logout', danger: true },
  ];

  return (
    <Layout style={{ minHeight: '100vh' }}>
      <Sider
        trigger={null}
        collapsible
        collapsed={collapsed}
        breakpoint="lg"
        onBreakpoint={(broken) => setCollapsed(broken)}
        style={{ background: '#fff', borderRight: '1px solid #f0f0f0' }}
      >
        <div style={{ padding: '16px', textAlign: 'center', borderBottom: '1px solid #f0f0f0' }}>
          <Text strong style={{ fontSize: collapsed ? 14 : 20, color: '#4F46E5' }}>
            {collapsed ? 'SP' : 'Staff Pro'}
          </Text>
        </div>
        <Menu
          mode="inline"
          selectedKeys={[location.pathname]}
          items={menuItems}
          onClick={({ key }) => navigate(key)}
          style={{ borderRight: 'none' }}
        />
      </Sider>
      <Layout>
        <Header style={{
          padding: '0 24px', background: '#fff', display: 'flex',
          alignItems: 'center', justifyContent: 'space-between',
          borderBottom: '1px solid #f0f0f0', height: 64,
        }}>
          <Button
            type="text"
            icon={collapsed ? <MenuUnfoldOutlined /> : <MenuFoldOutlined />}
            onClick={() => setCollapsed(!collapsed)}
          />
          <Space size="middle">
            <Badge count={notifData?.unreadCount || 0} size="small">
              <Button type="text" icon={<BellOutlined />} onClick={() => navigate('/notifications')} />
            </Badge>
            <Dropdown
              menu={{
                items: profileMenuItems,
                onClick: ({ key }) => {
                  if (key === 'logout') { dispatch(logout()); navigate('/login'); }
                },
              }}
              trigger={['click']}
            >
              <Space style={{ cursor: 'pointer' }}>
                <Avatar style={{ backgroundColor: '#4F46E5' }} icon={<UserOutlined />} />
                <Text>{user?.firstName} {user?.lastName}</Text>
              </Space>
            </Dropdown>
          </Space>
        </Header>
        <Content style={{ margin: 24, minHeight: 280 }}>
          <Outlet />
        </Content>
      </Layout>
    </Layout>
  );
}
