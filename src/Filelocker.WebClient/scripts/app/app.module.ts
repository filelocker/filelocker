import {NgModule}       from "@angular/core";
import {BrowserModule} from "@angular/platform-browser";
import {AppComponent}   from "./app.component";
import {routing} from "./app.routes";
import {HttpModule} from "@angular/http";
import {FilesModule} from "./files/files.module";

@NgModule({
    imports:      [
        BrowserModule,
        HttpModule,
        routing,

        FilesModule
    ],
    declarations: [
        AppComponent
    ],
    bootstrap:    [AppComponent],
})
export class AppModule {}