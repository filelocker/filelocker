import {RouterModule} from "@angular/router";
import {ModuleWithProviders} from "@angular/core";
import {FilesComponent} from "./files.component";

export const routing: ModuleWithProviders = RouterModule.forChild([
    { path: "files", component: FilesComponent}
]);