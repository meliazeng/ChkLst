import { ModuleWithProviders }  from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { HomeComponent } from './home/home.component';
import { SettingsComponent } from './settings/settings.component';
import { CheckinComponent } from './checkin/checkin.component';
import { CheckoutComponent } from './checkout/checkout.component';
import { DashbComponent } from './dashb/dashb.component';
import { AuthGuard } from './auth.guard';

const appRoutes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'settings', component: SettingsComponent, canActivate: [AuthGuard]},
  { path: 'checkin', component: CheckinComponent, canActivate: [AuthGuard]},
  { path: 'checkout', component: CheckoutComponent, canActivate: [AuthGuard] },
  { path: 'dashb', component: DashbComponent }
];

export const routing: ModuleWithProviders = RouterModule.forRoot(appRoutes);
