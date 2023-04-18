import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import {
  Box,
  Button,
  Stack,
  Meter,
  Grid,
  Text,
  Layer,
  Select,
  DateInput,
  Notification,
} from 'grommet';
import { TimeGridScheduler, classes } from '@remotelock/react-week-scheduler';
import { DateRange } from '@remotelock/react-week-scheduler/src/types';
import { Add, LinkPrevious, LinkNext } from 'grommet-icons';

import AuthTokenHelper from '../helpers/AuthTokenHelper';
import RestCallHelper from '../helpers/RestCallHelper';
import NetworkResources from '../resources/NetworkResources';

const defaultHourOptions: string[] = [];
for (let i = 6; i < 21; i += 1) {
  defaultHourOptions.push(`${i.toString()}:00`);
}
const prefix = 'Create';
const updateCreateOption = (option: string) => {
  const { length } = defaultHourOptions;
  if (defaultHourOptions[length - 1].includes(prefix)) {
    defaultHourOptions.pop();
  }
  if (!defaultHourOptions.includes(option)) {
    defaultHourOptions.push(`${prefix} '${option}'`);
  }
};
const getRegExp = (search: string) => {
  const escapedSearch = search.replace(/[-/\\^$*+?.()|[\]{}]/g, '\\$&');
  return new RegExp(escapedSearch, 'i');
};

function Timesheets() {
  const [currentDate, setCurrentDate] = useState(new Date());
  const [currentWeek, setCurrentWeek] = useState(new Date());
  const [date, setDate] = useState();
  const [logStartDate, setLogStartDate] = useState('9:00');
  const [logEndDate, setLogEndDate] = useState('17:00');
  const [logHoursTime, setLogHoursTime] = useState<string[][]>([]);
  const [showLogHours, setShowLogHours] = useState(false);
  const [hourOptions, setHourOptions] = useState(defaultHourOptions);
  const [searchHour, setSearchHour] = useState('');
  const [logHoursNotification, setLogHoursNotification] = useState(false);
  const [addTimeslotNotification, setAddTimeslotNotification] = useState(false);
  const [invalidLogHours, setInvalidLogHours] = useState(false);

  const [hoursSunday, setHoursSunday] = useState(0);
  const [hoursMonday, setHoursMonday] = useState(0);
  const [hoursTuesday, setHoursTuesday] = useState(0);
  const [hoursWednesday, setHoursWednesday] = useState(0);
  const [hoursThursday, setHoursThursday] = useState(0);
  const [hoursFriday, setHoursFriday] = useState(0);
  const [hoursSaturday, setHoursSaturday] = useState(0);

  const initialWeekState: DateRange[] = [];
  const deleteThreshold = 20 * 60 * 1000;
  const [timesheet, setTimesheet] = useState(initialWeekState);
  const [timesheetActiveWeek, setTimesheetActiveWeek] = useState(new Date());
  const [timesheetStartDate, setTimesheetStartDate] = useState(new Date());
  const [timesheetEndDate, setTimesheetEndDate] = useState(new Date());
  const [weeklyHours, setWeeklyHours] = useState<any[]>([]);
  const [showTimesheetConfirmation, setShowTimesheetConfirmation] =
    useState(false);
  const [timesheetSubmissionStatus, setTimesheetSubmissionStatus] =
    useState('Draft');
  const [timesheetSubmissionNotification, setTimesheetSubmissionNotification] =
    useState(false);

  const hoursLogged =
    hoursSunday +
    hoursMonday +
    hoursTuesday +
    hoursWednesday +
    hoursThursday +
    hoursFriday +
    hoursSaturday;
  const hoursPercentage = (hoursLogged / 40) * 100;

  const onChangeDate = (event: any) => {
    const nextDate = event.value;
    setCurrentDate(new Date(nextDate));
  };
  const appendHoursTime = (start: string, end: string) => {
    const arrayHours = logHoursTime;
    arrayHours.push([start, end]);
    setLogHoursTime(arrayHours);
  };
  const afterLogHoursSubmit = () => {
    setLogHoursNotification(true);
    setLogHoursTime([]);
    setLogStartDate('9:00');
    setLogEndDate('17:00');
    setTimeout(() => {
      setLogHoursNotification(false);
      setInvalidLogHours(false);
    }, 3000);
  };
  const onLogHoursSubmit = async () => {
    let start;
    let end;
    let submission = [];
    for (let i = 0; i < logHoursTime.length; i += 1) {
      start = new Date(
        currentDate.getFullYear(),
        currentDate.getMonth(),
        currentDate.getDate(),
        parseInt(logHoursTime[i][0].split(':')[0], 10),
        parseInt(logHoursTime[i][0].split(':')[1], 10)
      );
      end = new Date(
        currentDate.getFullYear(),
        currentDate.getMonth(),
        currentDate.getDate(),
        parseInt(logHoursTime[i][1].split(':')[0], 10),
        parseInt(logHoursTime[i][1].split(':')[1], 10)
      );
      submission.push({
        Date: currentDate,
        StartTime: start,
        EndTime: end,
      });
    }
    for (let i = 0; i < submission.length; i += 1) {
      if (!invalidLogHours) {
        for (let j = 0; j < submission.length; j += 1) {
          if (i !== j) {
            if (
              submission[i].StartTime.getTime() <
                submission[j].EndTime.getTime() &&
              submission[i].EndTime.getTime() >
                submission[j].StartTime.getTime()
            ) {
              setInvalidLogHours(true);
              submission = [];
              break;
            }
          }
        }
      }
    }
    const token = AuthTokenHelper.getFromLocalStorage();
    const logHoursUrl = `${NetworkResources.API_URL}/Timesheet/LogHours`;
    const logHoursParams = JSON.stringify({
      sessionToken: token,
    });
    const logHoursBody = JSON.stringify(submission);
    if (submission.length > 0) {
      const { status } = await RestCallHelper.PutRequest(
        logHoursUrl,
        logHoursParams,
        logHoursBody
      );
      if (status === 400) {
        setInvalidLogHours(true);
      }
    }
    afterLogHoursSubmit();
    setShowLogHours(false);
  };

  const appendHoursAndSubmit = () => {
    appendHoursTime(logStartDate, logEndDate);
    onLogHoursSubmit();
  };

  const appendHoursAndAdd = () => {
    appendHoursTime(logStartDate, logEndDate);
    setLogStartDate('9:00');
    setLogEndDate('17:00');
    setAddTimeslotNotification(true);
    setTimeout(() => {
      setAddTimeslotNotification(false);
    }, 1000);
  };

  // for checking specific logged days
  // const getLoggedHours = async () => {
  //   const token = AuthTokenHelper.getFromLocalStorage();
  //   const logHoursUrl = `${NetworkResources.API_URL}/Timesheet/GetHours`;
  //   const ddate = new Date(2022, 10, 23);
  //   const logHoursParams = JSON.stringify({
  //     day: ddate,
  //     sessionToken: token,
  //   });
  //   const { data, status } = await RestCallHelper.GetRequest(
  //     logHoursUrl,
  //     logHoursParams
  //   );
  // };

  const convertLogDayIntoHours = (day: any) => {
    let totalHours = 0;
    for (let i = 0; i < day.length; i += 1) {
      const start = new Date(day[i].startTime);
      const end = new Date(day[i].endTime);
      const time = (end.getTime() - start.getTime()) / 1000 / 60 / 60;
      totalHours += time;
    }
    return totalHours;
  };

  const onCloseAndReset = () => {
    setShowLogHours(false);
    setLogHoursTime([]);
    setLogStartDate('9:00');
    setLogEndDate('17:00');
  };

  const parseTimesheetData = (data: any) => {
    const parsedData: DateRange[] = [];
    for (let i = 0; i < data.length; i += 1) {
      if (data[i].timeslots.length > 0) {
        for (let j = 0; j < data[i].timeslots.length; j += 1) {
          const start = new Date(data[i].timeslots[j].startTime);
          const end = new Date(data[i].timeslots[j].endTime);
          parsedData.push([start, end]);
        }
      }
    }
    setTimesheet(parsedData);
  };

  const getWeeklyTimesheet = async (week?: Date) => {
    if (week !== undefined) {
      setCurrentWeek(week);
    }
    const token = AuthTokenHelper.getFromLocalStorage();
    const logHoursUrl = `${NetworkResources.API_URL}/Timesheet/GetTimeSheet`;
    const logHoursParams = JSON.stringify({
      date: currentWeek,
      sessionToken: token,
    });
    const { data } = await RestCallHelper.GetRequest(
      logHoursUrl,
      logHoursParams
    );
    let count = 0;
    let arr;
    try {
      arr = JSON.parse(data);
    } catch (e) {
      arr = [];
    }
    const timeForWeek = [];
    for (let i = 0; i < arr.length; i += 1) {
      if (arr[i].length > 0) {
        for (let j = 0; j < arr[i].length; j += 1) {
          setTimesheetSubmissionStatus(arr[i][j].status);
        }
      } else {
        count += 1;
      }
      if (count === 7) {
        setTimesheetSubmissionStatus('Draft');
      }
      // eslint-disable-next-line default-case
      switch (i) {
        case 0:
          setHoursSunday(convertLogDayIntoHours(arr[i]));
          timeForWeek.push({
            day: 'Sunday',
            id: 0,
            hours: hoursSunday,
            timeslots: arr[i],
          });
          break;
        case 1:
          setHoursMonday(convertLogDayIntoHours(arr[i]));
          timeForWeek.push({
            day: 'Monday',
            id: 1,
            hours: hoursMonday,
            timeslots: arr[i],
          });
          break;
        case 2:
          setHoursTuesday(convertLogDayIntoHours(arr[i]));
          timeForWeek.push({
            day: 'Tuesday',
            id: 2,
            hours: hoursTuesday,
            timeslots: arr[i],
          });
          break;
        case 3:
          setHoursWednesday(convertLogDayIntoHours(arr[i]));
          timeForWeek.push({
            day: 'Wednesday',
            id: 3,
            hours: hoursWednesday,
            timeslots: arr[i],
          });
          break;
        case 4:
          setHoursThursday(convertLogDayIntoHours(arr[i]));
          timeForWeek.push({
            day: 'Thursday',
            id: 4,
            hours: hoursThursday,
            timeslots: arr[i],
          });
          break;
        case 5:
          setHoursFriday(convertLogDayIntoHours(arr[i]));
          timeForWeek.push({
            day: 'Friday',
            id: 5,
            hours: hoursFriday,
            timeslots: arr[i],
          });
          break;
        case 6:
          setHoursSaturday(convertLogDayIntoHours(arr[i]));
          timeForWeek.push({
            day: 'Saturday',
            id: 6,
            hours: hoursSaturday,
            timeslots: arr[i],
          });
          break;
      }
    }
    setWeeklyHours(timeForWeek);
    parseTimesheetData(timeForWeek);
  };

  const afterTimesheetSubmission = () => {
    setTimesheetSubmissionStatus('');
    setTimesheetSubmissionNotification(true);
    setTimeout(() => {
      setTimesheetSubmissionNotification(false);
    }, 2000);
  };

  const getBoundsOfWeek = (week: Date) => {
    const start = new Date(week);
    start.setDate(start.getDate() - start.getDay());
    setTimesheetStartDate(start);
    const end = new Date(start);
    end.setDate(end.getDate() + 6);
    setTimesheetEndDate(end);
    return { start, end };
  };

  const changeTimesheetWeek = (timesheetWeek: Date) => {
    setTimesheetActiveWeek(timesheetWeek);
    getBoundsOfWeek(timesheetWeek);
    getWeeklyTimesheet(timesheetWeek);
  };

  const submitTimesheet = async () => {
    let start;
    let end;
    let day;
    const submission = [];
    for (let i = 0; i < timesheet.length; i += 1) {
      start = new Date(
        timesheet[i][0].getFullYear(),
        timesheet[i][0].getMonth(),
        timesheet[i][0].getDate(),
        timesheet[i][0].getHours(),
        timesheet[i][0].getMinutes()
      );
      end = new Date(
        timesheet[i][1].getFullYear(),
        timesheet[i][1].getMonth(),
        timesheet[i][1].getDate(),
        timesheet[i][1].getHours(),
        timesheet[i][1].getMinutes()
      );
      day = new Date(
        timesheet[i][0].getFullYear(),
        timesheet[i][0].getMonth(),
        timesheet[i][0].getDate()
      );
      submission.push({
        Date: day,
        StartTime: start,
        EndTime: end,
      });
    }
    const token = AuthTokenHelper.getFromLocalStorage();
    const submitTimesheetUrl = `${NetworkResources.API_URL}/Timesheet/SubmitTime`;
    const submitTimesheetParams = JSON.stringify({
      sessionToken: token,
    });
    const submitTimesheetBody = JSON.stringify(submission);
    await RestCallHelper.PutRequest(
      submitTimesheetUrl,
      submitTimesheetParams,
      submitTimesheetBody
    );
    getWeeklyTimesheet(day);
    setShowTimesheetConfirmation(false);
    afterTimesheetSubmission();
  };

  const navigate = useNavigate();
  useEffect(() => {
    const validToken = AuthTokenHelper.validateToken();
    if (!validToken) {
      navigate('/login');
    }
  }, [navigate]);
  useEffect(() => {
    getWeeklyTimesheet();
    changeTimesheetWeek(new Date());
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);
  return (
    // <Grommet full>
    <Box>
      <h1>&nbsp;&nbsp;Timesheets</h1>
      <Grid
        rows={['auto', 'flex']}
        columns={{ count: 2, size: ['medium', 'large'] }}
      >
        <Box align="center" gap="small" pad="small" height="80vh" width="50%">
          <Text
            size="large"
            weight="bold"
            alignSelf="center"
            margin={{ top: 'xsmall' }}
          >
            Weekly Hours
          </Text>
          <Stack anchor="center">
            <Meter
              type="circle"
              background="light-2"
              values={[{ value: hoursPercentage }]}
              size="small"
              thickness="large"
              aria-label="meter"
              data-testid="hoursMeter"
            />
            <Box direction="row" pad={{ bottom: 'xsmall' }}>
              <Text size="large" weight="bold" alignSelf="center">
                {`${hoursPercentage.toString().slice(0, 5)} %`}
              </Text>
            </Box>
          </Stack>
          <Box direction="row" pad={{ bottom: 'xsmall' }}>
            <Text size="large" weight="bold" alignSelf="center">
              {hoursLogged.toString().slice(0, 5)} Hours Logged
            </Text>
          </Box>
          <Box>
            <br />
          </Box>
          {timesheetSubmissionStatus === 'Approved' ||
          timesheetSubmissionStatus === 'Submitted' ? (
            <Button label="Timesheet already submitted" primary disabled />
          ) : (
            <Button
              label="Log Hours"
              primary
              onClick={() => setShowLogHours(!showLogHours)}
            />
          )}

          <Box margin={{ top: 'large' }} gap="small" align="center">
            <Box
              direction="row"
              align="center"
              justify="center"
              pad={{ bottom: 'xsmall' }}
            >
              <Text
                size="large"
                weight="bold"
                alignSelf="center"
                margin={{ top: 'xsmall' }}
              >
                Timesheet Status: &nbsp;
              </Text>
              <Text
                size="large"
                weight="bold"
                color={
                  // eslint-disable-next-line no-nested-ternary
                  timesheetSubmissionStatus === 'Approved'
                    ? 'green'
                    : timesheetSubmissionStatus === 'Submitted'
                    ? 'blue'
                    : 'orange'
                }
                alignSelf="center"
                margin={{ top: 'xsmall' }}
              >
                {timesheetSubmissionStatus}
              </Text>
            </Box>
            {timesheetSubmissionStatus === 'Approved' ? (
              <Button label="Timesheet Approved" primary disabled />
            ) : (
              <Button
                label="Submit Timesheet"
                primary
                onClick={() => {
                  setShowTimesheetConfirmation(true);
                }}
              />
            )}
          </Box>
        </Box>
        <Box
          align="center"
          gap="small"
          round="small"
          pad="small"
          height="80vh"
          width="80%"
        >
          <Box direction="row" gap="medium" alignSelf="center">
            <Button
              label="Previous Week"
              alignSelf="center"
              icon={<LinkPrevious />}
              onClick={() => {
                timesheetActiveWeek.setDate(timesheetActiveWeek.getDate() - 7);
                changeTimesheetWeek(timesheetActiveWeek);
              }}
              data-testid="previousWeekButton"
            />
            <Text alignSelf="center">
              Week of {timesheetStartDate.toDateString()} to{' '}
              {timesheetEndDate.toDateString()}
            </Text>
            <Button
              label="Next Week"
              alignSelf="center"
              icon={<LinkNext />}
              reverse
              onClick={() => {
                timesheetActiveWeek.setDate(timesheetActiveWeek.getDate() + 7);
                changeTimesheetWeek(timesheetActiveWeek);
              }}
              data-testid="nextWeekButton"
            />
          </Box>
          <TimeGridScheduler
            classes={classes}
            style={{ width: '70vw', height: '100%' }}
            originDate={timesheetStartDate}
            onChange={(event) => {
              event.forEach((item) => {
                const timeDiff = Math.abs(
                  item[0].getTime() - item[1].getTime()
                );
                if (timeDiff < deleteThreshold) {
                  event.splice(event.indexOf(item), 1);
                }
              });
              setTimesheet(event);
            }}
            schedule={timesheet}
            defaultHours={[10, 20]}
            visualGridVerticalPrecision={60}
            verticalPrecision={15}
            cellClickPrecision={60}
          />
        </Box>
      </Grid>
      {logHoursNotification && !invalidLogHours && (
        <Notification
          toast
          status="normal"
          title="Hours Logged"
          message="Log Hours Successfully"
          onClose={() => setLogHoursNotification(false)}
        />
      )}
      {logHoursNotification && invalidLogHours && (
        <Notification
          toast
          status="warning"
          title="Invalid Submission"
          message="Hours Overlapping in Submission or logged hours for this date has already been submitted"
          onClose={() => {
            setLogHoursNotification(false);
            setInvalidLogHours(false);
          }}
        />
      )}
      {addTimeslotNotification && (
        <Notification
          toast
          status="normal"
          title="Timeslot Added"
          message="Timeslot Added Successfully"
          onClose={() => setAddTimeslotNotification(false)}
        />
      )}
      {timesheetSubmissionNotification && (
        <Notification
          toast
          status="normal"
          title="Timesheet Submission"
          message="Timesheet Submitted Successfully"
          onClose={() => setTimesheetSubmissionNotification(false)}
        />
      )}
      {showLogHours && (
        <Layer
          position="center"
          modal
          onClickOutside={() => setShowLogHours(false)}
          onEsc={() => setShowLogHours(false)}
        >
          <Box pad="medium" gap="small" width="medium">
            <Box
              direction="row"
              justify="between"
              align="center"
              pad={{ vertical: 'small' }}
            >
              <Text size="large" weight="bold">
                Log Hours Period
              </Text>
              <Button
                icon={<Add />}
                primary
                onClick={() => {
                  appendHoursAndAdd();
                }}
                data-testid="addHoursButton"
                tip={{
                  plain: true,
                  dropProps: { align: { bottom: 'top' } },
                  content: (
                    <Box
                      pad="xxsmall"
                      elevation="small"
                      background="#EDEDED"
                      round="xsmall"
                      margin="xsmall"
                      overflow="hidden"
                      align="center"
                    >
                      <svg
                        viewBox="0 0 22 22"
                        version="1.1"
                        width="22px"
                        height="6px"
                      >
                        <polygon transform="matrix(-1 0 0 1 30 0)" />
                      </svg>
                      <Box direction="row" pad="small" round="xsmall">
                        <Text>
                          This will add the current timeslot selected and
                          refresh the current timeslot to add
                        </Text>
                      </Box>
                    </Box>
                  ),
                }}
              />
            </Box>
            <DateInput
              format="dd/mm/yyyy"
              value={date}
              onChange={onChangeDate}
              data-testid="calendarDateInput"
            />
            <Select
              size="medium"
              placeholder="Select Start Date"
              options={hourOptions}
              value={logStartDate}
              defaultValue="9:00"
              data-testid="startDateSelect"
              onChange={({ option }) => {
                if (option.includes(prefix)) {
                  defaultHourOptions.pop();
                  defaultHourOptions.push(searchHour);
                  setLogStartDate(searchHour);
                } else {
                  setLogStartDate(option);
                }
              }}
              onClose={() => setHourOptions(defaultHourOptions)}
              onSearch={(text: string) => {
                updateCreateOption(text);
                const exp = getRegExp(text);
                setHourOptions(
                  defaultHourOptions.filter((o) => exp.test(o.toString()))
                );
                setSearchHour(text);
              }}
            />
            <Select
              size="medium"
              placeholder="Select End Date"
              options={hourOptions}
              value={logEndDate}
              defaultValue="17:00"
              data-testid="endDateSelect"
              onChange={({ option }) => {
                if (option.includes(prefix)) {
                  defaultHourOptions.pop();
                  defaultHourOptions.push(searchHour);
                  setLogEndDate(searchHour);
                } else {
                  setLogEndDate(option);
                }
              }}
              onClose={() => setHourOptions(defaultHourOptions)}
              onSearch={(text: string) => {
                updateCreateOption(text);
                const exp = getRegExp(text);
                setHourOptions(
                  defaultHourOptions.filter((o) => exp.test(o.toString()))
                );
                setSearchHour(text);
              }}
            />
            <Box
              direction="row"
              gap="small"
              justify="center"
              align="center"
              pad="small"
            >
              <Button
                label="Log"
                primary
                onClick={() => appendHoursAndSubmit()}
              />
              <Button label="Close" primary onClick={() => onCloseAndReset()} />
            </Box>
          </Box>
        </Layer>
      )}
      {showTimesheetConfirmation && (
        <Layer
          position="center"
          modal
          onClickOutside={() => setShowTimesheetConfirmation(false)}
          onEsc={() => setShowTimesheetConfirmation(false)}
        >
          <Box pad="medium" gap="small" width="medium">
            <Box
              direction="row"
              justify="between"
              align="center"
              pad={{ vertical: 'small' }}
            >
              <Text size="large" weight="bold">
                Are you sure you want to submit this week&apos;s timesheet?
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
                label="Submit"
                primary
                onClick={() => submitTimesheet()}
              />
              <Button
                label="Close"
                primary
                onClick={() => setShowTimesheetConfirmation(false)}
              />
            </Box>
          </Box>
        </Layer>
      )}
    </Box>
    // </Grommet>
  );
}

export default Timesheets;
