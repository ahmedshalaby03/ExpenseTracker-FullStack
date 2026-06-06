import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterLink } from '@angular/router';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';

import { AuthService } from '../../../core/services/auth';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './register.html',
  styleUrl: './register.css'
})
export class RegisterComponent {
  private fb = inject(FormBuilder);
  private authService = inject(AuthService);
  private router = inject(Router);

  isLoading = false;
  errorMessages: string[] = [];

  registerForm = this.fb.group({
    fullName: ['', [Validators.required, Validators.minLength(3)]],
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required]],
    confirmPassword: ['', [Validators.required]]
  });

  submit(): void {
    this.errorMessages = [];

    if (this.registerForm.invalid) {
      this.registerForm.markAllAsTouched();
      return;
    }

    const password = this.registerForm.value.password;
    const confirmPassword = this.registerForm.value.confirmPassword;

    if (password !== confirmPassword) {
      this.errorMessages = ['Password and confirm password do not match'];
      return;
    }

    this.isLoading = true;

    const request = {
      fullName: this.registerForm.value.fullName!,
      email: this.registerForm.value.email!,
      password: this.registerForm.value.password!,
      confirmPassword: this.registerForm.value.confirmPassword!
    };

    this.authService.register(request).subscribe({
      next: () => {
        this.isLoading = false;
        this.router.navigate(['/dashboard']);
      },
      error: (error) => {
        this.isLoading = false;

        if (Array.isArray(error.error)) {
          this.errorMessages = error.error;
        } else if (typeof error.error === 'string') {
          this.errorMessages = [error.error];
        } else {
          this.errorMessages = ['Registration failed'];
        }
      }
    });
  }
}