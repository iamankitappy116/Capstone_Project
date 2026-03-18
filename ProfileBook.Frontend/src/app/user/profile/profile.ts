import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { PostService } from '../../core/services/post';
import { UserService } from '../../core/services/user';

@Component({
  selector: 'app-profile',
  standalone: false,
  templateUrl: './profile.html',
  styleUrl: './profile.css',
})
export class Profile implements OnInit{
  userProfile: any = null;
  userPosts: any[] = [];
  currentUserId: number = 0;

  isEditing: boolean = false;
  isUploading: boolean = false;
  editProfile: any = {};

  constructor(
    private userService: UserService, 
    private postService: PostService,
    private cdr: ChangeDetectorRef
  ) {}
  
  ngOnInit(): void {
    this.loadMyProfile();
  }

  loadMyProfile(): void {
    this.userService.getMyProfile().subscribe({
      next: (data: any) => {
        console.log('Profile loaded:', data);
        this.userProfile = data;
        this.currentUserId = data.userId; 
        this.editProfile = { ...data }; 
        this.loadUserPosts();
        this.cdr.detectChanges(); 
      },
      error: (error: any) => {
        console.error('Error loading my profile:', error);
      }
    });
  }

  toggleEditMode(): void {
    this.isEditing = !this.isEditing;
    if (!this.isEditing) {
      this.editProfile = { ...this.userProfile };
    }
  }

  getProfileImageUrl(): string {
    const imageUrl = this.isEditing && this.editProfile.profileImage 
      ? this.editProfile.profileImage 
      : this.userProfile?.profileImage;

    if (imageUrl) {
      return `http://localhost:5134${imageUrl}`;
    }

    return `https://ui-avatars.com/api/?name=${this.userProfile?.username}&background=random&size=128`;
  }

  onPhotoSelected(event: any): void {
    const file = event.target.files[0];
    if (file) {
      this.isUploading = true;
      this.postService.uploadMedia(file).subscribe({
        next: (response: any) => {
          this.editProfile.profileImage = response.url;
          this.isUploading = false;
          this.cdr.detectChanges(); 
        },
        error: (err: any) => {
          console.error('Failed to upload profile photo', err);
          this.isUploading = false;
          alert('Failed to upload photo. Please try again.');
        }
      });
    }
  }

  saveProfile(): void {
    const updateData = {
      username: this.editProfile.username,
      email: this.editProfile.email,
      bio: this.editProfile.bio,
      location: this.editProfile.location,
      profileImage: this.editProfile.profileImage
    };

    this.userService.updateUser(this.currentUserId, updateData).subscribe({
      next: (data: any) => {
        this.userProfile = data;
        this.editProfile = { ...data };
        this.isEditing = false;
        this.cdr.detectChanges();
      },
      error: (err: any) => console.error('Error updating profile:', err)
    });
  }
  loadUserPosts(): void {
    if (!this.currentUserId) return;
    
    this.postService.getPostsByUser(this.currentUserId).subscribe({
      next: (data: any) => {
        this.userPosts = data;
        this.cdr.detectChanges(); 
      },
      error: (error: any) => {
        console.error('Error loading user posts:', error);
      }
    });
  }
}
