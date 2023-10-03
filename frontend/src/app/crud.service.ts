import {inject, Injectable} from '@angular/core';
import {Box} from "./box";
import {firstValueFrom} from "rxjs";
import {HttpClient} from "@angular/common/http";
import {FormGroup} from "@angular/forms";

@Injectable({
  providedIn: 'root'
})
export class CrudService {
  private readonly http = inject(HttpClient)

  public boxes: Box[] = [];

  constructor() {
    this.getBoxes();
  }

  async createBox(formGroup: FormGroup) {
    const call = this.http.post<Box>("http://localhost:5000/api/boxes", formGroup.value);
    const response = await firstValueFrom<Box>(call);
    this.boxes.push(response);
    return response;

  }

  async getBoxes() {
    const call = this.http.get<Box[]>("http://localhost:5000/api/boxes");
    this.boxes = await firstValueFrom<Box[]>(call);
  }
}
