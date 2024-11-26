import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { RedirectComponent } from './routeComponents/RedirectComponent/redirect.component';
import { InflationPageComponent } from './inflationModule/inflation-page/inflation-page.component';
import { FlatsPageComponent } from './flatsModule/flatsScreen/flats-page.component';
import { HomePageComponent } from './home-page/home-page.component';
import { AdminComponent } from './admin/admin.component';

const routes: Routes = [
  { path: '', component: HomePageComponent },
  { path: 'callback', component: RedirectComponent },
  { path: 'admin', component: AdminComponent },
  { path: 'flats', component: FlatsPageComponent },
  { path: 'inflation', component: InflationPageComponent },
];

@NgModule({
  imports: [BrowserModule, [RouterModule.forRoot(routes)]],
  exports: [RouterModule],
})
export class AppRoutingModule {}
