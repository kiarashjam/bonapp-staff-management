import { useState } from 'react';
import { Table, Button, Tag, Space, Typography, Card, Modal, Form, Select, DatePicker, Input, message, Popconfirm } from 'antd';
import { PlusOutlined, CheckOutlined, CloseOutlined } from '@ant-design/icons';
import { useGetTimeOffRequestsQuery, useCreateTimeOffRequestMutation, useReviewTimeOffRequestMutation } from '../api/timeOffApi';
import { useGetLeaveTypesQuery } from '../api/settingsApi';
import { useGetEmployeesQuery } from '../api/employeeApi';
import { useAppSelector } from '../store/store';
import type { TimeOffRequest } from '../types';

const { Title } = Typography;
const { RangePicker } = DatePicker;

const statusColors: Record<string, string> = { Pending: 'gold', Approved: 'green', Denied: 'red', Cancelled: 'default' };

export default function TimeOffPage() {
  const { user } = useAppSelector((s) => s.auth);
  const isManager = user?.role === 'Admin' || user?.role === 'Manager' || user?.role === 'SuperAdmin';
  const [page, setPage] = useState(1);
  const [showCreate, setShowCreate] = useState(false);
  const { data, isLoading } = useGetTimeOffRequestsQuery({ page, pageSize: 20 });
  const { data: leaveTypes } = useGetLeaveTypesQuery();
  const { data: employees } = useGetEmployeesQuery({ pageSize: 100 });
  const [createRequest, { isLoading: creating }] = useCreateTimeOffRequestMutation();
  const [reviewRequest] = useReviewTimeOffRequestMutation();
  const [form] = Form.useForm();

  const columns = [
    { title: 'Employee', dataIndex: 'employeeName', key: 'employee' },
    { title: 'Type', key: 'type', render: (_: unknown, r: TimeOffRequest) => <Tag color={r.leaveTypeColor}>{r.leaveTypeName}</Tag> },
    { title: 'Dates', key: 'dates', render: (_: unknown, r: TimeOffRequest) => `${r.startDate} — ${r.endDate}` },
    { title: 'Days', dataIndex: 'totalDays', key: 'days' },
    { title: 'Reason', dataIndex: 'reason', key: 'reason', render: (v: string) => v || '-' },
    { title: 'Status', dataIndex: 'status', key: 'status', render: (v: string) => <Tag color={statusColors[v]}>{v}</Tag> },
    ...(isManager ? [{
      title: 'Actions', key: 'actions',
      render: (_: unknown, r: TimeOffRequest) => r.status === 'Pending' ? (
        <Space>
          <Popconfirm title="Approve this request?" onConfirm={() => reviewRequest({ id: r.id, approved: true })}>
            <Button type="text" icon={<CheckOutlined />} style={{ color: 'green' }} size="small">Approve</Button>
          </Popconfirm>
          <Popconfirm title="Deny this request?" onConfirm={() => reviewRequest({ id: r.id, approved: false })}>
            <Button type="text" icon={<CloseOutlined />} danger size="small">Deny</Button>
          </Popconfirm>
        </Space>
      ) : null,
    }] : []),
  ];

  const handleCreate = async (values: Record<string, unknown>) => {
    try {
      const dates = values.dates as [{ format: (f: string) => string }, { format: (f: string) => string }];
      await createRequest({
        employeeId: values.employeeId || user?.employeeId,
        leaveTypeId: values.leaveTypeId,
        startDate: dates[0].format('YYYY-MM-DD'),
        endDate: dates[1].format('YYYY-MM-DD'),
        reason: values.reason,
      }).unwrap();
      message.success('Time-off request submitted');
      setShowCreate(false);
      form.resetFields();
    } catch {
      message.error('Failed to submit request');
    }
  };

  return (
    <>
      <div style={{ display: 'flex', justifyContent: 'space-between', marginBottom: 16 }}>
        <Title level={3} style={{ margin: 0 }}>Time Off Requests</Title>
        <Button type="primary" icon={<PlusOutlined />} onClick={() => setShowCreate(true)}>Request Time Off</Button>
      </div>

      <Card>
        <Table columns={columns} dataSource={data?.items || []} rowKey="id" loading={isLoading} pagination={{ current: page, pageSize: 20, total: data?.totalCount || 0, onChange: setPage }} />
      </Card>

      <Modal title="Request Time Off" open={showCreate} onCancel={() => setShowCreate(false)} footer={null}>
        <Form form={form} layout="vertical" onFinish={handleCreate}>
          {isManager && (
            <Form.Item name="employeeId" label="Employee" rules={[{ required: true }]}>
              <Select showSearch placeholder="Select employee" optionFilterProp="label"
                options={(employees?.items || []).map(e => ({ value: e.id, label: `${e.firstName} ${e.lastName}` }))} />
            </Form.Item>
          )}
          <Form.Item name="leaveTypeId" label="Leave Type" rules={[{ required: true }]}>
            <Select placeholder="Select leave type" options={(leaveTypes || []).map(t => ({ value: t.id, label: t.name }))} />
          </Form.Item>
          <Form.Item name="dates" label="Dates" rules={[{ required: true }]}>
            <RangePicker style={{ width: '100%' }} />
          </Form.Item>
          <Form.Item name="reason" label="Reason">
            <Input.TextArea rows={3} placeholder="Optional reason..." />
          </Form.Item>
          <Space>
            <Button type="primary" htmlType="submit" loading={creating}>Submit Request</Button>
            <Button onClick={() => setShowCreate(false)}>Cancel</Button>
          </Space>
        </Form>
      </Modal>
    </>
  );
}
