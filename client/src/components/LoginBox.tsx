import React from 'react';

import {
  Box,
  Button,
  Form,
  Image,
  TextInput,
  Notification,
  Text,
  Heading,
} from 'grommet';

import { MailOption, Key } from 'grommet-icons';

import { useNavigate } from 'react-router-dom';

import NetworkResources from '../resources/NetworkResources';
import RestCallHelper from '../helpers/RestCallHelper';
import AuthTokenHelper from '../helpers/AuthTokenHelper';
import UIResources from '../resources/UIResources';

function LoginBox() {
  const [email, setEmail] = React.useState('');
  const [password, setPassword] = React.useState('');

  const [loginStatus, setLoginStatus] = React.useState(0);

  const navigate = useNavigate();

  const validateEmail = () => {
    const re =
      // eslint-disable-next-line no-useless-escape
      /^(([^<>()[\]\.,;:\s@\"]+(\.[^<>()[\]\.,;:\s@\"]+)*)|(\".+\"))@(([^<>()[\]\.,;:\s@\"]+\.)+[^<>()[\]\.,;:\s@\"]{2,})$/i;
    if (email.match(re)) {
      return true;
    }
    return false;
  };

  const handleLogin = async () => {
    const loginUrl = `${NetworkResources.API_URL}/Login/ValidateLogin`;
    const loginParams = JSON.stringify({
      Email: email,
      Password: password,
    });
    if (!validateEmail()) {
      setLoginStatus(1);
      return;
    }
    const loginBody = JSON.stringify({});
    const { status, data } = await RestCallHelper.PostRequest(
      loginUrl,
      loginParams,
      loginBody,
      true
    );
    if (status === 200) {
      AuthTokenHelper.saveToLocalStorage(JSON.parse(data).Token);
      AuthTokenHelper.setName(JSON.parse(data).Name);
      AuthTokenHelper.setAccessLevel(JSON.parse(data).AccessLevel);
      AuthTokenHelper.setUserId(JSON.parse(data).Id);
      navigate('/EmployeeDashboard');
    } else {
      setLoginStatus(1);
    }
  };

  return (
    <Box
      pad="large"
      gap="small"
      width="100vw"
      height="100vh"
      alignSelf="center"
      align="center"
      justify="center"
      animation={{ type: 'fadeIn', duration: 2000 }}
    >
      <Heading level="1" margin="none" size="large" weight="normal">
        Login
      </Heading>
      {loginStatus === 1 && (
        <Notification
          status="warning"
          toast
          message="Invalid email or password"
          onClose={() => setLoginStatus(0)}
        />
      )}
      <Image src="/src/assets/images/loginImage.png" alt="login_image" />
      <Box
        pad="medium"
        width="large"
        alignSelf="center"
        align="center"
        justify="center"
        alignContent="center"
      >
        <Box align="center" alignContent="center">
          <Form>
            {/* Email Field */}
            <Box
              width="medium"
              direction="row"
              margin={{
                top: 'medium',
                bottom: 'medium',
                right: 'small',
                left: 'small',
              }}
              align="center"
              round="small"
              border
            >
              <TextInput
                plain
                icon={<MailOption />}
                placeholder="Email"
                value={email}
                onChange={(event) => setEmail(event.target.value)}
                size="large"
              />
            </Box>
            {/* Password Field */}
            <Box
              width="medium"
              direction="row"
              margin={{
                top: 'medium',
                bottom: 'large',
                right: 'small',
                left: 'small',
              }}
              align="center"
              round="small"
              border
            >
              <TextInput
                plain
                icon={<Key />}
                type="password"
                placeholder="Password"
                value={password}
                onChange={(event) => setPassword(event.target.value)}
                size="large"
              />
            </Box>
            {/* Login Button */}
            <Box
              width="medium"
              direction="row"
              margin="small"
              align="center"
              justify="center"
              alignContent="center"
            >
              <Button
                label={
                  <Box width="10em">
                    <br />
                    <Text color="white" weight="bold" size="large">
                      Log In
                    </Text>
                    <br />
                  </Box>
                }
                onClick={handleLogin}
                color={UIResources.PALETTE.GRADIENT}
                primary
                size="medium"
                justify="center"
                alignSelf="center"
                margin="small"
                data-testid="loginButton"
              />
            </Box>
          </Form>
        </Box>
      </Box>
    </Box>
  );
}

export default LoginBox;
