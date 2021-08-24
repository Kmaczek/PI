import { Component, OnInit, ViewChild } from '@angular/core';
import { IdentityService } from 'src/app/services/external/identity.ext.service';
import { PiExtService } from 'src/app/services/external/pi.ext.service';
import { FlatService } from '../services/flats.api.service';
import { FlatSerie } from '../models/flatSerie';
import { PiLineChartComponent } from '../../piControls/piLineChart/piLineChart.component';
import { List } from 'linqts';
import * as moment from 'moment';

@Component({
  selector: 'flatsScreen',
  templateUrl: './flatsScreen.component.html',
  styleUrls: ['./flatsScreen.component.css']
})
export class FlatsScreenComponent implements OnInit
{
  @ViewChild('lineChart') lineChart: PiLineChartComponent;
  
  public flatSeries = [];

  constructor(
    private piService: PiExtService,
    private identity: IdentityService,
    private flatService: FlatService)
  {
    //this.flatSeries.push(new FlatSerie(2, new Date(), 343.3, 355.5, 34));
  }

  ngOnInit()
  {
    console.log('flats');
    this.flatService.GetFlatsSeries().subscribe(x => {
      moment()
      let series = new List<FlatSerie>(x);
      let data = series.Where(x => x.avgPrice > 0).Select(x => x.avgPricePerMeter);
      let labels = series.Where(x => x.avgPrice > 0).Select(x => moment(x.day).format('YY-MM-DD'));
      this.flatSeries = x;
      this.lineChart.data = data.ToArray();
      this.lineChart.labels = labels.ToArray();
      this.lineChart.loadChart();
    });
    //this.identity.login(); 
  }
}
