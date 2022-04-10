import { Component, Input, OnInit, ViewEncapsulation } from '@angular/core';
import { PresenceService } from 'src/app/_services/signalR/presence.service';

@Component({
  selector: 'app-display-user-image',
  templateUrl: './display-user-image.component.html',
  styleUrls: ['./display-user-image.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class DisplayUserImageComponent implements OnInit {

  @Input() username: string
  @Input() userId: number
  @Input() photoUrl:string
  @Input() isSmall:boolean

  isUserOnline: boolean = false


  constructor(private presenceService: PresenceService) { }

  ngOnInit(): void {
    this.presenceService.onlineUsersTimesSource$.subscribe((usernamesTimes) => {
      this.isUserOnline =usernamesTimes.map(e=>e.username).indexOf(this.username)>-1;
    });
  }
}
