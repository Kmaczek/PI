import { Routes } from '@angular/router';
import { InflationPageComponent } from './inflation-page/inflation-page.component';
import { UserRoles } from '../models/user/user-roles';
import { ProductEditPageComponent } from './product-edit-page/product-edit-page.component';

export const InflationRoutes: Routes = [
  { path: '', component: InflationPageComponent },
  { path: 'products/edit', component: ProductEditPageComponent, data: { roles: [UserRoles.Admin]} },
];
