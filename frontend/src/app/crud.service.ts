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

  constructor() { }

  async createBox(formGroup: FormGroup) {
    const call = this.http.post<Box>("http://localhost:5000/api/boxes", formGroup.value);
    return await firstValueFrom<Box>(call);
  }
}
