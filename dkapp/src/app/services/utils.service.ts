import { Injectable } from '@angular/core';
import { BreakpointObserver } from '@angular/cdk/layout';

@Injectable({
  providedIn: 'root',
})
export class UtilsService {
  constructor(private breakpointObserver: BreakpointObserver) {}

  isMobile(): boolean {
    return this.breakpointObserver.isMatched('(max-width: 768px)');
  }
}
