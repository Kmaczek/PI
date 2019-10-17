import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { PiFooterComponent } from './pi-footer/pi-footer.component';
import { PiNavbarComponent } from './pi-navbar/pi-navbar.component';
import { PiContentComponent } from './pi-content/pi-content.component';
import { PiBtnComponent } from './controls/pi-btn/pi-btn.component';
import { PiLineChartComponent } from './controls/pi-line-chart/pi-line-chart.component';


@NgModule({
  declarations: [
    AppComponent,
    PiNavbarComponent,
    PiContentComponent,
    PiFooterComponent,
    PiBtnComponent,
    PiLineChartComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    BrowserAnimationsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
