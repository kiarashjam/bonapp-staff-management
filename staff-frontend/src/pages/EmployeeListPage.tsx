import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { Table, Button, Input, Space, Tag, Avatar, Typography, Card } from 'antd';
import { PlusOutlined, SearchOutlined, UserOutlined } from '@ant-design/icons';
import { useGetEmployeesQuery } from '../api/employeeApi';
import type { EmployeeList } from '../types';

const { Title } = Typography;

const statusColors: Record<string, string> = {
  Active: 'green', OnLeave: 'orange', Suspended: 'red', Terminated: 'default',
};

export default function EmployeeListPage() {
  const navigate = useNavigate();
  const [page, setPage] = useState(1);
  const [search, setSearch] = useState('');
  const { data, isLoading } = useGetEmployeesQuery({ page, pageSize: 20, search: search || undefined });

  const columns = [
    {
      title: 'Employee', dataIndex: 'firstName', key: 'name',
      render: (_: unknown, r: EmployeeList) => (
        <Space>
          <Avatar src={r.profilePhotoUrl} icon={<UserOutlined />} style={{ backgroundColor: '#4F46E5' }} />
          <div>
            <div style={{ fontWeight: 500 }}>{r.firstName} {r.lastName}</div>
            <div style={{ color: '#999', fontSize: 12 }}>{r.email}</div>
          </div>
        </Space>
      ),
    },
    { title: 'Role', dataIndex: 'primaryRoleName', key: 'role', render: (v: string) => v ? <Tag color="blue">{v}</Tag> : '-' },
    { title: 'Location', dataIndex: 'locationName', key: 'location', render: (v: string) => v || '-' },
    { title: 'Status', dataIndex: 'status', key: 'status', render: (v: string) => <Tag color={statusColors[v]}>{v}</Tag> },
    { title: 'Hire Date', dataIndex: 'hireDate', key: 'hireDate', render: (v: string) => new Date(v).toLocaleDateString() },
  ];

  return (
    <>
      <div style={{ display: 'flex', justifyContent: 'space-between', marginBottom: 16 }}>
        <Title level={3} style={{ margin: 0 }}>Employees</Title>
        <Button type="primary" icon={<PlusOutlined />} onClick={() => navigate('/employees/new')}>Add Employee</Button>
      </div>

      <Card>
        <Space style={{ marginBottom: 16, width: '100%' }}>
          <Input
            placeholder="Search employees..."
            prefix={<SearchOutlined />}
            value={search}
            onChange={(e) => { setSearch(e.target.value); setPage(1); }}
            style={{ width: 300 }}
            allowClear
          />
        </Space>

        <Table
          columns={columns}
          dataSource={data?.items || []}
          rowKey="id"
          loading={isLoading}
          pagination={{ current: page, pageSize: 20, total: data?.totalCount || 0, onChange: setPage }}
          onRow={(record) => ({ onClick: () => navigate(`/employees/${record.id}`), style: { cursor: 'pointer' } })}
        />
      </Card>
    </>
  );
}
