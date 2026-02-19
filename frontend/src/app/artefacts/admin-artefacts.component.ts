// src/app/artefacts/admin-artefacts.component.ts
import { Component, computed, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';

type Artefact = {
  id: string;
  title: string;
  summary: string;
  category: 'Value Adds' | 'POV' | 'Case Studies' | 'Reusable Component';
  tags: string[];
  updatedAt: string; // ISO or display string
  owner: string;
  url?: string; // optional link if available
};

@Component({
  selector: 'app-admin-artefacts',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './admin-artefacts.component.html',
})
export class AdminArtefactsComponent {
  // Admin view permissions
  readonly isAdmin = true;

  // Top module nav (as in wireframe)
  readonly routePrefix = 'admin';

  // Search & category state
  query = signal<string>('');
  activeCategory = signal<'All' | Artefact['category']>('All');

  // Delete-confirm state
  confirmDeleteId = signal<string | null>(null);

  // Seed data (sample items)
  private readonly artefactsSeed: Artefact[] = [
    {
      id: 'a-101',
      title: 'Customer Value Canvas',
      summary: 'Template + guide to capture value adds for Chevron onboarding programs.',
      category: 'Value Adds',
      tags: ['doc', 'template'],
      updatedAt: '2026-01-21',
      owner: 'Onboarding Admin',
      url: '#'
    },
    {
      id: 'a-102',
      title: 'POV: Cloud Governance for New Teams',
      summary: 'Opinionated practices for early-stage cloud guardrails and access.',
      category: 'POV',
      tags: ['pdf', 'governance'],
      updatedAt: '2026-01-10',
      owner: 'Platform Team',
      url: '#'
    },
    {
      id: 'a-103',
      title: 'Case Study: Accelerated L1 Support',
      summary: 'How reusable runbooks reduced ticket TTR by 28% in quarter 3.',
      category: 'Case Studies',
      tags: ['runbook', 'ops'],
      updatedAt: '2025-12-02',
      owner: 'Operations',
      url: '#'
    },
    {
      id: 'a-104',
      title: 'Reusable Component: Onboarding Checklist Widget',
      summary: 'Drop‑in Angular component for day‑1/day‑7 checklist with progress state.',
      category: 'Reusable Component',
      tags: ['angular', 'ui', 'component'],
      updatedAt: '2026-02-01',
      owner: 'UX Guild',
      url: '#'
    },
    {
      id: 'a-105',
      title: 'POV: Security in Developer Workstations',
      summary: 'Recommended baseline hardening and safe defaults for dev laptops.',
      category: 'POV',
      tags: ['security'],
      updatedAt: '2025-11-14',
      owner: 'Security Office',
    },
    {
      id: 'a-106',
      title: 'Case Study: API-first Knowledge Library',
      summary: 'Cataloging artefacts via metadata-first approach for better discovery.',
      category: 'Case Studies',
      tags: ['api', 'metadata'],
      updatedAt: '2025-10-08',
      owner: 'Knowledge Mgmt',
    },
  ];

  artefacts = signal<Artefact[]>(this.artefactsSeed);

  readonly categories: Array<'All' | Artefact['category']> = [
    'All',
    'Value Adds',
    'POV',
    'Case Studies',
    'Reusable Component',
  ];

  filtered = computed(() => {
    const q = this.query().trim().toLowerCase();
    const cat = this.activeCategory();

    return this.artefacts().filter((a) => {
      const matchesText =
        !q ||
        a.title.toLowerCase().includes(q) ||
        a.summary.toLowerCase().includes(q) ||
        a.tags.some(t => t.toLowerCase().includes(q));

      const matchesCat = cat === 'All' ? true : a.category === cat;
      return matchesText && matchesCat;
    });
  });

  onSelectCategory(cat: 'All' | Artefact['category']) {
    this.activeCategory.set(cat);
  }

  onView(item: Artefact) {
    if (item.url && item.url !== '#') {
      window.open(item.url, '_blank');
    } else {
      console.log('View artefact:', item);
    }
  }

  onEdit(item: Artefact) {
    console.log('Edit artefact:', item);
    // Later: open edit drawer/modal
  }

  askDelete(item: Artefact) {
    this.confirmDeleteId.set(item.id);
  }

  cancelDelete() {
    this.confirmDeleteId.set(null);
  }

  confirmDelete(item: Artefact) {
    this.artefacts.update(list => list.filter(a => a.id !== item.id));
    this.confirmDeleteId.set(null);
  }

  tagColor(tag: string) {
    // Simple color assignment by tag
    const map: Record<string, string> = {
      doc: 'bg-emerald-50 text-emerald-700 ring-emerald-200',
      template: 'bg-teal-50 text-teal-700 ring-teal-200',
      pdf: 'bg-sky-50 text-sky-700 ring-sky-200',
      governance: 'bg-amber-50 text-amber-700 ring-amber-200',
      runbook: 'bg-purple-50 text-purple-700 ring-purple-200',
      ops: 'bg-fuchsia-50 text-fuchsia-700 ring-fuchsia-200',
      angular: 'bg-rose-50 text-rose-700 ring-rose-200',
      ui: 'bg-cyan-50 text-cyan-700 ring-cyan-200',
      component: 'bg-lime-50 text-lime-700 ring-lime-200',
      security: 'bg-red-50 text-red-700 ring-red-200',
      api: 'bg-indigo-50 text-indigo-700 ring-indigo-200',
      metadata: 'bg-slate-50 text-slate-700 ring-slate-200',
    };
    return map[tag] || 'bg-slate-50 text-slate-700 ring-slate-200';
  }
}