import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { take } from 'rxjs/operators';
import { Community } from 'src/app/_models/community';
import { Member } from 'src/app/_models/member';
import { Tag } from 'src/app/_models/tag';
import { User } from 'src/app/_models/user';
import { AccountService } from 'src/app/_services/account.service';
import { CommonService } from 'src/app/_services/common.service';
import { CommunityService } from 'src/app/_services/community.service';
import { UsersService } from 'src/app/_services/Users.service';

@Component({
  selector: 'app-edit-user',
  templateUrl: './edit-user.component.html',
  styleUrls: ['./edit-user.component.scss']
})
export class EditUserComponent implements OnInit {
  member: Member;
  user: User;
  allTags: Tag[];
  allCommunities: Community[];


  constructor(private usersService: UsersService, private route: ActivatedRoute,
    private accountService: AccountService,
    private commonService: CommonService,
    private communityService: CommunityService,
    private router: Router) {
    this.accountService.currentUser$.pipe(take(1)).subscribe(user => this.user = user);
  }

  ngOnInit(): void {
    this.loadMember();

    this.loadTags();
    this.loadCommunities();
  }

  loadTags() {
    this.commonService.getTags().subscribe(response => {
      this.allTags = response;
      error: (error) => console.log(error)
    });
  }

  loadCommunities() {
    this.communityService.getCommunitiesTags().subscribe(response => {
      this.allCommunities = response;
      error: (error) => console.log(error)
    });
  }


  editMember() {
    this.usersService.updateUser(this.member).subscribe(() => {
      this.router.navigateByUrl('members/' + this.user.username)
    });
  }




  loadMember() {
    this.usersService.getMember(this.route.snapshot.paramMap.get('username')).subscribe(response => {
      this.member = response;
      this.member.tags = [{
        value: 7,
        display: 'Angular'
      }];
    });
  }

  onUploadPhotoSuccess = (response: any) => {
    if (response) {
      const photo = JSON.parse(response);
      this.member.photoUrl = photo.url;
      this.member.photoId = photo.id;
      this.user.photoUrl = photo.url;
      this.accountService.setCurrentUser(this.user);
    }


  }

  deletePhoto() {
    this.usersService.deletePhoto().subscribe(() => {
      this.member.photoUrl = './assets/user.png';
      this.member.photoId = 0;
    })
  }
}

