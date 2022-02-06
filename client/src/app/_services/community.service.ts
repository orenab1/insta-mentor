import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs';
import { environment } from 'src/environments/environment';
import { CommunitySummary } from '../_models/Community';

@Injectable({
  providedIn: 'root'
})
export class CommunityService {

  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getCommunities(){
    return this.http.get<CommunitySummary[]>(this.baseUrl+'communities').pipe(
      map(communities => {
        return communities;
      })
    );
  }
}
