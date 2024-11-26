import { Component, OnInit } from '@angular/core';
import { PiExtService } from 'src/app/services/external/pi.ext-service';

@Component({
  selector: 'pi-home-page',
  templateUrl: './home-page.component.html',
  styleUrls: ['./home-page.component.scss']
})
export class HomePageComponent implements OnInit
{

  constructor(
    private piService: PiExtService)
  { }

  ngOnInit()
  {
    console.log('home');
    this.piService.GetWeatherForecast();
  }
}
