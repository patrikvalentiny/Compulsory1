import {Component, inject, OnInit} from '@angular/core';
import {MaterialService} from "../material.service";
import {Material} from "../material";

@Component({
  selector: 'app-materials-dropdown',
  templateUrl: './materials-dropdown.component.html',
  styleUrls: ['./materials-dropdown.component.css']
})
export class MaterialsDropdownComponent implements OnInit{
  public readonly service = inject(MaterialService);
  selectedMaterial = "All";

  ngOnInit(): void {
    this.service.getMaterials();
  }

  filterByMaterial(material: Material) {
    this.service.filterByMaterial(material);
    this.selectedMaterial = material.name;
  }
}
