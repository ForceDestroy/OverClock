import { useState, useEffect } from 'react';

import { useNavigate } from 'react-router-dom';

import {
  Text,
  Box,
  Grid,
  List,
  Button,
  Layer,
  Image,
  Anchor,
  Heading,
} from 'grommet';

import { Close } from 'grommet-icons';

import AuthTokenHelper from '../helpers/AuthTokenHelper';
import UIResources from '../resources/UIResources';

function CompanyPolicy() {
  const navigate = useNavigate();
  const [showLayer, setShowLayer] = useState(false);
  const [layerTitle, setLayerTitle] = useState('');
  const [selection, setSelection] = useState(0);

  const listOfRules = [
    [
      "1. Cleaning and sanitising the store's interior on a regular basis, paying particular attention to high-contact areas like door handles, counters, and display cases.",
      '2. To stop the transmission of contagious diseases, make sure that all personnel and customers wear masks inside the store.',
      "3. All personnel should get regular health examinations to make sure they are fit to work and won't endanger other customers.",
      '4. Supplying hand sanitizers and ensuring regular use by both staff and clients.',
      '5. Ensuring that workers maintain good hygiene, such as often washing their hands and protecting their mouths and noses when they cough or sneeze.',
      '6. Employees who might be exposed to hazardous compounds, such as those used in textile dyeing and printing, should be given the proper personal protective equipment (PPE).',
      '7. Ensuring adequate ventilation in the store to prevent the buildup of dangerous vapours or gases.',
      '8. Making sure there are no tripping risks inside the business and providing adequate illumination.',
      '9. Having a well-thought-out emergency plan in place, which covers first aid protocols and evacuation routes.',
      '10. Providing personnel with instruction on health and safety procedures and performing routine safety audits to find areas that need improvement.',
    ],
    [
      '1. Professionalism: All employees are required to conduct themselves in a professional manner at all times while on the job and to show consideration for their coworkers, bosses, and customers.',
      "2. Dress code: Workers are expected to follow the store's dress code policies, which may include donning traditional Indian garb or a uniform.",
      '3. Punctuality: Workers must be on time for work and must let their manager know if they will be late.',
      '4. Confidentiality: Workers are required to uphold confidentiality with relation to all business activities, client data, and shop operations.',
      '5. Conflict of interest: Workers should abstain from any associations or actions that might be perceived as harmful to the interests of the company or that might give rise to a conflict of interest with the store.',
      "6. Personal use of store resources: Workers shouldn't utilise company property for personal purposes, including using computers or other tools, without authorization.",
      '7. Safety: Workers are in charge of adhering to all safety regulations and informing their supervisor of any hazards or occurrences.',
      '8. Customer service: Staff members should treat customers with respect and promptly and professionally address any complaints or difficulties they may have.',
      '9. Anti-discrimination: Workers are expected to treat all clients and coworkers with respect and abstain from any form of discriminatory conduct or language.',
      '10. Social media: Workers should refrain from criticising the business or their coworkers on social media or any other open forum.',
    ],
    [
      '1. Leave options: The shop may provide a range of leave options, including bereavement leave, personal leave, sick leave, vacation leave and unspecified leave.',
      '2. Eligibility: Eligibility requirements for leaves of absence may be based on elements including length of service, employment position, and job duties.',
      "3. Request procedure: Workers should adhere to a certain request procedure when requesting time off, which may involve completing a form and getting their supervisor's approval.",
      '4. Requirements for giving notice: Depending on the type of leave requested and the policies of the store, employees may be obliged to give a specific amount of notice before taking time off.',
      '5. Documentation: In order to support their request for leave, employees may be required to present paperwork, such as a note from a physician.',
      '6. Length of leave: The shop may have specific policies regarding the most days of leave that may be taken at once or the most days of leave that may be taken annually.',
      "7. Pay during leave: Depending on the store's policy, employees may receive their usual salary, a portion of it, or nothing at all while on leave.",
      '8. Returning to work: Workers should be aware of the steps involved in returning to work following a leave of absence, including giving notice of their return and completing any documentation that may be required.',
      '9. Job protection: The store may have procedures in place to safeguard the employment of staff members who take time off, such as guaranteeing that their position is open upon their return.',
      '10. Legal compliance: The retailer should make sure that all applicable rules and regulations, such as the Family and Medical Leave Act, are met by its leave-of-absence policies (FMLA).',
    ],
    [
      '1. Regular attendance: Workers are required to show up for work on a regular basis and arrive on time for their shifts.',
      '2. Absence: Whether brought on by illness or for other causes, excessive absences may result in disciplinary action.',
      '3. Absence notification: If an employee is unable to report to work due to illness or another reason, they should contact their supervisor as soon as feasible.',
      "4. Documentation: In order to prove their absence, employees may be required to provide paperwork, such as a doctor's letter.",
      '5. Tardiness: Workers should report for their shifts on schedule, and they should let their supervisor know if they anticipate being late.',
      '6. Consequence of tardiness: Disciplinary action may be taken as a result of tardiness if it continues to be an issue.',
      '7. Schedule modifications: Whenever an employee has to request a schedule modification, such as a shift swap, they should contact their supervisor as soon as feasible.',
      "8. Requesting permission to take time off: Whether it's for a vacation or personal reasons, employees should ask their manager for permission before taking any time off.",
      '9. Consequences of unapproved time off: The consequences of taking unauthorised time off could include disciplinary action.',
      '10. Legal compliance: The retailer should make sure that all applicable rules and regulations, such as the Fair Labor Standards Act, are met by its attendance policies (FLSA).',
    ],
    [
      '1. Prohibition of substance abuse: Abuse of drugs and alcohol is prohibited at the workplace, as is the use of prescription medications that were not prescribed to the employee.',
      '2. Zero-tolerance policy: Substance misuse is not tolerated in the store, and anyone found in violation of this policy may be fired right away.',
      '3. Assistance programs: For workers who deal with substance misuse, the shop may offer help programmes like counselling or referrals to treatment facilities.',
      '4. Drug testing: As a condition of employment or following any workplace accident or injury, the store may demand drug testing.',
      '5. Confidentiality: For any employee who seeks help for drug misuse difficulties, the business will respect anonymity.',
      '6. Impairment: Workers are not allowed to work while under the influence of drugs or alcohol.',
      '7. Reporting: Staff members are urged to bring up any concerns they have about coworkers abusing drugs or alcohol, and management will look into them.',
      '8. Search policy: If there is a plausible suspicion of substance usage, the business maintains the right to check employee lockers or personal possessions.',
      '9. Legal compliance: The retailer must make sure that all applicable laws and rules, including the Americans with Disabilities Act (ADA) and the Occupational Safety and Health Act, are followed by its policies on substance misuse (OSHA).',
      '10. Communication: The store needs to make sure that all of its employees are aware of and understand its drug usage policies, as well as the repercussions of breaking them.',
    ],
    [
      "1. Regular schedule: Staff members are expected to adhere to the store's usual schedule of operation hours.",
      '2. Process for scheduling employees: The store may have a system or software for scheduling personnel, or it may just post a schedule in advance.',
      '3. Availability: Workers should let their manager know when they are available so that shifts can be scheduled for them that meet that timetable.',
      '4. Schedule modifications: Whenever an employee has to request a schedule modification, such as a shift swap, they should contact their supervisor as soon as feasible.',
      "5. Requesting permission to take time off: Whether it's for a vacation or personal reasons, employees should ask their manager for permission before taking any time off.",
      '6. Consequences of unapproved time off: Unapproved time off may result in disciplinary action, so be aware of the repercussions.',
      '7. Overtime: When it comes to overtime, the store may have policies in place that specify how it is calculated, when it is available, and how it is paid.',
      '8. Breaks: The retailer is responsible for making sure that its staff members receive the proper breaks in accordance with all applicable rules and regulations.',
      '9. Legal compliance: The retailer should make sure that all applicable rules and regulations, such as the Fair Labor Standards Act, are met by its scheduling practises (FLSA).',
      '10. Communication: The store should make sure that all employees are aware of its scheduling policies and receive updates on them on a regular basis. It should also make sure that everyone is aware of their obligations and responsibilities with relation to scheduling.',
    ],
    [
      '1. Nondiscrimination: The shop does not tolerate discrimination on the grounds of handicap, age, race, colour, national origin, or religion.',
      "2. Recruitment and employment: The retailer will only consider candidates who meet the store's requirements, regardless of their race, colour, national origin, religion, gender, sexual orientation, age, or disability.",
      "3. Training and development: Regardless of a person's race, colour, national origin, religion, gender, sexual orientation, age, or disability, the business shall offer equal training and development opportunities to every employee.",
      "4. Pay and benefits: Regardless of a person's race, colour, national origin, religion, gender, sexual orientation, age, or disability, the shop shall offer equal pay and benefits to all employees.",
      '5. Promotion and advancement: No matter their race, colour, national origin, religion, gender, sexual orientation, age, or disability, the shop will grant all employees equal chances for promotion and advancement.',
      '6. Harassment and retaliation: The company forbids harassment and retribution against staff members based on their race, colour, national origin, religion, gender, sexual orientation, age, or disability.',
      '7. Reasonable accommodations: To enable employees with disabilities to carry out their tasks, the shop will offer appropriate adjustments.',
      '8. Reporting discrimination: The store encourages staff to report any instances of discrimination or harassment they may have personally experienced or witnessed. The company will swiftly and completely look into any such allegations.',
      '9. Compliance with legislation: The retailer should make sure that all applicable laws and regulations, including Title VII of the Civil Rights Act, the Americans with Disabilities Act (ADA), and the Age Discrimination in Employment Act, are upheld through its equal opportunity policies (ADEA).',
      '10. Communication: The store should make sure that all employees are aware of its equal opportunity policies and receive updates on them on a regular basis.',
    ],
  ];
  const layerContent = (number: number) => {
    return (
      <Box>
        {number <= 6 && (
          <List
            data={listOfRules[number]}
            pad={{ left: 'small', right: 'none' }}
          />
        )}
        {number === 7 && (
          <Box>
            <Box border="bottom">
              <Text size="large" weight="bold">
                {' '}
                Address{' '}
              </Text>
            </Box>
            <br />
            <Text> 4787 boul. des Sources </Text>
            <Text> H8Y 1S3, QC </Text>
            <br />
            <Box border="bottom">
              <Text size="large" weight="bold">
                {' '}
                Contact{' '}
              </Text>
            </Box>
            <br />
            <Text> (514) 685 0329 </Text>
            <Text> (514) 887 2202 </Text>
            <Text> modedurgaa@gmail.com </Text>
          </Box>
        )}
        {number === 8 && (
          <Box>
            <Text>
              {' '}
              Mode Durgaa is a family run clothing store that specializes in
              traditional Indian clothing{' '}
            </Text>
          </Box>
        )}
      </Box>
    );
  };

  const closeAndResetLayer = () => {
    setShowLayer(false);
    setLayerTitle('');
    setSelection(0);
  };

  useEffect(() => {
    const validToken = AuthTokenHelper.validateToken();
    if (!validToken) {
      navigate('/login');
    }
  }, [navigate]);

  return (
    <Box fill>
      <Box direction="row">
        <Anchor href="/EmployeeDashboard">My Job</Anchor>
        <Text>&nbsp; {'>'} &nbsp;</Text>
        <Anchor href="/EmployeeDashboard/CompanyPolicy">
          Company Policies
        </Anchor>
      </Box>
      <h1>&nbsp;&nbsp;Company Policy</h1>
      <Grid
        rows={['auto', 'flex']}
        columns={{ count: 5, size: 'auto' }}
        margin={{ left: 'medium', right: 'medium' }}
        gap="medium"
      >
        <Box gap="medium">
          <Box
            background={UIResources.PALETTE.SECONDARY}
            pad="medium"
            round="small"
            border={{ color: UIResources.PALETTE.BORDER, size: 'small' }}
            width="35vw"
            height="35vh"
            onClick={() => {
              setShowLayer(true);
              setLayerTitle('Health and Safety Policies');
              setSelection(0);
            }}
          >
            <Image
              src="/src/assets/images/health.png"
              fit="contain"
              margin="medium"
            />
            <Text alignSelf="center" weight="bold" data-testid="healthBox">
              Health and Safety
            </Text>
          </Box>
          <Box
            background={UIResources.PALETTE.SECONDARY}
            pad="medium"
            round="small"
            border={{ color: UIResources.PALETTE.BORDER, size: 'small' }}
            width="35vw"
            height="35vh"
            onClick={() => {
              setShowLayer(true);
              setLayerTitle('Schedule Policies');
              setSelection(5);
            }}
          >
            <Image
              src="/src/assets/images/schedule.png"
              fit="contain"
              margin="medium"
            />
            <Text alignSelf="center" weight="bold" data-testid="scheduleBox">
              Schedule
            </Text>
          </Box>
        </Box>
        <Box gap="medium">
          <Box
            background={UIResources.PALETTE.SECONDARY}
            pad="medium"
            round="small"
            border={{ color: UIResources.PALETTE.BORDER, size: 'small' }}
            width="35vw"
            height="35vh"
            onClick={() => {
              setShowLayer(true);
              setLayerTitle('Employee Code of Conduct Policies');
              setSelection(1);
            }}
          >
            <Image
              src="/src/assets/images/conduct.png"
              fit="contain"
              margin="medium"
            />
            <Text alignSelf="center" weight="bold" data-testid="employeeBox">
              Employee Code of Conduct
            </Text>
          </Box>
          <Box
            background={UIResources.PALETTE.SECONDARY}
            pad="medium"
            round="small"
            border={{ color: UIResources.PALETTE.BORDER, size: 'small' }}
            width="35vw"
            height="35vh"
            onClick={() => {
              setShowLayer(true);
              setLayerTitle('Equal Opportunity Policies');
              setSelection(6);
            }}
          >
            <Image
              src="/src/assets/images/opportunity.png"
              fit="contain"
              margin="medium"
            />
            <Text alignSelf="center" weight="bold" data-testid="equalBox">
              Equal Opportunity
            </Text>
          </Box>
        </Box>
        <Box gap="medium">
          <Box
            background={UIResources.PALETTE.SECONDARY}
            pad="medium"
            round="small"
            border={{ color: UIResources.PALETTE.BORDER, size: 'small' }}
            width="35vw"
            height="35vh"
            onClick={() => {
              setShowLayer(true);
              setLayerTitle('Leave of Absence Policies');
              setSelection(2);
            }}
          >
            <Image
              src="/src/assets/images/leave.png"
              fit="contain"
              margin="medium"
            />
            <Text alignSelf="center" weight="bold" data-testid="leaveBox">
              Leave of Absence
            </Text>
          </Box>
          <Box
            background={UIResources.PALETTE.SECONDARY}
            pad="medium"
            round="small"
            border={{ color: UIResources.PALETTE.BORDER, size: 'small' }}
            width="35vw"
            height="35vh"
            onClick={() => {
              setShowLayer(true);
              setLayerTitle('Contact and Address Info');
              setSelection(7);
            }}
          >
            <Image
              src="/src/assets/images/phone.png"
              fit="contain"
              margin="medium"
            />
            <Text alignSelf="center" weight="bold" data-testid="contactBox">
              Contact and Address
            </Text>
          </Box>
        </Box>
        <Box gap="medium">
          <Box
            background={UIResources.PALETTE.SECONDARY}
            pad="medium"
            round="small"
            border={{ color: UIResources.PALETTE.BORDER, size: 'small' }}
            width="35vw"
            height="35vh"
            onClick={() => {
              setShowLayer(true);
              setLayerTitle('Attendance Policies');
              setSelection(3);
            }}
          >
            <Image
              src="/src/assets/images/attendance.png"
              fit="contain"
              margin="medium"
            />
            <Text alignSelf="center" weight="bold" data-testid="attendanceBox">
              Attendance
            </Text>
          </Box>
          <Box
            background={UIResources.PALETTE.SECONDARY}
            pad="medium"
            round="small"
            border={{ color: UIResources.PALETTE.BORDER, size: 'small' }}
            width="35vw"
            height="35vh"
            onClick={() => {
              setShowLayer(true);
              setLayerTitle('About the Company Info');
              setSelection(8);
            }}
          >
            <Image
              src="/src/assets/images/info.png"
              fit="contain"
              margin="medium"
            />
            <Text alignSelf="center" weight="bold" data-testid="aboutBox">
              About the Company
            </Text>
          </Box>
        </Box>
        <Box gap="medium">
          <Box
            background={UIResources.PALETTE.SECONDARY}
            pad="medium"
            round="small"
            border={{ color: UIResources.PALETTE.BORDER, size: 'small' }}
            width="35vw"
            height="35vh"
            onClick={() => {
              setShowLayer(true);
              setLayerTitle('Substance Abuse Policies');
              setSelection(4);
            }}
          >
            <Image
              src="/src/assets/images/drug.png"
              fit="contain"
              margin="medium"
            />
            <Text alignSelf="center" weight="bold" data-testid="drugBox">
              Substance Abuse
            </Text>
          </Box>
        </Box>
      </Grid>
      {showLayer && (
        <Layer
          position="center"
          modal
          onClickOutside={() => closeAndResetLayer()}
          onEsc={() => closeAndResetLayer()}
        >
          <Box pad="medium" gap="small">
            <Box direction="row">
              <Heading level={3} margin="none">
                {layerTitle}
              </Heading>
              <Box alignSelf="end" margin={{ left: 'auto' }}>
                <Button
                  icon={<Close />}
                  onClick={() => closeAndResetLayer()}
                  hoverIndicator
                  data-testid="closeLayerButton"
                />
              </Box>
            </Box>
            {layerContent(selection)}
          </Box>
        </Layer>
      )}
    </Box>
  );
}

export default CompanyPolicy;
