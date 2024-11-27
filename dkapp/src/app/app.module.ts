import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HTTP_INTERCEPTORS, provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AppComponent } from './app.component';
import { InflationModule } from './inflationModule/inflation.module';
import { PiContentComponent } from './pageComponents/pi-content/pi-content.component';
import { PiFooterComponent } from './pageComponents/pi-footer/pi-footer.component';
import { PiNavbarComponent } from './pageComponents/pi-navbar/pi-navbar.component';
import { FlatsModule } from './flatsModule/flats.module';
import { HomePageComponent } from './home-page/home-page.component';

import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { DropdownModule } from 'primeng/dropdown';
import { DividerModule } from 'primeng/divider';
import { ListboxModule } from 'primeng/listbox';
import { TabMenuModule } from 'primeng/tabmenu';
import { InputTextModule } from 'primeng/inputtext';
import { MenubarModule } from 'primeng/menubar';

import { PiLoginComponent } from './pageComponents/pi-login/pi-login.component';
import { AuthInterceptor } from './services/interceptors/auth-interceptor';
import { DateInterceptor } from './services/interceptors/date-interceptor';

@NgModule({
  declarations: [
    AppComponent,
    PiNavbarComponent,
    PiContentComponent,
    PiFooterComponent,
    PiLoginComponent,
    HomePageComponent,
  ],
  bootstrap: [AppComponent],
  imports: [
    BrowserModule,
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
    MenubarModule,
    FlatsModule,
    InflationModule,
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true,
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: DateInterceptor,
      multi: true,
    },
    provideHttpClient(withInterceptorsFromDi()),
  ],
})
export class AppModule {}
