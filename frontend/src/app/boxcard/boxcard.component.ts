import {Component, inject} from '@angular/core';
import {CrudService} from "../crud.service";

@Component({
  selector: 'app-boxcard',
  templateUrl: './boxcard.component.html',
  styleUrls: ['./boxcard.component.css']
})
export class BoxCardComponent {
  public readonly service = inject(CrudService);

  selectBox(guid: string) {

  }
}
