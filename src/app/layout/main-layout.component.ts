import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet, RouterLink, RouterLinkActive } from '@angular/router';

@Component({
  selector: 'app-main-layout',
  standalone: true,
  imports: [CommonModule, RouterOutlet, RouterLink, RouterLinkActive],
  templateUrl: './main-layout.component.html'
})
export class MainLayoutComponent {

role: 'admin' | 'user' = 'user';

ngOnInit() {
  const stored =
    localStorage.getItem('chevronOnboarding_auth') ||
    sessionStorage.getItem('chevronOnboarding_auth');

  if (stored) {
    const parsed = JSON.parse(stored);
    this.role = parsed.role === 'admin' ? 'admin' : 'user';
  }
}

}