import { createFeature, createReducer, on } from '@ngrx/store';
import { ProductActions } from './product.actions';
import { GroupedProducts } from '../models/groupedProducts';
import {
  createInitialState,
  LoadingState,
  setError,
  setLoading,
  setSuccess,
} from '../../state/state-model';

export const productFeatureKey = 'product';

export interface State {
  products: LoadingState<GroupedProducts[]>;
}

export const initialState: State = {
  products: createInitialState(),
};

export const reducer = createReducer(
  initialState,
  on(
    ProductActions.loadProducts,
    (state): State => ({ ...state, products: setLoading(state.products) })
  ),
  on(
    ProductActions.loadProductsSuccess,
    (state, { products }): State => ({
      ...state,
      products: setSuccess(state.products, products),
    })
  ),
  on(
    ProductActions.loadProductsFailure,
    (state, { error }): State => ({
      ...state,
      products: setError(state.products, error),
    })
  )
);

export const productFeature = createFeature({
  name: productFeatureKey,
  reducer,
});
