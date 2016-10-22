import { Component } from "@angular/core";
import {FilesService} from "./files.service";

@Component({
    template: `
        <my-nav></my-nav>
        <h1>Greeting test</h1>
        <p id="greeting">{{greeting}}</p>
    `
})
export class FilesComponent {
    constructor(private filesService: FilesService) {
    }

    ngOnInit() {
        this.greet("Michal");
    }

    greeting: string;

    greet(name: string): void {
        this.filesService
            .greet(name)
            .subscribe(data => this.greeting = data);
    }
}