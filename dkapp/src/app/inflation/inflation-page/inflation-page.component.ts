import { Component, OnInit } from '@angular/core';
import { List } from 'linqts';
import { map } from 'rxjs/operators';
import { GroupedProducts } from '../models/grouppedProducts';
import { IPriceEntry, PriceEntry } from '../models/priceEntry';
import { Product } from '../models/product';
import { ProductsService } from '../services/product.api.service';

import { Meta, Title } from '@angular/platform-browser';
import { ChartBasicData, ChartBasicOptions, ChartService } from '../../services/chart.service';
import { UtilsService } from '../../services/utils.service';
import { ThemeService } from '../../services/theme.service';

@Component({
  selector: 'pi-inflation-page',
  templateUrl: './inflation-page.component.html',
  styleUrls: ['./inflation-page.component.scss'],
})
export class InflationPageComponent implements OnInit {
  products: GroupedProducts[];
  selectedProduct: Product = null;

  productSeries: PriceEntry[];
  chartData: number[];
  labelData: string[];
  basicData: ChartBasicData;
  basicOptions: ChartBasicOptions;
  isMobile = false;

  constructor(
    private productService: ProductsService,
    private chartService: ChartService,
    private utilsService: UtilsService,
    private themeService: ThemeService,
    private meta: Meta,
    private title: Title
  ) {}

  ngOnInit() {
    this.title.setTitle('PI - product prices');
    this.meta.updateTag({ name: 'description', content: 'Track product prices' });

    this.productService.GetProducts().subscribe((x) => {
      this.products = x;
    });

    this.basicOptions = this.chartService.getDefaultBasicOptions();
    this.basicData = this.chartService.getDefaultBasicData();
    this.isMobile = this.utilsService.isMobile();
  }

  loadSeries(event: any) {
    this.selectedProduct = event.option as Product;
    this.productService
      .GetProductSeries(this.selectedProduct.id)
      .pipe(
        map((ps) => {
          const psList = new List<IPriceEntry>(ps);
          const result = psList.Select((s) => new PriceEntry(s.price, s.createdDate));

          return result.ToArray();
        })
      )
      .subscribe((ps) => {
        this.productSeries = ps;

        const series = new List<PriceEntry>(ps);
        const data = series.Select((x) => x.price);
        this.chartData = data.ToArray();
        this.labelData = series
          .Select((x) => x.createdDateAsMoment().format('YYYY-MM-DD'))
          .ToArray();

        this.basicData = {
          labels: this.labelData,
          datasets: [
            {
              label: this.selectedProduct.name,
              data: this.chartData,
              fill: false,
              borderColor: this.chartService.getPrimary(),
              tension: 0.4,
            },
          ],
        } as ChartBasicData;
      });
  }

  navigateToProduct(): void {
    window.open(this.selectedProduct.uri, '_blank');
  }
}
