import { Injectable } from '@angular/core'
import { Router } from '@angular/router'
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr'
import { ToastrService } from 'ngx-toastr'
import { BehaviorSubject } from 'rxjs'
import { take } from 'rxjs/operators'
import { UserConnectedDuration } from 'src/app/_models/userConnectedDuration'
import { environment } from 'src/environments/environment'
import { json } from 'stream/consumers'
import { User } from '../../_models/user'

@Injectable({
  providedIn: 'root',
})
export class PresenceService {
  hubUrl = environment.hubUrl
  private hubConnection: HubConnection


  private onlineUsersSource = new BehaviorSubject<string[]>([])
  onlineUsers$ = this.onlineUsersSource.asObservable()

  private onlineUsersTimesSource = new BehaviorSubject<UserConnectedDuration[]>([])
  onlineUsersTimesSource$ = this.onlineUsersTimesSource.asObservable()

  constructor(private router: Router, private toastr: ToastrService) {}
  

  createHubConnection(user: User) {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(this.hubUrl + 'presence', {
        accessTokenFactory: () => user.token
      })
      .withAutomaticReconnect()
      .build()

    this.hubConnection.start().catch((error) => console.log(error))

    // this.hubConnection.on('UserIsOnline', (username) => {

    //   this.onlineUsers$.pipe(take(1)).subscribe((usernames) => {
    //     this.onlineUsersSource.next([...usernames, username])
    //   })
    // })

    // this.hubConnection.on('UserIsOffline', (username) => {
    //   this.onlineUsers$.pipe(take(1)).subscribe((usernames) => {
    //     this.onlineUsersSource.next([
    //       ...usernames.filter((x) => x !== username),
    //     ])
    //   })
    // })

    // this.hubConnection.on('GetOnlineUsers', (usernames: string[]) => {
    //   this.onlineUsersSource.next(usernames)
    // })

    this.hubConnection.on('GetOnlineUsersWithTimes', (users: UserConnectedDuration[]) => {
      this.onlineUsersTimesSource.next(users)
    })

    this.hubConnection.on('NewOfferReceived', (questionId) => {
      this.toastr.info(
        `<div>Someone just offered to help you with your question!</div><br/><a href="${environment.baseUrl}display-question/${questionId}" target="_blank">Click here to view it</a>`,
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

    this.hubConnection.on('NewCommentReceived', (questionId) => {
      this.toastr.info(
        `<div>Your question just received a new comment!</div><br/><a href="${environment.baseUrl}display-question/${questionId}" target="_blank">Click here to read it</a>`,
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

    this.hubConnection.on('OffererLoggedIn', (questionId) => {
      this.toastr.info(
        `<div>Someone who offered to help you just logged in!</div><br/><a href="${environment.baseUrl}display-question/${questionId}" target="_blank">Click here to view the question they offered to help with</a>`,
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
    })

    this.hubConnection.on(
      'QuestionIsAskedAccordingToCommunityAndTags',
      (question) => {
      },
    )

    this.hubConnection.on('QuestionIsAskedAccordingToTags', (question) => {
    })
  }

  stopHubConnection() {
    this.hubConnection.stop().catch((error) => console.log(error))
  }
}
