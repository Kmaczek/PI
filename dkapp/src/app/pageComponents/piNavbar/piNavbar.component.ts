import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { MenuItem } from 'primeng/api';
import { IdentityService } from '../../Services/external/identity.ext.service';
import { List } from 'linqts';

@Component({
  selector: 'pi-navbar',
  templateUrl: './piNavbar.component.html',
  styleUrls: ['./piNavbar.component.css']
})
export class PiNavbarComponent implements OnInit
{
  items: MenuItem[];
  activeItem: MenuItem;

  constructor(
    private router: Router,
    public identityService: IdentityService) 
  {
  }

  ngOnInit()
  {
    this.items = [
      { label: 'Home', icon: 'pi pi-fw pi-desktop', routerLink: '/', routerLinkActiveOptions: {exact: true}},
      { label: 'Flats', icon: 'pi pi-fw pi-home', routerLink: '/flats', routerLinkActiveOptions: {exact: true}},
      { label: 'Inflation', icon: 'pi pi-fw pi-chart-line', routerLink: '/inflation', routerLinkActiveOptions: {exact: true}}
    ];

  }

  clickx(){
    let x = this.activeItem;
    debugger
  }

}
