import { Component, OnInit } from '@angular/core';
import { Task } from '../shared/models/task.interface';
import { TaskService } from '../shared/services/task.service';

@Component({
  templateUrl: './checkin.component.html',
  //styleUrls: ['./checkin.component.scss']
})
export class CheckinComponent implements OnInit {
  tasks: Task[];
  today: Date;

  selectedTask: Task;
  newTask: boolean;
  task: Task;

  displayDialog: boolean;
  cols: any[];

  constructor(private taskService: TaskService) {
    this.today = new Date();
  }

  ngOnInit() {
    this.cols = [
      { field: 'cTaskId', header: '#' },
      { field: 'descr', header: 'Task' },
    ];

    this.taskService.getTasks(true).subscribe(res => this.tasks = res);
  }

  showDialogToAdd() {
    this.newTask = true;
    this.displayDialog = true;
    this.task = {} as Task;
  }

  save() {
    let tasks = [...this.tasks];
    let newItem = this.task;
    if (this.newTask) {
      this.taskService.updateTask(this.task).subscribe(res => {
        newItem.cTaskId = res.id;
        tasks.push(newItem);
      });
    }
    else {
      tasks[this.tasks.indexOf(this.selectedTask)] = this.task;
      this.taskService.updateTask(this.task).subscribe();
    }

    this.tasks = tasks;
    this.task = null;
    this.displayDialog = false;
  }

  delete() {
    if (this.selectedTask == null)
    {
      this.task = null;
      this.displayDialog = false;
      return;
    }
    this.taskService.deleteTask(this.selectedTask.cTaskId).subscribe(res => {
      let index = this.tasks.indexOf(this.selectedTask);
      this.tasks = this.tasks.filter((val, i) => i != index);
      this.task = null;
      this.displayDialog = false;
    });
  }

  onRowSelect(event) {
    this.newTask = false;
    this.task = this.cloneSetting(event.data);
    this.displayDialog = true;
  }

  cloneSetting(c: Task): Task {
    let task = {} as Task;
    for (let prop in c) {
      task[prop] = c[prop];
    }
    return task;
  }
}
