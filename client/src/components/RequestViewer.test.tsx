import { render, screen, waitFor, cleanup } from '@testing-library/react';
import axios from 'axios';
import { describe, it, vi } from 'vitest';

import AuthTokenHelper from '../helpers/AuthTokenHelper';
import RequestViewer from './RequestViewer';

vi.mock('axios');
vi.mock('../helpers/AuthTokenHelper');
describe('RequestViewer', () => {
  beforeEach(() => {
    vi.clearAllMocks();
    cleanup();
  });
  afterEach(() => {
    vi.clearAllMocks();
    vi.resetAllMocks();
  });
  it('Renders RequestViewer to match snapshot', async () => {
    // Mock
    const requestScheduleMock = {
      data: null,
      status: 404,
      statusText: 'Not OK',
    };
    (axios.request as jest.Mock).mockReturnValue(requestScheduleMock);

    (AuthTokenHelper.validateToken as jest.Mock).mockReturnValue(
      'VerySecretToken'
    );
    // Arrange
    const snap = render(<RequestViewer />);

    // Assert
    expect(snap).toMatchSnapshot();
  });
  it('Renders RequestViewer', async () => {
    // Mock
    const requestScheduleMock = {
      data: null,
      status: 404,
      statusText: 'Not OK',
    };
    (axios.request as jest.Mock).mockReturnValue(requestScheduleMock);

    (AuthTokenHelper.validateToken as jest.Mock).mockReturnValue(
      'VerySecretToken'
    );
    render(<RequestViewer />);
    // Assert
    expect(screen.getByText('Date')).toBeInTheDocument();
    expect(screen.getByText('Reason')).toBeInTheDocument();
    expect(screen.getByText('Start')).toBeInTheDocument();
    expect(screen.getByText('End')).toBeInTheDocument();
    expect(screen.getByText('Status')).toBeInTheDocument();
  });
  it('Renders RequestViewer for Pending status', async () => {
    // Mock
    const requestScheduleMock = {
      data: [
        {
          id: 25,
          fromId: 'WEB_000011',
          toId: 'WEB_000010',
          body: '',
          date: '2023-01-17T00:00:00',
          startTime: '2023-01-03T00:00:00',
          endTime: '2023-01-03T00:00:00',
          type: 'Schedule',
          status: 'Pending',
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
    render(<RequestViewer type="Schedule" />);
    // Assert
    await waitFor(() => {
      expect(screen.getByText('Pending')).toBeInTheDocument();
    });
  });
  it('Renders RequestViewer for Approved status', async () => {
    // Mock
    const requestScheduleMock = {
      data: [
        {
          id: 25,
          fromId: 'WEB_000011',
          toId: 'WEB_000010',
          body: 'I want to change my schedule',
          date: '2023-01-17T00:00:00',
          startTime: '2023-01-03T00:00:00',
          endTime: '2023-01-03T00:00:00',
          type: 'Schedule',
          status: 'Approved',
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
    render(<RequestViewer type="Schedule" />);

    // Assert
    await waitFor(() => {
      expect(screen.getByText('Approved')).toBeInTheDocument();
      // expect text to not be in the document
      expect(screen.queryByText('No reason')).not.toBeInTheDocument();
    });
  });
  it('Renders RequestViewer for Denied status', async () => {
    // Mock
    const requestScheduleMock = {
      data: [
        {
          id: 25,
          fromId: 'WEB_000011',
          toId: 'WEB_000010',
          body: '',
          date: '2023-01-17T00:00:00',
          startTime: '2023-01-03T00:00:00',
          endTime: '2023-01-03T00:00:00',
          type: 'Schedule',
          status: 'Denied',
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
    render(<RequestViewer type="Schedule" />);

    // Assert
    await waitFor(() => {
      expect(screen.getByText('Denied')).toBeInTheDocument();
      expect(screen.getByText('No reason')).toBeInTheDocument();
    });
  });
  it('Renders RequestViewer for Acknowledged status', async () => {
    // Mock
    const requestScheduleMock = {
      data: [
        {
          id: 25,
          fromId: 'WEB_000011',
          toId: 'WEB_000010',
          body: 'I want to change my schedule',
          date: '2023-01-17T00:00:00',
          startTime: '2023-01-03T00:00:00',
          endTime: '2023-01-03T00:00:00',
          type: 'Schedule',
          status: 'Acknowledged',
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
    render(<RequestViewer type="Schedule" />);
    // Assert
    await waitFor(() => {
      expect(screen.getByText('Acknowledged')).toBeInTheDocument();
    });
  });
});
