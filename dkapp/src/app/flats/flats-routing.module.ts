import { Routes } from '@angular/router';
import { FlatsPageComponent } from './flatsPage/flats-page.component';

export const FlatsRoutes: Routes = [
  { path: ':chartType', component: FlatsPageComponent, data: { roles: []} },
  { path: '', component: FlatsPageComponent, data: { desc: 'abc'} },
];
