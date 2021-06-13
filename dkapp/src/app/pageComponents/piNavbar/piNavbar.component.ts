import { Component, OnInit } from '@angular/core';
import { IdentityService } from '../../services/external/identity.ext.service';

@Component({
  selector: 'pi-navbar',
  templateUrl: './piNavbar.component.html',
  styleUrls: ['./piNavbar.component.css']
})
export class PiNavbarComponent implements OnInit
{

  constructor(
    public identityService: IdentityService
  ) { }

  ngOnInit()
  {

  }



}
