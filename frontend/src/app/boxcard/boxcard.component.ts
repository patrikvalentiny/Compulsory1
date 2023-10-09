import {Component, inject, OnInit} from '@angular/core';
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
  confirmDelete: boolean = false;

    constructor() {
    }

    deleteBox(guid: string) {
      try {
        this.service.deleteBox(guid);
        this.confirmDelete = true;
        setTimeout(() => {
          this.confirmDelete = false;
        }, 1000);
      } catch (e) {
        console.log(e);
      }
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
