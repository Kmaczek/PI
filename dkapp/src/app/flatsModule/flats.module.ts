import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { ChartModule } from 'primeng/chart';
import { FlatsPageComponent } from './flatsScreen/flats-page.component';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { ButtonGroupModule } from 'primeng/buttongroup';

@NgModule({
    imports: [
        CommonModule,
        TableModule,
        ChartModule,
        ButtonModule,
        ButtonGroupModule
    ],
    declarations: [
        FlatsPageComponent,
    ],
    exports: [
        FlatsPageComponent,
    ]
})
export class FlatsModule { }