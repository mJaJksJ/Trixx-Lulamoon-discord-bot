import { NgModule } from "@angular/core";
import { PagesComponent } from "./pages.component";
import { ThemeModule } from "../themes/theme.module";
import { NbMenuModule } from "@nebular/theme";
import { SharedModule } from "../shared/modules/shared.module";
import { PagesRoutingModule } from "./pages-routing.module";

@NgModule({
  imports: [
    SharedModule,
    ThemeModule,
    NbMenuModule,
    PagesRoutingModule,
   ],
  declarations: [
    PagesComponent,
  ],
})
export class PagesModule {
}