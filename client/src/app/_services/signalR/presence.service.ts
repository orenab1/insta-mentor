import { Injectable } from '@angular/core'
import { Router } from '@angular/router'
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr'
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

  constructor(private router: Router) {}

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

    this.hubConnection.on(
      'AskerWantsToStartASession',
      question => {
        alert('AskerWantsToStartASession   '+ JSON.stringify(question));
      },
    )

    this.hubConnection.on(
      'OffererCancelsOffer',
      () => {
        alert('OffererCancelsOffer   ');
      },
    )

    this.hubConnection.on(
      'QuestionIsAskedAccordingToCommunityAndTags', (question) => {
        alert('')
      })

    this.hubConnection.on(
      'QuestionIsAskedAccordingToTags', (question) => {
        alert('')
      })
  }

  stopHubConnection() {
    this.hubConnection.stop().catch((error) => console.log(error))
  }
}
