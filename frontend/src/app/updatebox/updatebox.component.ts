import {Component, inject, Input, OnInit} from '@angular/core';
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {Box} from "../box";
import {CrudService} from "../crud.service";
@Component({
  selector: 'app-updatebox',
  templateUrl: './updatebox.component.html',
  styleUrls: ['./updatebox.component.css']
})
export class UpdateBoxComponent implements OnInit{
  private readonly service = inject(CrudService);
  @Input('guid') boxGuid = '';
  selectedBox: Box | undefined;

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

  ngOnInit(): void {

  }

  updateBox() {

  }
}
