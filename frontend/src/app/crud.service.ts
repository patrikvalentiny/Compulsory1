import {inject, Injectable} from '@angular/core';
import {Box} from "./box";
import {firstValueFrom} from "rxjs";
import {HttpClient} from "@angular/common/http";
import {FormGroup} from "@angular/forms";
import {BoxOverviewItem} from "./box-overview-item";

@Injectable({
  providedIn: 'root'
})
export class CrudService {
  private readonly http = inject(HttpClient)

  public boxes: BoxOverviewItem[] = [];
  public filteredBoxes: BoxOverviewItem[] = [];

  constructor() {
    this.getBoxes();
  }

  async createBox(formGroup: FormGroup) {
    const call = this.http.post<Box>("http://localhost:5000/api/boxes", formGroup.value);
    const response = await firstValueFrom<Box>(call);
    var boxoverviewitem = {guid : response.guid, title: response.title, width: response.width, height: response.height, depth: response.depth, quantity: response.quantity, materialName: response.material.name};
    this.boxes.push(boxoverviewitem);
    return response;

  }

  async getBoxes() {
    const call = this.http.get<BoxOverviewItem[]>("http://localhost:5000/api/boxes/feed");
    this.boxes = await firstValueFrom<BoxOverviewItem[]>(call);
    this.filteredBoxes = this.boxes;
  }

  async deleteBox(guid: string) {
    try{
      const call = this.http.delete(`http://localhost:5000/api/boxes/${guid}`);
      await firstValueFrom(call);
      this.boxes = this.boxes.filter(b => b.guid != guid);
    } catch (e){

    }
  }

  async updateBox(formGroup: FormGroup, boxGuid: string) {
    try{
      const call = this.http.put(`http://localhost:5000/api/boxes/${boxGuid}`, formGroup.value);
      await firstValueFrom(call);
    } catch (e){
        console.log(e)
    }
  }

  async getBox(boxGuid: string) {
    try{
      const call = this.http.get<Box>(`http://localhost:5000/api/boxes/${boxGuid}`);
      return await firstValueFrom<Box>(call);
    } catch (e){
      console.log(e);
      return undefined;
    }
  }
}
