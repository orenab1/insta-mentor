import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { take } from 'rxjs/operators';
import { Member } from 'src/app/_models/member';
import { Tag } from 'src/app/_models/tag';
import { User } from 'src/app/_models/user';
import { AccountService } from 'src/app/_services/account.service';
import { CommonService } from 'src/app/_services/common.service';
import { MembersService } from 'src/app/_services/members.service';

@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.scss']
})
export class MemberEditComponent implements OnInit {
  member: Member;
  user: User;
  allTags: Tag[];


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


  editMember() {
    this.membersService.updateUser(this.member).subscribe(() => {
      this.router.navigateByUrl('members/' + this.user.username)
    });
  }




  loadMember() {
    this.membersService.getMember(this.route.snapshot.paramMap.get('username')).subscribe(response => {
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
      console.log('member: ' + JSON.stringify(this.member));
      console.log(response);
      this.member.photoUrl = photo.url;
      this.member.photoId = photo.id;
      this.user.photoUrl = photo.url;
      this.accountService.setCurrentUser(this.user);
    }


  }

  deletePhoto() {
    this.membersService.deletePhoto().subscribe(() => {
      this.member.photoUrl = './assets/user.png';
      this.member.photoId = 0;
    })
  }
}

