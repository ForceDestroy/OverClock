import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import {
  Box,
  Button,
  Grid,
  Text,
  Stack,
  Meter,
  Layer,
  Calendar,
  Heading,
  TextArea,
  Select,
  Notification,
} from 'grommet';
import RequestViewer from '../components/RequestViewer';

import AuthTokenHelper from '../helpers/AuthTokenHelper';
import RestCallHelper from '../helpers/RestCallHelper';
import NetworkResources from '../resources/NetworkResources';

function TimeOff() {
  const totalVacation = 14;
  const [vacationUsed, setVacationUsed] = useState(0);
  const [vacationAvailable, setVacationAvailable] = useState(14);
  const [vacationActive, setVacationActive] = useState(0);
  const [vacationLabel, setVacationLabel] = useState('');
  const [vacationHighlight, setVacationHighlight] = useState(false);

  const totalSickDays = 14;
  const [sickDaysUsed, setSickDaysUsed] = useState(0);
  const [sickDaysAvailable, setSickDaysAvailable] = useState(14);
  const [sickDaysActive, setSickDaysActive] = useState(0);
  const [sickDaysLabel, setSickDaysLabel] = useState('');
  const [sickDaysHighlight, setSickDaysHighlight] = useState(false);

  const [requestDate, setRequestDate] = useState<any>();
  const onSelectRequestDate = (nextDate: any) => {
    setRequestDate(nextDate !== requestDate ? nextDate : undefined);
  };
  const [requestType, setRequestType] = useState('');
  const [showRequestLayer, setShowRequestLayer] = useState(false);
  const [requestReason, setRequestReason] = useState('');

  const [missingType, setMissingType] = useState(false);
  const [missingDate, setMissingDate] = useState(false);

  const [showCustomRequestLayer, setShowCustomRequestLayer] = useState(false);
  const [customRequestReason, setCustomRequestReason] = useState('');

  const navigate = useNavigate();

  const getAccountDetails = async () => {
    const token = AuthTokenHelper.getFromLocalStorage();
    const url = `${NetworkResources.API_URL}/User/GetAccountInfo`;
    const params = JSON.stringify({
      sessionToken: token,
    });
    const { data } = await RestCallHelper.GetRequest(url, params);
    if (data != null) {
      try {
        const dataObject = JSON.parse(data);
        setVacationAvailable(dataObject.vacationDays);
        setSickDaysAvailable(dataObject.sickDays);
        setVacationUsed(totalVacation - dataObject.vacationDays);
        setSickDaysUsed(totalSickDays - dataObject.sickDays);
      } catch (error) {
        console.log(error);
      }
    }
  };
  const InputCheck = () => {
    if (requestType === '') {
      setMissingType(true);
      return false;
    }
    if (requestDate === undefined) {
      setMissingDate(true);
      return false;
    }
    return true;
  };

  const submitRequest = async () => {
    const token = AuthTokenHelper.getFromLocalStorage();
    const url = `${NetworkResources.API_URL}/Request/RequestTimeOff`;
    const params = JSON.stringify({
      sessionToken: token,
    });
    if (!InputCheck()) {
      return;
    }
    let body;
    let start;
    let end;
    if (typeof requestDate === 'object') {
      start = new Date(
        new Date(requestDate[0][0]).getFullYear(),
        new Date(requestDate[0][0]).getMonth(),
        new Date(requestDate[0][0]).getDate(),
        0,
        0,
        0,
        0
      );
      end = new Date(
        new Date(requestDate[0][1]).getFullYear(),
        new Date(requestDate[0][1]).getMonth(),
        new Date(requestDate[0][1]).getDate(),
        23,
        59,
        59,
        0
      );
      body = JSON.stringify({
        Body: requestReason,
        Date: new Date(),
        StartTime: start,
        EndTime: end,
        Type: requestType,
      });
    } else {
      start = new Date(
        new Date(requestDate).getFullYear(),
        new Date(requestDate).getMonth(),
        new Date(requestDate).getDate(),
        0,
        0,
        0,
        0
      );
      end = new Date(
        new Date(requestDate).getFullYear(),
        new Date(requestDate).getMonth(),
        new Date(requestDate).getDate(),
        23,
        59,
        59,
        0
      );
      body = JSON.stringify({
        Body: requestReason,
        Date: new Date(),
        StartTime: start,
        EndTime: end,
        Type: requestType,
      });
    }
    await RestCallHelper.PostRequest(url, params, body);
    setShowRequestLayer(false);
  };

  const submitCustomRequest = async () => {
    const token = AuthTokenHelper.getFromLocalStorage();
    const url = `${NetworkResources.API_URL}/Request/CustomRequest`;
    const params = JSON.stringify({
      sessionToken: token,
    });
    const request = new Date();
    const start = new Date(
      new Date(request).getFullYear(),
      new Date(request).getMonth(),
      new Date(request).getDate(),
      0,
      0,
      0,
      0
    );
    const end = new Date(
      new Date(request).getFullYear(),
      new Date(request).getMonth(),
      new Date(request).getDate(),
      23,
      59,
      59,
      0
    );
    const body = JSON.stringify({
      Body: customRequestReason,
      Date: new Date(),
      StartTime: start,
      EndTime: end,
    });
    console.log(body);
    const { data, status } = await RestCallHelper.PostRequest(
      url,
      params,
      body
    );
    console.log(status);
    setShowCustomRequestLayer(false);
  };

  useEffect(() => {
    const validToken = AuthTokenHelper.validateToken();
    if (!validToken) {
      navigate('/login');
    }
  }, [navigate]);

  useEffect(() => {
    getAccountDetails();
  }, []);
  return (
    <Box>
      <h1>&nbsp;&nbsp;Time Off</h1>
      <Grid
        fill
        rows={['auto', 'flex']}
        columns={{ count: 2, size: 'auto' }}
        margin="medium"
        gap="small"
        width="90vw"
      >
        <Box>
          <Box align="center" gap="small">
            <Text
              size="large"
              weight="bold"
              alignSelf="center"
              margin={{ top: 'xsmall' }}
            >
              Vacation Days
            </Text>
            <Stack anchor="center">
              <Meter
                type="circle"
                background="light-2"
                values={[
                  {
                    value: vacationUsed,
                    color: 'purple',
                    onHover: (over) => {
                      setVacationActive(over ? vacationUsed : 0);
                      setVacationLabel(over ? 'Used' : '');
                    },
                    onClick: () => {
                      setVacationHighlight(() => !vacationHighlight);
                    },
                    highlight: vacationHighlight,
                  },
                  {
                    value: vacationAvailable,
                    onHover: (over) => {
                      setVacationActive(over ? vacationAvailable : 0);
                      setVacationLabel(over ? 'Available' : '');
                    },
                  },
                ]}
                max={totalVacation}
                size="medium"
                thickness="large"
                aria-label="meter"
                data-testid="vacation-meter"
              />
              <Box align="center">
                <Box direction="row" align="center" pad={{ bottom: 'xsmall' }}>
                  <Text size="xlarge" weight="bold">
                    {vacationActive || totalVacation}
                  </Text>
                </Box>
                <Text size="medium">{vacationLabel || 'Total'}</Text>
              </Box>
            </Stack>
          </Box>
          <Box align="center" gap="small" data-testid="sick-days">
            <Text
              size="large"
              weight="bold"
              alignSelf="center"
              margin={{ top: 'xsmall' }}
            >
              Sick Days
            </Text>
            <Stack anchor="center">
              <Meter
                type="circle"
                background="light-2"
                values={[
                  {
                    value: sickDaysUsed,
                    color: 'purple',
                    onHover: (over) => {
                      setSickDaysActive(over ? sickDaysUsed : 0);
                      setSickDaysLabel(over ? 'Used' : '');
                    },
                    onClick: () => {
                      setSickDaysHighlight(() => !sickDaysHighlight);
                    },
                    highlight: sickDaysHighlight,
                  },
                  {
                    value: sickDaysAvailable,
                    onHover: (over) => {
                      setSickDaysActive(over ? sickDaysAvailable : 0);
                      setSickDaysLabel(over ? 'Available' : '');
                    },
                  },
                ]}
                max={totalSickDays}
                size="medium"
                thickness="large"
                aria-label="meter"
                data-testid="sick-days-meter"
              />
              <Box align="center">
                <Box direction="row" align="center" pad={{ bottom: 'xsmall' }}>
                  <Text size="xlarge" weight="bold">
                    {sickDaysActive || totalSickDays}
                  </Text>
                </Box>
                <Text size="medium">{sickDaysLabel || 'Total'}</Text>
              </Box>
            </Stack>
          </Box>
        </Box>
        <Box>
          <Box
            align="center"
            gap="small"
            width="40vw"
            margin={{ left: 'xxxsmall', right: 'large' }}
          >
            <Box direction="row" gap="small">
              <Button
                label="Create Request"
                alignSelf="center"
                onClick={() => {
                  setShowRequestLayer(true);
                }}
                data-testid="createRequestButton"
              />
              <Button
                label="Create Custom Request"
                alignSelf="center"
                onClick={() => {
                  setShowCustomRequestLayer(true);
                }}
                data-testid="createCustomRequestButton"
              />
            </Box>
            <Text weight="bold"> Request Vacation Status </Text>
            <RequestViewer type="Vacation" />
            <Text weight="bold"> Request Sick Days Status </Text>
            <RequestViewer type="Sick" />
            <br />
            <Text weight="bold"> Request Personal Time Off Status </Text>
            <RequestViewer type="Personal" />
          </Box>
        </Box>
      </Grid>
      {showRequestLayer && (
        <Layer
          position="center"
          modal
          onClickOutside={() => setShowRequestLayer(false)}
          onEsc={() => setShowRequestLayer(false)}
        >
          <Grid
            fill
            rows={['auto', 'flex']}
            columns={{ count: 2, size: 'auto' }}
            gap="small"
          >
            <Box pad="medium" gap="small">
              <Heading level={2} margin="none">
                Create a Request
              </Heading>

              <Box border="horizontal">
                <Text>
                  Please select the type of request and fill in the range for
                  which you would like to take off
                </Text>
              </Box>
              <Select
                placeholder="Type of Request"
                options={['Vacation', 'Sick', 'Personal']}
                value={requestType}
                onChange={({ option }) => {
                  setRequestType(option);
                }}
                data-testid="requestType"
              />
              {requestType === 'Vacation' && (
                <Notification
                  title="Vacation Days"
                  message="Please note that these days will be deducted from your vacation days"
                  status="info"
                  data-testid="requestVacationNotification"
                />
              )}
              {requestType === 'Sick' && (
                <Notification
                  title="Sick Days"
                  message="Please note that these days will be deducted from your sick days"
                  status="info"
                  data-testid="requestSickNotification"
                />
              )}
              {requestType === 'Personal' && (
                <Notification
                  title="Personal Days"
                  message="Please note that these will be extra unpaid time off for yourself"
                  status="warning"
                  data-testid="requestPersonalNotification"
                />
              )}
              <TextArea
                placeholder="Extra notes for manager"
                value={requestReason}
                onChange={(event) => {
                  setRequestReason(event.target.value);
                }}
                resize={false}
                data-testid="requestReason"
              />
              <Box direction="row" alignSelf="center">
                <Button
                  label="Close"
                  onClick={() => setShowRequestLayer(false)}
                  margin="small"
                  data-testid="requestCloseButton"
                />
                <Button
                  label="Submit"
                  onClick={() => {
                    submitRequest();
                  }}
                  margin="small"
                  data-testid="requestSubmitButton"
                />
              </Box>
            </Box>
            <Box>
              <Box gap="small">
                <Heading level={3}>Calendar</Heading>
              </Box>
              <Calendar
                bounds={['2022-01-01', '2023-06-30']}
                onSelect={onSelectRequestDate}
                range
                size="medium"
              />
            </Box>
          </Grid>
        </Layer>
      )}
      {showCustomRequestLayer && (
        <Layer
          position="center"
          modal
          onClickOutside={() => setShowCustomRequestLayer(false)}
          onEsc={() => setShowCustomRequestLayer(false)}
        >
          <Grid
            fill
            rows={['auto', 'flex']}
            columns={{ count: 2, size: 'auto' }}
            gap="small"
          >
            <Box pad="medium" gap="small">
              <Heading level={2} margin="none">
                Create a Custom Request
              </Heading>
              <Box border="horizontal">
                <Text>
                  Please fill in the text box any extraneous requests other than
                  the available requests that you would like to make to your
                  employer
                </Text>
              </Box>
              <TextArea
                placeholder="Fill in your custom request here"
                value={customRequestReason}
                onChange={(event) => {
                  setCustomRequestReason(event.target.value);
                }}
                resize={false}
                data-testid="customRequestReason"
              />
              <Box direction="row" alignSelf="center">
                <Button
                  label="Close"
                  onClick={() => setShowCustomRequestLayer(false)}
                  margin="small"
                  data-testid="customRequestCloseButton"
                />
                <Button
                  label="Submit"
                  onClick={() => {
                    submitCustomRequest();
                  }}
                  margin="small"
                  data-testid="customRequestSubmitButton"
                />
              </Box>
            </Box>
            <Box>
              <Heading level={3}>Status</Heading>
              <RequestViewer type={null} />
            </Box>
          </Grid>
        </Layer>
      )}
      {missingType && (
        <Notification
          title="Missing Type"
          message="Please select a request type"
          status="warning"
          data-testid="missingTypeNotification"
          toast
          time={3000}
        />
      )}
      {missingDate && (
        <Notification
          title="Missing Date"
          message="Please add a date for your request"
          status="warning"
          data-testid="missingDateNotification"
          toast
          time={3000}
        />
      )}
    </Box>
  );
}

export default TimeOff;
