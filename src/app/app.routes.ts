import { Routes } from '@angular/router';
import { AdminLearningComponent } from './learning/admin-learning.component';
import { AdminArtefactsComponent } from './artefacts/admin-artefacts.component';
import { AdminHomeComponent } from './home/admin-home.component';
import { UserArtefactsComponent } from './artefacts/user-artefacts.component';
import { UserHomeComponent } from './home/user-home.component';
import { UserLearningComponent } from './learning/user-learning.component';
import { MainLayoutComponent } from './layout/main-layout.component';

export const routes: Routes = [
  {
    path: '',
    component: MainLayoutComponent,
    children: [
      { path: 'admin', component: AdminHomeComponent },
      { path: 'user', component: UserHomeComponent },
      { path: 'admin/learning', component: AdminLearningComponent },
      { path: 'user/learning', component: UserLearningComponent },
      { path: 'admin/artefacts', component: AdminArtefactsComponent },
      { path: 'user/artefacts', component: UserArtefactsComponent },
    ]
  }
];