import {Component, inject, OnInit} from '@angular/core';
import {Material} from "../material";
import {CrudService} from "../crud.service";

@Component({
    selector: 'app-materials-dropdown',
    templateUrl: './materials-dropdown.component.html',
    styleUrls: ['./materials-dropdown.component.css']
})
export class MaterialsDropdownComponent implements OnInit {
    public readonly boxService = inject(CrudService);
    protected readonly self = self;

    ngOnInit(): void {
        this.boxService.getMaterials();
    }

    filterByMaterial(material: Material) {
        if (material === this.boxService.selectedMaterial) {
            this.boxService.selectedMaterial = null;
            this.boxService.filterBoxes();
        } else {
            this.boxService.selectedMaterial = material;
            this.boxService.filterBoxes();
        }
    }
}
