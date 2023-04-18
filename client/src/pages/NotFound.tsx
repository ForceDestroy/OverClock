import { Box, Button, Grommet, Text } from 'grommet';
import cry from '../assets/gif/cry.gif';
import AuthTokenHelper from '../helpers/AuthTokenHelper';

function NotFound() {
  const token = AuthTokenHelper.getFromLocalStorage();
  const homeRef = token ? '/EmployeeDashboard' : '/';
  return (
    <Grommet full>
      <Box
        direction="column"
        align="center"
        justify="center"
        pad="large"
        gap="medium"
      >
        <Text
          size="xlarge"
          weight="bold"
          textAlign="center"
          margin={{ bottom: 'medium' }}
        >
          This page does not exist
        </Text>
        <Box>
          <img src={cry} alt="404" />
        </Box>
        <Box direction="row" gap="medium">
          <Button
            primary
            label="Go Home"
            size="large"
            href={homeRef}
            data-testid="goHomeButton"
          />
        </Box>
      </Box>
    </Grommet>
  );
}

export default NotFound;
