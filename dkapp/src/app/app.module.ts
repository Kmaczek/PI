import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './appRouting.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AppComponent } from './app.component';
import { InflationModule } from './InflationModule/inflation.module';
import { PiContentComponent } from './pageComponents/piContent/piContent.component';
import { PiFooterComponent } from './pageComponents/piFooter/piFooter.component';
import { PiNavbarComponent } from './pageComponents/piNavbar/piNavbar.component';
import { PiControlsModule } from './piControls/piControls.module';
import { FlatsModule } from './flatsModule/flats.module';
import { HomeScreenComponent } from './homeScreen/homeScreen.component';

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
    PiControlsModule,
    FlatsModule,
    InflationModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
