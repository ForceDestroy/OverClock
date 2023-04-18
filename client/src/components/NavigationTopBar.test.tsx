import { render, screen } from '@testing-library/react';
import axios from 'axios';
import { MemoryRouter } from 'react-router';
import { describe, it, vi } from 'vitest';

import NavigationTopBar from './NavigationTopBar';
import AuthTokenHelper from '../helpers/AuthTokenHelper';

vi.mock('axios');
vi.mock('../helpers/AuthTokenHelper');

describe('NavigationTopBar', () => {
  afterEach(() => {
    vi.clearAllMocks();
    vi.resetAllMocks();
  });
  it('Renders the Navigation Top Bar to match snapshot', () => {
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

    // Arrange
    const snap = render(
      <MemoryRouter>
        <NavigationTopBar />
      </MemoryRouter>
    );

    // Assert
    expect(snap).toMatchSnapshot();
  });
  it('Renders the home button in the Navigation Bar and navigates to home', () => {
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

    // Arrange
    render(
      <MemoryRouter>
        <NavigationTopBar />
      </MemoryRouter>
    );

    // Assert
    expect(screen.getByRole('link', { name: /home/i })).toBeInTheDocument();
  });
  it('Renders calls the remove token after clicking on the logout button', () => {
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

    (AuthTokenHelper.getName as jest.Mock).mockReturnValue('web test');
    // Arrange
    render(
      <MemoryRouter>
        <NavigationTopBar />
      </MemoryRouter>
    );

    // Assert
    expect(screen.getByRole('link', { name: /home/i })).toBeInTheDocument();
    expect(screen.getByText('web test')).toBeInTheDocument();
  });
  it('Renders calls the remove token after clicking on the calendar', () => {
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

    // Arrange
    render(
      <MemoryRouter>
        <NavigationTopBar />
      </MemoryRouter>
    );

    // Assert
    expect(screen.getByRole('link', { name: /Calendar/i })).toBeInTheDocument();
  });
  it('Renders calls the remove token after clicking on the my job button', () => {
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

    // Arrange
    render(
      <MemoryRouter>
        <NavigationTopBar />
      </MemoryRouter>
    );

    // Assert
    expect(screen.getByRole('link', { name: /MyJob/i })).toBeInTheDocument();
  });
  it('Renders calls the remove token after clicking on the management page', () => {
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

    (AuthTokenHelper.getAccessLevel as jest.Mock).mockReturnValue('1');

    // Arrange
    render(
      <MemoryRouter>
        <NavigationTopBar />
      </MemoryRouter>
    );

    // Assert
    expect(
      screen.getByRole('link', { name: /Management/i })
    ).toBeInTheDocument();
  });
});
