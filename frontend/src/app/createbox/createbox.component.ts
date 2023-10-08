import {Component, inject, OnInit} from '@angular/core';
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {CrudService} from "../crud.service";
import {MaterialService} from "../material.service";
import {BoxWithMaterialId} from "../box-with-material-id";

@Component({
    selector: 'app-createbox',
    templateUrl: './createbox.component.html',
    styleUrls: ['./createbox.component.css']
})
export class CreateBoxComponent implements OnInit {
    readonly service = inject(CrudService);
    private readonly materialService = inject(MaterialService);

    widthInput = new FormControl(0, [Validators.required, Validators.min(0)]);
    heightInput = new FormControl(0, [Validators.required, Validators.min(0)]);
    depthInput = new FormControl(0, [Validators.required, Validators.min(0)]);
    descriptionInput = new FormControl("", [Validators.required]);
    locationInput = new FormControl("", [Validators.required]);
    titleInput = new FormControl("", [Validators.required, Validators.minLength(3)]);
    quantityInput = new FormControl(0, [Validators.required, Validators.min(0)]);

    respondBox: BoxWithMaterialId | undefined;

    formGroup = new FormGroup({
        width: this.widthInput,
        height: this.heightInput,
        depth: this.depthInput,
        description: this.descriptionInput,
        location: this.locationInput,
        title: this.titleInput,
        quantity: this.quantityInput,
    })

    constructor() {

    }

    async createBox() {
        this.respondBox = await this.service.createBox(this.formGroup);
        if (this.respondBox !== undefined) {
            this.formGroup.reset();
            this.service.selectedMaterial = null;
            setTimeout(() => {
                this.respondBox = undefined;
            }, 2000);
        }
    }

    ngOnInit(): void {
        this.service.selectedMaterial = null;
    }
}
