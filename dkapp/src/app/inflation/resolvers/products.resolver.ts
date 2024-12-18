import { Injectable } from '@angular/core';
import { Resolve } from '@angular/router';
import { Store } from '@ngrx/store';
import { first, Observable } from 'rxjs';
import { ProductActions } from '../state/product.actions';
import { selectIsProductsLoaded } from '../state/product.selectors';

@Injectable()
export class ProductsResolver implements Resolve<boolean | void> {
  constructor(private store: Store) {}

  resolve(): Observable<boolean | void> {
    this.store.dispatch(ProductActions.loadProducts());
    return this.store
      .select(selectIsProductsLoaded)
      .pipe(first((loaded) => loaded));
  }
}
