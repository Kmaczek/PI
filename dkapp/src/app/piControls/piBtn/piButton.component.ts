import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'piButton',
  templateUrl: './piButton.component.html',
  styleUrls: ['./piButton.component.css']
})
export class PiButtonComponent implements OnInit {

  @Input() text: string = 'button';

  constructor() { }

  ngOnInit() {
  }

}
