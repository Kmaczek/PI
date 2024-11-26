import { Component, OnInit } from '@angular/core'
import { Router } from '@angular/router'
import { MenuItem } from 'primeng/api'
import { IdentityService } from '../../services/external/identity.ext-service'

@Component({
  selector: 'pi-navbar',
  templateUrl: './pi-navbar.component.html',
  styleUrls: ['./pi-navbar.component.scss'],
})
export class PiNavbarComponent implements OnInit {
  items: MenuItem[]
  activeItem: MenuItem
  showSignIn = true;

  constructor(private router: Router, public identityService: IdentityService) {}

  ngOnInit() {
    this.showSignIn = this.identityService.isLoggedIn;
    this.identityService.isLoggedIn$.subscribe(isLoggedIn => {
      this.items = [
        {
          label: 'Home',
          icon: 'pi pi-desktop',
          routerLink: ['/'],
          routerLinkActiveOptions: { exact: true },
        },
        {
          label: 'Flats',
          icon: 'pi pi-home',
          routerLink: ['/flats'],
          routerLinkActiveOptions: { exact: true },
        },
        {
          label: 'Inflation',
          icon: 'pi pi-chart-line',
          routerLink: '/inflation',
          routerLinkActiveOptions: { exact: true },
        },
        {
          label: 'Sign In',
          icon: 'pi pi-sign-in',
          visible: !isLoggedIn,
          command: () => {
            this.showSignIn = true;
          }
        },
        {
          label: 'Sign Out',
          icon: 'pi pi-sign-out',
          visible: isLoggedIn,
          command: () => {
            this.identityService.logout();
            this.showSignIn = false;
          }
        },
      ]
    })
  }

  hideSignIn(): void {
    this.showSignIn = false;
  }
}
