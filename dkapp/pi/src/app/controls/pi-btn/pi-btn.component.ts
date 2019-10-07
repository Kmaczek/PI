import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'piBtn',
  templateUrl: './pi-btn.component.html',
  styleUrls: ['./pi-btn.component.css']
})
export class PiBtnComponent implements OnInit {

  @Input() text: string = 'button';

  constructor() { }

  ngOnInit() {
  }

}
