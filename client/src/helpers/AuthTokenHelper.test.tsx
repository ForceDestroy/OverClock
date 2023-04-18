import { describe, it } from 'vitest';

import AuthTokenHelper from './AuthTokenHelper';

// Tests
describe('AuthTokenHelper', () => {
  it('Retrieves the token from the local storage', () => {
    // Arrange
    const token = 'thisISaVERYsecureTOKEN123456890';
    localStorage.setItem('JWT_OC', token);

    // Act
    const retrievedToken = AuthTokenHelper.getFromLocalStorage();

    // Assert
    expect(retrievedToken).toBe(token);

    // Dispose
    localStorage.removeItem('JWT_OC');
  });

  it('Does not retrieve the token from the local storage if its not there', () => {
    // Arrange

    // Act
    const retrievedToken = AuthTokenHelper.getFromLocalStorage();

    // Assert
    expect(retrievedToken).toBeNull();

    // Dispose
    localStorage.removeItem('JWT_OC');
  });

  it('Saves the token to the local storage', () => {
    // Arrange
    const token = 'thisISaVERYsecureTOKEN123456890';

    // Act
    AuthTokenHelper.saveToLocalStorage(token);

    // Assert
    const retrievedToken = localStorage.getItem('JWT_OC');
    expect(retrievedToken).toBe(token);

    // Dispose
    localStorage.removeItem('JWT_OC');
  });

  it('Saves the token with timestamp to the local storage', () => {
    // Arrange
    const token = 'thisISaVERYsecureTOKEN123456890';
    const timestamp = '0';

    // Act
    AuthTokenHelper.saveToLocalStorage(token, timestamp);

    // Assert
    const retrievedToken = localStorage.getItem('JWT_OC');
    expect(retrievedToken).toBe(token);
    const retrievedTimestamp = localStorage.getItem('JWT_OC_TIMESTAMP');
    expect(retrievedTimestamp).toBe(timestamp);

    // Dispose
    localStorage.removeItem('JWT_OC');
  });

  it('Updates the token in the local storage', () => {
    // Arrange
    const token = 'thisISaVERYsecureTOKEN123456890';
    localStorage.setItem('JWT_OC', token);

    const newToken = 'THISisNOTaVERYsecureTOKEN0987654321';

    // Act
    AuthTokenHelper.updateInLocalStorage(newToken);

    // Assert
    const retrievedToken = localStorage.getItem('JWT_OC');
    expect(retrievedToken).not.toBe(token);
    expect(retrievedToken).toBe(newToken);

    // Dispose
    localStorage.removeItem('JWT_OC');
  });

  it('Updates the token with timestamp in the local storage', () => {
    // Arrange
    const token = 'thisISaVERYsecureTOKEN123456890';
    localStorage.setItem('JWT_OC', token);

    const newToken = 'THISisNOTaVERYsecureTOKEN0987654321';
    const timestamp = '1234567890';

    // Act
    AuthTokenHelper.updateInLocalStorage(newToken, timestamp);

    // Assert
    const retrievedToken = localStorage.getItem('JWT_OC');
    expect(retrievedToken).not.toBe(token);
    expect(retrievedToken).toBe(newToken);

    // Dispose
    localStorage.removeItem('JWT_OC');
  });

  it('Does not update the token in the local storage if its not there', () => {
    // Arrange
    const newToken = 'THISisNOTaVERYsecureTOKEN0987654321';

    // Act
    AuthTokenHelper.updateInLocalStorage(newToken);

    // Assert
    const retrievedToken = localStorage.getItem('JWT_OC');
    expect(retrievedToken).toBeNull();

    // Dispose
    localStorage.removeItem('JWT_OC');
  });

  it('Removes the token from the local storage', () => {
    // Arrange
    const token = 'thisISaVERYsecureTOKEN123456890';
    localStorage.setItem('JWT_OC', token);

    // Act
    AuthTokenHelper.removeFromLocalStorage();

    // Assert
    const retrievedToken = localStorage.getItem('JWT_OC');
    expect(retrievedToken).not.toBe(token);
    expect(retrievedToken).toBeNull();

    // Dispose
    localStorage.removeItem('JWT_OC');
  });

  it('Does not remove the token from the local storage if its not there', () => {
    // Arrange
    const token = localStorage.getItem('JWT_OC');
    expect(token).toBeNull();

    // Act
    AuthTokenHelper.removeFromLocalStorage();

    // Assert
    const retrievedToken = localStorage.getItem('JWT_OC');
    expect(retrievedToken).toBeNull();

    // Dispose
    localStorage.removeItem('JWT_OC');
  });

  it('Validates the token from the local storage', () => {
    // Arrange
    const token = 'thisISaVERYsecureTOKEN123456890';
    localStorage.setItem('JWT_OC', token);
    localStorage.setItem('JWT_OC_TIMESTAMP', new Date().getTime().toString());

    // Act
    const retrievedToken = AuthTokenHelper.validateToken();

    // Assert
    expect(retrievedToken).toBe(true);

    // Dispose
    localStorage.removeItem('JWT_OC');
  });

  it('Invalidates the token from the local storage', () => {
    // Arrange
    const token = 'thisISaVERYsecureTOKEN123456890';
    localStorage.setItem('JWT_OC', token);
    localStorage.setItem('JWT_OC_TIMESTAMP', '0');

    // Act
    const retrievedToken = AuthTokenHelper.validateToken();

    // Assert
    expect(retrievedToken).toBe(false);

    // Dispose
    localStorage.removeItem('JWT_OC');
  });

  it('Saves the name into local storage', () => {
    // Arrange
    const name = 'John Doe';
    const token = 'thisISaVERYsecureTOKEN123456890';
    localStorage.setItem('JWT_OC', token);

    // Act
    AuthTokenHelper.setName(name);

    // Assert
    const retrievedName = localStorage.getItem('JWT_OC_NAME');
    expect(retrievedName).toBe(name);

    // Dispose
    localStorage.removeItem('JWT_OC_NAME');
  });

  it('Retrieves the name from local storage', () => {
    // Arrange
    const name = 'John Doe';
    localStorage.setItem('JWT_OC_NAME', name);
    const token = 'thisISaVERYsecureTOKEN123456890';
    localStorage.setItem('JWT_OC', token);

    // Act
    const retrievedName = AuthTokenHelper.getName();

    // Assert
    expect(retrievedName).toBe(name);

    // Dispose
    localStorage.removeItem('JWT_OC_NAME');
  });

  it('Saves the access level into local storage', () => {
    // Arrange
    const accessLevel = 'admin';
    const token = 'thisISaVERYsecureTOKEN123456890';
    localStorage.setItem('JWT_OC', token);

    // Act
    AuthTokenHelper.setAccessLevel(accessLevel);

    // Assert
    const retrievedAccessLevel = localStorage.getItem('JWT_OC_ACCESS_LEVEL');
    expect(retrievedAccessLevel).toBe(accessLevel);

    // Dispose
    localStorage.removeItem('JWT_OC_ACCESS_LEVEL');
  });

  it('Retrieves the access level from local storage', () => {
    // Arrange
    const accessLevel = 'admin';
    localStorage.setItem('JWT_OC_ACCESS_LEVEL', accessLevel);
    const token = 'thisISaVERYsecureTOKEN123456890';
    localStorage.setItem('JWT_OC', token);

    // Act
    const retrievedAccessLevel = AuthTokenHelper.getAccessLevel();

    // Assert
    expect(retrievedAccessLevel).toBe(accessLevel);

    // Dispose
    localStorage.removeItem('JWT_OC_ACCESS_LEVEL');
  });

  it('Saves the user id into local storage', () => {
    // Arrange
    const userId = '1234567890';
    const token = 'thisISaVERYsecureTOKEN123456890';
    localStorage.setItem('JWT_OC', token);

    // Act
    AuthTokenHelper.setUserId(userId);

    // Assert
    const retrievedAccessLevel = localStorage.getItem('JWT_OC_USER_ID');
    expect(retrievedAccessLevel).toBe(userId);

    // Dispose
    localStorage.removeItem('JWT_OC_USER_ID');
  });

  it('Retrieves the user id from local storage', () => {
    // Arrange
    const userId = '1234567890';
    localStorage.setItem('JWT_OC_USER_ID', userId);
    const token = 'thisISaVERYsecureTOKEN123456890';
    localStorage.setItem('JWT_OC', token);

    // Act
    const retrievedUserId = AuthTokenHelper.getUserId();

    // Assert
    expect(retrievedUserId).toBe(userId);

    // Dispose
    localStorage.removeItem('JWT_OC_USER_ID');
  });
});
