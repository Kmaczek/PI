import { Injectable, signal } from '@angular/core';
import { $dt } from '@primeng/themes';

@Injectable({
  providedIn: 'root',
})
export class ThemeService {
  isDark = signal(true);

  constructor() {
    this.setDarkByDefault();
  }

  check(): void {
    console.log($dt('primary.color'));
    console.log($dt('highlight.color'));
    console.log($dt('blue.500'));
    console.log('Dark', $dt('surface.500'));
  }

  toggleDarkMode() {
    const element = document.querySelector('html');
    element.classList.toggle('pi-dark');
    this.isDark.update(current => !current);
  }

  setDarkByDefault(): void {
    const element = document.querySelector('html');
    element.classList.add('pi-dark');
  }
}
