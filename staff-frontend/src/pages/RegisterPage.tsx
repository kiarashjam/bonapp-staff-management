import { useState } from 'react';
import { useNavigate, Link } from 'react-router-dom';
import { Card, Form, Input, Button, Typography, Alert, Space } from 'antd';
import { MailOutlined, LockOutlined, UserOutlined, ShopOutlined } from '@ant-design/icons';
import { useRegisterMutation } from '../api/authApi';
import { useAppDispatch } from '../store/store';
import { setCredentials } from '../store/authSlice';

const { Title, Text } = Typography;

export default function RegisterPage() {
  const [register, { isLoading }] = useRegisterMutation();
  const dispatch = useAppDispatch();
  const navigate = useNavigate();
  const [error, setError] = useState('');

  const onFinish = async (values: { email: string; password: string; firstName: string; lastName: string; organizationName: string }) => {
    try {
      setError('');
      const data = await register(values).unwrap();
      dispatch(setCredentials({ user: data.user, accessToken: data.accessToken, refreshToken: data.refreshToken }));
      navigate('/dashboard');
    } catch (err: unknown) {
      const e = err as { data?: { message?: string } };
      setError(e?.data?.message || 'Registration failed');
    }
  };

  return (
    <div style={{ minHeight: '100vh', display: 'flex', alignItems: 'center', justifyContent: 'center', background: 'linear-gradient(135deg, #667eea 0%, #764ba2 100%)' }}>
      <Card style={{ width: 480, boxShadow: '0 20px 60px rgba(0,0,0,0.15)', borderRadius: 16 }}>
        <Space direction="vertical" size="large" style={{ width: '100%' }}>
          <div style={{ textAlign: 'center' }}>
            <Title level={2} style={{ color: '#4F46E5', marginBottom: 4 }}>Create Account</Title>
            <Text type="secondary">Set up your restaurant staff management</Text>
          </div>

          {error && <Alert type="error" message={error} showIcon closable onClose={() => setError('')} />}

          <Form layout="vertical" onFinish={onFinish} size="large">
            <Space style={{ width: '100%' }} size="middle">
              <Form.Item name="firstName" rules={[{ required: true }]} style={{ flex: 1 }}>
                <Input prefix={<UserOutlined />} placeholder="First name" />
              </Form.Item>
              <Form.Item name="lastName" rules={[{ required: true }]} style={{ flex: 1 }}>
                <Input prefix={<UserOutlined />} placeholder="Last name" />
              </Form.Item>
            </Space>
            <Form.Item name="organizationName" rules={[{ required: true }]}>
              <Input prefix={<ShopOutlined />} placeholder="Restaurant / Organization name" />
            </Form.Item>
            <Form.Item name="email" rules={[{ required: true, type: 'email' }]}>
              <Input prefix={<MailOutlined />} placeholder="Email address" />
            </Form.Item>
            <Form.Item name="password" rules={[{ required: true, min: 8, message: 'Min 8 characters' }]}>
              <Input.Password prefix={<LockOutlined />} placeholder="Password (8+ characters)" />
            </Form.Item>
            <Form.Item>
              <Button type="primary" htmlType="submit" block loading={isLoading} style={{ height: 44, borderRadius: 8, background: '#4F46E5' }}>
                Create Account
              </Button>
            </Form.Item>
          </Form>

          <Text style={{ textAlign: 'center', display: 'block' }}>
            Already have an account? <Link to="/login">Sign in</Link>
          </Text>
        </Space>
      </Card>
    </div>
  );
}
