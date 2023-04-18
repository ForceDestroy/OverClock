import { render, screen } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import axios from 'axios';
import { MemoryRouter } from 'react-router-dom';
import { describe, it, vi } from 'vitest';

import { App } from '../App';
import AuthTokenHelper from '../helpers/AuthTokenHelper';

vi.mock('axios');
vi.mock('../helpers/AuthTokenHelper');

describe('EmployerDashboard', () => {
  afterEach(() => {
    vi.clearAllMocks();
    vi.resetAllMocks();
  });
  it('Renders Employer Dashboard to match snapshot', () => {
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
      <MemoryRouter initialEntries={['/EmployerDashboard']}>
        <App />
      </MemoryRouter>
    );

    // Assert
    expect(snap).toMatchSnapshot();
  });
  it('Renders employees on the Employer Dashboard page', async () => {
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

    // A
    render(
      <MemoryRouter initialEntries={['/EmployerDashboard']}>
        <App />
      </MemoryRouter>
    );

    // Act
    // No act

    // Assert
    expect(screen.getByTestId('employeeDataTable')).toBeInTheDocument();
    expect(screen.getByText('ID')).toBeInTheDocument();
    expect(screen.getByText('Name')).toBeInTheDocument();
    expect(screen.getByText('Email')).toBeInTheDocument();
  });

  it('Renders the add employee pop up on the Employer Dashboard page', async () => {
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
      <MemoryRouter initialEntries={['/EmployerDashboard']}>
        <App />
      </MemoryRouter>
    );

    // Act
    const editButton = screen.getByTestId('addEmployeesButton');
    await user.click(editButton);

    // Assert
    expect(screen.getByPlaceholderText('Name')).toBeInTheDocument();
    expect(screen.getByPlaceholderText('Email')).toBeInTheDocument();
    expect(screen.getByPlaceholderText('Phone Number')).toBeInTheDocument();
    expect(screen.getByPlaceholderText('Password')).toBeInTheDocument();
    expect(
      screen.getByPlaceholderText('Social Insurance Number')
    ).toBeInTheDocument();
    expect(screen.getByPlaceholderText('Address')).toBeInTheDocument();
    expect(screen.getByPlaceholderText('Vacation Days')).toBeInTheDocument();
    expect(screen.getByPlaceholderText('Sick Days')).toBeInTheDocument();
    expect(
      screen.getByPlaceholderText('Banking Information')
    ).toBeInTheDocument();
    expect(screen.getByPlaceholderText('Salary')).toBeInTheDocument();
  });
  it('Renders back to dashboard when the user has the wrong access level', async () => {
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
    (AuthTokenHelper.getAccessLevel as jest.Mock).mockReturnValue('0');

    // A
    render(
      <MemoryRouter initialEntries={['/EmployerDashboard']}>
        <App />
      </MemoryRouter>
    );

    // Act
    // No act

    // Assert
    expect(screen.getByText('Mode Durgaa')).toBeInTheDocument();
  });
});
