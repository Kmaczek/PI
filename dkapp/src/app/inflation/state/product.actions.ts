import { createActionGroup, emptyProps, props } from '@ngrx/store';
import { GroupedProducts } from '../models/groupedProducts';

export const ProductActions = createActionGroup({
  source: 'Product',
  events: {
    'Load Products': emptyProps(),
    'Load Products Success': props<{ products: GroupedProducts[] }>(),
    'Load Products Failure': props<{ error: string }>(),
  }
});
