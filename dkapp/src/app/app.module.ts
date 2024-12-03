import {
  HTTP_INTERCEPTORS,
  provideHttpClient,
  withInterceptorsFromDi,
} from '@angular/common/http';
import { APP_INITIALIZER, NgModule, isDevMode } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';

import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HomePageComponent } from './home-page/home-page.component';
import { PiContentComponent } from './layout/pi-content/pi-content.component';
import { PiFooterComponent } from './layout/pi-footer/pi-footer.component';
import { PiNavbarComponent } from './layout/pi-navbar/pi-navbar.component';

import { ButtonModule } from 'primeng/button';
import { DividerModule } from 'primeng/divider';
import { DropdownModule } from 'primeng/dropdown';
import { InputTextModule } from 'primeng/inputtext';
import { ListboxModule } from 'primeng/listbox';
import { MenubarModule } from 'primeng/menubar';
import { TableModule } from 'primeng/table';
import { TabMenuModule } from 'primeng/tabmenu';

import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { StoreDevtoolsModule } from '@ngrx/store-devtools';
import { firstValueFrom } from 'rxjs';
import { FlatsModule } from './flats/flats.module';
import { InflationModule } from './inflation/inflation.module';
import { PiLoginComponent } from './layout/pi-login/pi-login.component';
import { InitializeService } from './services/initialize.service';
import { AuthInterceptor } from './services/interceptors/auth-interceptor';
import { DateInterceptor } from './services/interceptors/date-interceptor';
import { UserEffects } from './state/user-effects';
import { userReducer } from './state/user-reducer';

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
    StoreModule.forRoot({}),
    EffectsModule.forRoot([]),

    StoreModule.forFeature('user', userReducer),
    EffectsModule.forFeature(UserEffects),
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
    StoreDevtoolsModule.instrument({ maxAge: 40, logOnly: !isDevMode(), trace: true, traceLimit: 75 }),
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
    {
      provide: APP_INITIALIZER,
      useFactory: (initializeService: InitializeService) => () => {
        return firstValueFrom(initializeService.initialize());
      },
      deps: [InitializeService],
      multi: true
    }
  ],
})
export class AppModule {}
