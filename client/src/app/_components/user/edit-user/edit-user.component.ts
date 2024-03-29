import { Component, OnInit, ViewChild } from '@angular/core'
import { ActivatedRoute, Router } from '@angular/router'
import { take } from 'rxjs/operators'
import { Community } from 'src/app/_models/community'
import { Member } from 'src/app/_models/member'
import { Tag } from 'src/app/_models/tag'
import { User } from 'src/app/_models/user'
import { AccountService } from 'src/app/_services/account.service'
import { CommonService } from 'src/app/_services/common.service'
import { CommunityService } from 'src/app/_services/community.service'
import { UsersService } from 'src/app/_services/Users.service'

@Component({
  selector: 'app-edit-user',
  templateUrl: './edit-user.component.html',
  styleUrls: ['./edit-user.component.scss'],
})
export class EditUserComponent implements OnInit {
  member: Member
  user: User
  allTags: Tag[]
  allCommunities: Community[]
  errorMessage:string

  constructor(
    private usersService: UsersService,
    private route: ActivatedRoute,
    private accountService: AccountService,
    private commonService: CommonService,
    private communityService: CommunityService,
    private router: Router,
  ) {
    this.accountService.currentUser$
      .pipe(take(1))
      .subscribe((user) => (this.user = user))
  }

  ngOnInit(): void {
    this.loadMember()

    this.loadTags()
   // this.loadCommunities()
  }

  loadTags() {
    this.commonService.getTags().subscribe((response) => {
      this.allTags = response
      error: (error) => console.log(error)
    })
  }

  // loadCommunities() {
  //   this.communityService.getCommunitiesTags().subscribe((response) => {
  //     this.allCommunities = response
  //     error: (error) => console.log(error)
  //   })
  // }

  editMember() {
    this.errorMessage=''
    if (this.member.tags!=null && this.member.tags.length!=0) {
      for (let i=0;i<this.member.tags.length; i++){

        if (isNaN(this.member.tags[i].value)){
          this.member.tags[i].value=0;
        }
      }
    }

    this.usersService.updateUser(this.member).subscribe(() => {
      if (this.member.username.length===0){
        this.errorMessage='Username must contain at least 1 character'
        return;
      }

      this.accountService.setCurrentUser({username:this.member.username,token:this.user.token,photoUrl:this.member.photoUrl,id:this.member.id})
      this.router.navigateByUrl('users/' + this.user.username)
    },error=>{
      this.errorMessage=error.error
    })
  }

  loadMember() {
    this.usersService
      .getMember(this.route.snapshot.paramMap.get('username'))
      .subscribe((response) => {
        this.member = response
      })
      
  }

  onUploadPhotoSuccess = (response: any) => {
    if (response) {
      const photo = JSON.parse(response)
      this.member.photoUrl = photo.url
      this.member.photoId = photo.id
      this.user.photoUrl = photo.url
      this.accountService.setCurrentUser(this.user)
    }
  }

  onDeletePhoto() {
    this.usersService.deletePhoto()
    .subscribe(() => {
      if (this.member) {
        this.member.photoUrl = './assets/user.png'
        this.member.photoId = 0
      }
    })
  }
}
