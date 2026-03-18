import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { PostService } from '../../core/services/post';
import { UserService } from '../../core/services/user';
@Component({
  selector: 'app-feed',
  standalone: false,
  templateUrl: './feed.html',
  styleUrl: './feed.css',
})
export class Feed implements OnInit {
  posts: any[] = [];
  newPostContent: string = '';
  currentUser: string | null = '';
  currentUserProfileImage: string | null = null;
  
  selectedFile: File | null = null;
  selectedFileType: 'image' | 'video' | null = null;
  mediaPreview: string | null = null;

  constructor(
    private postService: PostService, 
    private userService: UserService,
    private cdr: ChangeDetectorRef
  ) { }
  ngOnInit() {
    this.currentUser = localStorage.getItem('username');
    this.userService.getMyProfile().subscribe(data => {
      this.currentUserProfileImage = data.profileImage;
      this.cdr.detectChanges();
    });
    this.loadPosts();
  }
  loadPosts() {
    this.postService.getPosts().subscribe({
      next: (data: any) => {
        this.posts = data;
        this.cdr.detectChanges();
      },
      error: (err: any) => console.error('Error loading posts', err)
    });
  }
  onFileSelected(event: any) {
    const file = event.target.files[0];
    if (file) {
      this.selectedFile = file;
      this.selectedFileType = file.type.startsWith('image/') ? 'image' : 'video';
      
      const reader = new FileReader();
      reader.onload = () => {
        this.mediaPreview = reader.result as string;
      };
      reader.readAsDataURL(file);
    }
  }

  removeSelectedFile() {
    this.selectedFile = null;
    this.selectedFileType = null;
    this.mediaPreview = null;
  }

  createPost() {
    if (!this.newPostContent.trim() && !this.selectedFile) return;

    if (this.selectedFile) {
      this.postService.uploadMedia(this.selectedFile).subscribe({
        next: (res: any) => {
          this.executeCreatePost(res.url, this.selectedFileType!);
        },
        error: (err: any) => console.error('Error uploading media', err)
      });
    } else {
      this.executeCreatePost();
    }
  }

  private executeCreatePost(mediaUrl?: string, mediaType?: string) {
    this.postService.createPost(this.newPostContent, mediaUrl, mediaType).subscribe({
      next: (res: any) => {
        alert("Post submitted! It is now Pending and waiting for Admin approval.");
        this.newPostContent = '';
        this.removeSelectedFile();
        this.loadPosts();
      },
      error: (err: any) => console.error('Error creating post', err)
    });
  }

  toggleLike(post: any) {
    if (post.isLiked) {
      this.postService.removeLike(post.postId).subscribe(() => {
        post.isLiked = false;
        post.likeCount--;
        this.cdr.detectChanges();
      });
    } else {
      this.postService.likePost(post.postId).subscribe(() => {
        post.isLiked = true;
        post.likeCount++;
        this.cdr.detectChanges();
      });
    }
  }

  toggleComments(post: any) {
    post.showComments = !post.showComments;
    if (post.showComments && (!post.comments || post.comments.length === 0)) {
      this.loadComments(post);
    }
    this.cdr.detectChanges();
  }

  loadComments(post: any) {
    this.postService.getComments(post.postId).subscribe(comments => {
      post.comments = comments;
      this.cdr.detectChanges();
    });
  }

  addComment(post: any, commentContent: string) {
    if (!commentContent.trim()) return;
    this.postService.addComment(post.postId, commentContent).subscribe(newComment => {
      if (!post.comments) post.comments = [];
      post.comments.push(newComment);
      post.commentCount++;
      post.newCommentText = '';
      this.cdr.detectChanges();
    });
  }
}
