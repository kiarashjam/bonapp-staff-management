import { useState } from 'react';
import { Table, Button, DatePicker, Space, Typography, Card, Statistic, Row, Col } from 'antd';
import { DownloadOutlined, DollarOutlined } from '@ant-design/icons';
import { useGetPayrollSummaryQuery } from '../api/clockApi';
import type { PayrollExport } from '../types';
import dayjs from 'dayjs';

const { Title } = Typography;
const { RangePicker } = DatePicker;

export default function PayrollPage() {
  const [dates, setDates] = useState<[string, string]>([
    dayjs().startOf('month').format('YYYY-MM-DD'),
    dayjs().endOf('month').format('YYYY-MM-DD'),
  ]);
  const { data, isLoading } = useGetPayrollSummaryQuery({ periodStart: dates[0], periodEnd: dates[1] });

  const totalGross = (data || []).reduce((sum, r) => sum + r.totalGrossPay, 0);
  const totalHours = (data || []).reduce((sum, r) => sum + r.regularHours + r.overtimeHours, 0);

  const columns = [
    { title: 'Employee', dataIndex: 'employeeName', key: 'name' },
    { title: 'Contract', dataIndex: 'contractType', key: 'contract', render: (v: string) => v || '-' },
    { title: 'Rate', dataIndex: 'hourlyRate', key: 'rate', render: (v: number) => `CHF ${v.toFixed(2)}/h` },
    { title: 'Regular', dataIndex: 'regularHours', key: 'regular', render: (v: number) => `${v.toFixed(1)}h` },
    { title: 'OT', dataIndex: 'overtimeHours', key: 'ot', render: (v: number) => `${v.toFixed(1)}h` },
    { title: 'Regular Pay', dataIndex: 'regularPay', key: 'regPay', render: (v: number) => `CHF ${v.toFixed(2)}` },
    { title: 'OT Pay', dataIndex: 'overtimePay', key: 'otPay', render: (v: number) => `CHF ${v.toFixed(2)}` },
    { title: 'Total Gross', dataIndex: 'totalGrossPay', key: 'total', render: (v: number) => <strong>CHF {v.toFixed(2)}</strong> },
  ];

  const handleExport = () => {
    if (!data) return;
    const headers = ['Employee', 'Email', 'Contract', 'Rate', 'Regular Hours', 'OT Hours', 'Regular Pay', 'OT Pay', 'Total Gross'];
    const rows = data.map(r => [r.employeeName, r.employeeEmail || '', r.contractType || '', r.hourlyRate, r.regularHours, r.overtimeHours, r.regularPay, r.overtimePay, r.totalGrossPay]);
    const csv = [headers.join(','), ...rows.map(r => r.join(','))].join('\n');
    const blob = new Blob([csv], { type: 'text/csv' });
    const url = URL.createObjectURL(blob);
    const a = document.createElement('a');
    a.href = url;
    a.download = `payroll_${dates[0]}_${dates[1]}.csv`;
    a.click();
  };

  return (
    <>
      <div style={{ display: 'flex', justifyContent: 'space-between', marginBottom: 16 }}>
        <Title level={3} style={{ margin: 0 }}>Payroll Report</Title>
        <Space>
          <RangePicker
            value={[dayjs(dates[0]), dayjs(dates[1])]}
            onChange={(d) => d && setDates([d[0]!.format('YYYY-MM-DD'), d[1]!.format('YYYY-MM-DD')])}
          />
          <Button icon={<DownloadOutlined />} onClick={handleExport} disabled={!data?.length}>Export CSV</Button>
        </Space>
      </div>

      <Row gutter={[16, 16]} style={{ marginBottom: 16 }}>
        <Col xs={12} sm={8}><Card><Statistic title="Total Employees" value={data?.length || 0} /></Card></Col>
        <Col xs={12} sm={8}><Card><Statistic title="Total Hours" value={totalHours} precision={1} suffix="h" /></Card></Col>
        <Col xs={12} sm={8}><Card><Statistic title="Total Gross Pay" value={totalGross} precision={2} prefix={<DollarOutlined />} suffix="CHF" /></Card></Col>
      </Row>

      <Card>
        <Table columns={columns} dataSource={data || []} rowKey="employeeId" loading={isLoading} pagination={false} />
      </Card>
    </>
  );
}
