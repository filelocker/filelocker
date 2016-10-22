import {Routes, RouterModule} from "@angular/router";
import {ModuleWithProviders} from "@angular/core";

const appRoutes: Routes  = [
    { path: "", redirectTo: "files", pathMatch: "full" },
    { path: "product", loadChildren: "app/product/product.module#ProductModule"  }
];

export const routing: ModuleWithProviders  = RouterModule.forRoot(appRoutes);