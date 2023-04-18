import { useState, useEffect } from 'react';
import { Box, Text, DataTable } from 'grommet';

import AuthTokenHelper from '../helpers/AuthTokenHelper';
import RestCallHelper from '../helpers/RestCallHelper';
import NetworkResources from '../resources/NetworkResources';

function RequestViewer(props: any) {
  const { type } = props;
  const [employeeRequests, setEmployeeRequests] = useState<any[]>([]);
  const formatDate = (date: any) => {
    const year = date.slice(0, 4);
    const month = date.slice(5, 7);
    const day = date.slice(8, 10);
    const hour = date.slice(11, 13);
    const minute = date.slice(14, 16);
    return `${year}-${month}-${day} ${hour}:${minute}`;
  };
  const requestColumns = [
    {
      property: 'date',
      header: 'Date',
      render: (data: any) => <Text>{data.date.slice(0, 10)}</Text>,
    },
    {
      property: 'reason',
      header: 'Reason',
      render: (data: any) => (
        <Text>{data.body.length === 0 ? 'No reason' : data.body}</Text>
      ),
    },
    {
      property: 'start',
      header: 'Start',
      render: (data: any) => <Text>{formatDate(data.startTime)}</Text>,
    },
    {
      property: 'end',
      header: 'End',
      render: (data: any) => <Text>{formatDate(data.endTime)}</Text>,
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

  const requestCustomColumns = [
    {
      property: 'date',
      header: 'Date',
      render: (data: any) => <Text>{data.date.slice(0, 10)}</Text>,
    },
    {
      property: 'reason',
      header: 'Reason',
      render: (data: any) => (
        <Text>{data.body.length === 0 ? 'No reason' : data.body}</Text>
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

  const getEmployeeRequests = async () => {
    const token = AuthTokenHelper.getFromLocalStorage();
    const getRequestsUrl = `${NetworkResources.API_URL}/Request/GetOwnRequests`;
    const getRequestsParams = JSON.stringify({
      sessionToken: token,
    });
    const { data } = await RestCallHelper.GetRequest(
      getRequestsUrl,
      getRequestsParams
    );
    if (data != null) {
      try {
        const dataObject = JSON.parse(data);
        const request: any[] = [];
        for (let i = 0; i < dataObject.length; i += 1) {
          if (dataObject[i].type === type) {
            request.push(dataObject[i]);
          }
        }
        setEmployeeRequests(request);
      } catch (error) {
        console.log(error);
      }
    }
  };

  useEffect(() => {
    getEmployeeRequests();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  return (
    <Box>
      <DataTable
        columns={type !== null ? requestColumns : requestCustomColumns}
        data={employeeRequests}
        step={10}
        size="large"
        sortable
      />
    </Box>
  );
}
export default RequestViewer;
