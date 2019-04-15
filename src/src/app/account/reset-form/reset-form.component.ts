import { Subscription } from 'rxjs';
import { Component, OnInit,OnDestroy } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';

import { PasswordOnly} from '../../shared/models/credentials.interface';
import { UserService } from '../../shared/services/user.service';

@Component({
  selector: 'app-reset-form',
  templateUrl: './reset-form.component.html',
})

export class ResetFormComponent implements OnInit, OnDestroy {

  private subscription: Subscription;

  brandNew: boolean;
  errors: string;
  isRequesting: boolean;
  submitted: boolean = false;
  credentials: PasswordOnly = { password: '' };

  constructor(private userService: UserService, private router: Router,private activatedRoute: ActivatedRoute) { }

  ngOnInit() {
    // subscribe to router event
    this.subscription = this.activatedRoute.queryParams.subscribe(
      (param: any) => {
        this.brandNew = param['brandNew'];
        //this.credentials. = param['email'];
      });      

  }

   ngOnDestroy() {
    // prevent memory leak by unsubscribing
    this.subscription.unsubscribe();
  }

  login({ value, valid }: { value: PasswordOnly, valid: boolean }) {
    this.submitted = true;
    this.isRequesting = true;
    this.errors='';
    if (valid) {
      this.userService.reset('test', value.password)
        .finally(() => this.isRequesting = false)
        .subscribe(
        result => {         
          if (result) {
             this.router.navigate(['/']);             
          }
        },
        error => this.errors = error);
    }
  }
}
