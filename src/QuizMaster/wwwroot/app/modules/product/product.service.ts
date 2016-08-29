import { Injectable, Inject } from "@angular/core";
import { Http, Response } from "@angular/http";
import { Product } from "./product";
import "rxjs/add/operator/map";

@Injectable()
export class ProductService {
    private serviceUrl: string = "api/product";

    // Once Visual Studio support for Typescript improves, we will remove the @Inject
    constructor(@Inject(Http) private http: any) { }

    getProducts() {
        return this.http.get(this.serviceUrl).map((response: Response) => <Product[]>response.json());
    }

    getProduct(id: number) {
        return this.http.get(`${this.serviceUrl}/${id}`).map((response: Response) => <Product>response.json());
    }
}