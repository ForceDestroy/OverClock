import { Box, Button, DataTable, Grid, Layer, Text } from 'grommet';
import { Refresh } from 'grommet-icons';
import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import AuthTokenHelper from '../helpers/AuthTokenHelper';
import RestCallHelper from '../helpers/RestCallHelper';
import NetworkResources from '../resources/NetworkResources';
import UIResources from '../resources/UIResources';

function EmployerRequests() {
  const navigate = useNavigate();

  // Fields for requests
  const [requests, setRequests] = useState([]);
  const [showRequest, setShowRequest] = useState(false);
  const [request, setRequest] = useState({} as any);

  const requestColumns = [
    {
      property: 'date',
      header: 'Date',
      render: (data: any) => <Text>{data.date.slice(0, -9)}</Text>,
    },
    {
      property: 'name',
      header: 'Name',
      render: (data: any) => <Text>{data.fromName}</Text>,
    },
    {
      property: 'type',
      header: 'Type',
      render: (data: any) => (
        <Text>{data.type !== null ? data.type : 'Custom'}</Text>
      ),
    },
    {
      property: 'status',
      header: 'Status',
      render: (data: any) => (
        <Text
          color={
            // eslint-disable-next-line no-nested-ternary
            data.status === 'Pending'
              ? 'orange'
              : // eslint-disable-next-line no-nested-ternary
              data.status === 'Approved'
              ? 'green'
              : data.status === 'Acknowledged'
              ? 'blue'
              : 'red'
          }
        >
          {data.status}
        </Text>
      ),
    },
  ];

  const getAllRequests = async () => {
    const url = `${NetworkResources.API_URL}/Request/EmployerGetAllRequests`;
    const token = AuthTokenHelper.getFromLocalStorage();
    const params = JSON.stringify({
      sessionToken: token,
    });

    const { data } = await RestCallHelper.GetRequest(url, params);
    try {
      const dataObject = JSON.parse(data);
      setRequests(dataObject);
    } catch (error) {
      console.log(error);
    }
  };

  const approveRequest = async () => {
    const url = `${NetworkResources.API_URL}/Request/ApproveRequest`;
    const token = AuthTokenHelper.getFromLocalStorage();
    const params = JSON.stringify({
      sessionToken: token,
      requestId: request.id,
      newStatus: 'Approved',
    });
    const body = '{}';

    await RestCallHelper.PostRequest(url, params, body);

    getAllRequests();
  };

  const ackRequest = async (reqID: string) => {
    if (request.status !== 'Pending') return;

    const url = `${NetworkResources.API_URL}/Request/ApproveRequest`;
    const token = AuthTokenHelper.getFromLocalStorage();
    const params = JSON.stringify({
      requestId: reqID,
      newStatus: 'Acknowledged',
      sessionToken: token,
    });
    const body = '{}';

    await RestCallHelper.PostRequest(url, params, body);

    getAllRequests();
  };

  const denyRequest = async () => {
    const url = `${NetworkResources.API_URL}/Request/ApproveRequest`;
    const token = AuthTokenHelper.getFromLocalStorage();
    const params = JSON.stringify({
      requestId: request.id,
      newStatus: 'Denied',
      sessionToken: token,
    });
    const body = '{}';

    await RestCallHelper.PostRequest(url, params, body);

    getAllRequests();
  };

  // Pre Renders
  useEffect(() => {
    const accessPage = AuthTokenHelper.getAccessLevel();
    if (accessPage === '0') {
      navigate('/EmployeeDashboard');
    }
  }, []);

  useEffect(() => {
    const validToken = AuthTokenHelper.validateToken();
    if (!validToken) {
      navigate('/login');
    } else {
      getAllRequests();
    }
  }, [navigate]);

  return (
    <Grid
      rows={['flex', 'auto']}
      columns={{ count: 1, size: 'auto' }}
      gap="medium"
      margin="small"
    >
      <Box
        width="60vw"
        background={UIResources.PALETTE.SECONDARY}
        pad="medium"
        round="small"
        gap="medium"
        border={{ color: UIResources.PALETTE.BORDER, size: 'small' }}
        height="80vh"
        overflow={{ vertical: 'scroll', horizontal: 'hidden' }}
      >
        <Text size="xxlarge" weight="bold">
          Mode Durgaa - Approve Requests
        </Text>
        <Box direction="row" gap="small" margin="small">
          <Button
            margin={{ bottom: 'small' }}
            label={
              <Box direction="row" gap="small">
                <Refresh size="medium" />
                <Text size="medium">Refresh</Text>
              </Box>
            }
            data-testid="refreshButton"
            onClick={() => {
              getAllRequests();
            }}
            plain
            hoverIndicator
          />
        </Box>
        <DataTable
          columns={requestColumns}
          data={requests}
          step={10}
          onClickRow={(event) => {
            setRequest(event.datum);
            ackRequest(event.datum.id);
            setShowRequest(true);
          }}
          data-testid="timesheetDataTable"
        />
      </Box>
      {showRequest && (
        <Layer position="center" onClickOutside={() => setShowRequest(false)}>
          <Grid
            rows={['auto', 'flex']}
            columns={{ count: 1, size: 'auto' }}
            gap="medium"
            margin="small"
          >
            <Box
              pad="xsmall"
              elevation="small"
              background="dark-1"
              round="xsmall"
              overflow="hidden"
              align="center"
            >
              <Text>Request Submitted On {request.date.slice(0, -9)}</Text>
            </Box>
            <Box pad="xsmall" round="xsmall">
              <Text>Employee Details</Text>
              <Text>Name: {request.fromName}</Text>
              <Text>ID: {request.fromId}</Text>
              <Text>Request Details</Text>
              <Text>Type: {request.type}</Text>
              <Text>Reason: {request.body}</Text>
              <Text>Start Date: {request.startTime.slice(0, -9)}</Text>
              <Text>End Date: {request.endTime.slice(0, -9)}</Text>
              <Text
                color={
                  // eslint-disable-next-line no-nested-ternary
                  request.status === 'Pending'
                    ? 'orange'
                    : // eslint-disable-next-line no-nested-ternary
                    request.status === 'Approved'
                    ? 'green'
                    : request.status === 'Acknowledged'
                    ? 'blue'
                    : 'red'
                }
              >
                Status: {request.status}
              </Text>
            </Box>
            <Button
              margin={{ top: 'small' }}
              label="Approve Request"
              onClick={() => {
                approveRequest();
                setShowRequest(false);
              }}
            />
            <Button
              margin={{ top: 'small' }}
              label="Deny Request"
              color="red"
              onClick={() => {
                denyRequest();
                setShowRequest(false);
              }}
            />
          </Grid>
        </Layer>
      )}
    </Grid>
  );
}

export default EmployerRequests;
