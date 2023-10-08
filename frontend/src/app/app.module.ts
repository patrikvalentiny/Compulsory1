import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { CreateBoxComponent } from './createbox/createbox.component';
import {ReactiveFormsModule} from "@angular/forms";
import {HttpClientModule} from "@angular/common/http";
import {BoxCardComponent } from './boxcard/boxcard.component';
import {HomeComponent} from "./home/home.component";
import { MaterialsDropdownComponent } from './materials-dropdown/materials-dropdown.component';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    CreateBoxComponent,
    BoxCardComponent,
    MaterialsDropdownComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    ReactiveFormsModule,
    HttpClientModule,
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
