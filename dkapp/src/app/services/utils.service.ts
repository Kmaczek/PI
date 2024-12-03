import { Injectable, signal } from '@angular/core';
import { BreakpointObserver } from '@angular/cdk/layout';

@Injectable({
  providedIn: 'root',
})
export class UtilsService {
  isMobile = signal(false);

  constructor(private breakpointObserver: BreakpointObserver) {}

  checkForMobile(): void {
    this.breakpointObserver.observe('(max-width: 767px)').subscribe(isMobileMatched => {
      this.isMobile.set(isMobileMatched.matches);
    });
  }
}
