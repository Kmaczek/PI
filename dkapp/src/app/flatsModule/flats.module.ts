import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { ChartModule } from 'primeng/chart';
import { FlatsPageComponent } from './flatsScreen/flats-page.component';
import { TableModule } from 'primeng/table';

@NgModule({
    imports: [
        CommonModule,
        TableModule,
        ChartModule
    ],
    declarations: [
        FlatsPageComponent,
    ],
    exports: [
        FlatsPageComponent,
    ]
})
export class FlatsModule { }