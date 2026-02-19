import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { environment } from '../../environments/environment';
 
@Component({
  selector: 'app-admin-home',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './admin-home.component.html'
})
export class AdminHomeComponent {
  // Top chips (admin routes)
  navChips = [
    { label: 'C. Library',    link: '/admin' },                 // keep on home for now
    { label: 'C. Learning',   link: '/admin/learning' },
    { label: 'C. Certifications', link: '/admin' },             // stub routes you can wire later
    { label: 'Skills',        link: '/admin' },
    { label: 'AI',            link: '/admin' },
    { label: 'C. Artefacts',  link: '/admin/artefacts' },
  ];
 
  // Power BI (Admin) – prefer secure embed; for demo we use an environment URL
  private powerBiUrl: string = environment.powerbiAdminDashboardUrl;
  powerBiSafeUrl: SafeResourceUrl;
 
  constructor(private sanitizer: DomSanitizer) {
    // Add minimal filters/params here if needed, e.g., ?ctid=...&filter=...
    this.powerBiSafeUrl = this.sanitizer.bypassSecurityTrustResourceUrl(this.powerBiUrl);
  }
 
  refresh(): void {
    // simplest way to refresh the iframe
    this.powerBiSafeUrl = this.sanitizer.bypassSecurityTrustResourceUrl(
      `${this.powerBiUrl}${this.powerBiUrl.includes('?') ? '&' : '?'}ts=${Date.now()}`
    );
  }
}