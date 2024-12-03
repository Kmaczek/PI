import { Injectable } from '@angular/core';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { loadUser } from '../state/user-actions';
import { selectIsUserProfileLoaded } from '../state/user-selectors';

@Injectable({
  providedIn: 'root',
})
export class InitializeService {
  constructor(private store: Store) {}

  initialize(): Observable<unknown> {
    this.store.dispatch(loadUser());

    return this.store.select(selectIsUserProfileLoaded);
  }
}
