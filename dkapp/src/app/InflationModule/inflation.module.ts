import { NgModule } from '@angular/core';
import { InflationScreenComponent } from './inflationScreen/inflationScreen.component';
import { PiControlsModule } from '../piControls/piControls.module';

@NgModule({
    imports: [
        PiControlsModule
    ],
    declarations: [
        InflationScreenComponent,
    ],
    exports: [
        InflationScreenComponent,
    ]
})
export class InflationModule { }