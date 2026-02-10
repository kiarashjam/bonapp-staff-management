import { useState } from 'react';
import { Card, Button, Typography, Space, Tag, List, Avatar, Modal, Form, Input, Switch, message } from 'antd';
import { PlusOutlined, PushpinOutlined, SoundOutlined, CheckCircleOutlined } from '@ant-design/icons';
import { useGetAnnouncementsQuery, useCreateAnnouncementMutation } from '../api/settingsApi';
import { useAppSelector } from '../store/store';
import type { Announcement } from '../types';

const { Title, Text, Paragraph } = Typography;

export default function AnnouncementsPage() {
  const { user } = useAppSelector((s) => s.auth);
  const isManager = user?.role === 'Admin' || user?.role === 'Manager' || user?.role === 'SuperAdmin';
  const [showCreate, setShowCreate] = useState(false);
  const { data, isLoading } = useGetAnnouncementsQuery({ page: 1, pageSize: 50 });
  const [createAnnouncement, { isLoading: creating }] = useCreateAnnouncementMutation();
  const [form] = Form.useForm();

  const handleCreate = async (values: Record<string, unknown>) => {
    try {
      await createAnnouncement({ ...values, targetType: 'All' }).unwrap();
      message.success('Announcement posted');
      setShowCreate(false);
      form.resetFields();
    } catch {
      message.error('Failed to post');
    }
  };

  return (
    <>
      <div style={{ display: 'flex', justifyContent: 'space-between', marginBottom: 16 }}>
        <Title level={3} style={{ margin: 0 }}>Announcements</Title>
        {isManager && <Button type="primary" icon={<PlusOutlined />} onClick={() => setShowCreate(true)}>New Announcement</Button>}
      </div>

      <List
        loading={isLoading}
        dataSource={data?.items || []}
        locale={{ emptyText: 'No announcements yet' }}
        renderItem={(item: Announcement) => (
          <Card style={{ marginBottom: 12 }}>
            <Space direction="vertical" style={{ width: '100%' }}>
              <div style={{ display: 'flex', justifyContent: 'space-between' }}>
                <Space>
                  <Avatar style={{ backgroundColor: '#4F46E5' }} icon={<SoundOutlined />} />
                  <div>
                    <Text strong style={{ fontSize: 16 }}>{item.title}</Text>
                    {item.isPinned && <Tag icon={<PushpinOutlined />} color="orange" style={{ marginLeft: 8 }}>Pinned</Tag>}
                  </div>
                </Space>
                <Space>
                  <Tag icon={<CheckCircleOutlined />} color="green">{item.readCount} read</Tag>
                  <Text type="secondary">{new Date(item.createdAt).toLocaleDateString()}</Text>
                </Space>
              </div>
              <Paragraph style={{ marginBottom: 0, marginLeft: 48 }}>{item.content}</Paragraph>
              <Text type="secondary" style={{ marginLeft: 48 }}>Posted by {item.postedBy}</Text>
            </Space>
          </Card>
        )}
      />

      <Modal title="Post Announcement" open={showCreate} onCancel={() => setShowCreate(false)} footer={null}>
        <Form form={form} layout="vertical" onFinish={handleCreate}>
          <Form.Item name="title" label="Title" rules={[{ required: true }]}>
            <Input placeholder="Important update..." />
          </Form.Item>
          <Form.Item name="content" label="Message" rules={[{ required: true }]}>
            <Input.TextArea rows={4} placeholder="Write your announcement..." />
          </Form.Item>
          <Form.Item name="isPinned" label="Pin to top" valuePropName="checked" initialValue={false}>
            <Switch />
          </Form.Item>
          <Space>
            <Button type="primary" htmlType="submit" loading={creating}>Post Announcement</Button>
            <Button onClick={() => setShowCreate(false)}>Cancel</Button>
          </Space>
        </Form>
      </Modal>
    </>
  );
}
