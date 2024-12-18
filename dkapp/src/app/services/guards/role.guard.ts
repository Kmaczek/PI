import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { map, take } from 'rxjs/operators';
import { selectUserRoles } from '../../state/user-selectors';
import { UserRoles } from '../../models/user/user-roles';

export const roleGuard: CanActivateFn = (route) => {
  const store = inject(Store);
  const router = inject(Router);

  const requiredRoles = route.data['roles'] as UserRoles[];

  return store.select(selectUserRoles).pipe(
    take(1),
    map((userRoles) => {
      const hasRequiredRole = requiredRoles.some((role) =>
        userRoles.includes(role)
      );

      if (!hasRequiredRole) {
        router.navigate(['/']);
        return false;
      }

      return true;
    })
  );
};
