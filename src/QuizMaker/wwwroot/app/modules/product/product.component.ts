import { Component, Input, OnInit, OnDestroy } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs/Rx';
import { Product } from './product';
import { ProductService } from './product.service';
import { Subscription } from 'rxjs/Subscription';

@Component({
    selector: 'product-detail',
    templateUrl: 'product.component.html',
    moduleId: module.id
})
export class ProductComponent implements OnInit, OnDestroy {

    private product: Product;

    private sub: Subscription;

    constructor(
        private route: ActivatedRoute,
        private router: Router,
        private productService: ProductService) { }

    ngOnInit() {
        this.sub = this.route.params.subscribe(params => {
            let id = +params['id']; // (+) converts string 'id' to a number
            this.productService.getProduct(id).then(product => this.product = product);
        });
        // or if component will not be reused (ngOnDestroy not needed)
        /*
            let id = +this.route.snapshot.params['id'];
            this.productService.getHero(id).then(product => this.product = product);
        */
    }

    ngOnDestroy() {
        this.sub.unsubscribe();
    }

    goToProducts() {
        this.router.navigate(['/products']);
    }
}