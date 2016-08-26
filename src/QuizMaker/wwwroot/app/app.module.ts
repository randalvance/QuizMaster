import { NgModule }      from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { routing, appRoutingProviders } from './app.routing';

import { ProductModule } from './modules/product/product.module';

import { HighlightDirective } from './directives/highlight';

import { AppComponent }  from './app.component';

@NgModule({
    imports: [BrowserModule, routing, ProductModule],
    declarations: [AppComponent, HighlightDirective],
    providers: [appRoutingProviders],
    bootstrap: [AppComponent]
})
export class AppModule { }