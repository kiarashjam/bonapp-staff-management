import { useParams, useNavigate } from 'react-router-dom';
import { Card, Descriptions, Tag, Tabs, Button, Space, Typography, Spin, Row, Col, Statistic } from 'antd';
import { ArrowLeftOutlined, EditOutlined } from '@ant-design/icons';
import { useGetEmployeeQuery, useGetLeaveBalancesQuery } from '../api/employeeApi';

const { Title, Text } = Typography;

const statusColors: Record<string, string> = {
  Active: 'green', OnLeave: 'orange', Suspended: 'red', Terminated: 'default',
};

export default function EmployeeDetailPage() {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const { data: employee, isLoading } = useGetEmployeeQuery(id!);
  const { data: balances } = useGetLeaveBalancesQuery({ id: id!, year: new Date().getFullYear() });

  if (isLoading) return <Spin size="large" />;
  if (!employee) return <Text>Employee not found</Text>;

  const tabItems = [
    {
      key: 'details', label: 'Details',
      children: (
        <Descriptions bordered column={{ xs: 1, sm: 2 }}>
          <Descriptions.Item label="Email">{employee.email}</Descriptions.Item>
          <Descriptions.Item label="Phone">{employee.phone || '-'}</Descriptions.Item>
          <Descriptions.Item label="Date of Birth">{employee.dateOfBirth ? new Date(employee.dateOfBirth).toLocaleDateString() : '-'}</Descriptions.Item>
          <Descriptions.Item label="Hire Date">{new Date(employee.hireDate).toLocaleDateString()}</Descriptions.Item>
          <Descriptions.Item label="Address">{[employee.address, employee.city, employee.postalCode].filter(Boolean).join(', ') || '-'}</Descriptions.Item>
          <Descriptions.Item label="Location">{employee.locationName || '-'}</Descriptions.Item>
          <Descriptions.Item label="Emergency Contact">{employee.emergencyContactName ? `${employee.emergencyContactName} (${employee.emergencyContactRelation}) - ${employee.emergencyContactPhone}` : '-'}</Descriptions.Item>
        </Descriptions>
      ),
    },
    {
      key: 'contract', label: 'Contract',
      children: employee.activeContract ? (
        <Descriptions bordered column={{ xs: 1, sm: 2 }}>
          <Descriptions.Item label="Type"><Tag>{employee.activeContract.contractType}</Tag></Descriptions.Item>
          <Descriptions.Item label="Hours/Week">{employee.activeContract.contractedHoursPerWeek}h</Descriptions.Item>
          <Descriptions.Item label="Hourly Rate">CHF {employee.activeContract.hourlyRate.toFixed(2)}</Descriptions.Item>
          <Descriptions.Item label="Start Date">{new Date(employee.activeContract.startDate).toLocaleDateString()}</Descriptions.Item>
          <Descriptions.Item label="End Date">{employee.activeContract.endDate ? new Date(employee.activeContract.endDate).toLocaleDateString() : 'Permanent'}</Descriptions.Item>
          <Descriptions.Item label="Notice Period">{employee.activeContract.noticePeriodDays} days</Descriptions.Item>
        </Descriptions>
      ) : <Text type="secondary">No active contract</Text>,
    },
    {
      key: 'roles', label: 'Roles',
      children: (
        <Space wrap>
          {employee.roles.map((r) => (
            <Tag key={r.roleId} color={r.roleColor} style={{ padding: '4px 12px', fontSize: 14 }}>
              {r.roleName} ({r.proficiencyLevel}) {r.isPrimary && '★'}
            </Tag>
          ))}
          {employee.roles.length === 0 && <Text type="secondary">No roles assigned</Text>}
        </Space>
      ),
    },
    {
      key: 'leave', label: 'Leave Balances',
      children: (
        <Row gutter={[16, 16]}>
          {(balances || []).map((lb) => (
            <Col xs={12} sm={6} key={lb.id}>
              <Card size="small"><Statistic title={lb.leaveTypeName} value={lb.remaining} suffix={`/ ${lb.entitled} days`} precision={1} /></Card>
            </Col>
          ))}
          {(!balances || balances.length === 0) && <Text type="secondary">No leave balances</Text>}
        </Row>
      ),
    },
  ];

  return (
    <>
      <Space style={{ marginBottom: 16 }}>
        <Button icon={<ArrowLeftOutlined />} onClick={() => navigate('/employees')}>Back</Button>
      </Space>

      <Card>
        <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: 16 }}>
          <div>
            <Title level={3} style={{ margin: 0 }}>{employee.firstName} {employee.lastName}</Title>
            <Tag color={statusColors[employee.status]}>{employee.status}</Tag>
          </div>
          <Button icon={<EditOutlined />}>Edit</Button>
        </div>

        <Tabs items={tabItems} />
      </Card>
    </>
  );
}
