import {inject, Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {firstValueFrom} from "rxjs";
import {Material} from "./material";

@Injectable({
  providedIn: 'root'
})
export class MaterialService {
  private readonly http = inject(HttpClient);
  public materials:Material[] = [];

  constructor() {
    this.getMaterials();
  }

  async getMaterials() {
    const call = this.http.get<Material[]>("http://localhost:5000/api/materials");
      this.materials = await firstValueFrom<Material[]>(call);
  }
}
