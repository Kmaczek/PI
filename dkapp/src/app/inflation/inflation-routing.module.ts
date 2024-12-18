import { Routes } from '@angular/router';
import { InflationPageComponent } from './inflation-page/inflation-page.component';
import { UserRoles } from '../models/user/user-roles';
import { ProductEditPageComponent } from './product-edit-page/product-edit-page.component';
import { ProductsResolver } from './resolvers/products.resolver';
import { roleGuard } from '../services/guards/role.guard';

export const InflationRoutes: Routes = [
  {
    path: '',
    component: InflationPageComponent,
    resolve: {
      products: ProductsResolver,
    },
  },
  {
    path: 'products/edit',
    component: ProductEditPageComponent,
    canActivate: [roleGuard],
    data: { roles: [UserRoles.Admin] },
    resolve: {
      products: ProductsResolver,
    },
  },
];
