import { Component } from '@angular/core';
import { definePreset, palette } from '@primeng/themes';
import Lara from '@primeng/themes/lara';
import { PrimeNG } from 'primeng/config';

const primaryColor = palette('{amber}');
const darkSurfaceColor = palette('{stone}');
const PiPreset = definePreset(Lara, {
  semantic: {
    primary: primaryColor,
    colorScheme: {
      dark: {
        surface: darkSurfaceColor,
        text: {
          color: '{surface.200}',
          hoverColor: '{surface.50}',
          mutedColor: '{surface.400}',
          hoverMutedColor: '{surface.300}',
        },
      },
    },
  },
  components: {
    button: {
      label: {
        fontWeight: 500,
      },
    },
  },
});

@Component({
  selector: 'pi-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent {
  title = 'pi';

  constructor(private config: PrimeNG) {
    // Default theme configuration
    console.log(PiPreset);
    this.config.theme.set({
      preset: PiPreset,
      options: {
        prefix: 'pi',
        darkModeSelector: '.pi-dark',
      },
    });
    const element = document.querySelector('html');
    element.classList.add('pi-dark');
  }
}
