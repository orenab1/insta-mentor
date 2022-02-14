import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { take } from 'rxjs/operators';
import { Member } from 'src/app/_models/member';
import { Tag } from 'src/app/_models/tag';
import { User } from 'src/app/_models/user';
import { AccountService } from 'src/app/_services/account.service';
import { CommonService } from 'src/app/_services/common.service';
import { MembersService } from 'src/app/_services/members.service';

@Component({
  selector: 'app-member-detail',
  templateUrl: './display-user.component.html',
  styleUrls: ['./display-user.component.scss']
})
export class DisplayUserComponent implements OnInit {
  member: Member;
  user: User;
  allTags: Tag[];
  communitiesAsString:string;


  constructor(private membersService: MembersService, private route: ActivatedRoute,
    private accountService: AccountService,
    private commonService: CommonService,
    private router: Router) {
    this.accountService.currentUser$.pipe(take(1)).subscribe(user => this.user = user);
  }

  ngOnInit(): void {
    this.loadMember();

    this.loadTags();
  }

  loadTags() {
    this.commonService.getTags().subscribe(response => {
      this.allTags = response;
      error: (error) => console.log(error)
    });
  }






  loadMember() {
    this.membersService.getMember(this.route.snapshot.paramMap.get('username')).subscribe(response => {
      this.member = response;
      if (!this.member.aboutMe){
        this.member.aboutMe='Nothing to tell yet'
      }

      this.communitiesAsString=this.member.communities.map((item)=>item.display).join(', ')
    });
  }


  

  deletePhoto() {
    this.membersService.deletePhoto().subscribe(() => {
      this.member.photoUrl = './assets/user.png';
      this.member.photoId = 0;
    })
  }

  edit() {
    this.router.navigateByUrl('edit-user/' + this.route.snapshot.paramMap.get('username'));
  }
}

