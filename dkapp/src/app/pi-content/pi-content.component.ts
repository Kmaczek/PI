import { Component, OnInit } from '@angular/core';
import { PiExtService } from '../Services/External/pi.ext.service';
import { IdentityService } from '../Services/External/identity.ext.service';

@Component({
  selector: 'pi-content',
  templateUrl: './pi-content.component.html',
  styleUrls: ['./pi-content.component.css']
})
export class PiContentComponent implements OnInit
{

  constructor(
    private piService: PiExtService,
    private identity: IdentityService)
  { }

  ngOnInit()
  {
    this.piService.GetWeatherForecast();
    //this.identity.login();
  }
}
