import { render, screen, waitFor, act, cleanup } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import axios from 'axios';
import { MemoryRouter } from 'react-router-dom';
import { describe, it, vi } from 'vitest';
import { App } from '../App';

import AuthTokenHelper from '../helpers/AuthTokenHelper';

vi.mock('axios');
vi.mock('../helpers/AuthTokenHelper');
describe('Schedule', () => {
  beforeEach(() => {
    vi.clearAllMocks();
    cleanup();
  });
  it('Renders TimeOff to match snapshot', async () => {
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
    (AuthTokenHelper.validateToken as jest.Mock).mockReturnValue(
      'VerySecretToken'
    );
    // Arrange
    const snap = render(
      <MemoryRouter initialEntries={['/employeedashboard/timeoff']}>
        <App />
      </MemoryRouter>
    );

    // Assert
    expect(snap).toMatchSnapshot();
  });
  it('Renders TimeOff for Employee', async () => {
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
    (AuthTokenHelper.validateToken as jest.Mock).mockReturnValue(
      'VerySecretToken'
    );

    // Arrange
    render(
      <MemoryRouter initialEntries={['/employeedashboard/timeoff']}>
        <App />
      </MemoryRouter>
    );

    // Assert
    expect(screen.getByText('Time Off')).toBeInTheDocument();
    expect(screen.getByText('Vacation Days')).toBeInTheDocument();
    expect(screen.getByText('Sick Days')).toBeInTheDocument();
    expect(screen.queryAllByText('Total')[0]).toBeInTheDocument();
    expect(screen.queryAllByText('Total')[1]).toBeInTheDocument();
    expect(screen.getByText('Request Vacation Status')).toBeInTheDocument();
    expect(screen.getByText('Request Sick Days Status')).toBeInTheDocument();
    expect(
      screen.getByText('Request Personal Time Off Status')
    ).toBeInTheDocument();
  });
  it('Renders the create request modal and closes it', async () => {
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
    (AuthTokenHelper.validateToken as jest.Mock).mockReturnValue(
      'VerySecretToken'
    );
    // Arrange
    const user = userEvent.setup();

    render(
      <MemoryRouter initialEntries={['/employeedashboard/timeoff']}>
        <App />
      </MemoryRouter>
    );
    const createRequestButton = screen.getByTestId('createRequestButton');
    await user.click(createRequestButton);

    expect(screen.getByText('Create a Request')).toBeInTheDocument();

    const cancelButton = screen.getByTestId('requestCloseButton');
    await user.click(cancelButton);
    // Assert
    await waitFor(() => {
      expect(screen.queryByText('Create a Request')).not.toBeInTheDocument();
    });
  });
  it('Renders the create request modal and fails to submit without type', async () => {
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
    (AuthTokenHelper.validateToken as jest.Mock).mockReturnValue(
      'VerySecretToken'
    );
    // Arrange
    const user = userEvent.setup();

    render(
      <MemoryRouter initialEntries={['/employeedashboard/timeoff']}>
        <App />
      </MemoryRouter>
    );
    const createRequestButton = screen.getByTestId('createRequestButton');
    await user.click(createRequestButton);
    const requestSubmitButton = screen.getByTestId('requestSubmitButton');
    await user.click(requestSubmitButton);
    await waitFor(() => {
      expect(
        screen.getByText('Please select a request type')
      ).toBeInTheDocument();
    });
    const requestTypeSelect = screen.getByTestId('requestType');
    await user.click(requestTypeSelect);
    const requestTypeVacation = screen.getByText('Vacation');
    await user.click(requestTypeVacation);
    // cleanup
    const cancelButton = screen.getByTestId('requestCloseButton');
    await user.click(cancelButton);
    await waitFor(() => {
      expect(screen.queryByText('Create a Request')).not.toBeInTheDocument();
    });
  });
  it('Renders the create request modal and submits a request with one date', async () => {
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
    (AuthTokenHelper.validateToken as jest.Mock).mockReturnValue(
      'VerySecretToken'
    );
    // Arrange
    const user = userEvent.setup();

    render(
      <MemoryRouter initialEntries={['/employeedashboard/timeoff']}>
        <App />
      </MemoryRouter>
    );
    const createRequestButton = screen.getByTestId('createRequestButton');
    await user.click(createRequestButton);
    const requestTypeSelect = screen.getByTestId('requestType');
    await user.click(requestTypeSelect);
    const requestTypeVacation = screen.getByText('Vacation');
    await user.click(requestTypeVacation);
    const requestStartDate = screen.getByText('17');
    await user.click(requestStartDate);
    const requestReason = screen.getByTestId('requestReason');
    await user.type(requestReason, 'I need a break');
    const requestSubmitButton = screen.getByTestId('requestSubmitButton');
    await user.click(requestSubmitButton);
    // Assert
    await waitFor(() => {
      expect(screen.queryByText('Create a Request')).not.toBeInTheDocument();
      expect(screen.queryByText('I need a break')).not.toBeInTheDocument();
    });
  });
  it('Renders the create request modal and submits a request with two dates', async () => {
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
    (AuthTokenHelper.validateToken as jest.Mock).mockReturnValue(
      'VerySecretToken'
    );
    // Arrange
    const user = userEvent.setup();

    render(
      <MemoryRouter initialEntries={['/employeedashboard/timeoff']}>
        <App />
      </MemoryRouter>
    );
    const createRequestButton = screen.getByTestId('createRequestButton');
    await user.click(createRequestButton);
    const requestTypeSelect = screen.getByTestId('requestType');
    await user.click(requestTypeSelect);
    const requestTypeVacation = screen.getByText('Vacation');
    await user.click(requestTypeVacation);
    const requestStartDate = screen.getByText('17');
    await user.click(requestStartDate);
    const requestEndDate = screen.getByText('19');
    await user.click(requestEndDate);
    const requestReason = screen.getByTestId('requestReason');
    await user.type(requestReason, 'I need a break');
    const requestSubmitButton = screen.getByTestId('requestSubmitButton');
    await user.click(requestSubmitButton);
    // Assert
    await waitFor(() => {
      expect(screen.queryByText('Create a Request')).not.toBeInTheDocument();
      expect(screen.queryByText('I need a break')).not.toBeInTheDocument();
    });
  });
  it('Renders the create request modal and cycles through to sick day', async () => {
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
    (AuthTokenHelper.validateToken as jest.Mock).mockReturnValue(
      'VerySecretToken'
    );
    // Arrange
    const user = userEvent.setup();

    render(
      <MemoryRouter initialEntries={['/employeedashboard/timeoff']}>
        <App />
      </MemoryRouter>
    );
    const createRequestButton = screen.getByTestId('createRequestButton');
    await user.click(createRequestButton);
    const requestTypeSelect = screen.getByTestId('requestType');
    await user.click(requestTypeSelect);
    const requestTypeVacation = screen.getByText('Sick');
    await user.click(requestTypeVacation);
    await waitFor(() => {
      expect(
        screen.queryByText(
          'Please note that these days will be deducted from your sick days'
        )
      ).toBeInTheDocument();
    });
    // cleanup
    const cancelButton = screen.getByTestId('requestCloseButton');
    await user.click(cancelButton);
    await waitFor(() => {
      expect(screen.queryByText('Create a Request')).not.toBeInTheDocument();
    });
  });
  it('Renders the create request modal and cycles through to personal time off', async () => {
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
    (AuthTokenHelper.validateToken as jest.Mock).mockReturnValue(
      'VerySecretToken'
    );
    // Arrange
    const user = userEvent.setup();

    render(
      <MemoryRouter initialEntries={['/employeedashboard/timeoff']}>
        <App />
      </MemoryRouter>
    );
    const createRequestButton = screen.getByTestId('createRequestButton');
    await user.click(createRequestButton);
    const requestTypeSelect = screen.getByTestId('requestType');
    await user.click(requestTypeSelect);
    const requestTypeVacation = screen.getByText('Personal');
    await user.click(requestTypeVacation);
    await waitFor(() => {
      expect(
        screen.queryByText(
          'Please note that these will be extra unpaid time off for yourself'
        )
      ).toBeInTheDocument();
    });
    // cleanup
    const cancelButton = screen.getByTestId('requestCloseButton');
    await user.click(cancelButton);
    await waitFor(() => {
      expect(screen.queryByText('Create a Request')).not.toBeInTheDocument();
    });
  });
  it('Renders the create custom request modal and closes', async () => {
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
    (AuthTokenHelper.validateToken as jest.Mock).mockReturnValue(
      'VerySecretToken'
    );
    // Arrange
    const user = userEvent.setup();

    render(
      <MemoryRouter initialEntries={['/employeedashboard/timeoff']}>
        <App />
      </MemoryRouter>
    );
    const createRequestButton = screen.getByTestId('createCustomRequestButton');
    await user.click(createRequestButton);
    const cancelButton = screen.getByTestId('customRequestCloseButton');
    await user.click(cancelButton);
    await waitFor(() => {
      expect(
        screen.queryByText('Create a Custom Request')
      ).not.toBeInTheDocument();
    });
  });
  it('Renders the create custom request modal and submits', async () => {
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
    (AuthTokenHelper.validateToken as jest.Mock).mockReturnValue(
      'VerySecretToken'
    );
    // Arrange
    const user = userEvent.setup();

    render(
      <MemoryRouter initialEntries={['/employeedashboard/timeoff']}>
        <App />
      </MemoryRouter>
    );
    const createRequestButton = screen.getByTestId('createCustomRequestButton');
    await user.click(createRequestButton);
    await waitFor(() => {
      expect(screen.queryByText('Create a Custom Request')).toBeInTheDocument();
    });
    const requestReason = screen.getByTestId('customRequestReason');
    await user.type(requestReason, 'I need a custom request');
    const submitButton = screen.getByTestId('customRequestSubmitButton');
    await user.click(submitButton);
    await waitFor(() => {
      expect(
        screen.queryByText('Create a Custom Request')
      ).not.toBeInTheDocument();
    });
  });
});
