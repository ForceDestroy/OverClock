import { render, screen } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import axios from 'axios';
import { MemoryRouter } from 'react-router-dom';
import { describe, it, vi } from 'vitest';

import { App } from '../App';
import AuthTokenHelper from '../helpers/AuthTokenHelper';

vi.mock('axios');
vi.mock('../helpers/AuthTokenHelper');

describe('Employer Timesheets', () => {
  afterEach(() => {
    vi.clearAllMocks();
    vi.resetAllMocks();
  });
  it('Renders Employer Timesheets to match snapshot', () => {
    // Mock
    (axios.request as jest.Mock).mockReturnValue({
      data: [
        {
          userId: 'AAA_000004',
          name: 'Danny',
          salary: 9,
          hoursWorked: 40,
          overtimeWorked: 15.966666666666669,
          doubleOvertimeWorked: 0,
          paidSick: 0,
          paidHoliday: 0,
          date: '2023-01-01T00:00:00',
        },
        {
          userId: 'WEB_000000',
          name: 'web test',
          salary: 21,
          hoursWorked: 32,
          overtimeWorked: 0,
          doubleOvertimeWorked: 0,
          paidSick: 0,
          paidHoliday: 0,
          date: '2022-11-20T00:00:00',
        },
      ],
      status: 200,
      statusText: 'OK',
    });
    (AuthTokenHelper.validateToken as jest.Mock).mockReturnValue(
      'VerySecretToken'
    );

    // Arrange
    const snap = render(
      <MemoryRouter initialEntries={['/Employer/Timesheets']}>
        <App />
      </MemoryRouter>
    );

    // Assert
    expect(snap).toMatchSnapshot();
  });
  it('Renders a list of all submitted timesheets', async () => {
    // Mock
    (axios.request as jest.Mock).mockReturnValue({
      data: [
        {
          userId: 'AAA_000004',
          name: 'Danny',
          salary: 9,
          hoursWorked: 40,
          overtimeWorked: 15.966666666666669,
          doubleOvertimeWorked: 0,
          paidSick: 0,
          paidHoliday: 0,
          date: '2023-01-01T00:00:00',
        },
        {
          userId: 'WEB_000000',
          name: 'web test',
          salary: 21,
          hoursWorked: 32,
          overtimeWorked: 0,
          doubleOvertimeWorked: 0,
          paidSick: 0,
          paidHoliday: 0,
          date: '2022-11-20T00:00:00',
        },
      ],
      status: 200,
      statusText: 'OK',
    });
    (AuthTokenHelper.validateToken as jest.Mock).mockReturnValue(
      'VerySecretToken'
    );

    // Arrange
    const user = userEvent.setup();

    render(
      <MemoryRouter initialEntries={['/Employer/Timesheets']}>
        <App />
      </MemoryRouter>
    );

    // Act
    await user.click(screen.getByText('Refresh'));

    // Assert
    expect(screen.getByText('Danny')).toBeInTheDocument();
    expect(screen.getByText('AAA_000004')).toBeInTheDocument();
    expect(screen.getByText('2023-01-01')).toBeInTheDocument();
    expect(screen.getByText('web test')).toBeInTheDocument();
    expect(screen.getByText('WEB_000000')).toBeInTheDocument();
    expect(screen.getByText('2022-11-20')).toBeInTheDocument();
  });
  it('Renders a full detailed list of a single timesheet', async () => {
    // Mock
    (axios.request as jest.Mock).mockReturnValue({
      data: [
        {
          userId: 'AAA_000004',
          name: 'Danny',
          salary: 9,
          hoursWorked: 40,
          overtimeWorked: 15.966666666666669,
          doubleOvertimeWorked: 0,
          paidSick: 0,
          paidHoliday: 0,
          date: '2023-01-01T00:00:00',
        },
        {
          userId: 'WEB_000000',
          name: 'web test',
          salary: 21,
          hoursWorked: 32,
          overtimeWorked: 0,
          doubleOvertimeWorked: 0,
          paidSick: 0,
          paidHoliday: 0,
          date: '2022-11-20T00:00:00',
        },
      ],
      status: 200,
      statusText: 'OK',
    });
    (AuthTokenHelper.validateToken as jest.Mock).mockReturnValue(
      'VerySecretToken'
    );

    // Arrange
    const user = userEvent.setup();

    render(
      <MemoryRouter initialEntries={['/Employer/Timesheets']}>
        <App />
      </MemoryRouter>
    );

    // Act
    await user.click(screen.getByText('Refresh'));
    await user.click(screen.getByText('AAA_000004'));

    // Assert
    expect(screen.getByText('Name: Danny')).toBeInTheDocument();
    expect(screen.getByText('ID: AAA_000004')).toBeInTheDocument();
    expect(screen.getByText('Salary: 9.00')).toBeInTheDocument();
    expect(screen.getByText('Regular Hours: 40.00')).toBeInTheDocument();
    expect(screen.getByText('Overtime Hours: 15.97')).toBeInTheDocument();
    expect(screen.getByText('Double Overtime Hours: 0.00')).toBeInTheDocument();
    expect(screen.getByText('Sick Hours: 0.00')).toBeInTheDocument();
    expect(screen.getByText('Vacation Hours: 0.00')).toBeInTheDocument();
  });
  it('Renders back to dashboard when the user has the wrong access level', async () => {
    // Mock
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
    (AuthTokenHelper.validateToken as jest.Mock).mockReturnValue(
      'VerySecretToken'
    );

    // Arrange

    render(
      <MemoryRouter initialEntries={['/Employer/Timesheets']}>
        <App />
      </MemoryRouter>
    );

    // Assert
    expect(screen.getByText('Mode Durgaa')).toBeInTheDocument();
  });
});
