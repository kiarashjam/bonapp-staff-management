import { useNavigate } from 'react-router-dom';
import { Card, Form, Input, Button, Select, DatePicker, InputNumber, Space, Typography, message, Divider } from 'antd';
import { ArrowLeftOutlined } from '@ant-design/icons';
import { useCreateEmployeeMutation } from '../api/employeeApi';
import { useGetRolesQuery, useGetLocationsQuery } from '../api/settingsApi';

const { Title } = Typography;

export default function EmployeeCreatePage() {
  const navigate = useNavigate();
  const [form] = Form.useForm();
  const [createEmployee, { isLoading }] = useCreateEmployeeMutation();
  const { data: roles } = useGetRolesQuery();
  const { data: locations } = useGetLocationsQuery();

  const onFinish = async (values: Record<string, unknown>) => {
    try {
      const body = {
        ...values,
        hireDate: (values.hireDate as { toISOString: () => string })?.toISOString(),
        dateOfBirth: values.dateOfBirth ? (values.dateOfBirth as { toISOString: () => string })?.toISOString() : null,
        contract: values.hourlyRate ? {
          contractType: values.contractType || 0,
          contractedHoursPerWeek: values.contractedHoursPerWeek || 40,
          hourlyRate: values.hourlyRate,
          startDate: (values.hireDate as { toISOString: () => string })?.toISOString(),
          noticePeriodDays: 30,
        } : null,
      };
      const result = await createEmployee(body).unwrap();
      message.success('Employee created successfully');
      navigate(`/employees/${result.id}`);
    } catch {
      message.error('Failed to create employee');
    }
  };

  return (
    <>
      <Space style={{ marginBottom: 16 }}>
        <Button icon={<ArrowLeftOutlined />} onClick={() => navigate('/employees')}>Back</Button>
      </Space>

      <Card>
        <Title level={3}>Add New Employee</Title>
        <Form form={form} layout="vertical" onFinish={onFinish} style={{ maxWidth: 800 }}>
          <Space style={{ width: '100%' }} size="large" direction="vertical">
            <div>
              <Title level={5}>Personal Information</Title>
              <Space style={{ width: '100%' }} size="middle" wrap>
                <Form.Item name="firstName" label="First Name" rules={[{ required: true }]} style={{ minWidth: 200 }}>
                  <Input placeholder="John" />
                </Form.Item>
                <Form.Item name="lastName" label="Last Name" rules={[{ required: true }]} style={{ minWidth: 200 }}>
                  <Input placeholder="Smith" />
                </Form.Item>
              </Space>
              <Space style={{ width: '100%' }} size="middle" wrap>
                <Form.Item name="email" label="Email" rules={[{ required: true, type: 'email' }]} style={{ minWidth: 300 }}>
                  <Input placeholder="john@restaurant.com" />
                </Form.Item>
                <Form.Item name="phone" label="Phone" style={{ minWidth: 200 }}>
                  <Input placeholder="+41 79 123 4567" />
                </Form.Item>
              </Space>
              <Space style={{ width: '100%' }} size="middle" wrap>
                <Form.Item name="dateOfBirth" label="Date of Birth">
                  <DatePicker style={{ width: 200 }} />
                </Form.Item>
                <Form.Item name="hireDate" label="Hire Date" rules={[{ required: true }]}>
                  <DatePicker style={{ width: 200 }} />
                </Form.Item>
                <Form.Item name="locationId" label="Location">
                  <Select placeholder="Select location" style={{ width: 200 }} allowClear options={(locations || []).map(l => ({ value: l.id, label: l.name }))} />
                </Form.Item>
              </Space>
            </div>

            <Divider />

            <div>
              <Title level={5}>Contract (Optional)</Title>
              <Space style={{ width: '100%' }} size="middle" wrap>
                <Form.Item name="contractType" label="Contract Type">
                  <Select placeholder="Select type" style={{ width: 200 }} options={[
                    { value: 0, label: 'Full-Time' }, { value: 1, label: 'Part-Time' },
                    { value: 2, label: 'Hourly' }, { value: 3, label: 'Internship' }, { value: 4, label: 'Seasonal' },
                  ]} />
                </Form.Item>
                <Form.Item name="hourlyRate" label="Hourly Rate (CHF)">
                  <InputNumber min={0} precision={2} placeholder="25.00" style={{ width: 150 }} />
                </Form.Item>
                <Form.Item name="contractedHoursPerWeek" label="Hours/Week">
                  <InputNumber min={1} max={168} placeholder="40" style={{ width: 120 }} />
                </Form.Item>
              </Space>
            </div>
          </Space>

          <Form.Item style={{ marginTop: 24 }}>
            <Space>
              <Button type="primary" htmlType="submit" loading={isLoading} style={{ background: '#4F46E5' }}>Create Employee</Button>
              <Button onClick={() => navigate('/employees')}>Cancel</Button>
            </Space>
          </Form.Item>
        </Form>
      </Card>
    </>
  );
}
