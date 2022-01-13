import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Member } from '../_models/member';
import { User } from '../_models/user';

@Injectable({
  providedIn: 'root'
})
export class MembersService {
  baseUrl = environment.apiUrl;
  constructor(private http: HttpClient) { }

  getMembers() {
    return this.http.get<Member[]>(this.baseUrl + 'users');
  }

  getMember(username:string) {
    return this.http.get<Member>(this.baseUrl + 'users/'+username);
  }

  deletePhoto() {
    return this.http.delete(this.baseUrl + 'users/delete-photo');
  }

  updateUser(model: Member) {
    return this.http.put(this.baseUrl + 'users/update-user', model).pipe(
      map(() => {
        
      })
    );
  }
}
