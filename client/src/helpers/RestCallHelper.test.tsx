import { describe, it, vi } from 'vitest';

import axios from 'axios';
import RestCallHelper from './RestCallHelper';
import NetworkResources from '../resources/NetworkResources';

vi.mock('axios');
describe('RestCallHelper', () => {
  afterEach(() => {
    vi.restoreAllMocks();
  });
  it('Get Request Test', async () => {
    // Mock
    (axios.request as jest.Mock).mockReturnValue({
      args: {
        email: 'test1@gmail.com',
        password: 'pass',
      },
      status: 200,
      data: 'data',
    });

    // Arrange
    const requestUrl = `${NetworkResources.TEST_URL}/get`;
    const requestParams = JSON.stringify({
      email: 'test1@gmail.com',
      password: 'pass',
    });

    // Act
    const response = await RestCallHelper.GetRequest(requestUrl, requestParams);

    // Assert
    expect(response).not.toBeNull();
    expect(response.status).toBe(200);
    expect(response.data).not.toBeNull();
  });

  it('Post Request Test', async () => {
    // Mock
    (axios.request as jest.Mock).mockReturnValue({
      args: {
        email: 'test1@gmail.com',
        password: 'pass',
      },
      status: 200,
      data: 'data',
    });
    // Arrange
    const requestUrl = `${NetworkResources.TEST_URL}/post`;
    const requestParams = JSON.stringify({
      email: 'test1@gmail.com',
      password: 'pass',
    });
    const requestBody = JSON.stringify({});

    // Act
    const response = await RestCallHelper.PostRequest(
      requestUrl,
      requestParams,
      requestBody
    );

    // Assert
    expect(response).not.toBeNull();
    expect(response.status).toBe(200);
    expect(response.data).not.toBeNull();
  });

  it('Post Request Test on API', async () => {
    // Mock
    (axios.request as jest.Mock).mockReturnValue({
      status: 200,
      data: 'data',
    });
    // Arrange
    const requestUrl = `${NetworkResources.API_URL}/Login/ValidateLogin`;
    const requestParams = JSON.stringify({
      email: 'webtest@gmail.com',
      password: 'password',
    });
    const requestBody = JSON.stringify({});

    // Act
    const response = await RestCallHelper.PostRequest(
      requestUrl,
      requestParams,
      requestBody
    );

    // Assert
    expect(response).not.toBeNull();
    expect(response.status).toBe(200);
    expect(response.data).not.toBeNull();
  });

  it('Put Request Test', async () => {
    // Mock
    (axios.request as jest.Mock).mockReturnValue({
      args: {
        email: 'test1@gmail.com',
        password: 'pass',
      },
      status: 200,
      data: 'data',
    });
    // Arrange
    const requestUrl = `${NetworkResources.TEST_URL}/put`;
    const requestParams = JSON.stringify({
      email: 'test1@gmail.com',
      password: 'pass',
    });
    const requestBody = JSON.stringify({});

    // Act
    const response = await RestCallHelper.PutRequest(
      requestUrl,
      requestParams,
      requestBody
    );

    // Assert
    expect(response).not.toBeNull();
    expect(response.status).toBe(200);
    expect(response.data).not.toBeNull();
  });

  it('Delete Request Test', async () => {
    // Mock
    (axios.request as jest.Mock).mockReturnValue({
      args: {
        email: 'test1@gmail.com',
        password: 'pass',
      },
      status: 200,
      data: 'data',
    });
    // Arrange
    const requestUrl = `${NetworkResources.TEST_URL}/delete`;
    const requestParams = JSON.stringify({
      email: 'test1@gmail.com',
      password: 'pass',
    });

    // Act
    const response = await RestCallHelper.DeleteRequest(
      requestUrl,
      requestParams
    );

    // Assert
    expect(response).not.toBeNull();
    expect(response.status).toBe(200);
    expect(response.data).not.toBeNull();
  });
});
