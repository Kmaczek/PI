import { Component, OnInit } from '@angular/core';
import { IdentityService } from 'src/app/Services/external/identity.ext.service';
import { PiExtService } from 'src/app/Services/external/pi.ext.service';

@Component({
  selector: 'flatsScreen',
  templateUrl: './flatsScreen.component.html',
  styleUrls: ['./flatsScreen.component.css']
})
export class FlatsScreenComponent implements OnInit
{

  constructor(
    private piService: PiExtService,
    private identity: IdentityService)
  { }

  ngOnInit()
  {
    console.log('flats');
    //this.piService.GetWeatherForecast();
    //this.identity.login(); 
  }
}
