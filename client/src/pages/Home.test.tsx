import { render, screen } from '@testing-library/react';
import { describe, it } from 'vitest';

import { WrappedApp } from '../App';

describe('Home', () => {
  it('Renders Home to match snapshot', () => {
    // Arrange
    const snap = render(<WrappedApp />);

    // Assert
    expect(snap).toMatchSnapshot();
  });
  it('Renders Home Box Elements', () => {
    // Arrange
    render(<WrappedApp />);

    // Act
    // No Act for this Test

    // Assert
    expect(screen.getByAltText('overclock_logo')).toBeInTheDocument();
    expect(
      screen.getByText('Workplace Management Accelerated')
    ).toBeInTheDocument();
    expect(screen.getByText('Get Started')).toBeInTheDocument();
  });

  it('Redirects to the login page', async () => {
    // Arrange
    render(<WrappedApp />);

    // Act
    const button = screen.getByRole('link', { name: 'Get Started' });

    // Assert
    expect(button).toHaveAttribute('href', '/Login');
  });
});
