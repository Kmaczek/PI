import { Injectable } from '@angular/core';
import { Store } from '@ngrx/store';
import { selectUserRoles } from '../state/user-selectors';
import { UserRoles } from '../models/user/user-roles';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  public IsAdmin = false;

  getRoles$ = this.store.select(selectUserRoles);
  
  constructor(private store: Store) {
    this.getRoles$.subscribe(roles => {
      this.IsAdmin = roles.includes(UserRoles.Admin);
    });
  }
}
