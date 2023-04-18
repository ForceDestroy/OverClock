import CryptoJS from 'crypto-js';
import NetworkResources from '../resources/NetworkResources';

export interface EncryptedData {
  data: string;
}

export default class EncryptionHelper {
  static Decrypt = async (ciphertext: string): Promise<string> => {
    const key = CryptoJS.enc.Utf8.parse(NetworkResources.ENCRYPTION_KEY);
    const iv = CryptoJS.enc.Utf8.parse(NetworkResources.ENCRYPTION_IV);

    const decrypted = CryptoJS.AES.decrypt(ciphertext, key, {
      keySize: 128 / 8,
      iv,
      mode: CryptoJS.mode.CBC,
      padding: CryptoJS.pad.Pkcs7,
    }).toString(CryptoJS.enc.Utf8);

    return decrypted;
  };

  static Encrypt = async (plaintext: string): Promise<EncryptedData> => {
    const key = CryptoJS.enc.Utf8.parse(NetworkResources.ENCRYPTION_KEY);
    const iv = CryptoJS.enc.Utf8.parse(NetworkResources.ENCRYPTION_IV);

    const encrypted = CryptoJS.AES.encrypt(
      CryptoJS.enc.Utf8.parse(plaintext),
      key,
      {
        keySize: 128 / 8,
        iv,
        mode: CryptoJS.mode.CBC,
        padding: CryptoJS.pad.Pkcs7,
      }
    ).toString();

    return { data: encrypted };
  };
}
