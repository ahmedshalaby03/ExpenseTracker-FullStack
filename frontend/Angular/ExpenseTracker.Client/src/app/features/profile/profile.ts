import { Component, OnInit, inject, ChangeDetectorRef } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { catchError, of } from 'rxjs';
import { environment } from '../../../environments/environment';
import { ProfileService } from '../../core/services/profile';
import { Profile } from '../../core/models/profile.models';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, DatePipe],
  templateUrl: './profile.html',
  styleUrl: './profile.css'
})
export class ProfileComponent implements OnInit {
  private fb = inject(FormBuilder);
  private profileService = inject(ProfileService);
  private cdr = inject(ChangeDetectorRef);

  profile: Profile | null = null;
  apiBaseUrl = environment.apiUrl.replace('/api', '');  isLoading = false;
  isSavingProfile = false;
  isSavingPreferences = false;
  isChangingPassword = false;

  errorMessage = '';
  successMessage = '';

  profileForm = this.fb.group({
    fullName: ['', [Validators.required, Validators.minLength(3)]],
    preferredCurrency: ['EGP']
  });

  preferencesForm = this.fb.group({
    defaultPaymentMethod: ['Credit Card'],
    monthlyBudgetLimit: [0],
    emailNotifications: [true],
    pushNotifications: [true],
    smsAlerts: [false]
  });

  passwordForm = this.fb.group({
    currentPassword: ['', [Validators.required]],
    newPassword: ['', [Validators.required]],
    confirmNewPassword: ['', [Validators.required]]
  });

  ngOnInit(): void {
    this.loadProfile();
  }

  loadProfile(): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.profileService.getProfile()
      .pipe(
        catchError((err) => {
          console.log(err);
          this.errorMessage = 'Failed to load profile';
          this.isLoading = false;
          this.cdr.detectChanges();

          return of(null);
        })
      )
      .subscribe((data) => {
        if (!data) {
          this.cdr.detectChanges();
          return;
        }

        this.profile = data;
        this.patchForms(data);
        this.isLoading = false;
        this.cdr.detectChanges();
      });
  }

  patchForms(profile: Profile): void {
    this.profileForm.patchValue({
      fullName: profile.fullName,
      preferredCurrency: profile.preferredCurrency || 'EGP'
    });

    this.preferencesForm.patchValue({
      defaultPaymentMethod: profile.defaultPaymentMethod || 'Credit Card',
      monthlyBudgetLimit: profile.monthlyBudgetLimit || 0,
      emailNotifications: profile.emailNotifications,
      pushNotifications: profile.pushNotifications,
      smsAlerts: profile.smsAlerts
    });

    this.cdr.detectChanges();
  }

  onAvatarSelected(event: Event): void {
  const input = event.target as HTMLInputElement;

  if (!input.files || input.files.length === 0) {
    return;
  }

  const file = input.files[0];

  this.errorMessage = '';
  this.successMessage = '';

  this.profileService.uploadAvatar(file).subscribe({
    next: (response) => {
      this.successMessage = 'Profile image uploaded successfully';

      if (this.profile) {
        this.profile.avatarUrl = response.avatarUrl;
      }
    },
    error: (error) => {
      if (Array.isArray(error.error)) {
        this.errorMessage = error.error.join(', ');
      } else if (typeof error.error === 'string') {
        this.errorMessage = error.error;
      } else {
        this.errorMessage = 'Failed to upload profile image';
      }
    }
  });
}


getAvatarFullUrl(): string | null {
  if (!this.profile?.avatarUrl) {
    return null;
  }

  if (this.profile.avatarUrl.startsWith('http')) {
    return this.profile.avatarUrl;
  }

  return `${this.apiBaseUrl}${this.profile.avatarUrl}`;
}



  saveProfile(): void {
    this.errorMessage = '';
    this.successMessage = '';

    if (this.profileForm.invalid) {
      this.profileForm.markAllAsTouched();
      this.cdr.detectChanges();
      return;
    }

    this.isSavingProfile = true;
    this.cdr.detectChanges();

    const request = {
      fullName: this.profileForm.value.fullName!,
      preferredCurrency: this.profileForm.value.preferredCurrency || 'EGP'
    };

    this.profileService.updateProfile(request).subscribe({
      next: (data) => {
        this.profile = data;
        this.successMessage = 'Profile updated successfully';
        this.isSavingProfile = false;
        this.cdr.detectChanges();
      },
      error: (error) => this.handleError(error)
    });
  }

  savePreferences(): void {
    this.errorMessage = '';
    this.successMessage = '';
    this.isSavingPreferences = true;
    this.cdr.detectChanges();

    const request = {
      defaultPaymentMethod:
        this.preferencesForm.value.defaultPaymentMethod || null,

      monthlyBudgetLimit:
        Number(this.preferencesForm.value.monthlyBudgetLimit) || null,

      emailNotifications:
        !!this.preferencesForm.value.emailNotifications,

      pushNotifications:
        !!this.preferencesForm.value.pushNotifications,

      smsAlerts:
        !!this.preferencesForm.value.smsAlerts
    };

    this.profileService.updatePreferences(request).subscribe({
      next: (data) => {
        this.profile = data;
        this.successMessage = 'Preferences updated successfully';
        this.isSavingPreferences = false;
        this.cdr.detectChanges();
      },
      error: (error) => this.handleError(error)
    });
  }

  changePassword(): void {
    this.errorMessage = '';
    this.successMessage = '';

    if (this.passwordForm.invalid) {
      this.passwordForm.markAllAsTouched();
      this.cdr.detectChanges();
      return;
    }

    const newPassword = this.passwordForm.value.newPassword;
    const confirmNewPassword = this.passwordForm.value.confirmNewPassword;

    if (newPassword !== confirmNewPassword) {
      this.errorMessage = 'New password and confirm password do not match';
      this.cdr.detectChanges();
      return;
    }

    this.isChangingPassword = true;
    this.cdr.detectChanges();

    const request = {
      currentPassword: this.passwordForm.value.currentPassword!,
      newPassword: this.passwordForm.value.newPassword!,
      confirmNewPassword: this.passwordForm.value.confirmNewPassword!
    };

    this.profileService.changePassword(request).subscribe({
      next: () => {
        this.successMessage = 'Password changed successfully';
        this.passwordForm.reset();
        this.isChangingPassword = false;
        this.cdr.detectChanges();
      },
      error: (error) => this.handleError(error)
    });
  }

  getInitials(): string {
    if (!this.profile?.fullName) {
      return 'U';
    }

    return this.profile.fullName
      .split(' ')
      .map((part) => part.charAt(0))
      .join('')
      .substring(0, 2)
      .toUpperCase();
  }

  getMemberSince(): string {
    return this.profile?.createdAt || new Date().toISOString();
  }

  private handleError(error: any): void {
    console.log(error);

    this.isSavingProfile = false;
    this.isSavingPreferences = false;
    this.isChangingPassword = false;

    if (Array.isArray(error.error)) {
      this.errorMessage = error.error.join(', ');
    } else if (typeof error.error === 'string') {
      this.errorMessage = error.error;
    } else {
      this.errorMessage = 'Something went wrong';
    }

    this.cdr.detectChanges();
  }
}