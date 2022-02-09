import { Component, OnInit } from '@angular/core';
import { CommunityFull } from '../../../_models/Community';
import { CommunityService } from '../../../_services/community.service';

@Component({
  selector: 'app-communities',
  templateUrl: './communities.component.html',
  styleUrls: ['./communities.component.scss']
})
export class CommunitiesComponent implements OnInit {
  communities:CommunityFull[];

  constructor(private communityService: CommunityService) { 
  }

  ngOnInit(): void {
    this.loadCommunities();
  }

  loadCommunities(): void {
    this.communityService.getCommunities().subscribe(response => {
      this.communities = response;
    }, error => {
      console.log(error);
    });
  }
}
