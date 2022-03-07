import { Component, Input, OnInit } from '@angular/core'
import { Router } from '@angular/router'
import { UserSummary } from 'src/app/_models/user'
import { UsersService } from 'src/app/_services/Users.service'

@Component({
  selector: 'app-display-user-summary',
  templateUrl: './display-user-summary.component.html',
  styleUrls: ['./display-user-summary.component.scss'],
})
export class DisplayUserSummaryComponent implements OnInit {
  model: UserSummary
  @Input() username: string
  @Input() userId: number
  communitiesAsString: string

  constructor(private usersService: UsersService,private router: Router) {}

  ngOnInit(): void {
    this.loadUserSummary()
  }

  loadUserSummary(): void {
    if (this.username) {
      this.usersService.getUserSummary(this.username).subscribe((response) => {
        this.model = response
        error: (error) => console.log(error)
      })
    } else {
      this.usersService.getUserSummaryById(this.userId).subscribe((response) => {
        this.model = response
        error: (error) => console.log(error)
      })
    }
  }

  clicked(){
    this.router.navigateByUrl('users/' + this.username);
  }
}
