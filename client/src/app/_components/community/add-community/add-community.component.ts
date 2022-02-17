import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AddCommunity } from 'src/app/_models/community';
import { CommunityService } from 'src/app/_services/community.service';

@Component({
  selector: 'app-add-community',
  templateUrl: './add-community.component.html',
  styleUrls: ['./add-community.component.scss']
})
export class AddCommunityComponent implements OnInit {
  model:AddCommunity;

  mouseOveredAdd:boolean;
  isEditMode:boolean = false;
  communityName:string='';
  communityDesc:string='';



  constructor(private communityService:CommunityService,private router: Router) {
    this.model={
      name:'',
      description:''
    }
   }

  ngOnInit(): void {
  }

  create(){

    this.communityService.createCommunity(this.model).subscribe({
      next: () => this.reloadCurrentRoute(),
      error: (error) => console.log(error)
    });
  }

  cancelEditMode(event){
    event.stopPropagation();
    if (this.isEditMode)
    { 
      this.isEditMode=false;
    }
    
  }

  enterEditMode(){
    this.isEditMode=true;
  }

  reloadCurrentRoute() {
    const currentUrl = this.router.url;
    this.router.navigateByUrl('/', { skipLocationChange: true }).then(() => {
      this.router.navigate([currentUrl]);
    });
  }
}
