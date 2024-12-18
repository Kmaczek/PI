import { CommonModule } from '@angular/common';
import { Component, Input, OnInit } from '@angular/core';
import { Title, Meta } from '@angular/platform-browser';

@Component({
  selector: 'pi-title',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './title.component.html',
  styleUrl: './title.component.scss',
})
export class TitleComponent implements OnInit {
  @Input() displayTitle: string = '';
  @Input() pageTitle: string = '';
  @Input() metaDescription: string = '';

  constructor(private titleService: Title, private metaService: Meta) {}

  ngOnInit() {
    if (this.pageTitle) {
      this.titleService.setTitle(this.pageTitle);
    }

    if (this.metaDescription) {
      this.metaService.updateTag({
        name: 'description',
        content: this.metaDescription,
      });
    }
  }
}
