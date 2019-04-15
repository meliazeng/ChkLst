import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Task } from '../shared/models/task.interface';
import { TaskService } from '../shared/services/task.service';

@Component({
  templateUrl: './checkout.component.html',
})
export class CheckoutComponent implements OnInit {
  tasks: Task[];
  today: Date;
//  selectedTasks: Task[];

  constructor(private taskService: TaskService, private router: Router) {
    this.today = new Date();
  }

  ngOnInit() {
//    this.selectedTasks = [];
    this.taskService.getTasks(false).subscribe(res => {
      this.tasks = res;
    });

  }

  cancel() {
    this.router.navigate(['/']);
  }

  save() {
    this.taskService.updateTasks(this.tasks).subscribe(
      res => {
        this.router.navigate(['/']);
      }
    );
  }
}
