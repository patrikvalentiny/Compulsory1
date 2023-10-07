import {Component, inject, OnInit} from '@angular/core';
import {MaterialService} from "../material.service";
import {Material} from "../material";
import {CrudService} from "../crud.service";

@Component({
  selector: 'app-materials-dropdown',
  templateUrl: './materials-dropdown.component.html',
  styleUrls: ['./materials-dropdown.component.css']
})
export class MaterialsDropdownComponent implements OnInit{
  public readonly service = inject(MaterialService);
  public readonly boxService = inject(CrudService);
  selectedMaterial = "All";

  ngOnInit(): void {
    this.service.getMaterials();
  }

  filterByMaterial(material: Material) {
    this.selectedMaterial = material.name;
    this.boxService.filterMaterial = material.name;
    this.boxService.filterBoxes();
  }
}