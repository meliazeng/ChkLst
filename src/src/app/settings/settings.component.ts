import { Component, OnInit } from '@angular/core';
import { Setting } from '../shared/models/setting.interface';
import { Pattern } from '../shared/models/pattern.interface';
import { TaskService } from '../shared/services/task.service';
import {
  SelectItem,
} from 'primeng/primeng';

@Component({
  //selector: 'app-settings',
  templateUrl: './settings.component.html',
})
export class SettingsComponent implements OnInit {

  patterns: SelectItem[];
  settings: Setting[];

  selectedSetting: Setting;
  newSetting: boolean;
  setting: Setting;

  displayDialog: boolean;
  cols: any[];

  constructor(private taskService: TaskService) { }
  
  ngOnInit() {
    this.patterns = [];
    this.taskService.getPatterns().subscribe(res => {
      res.forEach(
        x => {
          this.patterns.push({ label: x.descr, value: x.repeatPatternId });
        }
      )

      this.taskService.getSettings().subscribe(res => {
        this.settings = res;
      });
    });

    this.cols = [
      { field: 'settingId', header: '#' },
      { field: 'descr', header: 'Task' },
      { field: 'endDate', header: 'Expired at' }
    ]; 
  }

  showDialogToAdd() {
    this.newSetting = true;
    this.displayDialog = true;
    this.setting = {} as Setting;
    this.setting.repeatPatternId = 1;
  }

  save() {
    let settings = [...this.settings];
    let newItem = this.setting;
    if (this.newSetting) {
      this.taskService.updateSetting(this.setting).subscribe(res => {
        newItem.settingId = res.id;
        newItem.repeatDescr = this.patterns.find(x => x.value == newItem.repeatPatternId).label;
        settings.push(newItem);
      });
    }
    else {
      this.setting.repeatDescr = this.patterns.find(x => x.value == this.setting.repeatPatternId).label;
      settings[this.settings.indexOf(this.selectedSetting)] = this.setting;
      this.taskService.updateSetting(this.setting).subscribe();
    }

    this.settings = settings;
    this.setting = null;
    this.displayDialog = false;
  }

  delete() {
    if (this.selectedSetting == null) {
      this.setting = null;
      this.displayDialog = false;
      return;
    }
    this.taskService.deleteSetting(this.selectedSetting.settingId).subscribe(res => {
      let index = this.settings.indexOf(this.selectedSetting);
      this.settings = this.settings.filter((val, i) => i != index);
      this.setting = null;
      this.displayDialog = false;
    });
  }

  onRowSelect(event) {
    this.newSetting = false;
    this.setting = this.cloneSetting(event.data);
    if (event.data.endDate != null)
      this.setting.endDate = new Date(event.data.endDate.toString());
    this.displayDialog = true;
  }

  cloneSetting(c: Setting): Setting {
    let setting = {} as Setting;
    for (let prop in c) {
      setting[prop] = c[prop];
    }
    return setting;
  }

}
