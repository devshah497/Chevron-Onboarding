// src/app/auth/login.component.ts
import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './login.component.html',
})
export class LoginComponent {
  // Demo credentials
  private readonly ADMIN_EMAIL = 'admin@example.com';
  private readonly ADMIN_PASS = 'admin';
  private readonly EMP_EMAIL = 'employee@example.com';
  private readonly EMP_PASS = 'employee';

  showPassword = false;
  submitting = false;
  errorMsg: string | null = null;

  // Declare, but do not initialize here
  form!: FormGroup;

  constructor(private fb: FormBuilder, private router: Router) {
    // ✅ Initialize here so `this.fb` is definitely available
    this.form = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required]],
      remember: [false],
    });
  }

  togglePassword() {
    this.showPassword = !this.showPassword;
  }

  async onSubmit() {
    this.errorMsg = null;

    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.submitting = true;

    const email = (this.form.value.email || '').trim().toLowerCase();
    const password = (this.form.value.password || '').trim();

    await new Promise((r) => setTimeout(r, 300));

    if (email === this.ADMIN_EMAIL && password === this.ADMIN_PASS) {
      this.persist('admin', !!this.form.value.remember);
      this.router.navigate(['/admin']);
    } else if (email === this.EMP_EMAIL && password === this.EMP_PASS) {
      this.persist('employee', !!this.form.value.remember);
      this.router.navigate(['/user']);
    } else {
      this.errorMsg = 'Invalid email or password.';
    }

    this.submitting = false;
  }

  private persist(role: 'admin' | 'employee', remember: boolean) {
    const payload = JSON.stringify({ role, ts: Date.now() });
    if (remember) {
      localStorage.setItem('chevronOnboarding_auth', payload);
    } else {
      sessionStorage.setItem('chevronOnboarding_auth', payload);
    }
  }

  get emailCtrl() { return this.form.get('email'); }
  get passwordCtrl() { return this.form.get('password'); }
}