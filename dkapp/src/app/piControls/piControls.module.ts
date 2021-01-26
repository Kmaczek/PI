import { NgModule } from '@angular/core';
import { PiLineChartComponent } from './piLineChart/piLineChart.component';
import { PiButtonComponent } from './piBtn/piButton.component';

@NgModule({
    imports: [

    ],
    declarations: [
        PiButtonComponent,
        PiLineChartComponent
    ],
    exports: [
        PiButtonComponent,
        PiLineChartComponent
    ]
})
export class PiControlsModule { }