import { render, screen } from '@testing-library/react';
import { describe, it } from 'vitest';

import HomeBox from './HomeBox';

describe('HomeBox', () => {
  it('Renders HomeBox to match snapshot', () => {
    // Arrange
    const snap = render(<HomeBox />);
    // Assert
    expect(snap).toMatchSnapshot();
  });
  it('Renders the navigation button', () => {
    // Arrange
    render(<HomeBox />);

    // Act
    // No Act for this Test

    // Assert
    const loginButton = screen.getByText('Get Started');
    expect(loginButton).not.toBeNull();
  });
});
