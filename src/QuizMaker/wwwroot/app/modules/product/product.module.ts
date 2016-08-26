import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';

import { ProductListComponent } from './product-list.component';
import { ProductComponent } from './product.component';
import { ProductService } from './product.service';

import { productRouting } from './product.routing';

@NgModule({
    imports: [
        FormsModule,
        CommonModule,
        HttpModule,
        productRouting
    ],
    declarations: [
        ProductListComponent,
        ProductComponent
    ],
    providers: [ProductService]
})
export class ProductModule { }