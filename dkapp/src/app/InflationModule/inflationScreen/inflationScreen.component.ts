import { Component, OnInit } from '@angular/core';
import { IdentityService } from 'src/app/services/external/identity.ext.service';
import { ProductsService } from '../services/product.api.service';
import { Product } from '../models/product';
import { PriceSerie, IPriceSerie } from '../models/priceSerie';
import { List } from 'linqts';
import { map } from 'rxjs/operators';

@Component({
  selector: 'inflationScreen',
  templateUrl: './inflationScreen.component.html',
  styleUrls: ['./inflationScreen.component.css']
})
export class InflationScreenComponent implements OnInit
{
  products: Product[];
  selectedProduct: Product;

  productSeries: PriceSerie[];
  chartData: number[];
  labelData: string[];
  basicData: any;
  basicOptions: any;

  constructor(
    private productService: ProductsService,
    private identity: IdentityService)
  {

  }

  ngOnInit()
  {
    console.log('inflation');
    this.productService.GetProducts().subscribe(x =>
    {
      this.products = x;
    });

    this.basicData = {
      labels: ['January', 'February', 'March', 'April', 'May', 'June', 'July'],
      datasets: [
        {
          label: 'First Dataset',
          data: this.chartData,
          fill: false,
          borderColor: '#42A5F5'
        }
      ]
    }

    this.basicOptions = {
      legend: {
        labels: {
          fontColor: '#495057'
        }
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
}
