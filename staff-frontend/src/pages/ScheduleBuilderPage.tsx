import { useParams, useNavigate } from 'react-router-dom';
import { Card, Button, Tag, Space, Typography, Spin, Table, Modal, Form, Select, TimePicker, DatePicker, message, Popconfirm, Alert } from 'antd';
import { ArrowLeftOutlined, PlusOutlined, SendOutlined, LockOutlined, DeleteOutlined } from '@ant-design/icons';
import { useState } from 'react';
import { useGetScheduleDetailQuery, useCreateAssignmentMutation, useDeleteAssignmentMutation, usePublishScheduleMutation } from '../api/scheduleApi';
import { useGetEmployeesQuery } from '../api/employeeApi';
import { useGetRolesQuery, useGetStationsQuery, useGetShiftTemplatesQuery } from '../api/settingsApi';
import type { ShiftAssignment } from '../types';
import dayjs from 'dayjs';

const { Title, Text } = Typography;

export default function ScheduleBuilderPage() {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const { data: schedule, isLoading } = useGetScheduleDetailQuery(id!);
  const { data: employees } = useGetEmployeesQuery({ pageSize: 100 });
  const { data: roles } = useGetRolesQuery();
  const { data: stations } = useGetStationsQuery();
  const { data: templates } = useGetShiftTemplatesQuery();
  const [createAssignment, { isLoading: assigning }] = useCreateAssignmentMutation();
  const [deleteAssignment] = useDeleteAssignmentMutation();
  const [publishSchedule, { isLoading: publishing }] = usePublishScheduleMutation();
  const [showAdd, setShowAdd] = useState(false);
  const [form] = Form.useForm();

  if (isLoading) return <Spin size="large" />;
  if (!schedule) return <Text>Schedule not found</Text>;

  const isDraft = schedule.status === 'Draft';

  const handleAddAssignment = async (values: Record<string, unknown>) => {
    try {
      await createAssignment({
        schedulePeriodId: id!,
        employeeId: values.employeeId as string,
        roleId: values.roleId as string | undefined,
        stationId: values.stationId as string | undefined,
        shiftTemplateId: values.shiftTemplateId as string | undefined,
        date: (values.date as dayjs.Dayjs).format('YYYY-MM-DD'),
        startTime: (values.startTime as dayjs.Dayjs).format('HH:mm'),
        endTime: (values.endTime as dayjs.Dayjs).format('HH:mm'),
        breakDurationMinutes: 30,
        breakIsPaid: false,
      }).unwrap();
      message.success('Shift assigned');
      setShowAdd(false);
      form.resetFields();
    } catch {
      message.error('Failed to assign shift — check conflicts');
    }
  };

  const handlePublish = async () => {
    try {
      await publishSchedule(id!).unwrap();
      message.success('Schedule published! Employees notified.');
    } catch {
      message.error('Failed to publish');
    }
  };

  const columns = [
    { title: 'Date', dataIndex: 'date', key: 'date', sorter: (a: ShiftAssignment, b: ShiftAssignment) => a.date.localeCompare(b.date) },
    { title: 'Employee', dataIndex: 'employeeName', key: 'employee' },
    { title: 'Role', key: 'role', render: (_: unknown, r: ShiftAssignment) => r.roleName ? <Tag color={r.roleColor}>{r.roleName}</Tag> : '-' },
    { title: 'Station', dataIndex: 'stationName', key: 'station', render: (v: string) => v || '-' },
    { title: 'Time', key: 'time', render: (_: unknown, r: ShiftAssignment) => `${r.startTime} - ${r.endTime}` },
    { title: 'Hours', dataIndex: 'netHours', key: 'hours', render: (v: number) => `${v.toFixed(1)}h` },
    ...(isDraft ? [{
      title: '', key: 'actions',
      render: (_: unknown, r: ShiftAssignment) => (
        <Popconfirm title="Remove this shift?" onConfirm={() => deleteAssignment(r.id)}>
          <Button type="text" danger icon={<DeleteOutlined />} size="small" />
        </Popconfirm>
      ),
    }] : []),
  ];

  return (
    <>
      <Space style={{ marginBottom: 16 }}>
        <Button icon={<ArrowLeftOutlined />} onClick={() => navigate('/schedules')}>Back</Button>
      </Space>

      <Card>
        <div style={{ display: 'flex', justifyContent: 'space-between', marginBottom: 16 }}>
          <div>
            <Title level={3} style={{ margin: 0 }}>
              {schedule.locationName}: {schedule.startDate} — {schedule.endDate}
            </Title>
            <Tag color={schedule.status === 'Draft' ? 'default' : schedule.status === 'Published' ? 'blue' : 'green'}>
              {schedule.status}
            </Tag>
          </div>
          <Space>
            {isDraft && <Button icon={<PlusOutlined />} onClick={() => setShowAdd(true)}>Add Shift</Button>}
            {isDraft && (
              <Button type="primary" icon={<SendOutlined />} loading={publishing} onClick={handlePublish} style={{ background: '#4F46E5' }}>
                Publish Schedule
              </Button>
            )}
            {schedule.status === 'Published' && (
              <Button icon={<LockOutlined />}>Lock Schedule</Button>
            )}
          </Space>
        </div>

        {schedule.assignments.length === 0 && <Alert message="No shifts assigned yet. Click 'Add Shift' to start building the schedule." type="info" showIcon style={{ marginBottom: 16 }} />}

        <Table columns={columns} dataSource={schedule.assignments} rowKey="id" pagination={false} />
      </Card>

      <Modal title="Assign Shift" open={showAdd} onCancel={() => setShowAdd(false)} footer={null} width={600}>
        <Form form={form} layout="vertical" onFinish={handleAddAssignment}>
          <Form.Item name="employeeId" label="Employee" rules={[{ required: true }]}>
            <Select showSearch placeholder="Select employee" optionFilterProp="label"
              options={(employees?.items || []).map(e => ({ value: e.id, label: `${e.firstName} ${e.lastName}` }))} />
          </Form.Item>
          <Space style={{ width: '100%' }} size="middle">
            <Form.Item name="date" label="Date" rules={[{ required: true }]}>
              <DatePicker />
            </Form.Item>
            <Form.Item name="shiftTemplateId" label="Template">
              <Select allowClear placeholder="Quick fill" style={{ width: 180 }}
                options={(templates || []).map(t => ({ value: t.id, label: `${t.name} (${t.startTime}-${t.endTime})` }))} />
            </Form.Item>
          </Space>
          <Space style={{ width: '100%' }} size="middle">
            <Form.Item name="startTime" label="Start Time" rules={[{ required: true }]}>
              <TimePicker format="HH:mm" minuteStep={15} />
            </Form.Item>
            <Form.Item name="endTime" label="End Time" rules={[{ required: true }]}>
              <TimePicker format="HH:mm" minuteStep={15} />
            </Form.Item>
          </Space>
          <Space style={{ width: '100%' }} size="middle">
            <Form.Item name="roleId" label="Role">
              <Select allowClear placeholder="Select role" style={{ width: 200 }}
                options={(roles || []).map(r => ({ value: r.id, label: r.name }))} />
            </Form.Item>
            <Form.Item name="stationId" label="Station">
              <Select allowClear placeholder="Select station" style={{ width: 200 }}
                options={(stations || []).map(s => ({ value: s.id, label: s.name }))} />
            </Form.Item>
          </Space>
          <Space>
            <Button type="primary" htmlType="submit" loading={assigning}>Assign Shift</Button>
            <Button onClick={() => setShowAdd(false)}>Cancel</Button>
          </Space>
        </Form>
      </Modal>
    </>
  );
}
