import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { PiControlsModule } from '../piControls/piControls.module';
import { FlatsScreenComponent } from './flatsScreen/flatsScreen.component';
import { TableModule } from 'primeng/table';

@NgModule({
    imports: [
        CommonModule,
        TableModule,
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