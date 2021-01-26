import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { RedirectComponent } from './RouteComponents/RedirectComponent/redirect.component';
import { AppComponent } from './app.component';


const routes: Routes = [
  { path: 'callback', component: RedirectComponent},
  { path: '',
    redirectTo: '/',
    pathMatch: 'full'
  },
  { path: 'flats', component: AppComponent },
  { path: '**', component: AppComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
