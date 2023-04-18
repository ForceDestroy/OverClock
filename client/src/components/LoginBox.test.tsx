import { render, screen, waitFor } from '@testing-library/react';
import { MemoryRouter } from 'react-router-dom';
import userEvent from '@testing-library/user-event';
import { describe, it, vi } from 'vitest';

import axios from 'axios';
import { App } from '../App';
import AuthTokenHelper from '../helpers/AuthTokenHelper';

vi.mock('axios');
vi.mock('../helpers/AuthTokenHelper');
describe('LoginBox', () => {
  afterEach(() => {
    vi.restoreAllMocks();
  });
  it('Renders LoginBox to match snapshot', () => {
    // Arrange
    const snap = render(
      <MemoryRouter initialEntries={['/Login']}>
        <App />
      </MemoryRouter>
    );

    // Assert
    expect(snap).toMatchSnapshot();
  });
  it('Renders both input fields and login button', () => {
    // Arrange
    render(
      <MemoryRouter initialEntries={['/Login']}>
        <App />
      </MemoryRouter>
    );

    // Act

    // Assert
    const emailField = screen.getByPlaceholderText('Email');
    expect(emailField).not.toBeNull();
    const passwordField = screen.getByPlaceholderText('Password');
    expect(passwordField).not.toBeNull();
    const loginButton = screen.getByText('Login');
    expect(loginButton).not.toBeNull();
  });

  it('Accepts input in email field', async () => {
    // Arrange
    const user = userEvent.setup();
    render(
      <MemoryRouter initialEntries={['/Login']}>
        <App />
      </MemoryRouter>
    );

    // Act
    const emailField = screen.getByPlaceholderText('Email');
    await user.click(emailField);
    await user.keyboard('foo');

    // Assert
    expect(emailField).toHaveValue('foo');
  });

  it('Accepts hidden input in password field', async () => {
    // Arrange
    const user = userEvent.setup();
    render(
      <MemoryRouter initialEntries={['/Login']}>
        <App />
      </MemoryRouter>
    );

    // Act
    const passwordField = screen.getByPlaceholderText('Password');
    await user.click(passwordField);
    await user.keyboard('foo');

    // Assert
    expect(passwordField).toHaveValue('foo');
    expect(passwordField.textContent).not.toBe('foo');
  });

  it('Renders notification on invalid email', async () => {
    // Mock
    (axios.request as jest.Mock).mockReturnValue({
      args: {
        email: 'foo',
        password: 'bar',
      },
      status: 404,
      data: 'data',
    });
    (AuthTokenHelper.saveToLocalStorage as jest.Mock).mockReturnValue(
      'VerySecretToken'
    );
    // Arrange
    const user = userEvent.setup();
    render(
      <MemoryRouter initialEntries={['/Login']}>
        <App />
      </MemoryRouter>
    );

    // Act
    const emailField = screen.getByPlaceholderText('Email');
    await user.click(emailField);
    await user.keyboard('foo');

    const passwordField = screen.getByPlaceholderText('Password');
    await user.click(passwordField);
    await user.keyboard('foo');

    const loginButton = screen.getByText('Log In');
    await user.click(loginButton);

    // Assert
    await waitFor(() =>
      expect(screen.getByText('Invalid email or password')).toBeInTheDocument()
    );
  });
  it('Renders notification on invalid password', async () => {
    // Mock
    (axios.request as jest.Mock).mockReturnValue({
      args: {
        email: 'webtest@gmail.com',
        password: 'foo',
      },
      status: 400,
      data: 'data',
    });
    (AuthTokenHelper.saveToLocalStorage as jest.Mock).mockReturnValue(
      'VerySecretToken'
    );
    // Arrange
    const user = userEvent.setup();
    render(
      <MemoryRouter initialEntries={['/Login']}>
        <App />
      </MemoryRouter>
    );

    // Act
    const emailField = screen.getByPlaceholderText('Email');
    await user.click(emailField);
    await user.keyboard('test1@gmail.com');

    const passwordField = screen.getByPlaceholderText('Password');
    await user.click(passwordField);
    await user.keyboard('foo');

    const loginButton = screen.getByText('Log In');
    await user.click(loginButton);

    // Assert
    await waitFor(() =>
      expect(screen.getByText('Invalid email or password')).toBeInTheDocument()
    );
  });
});
