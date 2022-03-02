import { Injectable } from '@angular/core'
import { Router } from '@angular/router'
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr'
import { ToastrService } from 'ngx-toastr'
import { BehaviorSubject } from 'rxjs'
import { take } from 'rxjs/operators'
import { environment } from 'src/environments/environment'
import { User } from '../../_models/user'

@Injectable({
  providedIn: 'root',
})
export class PresenceService {
  hubUrl = environment.hubUrl
  private hubConnection: HubConnection
  private hubConnection2: HubConnection
  private onlineUsersSource = new BehaviorSubject<string[]>([])
  onlineUsers$ = this.onlineUsersSource.asObservable()

  constructor(private router: Router, private toastr: ToastrService) {}

  createHubConnection(user: User) {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(this.hubUrl + 'presence', {
        accessTokenFactory: () => user.token,
      })
      .withAutomaticReconnect()
      .build()

    this.hubConnection.start().catch((error) => console.log(error))

    this.hubConnection.on('UserIsOnline', (username) => {
      alert(username + ' connected')
      this.onlineUsers$.pipe(take(1)).subscribe((usernames) => {
        this.onlineUsersSource.next([...usernames, username])
      })
    })

    this.hubConnection.on('UserIsOffline', (username) => {
      this.onlineUsers$.pipe(take(1)).subscribe((usernames) => {
        this.onlineUsersSource.next([
          ...usernames.filter((x) => x !== username),
        ])
      })
    })

    this.hubConnection.on('GetOnlineUsers', (usernames: string[]) => {
      this.onlineUsersSource.next(usernames)
    })

    this.hubConnection.on('NewOfferReceived', (questionid) => {
      alert('new offer! ' + questionid)
    })

    this.hubConnection.on('NewCommentReceived', (questionid) => {
      alert('new comment! ' + questionid)
    })

    this.hubConnection.on('OffererLoggedIn', (questionid) => {
      alert('')
    })

    this.hubConnection.on('AskerAcceptedOffer', (askerAcceptedOfferDto) => {
      this.toastr.info(
        `<div><b>${askerAcceptedOfferDto.askerUsername}</b> wants your help in answering their question:</div><br/><div><b>${askerAcceptedOfferDto.questionHeader}</b></div><br/> <a href="${askerAcceptedOfferDto.meetingUrl}" target="_blank">Start a Zoom session with ${askerAcceptedOfferDto.askerUsername}</a><br/><br/> <a href="${environment.baseUrl}display-question/${askerAcceptedOfferDto.questionId}" target="_blank">See the question</a>`,
        '',
        {
          timeOut: 600000,
          enableHtml: true,
          progressBar: true,
          progressAnimation: 'increasing',
          tapToDismiss:false,
          closeButton:true,
          disableTimeOut:'extendedTimeOut'
        },
      )
    })

    this.hubConnection.on('OffererCancelsOffer', () => {
      alert('OffererCancelsOffer   ')
    })

    this.hubConnection.on(
      'QuestionIsAskedAccordingToCommunityAndTags',
      (question) => {
        alert('')
      },
    )

    this.hubConnection.on('QuestionIsAskedAccordingToTags', (question) => {
      alert('')
    })
  }

  stopHubConnection() {
    this.hubConnection.stop().catch((error) => console.log(error))
  }
}
