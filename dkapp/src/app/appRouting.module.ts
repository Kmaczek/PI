import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { RedirectComponent } from './routeComponents/RedirectComponent/redirect.component';
import { InflationScreenComponent } from './inflationModule/inflationScreen/inflationScreen.component';
import { FlatsScreenComponent } from './flatsModule/flatsScreen/flatsScreen.component';
import { HomeScreenComponent } from './homeScreen/homeScreen.component';

const routes: Routes = [
  { path: 'callback', component: RedirectComponent},
  { path: '',
    redirectTo: '/',
    pathMatch: 'full'
  },
  { path: 'flats', component: FlatsScreenComponent },
  { path: 'inflation', component: InflationScreenComponent },
  { path: '**', component: HomeScreenComponent }
];

@NgModule({
  imports: [
    BrowserModule,
    [RouterModule.forRoot(routes)]
  ],
  exports: [RouterModule]
})
export class AppRoutingModule { }
