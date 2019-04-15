import { Injectable } from '@angular/core';
import { Http, Response, Headers, RequestOptions } from '@angular/http';

import { Pattern } from '../models/pattern.interface';
import { Setting } from '../models/setting.interface';
import { Task } from '../models/task.interface';

import { ConfigService } from '../../shared/utils/config.service';

import {BaseService} from '../../shared/services/base.service';

import { Observable } from 'rxjs/Rx'; 

// Add the RxJS Observable operators we need in this app.
import '../../rxjs-operators';

@Injectable()

export class TaskService extends BaseService {

  baseUrl: string = ''; 

  constructor(private http: Http, private configService: ConfigService) {
     super();
     this.baseUrl = configService.getApiURI();
  }

  getPatterns(): Observable<Pattern[]> {
      let headers = new Headers();
      headers.append('Content-Type', 'application/json');
      let authToken = localStorage.getItem('auth_token');
      headers.append('Authorization', `Bearer ${authToken}`);
  
      return this.http.get(this.baseUrl + "/tasks/getpatterns",{headers})
      .map(response => response.json())
      .catch(this.handleError);
  }

  getSettings(): Observable<Setting[]> {
      let headers = new Headers();
      headers.append('Content-Type', 'application/json');
      let authToken = localStorage.getItem('auth_token');
      headers.append('Authorization', `Bearer ${authToken}`);

      return this.http.get(this.baseUrl + "/tasks/getsettings", { headers })
      .map(response => response.json())
      .catch(this.handleError);
  }

  getTasks(isCheckin: boolean): Observable<Task[]> {
    let headers = new Headers();
    headers.append('Content-Type', 'application/json');
    let authToken = localStorage.getItem('auth_token');
    headers.append('Authorization', `Bearer ${authToken}`);
    let options = new RequestOptions({ headers: headers, params: { isCheckin } });

    return this.http.get(this.baseUrl + "/tasks/gettasks", options)
      .map(response => response.json())
      .catch(this.handleError);
  }

  updateTask(model: Task): Observable<any> {
    let id: number = model.cTaskId;
    let descr: string = model.descr;
    let status: boolean = model.status
    let note: string = model.note;
    let body = JSON.stringify({ id, descr, status, note });
    let headers = new Headers({ 'Content-Type': 'application/json' });
    let authToken = localStorage.getItem('auth_token');
    headers.append('Authorization', `Bearer ${authToken}`);
    let options = new RequestOptions({ headers: headers });

    return this.http.post(this.baseUrl + "/tasks/updatetask", body, options)
      .map(res => res.json())
      .catch(this.handleError);
  }

  updateTasks(model: Task[]): Observable<any> {

    let body = JSON.stringify({ model }); //.replace('"model":', '');
    let headers = new Headers({ 'Content-Type': 'application/json' });
    let authToken = localStorage.getItem('auth_token');
    headers.append('Authorization', `Bearer ${authToken}`);
    let options = new RequestOptions({ headers: headers });

    return this.http.post(this.baseUrl + "/tasks/updatetasks", body, options)
      .map(res => res.json())
      .catch(this.handleError);
  }

  updateTasksGeneral(model: Task[]): Observable<any> {

    let body = JSON.stringify({ model }); //.replace('"model":', '');
    let headers = new Headers({ 'Content-Type': 'application/json' });
    let options = new RequestOptions({ headers: headers });

    return this.http.post(this.baseUrl + "/tasks/updatetasksGen", body, options)
      .map(res => res.json())
      .catch(this.handleError);
  }

  updateSetting(model: Setting): Observable<any> {
    let id: number = model.settingId;
    let descr: string = model.descr;
    let endDate: Date = model.endDate;
    let repeatPatternId: number = model.repeatPatternId;
    let body = JSON.stringify({ id, descr, endDate, repeatPatternId });
    let headers = new Headers({ 'Content-Type': 'application/json' });
    let authToken = localStorage.getItem('auth_token');
    headers.append('Authorization', `Bearer ${authToken}`);
    let options = new RequestOptions({ headers: headers });

    return this.http.post(this.baseUrl + "/tasks/updatesetting", body, options)
      .map(res => res.json())
      .catch(this.handleError);
  }

  deleteSetting(id: number) {
    let headers = new Headers({ 'Content-Type': 'application/json' });
    let authToken = localStorage.getItem('auth_token');
    headers.append('Authorization', `Bearer ${authToken}`);
    let options = new RequestOptions({ headers: headers, params: { id } });

    return this.http.delete(this.baseUrl + "/tasks/deletesetting", options)
      .map(res => true)
      .catch(this.handleError);
  }

  deleteTask(id: number) {
    let headers = new Headers({ 'Content-Type': 'application/json' });
    let authToken = localStorage.getItem('auth_token');
    headers.append('Authorization', `Bearer ${authToken}`);
    let options = new RequestOptions({ headers: headers, params: { id } });

    return this.http.delete(this.baseUrl + "/tasks/deletetask", options)
      .map(res => true)
      .catch(this.handleError);
  }

  getTasksForUser(userId: string): Observable<Task[]> {
    let headers = new Headers();
    headers.append('Content-Type', 'application/json');
    let authToken = localStorage.getItem('auth_token');
    headers.append('Authorization', `Bearer ${authToken}`);
    let options = new RequestOptions({ headers: headers, params: { userId } });

    return this.http.get(this.baseUrl + "/tasks/GetTasksForUser", options)
      .map(response => response.json())
      .catch(this.handleError);
  }

}
