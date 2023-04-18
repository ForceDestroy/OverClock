import { render, screen, waitFor } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import axios from 'axios';
import { MemoryRouter } from 'react-router-dom';
import { describe, it, vi } from 'vitest';
import { App } from '../App';

import AuthTokenHelper from '../helpers/AuthTokenHelper';

vi.mock('axios');
vi.mock('../helpers/AuthTokenHelper');

describe('Timesheet', () => {
  afterEach(() => {
    vi.clearAllMocks();
    vi.resetAllMocks();
  });
  it('Renders Timesheets and changes the week to the previous and back with submission', async () => {
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
      },
      status: 200,
      statusText: 'OK',
    });
    (AuthTokenHelper.validateToken as jest.Mock).mockReturnValue(
      'VerySecretToken'
    );

    // Arrange
    const user = userEvent.setup();

    render(
      <MemoryRouter initialEntries={['/employeedashboard/timesheets']}>
        <App />
      </MemoryRouter>
    );

    const previousWeekButton = screen.getByTestId('previousWeekButton');
    await user.click(previousWeekButton);
    const nextWeekButton = screen.getByTestId('nextWeekButton');
    await user.click(nextWeekButton);

    const timesheetButton = screen.getByText('Submit Timesheet');
    await user.click(timesheetButton);

    await waitFor(() => {
      const submitButton = screen.getByText('Submit');
      user.click(submitButton);
    });

    // Assert
    expect(screen.getByText('Timesheets')).toBeInTheDocument();
  });
  it('Renders Timesheet page with the title', async () => {
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
      },
      status: 200,
      statusText: 'OK',
    });
    (AuthTokenHelper.validateToken as jest.Mock).mockReturnValue(
      'VerySecretToken'
    );

    // Arrange
    render(
      <MemoryRouter initialEntries={['/employeedashboard/timesheets']}>
        <App />
      </MemoryRouter>
    );

    // Assert
    expect(screen.getByText('Timesheets')).toBeInTheDocument();
    expect(screen.getByText('Weekly Hours')).toBeInTheDocument();
  });
  it('Renders Timesheets popup after clicking log hours button', async () => {
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
      },
      status: 200,
      statusText: 'OK',
    });
    (AuthTokenHelper.validateToken as jest.Mock).mockReturnValue(
      'VerySecretToken'
    );

    // Arrange
    const user = userEvent.setup();

    render(
      <MemoryRouter initialEntries={['/employeedashboard/timesheets']}>
        <App />
      </MemoryRouter>
    );

    const logHoursButton = screen.getByText('Log Hours');
    await user.click(logHoursButton);
    const closeButton = screen.getByText('Close');

    // Assert
    expect(screen.queryByText('Log Hours Period')).toBeInTheDocument();
    await user.click(closeButton);
    await waitFor(() => {
      expect(screen.queryByText('Log Hours Period')).not.toBeInTheDocument();
    });
  });
});

describe('Timesheet Log Hours', () => {
  it('Renders Timesheets and logs multiple hours', async () => {
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
      },
      status: 200,
      statusText: 'OK',
    });
    (AuthTokenHelper.validateToken as jest.Mock).mockReturnValue(
      'VerySecretToken'
    );

    // Arrange
    const snap = render(
      <MemoryRouter initialEntries={['/employeedashboard/timesheets']}>
        <App />
      </MemoryRouter>
    );

    // Assert
    expect(snap).toMatchSnapshot();
  });
  it('Renders Timesheets and logs multiple hours', async () => {
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
      },
      status: 200,
      statusText: 'OK',
    });
    (AuthTokenHelper.validateToken as jest.Mock).mockReturnValue(
      'VerySecretToken'
    );

    // Arrange
    const user = userEvent.setup();

    render(
      <MemoryRouter initialEntries={['/employeedashboard/timesheets']}>
        <App />
      </MemoryRouter>
    );

    const logHoursButton = screen.getByText('Log Hours');
    await user.click(logHoursButton);
    const dateInputField = screen.getByTestId('calendarDateInput');
    await user.click(dateInputField);
    await user.keyboard('22/11/2022');

    const addHoursButton = screen.getByTestId('addHoursButton');
    await user.click(addHoursButton);

    const logButton = screen.getByText('Log');
    await user.click(logButton);

    // Assert
    expect(screen.getByText('Log Hours Period')).toBeInTheDocument();
  });
});
