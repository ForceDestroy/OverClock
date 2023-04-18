import {
  Box,
  Button,
  DataTable,
  DateInput,
  Grid,
  Layer,
  Text,
  TextInput,
  Tip,
  Grommet,
  Heading,
  TextArea,
} from 'grommet';
import {
  Aid,
  Attraction,
  CreditCard,
  Currency,
  Gift,
  Home,
  Key,
  LinkNext,
  LinkPrevious,
  MailOption,
  Money,
  Phone,
  User,
  Refresh,
} from 'grommet-icons';
import { TimeGridScheduler, classes } from '@remotelock/react-week-scheduler';
import '../styles/GridScheduler.css';
import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { DateRange } from '@remotelock/react-week-scheduler/src/types';
import AuthTokenHelper from '../helpers/AuthTokenHelper';
import RestCallHelper from '../helpers/RestCallHelper';
import NetworkResources from '../resources/NetworkResources';
import UIResources from '../resources/UIResources';

function EmployerDashboard() {
  const navigate = useNavigate();

  // Fields for Employee Creation
  const [showCreateEmployee, setShowCreateEmployee] = useState(false);

  const [id, setId] = useState('');
  const [name, setName] = useState('');
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const accessLevel = 0;
  const [address, setAddress] = useState('');
  const [salary, setSalary] = useState(0);
  const [vacationDays, setVacationDays] = useState(0);
  const [sickDays, setSickDays] = useState(0);
  const [date, setDate] = useState();
  const [birthday, setBirthday] = useState(new Date());
  const [phoneNumber, setPhoneNumber] = useState(0);
  const themeColor = 0;
  const [bankingNumber, setBankingNumber] = useState('');
  const [SIN, setSIN] = useState('');

  // Fields for List of Employees
  const [employees, setEmployees] = useState([]);
  const [employeeId, setEmployeeId] = useState('');

  const [showEmployee, setShowEmployee] = useState(false);
  const [confirmDelete, setConfirmDelete] = useState(false);
  const employeeColumns = [
    {
      property: 'avatar',
      header: '',
      render: (data: any) => <User size="large" />,
    },
    {
      property: 'id',
      header: 'ID',
      render: (data: any) => <Text>{data.id}</Text>,
    },
    {
      property: 'name',
      header: 'Name',
      render: (data: any) => <Text>{data.name}</Text>,
    },
    {
      property: 'email',
      header: 'Email',
      render: (data: any) => <Text>{data.email}</Text>,
    },
  ];

  // Fields for Schedule
  const initialState: DateRange[] = [];
  const deleteThreshold = 20 * 60 * 1000;
  const [schedule, setSchedule] = useState(initialState);
  const [activeWeek, setActiveWeek] = useState(new Date());
  const [startDate, setStartDate] = useState(new Date());
  const [endDate, setEndDate] = useState(new Date());
  const [position, setPosition] = useState('');
  const [breakTime, setBreakTime] = useState(0);

  // Fields for Announcements
  const [showAnnouncements, setShowAnnouncements] = useState(false);
  const [announcementTitle, setAnnouncementTitle] = useState('');
  const [announcement, setAnnouncement] = useState('');

  const getBoundsOfWeek = (_activeWeek: Date) => {
    const start = new Date(_activeWeek);
    start.setDate(start.getDate() - start.getDay());
    setStartDate(start);
    const end = new Date(start);
    end.setDate(end.getDate() + 6);
    setEndDate(end);
    return { start, end };
  };

  // Rest Calls for Schedules
  const getEmployeeSchedule = async (
    _employeeId: string,
    _activeWeek: Date
  ) => {
    const url = `${NetworkResources.API_URL}/Schedule/GetSchedulesForAnEmployee`;
    const token = AuthTokenHelper.getFromLocalStorage();
    const params = JSON.stringify({
      employeeId: _employeeId,
      sessionToken: token,
      date: _activeWeek,
    });

    const { data } = await RestCallHelper.GetRequest(url, params);

    if (data != null) {
      const dataObject = JSON.parse(data);
      const blockList: DateRange[] = [];
      dataObject.forEach((TimeBlock: any) => {
        const timeRange: DateRange = [
          new Date(TimeBlock.startTime),
          new Date(TimeBlock.endTime),
        ];
        blockList.push(timeRange);
        setPosition(TimeBlock.position);
        setBreakTime(TimeBlock.allowedBreakTime);
      });
      setSchedule(blockList);
    }
  };

  const changeWeek = (_activeWeek: Date) => {
    setActiveWeek(_activeWeek);
    getBoundsOfWeek(_activeWeek);
    getEmployeeSchedule(employeeId, _activeWeek);
  };

  const updateSchedule = () => {
    const url = `${NetworkResources.API_URL}/Schedule/ModifySchedule`;
    const token = AuthTokenHelper.getFromLocalStorage();
    const params = JSON.stringify({
      sessionToken: token,
      userId: employeeId,
    });
    const bodyData = [];
    for (let i = 0; i < schedule.length; i++) {
      const startTime = schedule[i][0];
      const endTime = schedule[i][1];
      const nDate = new Date(
        startTime.getFullYear(),
        startTime.getMonth(),
        startTime.getDate()
      );
      bodyData.push({
        date: nDate,
        startTime,
        endTime,
        userName: name,
        userId: employeeId,
        position,
        allowedBreakTime: breakTime,
      });
    }
    const body = JSON.stringify(bodyData);
    RestCallHelper.PostRequest(url, params, body);
  };

  // Rest Calls for Announcements
  const submitNewAnnouncement = async () => {
    const announcementObject = {
      Title: announcementTitle,
      Body: announcement,
      Date: new Date(),
    };

    const url = `${NetworkResources.API_URL}/Bulletin/CreateAnnouncement`;
    const token = AuthTokenHelper.getFromLocalStorage();
    const params = JSON.stringify({
      sessionToken: token,
      announcement: announcementObject,
    });

    const { data } = await RestCallHelper.PostRequest(url, params, '', true);
  };

  // Rest Calls for Employees
  // eslint-disable-next-line react-hooks/exhaustive-deps
  const getAllEmployees = async () => {
    const url = `${NetworkResources.API_URL}/Employer/GetListOfEmployees`;
    const token = AuthTokenHelper.getFromLocalStorage();
    const params = JSON.stringify({
      sessionToken: token,
    });

    const { data } = await RestCallHelper.GetRequest(url, params);
    try {
      const dataObject = JSON.parse(data);
      setEmployees(dataObject);
    } catch (error) {
      console.log(error);
    }
  };

  const addEmployee = async () => {
    const url = `${NetworkResources.API_URL}/Employer/CreateEmployee`;
    const token = AuthTokenHelper.getFromLocalStorage();
    const params = JSON.stringify({
      sessionToken: token,
    });

    const body = JSON.stringify({
      id,
      name,
      email,
      password,
      accessLevel,
      address,
      salary,
      vacationDays,
      sickDays,
      birthday,
      phoneNumber,
      themeColor,
      bankingNumber,
      sin: SIN,
    });

    const { data } = await RestCallHelper.PostRequest(url, params, body);

    if (data != null) {
      getAllEmployees();
    }
  };

  const deleteEmployee = async () => {
    const url = `${NetworkResources.API_URL}/Employer/DeleteEmployee`;
    const token = AuthTokenHelper.getFromLocalStorage();
    const params = JSON.stringify({
      userId: employeeId,
      sessionToken: token,
    });

    const { data } = await RestCallHelper.DeleteRequest(url, params);

    if (data != null) {
      getAllEmployees();
    }
  };

  const updateEmployee = async () => {
    const url = `${NetworkResources.API_URL}/Employer/UpdateEmployee`;
    const token = AuthTokenHelper.getFromLocalStorage();
    const params = JSON.stringify({
      sessionToken: token,
    });
    const body = JSON.stringify({
      id,
      name,
      email,
      password,
      accessLevel,
      address,
      salary,
      vacationDays,
      sickDays,
      birthday,
      phoneNumber,
      themeColor,
      bankingNumber,
      sin: SIN,
    });
    const { data } = await RestCallHelper.PutRequest(url, params, body);

    if (data != null) {
      getAllEmployees();
    }
  };

  // Pre Renders
  useEffect(() => {
    getAllEmployees();
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
      getAllEmployees();
    }
  }, [navigate]);

  const setFieldHooks = (_employee: any) => {
    setId(_employee.id);
    setName(_employee.name);
    setEmail(_employee.email);
    setPassword(_employee.password);
    setAddress(_employee.address);
    setSalary(_employee.salary);
    setVacationDays(_employee.vacationDays);
    setSickDays(_employee.sickDays);
    setBirthday(new Date(_employee.birthday));
    setDate(_employee.birthday);
    setPhoneNumber(_employee.phoneNumber);
    setBankingNumber(_employee.bankingNumber);
    setSIN(_employee.sin);
    setActiveWeek(new Date());
    getBoundsOfWeek(activeWeek);
  };

  const emptyFieldHooks = () => {
    setId('');
    setName('');
    setEmail('');
    setPassword('');
    setAddress('');
    setSalary(0);
    setVacationDays(0);
    setSickDays(0);
    setDate(undefined);
    setBirthday(new Date());
    setPhoneNumber(0);
    setBankingNumber('');
    setSIN('');
  };

  return (
    <Grommet>
      <h1>&nbsp;&nbsp;Management</h1>
      {showAnnouncements && (
        <Layer
          position="center"
          modal
          onClickOutside={() => setShowAnnouncements(false)}
          onEsc={() => setShowAnnouncements(false)}
        >
          <Grid
            fill
            rows={['auto', 'flex']}
            columns={{ count: 1, size: 'auto' }}
            gap="small"
          >
            <Box pad="medium" gap="small">
              <Heading level={2} margin="none">
                Send a company wide announcement
              </Heading>
              <Box>
                <Text>
                  Please fill in the text box with any announcements you would
                  like to send to all employees.
                </Text>
              </Box>
              <TextInput
                placeholder="Enter the announcement title here."
                value={announcementTitle}
                onChange={(event) => {
                  setAnnouncementTitle(event.target.value);
                }}
                data-testid="customRequestReason"
              />
              <TextArea
                placeholder="Enter the announcement here."
                value={announcement}
                onChange={(event) => {
                  setAnnouncement(event.target.value);
                }}
                resize={false}
                data-testid="customRequestReason"
              />
              <Box direction="row" alignSelf="center">
                <Button
                  label="Close"
                  onClick={() => setShowAnnouncements(false)}
                  margin="small"
                  data-testid="newAnnouncementCloseButton"
                />
                <Button
                  label="Submit"
                  onClick={() => {
                    submitNewAnnouncement();
                    setShowAnnouncements(false);
                    setAnnouncement('');
                    setAnnouncementTitle('');
                  }}
                  margin="small"
                  data-testid="newAnnouncementSubmitButton"
                />
              </Box>
            </Box>
          </Grid>
        </Layer>
      )}
      <Grid
        rows={['medium', 'medium']}
        columns={['3/4', '1/4']}
        gap="medium"
        margin="small"
        fill
      >
        <Box
          width="60vw"
          background={UIResources.PALETTE.SECONDARY}
          pad="medium"
          round="small"
          border={{ color: UIResources.PALETTE.BORDER, size: 'small' }}
          height="70vh"
          overflow={{ vertical: 'auto', horizontal: 'hidden' }}
        >
          <Box gap="medium" margin={{ bottom: 'medium' }}>
            <Text size="xxlarge" weight="bold">
              Mode Durgaa - Employees
            </Text>
            <Box direction="row" gap="small">
              <Button
                label="+ New Employee"
                data-testid="addEmployeesButton"
                onClick={(event) => {
                  emptyFieldHooks();
                  setShowCreateEmployee(true);
                }}
                plain
                hoverIndicator
              />
              <Button
                label={
                  <Box direction="row" gap="small">
                    <Refresh size="medium" />
                    <Text size="medium">Get All Employees</Text>
                  </Box>
                }
                data-testid="getEmployeesButton"
                onClick={getAllEmployees}
                plain
                hoverIndicator
              />
            </Box>
          </Box>
          <DataTable
            columns={employeeColumns}
            data={employees}
            step={10}
            onClickRow={(event) => {
              setShowEmployee(!showEmployee);
              setEmployeeId(event.datum.id);
              setFieldHooks(event.datum);
              setActiveWeek(new Date());
              getBoundsOfWeek(new Date());
              getEmployeeSchedule(event.datum.id, new Date());
            }}
            data-testid="employeeDataTable"
          />
          {showEmployee && (
            <Layer
              position="center"
              onClickOutside={() => setShowEmployee(false)}
            >
              <Grid
                rows={['auto', 'flex']}
                columns={{ count: 2, size: 'auto' }}
                gap="medium"
                margin="small"
              >
                <Box pad="large" gap="medium" width="medium">
                  <Tip
                    dropProps={{
                      align: { left: 'right' },
                    }}
                    content={
                      <Box
                        pad="xxsmall"
                        elevation="small"
                        background="dark-1"
                        round="xsmall"
                        margin="xsmall"
                        overflow="hidden"
                        align="center"
                        width={{ max: 'small' }}
                      >
                        <svg
                          viewBox="0 0 22 22"
                          version="1.1"
                          width="10px"
                          height="6px"
                        >
                          <polygon transform="matrix(-1 0 0 1 30 0)" />
                        </svg>
                        <Box direction="row" pad="small" round="xsmall">
                          <Text>Name</Text>
                        </Box>
                      </Box>
                    }
                    plain
                  >
                    <Box>
                      <TextInput
                        placeholder="Name"
                        icon={<User />}
                        value={name}
                        disabled
                      />
                    </Box>
                  </Tip>
                  <Tip
                    dropProps={{
                      align: { left: 'right' },
                    }}
                    content={
                      <Box
                        pad="xxsmall"
                        elevation="small"
                        background="dark-1"
                        round="xsmall"
                        margin="xsmall"
                        overflow="hidden"
                        align="center"
                        width={{ max: 'small' }}
                      >
                        <svg
                          viewBox="0 0 22 22"
                          version="1.1"
                          width="10px"
                          height="6px"
                        >
                          <polygon transform="matrix(-1 0 0 1 30 0)" />
                        </svg>
                        <Box direction="row" pad="small" round="xsmall">
                          <Text>Email</Text>
                        </Box>
                      </Box>
                    }
                    plain
                  >
                    <TextInput
                      placeholder="Email"
                      icon={<MailOption />}
                      value={email}
                      onChange={(event) => setEmail(event.target.value)}
                    />
                  </Tip>
                  <Tip
                    dropProps={{
                      align: { left: 'right' },
                    }}
                    content={
                      <Box
                        pad="xxsmall"
                        elevation="small"
                        background="dark-1"
                        round="xsmall"
                        margin="xsmall"
                        overflow="hidden"
                        align="center"
                        width={{ max: 'small' }}
                      >
                        <svg
                          viewBox="0 0 22 22"
                          version="1.1"
                          width="10px"
                          height="6px"
                        >
                          <polygon transform="matrix(-1 0 0 1 30 0)" />
                        </svg>
                        <Box direction="row" pad="small" round="xsmall">
                          <Text>Phone Number</Text>
                        </Box>
                      </Box>
                    }
                    plain
                  >
                    <TextInput
                      placeholder="Phone Number"
                      icon={<Phone />}
                      value={phoneNumber}
                      onChange={(event) =>
                        setPhoneNumber(Number(event.target.value))
                      }
                    />
                  </Tip>
                  <Tip
                    dropProps={{
                      align: { left: 'right' },
                    }}
                    content={
                      <Box
                        pad="xxsmall"
                        elevation="small"
                        background="dark-1"
                        round="xsmall"
                        margin="xsmall"
                        overflow="hidden"
                        align="center"
                        width={{ max: 'small' }}
                      >
                        <svg
                          viewBox="0 0 22 22"
                          version="1.1"
                          width="10px"
                          height="6px"
                        >
                          <polygon transform="matrix(-1 0 0 1 30 0)" />
                        </svg>
                        <Box direction="row" pad="small" round="xsmall">
                          <Text>Birthday</Text>
                        </Box>
                      </Box>
                    }
                    plain
                  >
                    <Box>
                      <DateInput
                        format="dd/mm/yy"
                        reverse
                        icon={<Gift />}
                        value={date}
                        disabled
                      />
                    </Box>
                  </Tip>
                  <Tip
                    dropProps={{
                      align: { left: 'right' },
                    }}
                    content={
                      <Box
                        pad="xxsmall"
                        elevation="small"
                        background="dark-1"
                        round="xsmall"
                        margin="xsmall"
                        overflow="hidden"
                        align="center"
                        width={{ max: 'small' }}
                      >
                        <svg
                          viewBox="0 0 22 22"
                          version="1.1"
                          width="10px"
                          height="6px"
                        >
                          <polygon transform="matrix(-1 0 0 1 30 0)" />
                        </svg>
                        <Box direction="row" pad="small" round="xsmall">
                          <Text>Address</Text>
                        </Box>
                      </Box>
                    }
                    plain
                  >
                    <TextInput
                      placeholder="Address"
                      icon={<Home />}
                      value={address}
                      onChange={(event) => setAddress(event.target.value)}
                    />
                  </Tip>
                  <Tip
                    dropProps={{
                      align: { left: 'right' },
                    }}
                    content={
                      <Box
                        pad="xxsmall"
                        elevation="small"
                        background="dark-1"
                        round="xsmall"
                        margin="xsmall"
                        overflow="hidden"
                        align="center"
                        width={{ max: 'small' }}
                      >
                        <svg
                          viewBox="0 0 22 22"
                          version="1.1"
                          width="10px"
                          height="6px"
                        >
                          <polygon transform="matrix(-1 0 0 1 30 0)" />
                        </svg>
                        <Box direction="row" pad="small" round="xsmall">
                          <Text>Vacation Days</Text>
                        </Box>
                      </Box>
                    }
                    plain
                  >
                    <TextInput
                      placeholder="Vacation Days"
                      icon={<Attraction />}
                      value={vacationDays}
                      onChange={(event) =>
                        setVacationDays(Number(event.target.value))
                      }
                    />
                  </Tip>
                  <Tip
                    dropProps={{
                      align: { left: 'right' },
                    }}
                    content={
                      <Box
                        pad="xxsmall"
                        elevation="small"
                        background="dark-1"
                        round="xsmall"
                        margin="xsmall"
                        overflow="hidden"
                        align="center"
                        width={{ max: 'small' }}
                      >
                        <svg
                          viewBox="0 0 22 22"
                          version="1.1"
                          width="10px"
                          height="6px"
                        >
                          <polygon transform="matrix(-1 0 0 1 30 0)" />
                        </svg>
                        <Box direction="row" pad="small" round="xsmall">
                          <Text>Sick Days</Text>
                        </Box>
                      </Box>
                    }
                    plain
                  >
                    <TextInput
                      placeholder="Sick Days"
                      icon={<Aid />}
                      value={sickDays}
                      onChange={(event) =>
                        setSickDays(Number(event.target.value))
                      }
                    />
                  </Tip>
                  <Tip
                    dropProps={{
                      align: { left: 'right' },
                    }}
                    content={
                      <Box
                        pad="xxsmall"
                        elevation="small"
                        background="dark-1"
                        round="xsmall"
                        margin="xsmall"
                        overflow="hidden"
                        align="center"
                        width={{ max: 'small' }}
                      >
                        <svg
                          viewBox="0 0 22 22"
                          version="1.1"
                          width="10px"
                          height="6px"
                        >
                          <polygon transform="matrix(-1 0 0 1 30 0)" />
                        </svg>
                        <Box direction="row" pad="small" round="xsmall">
                          <Text>Salary</Text>
                        </Box>
                      </Box>
                    }
                    plain
                  >
                    <TextInput
                      placeholder="Salary"
                      icon={<Money />}
                      value={salary.toFixed(2)}
                      onChange={(event) =>
                        setSalary(Number(event.target.value))
                      }
                    />
                  </Tip>
                  <Button
                    margin={{ top: 'small' }}
                    label="Delete Employee"
                    color="red"
                    onClick={() => {
                      setConfirmDelete(true);
                    }}
                  />
                  <Button
                    margin={{ top: 'small' }}
                    label="Save Schedule"
                    onClick={() => {
                      updateSchedule();
                    }}
                  />
                  <Button
                    margin={{ top: 'small' }}
                    label="Save & Close"
                    onClick={() => {
                      setShowEmployee(false);
                      updateSchedule();
                      updateEmployee();
                    }}
                  />
                </Box>
                <Box pad="small" gap="medium" width="xlarge">
                  <Box direction="row" gap="medium" alignSelf="center">
                    <Button
                      label="Previous Week"
                      alignSelf="center"
                      icon={<LinkPrevious />}
                      onClick={() => {
                        activeWeek.setDate(activeWeek.getDate() - 7);
                        changeWeek(activeWeek);
                      }}
                    />
                    <Text alignSelf="center">
                      Week of {startDate.toDateString()} to{' '}
                      {endDate.toDateString()}
                    </Text>
                    <Button
                      label="Next Week"
                      alignSelf="center"
                      icon={<LinkNext />}
                      reverse
                      onClick={() => {
                        activeWeek.setDate(activeWeek.getDate() + 7);
                        changeWeek(activeWeek);
                      }}
                    />
                  </Box>
                  <TimeGridScheduler
                    classes={classes}
                    style={{ width: '100%', height: '100%' }}
                    originDate={startDate}
                    onChange={(event) => {
                      event.forEach((item) => {
                        const timeDiff = Math.abs(
                          item[0].getTime() - item[1].getTime()
                        );
                        if (timeDiff < deleteThreshold) {
                          event.splice(event.indexOf(item), 1);
                        }
                      });
                      setSchedule(event);
                    }}
                    schedule={schedule}
                    defaultHours={[10, 20]}
                    visualGridVerticalPrecision={60}
                    verticalPrecision={15}
                    cellClickPrecision={60}
                  />
                </Box>
              </Grid>
            </Layer>
          )}
          {confirmDelete && (
            <Layer
              position="center"
              onClickOutside={() => setConfirmDelete(false)}
            >
              <Box pad="medium" gap="small" width="medium">
                <Box
                  direction="row"
                  justify="between"
                  align="center"
                  pad={{ vertical: 'small' }}
                >
                  <Text size="large" weight="bold">
                    Are you sure you want to delete this employee?
                  </Text>
                </Box>
                <Box
                  direction="row"
                  gap="small"
                  justify="center"
                  align="center"
                  pad="small"
                >
                  <Button
                    label="Confirm"
                    color="red"
                    onClick={() => {
                      deleteEmployee();
                      setConfirmDelete(false);
                      setShowEmployee(false);
                    }}
                  />
                  <Button
                    label="Close"
                    onClick={() => setConfirmDelete(false)}
                  />
                </Box>
              </Box>
            </Layer>
          )}
        </Box>
        <Box gap="medium" height="70vh">
          <Box
            background={UIResources.PALETTE.SECONDARY}
            pad="medium"
            justify="center"
            round="small"
            gap="medium"
            border={{ color: UIResources.PALETTE.BORDER, size: 'small' }}
            fill
          >
            <Text size="large" alignSelf="center">
              View timesheet submissions
            </Text>
            <Box direction="row" justify="center" align="center" gap="xlarge">
              <Button
                label={
                  <Box width="10em">
                    <br />
                    <Text color="white" weight="bold">
                      View Timesheets
                    </Text>
                    <br />
                  </Box>
                }
                onClick={() => navigate('/Employer/Timesheets')}
                color={UIResources.PALETTE.GRADIENT}
                primary
              />
            </Box>
          </Box>
          <Box
            background={UIResources.PALETTE.SECONDARY}
            pad="medium"
            justify="center"
            round="small"
            gap="medium"
            border={{ color: UIResources.PALETTE.BORDER, size: 'small' }}
            fill
          >
            <Text size="large" alignSelf="center">
              View requests submissions
            </Text>
            <Box direction="row" justify="center" align="center" gap="xlarge">
              <Button
                label={
                  <Box width="10em">
                    <br />
                    <Text color="white" weight="bold">
                      View Requests
                    </Text>
                    <br />
                  </Box>
                }
                onClick={() => navigate('/Employer/Requests')}
                color={UIResources.PALETTE.GRADIENT}
                primary
              />
            </Box>
          </Box>
          <Box
            background={UIResources.PALETTE.SECONDARY}
            pad="medium"
            justify="center"
            round="small"
            gap="medium"
            border={{ color: UIResources.PALETTE.BORDER, size: 'small' }}
            fill
          >
            <Text size="large" alignSelf="center" textAlign="center">
              Send Company Wide Announcements
            </Text>
            <Box direction="row" justify="center" align="center" gap="xlarge">
              <Button
                label={
                  <Box width="10em">
                    <br />
                    <Text color="white" weight="bold">
                      Send Announcement
                    </Text>
                    <br />
                  </Box>
                }
                onClick={() => setShowAnnouncements(true)}
                color={UIResources.PALETTE.GRADIENT}
                primary
              />
            </Box>
          </Box>
          {showCreateEmployee && (
            <Layer
              position="center"
              onClickOutside={() => setShowCreateEmployee(false)}
            >
              <Box pad="medium" gap="small" width="medium">
                <Tip
                  dropProps={{
                    align: { left: 'right' },
                  }}
                  content={
                    <Box
                      pad="xxsmall"
                      elevation="small"
                      background="dark-1"
                      round="xsmall"
                      margin="xsmall"
                      overflow="hidden"
                      align="center"
                      width={{ max: 'medium' }}
                    >
                      <svg
                        viewBox="0 0 22 22"
                        version="1.1"
                        width="10px"
                        height="6px"
                      >
                        <polygon transform="matrix(-1 0 0 1 30 0)" />
                      </svg>
                      <Box direction="row" pad="small" round="xsmall">
                        <Text>Name</Text>
                      </Box>
                    </Box>
                  }
                  plain
                >
                  <TextInput
                    placeholder="Name"
                    icon={<User />}
                    value={name}
                    onChange={(event) => setName(event.target.value)}
                  />
                </Tip>
                <Tip
                  dropProps={{
                    align: { left: 'right' },
                  }}
                  content={
                    <Box
                      pad="xxsmall"
                      elevation="small"
                      background="dark-1"
                      round="xsmall"
                      margin="xsmall"
                      overflow="hidden"
                      align="center"
                      width={{ max: 'medium' }}
                    >
                      <svg
                        viewBox="0 0 22 22"
                        version="1.1"
                        width="10px"
                        height="6px"
                      >
                        <polygon transform="matrix(-1 0 0 1 30 0)" />
                      </svg>
                      <Box direction="row" pad="small" round="xsmall">
                        <Text>Email</Text>
                      </Box>
                    </Box>
                  }
                  plain
                >
                  <TextInput
                    placeholder="Email"
                    icon={<MailOption />}
                    value={email}
                    onChange={(event) => setEmail(event.target.value)}
                  />
                </Tip>
                <Tip
                  dropProps={{
                    align: { left: 'right' },
                  }}
                  content={
                    <Box
                      pad="xxsmall"
                      elevation="small"
                      background="dark-1"
                      round="xsmall"
                      margin="xsmall"
                      overflow="hidden"
                      align="center"
                      width={{ max: 'medium' }}
                    >
                      <svg
                        viewBox="0 0 22 22"
                        version="1.1"
                        width="10px"
                        height="6px"
                      >
                        <polygon transform="matrix(-1 0 0 1 30 0)" />
                      </svg>
                      <Box direction="row" pad="small" round="xsmall">
                        <Text>Phone Number</Text>
                      </Box>
                    </Box>
                  }
                  plain
                >
                  <TextInput
                    placeholder="Phone Number"
                    icon={<Phone />}
                    value={phoneNumber}
                    onChange={(event) =>
                      setPhoneNumber(Number(event.target.value))
                    }
                  />
                </Tip>
                <Tip
                  dropProps={{
                    align: { left: 'right' },
                  }}
                  content={
                    <Box
                      pad="xxsmall"
                      elevation="small"
                      background="dark-1"
                      round="xsmall"
                      margin="xsmall"
                      overflow="hidden"
                      align="center"
                      width={{ max: 'medium' }}
                    >
                      <svg
                        viewBox="0 0 22 22"
                        version="1.1"
                        width="10px"
                        height="6px"
                      >
                        <polygon transform="matrix(-1 0 0 1 30 0)" />
                      </svg>
                      <Box direction="row" pad="small" round="xsmall">
                        <Text>Birthday</Text>
                      </Box>
                    </Box>
                  }
                  plain
                >
                  <DateInput
                    format="dd/mm/yyyy"
                    placeholder="dd/mm/yyyy"
                    reverse
                    icon={<Gift />}
                    value={date}
                    onChange={(event: any) => {
                      const nextValue = event.value;
                      setBirthday(new Date(nextValue));
                    }}
                  />
                </Tip>
                <Tip
                  dropProps={{
                    align: { left: 'right' },
                  }}
                  content={
                    <Box
                      pad="xxsmall"
                      elevation="small"
                      background="dark-1"
                      round="xsmall"
                      margin="xsmall"
                      overflow="hidden"
                      align="center"
                      width={{ max: 'medium' }}
                    >
                      <svg
                        viewBox="0 0 22 22"
                        version="1.1"
                        width="10px"
                        height="6px"
                      >
                        <polygon transform="matrix(-1 0 0 1 30 0)" />
                      </svg>
                      <Box direction="row" pad="small" round="xsmall">
                        <Text>Password</Text>
                      </Box>
                    </Box>
                  }
                  plain
                >
                  <TextInput
                    placeholder="Password"
                    icon={<Key />}
                    value={password}
                    onChange={(event) => setPassword(event.target.value)}
                  />
                </Tip>
                <Tip
                  dropProps={{
                    align: { left: 'right' },
                  }}
                  content={
                    <Box
                      pad="xxsmall"
                      elevation="small"
                      background="dark-1"
                      round="xsmall"
                      margin="xsmall"
                      overflow="hidden"
                      align="center"
                      width={{ max: 'medium' }}
                    >
                      <svg
                        viewBox="0 0 22 22"
                        version="1.1"
                        width="10px"
                        height="6px"
                      >
                        <polygon transform="matrix(-1 0 0 1 30 0)" />
                      </svg>
                      <Box direction="row" pad="small" round="xsmall">
                        <Text>Social Insurance Number</Text>
                      </Box>
                    </Box>
                  }
                  plain
                >
                  <TextInput
                    placeholder="Social Insurance Number"
                    icon={<CreditCard />}
                    value={SIN}
                    onChange={(event) => setSIN(event.target.value)}
                  />
                </Tip>
                <Tip
                  dropProps={{
                    align: { left: 'right' },
                  }}
                  content={
                    <Box
                      pad="xxsmall"
                      elevation="small"
                      background="dark-1"
                      round="xsmall"
                      margin="xsmall"
                      overflow="hidden"
                      align="center"
                      width={{ max: 'medium' }}
                    >
                      <svg
                        viewBox="0 0 22 22"
                        version="1.1"
                        width="10px"
                        height="6px"
                      >
                        <polygon transform="matrix(-1 0 0 1 30 0)" />
                      </svg>
                      <Box direction="row" pad="small" round="xsmall">
                        <Text>Address</Text>
                      </Box>
                    </Box>
                  }
                  plain
                >
                  <TextInput
                    placeholder="Address"
                    icon={<Home />}
                    value={address}
                    onChange={(event) => setAddress(event.target.value)}
                  />
                </Tip>
                <Tip
                  dropProps={{
                    align: { left: 'right' },
                  }}
                  content={
                    <Box
                      pad="xxsmall"
                      elevation="small"
                      background="dark-1"
                      round="xsmall"
                      margin="xsmall"
                      overflow="hidden"
                      align="center"
                      width={{ max: 'medium' }}
                    >
                      <svg
                        viewBox="0 0 22 22"
                        version="1.1"
                        width="10px"
                        height="6px"
                      >
                        <polygon transform="matrix(-1 0 0 1 30 0)" />
                      </svg>
                      <Box direction="row" pad="small" round="xsmall">
                        <Text>Vacation Days</Text>
                      </Box>
                    </Box>
                  }
                  plain
                >
                  <TextInput
                    placeholder="Vacation Days"
                    icon={<Attraction />}
                    value={vacationDays}
                    onChange={(event) =>
                      setVacationDays(Number(event.target.value))
                    }
                  />
                </Tip>
                <Tip
                  dropProps={{
                    align: { left: 'right' },
                  }}
                  content={
                    <Box
                      pad="xxsmall"
                      elevation="small"
                      background="dark-1"
                      round="xsmall"
                      margin="xsmall"
                      overflow="hidden"
                      align="center"
                      width={{ max: 'medium' }}
                    >
                      <svg
                        viewBox="0 0 22 22"
                        version="1.1"
                        width="10px"
                        height="6px"
                      >
                        <polygon transform="matrix(-1 0 0 1 30 0)" />
                      </svg>
                      <Box direction="row" pad="small" round="xsmall">
                        <Text>Sick Days</Text>
                      </Box>
                    </Box>
                  }
                  plain
                >
                  <TextInput
                    placeholder="Sick Days"
                    icon={<Aid />}
                    value={sickDays}
                    onChange={(event) =>
                      setSickDays(Number(event.target.value))
                    }
                  />
                </Tip>
                <Tip
                  dropProps={{
                    align: { left: 'right' },
                  }}
                  content={
                    <Box
                      pad="xxsmall"
                      elevation="small"
                      background="dark-1"
                      round="xsmall"
                      margin="xsmall"
                      overflow="hidden"
                      align="center"
                      width={{ max: 'medium' }}
                    >
                      <svg
                        viewBox="0 0 22 22"
                        version="1.1"
                        width="10px"
                        height="6px"
                      >
                        <polygon transform="matrix(-1 0 0 1 30 0)" />
                      </svg>
                      <Box direction="row" pad="small" round="xsmall">
                        <Text>Banking Information</Text>
                      </Box>
                    </Box>
                  }
                  plain
                >
                  <TextInput
                    placeholder="Banking Information"
                    icon={<Currency />}
                    value={bankingNumber}
                    onChange={(event) => setBankingNumber(event.target.value)}
                  />
                </Tip>
                <Tip
                  dropProps={{
                    align: { left: 'right' },
                  }}
                  content={
                    <Box
                      pad="xxsmall"
                      elevation="small"
                      background="dark-1"
                      round="xsmall"
                      margin="xsmall"
                      overflow="hidden"
                      align="center"
                      width={{ max: 'medium' }}
                    >
                      <svg
                        viewBox="0 0 22 22"
                        version="1.1"
                        width="10px"
                        height="6px"
                      >
                        <polygon transform="matrix(-1 0 0 1 30 0)" />
                      </svg>
                      <Box direction="row" pad="small" round="xsmall">
                        <Text>Salary</Text>
                      </Box>
                    </Box>
                  }
                  plain
                >
                  <TextInput
                    placeholder="Salary"
                    icon={<Money />}
                    value={salary.toFixed(2)}
                    onChange={(event) => setSalary(Number(event.target.value))}
                  />
                </Tip>
                <Button
                  margin={{ top: 'small' }}
                  label="Create & Close"
                  onClick={() => {
                    setShowCreateEmployee(false);
                    addEmployee();
                  }}
                />
              </Box>
            </Layer>
          )}
        </Box>
      </Grid>
    </Grommet>
  );
}

export default EmployerDashboard;
