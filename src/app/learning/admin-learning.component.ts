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
  updatedAt: string;
}

@Component({
  selector: 'app-admin-learning',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './admin-learning.component.html'
})
export class AdminLearningComponent implements OnInit {
  tabs = [
    { key: 'library', label: 'Chevron Library', route: '/admin' },
    { key: 'learning', label: 'Chevron Learning', route: '/admin/learning' },
    { key: 'cert', label: 'Chevron Certification', route: null },
    { key: 'skills', label: 'Skills', route: null },
    { key: 'ai', label: 'AI', route: null },
    { key: 'artefacts', label: 'Chevron Artefacts', route: '/admin/artefacts' },
  ];
  activeTabKey = 'learning';

  accent = {
    solid: 'bg-emerald-600 text-white hover:bg-emerald-700',
    soft: 'text-emerald-700 bg-emerald-50 ring-1 ring-emerald-100',
    halo: 'ring-1 ring-gray-100 bg-white/80 backdrop-blur',
  };

  categories: Category[] = ['domain', 'process', 'technology', 'poc', 'assessment'];

  items: LearningItem[] = [];

  // 🔐 LocalStorage key
  private STORAGE_KEY = 'chevron.learning.items';

  // Filters
  search = '';
  categoryFilter: '' | Category = '';

  // Stats
  get total() { return this.items.length; }

  // Category meta
  categoryMeta: Record<Category, { title: string; subtitle: string; icon: string; tint: string }> = {
    domain:     { title: 'Domain',     subtitle: '',      icon: 'domain',     tint: 'from-emerald-50 to-emerald-100' },
    process:    { title: 'Process & Methodology', subtitle: '',     icon: 'process',    tint: 'from-lime-50 to-lime-100' },
    technology: { title: 'Technology',            subtitle: '',      icon: 'tech',       tint: 'from-sky-50 to-sky-100' },
    poc:        { title: 'POC',                   subtitle: '',   icon: 'poc',        tint: 'from-indigo-50 to-indigo-100' },
    assessment: { title: 'Assessment',            subtitle: '',               icon: 'assessment', tint: 'from-amber-50 to-amber-100' },
  };

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

  private saveToStorage() {
    localStorage.setItem(this.STORAGE_KEY, JSON.stringify(this.items));
  }

  getMeta(c: Category) { return this.categoryMeta[c]; }

  itemsByCategory(cat: Category) {
    return this.items
      .filter(i => i.category === cat)
      .sort((a, b) => (a.updatedAt < b.updatedAt ? 1 : -1))
      .slice(0, 4);
  }

  // Slide-over sheet controls
  showSheet = false;
  editing: LearningItem | null = null;

  form: Partial<LearningItem> = {
    title: '',
    category: 'domain',
    owner: 'Admin',
    tags: [],
    link: '',
    updatedAt: new Date().toISOString()
  };

  openAdd(cat?: Category) {
    this.editing = null;
    this.form = {
      title: '',
      category: cat ?? 'domain',
      owner: 'Admin',
      tags: [],
      link: '',
      updatedAt: new Date().toISOString()
    };
    this.showSheet = true;
  }

  openEdit(item: LearningItem) {
    this.editing = item;
    this.form = { ...item, tags: [...item.tags] };
    this.showSheet = true;
  }

  save() {
    if (!this.form.title || !this.form.category) return;

    if (this.editing) {
      Object.assign(this.editing, {
        ...this.form,
        updatedAt: new Date().toISOString()
      });
    } else {
      const newItem: LearningItem = {
        id: Date.now(),
        title: this.form.title!,
        category: this.form.category as Category,
        owner: this.form.owner || 'Admin',
        tags: this.form.tags || [],
        link: this.form.link || '',
        updatedAt: new Date().toISOString()
      };
      this.items.unshift(newItem);
    }

    this.showSheet = false;
    this.saveToStorage(); // ✅ persist after save
  }

  remove(item: LearningItem) {
    this.items = this.items.filter(i => i.id !== item.id);
    this.saveToStorage(); // ✅ persist after delete
  }

  // tags <-> text field
  get tagsString() { return (this.form.tags || []).join(', '); }
  set tagsString(v: string) { this.form.tags = v.split(',').map(s => s.trim()).filter(Boolean); }

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
}