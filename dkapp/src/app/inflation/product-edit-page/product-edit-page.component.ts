import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { Store } from '@ngrx/store';
import { ListboxClickEvent } from 'primeng/listbox';
import { UtilsService } from '../../services/utils.service';
import { Product } from '../models/product';
import { ProductApiService } from '../services/product.api.service';
import { selectProducts } from '../state/product.selectors';

@Component({
  selector: 'pi-product-edit',
  templateUrl: './product-edit-page.component.html',
  styleUrl: './product-edit-page.component.scss',
})
export class ProductEditPageComponent implements OnInit {
  products$ = this.store.select(selectProducts);
  isMobile = false;
  selectedProduct: Product = null;
  newProduct: Product = null;

  formGroup: FormGroup | undefined;

  constructor(private store: Store, private utilsService: UtilsService, private productApi: ProductApiService) {}

  ngOnInit(): void {
    this.isMobile = this.utilsService.isMobile();
    this.formGroup = new FormGroup({
      id: new FormControl<number | null>({ value: null, disabled: true }),
      name: new FormControl<string | null>({ value: null, disabled: true }),
      code: new FormControl<string | null>({ value: null, disabled: true }),
      createdDate: new FormControl<Date | null>({ value: null, disabled: true }),
      updatedDate: new FormControl<Date | null>({ value: null, disabled: true }),

      uri: new FormControl<string | null>(null),
      parserType: new FormControl<number | null>(null),
      params: new FormControl<string | null>(null),
      track: new FormControl<boolean | null>(null),
      activeFrom: new FormControl<Date | null>(null),
      activeTo: new FormControl<Date | null>(null),
      category: new FormControl<number | null>(null),
    });
  }

  selectProduct(event: ListboxClickEvent): void {
    console.log(event);
    const pickedProduct = event.option as Product;
    //this.selectedProduct = event.option as Product;
    this.newProduct = null;
    this.productApi.getProduct(pickedProduct.id).subscribe(p => {
      this.selectedProduct = p;
      this.formGroup.patchValue(
        this.selectedProduct
      );
      this.formGroup.markAsPristine();
      this.formGroup.markAsUntouched();
    });
  }

  addNew(): void {
    this.selectedProduct = null;
    this.newProduct = {} as Product;
    this.formGroup.patchValue(this.newProduct);
    this.formGroup.markAsPristine();
    this.formGroup.markAsUntouched();
    this.formGroup.reset();
  }

  resetData(): void {
    this.clearEdits();
  }

  save(): void {}

  private clearEdits() {
    if (this.selectedProduct) {
      this.formGroup.patchValue(
        this.selectedProduct ? this.selectedProduct : this.newProduct
      );
      this.formGroup.markAsPristine();
      this.formGroup.markAsUntouched();
    } else {
      this.formGroup.reset();
    }
  }
}
