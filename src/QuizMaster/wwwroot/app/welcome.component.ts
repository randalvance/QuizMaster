import { Component } from '@angular/core';

@Component({
    selector: 'welcome',
    templateUrl: 'app/welcome.component.html'
})
export class WelcomeComponent {
    firstName: string = "Foo";
    lastName: string = "Bar";
}