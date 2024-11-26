import { Component, OnInit } from '@angular/core';
import { IdentityService } from 'src/app/services/external/identity.ext-service';
import { PiExtService } from 'src/app/services/external/pi.ext-service';

@Component({
  selector: 'pi-home-page',
  templateUrl: './home-page.component.html',
  styleUrls: ['./home-page.component.scss']
})
export class HomePageComponent implements OnInit
{

  constructor(
    private piService: PiExtService,
    private identity: IdentityService)
  { }

  ngOnInit()
  {
    console.log('home');
    this.piService.GetWeatherForecast();
    //this.identity.login(); 
  }
}
