import { render, screen, waitFor, act, cleanup } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import axios from 'axios';
import { MemoryRouter } from 'react-router-dom';
import { describe, it, vi } from 'vitest';
import { App } from '../App';

import AuthTokenHelper from '../helpers/AuthTokenHelper';
import Schedule from './Schedule';

vi.mock('axios');
vi.mock('../helpers/AuthTokenHelper');

describe('Schedule', () => {
  beforeEach(() => {
    vi.clearAllMocks();
    cleanup();
  });
  it('Renders Schedule to match snapshot', async () => {
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
      <MemoryRouter initialEntries={['/employeedashboard/schedule']}>
        <App />
      </MemoryRouter>
    );

    // Assert
    expect(snap).toMatchSnapshot();
  });
  it('Renders Schedule for Employee', async () => {
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
      <MemoryRouter initialEntries={['/employeedashboard/schedule']}>
        <App />
      </MemoryRouter>
    );

    // Assert
    expect(screen.getByText('Schedule')).toBeInTheDocument();
  });
  it('Renders Schedule and goes to previous schedule and back', async () => {
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
      <MemoryRouter initialEntries={['/employeedashboard/schedule']}>
        <App />
      </MemoryRouter>
    );

    const previousButton = screen.getByTestId('previousWeekButton');
    await user.click(previousButton);
    const nextButton = screen.getByTestId('nextWeekButton');
    await user.click(nextButton);

    // Assert
    expect(screen.getByText('Request Schedule Change')).toBeInTheDocument();
  });
  it('Renders the modal of the request status change and submits', async () => {
    const requestScheduleMock = {
      data: [
        {
          date: '2023-01-01T00:00:00',
          startTime: '2023-01-01T08:00:00',
          endTime: '2023-01-01T16:00:00',
          userName: 'web test',
          userId: 'WEB_000000',
          allowedBreakTime: 0,
          position: 'Employee',
        },
      ],
      status: 200,
      statusText: 'OK',
    };
    (axios.request as jest.Mock).mockReturnValue(requestScheduleMock);

    (AuthTokenHelper.validateToken as jest.Mock).mockReturnValue(
      'VerySecretToken'
    );
    // Arrange
    const user = userEvent.setup();
    render(
      <MemoryRouter initialEntries={['/employeedashboard/schedule']}>
        <App />
      </MemoryRouter>
    );

    const requestScheduleChangeButton = screen.getByTestId(
      'requestScheduleChangeButton'
    );
    await user.click(requestScheduleChangeButton);

    expect(
      screen.getByText('Create Schedule Change Request')
    ).toBeInTheDocument();

    const requestScheduleChangeTextArea = screen.getByTestId(
      'requestScheduleChangeMessageTextArea'
    );
    await user.click(requestScheduleChangeTextArea);
    await user.type(
      requestScheduleChangeTextArea,
      'I want to change my schedule'
    );

    const requestScheduleChangeSubmitButton = screen.getByTestId(
      'requestScheduleChangeSubmitButton'
    );
    await user.click(requestScheduleChangeSubmitButton);
    expect(screen.getByText('Review Requests')).toBeInTheDocument();
  });
  it('Renders the modal of the request status change and closes', async () => {
    // Mock
    // Schedule Mock
    const requestScheduleMock = {
      data: [
        {
          date: '2023-01-01T00:00:00',
          startTime: '2023-01-01T08:00:00',
          endTime: '2023-01-01T16:00:00',
          userName: 'web test',
          userId: 'WEB_000000',
          allowedBreakTime: 0,
          position: 'Employee',
        },
      ],
      status: 200,
      statusText: 'OK',
    };
    (axios.get as jest.Mock).mockReturnValue(requestScheduleMock);

    (AuthTokenHelper.validateToken as jest.Mock).mockReturnValue(
      'VerySecretToken'
    );
    // Arrange
    const user = userEvent.setup();
    render(
      <MemoryRouter initialEntries={['/employeedashboard/schedule']}>
        <App />
      </MemoryRouter>
    );

    const requestScheduleChangeButton = screen.getByTestId(
      'requestScheduleChangeButton'
    );
    await user.click(requestScheduleChangeButton);

    await waitFor(() =>
      expect(
        screen.getByText('Create Schedule Change Request')
      ).toBeInTheDocument()
    );

    const requestScheduleChangeCloseButton = screen.getByTestId(
      'requestScheduleChangeCloseButton'
    );
    await user.click(requestScheduleChangeCloseButton);
    expect(screen.getByText('Review Requests')).toBeInTheDocument();
  });
});
