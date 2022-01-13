import { HttpClient, JsonpClientBackend } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ReplaySubject } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Tag } from '../_models/tag';
import { User } from '../_models/user';

@Injectable({
  providedIn: 'root'
})
export class CommonService {
  baseUrl = environment.apiUrl;
  private currentUserSource = new ReplaySubject<User>(1);
  currentUser$ = this.currentUserSource.asObservable();

  constructor(private http: HttpClient) { }


  getTags() {
    return this.http.get<Tag[]>(this.baseUrl + 'common/get-tags').pipe(
      map(tags => {
        return tags;
      })
    );
  }
}