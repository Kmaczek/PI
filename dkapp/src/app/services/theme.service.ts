import { Injectable, signal } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class ThemeService {
  isDark = signal(true);

  constructor() {
    this.setDarkByDefault();
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
