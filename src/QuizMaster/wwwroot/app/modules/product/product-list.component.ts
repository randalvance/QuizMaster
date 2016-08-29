import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs/Rx';
import { Product } from './product';
import { ProductService } from './product.service';


@Component({
    selector: 'product-list',
    templateUrl: 'product-list.component.html',
    moduleId: module.id
})
export class ProductListComponent implements OnInit {
    products: Observable<Product>;

    constructor(
        private router: Router,
        private productService: ProductService) { }

    ngOnInit() {
        this.getProducts();
    }

    onSelect(product: Product) {
        this.router.navigate(['/product', product.id]);
    }


    getProducts() {
        this.products = this.productService.getProducts();
    }
}