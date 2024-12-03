import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { ChartModule } from 'primeng/chart';
import { FlatsPageComponent } from './flatsPage/flats-page.component';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { ButtonGroupModule } from 'primeng/buttongroup';
import { RouterModule } from '@angular/router';
import { FlatsRoutes } from './flats-routing.module';

@NgModule({
    imports: [
        [RouterModule.forChild(FlatsRoutes)],
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