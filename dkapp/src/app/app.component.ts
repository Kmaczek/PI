import { Component, OnInit } from '@angular/core';
import { Meta, Title } from '@angular/platform-browser';
import { definePreset, palette } from '@primeng/themes';
import Lara from '@primeng/themes/lara';
import { PrimeNG } from 'primeng/config';
import { InitializeService } from './services/initialize.service';
import { UserService } from './services/user.service';
import { UserRoles } from './models/user/user-roles';

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
export class AppComponent implements OnInit {
  constructor(
    private config: PrimeNG,
    private meta: Meta,
    private title: Title,
    private initializeService: InitializeService,
    private userService: UserService
  ) {
    this.meta.addTags([
      {
        name: 'keywords',
        content:
          'investment, inflation, price, price over time, products, price tracker, flats',
      },
      { name: 'robots', content: 'index, follow' },
      { name: 'author', content: 'Damian Kmak' },
      { name: 'date', content: '2024-11-28', scheme: 'YYYY-MM-DD' },
    ]);
    this.title.setTitle('PI - personal investments');
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

  ngOnInit(): void {
    this.initializeService.initialize();
  }
}
