import {Component, Inject, inject, OnInit} from '@angular/core';
import {CrudService} from "../crud.service";
import {FormControl, Validators} from "@angular/forms";

@Component({
    selector: 'app-boxcard',
    templateUrl: './boxcard.component.html',
    styleUrls: ['./boxcard.component.css']
})
export class BoxCardComponent implements OnInit {
    public readonly service = inject(CrudService);
    searchTerm = new FormControl("", [Validators.required]);

    constructor() {
    }

    deleteBox(guid: string) {
        this.service.deleteBox(guid);
    }

    search() {
        this.service.searchTerm = this.searchTerm.value;
        this.service.filterBoxes();
    }

    async ngOnInit() {
        this.service.selectedMaterial = null;
        await this.service.getBoxes();
    }
}
