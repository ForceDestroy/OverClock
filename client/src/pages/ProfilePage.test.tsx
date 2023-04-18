import { render, screen, waitFor } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import axios from 'axios';
import { MemoryRouter } from 'react-router-dom';
import { describe, it, vi } from 'vitest';

import { App } from '../App';
import AuthTokenHelper from '../helpers/AuthTokenHelper';

vi.mock('axios');
vi.mock('../helpers/AuthTokenHelper');

describe('ProfilePage', () => {
  afterEach(() => {
    vi.clearAllMocks();
    vi.resetAllMocks();
  });
  it('Renders profile page to match snapshot', async () => {
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
        newPassword: '',
        confirmPassword: '',
      },
      status: 200,
      statusText: 'OK',
    });
    (AuthTokenHelper.validateToken as jest.Mock).mockReturnValue(
      'VerySecretToken'
    );

    // Arrange
    const snap = render(
      <MemoryRouter initialEntries={['/ProfilePage']}>
        <App />
      </MemoryRouter>
    );

    // Assert
    expect(snap).toMatchSnapshot();
  });
  it('Renders profile page after editing all fields', async () => {
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
        newPassword: '',
        confirmPassword: '',
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
      <MemoryRouter initialEntries={['/ProfilePage']}>
        <App />
      </MemoryRouter>
    );

    // Act
    const nameField = screen.getByTestId('profileName');
    await user.type(nameField, ' web');
    const dobField = screen.getByTestId('profileBirthday');
    await user.type(dobField, '0001-01-01T00:00:00');
    const emailField = screen.getByTestId('profileEmail');
    await user.type(emailField, 'web');
    const phoneField = screen.getByTestId('profilePhoneNumber');
    await user.type(phoneField, '123');
    const addressField = screen.getByTestId('profileAddress');
    await user.type(addressField, ' Street');
    const newPasswordField = screen.getByTestId('profileNewPassword');
    await user.type(newPasswordField, 'newpassword');
    const confirmPasswordField = screen.getByTestId('profileConfirmPassword');
    await user.type(confirmPasswordField, 'newpassword');

    const saveButton = screen.getByTestId('saveButton');
    await user.click(saveButton);
    // Assert
    await waitFor(() => {
      expect(screen.getByText('web test web')).toBeInTheDocument();
      expect(screen.getByTestId('profileName')).toBeInTheDocument();
      expect(screen.getByTestId('profileEmail')).toBeInTheDocument();
      expect(screen.getByTestId('profileUserID')).toBeInTheDocument();
      expect(screen.getByTestId('profilePhoneNumber')).toBeInTheDocument();
      expect(screen.getByTestId('profileBirthday')).toBeInTheDocument();
      expect(screen.getByTestId('profileAddress')).toBeInTheDocument();
      expect(screen.getByTestId('profilePassword')).toBeInTheDocument();
      expect(screen.getByTestId('profileNewPassword')).toBeInTheDocument();
      expect(screen.getByTestId('profileConfirmPassword')).toBeInTheDocument();
      expect(
        screen.queryByText('Passwords do not match')
      ).not.toBeInTheDocument();
    });
  });

  it('Renders profile page after editing a field and cancelling', async () => {
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
        newPassword: '',
        confirmPassword: '',
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
      <MemoryRouter initialEntries={['/ProfilePage']}>
        <App />
      </MemoryRouter>
    );

    // Act
    const nameField = screen.getByTestId('profileName');
    await user.type(nameField, 'web web');

    const cancelButton = screen.getByTestId('cancelButton');
    await user.click(cancelButton);
    // Assert
    await waitFor(() => {
      expect(screen.getByText('web test')).toBeInTheDocument();
    });
  });
  it('Renders profile page after entering mismatched passwords', async () => {
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
        newPassword: '',
        confirmPassword: '',
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
      <MemoryRouter initialEntries={['/ProfilePage']}>
        <App />
      </MemoryRouter>
    );

    // Act
    const newPasswordField = screen.getByTestId('profileNewPassword');
    await user.type(newPasswordField, 'newpassword');
    const confirmPasswordField = screen.getByTestId('profileConfirmPassword');
    await user.type(confirmPasswordField, 'incorrect');

    const saveButton = screen.getByTestId('saveButton');
    await user.click(saveButton);
    // Assert
    await waitFor(() => {
      expect(screen.getByText('Passwords do not match')).toBeInTheDocument();
    });
  });
});
