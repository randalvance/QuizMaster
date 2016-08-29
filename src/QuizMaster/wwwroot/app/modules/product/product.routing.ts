import { Routes, RouterModule } from '@angular/router';
import { ProductListComponent } from './product-list.component';
import { ProductComponent } from './product.component';

const productRoutes: Routes = [
    { path: 'products', component: ProductListComponent },
    { path: 'product/:id', component: ProductComponent }
];

export const productRouting = RouterModule.forChild(productRoutes);