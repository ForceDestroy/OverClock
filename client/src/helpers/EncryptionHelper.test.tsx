import { describe, it } from 'vitest';

import EncryptionHelper from './EncryptionHelper';

describe('Encryption Helper', () => {
  it('Encrypts the data properly', async () => {
    const plaintext = 'What is that melody?';
    const ciphertext = 'MIMlJR8bZNKKByLQomu53bJdj8n3OfAI2COiDHgD0Hc=';

    const encryptedData = await EncryptionHelper.Encrypt(plaintext);

    expect(encryptedData).not.toBeNull();
    expect(encryptedData.data).toBe(ciphertext);
  });

  it('Decrypts the data properly', async () => {
    const plaintext = 'What is that melody?';
    const ciphertext = 'MIMlJR8bZNKKByLQomu53bJdj8n3OfAI2COiDHgD0Hc=';

    const decryptedData = await EncryptionHelper.Decrypt(ciphertext);

    expect(decryptedData).not.toBeNull();
    expect(decryptedData).toBe(plaintext);
  });
});
