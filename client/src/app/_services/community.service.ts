import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs';
import { environment } from 'src/environments/environment';
import { AddCommunity, CommunityFull } from '../_models/Community';

@Injectable({
  providedIn: 'root'
})
export class CommunityService {

  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getCommunities(){
    return this.http.get<CommunityFull[]>(this.baseUrl+'communities/all-communities').pipe(
      map(communities => {
        return communities;
      })
    );
  }

  createCommunity(addCommunity:AddCommunity){
    return this.http.post(this.baseUrl+'communities/create',addCommunity);         
  }

  inviteToCommunity(communityId:number){
    return this.http.put(this.baseUrl+'communities/invite?communityId='+communityId,null);         
  }

  joinCommunity(communityId:number){
    return this.http.put(this.baseUrl+'communities/join?communityId='+communityId,null);         
  }

  deleteCommunity(communityId:number){
    return this.http.delete(this.baseUrl+'communities/delete?communityId='+communityId);         
  }
}
