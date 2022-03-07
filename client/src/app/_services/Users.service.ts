import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Member } from '../_models/member';
import { User, UserSummary } from '../_models/user';

@Injectable({
  providedIn: 'root'
})
export class UsersService {
  baseUrl = environment.apiUrl;
  constructor(private http: HttpClient) { }

  getMember(username:string) {
    return this.http.get<Member>(this.baseUrl + 'users/get-user/'+username);
  }

  getUserSummary(username:string) {
    return this.http.get<UserSummary>(this.baseUrl + 'users/get-user-summary/'+username);
  }

  getUserSummaryById(id:number) {
    return this.http.get<UserSummary>(this.baseUrl + 'users/get-user-summary-by-id/'+id);
  }

  deletePhoto() {
    return this.http.delete(this.baseUrl + 'users/delete-photo').pipe(
      map(() => {        
      })
    );
  }

  updateUser(member: Member) {
    return this.http.put(this.baseUrl + 'users/update-user', member);
  }
}
