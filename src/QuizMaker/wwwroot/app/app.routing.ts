import { Routes, RouterModule } from '@angular/router';
import { WelcomeComponent } from './welcome.component';

const appRoutes: Routes = [
    { path: '', component: WelcomeComponent }
];

export const appRoutingProviders: any[] = [];

export const routing = RouterModule.forRoot(appRoutes);