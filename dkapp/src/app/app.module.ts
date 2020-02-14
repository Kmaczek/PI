import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { PiFooterComponent } from './pi-footer/pi-footer.component';
import { PiNavbarComponent } from './pi-navbar/pi-navbar.component';
import { PiContentComponent } from './pi-content/pi-content.component';
import { PiBtnComponent } from './controls/pi-btn/pi-btn.component';
import { PiLineChartComponent } from './controls/pi-line-chart/pi-line-chart.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AppComponent } from './app.component';


@NgModule({
  declarations: [
    AppComponent,
    PiNavbarComponent,
    PiContentComponent,
    PiFooterComponent,
    PiBtnComponent,
    PiLineChartComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    AppRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    BrowserAnimationsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
