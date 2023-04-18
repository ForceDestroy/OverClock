/* eslint-disable consistent-return */
export default class AuthTokenHelper {
  static getFromLocalStorage = (): string | null => {
    try {
      if (localStorage.getItem('JWT_OC') === null) {
        throw ReferenceError();
      }
      const token = localStorage.getItem('JWT_OC');

      return token;
    } catch (error: unknown) {
      return null;
    }
  };

  static saveToLocalStorage = (token: string, timestamp?: string) => {
    try {
      localStorage.setItem('JWT_OC', token);
      if (timestamp) {
        localStorage.setItem('JWT_OC_TIMESTAMP', timestamp);
      } else {
        localStorage.setItem(
          'JWT_OC_TIMESTAMP',
          new Date().getTime().toString()
        );
      }
    } catch (error: unknown) {
      return null;
    }
  };

  static updateInLocalStorage = (token: string, timestamp?: string) => {
    try {
      if (localStorage.getItem('JWT_OC') === null) {
        throw ReferenceError();
      }
      localStorage.removeItem('JWT_OC');
      localStorage.setItem('JWT_OC', token);
      localStorage.removeItem('JWT_OC_TIMESTAMP');
      if (timestamp) {
        localStorage.setItem('JWT_OC_TIMESTAMP', timestamp);
      } else {
        localStorage.setItem(
          'JWT_OC_TIMESTAMP',
          new Date().getTime().toString()
        );
      }
    } catch (error: unknown) {
      return null;
    }
  };

  static removeFromLocalStorage = () => {
    try {
      if (localStorage.getItem('JWT_OC') === null) {
        throw ReferenceError();
      }
      localStorage.removeItem('JWT_OC');
      localStorage.removeItem('JWT_OC_TIMESTAMP');
      localStorage.removeItem('JWT_OC_NAME');
      localStorage.removeItem('JWT_OC_ACCESS_LEVEL');
      localStorage.removeItem('JWT_OC_USER_ID');
    } catch (error: unknown) {
      return null;
    }
    return null;
  };

  static validateToken = (): boolean => {
    try {
      if (localStorage.getItem('JWT_OC') === null) {
        throw ReferenceError();
      }
      const tokenTimestamp = localStorage.getItem('JWT_OC_TIMESTAMP');
      const timestamp = Date.parse(tokenTimestamp || '');

      if (timestamp + 30 * 60 * 1000 < new Date().getTime()) {
        this.removeFromLocalStorage();
        return false;
      }

      return true;
    } catch (error: unknown) {
      return false;
    }
  };

  static setName = (name: string) => {
    try {
      if (localStorage.getItem('JWT_OC') === null) {
        throw ReferenceError();
      }
      localStorage.setItem('JWT_OC_NAME', name);
    } catch (error: unknown) {
      return null;
    }
  };

  static getName = (): string | null => {
    try {
      if (localStorage.getItem('JWT_OC') === null) {
        throw ReferenceError();
      }
      const name = localStorage.getItem('JWT_OC_NAME');

      return name;
    } catch (error: unknown) {
      return null;
    }
  };

  static setAccessLevel = (accessLevel: string) => {
    try {
      if (localStorage.getItem('JWT_OC') === null) {
        throw ReferenceError();
      }
      localStorage.setItem('JWT_OC_ACCESS_LEVEL', accessLevel);
    } catch (error: unknown) {
      return null;
    }
  };

  static getAccessLevel = (): string | null => {
    try {
      if (localStorage.getItem('JWT_OC') === null) {
        throw ReferenceError();
      }
      const accessLevel = localStorage.getItem('JWT_OC_ACCESS_LEVEL');

      return accessLevel;
    } catch (error: unknown) {
      return null;
    }
  };

  static setUserId = (userId: string) => {
    try {
      if (localStorage.getItem('JWT_OC') === null) {
        throw ReferenceError();
      }
      localStorage.setItem('JWT_OC_USER_ID', userId);
    } catch (error: unknown) {
      return null;
    }
  };

  static getUserId = (): string | null => {
    try {
      if (localStorage.getItem('JWT_OC') === null) {
        throw ReferenceError();
      }
      const userId = localStorage.getItem('JWT_OC_USER_ID');

      return userId;
    } catch (error: unknown) {
      return null;
    }
  };
}
