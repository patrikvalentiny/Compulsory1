import {inject, Injectable} from '@angular/core';
import {Box} from "./box";
import {firstValueFrom} from "rxjs";
import {HttpClient} from "@angular/common/http";
import {FormGroup} from "@angular/forms";
import {BoxOverviewItem} from "./box-overview-item";
import {Material} from "./material";
import {BoxWithMaterialId} from "./box-with-material-id";

@Injectable({
    providedIn: 'root'
})
export class CrudService {
    public boxes: BoxOverviewItem[] = [];
    public filteredBoxes: BoxOverviewItem[] = [];
    public materials: Material[] = [];
    searchTerm: string | null = "";
    selectedMaterial: Material | null = null;
    private readonly http = inject(HttpClient)

    constructor() {
        this.getBoxes();
    }

    async createBox(formGroup: FormGroup) {
        const call = this.http.post<BoxWithMaterialId>("http://localhost:5000/api/boxes", {guid: formGroup.value.guid, title: formGroup.value.title, width: formGroup.value.width, height: formGroup.value.height, depth: formGroup.value.depth, quantity: formGroup.value.quantity, location: formGroup.value.location, description: formGroup.value.description, materialId: this.selectedMaterial?.id});
        const response = await firstValueFrom<BoxWithMaterialId>(call);
        var boxoverviewitem = {guid: response.guid, title: response.title, width: response.width, height: response.height, depth: response.depth, quantity: response.quantity, materialName: this.materials.find(m =>m.id === this.selectedMaterial?.id)?.name ?? ""};
        this.boxes.push(boxoverviewitem);
        return response;

    }

    async getBoxes() {
        const call = this.http.get<BoxOverviewItem[]>("http://localhost:5000/api/boxes/feed");
        this.boxes = await firstValueFrom<BoxOverviewItem[]>(call);
        this.filteredBoxes = this.boxes;
    }

    async deleteBox(guid: string) {
        try {
            const call = this.http.delete(`http://localhost:5000/api/boxes/${guid}`);
            await firstValueFrom(call);
            this.filteredBoxes = this.filteredBoxes.filter(b => b.guid != guid);
        } catch (e) {

        }
    }

    async updateBox(formGroup: FormGroup, boxGuid: string) {
        try {
            const call = this.http.put(`http://localhost:5000/api/boxes`, {guid: boxGuid, title: formGroup.value.title, width: formGroup.value.width, height: formGroup.value.height, depth: formGroup.value.depth, quantity: formGroup.value.quantity, location: formGroup.value.location, description: formGroup.value.description, materialId: this.selectedMaterial?.id});
            await firstValueFrom(call);
        } catch (e) {
            throw e;
        }
    }

    async getBox(boxGuid: string) {
        try {
            const call = this.http.get<Box>(`http://localhost:5000/api/boxes/${boxGuid}`);
            return await firstValueFrom<Box>(call);
        } catch (e) {
            console.log(e);
            return undefined;
        }
    }

    filterBoxes() {
        this.filteredBoxes = this.boxes.filter(b => b.materialName === this.selectedMaterial?.name || this.selectedMaterial === null);
        if (this.searchTerm != null && this.searchTerm != "") {
            this.filteredBoxes = this.filteredBoxes.filter(b => b.title.toLowerCase().includes(this.searchTerm!.toLowerCase()));
        }
    }

    async getMaterials() {
        const call = this.http.get<Material[]>("http://localhost:5000/api/materials");
        this.materials = await firstValueFrom<Material[]>(call);
    }
}
