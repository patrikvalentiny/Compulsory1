import {Component, inject, Input, OnInit} from '@angular/core';
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {Box} from "../box";
import {CrudService} from "../crud.service";
@Component({
    selector: 'app-updatebox',
    templateUrl: './updatebox.component.html',
    styleUrls: ['./updatebox.component.css']
})
export class UpdateBoxComponent implements OnInit {
    private readonly service = inject(CrudService);
    @Input('guid') boxGuid = '';

    selectedBox: Box | undefined;
    widthInput = new FormControl(0, [Validators.required, Validators.min(0)]);
    heightInput = new FormControl(0, [Validators.required, Validators.min(0)]);
    depthInput = new FormControl(0, [Validators.required, Validators.min(0)]);
    descriptionInput = new FormControl("", [Validators.required]);
    locationInput = new FormControl("", [Validators.required]);

    respondBox: Box | undefined;

    formGroup = new FormGroup({
        width: this.widthInput,
        height: this.heightInput,
        depth: this.depthInput,
        description: this.descriptionInput,
        location: this.locationInput
    })
    editingAllowed: boolean = false;

    constructor() {

    }

    async ngOnInit() {
        this.selectedBox = await this.service.getBox(this.boxGuid);

        this.widthInput.setValue(this.selectedBox?.width || null);
        this.heightInput.setValue(this.selectedBox?.height || null);
        this.depthInput.setValue(this.selectedBox?.depth || null);
        this.descriptionInput.setValue(this.selectedBox?.description || null);
        this.locationInput.setValue(this.selectedBox?.location || null);

        this.formGroup.disable();

    }

    async updateBox() {
        await this.service.updateBox(this.formGroup, this.boxGuid);
    }

    allowEdit() {
        this.editingAllowed = !this.editingAllowed;
        if (this.editingAllowed) {
            this.formGroup.enable();
        } else {
            this.formGroup.disable();
        }
    }
}
