import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { CommunityFull } from 'src/app/_models/Community';
import { CommunityService } from 'src/app/_services/community.service';
import {NgbModal, ModalDismissReasons} from '@ng-bootstrap/ng-bootstrap';
import { Router } from '@angular/router';

@Component({
  selector: 'app-display-community',
  templateUrl: './display-community.component.html',
  styleUrls: ['./display-community.component.scss']
})
export class DisplayCommunityComponent implements OnInit {
  @Input() community: CommunityFull;
  canUserDeleteCommunity:boolean=false;

  @ViewChild('content', { static: false }) private content;

  constructor(private communityService: CommunityService,private modalService: NgbModal,private router: Router) { }

  ngOnInit(): void {
    this.canUserDeleteCommunity=
    this.community.isCurrentUserCreator && 
    this.community.isCurrentUserMember &&
    this.community.numOfMembers<5;
  }

  invite(){
    this.communityService.inviteToCommunity(this.community.id).subscribe({
      next: () => this.modalService.open(this.content),
      error: (error) => console.log(error)
    });
  }

  delete(){
    this.communityService.deleteCommunity(this.community.id).subscribe({
      next: () => this.reloadCurrentRoute(),
      error: (error) => console.log(error)
    });
  }

  join(){
    this.communityService.joinCommunity(this.community.id).subscribe({
      next: () => this.reloadCurrentRoute(),
      error: (error) => console.log(error)
    });
  }

  reloadCurrentRoute() {
    const currentUrl = this.router.url;
    this.router.navigateByUrl('/', { skipLocationChange: true }).then(() => {
      this.router.navigate([currentUrl]);
    });
  }


  open(content) {
    this.modalService.open(content, {ariaLabelledBy: 'modal-basic-title'}).result.then((result) => {
     
    });
  }
}
