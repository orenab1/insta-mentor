import { HttpClient, JsonpClientBackend } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ReplaySubject } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { User } from '../_models/user';
import { PresenceService } from './signalR/presence.service';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  baseUrl = environment.apiUrl;
  private currentUserSource = new ReplaySubject<User>(1);
  currentUser$ = this.currentUserSource.asObservable();

  constructor(private http: HttpClient, private presenceService: PresenceService) { }

  login(model: any) {      
    return this.http.post(this.baseUrl + 'account/login', model)
      .pipe(
        map((user: User) => {
          if (user) {
            this.setCurrentUser(user);
            this.presenceService.createHubConnection(user);
            
          }
        })
      );
  }

  register(model: any) {
    return this.http.post(this.baseUrl + 'account/register', model).pipe(
      map((user: User) => {
        if (user) {
          this.setCurrentUser(user);
          this.presenceService.createHubConnection(user);
        }
        return user;
      })
    )
  }

  forgotPassword(model: any) {
    return this.http.post(this.baseUrl + 'account/forgot-password', {'email':model}).pipe(
      map((response: any) => {
       
        return response;
      })
    )
  }


  setCurrentUser(user: User) {
    localStorage.setItem('user', JSON.stringify(user));
    this.currentUserSource.next(user);
  }

  logout() {
    localStorage.removeItem('user');
    this.currentUserSource.next(null);
    this.presenceService.stopHubConnection();
  }
}
