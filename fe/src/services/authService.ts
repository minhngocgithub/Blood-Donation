export interface LoginData {
  email: string;
  password: string;
  rememberMe: boolean;
}

export interface RegisterData {
  fullName: string;
  email: string;
  phone: string;
  password: string;
  confirmPassword: string;
  agreeToTerms: boolean;
}

export interface User {
  id: string;
  fullName: string;
  email: string;
  role: string;
}

export interface AuthResponse {
  success: boolean;
  message: string;
  user?: User;
  errors?: string[];
}

export interface CheckAuthResponse {
  success: boolean;
  isAuthenticated: boolean;
  user?: User;
}

class AuthService {
  private baseUrl = '/api/auth';

  async login(loginData: LoginData): Promise<AuthResponse> {
    try {
      const response = await fetch(`${this.baseUrl}/login`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(loginData),
        credentials: 'include'
      });

      const data = await response.json();
      return data;
    } catch (error) {
      console.error('Login error:', error);
      throw new Error('Có lỗi xảy ra khi đăng nhập');
    }
  }

  async register(registerData: RegisterData): Promise<AuthResponse> {
    try {
      const response = await fetch(`${this.baseUrl}/register`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(registerData),
        credentials: 'include'
      });

      const data = await response.json();
      return data;
    } catch (error) {
      console.error('Registration error:', error);
      throw new Error('Có lỗi xảy ra khi đăng ký');
    }
  }

  async logout(): Promise<AuthResponse> {
    try {
      const response = await fetch(`${this.baseUrl}/logout`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        credentials: 'include'
      });

      const data = await response.json();
      return data;
    } catch (error) {
      console.error('Logout error:', error);
      throw new Error('Có lỗi xảy ra khi đăng xuất');
    }
  }

  async checkAuth(): Promise<CheckAuthResponse> {
    try {
      const response = await fetch(`${this.baseUrl}/check-auth`, {
        method: 'GET',
        headers: {
          'Content-Type': 'application/json',
        },
        credentials: 'include'
      });

      const data = await response.json();
      return data;
    } catch (error) {
      console.error('Check auth error:', error);
      return { success: false, isAuthenticated: false };
    }
  }

  async checkEmailExists(email: string): Promise<{ exists: boolean }> {
    try {
      const response = await fetch(`${this.baseUrl}/check-email`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(email),
        credentials: 'include'
      });

      const data = await response.json();
      return data;
    } catch (error) {
      console.error('Check email error:', error);
      return { exists: false };
    }
  }

  // Helper method to get current user from localStorage or session
  getCurrentUser(): User | null {
    const userStr = localStorage.getItem('currentUser');
    if (userStr) {
      try {
        return JSON.parse(userStr);
      } catch (error) {
        console.error('Error parsing user from localStorage:', error);
        return null;
      }
    }
    return null;
  }

  // Helper method to set current user in localStorage
  setCurrentUser(user: User): void {
    localStorage.setItem('currentUser', JSON.stringify(user));
  }

  // Helper method to clear current user from localStorage
  clearCurrentUser(): void {
    localStorage.removeItem('currentUser');
  }

  // Helper method to check if user is authenticated
  isAuthenticated(): boolean {
    return this.getCurrentUser() !== null;
  }

  // Helper method to check if user has specific role
  hasRole(role: string): boolean {
    const user = this.getCurrentUser();
    return user?.role === role;
  }
}

export const authService = new AuthService();
export default authService; 