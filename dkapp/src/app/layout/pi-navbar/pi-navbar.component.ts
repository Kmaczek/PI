import { Component, computed } from '@angular/core';
import { MenuItem } from 'primeng/api';
import { IdentityService } from '../../services/identity.service';
import { ThemeService } from '../../services/theme.service';


@Component({
  selector: 'pi-navbar',
  templateUrl: './pi-navbar.component.html',
  styleUrls: ['./pi-navbar.component.scss'],
})
export class PiNavbarComponent {
  activeItem!: MenuItem;
  showSignIn = false;
  items = computed(() => {
    return [
      {
        label: 'Home',
        icon: 'pi pi-desktop',
        routerLink: ['/'],
        routerLinkActiveOptions: { exact: false },
      },
      {
        label: 'Flats',
        icon: 'pi pi-home',
        routerLink: ['/flats'],
        routerLinkActiveOptions: { exact: false },
      },
      {
        label: 'Inflation',
        icon: 'pi pi-chart-line',
        items: [
          {
            label: 'Check inflation',
            icon: 'pi pi-bolt',
            routerLink: '/inflation',
            routerLinkActiveOptions: { exact: false },
          },
          {
            label: 'Manage Products',
            icon: 'pi pi-server',
            routerLink: '/inflation/products/edit',
            routerLinkActiveOptions: { exact: false },
          },
        ],
      },
      {
        label: 'Mode',
        icon: this.themeService.isDark() ? 'pi pi-moon' : 'pi pi-sun',
        command: () => {
          this.themeService.toggleDarkMode();
        },
      },
      {
        label: 'Sign In',
        icon: 'pi pi-sign-in',
        visible: !this.identityService.isLoggedIn(),
        command: () => {
          this.showSignIn = true;
        },
      },
      {
        label: 'Sign Out',
        icon: 'pi pi-sign-out',
        visible: this.identityService.isLoggedIn(),
        command: () => {
          this.identityService.logout();
          this.showSignIn = false;
        },
      },
    ] as MenuItem[];
  });

  constructor(public identityService: IdentityService, private themeService: ThemeService) {}

  hideSignIn(): void {
    console.log('hsi');
    this.showSignIn = false;
  }
}
