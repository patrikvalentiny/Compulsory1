import {Component, inject, OnInit} from '@angular/core';
import {MaterialService} from "../material.service";
import {Material} from "../material";
import {CrudService} from "../crud.service";

@Component({
    selector: 'app-materials-dropdown',
    templateUrl: './materials-dropdown.component.html',
    styleUrls: ['./materials-dropdown.component.css']
})
export class MaterialsDropdownComponent implements OnInit {
    public readonly service = inject(MaterialService);
    public readonly boxService = inject(CrudService);
    selectedMaterial:Material | null = {name: "Select a material", id: -1};

    ngOnInit(): void {
        this.service.getMaterials();
    }

    filterByMaterial(material: Material) {
        if (material == this.selectedMaterial) {
            this.selectedMaterial = {name: "Select a material", id: -1};
            this.boxService.selectedMaterial = null;
            this.boxService.filterBoxes();
        } else {
            this.selectedMaterial = material;
            this.boxService.selectedMaterial = material;
            this.boxService.filterBoxes();
        }
    }
}
