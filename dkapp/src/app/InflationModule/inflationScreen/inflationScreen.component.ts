import { Component, OnInit } from '@angular/core';
import { IdentityService } from 'src/app/Services/external/identity.ext.service';
import { ProductsService } from '../services/product.api.service';
import { Product } from '../models/product';
import { PriceSerie, IPriceSerie } from '../models/priceSerie';
import { List } from 'linqts';
import { map } from 'rxjs/operators';
import { GrouppedProducts } from '../models/grouppedProducts';
import { PrimeNGConfig } from 'primeng/api';

@Component({
  selector: 'inflationScreen',
  templateUrl: './inflationScreen.component.html',
  styleUrls: ['./inflationScreen.component.scss']
})
export class InflationScreenComponent implements OnInit
{
  products: GrouppedProducts[];
  selectedProduct: Product = null;

  productSeries: PriceSerie[];
  chartData: number[];
  labelData: string[];
  basicData: any;
  basicOptions: any;

  constructor(
    private productService: ProductsService,
    private identity: IdentityService,
    private primengConfig: PrimeNGConfig)
  {

  }

  ngOnInit()
  {
    this.primengConfig.ripple = true;
    console.log('inflation');
    this.productService.GetProducts().subscribe(x =>
    {
      this.products = x;
    });

    this.basicOptions = {
      legend: {
        labels: {
          fontColor: '#495057'
        },
      },
      scales: {
        xAxes: [{
          ticks: {
            fontColor: '#495057'
          }
        }],
        yAxes: [{
          ticks: {
            fontColor: '#495057'
          }
        }]
      }
    };
  }

  loadSeries(event: any)
  {
    this.selectedProduct = event.option as Product;
    this.productService.GetProductSeries(this.selectedProduct.id)
      .pipe(map(ps => {
        let psList = new List<IPriceSerie>(ps);
        let result = psList.Select(s => new PriceSerie(s.price, s.createdDate));

        return result.ToArray();
      }))
      .subscribe(ps =>
      {
        this.productSeries = ps;

        let series = new List<PriceSerie>(ps);
        let data = series.Select(x => x.price);
        this.chartData = data.ToArray();
        this.labelData = series.Select(x => x.createdMoment().format('YYYY-MM-DD')).ToArray();

        this.basicData = {
          labels: this.labelData,
          datasets: [
            {
              label: this.selectedProduct.name,
              data: this.chartData,
              fill: false,
              borderColor: '#42A5F5'
            }
          ]
        }
      });
  }

  navigateToProduct(): void {
    window.open(this.selectedProduct.uri, "_blank");
  }
}
