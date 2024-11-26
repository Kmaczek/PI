import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { InflationPageComponent } from './inflation-page/inflation-page.component';
import { ButtonModule } from 'primeng/button';
import { DropdownModule } from 'primeng/dropdown';
import { ChartModule } from 'primeng/chart';
import { DividerModule } from 'primeng/divider';
import { ListboxModule } from 'primeng/listbox';
import { CommonModule } from '@angular/common';

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        ButtonModule,
        DropdownModule,
        ChartModule,
        DividerModule,
        ListboxModule
    ],
    declarations: [
        InflationPageComponent,
    ],
    exports: [
        InflationPageComponent,
    ]
})
export class InflationModule { }