import { ComponentFixture, TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { PiNavbarComponent } from './piNavbar.component';
import { IdentityService } from '../../Services/external/identity.ext.service';
import { By } from '@angular/platform-browser';
import { Router } from '@angular/router';
import { Moment } from 'moment';
import { TabMenuModule } from 'primeng/tabmenu';

describe('PiNavbarComponent', () => {
  let component: PiNavbarComponent;
  let fixture: ComponentFixture<PiNavbarComponent>;
  let router: Router;

  beforeEach(() => {
    const IdentityServiceMock = {
        login: () => {
            return null;
        },
        logout: () => {
            return null;
        },

        setSession: function (authResult: any): void {
        },
        isLoggedIn: function (): boolean {
            return null;
        },
        isLoggedOut: function (): boolean {
            return null;
        },
        getExpiration: function (): Moment {
            return null;
        }
    };
    TestBed.configureTestingModule({
      declarations: [ PiNavbarComponent ],
      imports: [ RouterTestingModule, TabMenuModule ],
      providers: [
        { provide: IdentityService, useValue: IdentityServiceMock }
    ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PiNavbarComponent);
    router = TestBed.inject(Router);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  fit('should navigate to home on click', () => {
    spyOn(router, 'navigateByUrl');
    const homeLink = fixture.debugElement.query(By.css('a[href="/"]'));
    homeLink.nativeElement.click();
    fixture.detectChanges();
    expect(router.navigateByUrl).toHaveBeenCalledWith('/');
  });

  it('should navigate to flats on click', () => {
    spyOn(router, 'navigateByUrl');
    const homeButton = fixture.debugElement.query(By.css('a[href="/flats"]'));
    homeButton.triggerEventHandler('click', null);
    fixture.detectChanges();
    expect(router.navigateByUrl).toHaveBeenCalledWith('/flats');
  });

  it('should navigate to inflation on click', () => {
    spyOn(router, 'navigateByUrl');
    const homeButton = fixture.debugElement.query(By.css('a[href="/inflation"]'));
    homeButton.triggerEventHandler('click', null);
    fixture.detectChanges();
    expect(router.navigateByUrl).toHaveBeenCalledWith('/inflation');
  });
});


