import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './appRouting.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AppComponent } from './app.component';
import { InflationModule } from './inflationModule/inflation.module';
import { PiContentComponent } from './pageComponents/piContent/piContent.component';
import { PiFooterComponent } from './pageComponents/piFooter/piFooter.component';
import { PiNavbarComponent } from './pageComponents/piNavbar/piNavbar.component';
import { PiControlsModule } from './piControls/piControls.module';
import { FlatsModule } from './flatsModule/flats.module';
import { HomeScreenComponent } from './homeScreen/homeScreen.component';
import { TableModule } from 'primeng/table';

@NgModule({
  declarations: [
    AppComponent,
    PiNavbarComponent,
    PiContentComponent,
    PiFooterComponent,
    HomeScreenComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    AppRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    BrowserAnimationsModule,

    TableModule,

    PiControlsModule,
    FlatsModule,
    InflationModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
