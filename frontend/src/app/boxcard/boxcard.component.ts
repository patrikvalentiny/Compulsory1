import {Component, Inject, inject} from '@angular/core';
import {CrudService} from "../crud.service";
import {Router} from "@angular/router";

@Component({
  selector: 'app-boxcard',
  templateUrl: './boxcard.component.html',
  styleUrls: ['./boxcard.component.css']
})
export class BoxCardComponent {
  public readonly service = inject(CrudService);
  public _router:Router = Inject(Router);

  constructor( ) {
  }

  selectBox(guid: string) {

  }

  deleteBox(guid: string) {
    this.service.deleteBox(guid);
  }

  navigateUpdateBox(guid: string) {
    this._router.navigateByUrl('/updatebox', {state: {guid: guid}});
  }
}
