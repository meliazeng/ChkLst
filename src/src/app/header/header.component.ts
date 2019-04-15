import { Component, OnInit,OnDestroy } from '@angular/core';
import { Subscription} from 'rxjs/Subscription';
import { Router } from '@angular/router';
import { Location } from '@angular/common';
import { UserService } from '../shared/services/user.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})

export class HeaderComponent implements OnInit,OnDestroy {


  status: boolean;
 subscription:Subscription;
 public url: string = '';

 constructor(private userService: UserService, private _router: Router, location: Location) {
   _router.events.subscribe((val) => {
     if (location.path() != '') {
       this.url = location.path();
     } else {
       this.url = 'Home'
     }
   });
   }

   logout() {
     this.userService.logout();       
  }

   ngOnInit() {
 
    this.subscription = this.userService.authNavStatus$.subscribe(status => this.status = status);
  }

   ngOnDestroy() {
    // prevent memory leak when component is destroyed
    this.subscription.unsubscribe();
  }
}
