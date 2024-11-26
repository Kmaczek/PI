import { Component, OnInit } from '@angular/core';
import { List } from 'linqts';
import * as moment from 'moment';
import { FlatSeries } from '../models/flatSerie';
import { FlatService } from '../services/flats.api.service';
import { ChartBasicData, ChartBasicOptions, ChartService } from 'src/app/services/chart.service';

@Component({
  selector: 'pi-flats-page',
  templateUrl: './flats-page.component.html',
  styleUrls: ['./flats-page.component.scss'],
})
export class FlatsPageComponent implements OnInit {
  public flatSeries = [];
  labelData: string[];
  basicData: ChartBasicData;
  basicOptions: ChartBasicOptions;

  constructor(private flatService: FlatService, private chartService: ChartService) {}

  ngOnInit() {
    const documentStyle = getComputedStyle(document.documentElement);
    // https://primeng.org/colors

    this.flatService.GetFlatsSeries().subscribe((x) => {
      moment();
      const series = new List<FlatSeries>(x);
      const data = series.Where((x) => x.avgPrice > 0).Select((x) => x.avgPricePerMeter);
      this.flatSeries = x;

      this.labelData = series.Select((x) => moment(x?.day).format('DD-MM-YYYY')).ToArray();

      this.basicData = {
        labels: this.labelData,
        datasets: [
          {
            label: 'Flat prices',
            data: data.ToArray(),
            fill: false,
            borderColor: documentStyle.getPropertyValue('--primary-500'),
            tension: 0.4,
          },
        ],
      };
    });

    this.basicOptions = this.chartService.getDefaultBasicOptions();
  }
}
