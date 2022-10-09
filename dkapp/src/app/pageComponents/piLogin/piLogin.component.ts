import { Component, OnInit } from '@angular/core';
import { IdentityService } from 'src/app/Services/external/identity.ext.service';

@Component({
  selector: 'pi-login',
  templateUrl: './piLogin.component.html',
  styleUrls: ['./piLogin.component.css']
})
export class PiLoginComponent implements OnInit
{
  username: string;
  password: string;

  constructor(private identityService: IdentityService) { }

  ngOnInit()
  {

  }

  login()
  {
    this.identityService.login('dkmak', 'Password2014').subscribe(x => {
      console.log(x);
    });
  }

}
