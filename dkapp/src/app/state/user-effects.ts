import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { of } from 'rxjs';
import { catchError, map, switchMap } from 'rxjs/operators';
import { UserApiService } from '../services/api/user.api.service';
import * as UserActions from './user-actions';

@Injectable()
export class UserEffects {
  constructor(
    private actions$: Actions,
    private userApiService: UserApiService
  ) {}

  loadUser$ = createEffect(() => {
    return this.actions$.pipe(
      ofType(UserActions.loadUser),
      switchMap(() =>
        this.userApiService.getUser().pipe(
          map((user) => UserActions.loadUserSuccess({ user })),
          catchError((error) =>
            of(UserActions.loadUserFailure({ error: error.message }))
          )
        )
      )
    );
  });
}
