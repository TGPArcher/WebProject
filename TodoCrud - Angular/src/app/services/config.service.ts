import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ConfigService {

  private readonly apiRoot = 'https://localhost:44311/api/';

  constructor() { }

  getWebApiRoot(): string {
    return this.apiRoot;
  }
}
