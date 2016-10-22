import {NgModule} from "@angular/core";
import {FilesComponent} from "./files.component";
import {FilesService} from "./files.service";
import {routing} from "./files.routing";

@NgModule({
    declarations: [FilesComponent],
    imports: [routing],
    providers: [FilesService],
    exports: [FilesComponent]
})
export class FilesModule {}
