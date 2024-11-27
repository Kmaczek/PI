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
    this.flatService.GetFlatsSeries().subscribe((x) => {
      this.flatSeries = x;

      const series = new List<FlatSeries>(x);
      this.labelData = series.Select((x) => moment(x?.day).format('DD-MM-YYYY')).ToArray();

      this.loadPriceDataChart();
    });

    this.basicOptions = this.chartService.getDefaultBasicOptions();
  }

  loadPriceDataChart(): void {
    const documentStyle = getComputedStyle(document.documentElement);
    const series = new List<FlatSeries>(this.flatSeries);
    const data = series.Where((x) => x.avgPrice > 0).Select((x) => x.avgPricePerMeter);
    this.basicData = {
      labels: this.labelData,
      datasets: [
        {
          label: 'Prices',
          data: data.ToArray(),
          fill: false,
          borderColor: documentStyle.getPropertyValue('--pi-primary-500'),
          tension: 0.4,
        },
      ],
    };
  }

  loadOffersDataChart(): void {
    const documentStyle = getComputedStyle(document.documentElement);
    const series = new List<FlatSeries>(this.flatSeries);
    const data = series.Where((x) => x.amount > 0).Select((x) => x.amount);
    this.basicData = {
      labels: this.labelData,
      datasets: [
        {
          label: 'Offers',
          data: data.ToArray(),
          fill: false,
          borderColor: documentStyle.getPropertyValue('--pi-primary-500'),
          tension: 0.4,
        },
      ],
    };
  }
}
