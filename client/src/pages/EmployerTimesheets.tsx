import { Box, Button, DataTable, Grid, Layer, Text } from 'grommet';
import { Refresh } from 'grommet-icons';
import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import AuthTokenHelper from '../helpers/AuthTokenHelper';
import RestCallHelper from '../helpers/RestCallHelper';
import NetworkResources from '../resources/NetworkResources';
import UIResources from '../resources/UIResources';

function EmployerTimesheets() {
  const navigate = useNavigate();

  // Fields for Employee
  const [showEmployee, setShowEmployee] = useState(false);
  const [employee, setEmployee] = useState({} as any);

  // Fields for Timesheets
  const [timesheets, setTimesheets] = useState([]);
  const timesheetColumns = [
    {
      property: 'date',
      header: 'Date',
      render: (data: any) => <Text>{data.date.slice(0, -9)}</Text>,
    },
    {
      property: 'id',
      header: 'ID',
      render: (data: any) => <Text>{data.userId}</Text>,
    },
    {
      property: 'name',
      header: 'Name',
      render: (data: any) => <Text>{data.name}</Text>,
    },
  ];

  const getAllTimesheets = async () => {
    const url = `${NetworkResources.API_URL}/Timesheet/GetSubmittedTimeSheets`;
    const token = AuthTokenHelper.getFromLocalStorage();
    const params = JSON.stringify({
      sessionToken: token,
    });

    const { data } = await RestCallHelper.GetRequest(url, params);
    try {
      const dataObject = JSON.parse(data);
      setTimesheets(dataObject);
    } catch (error) {
      console.log(error);
    }
  };

  const approveTimesheet = async () => {
    const url = `${NetworkResources.API_URL}/Timesheet/ApproveTimeSheet`;
    const token = AuthTokenHelper.getFromLocalStorage();
    const params = JSON.stringify({
      sessionToken: token,
    });
    const body = JSON.stringify(employee);

    await RestCallHelper.PostRequest(url, params, body);

    getAllTimesheets();
  };

  // Pre Renders
  useEffect(() => {
    const accessPage = AuthTokenHelper.getAccessLevel();
    if (accessPage === '0') {
      navigate('/EmployeeDashboard');
    }
    getAllTimesheets();
  }, []);

  useEffect(() => {
    const validToken = AuthTokenHelper.validateToken();
    if (!validToken) {
      navigate('/login');
    } else {
      getAllTimesheets();
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
          Mode Durgaa - Approve Timesheets
        </Text>
        <Box direction="row" gap="small">
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
              getAllTimesheets();
            }}
            plain
            hoverIndicator
          />
        </Box>
        <DataTable
          columns={timesheetColumns}
          data={timesheets}
          step={10}
          onClickRow={(event) => {
            setEmployee(event.datum);
            setShowEmployee(true);
          }}
          data-testid="timesheetDataTable"
        />
      </Box>
      {showEmployee && (
        <Layer position="center" onClickOutside={() => setShowEmployee(false)}>
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
              <Text>Timesheet Summary for {employee.date.slice(0, -9)}</Text>
            </Box>
            <Box pad="xsmall" round="xsmall">
              <Text>Employee Details</Text>
              <Text>Name: {employee.name}</Text>
              <Text>ID: {employee.userId}</Text>
              <Text>Salary: {employee.salary.toFixed(2)}</Text>
              <Text>Work Breakdown</Text>
              <Text>Regular Hours: {employee.hoursWorked.toFixed(2)}</Text>
              <Text>Overtime Hours: {employee.overtimeWorked.toFixed(2)}</Text>
              <Text>
                Double Overtime Hours:{' '}
                {employee.doubleOvertimeWorked.toFixed(2)}
              </Text>
              <Text>Vacation Hours: {employee.paidHoliday.toFixed(2)}</Text>
              <Text>Sick Hours: {employee.paidSick.toFixed(2)}</Text>
            </Box>
            <Box direction="row" gap="small">
              <Button
                margin={{ top: 'small' }}
                label="Close"
                color="red"
                onClick={() => setShowEmployee(false)}
                hoverIndicator
                data-testid="closeLayerButton"
              />
              <Button
                margin={{ top: 'small' }}
                label="Approve Timesheet"
                onClick={() => {
                  approveTimesheet();
                  setShowEmployee(false);
                }}
              />
            </Box>
          </Grid>
        </Layer>
      )}
    </Grid>
  );
}

export default EmployerTimesheets;
