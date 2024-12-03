import { Component, Input, OnInit } from '@angular/core';
import { Meta, Title } from '@angular/platform-browser';
import { List } from 'linqts';
import moment from 'moment';
import { FlatSeries } from '../models/flatSerie';
import { FlatService } from '../services/flats.api.service';
import { ChartBasicData, ChartBasicOptions, ChartService } from '../../services/chart.service';

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
  _chartType: string;

  @Input()
  set chartType(value: string) {
    this._chartType = value;
    if (this._chartType == 'price') {
      this.loadPriceDataChart();
    } else {
      this.loadOffersDataChart();
    }
  }
  @Input() desc!: string;

  constructor(
    private flatService: FlatService,
    private chartService: ChartService,
    private title: Title,
    private meta: Meta,
  ) {}

  ngOnInit() {
    this.title.setTitle('PI - flat prices');
    this.meta.updateTag({ name: 'description', content: 'Track flats prices' });

    this.flatService.GetFlatsSeries().subscribe((x) => {
      this.flatSeries = x;

      const series = new List<FlatSeries>(x);
      this.labelData = series.Select((x) => moment(x?.day).format('DD-MM-YYYY')).ToArray();

      if (this._chartType == 'price') {
        this.loadPriceDataChart();
      } else {
        this.loadOffersDataChart();
      }
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
