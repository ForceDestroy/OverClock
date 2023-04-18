import { render, screen, waitFor, act } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import axios from 'axios';
import { MemoryRouter } from 'react-router-dom';
import { describe, it, vi } from 'vitest';

import { App } from '../App';
import AuthTokenHelper from '../helpers/AuthTokenHelper';

vi.mock('axios');
vi.mock('../helpers/AuthTokenHelper');

describe('Employer Requests', () => {
  afterEach(() => {
    vi.clearAllMocks();
    vi.resetAllMocks();
  });
  it('Renders Employer Requests to match snapshot', async () => {
    // Mock
    (axios.request as jest.Mock).mockReturnValue({
      data: [
        {
          id: 9,
          fromId: 'WEB_000000',
          toId: 'MNG_000000',
          fromName: 'web test',
          toName: 'DrAiman Hanna',
          body: '30',
          date: '2023-01-30T00:00:00',
          startTime: '2023-01-30T00:00:00',
          endTime: '2023-01-31T00:00:00',
          type: 'Schedule',
          status: 'Approved',
        },
        {
          id: 10,
          fromId: 'WEB_000000',
          toId: 'MNG_000000',
          fromName: 'notweb test',
          toName: 'DrAiman Hanna',
          body: '',
          date: '2023-01-24T00:00:00',
          startTime: '2023-01-24T00:00:00',
          endTime: '2023-01-25T00:00:00',
          type: 'Vacation',
          status: 'Acknowledged',
        },
        {
          id: 27,
          fromId: 'AAA_000004',
          toId: 'MNG_000000',
          fromName: 'Danny',
          toName: 'DrAiman Hanna',
          body: 'I need my paystubs',
          date: '2023-01-19T00:00:00',
          startTime: '2023-01-19T00:00:00',
          endTime: '2023-01-19T00:00:00',
          type: 'Sick',
          status: 'Pending',
        },
      ],
      status: 200,
      statusText: 'OK',
    });
    (AuthTokenHelper.validateToken as jest.Mock).mockReturnValue(
      'VerySecretToken'
    );

    // Arrange);

    const snap = render(
      <MemoryRouter initialEntries={['/Employer/Requests']}>
        <App />
      </MemoryRouter>
    );

    // Assert
    expect(snap).toMatchSnapshot();
  });
  it('Renders a list of all submitted requests', async () => {
    // Mock
    (axios.request as jest.Mock).mockReturnValue({
      data: [
        {
          id: 9,
          fromId: 'WEB_000000',
          toId: 'MNG_000000',
          fromName: 'web test',
          toName: 'DrAiman Hanna',
          body: '30',
          date: '2023-01-30T00:00:00',
          startTime: '2023-01-30T00:00:00',
          endTime: '2023-01-31T00:00:00',
          type: 'Schedule',
          status: 'Approved',
        },
        {
          id: 10,
          fromId: 'WEB_000000',
          toId: 'MNG_000000',
          fromName: 'notweb test',
          toName: 'DrAiman Hanna',
          body: '',
          date: '2023-01-24T00:00:00',
          startTime: '2023-01-24T00:00:00',
          endTime: '2023-01-25T00:00:00',
          type: 'Vacation',
          status: 'Acknowledged',
        },
        {
          id: 27,
          fromId: 'AAA_000004',
          toId: 'MNG_000000',
          fromName: 'Danny',
          toName: 'DrAiman Hanna',
          body: 'I need my paystubs',
          date: '2023-01-19T00:00:00',
          startTime: '2023-01-19T00:00:00',
          endTime: '2023-01-19T00:00:00',
          type: 'Sick',
          status: 'Pending',
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
      <MemoryRouter initialEntries={['/Employer/Requests']}>
        <App />
      </MemoryRouter>
    );

    // Act
    await user.click(screen.getByText('Refresh'));

    // Assert
    expect(screen.getByText('Danny')).toBeInTheDocument();
    expect(screen.getByText('web test')).toBeInTheDocument();
    expect(screen.getByText('notweb test')).toBeInTheDocument();
    expect(screen.getByText('2023-01-19')).toBeInTheDocument();
    expect(screen.getByText('2023-01-24')).toBeInTheDocument();
    expect(screen.getByText('2023-01-30')).toBeInTheDocument();
    expect(screen.getByText('Schedule')).toBeInTheDocument();
    expect(screen.getByText('Vacation')).toBeInTheDocument();
    expect(screen.getByText('Sick')).toBeInTheDocument();
    expect(screen.getByText('Acknowledged')).toBeInTheDocument();
    expect(screen.getByText('Approved')).toBeInTheDocument();
    expect(screen.getByText('Pending')).toBeInTheDocument();
  });
  it('Renders a full detailed list of a single request', async () => {
    // Mock
    (axios.request as jest.Mock).mockReturnValue({
      data: [
        {
          id: 9,
          fromId: 'WEB_000000',
          toId: 'MNG_000000',
          fromName: 'web test',
          toName: 'DrAiman Hanna',
          body: '30',
          date: '2023-01-30T00:00:00',
          startTime: '2023-01-30T00:00:00',
          endTime: '2023-01-31T00:00:00',
          type: 'Schedule',
          status: 'Approved',
        },
        {
          id: 10,
          fromId: 'WEB_000000',
          toId: 'MNG_000000',
          fromName: 'notweb test',
          toName: 'DrAiman Hanna',
          body: '',
          date: '2023-01-24T00:00:00',
          startTime: '2023-01-24T00:00:00',
          endTime: '2023-01-25T00:00:00',
          type: 'Vacation',
          status: 'Acknowledged',
        },
        {
          id: 27,
          fromId: 'AAA_000004',
          toId: 'MNG_000000',
          fromName: 'Danny',
          toName: 'DrAiman Hanna',
          body: 'I need my paystubs',
          date: '2023-01-19T00:00:00',
          startTime: '2023-01-19T00:00:00',
          endTime: '2023-01-19T00:00:00',
          type: 'Sick',
          status: 'Pending',
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
      <MemoryRouter initialEntries={['/Employer/Requests']}>
        <App />
      </MemoryRouter>
    );

    // Act
    await user.click(screen.getByText('Refresh'));
    (axios.request as jest.Mock).mockReturnValue({
      data: [],
      status: 200,
      statusText: 'OK',
    });
    await user.click(screen.getByText('Danny'));

    // Assert
    expect(screen.getByText('Name: Danny')).toBeInTheDocument();
    expect(screen.getByText('ID: AAA_000004')).toBeInTheDocument();
    expect(screen.getByText('Reason: I need my paystubs')).toBeInTheDocument();
    expect(screen.getByText('Type: Sick')).toBeInTheDocument();
    expect(screen.getByText('Status: Pending')).toBeInTheDocument();
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
      <MemoryRouter initialEntries={['/Employer/Requests']}>
        <App />
      </MemoryRouter>
    );

    // Assert
    expect(screen.getByText('Mode Durgaa')).toBeInTheDocument();
  });
});
