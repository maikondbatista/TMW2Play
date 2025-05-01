import { Routes } from '@angular/router';

export const routes: Routes = [
    {
        path: 'Home',
        loadComponent: () => import('./home/home.component').then(m => m.HomeComponent)
    },
    {
        path: 'Profile/:steamUser',
        loadComponent: () => import('./profile/profile.component').then(m => m.ProfileComponent)
    },
    {
        path: '**',
        redirectTo: 'Home'
    }
];
