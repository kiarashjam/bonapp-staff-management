import { Card, Button, Typography, Space, Tag, Statistic, Row, Col, List } from 'antd';
import { LoginOutlined, LogoutOutlined, CoffeeOutlined } from '@ant-design/icons';
import { useGetClockStatusQuery, useClockActionMutation } from '../api/clockApi';
import { message } from 'antd';

const { Title, Text } = Typography;

export default function ClockPage() {
  const { data: status, isLoading } = useGetClockStatusQuery();
  const [clockAction, { isLoading: acting }] = useClockActionMutation();

  const handleClock = async (entryType: string) => {
    try {
      // Try to get GPS location
      let lat: number | undefined, lng: number | undefined, accuracy: number | undefined;
      try {
        const pos = await new Promise<GeolocationPosition>((resolve, reject) =>
          navigator.geolocation.getCurrentPosition(resolve, reject, { timeout: 5000 }));
        lat = pos.coords.latitude;
        lng = pos.coords.longitude;
        accuracy = pos.coords.accuracy;
      } catch { /* GPS not available, proceed without */ }

      await clockAction({ entryType, latitude: lat, longitude: lng, gpsAccuracyMeters: accuracy }).unwrap();
      const labels: Record<string, string> = { ClockIn: 'Clocked in', ClockOut: 'Clocked out', BreakStart: 'Break started', BreakEnd: 'Break ended' };
      message.success(labels[entryType] || 'Done');
    } catch {
      message.error('Clock action failed');
    }
  };

  const isClockedIn = status?.isClockedIn || false;
  const isOnBreak = status?.isOnBreak || false;

  return (
    <>
      <Title level={3}>Clock In / Out</Title>

      <Row gutter={[24, 24]} justify="center">
        <Col xs={24} sm={16} md={12}>
          <Card loading={isLoading} style={{ textAlign: 'center' }}>
            <Space direction="vertical" size="large" style={{ width: '100%' }}>
              <div>
                <Tag color={isClockedIn ? 'green' : 'default'} style={{ fontSize: 18, padding: '8px 24px' }}>
                  {isClockedIn ? (isOnBreak ? 'ON BREAK' : 'CLOCKED IN') : 'CLOCKED OUT'}
                </Tag>
              </div>

              {status?.workedSoFar && (
                <Statistic title="Worked Today" value={status.workedSoFar} />
              )}

              {status?.currentShiftInfo && (
                <Text type="secondary">Current shift: {status.currentShiftInfo}</Text>
              )}

              <Space size="large" wrap style={{ justifyContent: 'center' }}>
                {!isClockedIn && (
                  <Button type="primary" size="large" icon={<LoginOutlined />} loading={acting}
                    onClick={() => handleClock('ClockIn')}
                    style={{ height: 80, width: 200, fontSize: 18, background: '#52c41a', borderColor: '#52c41a', borderRadius: 12 }}>
                    Clock In
                  </Button>
                )}

                {isClockedIn && !isOnBreak && (
                  <>
                    <Button size="large" icon={<CoffeeOutlined />} loading={acting}
                      onClick={() => handleClock('BreakStart')}
                      style={{ height: 80, width: 160, fontSize: 16, borderRadius: 12 }}>
                      Start Break
                    </Button>
                    <Button type="primary" danger size="large" icon={<LogoutOutlined />} loading={acting}
                      onClick={() => handleClock('ClockOut')}
                      style={{ height: 80, width: 160, fontSize: 16, borderRadius: 12 }}>
                      Clock Out
                    </Button>
                  </>
                )}

                {isClockedIn && isOnBreak && (
                  <Button type="primary" size="large" icon={<CoffeeOutlined />} loading={acting}
                    onClick={() => handleClock('BreakEnd')}
                    style={{ height: 80, width: 200, fontSize: 18, borderRadius: 12, background: '#faad14', borderColor: '#faad14' }}>
                    End Break
                  </Button>
                )}
              </Space>

              {status?.lastClockIn && (
                <Text type="secondary">Last clock-in: {new Date(status.lastClockIn).toLocaleString()}</Text>
              )}
            </Space>
          </Card>
        </Col>
      </Row>
    </>
  );
}
