import { useState } from 'react';
import { Tabs, Card, Table, Button, Modal, Form, Input, InputNumber, Typography, Space, Tag, message, ColorPicker } from 'antd';
import { PlusOutlined } from '@ant-design/icons';
import {
  useGetRolesQuery, useCreateRoleMutation,
  useGetStationsQuery, useCreateStationMutation,
  useGetDepartmentsQuery, useCreateDepartmentMutation,
  useGetShiftTemplatesQuery, useCreateShiftTemplateMutation,
  useGetLeaveTypesQuery, useCreateLeaveTypeMutation,
  useGetLocationsQuery, useCreateLocationMutation,
} from '../api/settingsApi';
import type { Role, Station, ShiftTemplate, LeaveType } from '../types';

const { Title } = Typography;

function CrudSection<T extends { id: string }>({ title, columns, data, isLoading, onAdd, formFields }: {
  title: string; columns: object[]; data: T[] | undefined; isLoading: boolean;
  onAdd: (values: Record<string, unknown>) => Promise<void>;
  formFields: React.ReactNode;
}) {
  const [show, setShow] = useState(false);
  const [form] = Form.useForm();

  const handleSubmit = async (values: Record<string, unknown>) => {
    try {
      await onAdd(values);
      message.success(`${title.slice(0, -1)} created`);
      setShow(false);
      form.resetFields();
    } catch {
      message.error('Failed to create');
    }
  };

  return (
    <>
      <div style={{ display: 'flex', justifyContent: 'space-between', marginBottom: 12 }}>
        <Title level={5}>{title}</Title>
        <Button icon={<PlusOutlined />} onClick={() => setShow(true)}>Add</Button>
      </div>
      <Table columns={columns as any} dataSource={data || []} rowKey="id" loading={isLoading} pagination={false} size="small" />
      <Modal title={`Add ${title.slice(0, -1)}`} open={show} onCancel={() => setShow(false)} footer={null}>
        <Form form={form} layout="vertical" onFinish={handleSubmit}>
          {formFields}
          <Space><Button type="primary" htmlType="submit">Create</Button><Button onClick={() => setShow(false)}>Cancel</Button></Space>
        </Form>
      </Modal>
    </>
  );
}

export default function SettingsPage() {
  const { data: roles, isLoading: loadingRoles } = useGetRolesQuery();
  const [createRole] = useCreateRoleMutation();
  const { data: stations, isLoading: loadingStations } = useGetStationsQuery();
  const [createStation] = useCreateStationMutation();
  const { data: departments, isLoading: loadingDepts } = useGetDepartmentsQuery();
  const [createDept] = useCreateDepartmentMutation();
  const { data: templates, isLoading: loadingTemplates } = useGetShiftTemplatesQuery();
  const [createTemplate] = useCreateShiftTemplateMutation();
  const { data: leaveTypes, isLoading: loadingLeave } = useGetLeaveTypesQuery();
  const [createLeaveType] = useCreateLeaveTypeMutation();
  const { data: locations, isLoading: loadingLocs } = useGetLocationsQuery();
  const [createLocation] = useCreateLocationMutation();

  const tabItems = [
    {
      key: 'roles', label: 'Roles',
      children: <CrudSection<Role> title="Roles" data={roles} isLoading={loadingRoles}
        columns={[
          { title: 'Name', dataIndex: 'name' },
          { title: 'Color', dataIndex: 'color', render: (v: string) => <Tag color={v}>{v}</Tag> },
          { title: 'Rate', dataIndex: 'defaultHourlyRate', render: (v: number) => `CHF ${v.toFixed(2)}/h` },
          { title: 'Department', dataIndex: 'departmentName', render: (v: string) => v || '-' },
        ]}
        onAdd={async (v) => { await createRole(v).unwrap(); }}
        formFields={<>
          <Form.Item name="name" label="Name" rules={[{ required: true }]}><Input /></Form.Item>
          <Form.Item name="defaultHourlyRate" label="Hourly Rate (CHF)" rules={[{ required: true }]}><InputNumber min={0} precision={2} style={{ width: '100%' }} /></Form.Item>
          <Form.Item name="color" label="Color" initialValue="#3B82F6"><Input /></Form.Item>
        </>}
      />,
    },
    {
      key: 'stations', label: 'Stations',
      children: <CrudSection<Station> title="Stations" data={stations} isLoading={loadingStations}
        columns={[
          { title: 'Name', dataIndex: 'name' },
          { title: 'Max Capacity', dataIndex: 'maxCapacity' },
          { title: 'Location', dataIndex: 'locationName', render: (v: string) => v || '-' },
        ]}
        onAdd={async (v) => { await createStation(v).unwrap(); }}
        formFields={<>
          <Form.Item name="name" label="Name" rules={[{ required: true }]}><Input /></Form.Item>
          <Form.Item name="maxCapacity" label="Max Capacity" initialValue={5}><InputNumber min={1} /></Form.Item>
          <Form.Item name="description" label="Description"><Input.TextArea /></Form.Item>
        </>}
      />,
    },
    {
      key: 'templates', label: 'Shift Templates',
      children: <CrudSection<ShiftTemplate> title="Shift Templates" data={templates} isLoading={loadingTemplates}
        columns={[
          { title: 'Name', dataIndex: 'name' },
          { title: 'Start', dataIndex: 'startTime' },
          { title: 'End', dataIndex: 'endTime' },
          { title: 'Break', dataIndex: 'breakDurationMinutes', render: (v: number) => `${v} min` },
          { title: 'Net Hours', dataIndex: 'netHours', render: (v: number) => `${v.toFixed(1)}h` },
          { title: 'Color', dataIndex: 'color', render: (v: string) => <Tag color={v}>{v}</Tag> },
        ]}
        onAdd={async (v) => { await createTemplate(v).unwrap(); }}
        formFields={<>
          <Form.Item name="name" label="Name" rules={[{ required: true }]}><Input placeholder="Morning" /></Form.Item>
          <Form.Item name="startTime" label="Start Time" rules={[{ required: true }]}><Input placeholder="07:00" /></Form.Item>
          <Form.Item name="endTime" label="End Time" rules={[{ required: true }]}><Input placeholder="15:00" /></Form.Item>
          <Form.Item name="breakDurationMinutes" label="Break (min)" initialValue={30}><InputNumber min={0} /></Form.Item>
          <Form.Item name="color" label="Color" initialValue="#3B82F6"><Input /></Form.Item>
        </>}
      />,
    },
    {
      key: 'leave', label: 'Leave Types',
      children: <CrudSection<LeaveType> title="Leave Types" data={leaveTypes} isLoading={loadingLeave}
        columns={[
          { title: 'Name', dataIndex: 'name' },
          { title: 'Paid', dataIndex: 'isPaid', render: (v: boolean) => v ? <Tag color="green">Yes</Tag> : <Tag>No</Tag> },
          { title: 'Max Days/Year', dataIndex: 'maxDaysPerYear', render: (v: number | null) => v ?? 'Unlimited' },
          { title: 'Color', dataIndex: 'color', render: (v: string) => <Tag color={v}>{v}</Tag> },
        ]}
        onAdd={async (v) => { await createLeaveType(v).unwrap(); }}
        formFields={<>
          <Form.Item name="name" label="Name" rules={[{ required: true }]}><Input /></Form.Item>
          <Form.Item name="maxDaysPerYear" label="Max Days/Year"><InputNumber min={1} /></Form.Item>
          <Form.Item name="color" label="Color" initialValue="#F59E0B"><Input /></Form.Item>
        </>}
      />,
    },
    {
      key: 'locations', label: 'Locations',
      children: <CrudSection title="Locations" data={locations} isLoading={loadingLocs}
        columns={[
          { title: 'Name', dataIndex: 'name' },
          { title: 'City', dataIndex: 'city', render: (v: string) => v || '-' },
          { title: 'Active', dataIndex: 'isActive', render: (v: boolean) => v ? <Tag color="green">Active</Tag> : <Tag>Inactive</Tag> },
        ]}
        onAdd={async (v) => { await createLocation(v).unwrap(); }}
        formFields={<>
          <Form.Item name="name" label="Name" rules={[{ required: true }]}><Input /></Form.Item>
          <Form.Item name="address" label="Address"><Input /></Form.Item>
          <Form.Item name="city" label="City"><Input /></Form.Item>
          <Form.Item name="postalCode" label="Postal Code"><Input /></Form.Item>
        </>}
      />,
    },
  ];

  return (
    <>
      <Title level={3}>Settings</Title>
      <Card>
        <Tabs items={tabItems} />
      </Card>
    </>
  );
}
