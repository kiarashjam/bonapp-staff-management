import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { Table, Button, Tag, Space, Typography, Card, Modal, Form, DatePicker, Select, message } from 'antd';
import { PlusOutlined, CalendarOutlined } from '@ant-design/icons';
import { useGetSchedulesQuery, useCreateScheduleMutation } from '../api/scheduleApi';
import { useGetLocationsQuery } from '../api/settingsApi';
import type { SchedulePeriod } from '../types';
import dayjs from 'dayjs';

const { Title } = Typography;
const { RangePicker } = DatePicker;

const statusColors: Record<string, string> = { Draft: 'default', Published: 'blue', Locked: 'green' };

export default function ScheduleListPage() {
  const navigate = useNavigate();
  const [page, setPage] = useState(1);
  const [showCreate, setShowCreate] = useState(false);
  const { data, isLoading } = useGetSchedulesQuery({ page, pageSize: 20 });
  const { data: locations } = useGetLocationsQuery();
  const [createSchedule, { isLoading: creating }] = useCreateScheduleMutation();
  const [form] = Form.useForm();

  const columns = [
    {
      title: 'Period', key: 'period',
      render: (_: unknown, r: SchedulePeriod) => `${r.startDate} - ${r.endDate}`,
    },
    { title: 'Location', dataIndex: 'locationName', key: 'location' },
    { title: 'Status', dataIndex: 'status', key: 'status', render: (v: string) => <Tag color={statusColors[v]}>{v}</Tag> },
    { title: 'Assignments', dataIndex: 'totalAssignments', key: 'assignments' },
    { title: 'Employees', dataIndex: 'totalEmployees', key: 'employees' },
    {
      title: 'Actions', key: 'actions',
      render: (_: unknown, r: SchedulePeriod) => (
        <Button type="link" icon={<CalendarOutlined />} onClick={() => navigate(`/schedules/${r.id}`)}>
          {r.status === 'Draft' ? 'Edit' : 'View'}
        </Button>
      ),
    },
  ];

  const handleCreate = async (values: Record<string, unknown>) => {
    try {
      const dates = values.dates as [dayjs.Dayjs, dayjs.Dayjs];
      const result = await createSchedule({
        locationId: values.locationId as string,
        startDate: dates[0].format('YYYY-MM-DD'),
        endDate: dates[1].format('YYYY-MM-DD'),
        notes: values.notes as string | undefined,
      }).unwrap();
      message.success('Schedule period created');
      setShowCreate(false);
      form.resetFields();
      navigate(`/schedules/${result.id}`);
    } catch {
      message.error('Failed to create schedule period');
    }
  };

  return (
    <>
      <div style={{ display: 'flex', justifyContent: 'space-between', marginBottom: 16 }}>
        <Title level={3} style={{ margin: 0 }}>Schedules</Title>
        <Button type="primary" icon={<PlusOutlined />} onClick={() => setShowCreate(true)}>New Schedule</Button>
      </div>

      <Card>
        <Table columns={columns} dataSource={data?.items || []} rowKey="id" loading={isLoading} pagination={{ current: page, pageSize: 20, total: data?.totalCount || 0, onChange: setPage }} />
      </Card>

      <Modal title="Create Schedule Period" open={showCreate} onCancel={() => setShowCreate(false)} footer={null}>
        <Form form={form} layout="vertical" onFinish={handleCreate}>
          <Form.Item name="locationId" label="Location" rules={[{ required: true }]}>
            <Select placeholder="Select location" options={(locations || []).map(l => ({ value: l.id, label: l.name }))} />
          </Form.Item>
          <Form.Item name="dates" label="Date Range" rules={[{ required: true }]}>
            <RangePicker style={{ width: '100%' }} />
          </Form.Item>
          <Space>
            <Button type="primary" htmlType="submit" loading={creating}>Create</Button>
            <Button onClick={() => setShowCreate(false)}>Cancel</Button>
          </Space>
        </Form>
      </Modal>
    </>
  );
}
