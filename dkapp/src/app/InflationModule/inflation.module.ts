import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { InflationScreenComponent } from './inflationScreen/inflationScreen.component';
import { PiControlsModule } from '../piControls/piControls.module';
import { ButtonModule } from 'primeng/button';
import { DropdownModule } from 'primeng/dropdown';
import { ChartModule } from 'primeng/chart';
import { DividerModule } from 'primeng/divider';
import { ListboxModule } from 'primeng/listbox';

@NgModule({
    imports: [
        FormsModule,
        PiControlsModule,
        PiControlsModule,
        ButtonModule,
        DropdownModule,
        ChartModule,
        DividerModule,
        ListboxModule
    ],
    declarations: [
        InflationScreenComponent,
    ],
    exports: [
        InflationScreenComponent,
    ]
})
export class InflationModule { }