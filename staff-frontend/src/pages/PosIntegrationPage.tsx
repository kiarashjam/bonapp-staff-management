import { Typography, Card, Steps, Alert, Descriptions, Tag, Divider, Space, Button, Tabs, Table, Collapse } from 'antd';
import {
  ApiOutlined, KeyOutlined, SyncOutlined, CheckCircleOutlined,
  CloudServerOutlined, SafetyCertificateOutlined, CodeOutlined,
  RocketOutlined, LinkOutlined, BellOutlined, CopyOutlined,
  ShoppingCartOutlined, ClockCircleOutlined, TeamOutlined,
} from '@ant-design/icons';
import { useState } from 'react';

const { Title, Text, Paragraph } = Typography;

const copyToClipboard = (text: string) => {
  navigator.clipboard.writeText(text);
};

function CodeBlock({ code, language }: { code: string; language?: string }) {
  return (
    <div style={{ position: 'relative' }}>
      <Button
        type="text"
        size="small"
        icon={<CopyOutlined />}
        onClick={() => copyToClipboard(code)}
        style={{ position: 'absolute', top: 8, right: 8, zIndex: 1, color: '#999' }}
      />
      <pre style={{
        background: '#1e1e2e', color: '#cdd6f4', padding: '16px 20px',
        borderRadius: 8, overflow: 'auto', fontSize: 13, lineHeight: 1.6,
        fontFamily: "'Fira Code', 'Cascadia Code', Consolas, monospace",
      }}>
        <code>{code}</code>
      </pre>
    </div>
  );
}

export default function PosIntegrationPage() {
  const [currentStep, setCurrentStep] = useState(0);

  const endpointColumns = [
    { title: 'Method', dataIndex: 'method', key: 'method', render: (v: string) => <Tag color={v === 'POST' ? 'blue' : v === 'GET' ? 'green' : 'orange'}>{v}</Tag> },
    { title: 'Endpoint', dataIndex: 'endpoint', key: 'endpoint', render: (v: string) => <code style={{ fontSize: 13 }}>{v}</code> },
    { title: 'Description', dataIndex: 'description', key: 'description' },
    { title: 'Phase', dataIndex: 'phase', key: 'phase', render: (v: string) => <Tag>{v}</Tag> },
  ];

  const endpoints = [
    { method: 'POST', endpoint: '/api/pos/v1/clock', description: 'Send clock-in/out events from POS terminal login', phase: 'Phase 1' },
    { method: 'POST', endpoint: '/api/pos/v1/sales-summary', description: 'Send hourly/daily sales data for labor optimization', phase: 'Phase 2' },
    { method: 'GET', endpoint: '/api/pos/v1/schedule/today', description: 'Get today\'s schedule (who is working)', phase: 'Phase 1' },
    { method: 'GET', endpoint: '/api/pos/v1/employees', description: 'Get active employee list with roles', phase: 'Phase 1' },
    { method: 'POST', endpoint: '/api/pos/v1/tips', description: 'Send tip data from POS for automatic distribution', phase: 'Phase 3' },
  ];

  const webhookEvents = [
    { event: 'schedule.published', description: 'Triggered when a manager publishes a new schedule', payload: 'Schedule period with all shift assignments' },
    { event: 'employee.role_changed', description: 'Employee role was updated (e.g., promoted)', payload: 'Employee ID, old role, new role' },
    { event: 'employee.terminated', description: 'Employee was terminated — POS should revoke access', payload: 'Employee ID, termination date' },
    { event: 'shift_swap.approved', description: 'A shift swap was approved between two employees', payload: 'Both employee IDs, shift details' },
  ];

  return (
    <div style={{ maxWidth: 1000, margin: '0 auto' }}>
      <Space direction="vertical" size="large" style={{ width: '100%' }}>

        {/* Header */}
        <div>
          <Title level={2} style={{ marginBottom: 4 }}>
            <ApiOutlined style={{ marginRight: 12, color: '#4F46E5' }} />
            BonApp POS Integration Guide
          </Title>
          <Paragraph type="secondary" style={{ fontSize: 16, maxWidth: 700 }}>
            Step-by-step guide to connect Staff Pro with the BonApp POS system.
            Staff Pro uses an API-first integration model — BonApp connects as an External POS Connection,
            just like any other POS system.
          </Paragraph>
        </div>

        {/* Architecture Overview */}
        <Alert
          message="Integration Principle"
          description={
            <Space direction="vertical">
              <Text>Staff Pro and BonApp POS are <strong>completely independent systems</strong>. They have:</Text>
              <ul style={{ margin: '4px 0' }}>
                <li><strong>Separate databases</strong> — No shared tables, no shared schema</li>
                <li><strong>Separate authentication</strong> — Each has its own user accounts and JWT tokens</li>
                <li><strong>Separate deployments</strong> — Different Docker containers, different servers</li>
                <li><strong>API-only communication</strong> — All data flows through REST API calls + webhooks</li>
              </ul>
              <Text type="secondary">This means BonApp POS connects to Staff Pro the same way any POS system (Lightspeed, Toast, Square, etc.) would connect.</Text>
            </Space>
          }
          type="info"
          showIcon
          icon={<SafetyCertificateOutlined />}
        />

        {/* Step-by-Step Integration */}
        <Card>
          <Title level={3}>Integration Steps</Title>

          <Steps
            current={currentStep}
            onChange={setCurrentStep}
            direction="vertical"
            items={[
              {
                title: 'Step 1 — Register BonApp as a POS Connection',
                icon: <KeyOutlined />,
                description: 'Create an API key for BonApp in Staff Pro',
              },
              {
                title: 'Step 2 — Configure API Authentication',
                icon: <SafetyCertificateOutlined />,
                description: 'Set up the API key header for all requests',
              },
              {
                title: 'Step 3 — Sync Employee Data',
                icon: <TeamOutlined />,
                description: 'Read employee & role data from Staff Pro',
              },
              {
                title: 'Step 4 — Send Clock Events from POS',
                icon: <ClockCircleOutlined />,
                description: 'When staff logs into POS terminal, send clock-in event',
              },
              {
                title: 'Step 5 — Get Today\'s Schedule',
                icon: <SyncOutlined />,
                description: 'Pull the current schedule to know who should be working',
              },
              {
                title: 'Step 6 — Register Webhook Listener',
                icon: <BellOutlined />,
                description: 'Receive real-time notifications when schedules change',
              },
              {
                title: 'Step 7 — Send Sales Data (Optional)',
                icon: <ShoppingCartOutlined />,
                description: 'Send sales data for AI-powered labor optimization',
              },
              {
                title: 'Step 8 — Go Live',
                icon: <RocketOutlined />,
                description: 'Test, verify, and enable production integration',
              },
            ]}
          />
        </Card>

        {/* Step Details */}
        {currentStep === 0 && (
          <Card title={<Space><KeyOutlined /> Step 1 — Register BonApp as a POS Connection</Space>}>
            <Paragraph>
              In the Staff Pro admin panel, navigate to <strong>Settings &rarr; POS Connections</strong> and create a new connection.
            </Paragraph>
            <Descriptions bordered column={1} size="small" style={{ marginBottom: 16 }}>
              <Descriptions.Item label="Connection Name">BonApp POS</Descriptions.Item>
              <Descriptions.Item label="Provider Type">Custom / BonApp</Descriptions.Item>
              <Descriptions.Item label="Webhook URL">https://your-bonapp-server.com/api/webhooks/staffpro</Descriptions.Item>
              <Descriptions.Item label="Result">You receive an API Key (e.g., <code>sp_pos_a1b2c3d4e5f6...</code>)</Descriptions.Item>
            </Descriptions>
            <Alert
              message="Store this API key securely. It is shown only once. Store it in BonApp's environment variables or secrets manager."
              type="warning"
              showIcon
            />
            <Divider />
            <Text strong>BonApp Backend — Store the API key:</Text>
            <CodeBlock code={`// In BonApp's appsettings.json or environment variables:
{
  "StaffPro": {
    "BaseUrl": "https://staffpro.yourdomain.com",
    "ApiKey": "sp_pos_a1b2c3d4e5f6...",
    "WebhookSecret": "whsec_..."
  }
}`} />
          </Card>
        )}

        {currentStep === 1 && (
          <Card title={<Space><SafetyCertificateOutlined /> Step 2 — Configure API Authentication</Space>}>
            <Paragraph>
              All requests from BonApp to Staff Pro must include the API key in the <code>X-POS-Api-Key</code> header.
            </Paragraph>
            <CodeBlock code={`// C# HttpClient setup in BonApp backend:

public class StaffProClient
{
    private readonly HttpClient _http;
    private readonly string _baseUrl;

    public StaffProClient(IConfiguration config)
    {
        _http = new HttpClient();
        _baseUrl = config["StaffPro:BaseUrl"];
        _http.DefaultRequestHeaders.Add(
            "X-POS-Api-Key", config["StaffPro:ApiKey"]);
        _http.DefaultRequestHeaders.Add(
            "Content-Type", "application/json");
    }

    // All subsequent API calls use this _http client
    // The API key is sent automatically with every request
}`} />
            <Divider />
            <Alert
              message="Authentication Flow"
              description={
                <ol style={{ margin: 0, paddingLeft: 20 }}>
                  <li>BonApp sends request with <code>X-POS-Api-Key</code> header</li>
                  <li>Staff Pro validates the API key against registered POS connections</li>
                  <li>Staff Pro identifies the Organization from the API key</li>
                  <li>Request is processed within that organization's data scope</li>
                  <li>Response returned (or 401 Unauthorized if key is invalid)</li>
                </ol>
              }
              type="info"
              showIcon
            />
          </Card>
        )}

        {currentStep === 2 && (
          <Card title={<Space><TeamOutlined /> Step 3 — Sync Employee Data</Space>}>
            <Paragraph>
              BonApp can read the active employee list from Staff Pro to match POS terminal logins with Staff Pro employee records.
            </Paragraph>
            <CodeBlock code={`// GET /api/pos/v1/employees
// Returns all active employees with their roles

GET https://staffpro.yourdomain.com/api/pos/v1/employees
X-POS-Api-Key: sp_pos_a1b2c3d4e5f6...

// Response:
{
  "employees": [
    {
      "id": "emp_abc123",
      "firstName": "Maria",
      "lastName": "Garcia",
      "email": "maria@restaurant.com",
      "roles": [
        { "name": "Head Waitress", "isPrimary": true },
        { "name": "Bartender", "isPrimary": false }
      ],
      "locationId": "loc_xyz",
      "locationName": "Downtown Branch",
      "status": "Active"
    }
    // ... more employees
  ]
}`} />
            <Divider />
            <Text strong>How BonApp Should Use This:</Text>
            <ul>
              <li>Call this endpoint <strong>once daily</strong> (or on demand) to refresh the local employee cache</li>
              <li>Map BonApp POS user accounts to Staff Pro employee IDs using <strong>email address</strong> as the matching key</li>
              <li>Store the Staff Pro <code>employee.id</code> alongside the BonApp user record for future API calls</li>
            </ul>
          </Card>
        )}

        {currentStep === 3 && (
          <Card title={<Space><ClockCircleOutlined /> Step 4 — Send Clock Events from POS</Space>}>
            <Paragraph>
              When a staff member <strong>logs into the BonApp POS terminal</strong>, send a clock-in event to Staff Pro.
              When they <strong>log out</strong>, send a clock-out event. This keeps time tracking in sync automatically.
            </Paragraph>
            <CodeBlock code={`// POST /api/pos/v1/clock
// Send clock-in/out events from POS terminal

POST https://staffpro.yourdomain.com/api/pos/v1/clock
X-POS-Api-Key: sp_pos_a1b2c3d4e5f6...
Content-Type: application/json

{
  "employeeId": "emp_abc123",
  "entryType": "ClockIn",        // ClockIn | ClockOut | BreakStart | BreakEnd
  "timestamp": "2026-02-09T08:02:00Z",
  "source": "BonAppPOS",
  "terminalId": "terminal-01",    // Optional: which POS terminal
  "locationId": "loc_xyz"         // Optional: which location
}

// Response (201 Created):
{
  "id": "clk_789",
  "employeeId": "emp_abc123",
  "entryType": "ClockIn",
  "timestamp": "2026-02-09T08:02:00Z",
  "roundedTimestamp": "2026-02-09T08:00:00Z",
  "source": "BonAppPOS",
  "status": "accepted"
}`} />
            <Divider />
            <Text strong>When to trigger in BonApp:</Text>
            <Descriptions bordered column={1} size="small">
              <Descriptions.Item label={<Tag color="green">ClockIn</Tag>}>When staff logs into the POS terminal at the start of their shift</Descriptions.Item>
              <Descriptions.Item label={<Tag color="red">ClockOut</Tag>}>When staff logs out of the POS terminal at the end of their shift</Descriptions.Item>
              <Descriptions.Item label={<Tag color="orange">BreakStart</Tag>}>When staff starts a break (if tracked in BonApp)</Descriptions.Item>
              <Descriptions.Item label={<Tag color="blue">BreakEnd</Tag>}>When staff ends a break and resumes work</Descriptions.Item>
            </Descriptions>
            <Alert
              message="Staff Pro will automatically round timestamps according to the organization's rounding rules (e.g., round to nearest 5 minutes) and apply grace period logic."
              type="info"
              showIcon
              style={{ marginTop: 16 }}
            />
          </Card>
        )}

        {currentStep === 4 && (
          <Card title={<Space><SyncOutlined /> Step 5 — Get Today's Schedule</Space>}>
            <Paragraph>
              BonApp can pull the current day's schedule to display who is working, what roles they're assigned to, and their shift times.
              This helps the POS show relevant staff info on the terminal.
            </Paragraph>
            <CodeBlock code={`// GET /api/pos/v1/schedule/today?locationId=loc_xyz
// Returns today's shift assignments for a location

GET https://staffpro.yourdomain.com/api/pos/v1/schedule/today?locationId=loc_xyz
X-POS-Api-Key: sp_pos_a1b2c3d4e5f6...

// Response:
{
  "date": "2026-02-09",
  "locationId": "loc_xyz",
  "locationName": "Downtown Branch",
  "shifts": [
    {
      "employeeId": "emp_abc123",
      "employeeName": "Maria Garcia",
      "role": "Head Waitress",
      "station": "Section A",
      "startTime": "08:00",
      "endTime": "16:00",
      "breakMinutes": 30,
      "status": "Scheduled"
    },
    {
      "employeeId": "emp_def456",
      "employeeName": "Tom Wilson",
      "role": "Bartender",
      "station": "Main Bar",
      "startTime": "16:00",
      "endTime": "00:00",
      "breakMinutes": 30,
      "status": "Scheduled"
    }
  ]
}`} />
            <Divider />
            <Text strong>Use cases in BonApp:</Text>
            <ul>
              <li>Display today's roster on the POS manager dashboard</li>
              <li>Auto-assign the correct role/permissions when staff logs into the POS</li>
              <li>Show warnings if someone clocks in who isn't scheduled</li>
              <li>Cache this data and refresh every 15 minutes or on demand</li>
            </ul>
          </Card>
        )}

        {currentStep === 5 && (
          <Card title={<Space><BellOutlined /> Step 6 — Register Webhook Listener</Space>}>
            <Paragraph>
              Staff Pro sends real-time webhook notifications to BonApp when important events happen
              (schedule published, employee terminated, etc.). BonApp needs an endpoint to receive these.
            </Paragraph>
            <CodeBlock code={`// BonApp needs to create a webhook endpoint:
// POST https://your-bonapp-server.com/api/webhooks/staffpro

// Staff Pro sends HMAC-SHA256 signed payloads:
// Header: X-StaffPro-Signature: sha256=abc123...

// Example payload for "schedule.published":
{
  "event": "schedule.published",
  "timestamp": "2026-02-09T10:30:00Z",
  "organizationId": "org_xyz",
  "data": {
    "schedulePeriodId": "sp_123",
    "locationId": "loc_xyz",
    "locationName": "Downtown Branch",
    "startDate": "2026-02-10",
    "endDate": "2026-02-16",
    "totalShifts": 42,
    "affectedEmployees": ["emp_abc123", "emp_def456", ...]
  }
}

// BonApp webhook handler (C#):
[HttpPost("api/webhooks/staffpro")]
public async Task<IActionResult> HandleStaffProWebhook(
    [FromBody] JsonElement payload,
    [FromHeader(Name = "X-StaffPro-Signature")] string signature)
{
    // 1. Verify HMAC-SHA256 signature
    var secret = _config["StaffPro:WebhookSecret"];
    if (!VerifySignature(payload, signature, secret))
        return Unauthorized();

    // 2. Process the event
    var eventType = payload.GetProperty("event").GetString();
    switch (eventType)
    {
        case "schedule.published":
            await RefreshScheduleCache();
            break;
        case "employee.terminated":
            var empId = payload.GetProperty("data")
                .GetProperty("employeeId").GetString();
            await RevokePosAccess(empId);
            break;
    }

    return Ok();
}`} />
            <Divider />
            <Text strong>Available Webhook Events:</Text>
            <Table
              columns={[
                { title: 'Event', dataIndex: 'event', key: 'event', render: (v: string) => <code>{v}</code> },
                { title: 'Description', dataIndex: 'description', key: 'description' },
                { title: 'Payload', dataIndex: 'payload', key: 'payload' },
              ]}
              dataSource={webhookEvents}
              rowKey="event"
              pagination={false}
              size="small"
            />
          </Card>
        )}

        {currentStep === 6 && (
          <Card title={<Space><ShoppingCartOutlined /> Step 7 — Send Sales Data (Optional)</Space>}>
            <Paragraph>
              To enable <strong>AI-powered labor optimization</strong>, BonApp can send hourly sales data to Staff Pro.
              Staff Pro uses this to calculate labor cost percentages and recommend optimal staffing levels.
            </Paragraph>
            <CodeBlock code={`// POST /api/pos/v1/sales-summary
// Send hourly sales data for labor optimization

POST https://staffpro.yourdomain.com/api/pos/v1/sales-summary
X-POS-Api-Key: sp_pos_a1b2c3d4e5f6...
Content-Type: application/json

{
  "locationId": "loc_xyz",
  "date": "2026-02-09",
  "currency": "CHF",
  "hourlySales": [
    { "hour": 8,  "revenue": 450.00,  "covers": 12, "transactions": 15 },
    { "hour": 9,  "revenue": 680.50,  "covers": 18, "transactions": 22 },
    { "hour": 10, "revenue": 520.00,  "covers": 14, "transactions": 17 },
    { "hour": 11, "revenue": 1250.00, "covers": 35, "transactions": 40 },
    { "hour": 12, "revenue": 2100.00, "covers": 58, "transactions": 65 },
    { "hour": 13, "revenue": 1800.00, "covers": 50, "transactions": 55 }
    // ... rest of the day
  ],
  "dailyTotal": {
    "revenue": 12500.00,
    "covers": 340,
    "transactions": 380
  }
}

// Response (201 Created):
{
  "status": "accepted",
  "laborMetrics": {
    "splh": 52.08,        // Sales Per Labor Hour
    "laborCostPercent": 28.5,
    "recommendation": "Staffing levels are optimal for current sales volume"
  }
}`} />
            <Divider />
            <Text strong>Recommended schedule:</Text>
            <ul>
              <li>Send data <strong>every hour</strong> during operating hours (real-time SPLH tracking)</li>
              <li>Send daily summary at <strong>end of business day</strong> (for forecasting)</li>
              <li>Minimum <strong>4 weeks of data</strong> needed before AI forecasting activates</li>
            </ul>
            <Alert
              message="This step is optional. Staff Pro works perfectly without sales data — it just won't have AI labor recommendations."
              type="info"
              showIcon
              style={{ marginTop: 16 }}
            />
          </Card>
        )}

        {currentStep === 7 && (
          <Card title={<Space><RocketOutlined /> Step 8 — Go Live</Space>}>
            <Title level={5}>Pre-Launch Checklist</Title>
            <Collapse
              items={[
                {
                  key: '1',
                  label: '1. Test API connectivity',
                  children: (
                    <div>
                      <Paragraph>Make a test call to verify the API key works:</Paragraph>
                      <CodeBlock code={`curl -X GET https://staffpro.yourdomain.com/api/pos/v1/employees \\
  -H "X-POS-Api-Key: sp_pos_a1b2c3d4e5f6..."

# Should return 200 OK with employee list
# If 401, check your API key`} />
                    </div>
                  ),
                },
                {
                  key: '2',
                  label: '2. Verify employee mapping',
                  children: <Paragraph>Ensure every BonApp POS user has a matching Staff Pro employee (matched by email). Employees that exist in BonApp but not in Staff Pro won't have their clock events tracked.</Paragraph>,
                },
                {
                  key: '3',
                  label: '3. Test clock-in/out flow end-to-end',
                  children: (
                    <div>
                      <Paragraph>Have a test employee log into BonApp POS, verify the clock-in event appears in Staff Pro's time tracking. Then log out and verify the clock-out event.</Paragraph>
                      <Alert message="Check that timestamps match and rounding rules are applied correctly." type="info" showIcon />
                    </div>
                  ),
                },
                {
                  key: '4',
                  label: '4. Test webhook delivery',
                  children: <Paragraph>Publish a test schedule in Staff Pro and verify BonApp receives the <code>schedule.published</code> webhook. Check the HMAC signature verification works correctly.</Paragraph>,
                },
                {
                  key: '5',
                  label: '5. Configure error handling & retries',
                  children: (
                    <ul>
                      <li>If Staff Pro is unreachable, BonApp should <strong>queue clock events locally</strong> and retry</li>
                      <li>Implement <strong>exponential backoff</strong> (1s, 2s, 4s, 8s...) for failed API calls</li>
                      <li>Set a <strong>5-second timeout</strong> for API calls so POS terminal isn't blocked</li>
                      <li>Log all failed requests for manual reconciliation</li>
                    </ul>
                  ),
                },
                {
                  key: '6',
                  label: '6. Enable in production',
                  children: <Paragraph>Once all tests pass, update the BonApp production environment with the production Staff Pro API key. Monitor the first few days for any sync issues.</Paragraph>,
                },
              ]}
              defaultActiveKey={['1', '2', '3', '4', '5', '6']}
            />
          </Card>
        )}

        {/* API Reference Summary */}
        <Card title={<Space><CodeOutlined /> API Endpoint Reference</Space>}>
          <Table
            columns={endpointColumns}
            dataSource={endpoints}
            rowKey="endpoint"
            pagination={false}
            size="small"
          />
        </Card>

        {/* Data Flow Diagram */}
        <Card title={<Space><CloudServerOutlined /> Data Flow Overview</Space>}>
          <div style={{
            background: '#f8f9fa', borderRadius: 12, padding: 24,
            fontFamily: 'monospace', fontSize: 14, lineHeight: 2,
            overflow: 'auto',
          }}>
            <pre style={{ margin: 0 }}>{`
┌─────────────────┐                           ┌─────────────────┐
│                 │    API Key Auth            │                 │
│   BonApp POS    │ ─────────────────────────> │   Staff Pro     │
│                 │                            │                 │
│  ┌───────────┐  │  POST /pos/v1/clock        │  ┌───────────┐  │
│  │ Terminal  │──│──(clock-in/out events)────> │──│ Time Track│  │
│  └───────────┘  │                            │  └───────────┘  │
│                 │  POST /pos/v1/sales-summary │                 │
│  ┌───────────┐  │──(hourly sales data)──────> │  ┌───────────┐  │
│  │ Dashboard │──│                            │──│ Forecaster│  │
│  └───────────┘  │  GET /pos/v1/schedule/today │  └───────────┘  │
│                 │ <──(today's roster)──────── │                 │
│  ┌───────────┐  │                            │  ┌───────────┐  │
│  │ Webhook   │──│ <──(schedule.published)──── │──│ Scheduler │  │
│  │ Handler   │  │ <──(employee.terminated)─── │  └───────────┘  │
│  └───────────┘  │    (HMAC-SHA256 signed)     │                 │
│                 │                            │                 │
└─────────────────┘                           └─────────────────┘
            `}</pre>
          </div>
        </Card>

        {/* FAQ */}
        <Card title="Frequently Asked Questions">
          <Collapse items={[
            {
              key: '1',
              label: 'Can BonApp and Staff Pro share the same user accounts?',
              children: <Paragraph>No. They have completely separate authentication systems. An employee has one account in BonApp POS and a separate account in Staff Pro. They are linked by <strong>email address</strong> or <strong>employee ID mapping</strong> stored in BonApp.</Paragraph>,
            },
            {
              key: '2',
              label: 'What happens if Staff Pro is down when BonApp sends a clock event?',
              children: <Paragraph>BonApp should queue the event locally and retry with exponential backoff. Staff Pro accepts clock events with past timestamps, so late delivery is fine — the original timestamp is preserved.</Paragraph>,
            },
            {
              key: '3',
              label: 'Do employees need to clock in on both systems?',
              children: <Paragraph>No. If the POS integration is active, clocking into the BonApp POS terminal automatically sends a clock-in event to Staff Pro. Employees do NOT need to double-clock. However, they can still use Staff Pro's own clock feature if they prefer (e.g., mobile clock-in before arriving).</Paragraph>,
            },
            {
              key: '4',
              label: 'Can other POS systems (Lightspeed, Toast, Square) also connect?',
              children: <Paragraph>Yes! The API is POS-agnostic. Any POS system that can make HTTP requests can connect using the exact same endpoints. Each POS gets its own API key.</Paragraph>,
            },
            {
              key: '5',
              label: 'Is the sales data required?',
              children: <Paragraph>No. Sales data (Step 7) is optional. It enables AI labor forecasting and SPLH tracking, but Staff Pro's core features (scheduling, time tracking, payroll) work without any POS data at all.</Paragraph>,
            },
          ]} />
        </Card>

      </Space>
    </div>
  );
}
