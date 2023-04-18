/* eslint-disable func-names */
/* eslint-disable no-console */

import axios from 'axios';
import EncryptionHelper from './EncryptionHelper';

export interface ResponseInfo {
  status: number;
  data: string;
}

export default class RestCallHelper {
  static GetRequest = async (
    requestUrl: string,
    requestParams: string
  ): Promise<ResponseInfo> => {
    const options = {
      method: 'GET',
      url: `${requestUrl}Obsolete`,
      params: JSON.parse(requestParams),
      headers: { 'Content-Type': 'application/json' },
    };

    try {
      const requestResponse = await axios.request(options);

      const { status } = requestResponse;
      const data = JSON.stringify(requestResponse.data);
      return { status, data };
    } catch (error) {
      console.log(error);
    }

    return { status: 0, data: '' };
  };

  static PostRequest = async (
    requestUrl: string,
    requestParams: string,
    body: string,
    encryptedEndpoint = false
  ): Promise<ResponseInfo> => {
    // If the endpoint is encrypted, encrypt the request body and params
    if (encryptedEndpoint) {
      const encryptedData = await EncryptionHelper.Encrypt(requestParams);
      const sendBody = JSON.stringify(encryptedData);

      const options = {
        method: 'POST',
        url: requestUrl,
        params: '',
        headers: { 'Content-Type': 'application/json' },
        data: JSON.parse(sendBody),
      };
      try {
        const requestResponse = await axios.request(options);
        const { status } = requestResponse;
        const data = await EncryptionHelper.Decrypt(requestResponse.data);
        return { status, data };
      } catch (error) {
        console.log(error);
      }

      return { status: 0, data: '' };
    }

    // If the endpoint is not encrypted, send the request body and params as is
    const options = {
      method: 'POST',
      url: `${requestUrl}Obsolete`,
      params: JSON.parse(requestParams),
      headers: { 'Content-Type': 'application/json' },
      data: JSON.parse(body),
    };

    try {
      const requestResponse = await axios.request(options);
      const { status } = requestResponse;
      const data = JSON.stringify(requestResponse.data);
      return { status, data };
    } catch (error) {
      console.log(error);
    }

    return { status: 0, data: '' };
  };

  static PutRequest = async (
    requestUrl: string,
    requestParams: string,
    body: string,
    encryptedEndpoint = false
  ): Promise<ResponseInfo> => {
    // If the endpoint is encrypted, encrypt the request body and params
    if (encryptedEndpoint) {
      const encryptedData = await EncryptionHelper.Encrypt(requestParams);
      const sendBody = JSON.stringify(encryptedData);

      const options = {
        method: 'PUT',
        url: requestUrl,
        params: '',
        headers: { 'Content-Type': 'application/json' },
        data: JSON.parse(sendBody),
      };
      try {
        const requestResponse = await axios.request(options);
        const { status } = requestResponse;
        const data = await EncryptionHelper.Decrypt(requestResponse.data);
        return { status, data };
      } catch (error) {
        console.log(error);
      }

      return { status: 0, data: '' };
    }

    // If the endpoint is not encrypted, send the request body and params as is
    const options = {
      method: 'PUT',
      url: `${requestUrl}Obsolete`,
      params: JSON.parse(requestParams),
      headers: { 'Content-Type': 'application/json' },
      data: JSON.parse(body),
    };

    try {
      const requestResponse = await axios.request(options);
      const { status } = requestResponse;
      const data = JSON.stringify(requestResponse.data);
      return { status, data };
    } catch (error) {
      console.log(error);
    }

    return { status: 0, data: '' };
  };

  static DeleteRequest = async (
    requestUrl: string,
    requestParams: string,
    encryptedEndpoint = false
  ): Promise<ResponseInfo> => {
    // If the endpoint is encrypted, encrypt the request body and params
    if (encryptedEndpoint) {
      const encryptedData = await EncryptionHelper.Encrypt(requestParams);
      const sendBody = JSON.stringify(encryptedData);

      const options = {
        method: 'DELETE',
        url: requestUrl,
        params: '',
        headers: { 'Content-Type': 'application/json' },
        data: JSON.parse(sendBody),
      };
      try {
        const requestResponse = await axios.request(options);
        const { status } = requestResponse;
        const data = await EncryptionHelper.Decrypt(requestResponse.data);
        return { status, data };
      } catch (error) {
        console.log(error);
      }

      return { status: 0, data: '' };
    }

    const options = {
      method: 'DELETE',
      url: `${requestUrl}Obsolete`,
      params: JSON.parse(requestParams),
      headers: { 'Content-Type': 'application/json' },
    };

    try {
      const requestResponse = await axios.request(options);
      const { status } = requestResponse;
      const data = JSON.stringify(requestResponse.data);
      return { status, data };
    } catch (error) {
      console.log(error);
    }

    return { status: 0, data: '' };
  };
}
