import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { ButtonModule } from 'primeng/button';
import { ChartModule } from 'primeng/chart';
import { DividerModule } from 'primeng/divider';
import { DropdownModule } from 'primeng/dropdown';
import { ListboxModule } from 'primeng/listbox';
import { Toolbar } from 'primeng/toolbar';
import { TitleComponent } from '../shared/title/title.component';
import { InflationPageComponent } from './inflation-page/inflation-page.component';
import { InflationRoutes } from './inflation-routing.module';
import { ProductEditPageComponent } from './product-edit-page/product-edit-page.component';
import { ProductsResolver } from './resolvers/products.resolver';
import { ProductEffects } from './state/product.effects';
import * as fromProduct from './state/product.reducer';

@NgModule({
  imports: [
    [RouterModule.forChild(InflationRoutes)],
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    ButtonModule,
    DropdownModule,
    ChartModule,
    DividerModule,
    ListboxModule,
    StoreModule.forFeature(fromProduct.productFeatureKey, fromProduct.reducer),
    EffectsModule.forFeature([ProductEffects]),
    TitleComponent,
    Toolbar,
  ],
  declarations: [InflationPageComponent, ProductEditPageComponent],
  exports: [InflationPageComponent, ProductEditPageComponent],
  providers: [ProductsResolver],
})
export class InflationModule {}
