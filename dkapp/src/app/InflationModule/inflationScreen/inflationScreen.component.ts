import { Component, OnInit } from '@angular/core';
import { IdentityService } from 'src/app/Services/external/identity.ext.service';
import { PiExtService } from 'src/app/Services/external/pi.ext.service';

@Component({
  selector: 'inflationScreen',
  templateUrl: './inflationScreen.component.html',
  styleUrls: ['./inflationScreen.component.css']
})
export class InflationScreenComponent implements OnInit
{

  constructor(
    private piService: PiExtService,
    private identity: IdentityService)
  { }

  ngOnInit()
  {
    console.log('inflation');
    this.piService.GetWeatherForecast();
    //this.identity.login(); 
  }
}
