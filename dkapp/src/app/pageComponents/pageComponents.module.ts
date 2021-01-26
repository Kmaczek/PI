import { NgModule } from '@angular/core';
import { PiNavbarComponent } from './piNavbar/piNavbar.component';
import { PiFooterComponent } from './piFooter/piFooter.component';
import { PiContentComponent } from './piContent/piContent.component';
import { PiControlsModule } from '../piControls/piControls.module';

@NgModule({
    imports: [
        PiControlsModule
    ],
    declarations: [
        PiNavbarComponent,
        PiContentComponent,
        PiFooterComponent
    ],
    exports: [
        PiNavbarComponent,
        PiContentComponent,
        PiFooterComponent
    ]
})
export class PiComponentsModule { }