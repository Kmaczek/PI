import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
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
import { ButtonModule } from 'primeng/button';
import { DropdownModule } from 'primeng/dropdown';
import { DividerModule } from 'primeng/divider';
import { ListboxModule } from 'primeng/listbox';
import { TabMenuModule } from 'primeng/tabmenu';
import { InputTextModule } from 'primeng/inputtext';

import { PiLoginComponent } from './pageComponents/piLogin/piLogin.component';
import { AuthInterceptor } from './services/interceptors/authInterceptor';

@NgModule({
  declarations: [
    AppComponent,
    PiNavbarComponent,
    PiContentComponent,
    PiFooterComponent,
    PiLoginComponent,
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
    ButtonModule,
    DropdownModule,
    DividerModule,
    ListboxModule,
    TabMenuModule,
    InputTextModule,

    PiControlsModule,
    FlatsModule,
    InflationModule
  ],
  providers: [{
    provide: HTTP_INTERCEPTORS,
    useClass: AuthInterceptor,
    multi: true
  }],
  bootstrap: [AppComponent]
})
export class AppModule { }
