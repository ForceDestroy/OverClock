import { useState, useEffect } from 'react';
import {
  Anchor,
  Box,
  Notification as NotificationComponent,
  Text,
  Grid,
  Button,
  Avatar,
} from 'grommet';
import { useLocation } from 'react-router';
import { Logout } from 'grommet-icons';

import AuthTokenHelper from '../helpers/AuthTokenHelper';
import RestCallHelper from '../helpers/RestCallHelper';
import NetworkResources from '../resources/NetworkResources';
import UIResources from '../resources/UIResources';

function NavigationTopBar() {
  const [name, setName] = useState('');
  const [accessLevel, setAccessLevel] = useState('');
  const [errorStatus, setErrorStatus] = useState(0);
  const [errorMessage, setErrorMessage] = useState('');
  const removeToken = () => {
    const logoutUrl = `${NetworkResources.API_URL}/Login/Logout`;
    const logoutParams = JSON.stringify({
      token: AuthTokenHelper.getFromLocalStorage(),
    });
    const logoutBody = JSON.stringify({});
    AuthTokenHelper.removeFromLocalStorage();
    RestCallHelper.PostRequest(logoutUrl, logoutParams, logoutBody);
    window.location.reload();
  };

  const homeRef = name ? '/EmployeeDashboard' : '/';

  const location = useLocation();

  useEffect(() => {
    const n: any = AuthTokenHelper.getName();
    if (n) {
      setName(n);
    }
    const a: any = AuthTokenHelper.getAccessLevel();
    if (a) {
      setAccessLevel(a);
    }
  }, []);
  return (
    <div>
      <Grid
        fill
        rows={['auto', 'flex']}
        columns={['auto', 'flex']}
        areas={[
          { name: 'header', start: [0, 0], end: [1, 0] },
          { name: 'login', start: [1, 1], end: [1, 1] },
        ]}
      >
        <Box
          height={{ min: '10vh' }}
          justify="between"
          direction="row"
          background={UIResources.PALETTE.PRIMARY}
          gridArea="header"
        >
          {errorStatus === 1 && (
            <NotificationComponent
              status="critical"
              message={errorMessage}
              onClose={() => setErrorStatus(0)}
            />
          )}
          <Box direction="row" align="center" pad="small" gap="large">
            <Anchor
              label={
                <Box>
                  <Text weight="bold" size="xlarge" color="white">
                    {' '}
                    Home{' '}
                  </Text>
                </Box>
              }
              href={homeRef}
              aria-label="home"
              data-testid="home"
            />
            <Anchor
              label={
                <Box
                  border={
                    location.pathname === '/EmployeeDashboard/Schedule'
                      ? { color: 'white', size: 'medium', side: 'bottom' }
                      : false
                  }
                >
                  <Text weight="bold" size="xlarge" color="white">
                    {' '}
                    Calendar{' '}
                  </Text>
                </Box>
              }
              href="/EmployeeDashboard/Schedule"
              aria-label="Calendar"
              data-testid="Calendar"
            />
            <Anchor
              label={
                <Box
                  border={
                    location.pathname === '/EmployeeDashboard'
                      ? { color: 'white', size: 'medium', side: 'bottom' }
                      : false
                  }
                >
                  <Text weight="bold" size="xlarge" color="white">
                    {' '}
                    My Job{' '}
                  </Text>
                </Box>
              }
              href={homeRef}
              aria-label="MyJob"
              data-testid="MyJob"
            />
            {accessLevel === '1' && (
              <Anchor
                label={
                  <Box
                    border={
                      location.pathname === '/EmployerDashboard'
                        ? { color: 'white', size: 'medium', side: 'bottom' }
                        : false
                    }
                  >
                    <Text weight="bold" size="xlarge" color="white">
                      {' '}
                      Management{' '}
                    </Text>
                  </Box>
                }
                href="/EmployerDashboard"
                aria-label="Management"
                data-testid="Management"
              />
            )}
          </Box>
          <Box direction="row" align="center" pad="xsmall" gap="small">
            <Box direction="column">
              <Text weight="bold"> {name} </Text>
              <Text> Sales Associate </Text>
            </Box>
            <Anchor href="/ProfilePage">
              <Box
                width="xsmall"
                height="xsmall"
                round="full"
                align="center"
                alignSelf="center"
              >
                <Avatar
                  size="xxlarge"
                  src="https://www.gravatar.com/avatar/2c7d99fe281ecd3bcd65ab915bac6dd5?s=250"
                />
              </Box>
            </Anchor>
            <Button
              icon={<Logout />}
              onClick={removeToken}
              data-testid="logout"
            />
          </Box>
        </Box>
      </Grid>
    </div>
  );
}

export default NavigationTopBar;
