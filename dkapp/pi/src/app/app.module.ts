import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatGridListModule, 
  MatToolbarModule, 
  MatSidenavModule, 
  MatFormFieldModule, 
  MatButtonModule, 
  MatInputModule, 
  MatIconModule,
  MatSelectModule } 
  from '@angular/material';
import { PiFooterComponent } from './pi-footer/pi-footer.component';
import { PiNavbarComponent } from './pi-navbar/pi-navbar.component';
import { PiContentComponent } from './pi-content/pi-content.component';
import { PiBtnComponent } from './controls/pi-btn/pi-btn.component';


@NgModule({
  declarations: [
    AppComponent,
    PiNavbarComponent,
    PiContentComponent,
    PiFooterComponent,
    PiBtnComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    BrowserAnimationsModule,
    MatGridListModule,
    MatToolbarModule,
    MatSidenavModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatSelectModule,
    MatIconModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
