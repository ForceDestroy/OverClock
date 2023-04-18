import { render, screen } from '@testing-library/react';
import { MemoryRouter } from 'react-router-dom';
import { describe, it } from 'vitest';

import { App } from '../App';

describe('Login', () => {
  it('Renders Login to match snapshot', () => {
    // Arrange
    const snap = render(
      <MemoryRouter initialEntries={['/Login']}>
        <App />
      </MemoryRouter>
    );

    // Assert
    expect(snap).toMatchSnapshot();
  });
  it('Renders LoginBox', () => {
    // Arrange
    render(
      <MemoryRouter initialEntries={['/Login']}>
        <App />
      </MemoryRouter>
    );

    // Act
    // No Act for this Test

    // Assert
    expect(screen.getByAltText('login_image')).toBeInTheDocument();
    expect(screen.getByPlaceholderText('Email')).toBeInTheDocument();
    expect(screen.getByPlaceholderText('Password')).toBeInTheDocument();
    expect(screen.getByText('Login')).toBeInTheDocument();
  });
});
