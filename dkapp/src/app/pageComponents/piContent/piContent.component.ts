import { Component, OnInit } from '@angular/core';
import { IdentityService } from 'src/app/services/external/identity.ext.service';
import { PiExtService } from 'src/app/services/external/pi.ext.service';

@Component({
  selector: 'piContent',
  templateUrl: './piContent.component.html',
  styleUrls: ['./piContent.component.css']
})
export class PiContentComponent implements OnInit
{

  constructor(
    private piService: PiExtService,
    private identity: IdentityService)
  { }

  ngOnInit()
  {
    //this.identity.login();
  }
}
