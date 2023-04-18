import { render, screen, waitFor, cleanup } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import axios from 'axios';
import { MemoryRouter } from 'react-router-dom';
import { describe, it, vi } from 'vitest';
import { App } from '../App';

import AuthTokenHelper from '../helpers/AuthTokenHelper';

vi.mock('axios');
vi.mock('../helpers/AuthTokenHelper');

describe('Payslip', () => {
  beforeEach(() => {
    vi.clearAllMocks();
    cleanup();
  });
  afterEach(() => {
    vi.clearAllMocks();
    vi.resetAllMocks();
  });
  it('Renders Payslip to match snapshot', async () => {
    // Mock
    (axios.request as jest.Mock).mockReturnValue({
      data: [
        {
          address: 'somewhere',
          amountAccumulated: 2,
          ei: 1,
          eiAccumulated: 2,
          endDate: '2023-01-02T00:00:00',
          fit: 1,
          fitAccumulated: 2,
          grossAmount: 1,
          hoursAccumulated: 2,
          hoursWorked: 1,
          issueDate: '2023-01-05T00:00:00',
          name: 'test',
          netAmount: 1,
          netAccumulated: 3,
          qcpit: 1,
          qcpitAccumulated: 2,
          qpip: 1,
          qpipAccumulated: 2,
          qpp: 1,
          qppAccumulated: 2,
          startDate: '2023-01-01T00:00:00',
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
      <MemoryRouter initialEntries={['/employeedashboard/payslip']}>
        <App />
      </MemoryRouter>
    );

    // Assert
    expect(snap).toMatchSnapshot();
  });
  it('Renders Payslip for Employee', async () => {
    // Mock
    (axios.request as jest.Mock).mockReturnValue({
      data: [
        {
          address: 'somewhere',
          amountAccumulated: 2,
          ei: 1,
          eiAccumulated: 2,
          endDate: '2023-01-02T00:00:00',
          fit: 1,
          fitAccumulated: 2,
          grossAmount: 1,
          hoursAccumulated: 2,
          hoursWorked: 1,
          issueDate: '2023-01-05T00:00:00',
          name: 'test',
          netAmount: 1,
          netAccumulated: 3,
          qcpit: 1,
          qcpitAccumulated: 2,
          qpip: 1,
          qpipAccumulated: 2,
          qpp: 1,
          qppAccumulated: 2,
          startDate: '2023-01-01T00:00:00',
        },
      ],
      status: 200,
      statusText: 'OK',
    });

    (AuthTokenHelper.validateToken as jest.Mock).mockReturnValue(
      'VerySecretToken'
    );

    // Arrange
    render(
      <MemoryRouter initialEntries={['/employeedashboard/payslip']}>
        <App />
      </MemoryRouter>
    );

    await waitFor(() => {
      expect(screen.getByText('Payslips')).toBeInTheDocument();
      expect(screen.getByText('test')).toBeInTheDocument();
    });
  });
  it('Renders Payslip for Employee and clicks on the row to generate pdf', async () => {
    // Mock
    (axios.request as jest.Mock).mockReturnValue({
      data: [
        {
          address: 'somewhere',
          amountAccumulated: 2,
          ei: 1,
          eiAccumulated: 2,
          endDate: '2023-01-02T00:00:00',
          fit: 1,
          fitAccumulated: 2,
          grossAmount: 1,
          hoursAccumulated: 2,
          hoursWorked: 1,
          issueDate: '2023-01-05T00:00:00',
          name: 'test',
          netAmount: 1,
          netAccumulated: 3,
          qcpit: 1,
          qcpitAccumulated: 2,
          qpip: 1,
          qpipAccumulated: 2,
          qpp: 1,
          qppAccumulated: 2,
          startDate: '2023-01-01T00:00:00',
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
      <MemoryRouter initialEntries={['/employeedashboard/payslip']}>
        <App />
      </MemoryRouter>
    );
    await waitFor(() => {
      const row = screen.getByText('test');
      user.click(row);
    });

    await waitFor(() => {
      expect(screen.getByText('Payslips')).toBeInTheDocument();
    });
  });
});
