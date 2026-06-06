import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { environment } from '../../../environments/environment';
import { AuthService } from '../../core/services/auth';
import { TokenService } from '../../core/services/token';
import { ProfileService } from '../../core/services/profile';

@Component({
  selector: 'app-main-layout',
  standalone: true,
  imports: [CommonModule, RouterOutlet, RouterLink, RouterLinkActive],
  templateUrl: './main-layout.html',
  styleUrl: './main-layout.css'
})
export class MainLayoutComponent implements OnInit {
  private authService = inject(AuthService);
  private tokenService = inject(TokenService);
  private profileService = inject(ProfileService);
  private router = inject(Router);

  currentUser = this.tokenService.getUser();
  avatarUrl: string | null = null;

 ngOnInit(): void {
  this.profileService.getProfile().subscribe({
    next: (profile) => {
      this.avatarUrl = profile.avatarUrl 
        ? `${environment.apiUrl.replace('/api', '')}/${profile.avatarUrl}`
        : null;
    }
  });
}

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/auth/login']);
  }
}