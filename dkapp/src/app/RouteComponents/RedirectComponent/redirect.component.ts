import { Component } from '@angular/core';

@Component({
    selector: 'pi-redirect',
    templateUrl: './redirect.component.html'
})
export class RedirectComponent
{
    constructor()
    {
        console.log("in redirect");
    }
}