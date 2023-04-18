import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import {
  Box,
  Button,
  Grid,
  Text,
  Layer,
  Calendar,
  TextArea,
  Heading,
} from 'grommet';
import { TimeGridScheduler, classes } from '@remotelock/react-week-scheduler';
import { DateRange } from '@remotelock/react-week-scheduler/src/types';
import { LinkPrevious, LinkNext, Close } from 'grommet-icons';
import RequestViewer from '../components/RequestViewer';

import AuthTokenHelper from '../helpers/AuthTokenHelper';
import RestCallHelper from '../helpers/RestCallHelper';
import NetworkResources from '../resources/NetworkResources';

function Schedule() {
  const initialWeekState: DateRange[] = [];
  const deleteThreshold = 20 * 60 * 1000;
  const [schedule, setSchedule] = useState(initialWeekState);
  const [activeWeek, setActiveWeek] = useState(new Date());
  const [startDate, setStartDate] = useState(new Date());
  const [endDate, setEndDate] = useState(new Date());

  const initialState: DateRange[] = [];
  const [selectedTimeBlock, setSelectedTimeBlock] = useState(initialState);
  const [activeWeekRequestScheduleChange, setActiveWeekRequestScheduleChange] =
    useState(new Date());
  const [showRequestScheduleChangeLayer, setShowRequestScheduleChangeLayer] =
    useState(false);
  const [showRequestsLayer, setShowRequestsLayer] = useState(false);
  const [requestScheduleChangeDate, setRequestScheduleChangeDate] = useState(
    new Date()
  );
  const [startDateRequestScheduleChange, setStartDateRequestScheduleChange] =
    useState(new Date());
  const [endDateRequestScheduleChange, setEndDateRequestScheduleChange] =
    useState(new Date());
  const onSelectRequestScheduleChangeDate = (nextDate: any) => {
    setRequestScheduleChangeDate(
      nextDate !== requestScheduleChangeDate ? nextDate : undefined
    );
  };
  const [requestScheduleChangeMessage, setRequestScheduleChangeMessage] =
    useState('');

  const navigate = useNavigate();

  const getBoundsOfWeek = (_activeWeek: Date) => {
    const start = new Date(_activeWeek);
    start.setDate(start.getDate() - start.getDay());
    setStartDate(start);
    const end = new Date(start);
    end.setDate(end.getDate() + 6);
    setEndDate(end);
    return { start, end };
  };
  const getBoundsOfWeekRequestScheduleChange = (_activeWeek: Date) => {
    const start = new Date(_activeWeek);
    start.setDate(start.getDate() - start.getDay());
    setStartDateRequestScheduleChange(start);
    const end = new Date(start);
    end.setDate(end.getDate() + 6);
    setEndDateRequestScheduleChange(end);
    return { start, end };
  };

  const getEmployeeSchedule = async () => {
    const token = AuthTokenHelper.getFromLocalStorage();
    const getScheduleUrl = `${NetworkResources.API_URL}/Schedule/GetAllSchedulesForEmployee`;
    const getScheduleParams = JSON.stringify({
      sessionToken: token,
    });
    const { data } = await RestCallHelper.GetRequest(
      getScheduleUrl,
      getScheduleParams
    );
    if (data != null) {
      try {
        const dataObject = JSON.parse(data);
        const blockList: DateRange[] = [];
        dataObject.forEach((TimeBlock: any) => {
          const timeRange: DateRange = [
            new Date(TimeBlock.startTime),
            new Date(TimeBlock.endTime),
          ];
          blockList.push(timeRange);
        });
        setSchedule(blockList);
      } catch (error) {
        console.log(error);
      }
    }
  };

  const submitRequestScheduleChange = async () => {
    const token = AuthTokenHelper.getFromLocalStorage();
    const submitRequestScheduleChangeUrl = `${NetworkResources.API_URL}/Request/RequestMultipleTimesOff`;
    const submitRequestScheduleChangeParams = JSON.stringify({
      sessionToken: token,
    });
    const listOfRequests = [];
    for (let i = 0; i < selectedTimeBlock.length; i += 1) {
      listOfRequests.push({
        Body: requestScheduleChangeMessage,
        Date: new Date(),
        StartTime: selectedTimeBlock[i][0],
        EndTime: selectedTimeBlock[i][1],
        Type: 'Schedule',
      });
    }
    const submitRequestScheduleChangeBody = JSON.stringify(listOfRequests);
    await RestCallHelper.PostRequest(
      submitRequestScheduleChangeUrl,
      submitRequestScheduleChangeParams,
      submitRequestScheduleChangeBody
    );
    setShowRequestScheduleChangeLayer(false);
  };
  const changeWeek = (_activeWeek: Date) => {
    setActiveWeek(_activeWeek);
    getBoundsOfWeek(_activeWeek);
    getEmployeeSchedule();
  };

  const changeWeekRequestScheduleChange = (_activeWeek: Date) => {
    setActiveWeekRequestScheduleChange(_activeWeek);
    getBoundsOfWeekRequestScheduleChange(_activeWeek);
  };

  useEffect(() => {
    const validToken = AuthTokenHelper.validateToken();
    if (!validToken) {
      navigate('/login');
    }
  }, [navigate]);
  useEffect(() => {
    getEmployeeSchedule();
    changeWeek(new Date());
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);
  useEffect(() => {
    changeWeekRequestScheduleChange(requestScheduleChangeDate);
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [requestScheduleChangeDate]);
  return (
    <Box>
      <h1>&nbsp;&nbsp;Schedule</h1>
      <Grid fill rows={['auto', 'flex']} columns={['auto', 'flex']}>
        <Box pad="small" gap="medium" width="xlarge" height="80vh">
          <Box direction="row" gap="medium" alignSelf="center">
            <Button
              label="Previous Week"
              alignSelf="center"
              icon={<LinkPrevious />}
              onClick={() => {
                activeWeek.setDate(activeWeek.getDate() - 7);
                changeWeek(activeWeek);
              }}
              data-testid="previousWeekButton"
            />
            <Text alignSelf="center">
              Week of {startDate.toDateString()} to {endDate.toDateString()}
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
              data-testid="nextWeekButton"
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
            disabled
          />
        </Box>
        <Box pad="small" gap="medium" margin="large">
          <Button
            label="Request Schedule Change"
            alignSelf="center"
            onClick={() => {
              setShowRequestScheduleChangeLayer(true);
            }}
            data-testid="requestScheduleChangeButton"
          />
          <Button
            label="Review Requests"
            alignSelf="center"
            onClick={() => {
              setShowRequestsLayer(true);
            }}
            data-testid="requestsButton"
          />
        </Box>
        {showRequestsLayer && (
          <Layer
            onEsc={() => setShowRequestsLayer(false)}
            onClickOutside={() => setShowRequestsLayer(false)}
            animation="fadeIn"
          >
            <Box pad="small" gap="medium" overflow="auto">
              <Box direction="row">
                <Heading level={3} margin="none">
                  Request Schedule Change Status
                </Heading>
                <Box alignSelf="end" margin={{ left: 'auto' }}>
                  <Button
                    icon={<Close />}
                    onClick={() => setShowRequestsLayer(false)}
                    margin="small"
                    hoverIndicator
                    data-testid="reviewRequestsCloseButton"
                  />
                </Box>
              </Box>
              <Box>
                <RequestViewer type="Schedule" />
              </Box>
            </Box>
          </Layer>
        )}
        {showRequestScheduleChangeLayer && (
          <Layer
            onEsc={() => setShowRequestScheduleChangeLayer(false)}
            onClickOutside={() => setShowRequestScheduleChangeLayer(false)}
          >
            <Grid
              fill
              rows={['auto', 'flex']}
              columns={{ count: 2, size: 'auto' }}
              gap="small"
            >
              <Box pad="small" gap="medium" width="medium">
                <Heading level={3} margin="none">
                  Create Schedule Change Request
                </Heading>
                <Box border="horizontal">
                  <Text>
                    Please fill in the timeslots of the respective dates that
                    conflict with your schedule
                  </Text>
                </Box>
                <Calendar
                  size="medium"
                  date={requestScheduleChangeDate.toString()}
                  daysOfWeek
                  onSelect={onSelectRequestScheduleChangeDate}
                  bounds={['2022-01-01', '2023-06-30']}
                />
                <TextArea
                  placeholder="Reason for schedule change"
                  value={requestScheduleChangeMessage}
                  onChange={(event) => {
                    setRequestScheduleChangeMessage(event.target.value);
                  }}
                  resize={false}
                  data-testid="requestScheduleChangeMessageTextArea"
                />
                <Box direction="row" alignSelf="center">
                  <Button
                    label="Close"
                    onClick={() => setShowRequestScheduleChangeLayer(false)}
                    margin="small"
                    data-testid="requestScheduleChangeCloseButton"
                  />
                  <Button
                    label="Submit"
                    onClick={() => {
                      submitRequestScheduleChange();
                    }}
                    margin="small"
                    data-testid="requestScheduleChangeSubmitButton"
                  />
                </Box>
              </Box>
              <Box pad="large" gap="medium" width="xlarge" height="large">
                <Box direction="row" gap="medium">
                  <Text alignSelf="center">
                    Week of {startDateRequestScheduleChange.toDateString()} to{' '}
                    {endDateRequestScheduleChange.toDateString()}
                  </Text>
                  <Box alignSelf="end" margin={{ left: 'auto' }}>
                    <Button
                      icon={<Close />}
                      onClick={() => setShowRequestScheduleChangeLayer(false)}
                      margin="small"
                      hoverIndicator
                      data-testid="requestScheduleChangeCloseXButton"
                    />
                  </Box>
                </Box>
                <TimeGridScheduler
                  classes={classes}
                  style={{ width: '100%', height: '100%' }}
                  originDate={startDateRequestScheduleChange}
                  onChange={(event) => {
                    event.forEach((item) => {
                      const timeDiff = Math.abs(
                        item[0].getTime() - item[1].getTime()
                      );
                      if (timeDiff < deleteThreshold) {
                        event.splice(event.indexOf(item), 1);
                      }
                    });
                    setSelectedTimeBlock(event);
                  }}
                  schedule={selectedTimeBlock}
                  defaultHours={[10, 20]}
                  visualGridVerticalPrecision={60}
                  verticalPrecision={15}
                  cellClickPrecision={60}
                />
              </Box>
            </Grid>
          </Layer>
        )}
      </Grid>
    </Box>
  );
}

export default Schedule;
