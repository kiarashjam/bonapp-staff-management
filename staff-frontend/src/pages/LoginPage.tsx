import { useState } from 'react';
import { useNavigate, Link } from 'react-router-dom';
import { Card, Form, Input, Button, Typography, Alert, Space } from 'antd';
import { MailOutlined, LockOutlined } from '@ant-design/icons';
import { useLoginMutation } from '../api/authApi';
import { useAppDispatch } from '../store/store';
import { setCredentials } from '../store/authSlice';

const { Title, Text } = Typography;

export default function LoginPage() {
  const [login, { isLoading }] = useLoginMutation();
  const dispatch = useAppDispatch();
  const navigate = useNavigate();
  const [error, setError] = useState('');

  const onFinish = async (values: { email: string; password: string }) => {
    try {
      setError('');
      const data = await login(values).unwrap();
      dispatch(setCredentials({ user: data.user, accessToken: data.accessToken, refreshToken: data.refreshToken }));
      navigate('/dashboard');
    } catch (err: unknown) {
      const e = err as { data?: { message?: string } };
      setError(e?.data?.message || 'Login failed. Please check your credentials.');
    }
  };

  return (
    <div style={{ minHeight: '100vh', display: 'flex', alignItems: 'center', justifyContent: 'center', background: 'linear-gradient(135deg, #667eea 0%, #764ba2 100%)' }}>
      <Card style={{ width: 420, boxShadow: '0 20px 60px rgba(0,0,0,0.15)', borderRadius: 16 }}>
        <Space direction="vertical" size="large" style={{ width: '100%' }}>
          <div style={{ textAlign: 'center' }}>
            <Title level={2} style={{ color: '#4F46E5', marginBottom: 4 }}>Staff Pro</Title>
            <Text type="secondary">Sign in to manage your team</Text>
          </div>

          {error && <Alert type="error" message={error} showIcon closable onClose={() => setError('')} />}

          <Form layout="vertical" onFinish={onFinish} size="large">
            <Form.Item name="email" rules={[{ required: true, type: 'email', message: 'Valid email required' }]}>
              <Input prefix={<MailOutlined />} placeholder="Email address" />
            </Form.Item>
            <Form.Item name="password" rules={[{ required: true, message: 'Password required' }]}>
              <Input.Password prefix={<LockOutlined />} placeholder="Password" />
            </Form.Item>
            <Form.Item>
              <Button type="primary" htmlType="submit" block loading={isLoading} style={{ height: 44, borderRadius: 8, background: '#4F46E5' }}>
                Sign In
              </Button>
            </Form.Item>
          </Form>

          <Text style={{ textAlign: 'center', display: 'block' }}>
            Don't have an account? <Link to="/register">Create one</Link>
          </Text>
        </Space>
      </Card>
    </div>
  );
}
