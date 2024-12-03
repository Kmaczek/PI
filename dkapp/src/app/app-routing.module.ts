import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule, Routes, provideRouter, withComponentInputBinding, withRouterConfig } from '@angular/router';
import { AdminComponent } from './admin/admin.component';
import { HomePageComponent } from './home-page/home-page.component';
import { RedirectComponent } from './routeComponents/RedirectComponent/redirect.component';

const routes: Routes = [
  { path: '', component: HomePageComponent },
  { path: 'callback', component: RedirectComponent },
  { path: 'admin', component: AdminComponent },
  { path: 'flats', loadChildren: () => import('./flats/flats.module').then(m => m.FlatsModule) },
  { path: 'inflation', loadChildren: () => import('./inflation/inflation.module').then(m => m.InflationModule) },
];

@NgModule({
  imports: [BrowserModule, [RouterModule.forRoot(routes, { enableViewTransitions: true })]],
  exports: [RouterModule],
  providers: [
    provideRouter(routes, withComponentInputBinding(), withRouterConfig({ paramsInheritanceStrategy: 'always' }))
  ]
})
export class AppRoutingModule {}
