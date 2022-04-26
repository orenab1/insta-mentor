import { Component, Input, OnInit, ViewEncapsulation } from '@angular/core'
import { Router } from '@angular/router'
import { UserSummary } from 'src/app/_models/user'
import { PresenceService } from 'src/app/_services/signalR/presence.service'
import { UsersService } from 'src/app/_services/Users.service'

@Component({
  selector: 'app-display-user-summary',
  templateUrl: './display-user-summary.component.html',
  styleUrls: ['./display-user-summary.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class DisplayUserSummaryComponent implements OnInit {
  model: UserSummary
  @Input() username: string
  @Input() userId: number
  communitiesAsString: string
  isUserOnline: boolean = false
  @Input() showRating:false
  @Input() widthInPx:number=500;

  constructor(
    private usersService: UsersService,
    private presenceService: PresenceService,
    private router: Router,
  ) {}

  ngOnInit(): void {
    this.loadUserSummary()

    this.presenceService.onlineUsersTimesSource$.subscribe((userConnectedDurations) => {
      this.isUserOnline = userConnectedDurations.map(u=>u.username).indexOf(this.username) > -1
    })

  
  }

  loadUserSummary(): void {
    if (this.username) {
      this.usersService.getUserSummary(this.username).subscribe((response) => {
        this.model = response
        error: (error) => console.log(error)
      })
    } else {
      this.usersService
        .getUserSummaryById(this.userId)
        .subscribe((response) => {
          this.model = response
          error: (error) => console.log(error)
        })
    }
  }

  clicked() {
    this.router.navigateByUrl('users/' + this.username)
  }
}
