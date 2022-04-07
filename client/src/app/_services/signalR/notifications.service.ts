import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { BehaviorSubject } from 'rxjs';
import { take } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { User } from '../../_models/user';

@Injectable({
  providedIn: 'root'
})
export class NotificationsService {
  hubUrl = environment.hubUrl;
  private hubConnection: HubConnection;


  constructor() { }

  createHubConnection(user: User) {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(this.hubUrl + 'notify', {
        accessTokenFactory: () => user.token
      })
      .withAutomaticReconnect()
      .build()

    this.hubConnection
      .start()
      .catch(error => console.log(error));    
    

    // this.hubConnection.on('NewOfferReceived', ({questionId}) => {
    //   alert('offer!34343');
    //  });

     this.hubConnection.on("BroadcastMessage", () => {  
    });  



    //  this.hubConnection.on('UserIsOnline', username => {
    //   alert(username+' connected');
    // })

    // this.hubConnection.on('UserIsOffline', username => {
    // })

    // this.hubConnection.on('GetOnlineUsers', (usernames: string[]) => {
    //   alert(usernames+' connected');
    // })
    // }








    
    //  stopHubConnection() {
    //   this.hubConnection.stop().catch(error => console.log(error));
    // }
}
}