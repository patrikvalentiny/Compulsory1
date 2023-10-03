import {Component, inject} from '@angular/core';
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {Box} from "../box";
import {CrudService} from "../crud.service";

@Component({
  selector: 'app-createbox',
  templateUrl: './createbox.component.html',
  styleUrls: ['./createbox.component.css']
})
export class CreateBoxComponent {
  private readonly service = inject(CrudService);

  widthInput = new FormControl(0, [Validators.required, Validators.min(0)]);
  heightInput = new FormControl(0, [Validators.required, Validators.min(0)]);
  depthInput = new FormControl(0, [Validators.required, Validators.min(0)]);
  descriptionInput= new FormControl("", [Validators.required]);
  locationInput =  new FormControl("", [Validators.required]);

  respondBox: Box | undefined;

  formGroup = new FormGroup({
    width: this.widthInput,
    height: this.heightInput,
    depth: this.depthInput,
    description: this.descriptionInput,
    location: this.locationInput
  })
  constructor() {

  }

  async createBox() {
    this.respondBox = await this.service.createBox(this.formGroup);
  }
}
