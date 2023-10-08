import {Component, inject, Input, OnInit} from '@angular/core';
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {CrudService} from "../crud.service";
import {BoxWithMaterialId} from "../box-with-material-id";
import {Box} from "../box";

@Component({
    selector: 'app-createbox',
    templateUrl: './createbox.component.html',
    styleUrls: ['./createbox.component.css']
})
export class CreateBoxComponent implements OnInit {
    readonly service = inject(CrudService);

    @Input('guid') boxGuid = '';


    widthInput = new FormControl(0, [Validators.required, Validators.min(0)]);
    heightInput = new FormControl(0, [Validators.required, Validators.min(0)]);
    depthInput = new FormControl(0, [Validators.required, Validators.min(0)]);
    descriptionInput = new FormControl("", [Validators.required]);
    locationInput = new FormControl("", [Validators.required]);
    titleInput = new FormControl("", [Validators.required, Validators.minLength(3)]);
    quantityInput = new FormControl(0, [Validators.required, Validators.min(0)]);

    respondBox: BoxWithMaterialId | undefined;
    selectedBox: Box | undefined;

    formGroup = new FormGroup({
        width: this.widthInput,
        height: this.heightInput,
        depth: this.depthInput,
        description: this.descriptionInput,
        location: this.locationInput,
        title: this.titleInput,
        quantity: this.quantityInput,
    })

    editingAllowed = true;
    confirmationText: string = "Your box has been created!";
    showResponse: boolean = false;
    showResponseFail: boolean = false;

    constructor() {

    }

    async createBox() {
        this.respondBox = await this.service.createBox(this.formGroup);
        if (this.respondBox !== undefined) {
            this.formGroup.reset();
            this.confirmationText = "Your box has been created!";
            this.showResponse = true;
            this.service.selectedMaterial = null;
            setTimeout(() => {
                this.showResponse = false;
            }, 2000);
        }
    }

    async ngOnInit() {


        if (this.boxGuid !== undefined) {
            this.editingAllowed = false;
            this.selectedBox = await this.service.getBox(this.boxGuid);

            this.widthInput.setValue(this.selectedBox?.width || null);
            this.heightInput.setValue(this.selectedBox?.height || null);
            this.depthInput.setValue(this.selectedBox?.depth || null);
            this.descriptionInput.setValue(this.selectedBox?.description || null);
            this.locationInput.setValue(this.selectedBox?.location || null);
            this.titleInput.setValue(this.selectedBox?.title || null);
            this.quantityInput.setValue(this.selectedBox?.quantity || null);
            this.service.selectedMaterial = this.selectedBox?.material || null;

            this.formGroup.disable();


        } else {
            this.service.selectedMaterial = null;
        }
    }

    allowEdit() {
        this.editingAllowed = !this.editingAllowed;
        if (this.editingAllowed) {
            this.formGroup.enable();
        } else {
            this.formGroup.disable();
        }
    }

    async updateBox() {
        try {
            await this.service.updateBox(this.formGroup, this.boxGuid);
            this.confirmationText = "Your box has been updated!";
            this.showResponse = true;
            this.editingAllowed = false;
            setTimeout(() => {
                this.showResponse = false;
            }, 2000);
        } catch (e){
            this.confirmationText = "Whoopsy daisy!";
            this.showResponseFail = true;
            this.showResponse = true;
            setTimeout(() => {
                this.showResponse = false;
            }, 2000);
            console.log(e);
        }

    }
}
