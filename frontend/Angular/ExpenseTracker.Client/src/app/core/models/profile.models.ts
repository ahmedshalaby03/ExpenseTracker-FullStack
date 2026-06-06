export interface Profile {
  userId: string;
  fullName: string;
  email: string;
  preferredCurrency: string | null;
  defaultPaymentMethod: string | null;
  monthlyBudgetLimit: number | null;
  emailNotifications: boolean;
  pushNotifications: boolean;
  smsAlerts: boolean;
  avatarUrl: string | null;
  createdAt: string;
  securityScore: number;
}

export interface UpdateProfileRequest {
  fullName: string;
  preferredCurrency: string | null;
}

export interface UpdatePreferencesRequest {
  defaultPaymentMethod: string | null;
  monthlyBudgetLimit: number | null;
  emailNotifications: boolean;
  pushNotifications: boolean;
  smsAlerts: boolean;
}

export interface ChangePasswordRequest {
  currentPassword: string;
  newPassword: string;
  confirmNewPassword: string;
}