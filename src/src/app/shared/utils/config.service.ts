import { Injectable } from '@angular/core';
 
@Injectable()
export class ConfigService {
     
    _apiURI : string;
 
    constructor() {
      //this._apiURI = 'http://localhost:5000/api';
      this._apiURI = 'http://192.168.9.32/api';
     }
 
     getApiURI() {
         return this._apiURI;
     }    
}
