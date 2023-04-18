import { Anchor, Box, Button, Image, Text } from 'grommet';
import UIResources from '../resources/UIResources';

function HomeBox() {
  return (
    <Box
      pad="medium"
      gap="medium"
      width="100vw"
      height="100vh"
      alignSelf="center"
      align="center"
      animation={{ type: 'zoomIn', duration: 1000 }}
    >
      <Box align="center" alignContent="center" height="15vh">
        <Image
          src="/src/assets/images/logo.png"
          fit="cover"
          fill
          alt="overclock_logo"
        />
      </Box>
      <Text size="3xl">Workplace Management Accelerated</Text>
      <Anchor
        icon={
          <Button
            label={
              <Box width="20em">
                <br />
                <Text color="white" weight="bold" size="large">
                  Get Started
                </Text>
                <br />
              </Box>
            }
            color={UIResources.PALETTE.GRADIENT}
            primary
            size="medium"
            justify="center"
            alignSelf="center"
            margin="small"
            data-testid="getStartedButton"
          />
        }
        href="/Login"
      />
    </Box>
  );
}

export default HomeBox;
