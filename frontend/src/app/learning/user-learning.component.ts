import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';

type Category = 'domain' | 'process' | 'technology' | 'poc' | 'assessment';

interface LearningItem {
  id: number;
  title: string;
  category: Category;
  owner: string;
  tags: string[];
  link?: string;
  updatedAt: string; // ISO string
}

@Component({
  selector: 'app-user-learning',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './user-learning.component.html'
})
export class UserLearningComponent implements OnInit {

  // UI theme helpers (kept consistent with the Admin page)
  accent = {
    solid: 'bg-emerald-600 text-white hover:bg-emerald-700',
    soft: 'text-emerald-700 bg-emerald-50 ring-1 ring-emerald-100',
    halo: 'ring-1 ring-gray-100 bg-white/80 backdrop-blur',
  };

  // Category order
  categories: Category[] = ['domain', 'process', 'technology', 'poc', 'assessment'];

  // Category metadata
  categoryMeta: Record<Category, { title: string; subtitle: string; icon: string; tint: string }> = {
    domain:     { title: 'Domain',     subtitle: '',      icon: 'domain',     tint: 'from-emerald-50 to-emerald-100' },
    process:    { title: 'Process & Methodology', subtitle: '',     icon: 'process',    tint: 'from-lime-50 to-lime-100' },
    technology: { title: 'Technology',            subtitle: '',      icon: 'tech',       tint: 'from-sky-50 to-sky-100' },
    poc:        { title: 'POC',                   subtitle: '',   icon: 'poc',        tint: 'from-indigo-50 to-indigo-100' },
    assessment: { title: 'Assessment',            subtitle: '',               icon: 'assessment', tint: 'from-amber-50 to-amber-100' },
  };

  getMeta(c: Category) { return this.categoryMeta[c]; }

  // Data (read-only for users)
  items: LearningItem[] = [];

  // Filters (no status on the user page)
  search = '';
  categoryFilter: '' | Category = '';

  // Stats (only total modules)
  get total() { return this.items.length; }

  // LocalStorage key must match Admin component
  private STORAGE_KEY = 'chevron.learning.items';

  ngOnInit(): void {
    this.loadFromStorage();
  }

  private loadFromStorage() {
    try {
      const raw = localStorage.getItem(this.STORAGE_KEY);
      this.items = raw ? JSON.parse(raw) : [];
    } catch {
      this.items = [];
    }
  }

  itemsByCategory(cat: Category) {
    return this.items
      .filter(i => i.category === cat)
      .sort((a, b) => (a.updatedAt < b.updatedAt ? 1 : -1))
      .slice(0, 4);
  }

  get filtered() {
    return this.items.filter(i =>
      (!this.categoryFilter || i.category === this.categoryFilter) &&
      (!this.search ||
        i.title.toLowerCase().includes(this.search.toLowerCase()) ||
        i.tags.some(t => t.toLowerCase().includes(this.search.toLowerCase())))
    );
  }

  filterByCategory(c: Category) {
    this.categoryFilter = c;
    window.scrollTo({ top: document.body.scrollHeight, behavior: 'smooth' });
  }

  // For safety: ensure external links open in a new tab and are optional
  openLink(href?: string) {
    if (!href) { return; }
    // Could validate URL format here if needed
    window.open(href, '_blank', 'noopener,noreferrer');
  }
}