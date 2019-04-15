import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Task } from '../shared/models/task.interface';
import { TaskService } from '../shared/services/task.service';
import { Observable } from 'rxjs/Rx';

@Component({
  templateUrl: './dashb.component.html',
})
export class DashbComponent implements OnInit {

  mtasks: Task[];
  atasks: Task[];
  today: Date;

  //location: string = '';
  constructor(private taskService: TaskService, private router: Router) {
    this.today = new Date();
  }

  ngOnInit() {
//    this.location = this.router.url;
    Observable.interval(10000)
      .subscribe(x => {
        this.taskService.updateTasksGeneral(this.mtasks).subscribe(
          res => {
            this.taskService.updateTasksGeneral(this.atasks).subscribe(
              res => {
                this.taskService.getTasksForUser("9a210bc4-e992-41de-9e51-90bce40731f4").subscribe(res => {
                  this.mtasks = res;
                  this.taskService.getTasksForUser("d7ab4fd3-408b-45ff-be90-e952a022f6ae").subscribe(res => {
                    this.atasks = res;
                  });
                }
                );
              });
          });
      });
  }

  cancel() {
    this.router.navigate(['/']);
  }
}
