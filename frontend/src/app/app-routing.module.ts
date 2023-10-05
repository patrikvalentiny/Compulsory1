import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {CreateBoxComponent} from "./createbox/createbox.component";
import {BoxCardComponent} from "./boxcard/boxcard.component";
import {UpdateBoxComponent} from "./updatebox/updatebox.component";

const routes: Routes = [
  {
    path: '',
    component: BoxCardComponent
  },
  {
    path: 'home',
    redirectTo: ''  ,
    pathMatch: 'full'
  },
  {
    path: 'add',
    component:CreateBoxComponent
  },
  {
    path: 'update/:guid',
    component:UpdateBoxComponent
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes, {bindToComponentInputs:true})],
  exports: [RouterModule]
})
export class AppRoutingModule { }
