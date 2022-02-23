import { Component, Input, OnInit } from '@angular/core';
import { UserSummary } from 'src/app/_models/user';
import { UsersService } from 'src/app/_services/Users.service';

@Component({
  selector: 'app-display-user-summary',
  templateUrl: './display-user-summary.component.html',
  styleUrls: ['./display-user-summary.component.scss']
})
export class DisplayUserSummaryComponent implements OnInit {
  model: UserSummary;
  @Input() username:string;
  communitiesAsString:string;

  constructor(private usersService:UsersService) { }

  ngOnInit(): void {
    this.loadUserSummary()
  }

  loadUserSummary(): void {
    this.usersService.getUserSummary(this.username).subscribe(response => {
      this.model = response;      
      error: (error) => console.log(error)
    });
  }
}
