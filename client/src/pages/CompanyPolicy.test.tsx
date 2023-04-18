import { render, screen, waitFor } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import { MemoryRouter } from 'react-router-dom';
import { describe, it, vi } from 'vitest';

import axios from 'axios';
import AuthTokenHelper from '../helpers/AuthTokenHelper';
import { App } from '../App';

vi.mock('axios');
vi.mock('../helpers/AuthTokenHelper');

describe('CompanyPolicy', () => {
  afterEach(() => {
    vi.restoreAllMocks();
    vi.clearAllMocks();
  });
  it('Renders Company Policies to match snapshot', () => {
    // Mock
    (axios.request as jest.Mock).mockReturnValue({
      data: {
        name: 'web test',
      },
      status: 200,
    });

    (AuthTokenHelper.validateToken as jest.Mock).mockReturnValue(
      'VerySecureToken1234567890'
    );

    // Arrange
    const snap = render(
      <MemoryRouter initialEntries={['/EmployeeDashboard/CompanyPolicy']}>
        <App />
      </MemoryRouter>
    );

    // Assert
    expect(snap).toMatchSnapshot();
  });
  it('Renders the company policies page with all the policies', async () => {
    // Mock
    (AuthTokenHelper.validateToken as jest.Mock).mockReturnValue(
      'VerySecureToken1234567890'
    );

    (axios.request as jest.Mock).mockReturnValue({
      data: {
        name: 'web test',
      },
      status: 200,
    });

    // Arrange

    render(
      <MemoryRouter initialEntries={['/EmployeeDashboard/CompanyPolicy']}>
        <App />
      </MemoryRouter>
    );

    // Assert
    expect(screen.queryByText('Company Policy')).toBeInTheDocument();
    expect(screen.queryByText('Health and Safety')).toBeInTheDocument();
    expect(screen.queryByText('Employee Code of Conduct')).toBeInTheDocument();
    expect(screen.queryByText('Leave of Absence')).toBeInTheDocument();
    expect(screen.queryByText('Attendance')).toBeInTheDocument();
    expect(screen.queryByText('Substance Abuse')).toBeInTheDocument();
    expect(screen.queryByText('Schedule')).toBeInTheDocument();
    expect(screen.queryByText('Equal Opportunity')).toBeInTheDocument();
    expect(screen.queryByText('Contact and Address')).toBeInTheDocument();
    expect(screen.queryByText('About the Company')).toBeInTheDocument();
  });
  it('Renders the company policies for health and safety', async () => {
    // Mock
    (AuthTokenHelper.validateToken as jest.Mock).mockReturnValue(
      'VerySecureToken1234567890'
    );

    (axios.request as jest.Mock).mockReturnValue({
      data: {
        name: 'web test',
      },
      status: 200,
    });

    // Arrange
    const user = userEvent.setup();

    render(
      <MemoryRouter initialEntries={['/EmployeeDashboard/CompanyPolicy']}>
        <App />
      </MemoryRouter>
    );
    const healthBox = screen.getByTestId('healthBox');
    await user.click(healthBox);

    // Assert
    await waitFor(() => {
      expect(
        screen.queryByText('Health and Safety Policies')
      ).toBeInTheDocument();
    });

    // Cleanup
    const closeButton = screen.getByTestId('closeLayerButton');
    await user.click(closeButton);

    await waitFor(() => {
      expect(
        screen.queryByText('Health and Safety Policies')
      ).not.toBeInTheDocument();
    });
  });
  it('Renders the company policies for employee code of conduct', async () => {
    // Mock
    (AuthTokenHelper.validateToken as jest.Mock).mockReturnValue(
      'VerySecureToken1234567890'
    );

    (axios.request as jest.Mock).mockReturnValue({
      data: {
        name: 'web test',
      },
      status: 200,
    });

    // Arrange
    const user = userEvent.setup();

    render(
      <MemoryRouter initialEntries={['/EmployeeDashboard/CompanyPolicy']}>
        <App />
      </MemoryRouter>
    );
    const employeeBox = screen.getByTestId('employeeBox');
    await user.click(employeeBox);

    // Assert
    await waitFor(() => {
      expect(
        screen.queryByText('Employee Code of Conduct Policies')
      ).toBeInTheDocument();
    });

    // Cleanup
    const closeButton = screen.getByTestId('closeLayerButton');
    await user.click(closeButton);

    await waitFor(() => {
      expect(
        screen.queryByText('Employee Code of Conduct Policies')
      ).not.toBeInTheDocument();
    });
  });
  it('Renders the company policies for leave of absence', async () => {
    // Mock
    (AuthTokenHelper.validateToken as jest.Mock).mockReturnValue(
      'VerySecureToken1234567890'
    );

    (axios.request as jest.Mock).mockReturnValue({
      data: {
        name: 'web test',
      },
      status: 200,
    });

    // Arrange
    const user = userEvent.setup();

    render(
      <MemoryRouter initialEntries={['/EmployeeDashboard/CompanyPolicy']}>
        <App />
      </MemoryRouter>
    );
    const leaveBox = screen.getByTestId('leaveBox');
    await user.click(leaveBox);

    // Assert
    await waitFor(() => {
      expect(
        screen.queryByText('Leave of Absence Policies')
      ).toBeInTheDocument();
    });

    // Cleanup
    const closeButton = screen.getByTestId('closeLayerButton');
    await user.click(closeButton);

    await waitFor(() => {
      expect(
        screen.queryByText('Leave of Absence Policies')
      ).not.toBeInTheDocument();
    });
  });
  it('Renders the company policies for attendance', async () => {
    // Mock
    (AuthTokenHelper.validateToken as jest.Mock).mockReturnValue(
      'VerySecureToken1234567890'
    );

    (axios.request as jest.Mock).mockReturnValue({
      data: {
        name: 'web test',
      },
      status: 200,
    });

    // Arrange
    const user = userEvent.setup();

    render(
      <MemoryRouter initialEntries={['/EmployeeDashboard/CompanyPolicy']}>
        <App />
      </MemoryRouter>
    );
    const attendanceBox = screen.getByTestId('attendanceBox');
    await user.click(attendanceBox);

    // Assert
    await waitFor(() => {
      expect(screen.queryByText('Attendance Policies')).toBeInTheDocument();
    });

    // Cleanup
    const closeButton = screen.getByTestId('closeLayerButton');
    await user.click(closeButton);

    await waitFor(() => {
      expect(screen.queryByText('Attendance Policies')).not.toBeInTheDocument();
    });
  });
  it('Renders the company policies for substance abuse', async () => {
    // Mock
    (AuthTokenHelper.validateToken as jest.Mock).mockReturnValue(
      'VerySecureToken1234567890'
    );

    (axios.request as jest.Mock).mockReturnValue({
      data: {
        name: 'web test',
      },
      status: 200,
    });

    // Arrange
    const user = userEvent.setup();

    render(
      <MemoryRouter initialEntries={['/EmployeeDashboard/CompanyPolicy']}>
        <App />
      </MemoryRouter>
    );
    const drugBox = screen.getByTestId('drugBox');
    await user.click(drugBox);

    // Assert
    await waitFor(() => {
      expect(
        screen.queryByText('Substance Abuse Policies')
      ).toBeInTheDocument();
    });

    // Cleanup
    const closeButton = screen.getByTestId('closeLayerButton');
    await user.click(closeButton);

    await waitFor(() => {
      expect(
        screen.queryByText('Substance Abuse Policies')
      ).not.toBeInTheDocument();
    });
  });
  it('Renders the company policies for schedule', async () => {
    // Mock
    (AuthTokenHelper.validateToken as jest.Mock).mockReturnValue(
      'VerySecureToken1234567890'
    );

    (axios.request as jest.Mock).mockReturnValue({
      data: {
        name: 'web test',
      },
      status: 200,
    });

    // Arrange
    const user = userEvent.setup();

    render(
      <MemoryRouter initialEntries={['/EmployeeDashboard/CompanyPolicy']}>
        <App />
      </MemoryRouter>
    );
    const scheduleBox = screen.getByTestId('scheduleBox');
    await user.click(scheduleBox);

    // Assert
    await waitFor(() => {
      expect(screen.queryByText('Schedule Policies')).toBeInTheDocument();
    });

    // Cleanup
    const closeButton = screen.getByTestId('closeLayerButton');
    await user.click(closeButton);

    await waitFor(() => {
      expect(screen.queryByText('Schedule Policies')).not.toBeInTheDocument();
    });
  });
  it('Renders the company policies for equal opportunity', async () => {
    // Mock
    (AuthTokenHelper.validateToken as jest.Mock).mockReturnValue(
      'VerySecureToken1234567890'
    );

    (axios.request as jest.Mock).mockReturnValue({
      data: {
        name: 'web test',
      },
      status: 200,
    });

    // Arrange
    const user = userEvent.setup();

    render(
      <MemoryRouter initialEntries={['/EmployeeDashboard/CompanyPolicy']}>
        <App />
      </MemoryRouter>
    );
    const equalBox = screen.getByTestId('equalBox');
    await user.click(equalBox);

    // Assert
    await waitFor(() => {
      expect(
        screen.queryByText('Equal Opportunity Policies')
      ).toBeInTheDocument();
    });

    // Cleanup
    const closeButton = screen.getByTestId('closeLayerButton');
    await user.click(closeButton);

    await waitFor(() => {
      expect(
        screen.queryByText('Equal Opportunity Policies')
      ).not.toBeInTheDocument();
    });
  });
  it('Renders the company policies for contact and address', async () => {
    // Mock
    (AuthTokenHelper.validateToken as jest.Mock).mockReturnValue(
      'VerySecureToken1234567890'
    );

    (axios.request as jest.Mock).mockReturnValue({
      data: {
        name: 'web test',
      },
      status: 200,
    });

    // Arrange
    const user = userEvent.setup();

    render(
      <MemoryRouter initialEntries={['/EmployeeDashboard/CompanyPolicy']}>
        <App />
      </MemoryRouter>
    );
    const contactBox = screen.getByTestId('contactBox');
    await user.click(contactBox);

    // Assert
    await waitFor(() => {
      expect(
        screen.queryByText('Contact and Address Info')
      ).toBeInTheDocument();
    });

    // Cleanup
    const closeButton = screen.getByTestId('closeLayerButton');
    await user.click(closeButton);

    await waitFor(() => {
      expect(
        screen.queryByText('Contact and Address Info')
      ).not.toBeInTheDocument();
    });
  });
  it('Renders the company policies for about the company', async () => {
    // Mock
    (AuthTokenHelper.validateToken as jest.Mock).mockReturnValue(
      'VerySecureToken1234567890'
    );

    (axios.request as jest.Mock).mockReturnValue({
      data: {
        name: 'web test',
      },
      status: 200,
    });

    // Arrange
    const user = userEvent.setup();

    render(
      <MemoryRouter initialEntries={['/EmployeeDashboard/CompanyPolicy']}>
        <App />
      </MemoryRouter>
    );
    const aboutBox = screen.getByTestId('aboutBox');
    await user.click(aboutBox);

    // Assert
    await waitFor(() => {
      expect(screen.queryByText('About the Company Info')).toBeInTheDocument();
    });

    // Cleanup
    const closeButton = screen.getByTestId('closeLayerButton');
    await user.click(closeButton);

    await waitFor(() => {
      expect(
        screen.queryByText('About the Company Info')
      ).not.toBeInTheDocument();
    });
  });
});
