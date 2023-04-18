import {
  Text,
  Box,
  Grid,
  Avatar,
  TextInput,
  Button,
  Notification,
} from 'grommet';
import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';

import AuthTokenHelper from '../helpers/AuthTokenHelper';
import RestCallHelper from '../helpers/RestCallHelper';
import NetworkResources from '../resources/NetworkResources';
import UIResources from '../resources/UIResources';

function ProfilePage() {
  const [name, setName] = useState('');
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [address, setAddress] = useState('');
  const [phoneNumber, setPhoneNumber] = useState(0);
  const [userID, setUserID] = useState('');
  const [birthday, setBirthday] = useState('');
  const [themeColor, setThemeColor] = useState('');
  const [newPassword, setNewPassword] = useState('');
  const [confirmPassword, setConfirmPassword] = useState('');

  const navigate = useNavigate();
  const [errorStatus, setErrorStatus] = useState(0);
  const [errorMessage, setErrorMessage] = useState('');

  const getAccountDetails = async () => {
    const accountDetailsUrl = `${NetworkResources.API_URL}/User/GetAccountInfo`;
    const token = AuthTokenHelper.getFromLocalStorage();
    const accountDetailsParams = JSON.stringify({
      sessionToken: token,
    });

    const { data } = await RestCallHelper.GetRequest(
      accountDetailsUrl,
      accountDetailsParams
    );

    try {
      const accountDetails = JSON.parse(data);
      setName(accountDetails.name);
      setEmail(accountDetails.email);
      setPassword(accountDetails.password);
      setAddress(accountDetails.address);
      setPhoneNumber(accountDetails.phoneNumber);
      setUserID(accountDetails.id);
      setBirthday(accountDetails.birthday);
      setThemeColor(accountDetails.themeColor);
      setNewPassword('');
      setConfirmPassword('');
    } catch (error) {
      setErrorStatus(1);
      setErrorMessage('Unable to retrieve account details');
    }
  };

  const handleSave = async () => {
    const updateUrl = `${NetworkResources.API_URL}/User/UpdateAccount`;
    const token = AuthTokenHelper.getFromLocalStorage();
    const updateParams = JSON.stringify({
      sessionToken: token,
    });
    const updateBody = JSON.stringify({
      id: userID,
      name,
      email,
      password,
      address,
      birthday,
      phoneNumber: Number(phoneNumber),
      themeColor: Number(themeColor),
    });
    if (newPassword !== confirmPassword) {
      setErrorStatus(1);
      setErrorMessage('Passwords do not match');
      return;
    }
    await RestCallHelper.PutRequest(updateUrl, updateParams, updateBody);
  };

  useEffect(() => {
    getAccountDetails();
  }, []);

  useEffect(() => {
    const validToken = AuthTokenHelper.validateToken();
    if (!validToken) {
      navigate('/login');
    }
  }, [navigate]);

  return (
    <Box>
      <Grid
        rows={['auto', 'flex']}
        columns={{ count: 2, size: 'auto' }}
        gap="xxsmall"
        margin="small"
      >
        <Box
          align="center"
          gap="small"
          margin={{ top: 'large', bottom: 'xxsmall' }}
          border={{ color: UIResources.PALETTE.PRIMARY, size: 'medium' }}
          round="small"
          width="24vw"
          height="80vh"
          background="url('image.png')"
        >
          <Box
            width="small"
            height="small"
            round="full"
            border={{ color: UIResources.PALETTE.WHITE, size: 'medium' }}
            align="center"
            alignSelf="center"
            margin={{ top: 'xlarge', bottom: 'xxsmall' }}
          >
            <Avatar
              size="xxlarge"
              src="https://www.gravatar.com/avatar/2c7d99fe281ecd3bcd65ab915bac6dd5?s=250"
            />
          </Box>
          <Box
            background="light-1"
            align="center"
            alignSelf="center"
            margin={{ top: 'large', bottom: 'xxsmall' }}
            pad="xxsmall"
          >
            <Text size="xxlarge" weight="bold">
              {name}
            </Text>
            <br />
            <Text size="xlarge">Sales Associate</Text>
            <br />
            <br />
          </Box>
        </Box>
        <Box gap="small" width="65vw">
          <Box margin={{ top: 'large', bottom: 'xxsmall' }}>
            <Text size="xlarge" weight="bold">
              General Information
            </Text>
            <br />
            <Box direction="row" gap="medium">
              <Box width="medium">
                <Text> Name </Text>
                <TextInput
                  value={name}
                  onChange={(event) => setName(event.target.value)}
                  data-testid="profileName"
                />
              </Box>
            </Box>
            <br />
            <Box width="medium">
              <Text> Id </Text>
              <TextInput
                disabled
                value={userID}
                onChange={(event) => setUserID(event.target.value)}
                data-testid="profileUserID"
              />
            </Box>
            <br />
            <Box width="medium">
              <Text> Date of Birth </Text>
              <TextInput
                value={birthday.slice(0, 10)}
                onChange={(event) => setBirthday(event.target.value)}
                data-testid="profileBirthday"
                placeholder="YYYY-MM-DD"
              />
            </Box>
            <br />
            <Box width="medium">
              <Text> Email </Text>
              <TextInput
                value={email}
                onChange={(event) => setEmail(event.target.value)}
                data-testid="profileEmail"
              />
            </Box>
            <br />
            <Box direction="row" gap="small">
              <Box width="medium">
                <Text> Phone Number </Text>
                <TextInput
                  value={phoneNumber}
                  onChange={(event) =>
                    setPhoneNumber(Number(event.target.value))
                  }
                  data-testid="profilePhoneNumber"
                />
              </Box>
              <Box width="medium">
                <Text> Address </Text>
                <TextInput
                  value={address}
                  onChange={(event) => setAddress(event.target.value)}
                  data-testid="profileAddress"
                />
              </Box>
            </Box>
            <br />
            <Text size="xlarge" weight="bold">
              Password
            </Text>
            <Box direction="row" gap="small">
              <Box width="medium">
                <Text> Old Password </Text>
                <TextInput
                  disabled
                  value="************"
                  onChange={(event) => setPassword(event.target.value)}
                  data-testid="profilePassword"
                />
              </Box>
              <Box width="medium">
                <Text> New Password </Text>
                <TextInput
                  value={newPassword}
                  onChange={(event) => setNewPassword(event.target.value)}
                  data-testid="profileNewPassword"
                />
              </Box>
              <Box width="medium">
                <Text> Confirm New Password </Text>
                <TextInput
                  value={confirmPassword}
                  onChange={(event) => setConfirmPassword(event.target.value)}
                  data-testid="profileConfirmPassword"
                />
              </Box>
            </Box>
            <Box margin="large" align="end" alignContent="end">
              <Box direction="row" gap="medium">
                <Button
                  label={
                    <Box width="10em" align="center">
                      <br />
                      <Text size="large">Cancel</Text>
                      <br />
                    </Box>
                  }
                  plain
                  hoverIndicator
                  onClick={() => getAccountDetails()}
                  data-testid="cancelButton"
                />
                <Button
                  label={
                    <Box width="10em">
                      <br />
                      <Text size="large" color="white" weight="bold">
                        Save Changes
                      </Text>
                      <br />
                    </Box>
                  }
                  color={UIResources.PALETTE.GRADIENT}
                  primary
                  hoverIndicator
                  onClick={() => handleSave()}
                  data-testid="saveButton"
                />
              </Box>
            </Box>
          </Box>
        </Box>
      </Grid>
      {errorStatus === 1 && (
        <Notification
          toast
          message={errorMessage}
          status="critical"
          onClose={() => setErrorStatus(0)}
        />
      )}
    </Box>
  );
}

export default ProfilePage;
