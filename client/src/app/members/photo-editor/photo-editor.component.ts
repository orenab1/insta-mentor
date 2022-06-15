import { Component, Input, OnInit } from '@angular/core';
import { FileUploader } from 'ng2-file-upload';
import { take } from 'rxjs/operators';
import { User } from 'src/app/_models/user';
import { AccountService } from 'src/app/_services/account.service';
import { UsersService } from 'src/app/_services/Users.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-photo-editor',
  templateUrl: './photo-editor.component.html',
  styleUrls: ['./photo-editor.component.scss']
})
export class PhotoEditorComponent implements OnInit {
  @Input() addPhotoUrlExtension: string;
  @Input() onUploadSuccess: (response: any) => void;
  @Input() onDeletePhoto: () => void;

  uploader: FileUploader;
  hasBaseDropzoneOver = false;
  baseUrl = environment.apiUrl;
  user: User;
  hasUploaded:boolean;



  constructor(private accountService: AccountService, private usersService: UsersService) {
    this.accountService.currentUser$.pipe(take(1)).subscribe(user => this.user = user);
  }

  ngOnInit(): void {
    this.initializeUploader();
    this.hasUploaded=false;
  }

  fileOverBase(e: any) {
    this.hasBaseDropzoneOver = e;
  }

  deletePhoto() {
    this.onDeletePhoto();
  }

  initializeUploader() {
    this.uploader = new FileUploader({
      url: this.baseUrl + this.addPhotoUrlExtension,// 'users/add-photo',      
      isHTML5: true,
      allowedFileType: ['image'],
      removeAfterUpload: true,
      autoUpload: true,
      maxFileSize: 10 * 1024 * 1024
    });

    this.uploader.onAfterAddingFile = (file) => {
      file.withCredentials = false;
    }

    this.uploader.onSuccessItem = (item, response, status, headers) => {
      if (response) {
        this.onUploadSuccess(response);
        this.hasUploaded=true;
      }
    }
  }
}
