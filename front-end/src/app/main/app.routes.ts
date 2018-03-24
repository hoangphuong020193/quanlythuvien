import { Routes, CanActivate } from '@angular/router';

// GUARD
import { HomeGuard } from '../guards/home.guard';

// COMPONENT
import { HomeComponent } from '../components/home';

export const ROUTES: Routes = [
  { path: 'home', redirectTo: '', pathMatch: 'full' },
  {
    path: '',
    component: HomeComponent,
    canActivate: [HomeGuard],
    children: [
      {
        path: '',
        redirectTo: 'index',
        pathMatch: 'full'
      }
    ]
  },
  { path: '**', redirectTo: '', pathMatch: 'full' }
];
