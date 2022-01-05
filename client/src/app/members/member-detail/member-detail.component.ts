import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { take } from 'rxjs/operators';
import { Member } from 'src/app/_models/member';
import { User } from 'src/app/_models/user';
import { AccountService } from 'src/app/_services/account.service';
import { MembersService } from 'src/app/_services/members.service';

@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.scss']
})
export class MemberDetailComponent implements OnInit {
  member: Member;
  user: User;

  constructor(private membersService: MembersService, private route: ActivatedRoute,
    private accountService: AccountService) {
    this.accountService.currentUser$.pipe(take(1)).subscribe(user => this.user = user);
  }

  ngOnInit(): void {
    this.loadMember();
  }


  loadMember() {
    this.membersService.getMember(this.route.snapshot.paramMap.get('username')).subscribe(response => {
      this.member = response;
    });
  }

  onUploadPhotoSuccess(response: any) {
    if (response) {
      const photo = JSON.parse(response);
      this.member.photoUrl = photo.url;
      this.member.photoId = photo.id;
      this.user.photoUrl = photo.url;
      this.accountService.setCurrentUser(this.user);
    }

   
  }
  
  deletePhoto(){
    this.membersService.deletePhoto().subscribe(() => {
      this.member.photoUrl = './assets/user.png';
      this.member.photoId = 0;
    })
  }
}

