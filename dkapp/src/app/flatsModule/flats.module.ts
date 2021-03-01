import { NgModule } from '@angular/core';
import { PiControlsModule } from '../piControls/piControls.module';
import { FlatsScreenComponent } from './flatsScreen/flatsScreen.component';

@NgModule({
    imports: [
        PiControlsModule
    ],
    declarations: [
        FlatsScreenComponent,
    ],
    exports: [
        FlatsScreenComponent,
    ]
})
export class FlatsModule { }