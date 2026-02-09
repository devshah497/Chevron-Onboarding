import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { environment } from '../../environments/environment';
 
@Component({
  selector: 'app-user-home',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './user-home.component.html'
})
export class UserHomeComponent {
  // Top chips (user routes)
  navChips = [
    { label: 'C. Library',    link: '/user' },
    { label: 'C. Learning',   link: '/user/learning' },
    { label: 'C. Certifications', link: '/user' },
    { label: 'Skills',        link: '/user' },
    { label: 'AI',            link: '/user' },
    { label: 'C. Artefacts',  link: '/user/artefacts' },
  ];
 
  private powerBiUrl: string = environment.powerbiUserDashboardUrl;
  powerBiSafeUrl: SafeResourceUrl;
 
  constructor(private sanitizer: DomSanitizer) {
    this.powerBiSafeUrl = this.sanitizer.bypassSecurityTrustResourceUrl(this.powerBiUrl);
  }
 
  refresh(): void {
    this.powerBiSafeUrl = this.sanitizer.bypassSecurityTrustResourceUrl(
      `${this.powerBiUrl}${this.powerBiUrl.includes('?') ? '&' : '?'}ts=${Date.now()}`
    );
  }
}