import { Component, OnInit } from '@angular/core';
import { IdentityService } from 'src/app/services/external/identity.ext.service';
import { PiExtService } from 'src/app/services/external/pi.ext.service';

@Component({
  selector: 'homeScreen',
  templateUrl: './homeScreen.component.html',
  styleUrls: ['./homeScreen.component.css']
})
export class HomeScreenComponent implements OnInit
{

  constructor(
    private piService: PiExtService,
    private identity: IdentityService)
  { }

  ngOnInit()
  {
    console.log('home');
    //this.piService.GetWeatherForecast();
    //this.identity.login(); 
  }
}
