import { render, screen, waitFor } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import { MemoryRouter } from 'react-router-dom';
import { describe, it, vi } from 'vitest';

import axios from 'axios';
import AuthTokenHelper from '../helpers/AuthTokenHelper';
import { App } from '../App';

vi.mock('axios');
vi.mock('../helpers/AuthTokenHelper');

describe('EmployeeDashboard', () => {
  afterEach(() => {
    vi.restoreAllMocks();
    vi.clearAllMocks();
  });
  it('Renders Employee Dashboard to match snapshot', () => {
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
      <MemoryRouter initialEntries={['/EmployeeDashboard']}>
        <App />
      </MemoryRouter>
    );

    // Assert
    expect(snap).toMatchSnapshot();
  });
  it('Renders all details related to employees on the Employee Dashboard page', async () => {
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
      <MemoryRouter initialEntries={['/EmployeeDashboard']}>
        <App />
      </MemoryRouter>
    );

    // Assert
    expect(screen.getByText('Mode Durgaa')).toBeInTheDocument();
    expect(screen.getByText('Announcements')).toBeInTheDocument();
    expect(screen.getByText('Log your daily hours')).toBeInTheDocument();
    expect(screen.getByText('View your time off')).toBeInTheDocument();
    expect(screen.getByText('Access your payslips')).toBeInTheDocument();
    expect(screen.getByText('Send a custom request')).toBeInTheDocument();
    expect(screen.getByText('View your schedule')).toBeInTheDocument();
    expect(
      screen.getByText('View company policies and info')
    ).toBeInTheDocument();
    expect(
      screen.getByText('Access the job referal program')
    ).toBeInTheDocument();
    expect(screen.getByText('View your profile')).toBeInTheDocument();
  });
  it('Renders login page after logging out of Employee Dashboard', async () => {
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

    window.location = {
      reload: vi.fn(),
      // Hack to get around compile error
    } as unknown as Location;

    // Arrange
    const user = userEvent.setup();

    render(
      <MemoryRouter initialEntries={['/EmployeeDashboard']}>
        <App />
      </MemoryRouter>
    );

    // Act
    const logoutButton = screen.getByTestId('logout');
    await user.click(logoutButton);

    // Assert
    expect(window.location.reload).toHaveBeenCalledTimes(1);
  });
  it('Renders the correct page when clicking on log hours button', async () => {
    // Mock
    (AuthTokenHelper.validateToken as jest.Mock).mockReturnValue(
      'VerySecureToken1234567890'
    );

    (axios.request as jest.Mock).mockReturnValue({
      data: {
        id: 'WEB_000000',
        name: 'web test',
        email: 'webtest@gmail.com',
        password: 'securepassword',
        address: '31 Jump Street',
        birthday: '0001-01-01T00:00:00',
        phoneNumber: 123,
        themeColor: 0,
      },
      status: 200,
      statusText: 'OK',
    });
    // Arrange
    const user = userEvent.setup();

    render(
      <MemoryRouter initialEntries={['/EmployeeDashboard']}>
        <App />
      </MemoryRouter>
    );
    // Act
    const logHours = screen.getByTestId('logHoursButton');
    await user.click(logHours);

    // Assert
    await waitFor(() =>
      expect(screen.getByText('Timesheets')).toBeInTheDocument()
    );
  });
  it('Renders the correct page when clicking on view time off', async () => {
    // Mock
    (AuthTokenHelper.validateToken as jest.Mock).mockReturnValue(
      'VerySecureToken1234567890'
    );

    (axios.request as jest.Mock).mockReturnValue({
      data: {
        id: 'WEB_000000',
        name: 'web test',
        email: 'webtest@gmail.com',
        password: 'securepassword',
        address: '31 Jump Street',
        birthday: '0001-01-01T00:00:00',
        phoneNumber: 123,
        themeColor: 0,
        vacationDays: 5,
        sickDays: 7,
      },
      status: 200,
      statusText: 'OK',
    });
    // Arrange
    const user = userEvent.setup();

    render(
      <MemoryRouter initialEntries={['/EmployeeDashboard']}>
        <App />
      </MemoryRouter>
    );
    // Act
    const timeOffButton = screen.getByTestId('viewTimeOffButton');
    await user.click(timeOffButton);

    // Assert
    await waitFor(() =>
      expect(screen.getByText('Time Off')).toBeInTheDocument()
    );
  });
  it('Renders the correct page when clicking on send custom request', async () => {
    // Mock
    (AuthTokenHelper.validateToken as jest.Mock).mockReturnValue(
      'VerySecureToken1234567890'
    );

    (axios.request as jest.Mock).mockReturnValue({
      data: {
        id: 'WEB_000000',
        name: 'web test',
        email: 'webtest@gmail.com',
        password: 'securepassword',
        address: '31 Jump Street',
        birthday: '0001-01-01T00:00:00',
        phoneNumber: 123,
        themeColor: 0,
        vacationDays: 5,
        sickDays: 7,
      },
      status: 200,
      statusText: 'OK',
    });
    // Arrange
    const user = userEvent.setup();

    render(
      <MemoryRouter initialEntries={['/EmployeeDashboard']}>
        <App />
      </MemoryRouter>
    );
    // Act
    const customRequestButton = screen.getByTestId('sendRequestButton');
    await user.click(customRequestButton);

    // Assert
    await waitFor(() =>
      expect(screen.getByText('Time Off')).toBeInTheDocument()
    );
  });
  it('Renders the correct page when clicking on view schedule', async () => {
    // Mock
    (AuthTokenHelper.validateToken as jest.Mock).mockReturnValue(
      'VerySecureToken1234567890'
    );

    (axios.request as jest.Mock).mockReturnValue({
      data: {
        id: 'WEB_000000',
        name: 'web test',
        email: 'webtest@gmail.com',
        password: 'securepassword',
        address: '31 Jump Street',
        birthday: '0001-01-01T00:00:00',
        phoneNumber: 123,
        themeColor: 0,
      },
      status: 200,
      statusText: 'OK',
    });
    // Arrange
    const user = userEvent.setup();

    render(
      <MemoryRouter initialEntries={['/EmployeeDashboard']}>
        <App />
      </MemoryRouter>
    );
    // Act
    const viewScheduleButton = screen.getByTestId('viewScheduleButton');
    await user.click(viewScheduleButton);

    // Assert
    await waitFor(() =>
      expect(screen.getByText('Schedule')).toBeInTheDocument()
    );
  });
  it('Renders the correct page when clicking on view policies', async () => {
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
      <MemoryRouter initialEntries={['/EmployeeDashboard']}>
        <App />
      </MemoryRouter>
    );
    // Act
    const policiesButton = screen.getByTestId('viewPoliciesButton');
    await user.click(policiesButton);

    // Assert
    await waitFor(() =>
      expect(screen.getByText('Company Policy')).toBeInTheDocument()
    );
  });
  it('Renders the correct page when clicking on view referals', async () => {
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
      <MemoryRouter initialEntries={['/EmployeeDashboard']}>
        <App />
      </MemoryRouter>
    );
    // Act
    const referalsButton = screen.getByTestId('accessReferalsButton');
    await user.click(referalsButton);

    // Assert
    await waitFor(() =>
      expect(screen.getByText('Mode Durgaa')).toBeInTheDocument()
    );
  });
});
