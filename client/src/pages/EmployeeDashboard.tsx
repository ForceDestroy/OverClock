import { useState, useEffect } from 'react';

import { useNavigate } from 'react-router-dom';

import {
  Text,
  Box,
  Grid,
  Button,
  Layer,
  DataTable,
  Heading,
  TextArea,
  TextInput,
} from 'grommet';

import AuthTokenHelper from '../helpers/AuthTokenHelper';
import UIResources from '../resources/UIResources';
import NetworkResources from '../resources/NetworkResources';
import RestCallHelper from '../helpers/RestCallHelper';

function EmployeeDashboard() {
  const navigate = useNavigate();

  // Fields for Announcements
  const [announcements, setAnnouncements] = useState([]);
  const [showAnnouncement, setShowAnnouncement] = useState(false);
  const [activeAnnouncementTitle, setActiveAnnouncementTitle] = useState('');
  const [activeAnnouncement, setActiveAnnouncement] = useState('');
  const [activeAnnouncementDate, setActiveAnnouncementDate] = useState('');
  const [activeAnnouncementId, setActiveAnnouncementId] = useState(0);
  const [activeAnnouncementAuthor, setActiveAnnouncementAuthor] = useState('');
  const announcementsColumns = [
    {
      property: 'title',
      header: 'Announcements',
      render: (data: any) => <Text>{data.Title}</Text>,
    },
  ];

  // Rest Call for announcements
  const getAnnouncements = async () => {
    const url = `${NetworkResources.API_URL}/Bulletin/GetAllAnnouncements`;
    const token = AuthTokenHelper.getFromLocalStorage();
    const params = JSON.stringify({
      sessionToken: token,
    });

    const { data } = await RestCallHelper.PostRequest(url, params, '', true);
    if (data === '') return;
    setAnnouncements(JSON.parse(data));
  };

  const deleteAnnouncement = async () => {
    const url = `${NetworkResources.API_URL}/Bulletin/DeleteAnnouncement`;
    const token = AuthTokenHelper.getFromLocalStorage();
    const params = JSON.stringify({
      sessionToken: token,
      announcementId: activeAnnouncementId,
    });
    const { data } = await RestCallHelper.PostRequest(url, params, '', true);

    getAnnouncements();
  };

  // Fields for training modules
  const [trainingModules, setTrainingModules] = useState([]);
  const [showTrainingModule, setShowTrainingModule] = useState(false);
  const [showCreateTrainingModal, setShowCreateTrainingModal] = useState(false);
  const [newTrainingModuleTitle, setNewTrainingModuleTitle] = useState('');
  const [newTrainingModuleBody, setNewTrainingModuleBody] = useState('');

  const [activeTrainingTitle, setActiveTrainingTitle] = useState('');
  const [activeTraining, setActiveTraining] = useState('');
  const [activeTrainingDate, setActiveTrainingDate] = useState('');
  const [activeTrainingId, setActiveTrainingId] = useState(0);
  const [activeTrainingStatus, setActiveTrainingStatus] = useState('');

  const [showTrainingModuleStatus, setShowTrainingModuleStatus] =
    useState(false);
  const [activeTrainingStatuses, setActiveTrainingStatuses] = useState([]);
  const trainingColumns = [
    {
      property: 'title',
      header: 'Trainings',
      render: (data: any) => (
        <Text>
          [{data.Id}] {data.Title}
        </Text>
      ),
    },
  ];
  const trainingStatusColumn = [
    {
      property: 'id',
      header: 'Employee ID',
      render: (data: any) => <Text>{data.EmployeeId}</Text>,
    },
    {
      property: 'name',
      header: 'Name',
      render: (data: any) => <Text>{data.EmployeeName}</Text>,
    },
    {
      property: 'status',
      header: 'Status',
      render: (data: any) => (
        <Text color={data.Status !== 'Completed' ? 'Red' : 'Green'}>
          {data.Status}
        </Text>
      ),
    },
  ];

  // Rest Call for training modules
  const getTrainingModules = async () => {
    const url = `${NetworkResources.API_URL}/Module/GetAllModulesForAnEmployee`;
    const token = AuthTokenHelper.getFromLocalStorage();
    const params = JSON.stringify({
      sessionToken: token,
    });

    const { data } = await RestCallHelper.PostRequest(url, params, '', true);
    if (data === '') return;
    setTrainingModules(JSON.parse(data));
  };

  const getAllTrainingModules = async () => {
    const url = `${NetworkResources.API_URL}/Module/GetAllModulesForEmployer`;
    const token = AuthTokenHelper.getFromLocalStorage();
    const params = JSON.stringify({
      sessionToken: token,
    });

    const { data } = await RestCallHelper.PostRequest(url, params, '', true);
    if (data === '') return;
    setTrainingModules(JSON.parse(data));
  };

  const createTrainingModule = async () => {
    const url = `${NetworkResources.API_URL}/Module/CreateModule`;
    const token = AuthTokenHelper.getFromLocalStorage();
    const moduleInfo = JSON.stringify({
      Title: newTrainingModuleTitle,
      Body: newTrainingModuleBody,
    });
    const params = JSON.stringify({
      sessionToken: token,
      moduleInfo,
    });
    await RestCallHelper.PostRequest(url, params, '', true);
    getAllTrainingModules();
  };

  const deleteTrainingModule = async () => {
    const url = `${NetworkResources.API_URL}/Module/DeleteModule`;
    const token = AuthTokenHelper.getFromLocalStorage();
    const moduleId = activeTrainingId;
    const params = JSON.stringify({
      sessionToken: token,
      moduleId,
    });
    await RestCallHelper.DeleteRequest(url, params, true);
    getAllTrainingModules();
  };

  const updateTrainingModule = async (id: number, status: string) => {
    const url = `${NetworkResources.API_URL}/Module/UpdateModuleStatusEmployee`;
    const token = AuthTokenHelper.getFromLocalStorage();
    const params = JSON.stringify({
      sessionToken: token,
      moduleStatusId: id,
      status,
    });
    await RestCallHelper.PostRequest(url, params, '', true);
    getTrainingModules();
  };

  useEffect(() => {
    const validToken = AuthTokenHelper.validateToken();
    if (!validToken) {
      navigate('/login');
    }
    getAnnouncements();
    if (AuthTokenHelper.getAccessLevel() === '1') {
      getAllTrainingModules();
    } else {
      getTrainingModules();
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [navigate]);
  return (
    <Box fill>
      <h1>&nbsp;&nbsp;Mode Durgaa</h1>
      <Grid
        rows={['auto', 'flex']}
        columns={{ count: 4, size: 'auto' }}
        margin={{ left: 'medium', right: 'medium' }}
        gap="small"
      >
        <Box
          background={UIResources.PALETTE.SECONDARY}
          pad="medium"
          round="small"
          border={{ color: UIResources.PALETTE.BORDER, size: 'small' }}
          fill
        >
          <DataTable
            columns={announcementsColumns}
            size="xlarge"
            data={announcements}
            step={10}
            onClickRow={(event) => {
              setActiveAnnouncementTitle(event.datum.Title);
              setActiveAnnouncement(event.datum.Body);
              setActiveAnnouncementDate(event.datum.Date);
              setActiveAnnouncementId(event.datum.Id);
              setActiveAnnouncementAuthor(event.datum.PosterName);
              setShowAnnouncement(true);
            }}
            data-testid="employeeDataTable"
          />
          {showAnnouncement && (
            <Layer
              position="center"
              onClickOutside={() => setShowAnnouncement(false)}
            >
              <Box pad="medium" gap="xsmall">
                <Text size="xlarge" weight="bold" alignSelf="center">
                  {activeAnnouncementTitle}
                </Text>
                <Text size="medium" alignSelf="center">
                  {activeAnnouncement}
                </Text>
                <Text size="medium" weight="bold" alignSelf="center">
                  {activeAnnouncementAuthor}
                </Text>
                <Text size="medium" weight="bold" alignSelf="center">
                  {activeAnnouncementDate.substring(0, 10)}
                </Text>
                <Box direction="row" alignSelf="center">
                  <Button
                    label="Close"
                    onClick={() => setShowAnnouncement(false)}
                    margin="small"
                    data-testid="announcementCloseButton"
                  />
                  {AuthTokenHelper.getAccessLevel() === '1' && (
                    <Button
                      label="Delete Announcement"
                      color="red"
                      onClick={() => {
                        setShowAnnouncement(false);
                        deleteAnnouncement();
                      }}
                      margin="small"
                      data-testid="announcementDeleteButton"
                    />
                  )}
                </Box>
              </Box>
            </Layer>
          )}
        </Box>
        <Box gap="small" height="80vh">
          <Box
            background={UIResources.PALETTE.SECONDARY}
            pad="medium"
            round="small"
            gap="medium"
            border={{ color: UIResources.PALETTE.BORDER, size: 'small' }}
            fill
          >
            <Text size="large" alignSelf="center">
              Log your daily hours
            </Text>
            <Box direction="row" justify="center" align="center" gap="xlarge">
              <Button
                label={
                  <Box width="10em">
                    <br />
                    <Text color="white" weight="bold">
                      Log Hours
                    </Text>
                    <br />
                  </Box>
                }
                onClick={() => navigate('/EmployeeDashboard/Timesheets')}
                color={UIResources.PALETTE.GRADIENT}
                primary
                data-testid="logHoursButton"
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
              View your time off
            </Text>
            <Box direction="row" justify="center" align="center" gap="xlarge">
              <Button
                label={
                  <Box width="10em">
                    <br />
                    <Text color="white" weight="bold">
                      View Time Off
                    </Text>
                    <br />
                  </Box>
                }
                onClick={() => navigate('/EmployeeDashboard/TimeOff')}
                color={UIResources.PALETTE.GRADIENT}
                primary
                data-testid="viewTimeOffButton"
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
              Access your payslips
            </Text>
            <Box direction="row" justify="center" align="center" gap="xlarge">
              <Button
                label={
                  <Box width="10em">
                    <br />
                    <Text color="white" weight="bold">
                      Access Payslips
                    </Text>
                    <br />
                  </Box>
                }
                onClick={() => navigate('/EmployeeDashboard/Payslip')}
                color={UIResources.PALETTE.GRADIENT}
                primary
                data-testid="accessPayslipsButton"
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
              Send a custom request
            </Text>
            <Box direction="row" justify="center" align="center" gap="xlarge">
              <Button
                label={
                  <Box width="10em">
                    <br />
                    <Text color="white" weight="bold">
                      Send Request
                    </Text>
                    <br />
                  </Box>
                }
                onClick={() => navigate('/EmployeeDashboard/TimeOff')}
                color={UIResources.PALETTE.GRADIENT}
                primary
                data-testid="sendRequestButton"
              />
            </Box>
          </Box>
        </Box>
        <Box gap="small" height="80vh">
          <Box
            background={UIResources.PALETTE.SECONDARY}
            pad="medium"
            round="small"
            gap="medium"
            border={{ color: UIResources.PALETTE.BORDER, size: 'small' }}
            fill
          >
            <Text size="large" alignSelf="center">
              View your schedule
            </Text>
            <Box direction="row" justify="center" align="center" gap="xlarge">
              <Button
                label={
                  <Box width="10em">
                    <br />
                    <Text color="white" weight="bold">
                      View Schedule
                    </Text>
                    <br />
                  </Box>
                }
                onClick={() => navigate('/EmployeeDashboard/Schedule')}
                color={UIResources.PALETTE.GRADIENT}
                primary
                data-testid="viewScheduleButton"
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
              View company policies and info
            </Text>
            <Box direction="row" justify="center" align="center" gap="xlarge">
              <Button
                label={
                  <Box width="10em">
                    <br />
                    <Text color="white" weight="bold">
                      View Policies
                    </Text>
                    <br />
                  </Box>
                }
                onClick={() => navigate('/EmployeeDashboard/CompanyPolicy')}
                color={UIResources.PALETTE.GRADIENT}
                primary
                data-testid="viewPoliciesButton"
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
              Access the job referal program
            </Text>
            <Box direction="row" justify="center" align="center" gap="xlarge">
              <Button
                label={
                  <Box width="10em">
                    <br />
                    <Text color="white" weight="bold">
                      Access Referals
                    </Text>
                    <br />
                  </Box>
                }
                onClick={() => navigate('/EmployeeDashboard')}
                color={UIResources.PALETTE.GRADIENT}
                primary
                data-testid="accessReferalsButton"
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
              View your profile
            </Text>
            <Box direction="row" justify="center" align="center" gap="xlarge">
              <Button
                label={
                  <Box width="10em">
                    <br />
                    <Text color="white" weight="bold">
                      View Profile
                    </Text>
                    <br />
                  </Box>
                }
                onClick={() => navigate('/ProfilePage')}
                color={UIResources.PALETTE.GRADIENT}
                primary
                data-testid="viewProfileButton"
              />
            </Box>
          </Box>
        </Box>
        <Box
          background={UIResources.PALETTE.SECONDARY}
          pad="medium"
          round="small"
          border={{ color: UIResources.PALETTE.BORDER, size: 'small' }}
        >
          {AuthTokenHelper.getAccessLevel() === '0' && (
            <DataTable
              columns={trainingColumns}
              size="xlarge"
              data={trainingModules}
              step={10}
              onClickRow={(event) => {
                setActiveTrainingTitle(event.datum.Title);
                setActiveTraining(event.datum.Body);
                setActiveTrainingDate(event.datum.Date);
                setActiveTrainingId(event.datum.Status.Id);
                setActiveTrainingStatus(event.datum.Status.Status);
                if (event.datum.Status.Status === 'Unseen')
                  updateTrainingModule(event.datum.Status.Id, 'Pending');
                setShowTrainingModule(true);
              }}
            />
          )}

          {showTrainingModule && (
            <Layer
              position="center"
              onClickOutside={() => setShowTrainingModule(false)}
            >
              <Box width="large" pad="medium" gap="xsmall">
                <Text size="xlarge" weight="bold" alignSelf="center">
                  {activeTrainingTitle}
                </Text>
                <Text size="medium" alignSelf="center">
                  {activeTraining}
                </Text>
                <Text
                  size="medium"
                  weight="bold"
                  alignSelf="center"
                  color={activeTrainingStatus === 'Pending' ? 'Red' : 'Green'}
                >
                  {activeTrainingStatus === 'Unseen'
                    ? 'Pending'
                    : activeTrainingStatus}
                </Text>
                <Text size="medium" weight="bold" alignSelf="center">
                  {activeTrainingDate.substring(0, 10)}
                </Text>
                <Box direction="row" alignSelf="center">
                  <Button
                    label="Close"
                    onClick={() => setShowTrainingModule(false)}
                    margin="small"
                  />
                  <Button
                    label="Mark as Complete"
                    onClick={() => {
                      updateTrainingModule(activeTrainingId, 'Completed');
                      setShowTrainingModule(false);
                    }}
                    margin="small"
                  />
                </Box>
              </Box>
            </Layer>
          )}
          <Box align="center">
            {AuthTokenHelper.getAccessLevel() === '1' && (
              <Box>
                <DataTable
                  columns={trainingColumns}
                  size="xlarge"
                  data={trainingModules}
                  step={10}
                  onClickRow={(event) => {
                    setActiveTrainingTitle(event.datum.Title);
                    setActiveTrainingId(event.datum.Id);
                    setActiveTraining(event.datum.Body);
                    setActiveTrainingStatuses(event.datum.Statuses);
                    setShowTrainingModuleStatus(true);
                  }}
                />
                <Button
                  label={
                    <Box width="15em">
                      <br />
                      <Text color="white" weight="bold">
                        Create Training Modules
                      </Text>
                      <br />
                    </Box>
                  }
                  onClick={() => {
                    setNewTrainingModuleTitle('');
                    setNewTrainingModuleBody('');
                    setShowCreateTrainingModal(true);
                  }}
                  color={UIResources.PALETTE.GRADIENT}
                  primary
                  data-testid="showCreateTrainingModalButton"
                />
              </Box>
            )}
          </Box>
          {showCreateTrainingModal && (
            <Layer
              position="center"
              onClickOutside={() => setShowCreateTrainingModal(false)}
            >
              <Box pad="medium" gap="small">
                <Heading level={2} margin="none">
                  Create a new training module
                </Heading>
                <Box>
                  <Text>
                    Please fill in the text box with any training you would like
                    all employees to complete.
                  </Text>
                </Box>
                <TextInput
                  placeholder="Enter the training title here."
                  value={newTrainingModuleTitle}
                  onChange={(event) => {
                    setNewTrainingModuleTitle(event.target.value);
                  }}
                  data-testid="customRequestReason"
                />
                <TextArea
                  placeholder="Enter the training details here."
                  value={newTrainingModuleBody}
                  onChange={(event) => {
                    setNewTrainingModuleBody(event.target.value);
                  }}
                  resize={false}
                  data-testid="customRequestReason"
                />
                <Box direction="row" alignSelf="center">
                  <Button
                    label="Close"
                    onClick={() => {
                      setShowCreateTrainingModal(false);
                    }}
                    margin="small"
                    data-testid="newAnnouncementCloseButton"
                  />
                  <Button
                    label="Submit"
                    onClick={() => {
                      createTrainingModule();
                      setShowCreateTrainingModal(false);
                    }}
                    margin="small"
                    data-testid="newAnnouncementSubmitButton"
                  />
                </Box>
              </Box>
            </Layer>
          )}
          {showTrainingModuleStatus && (
            <Layer
              position="center"
              onClickOutside={() => setShowTrainingModuleStatus(false)}
            >
              <Box width="large" pad="medium" gap="small" align="center">
                <Heading level={2} margin="none">
                  {activeTrainingTitle}
                </Heading>
                <Text>{activeTraining}</Text>
                <DataTable
                  columns={trainingStatusColumn}
                  size="xlarge"
                  data={activeTrainingStatuses}
                  step={10}
                  data-testid="employeeDataTable"
                />
                <Box direction="row" alignSelf="center">
                  <Button
                    label="Close"
                    onClick={() => setShowTrainingModuleStatus(false)}
                    margin="small"
                  />
                  <Button
                    label="Delete"
                    color="red"
                    onClick={() => {
                      deleteTrainingModule();
                      setShowTrainingModuleStatus(false);
                    }}
                    margin="small"
                  />
                </Box>
              </Box>
            </Layer>
          )}
        </Box>
      </Grid>
    </Box>
  );
}

export default EmployeeDashboard;
