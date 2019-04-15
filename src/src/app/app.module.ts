import { BrowserModule } from '@angular/platform-browser';
import { CommonModule } from "@angular/common";
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms'; 
import { HttpModule, XHRBackend } from '@angular/http';
import { AuthenticateXHRBackend } from './authenticate-xhr.backend';
import { routing } from './app.routing';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

/* App Root */
import { AppComponent } from './app.component';
import { HeaderComponent } from './header/header.component';
import { HomeComponent } from './home/home.component';

import { SettingsComponent } from './settings/settings.component';
import { CheckinComponent } from './checkin/checkin.component';
import { CheckoutComponent } from './checkout/checkout.component';
import { DashbComponent } from './dashb/dashb.component';

/* Account Imports */
import { AccountModule }  from './account/account.module';
/* Dashboard Imports */
import { DashboardModule }  from './dashboard/dashboard.module';

import { ConfigService } from './shared/utils/config.service';
import { TaskService } from './shared/services/task.service';
import {
  InputTextModule,
  DropdownModule,
  ButtonModule,
  FieldsetModule,
  CalendarModule,
  PaginatorModule,
  PickListModule,
  DialogModule,
  SharedModule,
} from 'primeng/primeng';
import { TableModule } from 'primeng/table';

@NgModule({
  declarations: [
    AppComponent,
    HeaderComponent,
    HomeComponent,      
    SettingsComponent,
    CheckinComponent,
    CheckoutComponent,
    DashbComponent
  ],
  imports: [
    CommonModule,
    AccountModule,
    BrowserAnimationsModule,
    DashboardModule,
    BrowserModule,
    FormsModule,
    HttpModule,
    routing,
    TableModule,
    InputTextModule,
    DropdownModule,
    ButtonModule,
    FieldsetModule,
    CalendarModule,
    PaginatorModule,
    PickListModule,
    DialogModule,
    SharedModule,
  ],
  providers: [ConfigService, TaskService, { 
    provide: XHRBackend, 
    useClass: AuthenticateXHRBackend
  }],
  bootstrap: [AppComponent]
})
export class AppModule { }
