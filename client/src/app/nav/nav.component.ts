import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { AccountService } from '../_services/account.service';
import { User } from '../_models/user';
import { Router } from '@angular/router';
import { take } from 'rxjs/operators';
import { JsonpClientBackend } from '@angular/common/http';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.scss']
})
export class NavComponent implements OnInit {
  user: any={};
  navbarCollapsed:boolean;

  constructor(public accountService: AccountService, private router: Router) {
    this.user={
      username:'',      
    }    
    this.accountService.currentUser$.pipe(take(1))
      .subscribe(user => this.user = user);
  }

  ngOnInit(): void {
    
  }

  logout() {
    this.accountService.logout();
    this.router.navigateByUrl('/questions');
  }
}
