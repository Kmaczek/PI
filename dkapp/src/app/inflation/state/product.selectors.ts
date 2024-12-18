import { createFeatureSelector, createSelector } from '@ngrx/store';
import * as fromProduct from './product.reducer';
import { LoadingStatus } from '../../state/state-model';

export const selectProductState = createFeatureSelector<fromProduct.State>(
  fromProduct.productFeatureKey
);

export const selectProductsState = createSelector(
  selectProductState,
  (state) => state.products
);

export const selectProducts = createSelector(
  selectProductState,
  (state) => state.products.data
);

export const selectProductsStatus = createSelector(
  selectProductsState,
  (profile) => profile.status
);

export const selectIsProductsLoaded = createSelector(
  selectProductsStatus,
  (status) =>
    status === LoadingStatus.SUCCESS || status === LoadingStatus.FAILED
);
