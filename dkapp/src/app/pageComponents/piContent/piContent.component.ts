import { Component, OnInit } from '@angular/core';
import { IdentityService } from 'src/app/Services/external/identity.ext.service';
import { PiExtService } from 'src/app/Services/external/pi.ext.service';

@Component({
  selector: 'pi-content',
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
