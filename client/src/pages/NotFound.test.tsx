import { render, screen, waitFor, act } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import { MemoryRouter } from 'react-router-dom';
import { describe, it, vi } from 'vitest';
import AuthTokenHelper from '../helpers/AuthTokenHelper';
import { App } from '../App';

vi.mock('axios');
vi.mock('../helpers/AuthTokenHelper');

describe('NotFound', () => {
  afterEach(() => {
    vi.clearAllMocks();
    vi.resetAllMocks();
  });
  it('Renders Not Found to match snapshot', () => {
    // Arrange
    const snap = render(
      <MemoryRouter initialEntries={['/something']}>
        <App />
      </MemoryRouter>
    );

    // Assert
    expect(snap).toMatchSnapshot();
  });
  it('Renders Not Found on a non-existent page', () => {
    // Arrange
    render(
      <MemoryRouter initialEntries={['/something']}>
        <App />
      </MemoryRouter>
    );

    // Act
    // No Act for this Test

    // Assert
    expect(screen.getByText('This page does not exist')).toBeInTheDocument();
  });
  it('Renders Not Found on a non-existent page and navigate back home', async () => {
    // Arrange
    render(
      <MemoryRouter initialEntries={['/something']}>
        <App />
      </MemoryRouter>
    );
    const user = userEvent.setup();

    // Act
    const homeButton = screen.getByTestId('goHomeButton');
    await user.click(homeButton);

    // Assert
    await waitFor(() => {
      const links: HTMLAnchorElement[] = screen.getAllByRole('link');
      expect(links[0].href).toContain('/');
    });
  });
  it('Renders Not Found on a non-existent page and navigate to EmployeeDashboard if logged in', async () => {
    // Mock
    (AuthTokenHelper.getFromLocalStorage as jest.Mock).mockReturnValue(
      'VerySecureToken1234567890'
    );

    // Arrange
    // const user = userEvent.setup();

    render(
      <MemoryRouter initialEntries={['/something']}>
        <App />
      </MemoryRouter>
    );

    // Act
    const homeButton = screen.getByTestId('goHomeButton') as HTMLAnchorElement;

    // Assert
    await waitFor(() => {
      expect(homeButton.href).toContain('/EmployeeDashboard');
    });
  });
});
