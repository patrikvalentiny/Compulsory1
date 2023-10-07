import {Component, Inject, inject} from '@angular/core';
import {CrudService} from "../crud.service";
import {Router} from "@angular/router";
import {FormControl, Validators} from "@angular/forms";

@Component({
  selector: 'app-boxcard',
  templateUrl: './boxcard.component.html',
  styleUrls: ['./boxcard.component.css']
})
export class BoxCardComponent {
  public readonly service = inject(CrudService);
  public _router:Router = Inject(Router);
  searchTerm = new FormControl("", [Validators.required]);

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

    search() {
      this.service.searchTerm= this.searchTerm.value;
        this.service.filterBoxes();
    }
}