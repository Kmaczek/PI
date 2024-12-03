import { Component, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { PiExtService } from '../services/api/pi.ext-service';

@Component({
  selector: 'pi-home-page',
  templateUrl: './home-page.component.html',
  styleUrls: ['./home-page.component.scss'],
})
export class HomePageComponent implements OnInit {
  constructor(private piService: PiExtService, private title: Title) {}

  ngOnInit() {
    this.title.setTitle('PI - personal investments home page');
    console.log('home');
    this.piService.GetWeatherForecast();
  }
}
