import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { PostService } from '../../core/services/post';
import { UserService } from '../../core/services/user';
import { FollowService } from '../../core/services/follow';
import { forkJoin, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
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

  // Sidebar
  suggestedUsers: any[] = [];
  trendingPosts = [
    { tag: '#WebDevelopment', count: '2.4k posts' },
    { tag: '#DesignInspiration', count: '1.8k posts' },
    { tag: '#TechNews', count: '3.2k posts' },
    { tag: '#Angular', count: '1.1k posts' },
    { tag: '#CSharp', count: '900 posts' },
  ];
  
  constructor(
    private postService: PostService, 
    private userService: UserService,
    private followService: FollowService,
    private cdr: ChangeDetectorRef
  ) { }
  ngOnInit() {
    this.currentUser = sessionStorage.getItem('username');
    this.userService.getMyProfile().subscribe(data => {
      this.currentUserProfileImage = data.profileImage;
      this.cdr.detectChanges();
    });
    this.loadPosts();
    this.loadSuggestedUsers();
  }

  loadSuggestedUsers(): void {
    this.userService.getAllUsers().subscribe({
      next: (users: any[]) => {
        const currentUserId = Number(sessionStorage.getItem('userId')) || 0;
        // Filter out current user and take a larger pool (e.g., up to 20)
        const potentialPool = users
          .filter(u => u.userId !== currentUserId)
          .slice(0, 20);

        if (potentialPool.length === 0) {
          this.suggestedUsers = [];
          this.cdr.detectChanges();
          return;
        }

        // Check follow status for the pool
        const statusChecks = potentialPool.map(user => 
          this.followService.isFollowing(user.userId).pipe(
            catchError(() => of({ isFollowing: false }))
          )
        );

        forkJoin(statusChecks).subscribe(results => {
          const suggestions = [];
          for (let i = 0; i < potentialPool.length; i++) {
            if (!results[i].isFollowing) {
              suggestions.push(potentialPool[i]);
            }
          }
          // Take only top 3 that are NOT followed
          this.suggestedUsers = suggestions.slice(0, 3);
          this.cdr.detectChanges();
        });
      },
      error: () => {} 
    });
  }

  toggleFollow(user: any): void {
    if (user.isFollowing) {
      this.followService.unfollow(user.userId).subscribe({
        next: () => {
          user.isFollowing = false;
          this.cdr.detectChanges();
        },
        error: (err) => console.error('Error unfollowing', err)
      });
    } else {
      this.followService.follow(user.userId).subscribe({
        next: () => {
          // Immediately remove from suggestions list after following
          this.suggestedUsers = this.suggestedUsers.filter(u => u.userId !== user.userId);
          
          // If we ran out of local suggestions, try fetching a fresh batch
          if (this.suggestedUsers.length === 0) {
            this.loadSuggestedUsers();
          } else {
            this.cdr.detectChanges();
          }
        },
        error: (err) => console.error('Error following', err)
      });
    }
  }

  loadPosts() {
    this.postService.getPosts().subscribe({
      next: (data: any) => {
        const posts = data;
        if (posts.length === 0) {
          this.posts = [];
          this.cdr.detectChanges();
          return;
        }
        const likeChecks: import('rxjs').Observable<{ isLiked: boolean }>[] = posts.map((post: any) =>
          this.postService.isLiked(post.postId).pipe(
            catchError(() => of({ isLiked: false }))
          )
        );
        forkJoin(likeChecks).subscribe({
          next: (results) => {
            results.forEach((res, i) => {
              posts[i].isLiked = res.isLiked;
            });
            this.posts = posts;
            this.cdr.detectChanges();
          },
          error: () => {
            // If batch like-check fails, just render posts without liked state
            this.posts = posts;
            this.cdr.detectChanges();
          }
        });
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
      this.postService.removeLike(post.postId).subscribe({
        next: () => {
          post.isLiked = false;
          post.likeCount--;
          this.cdr.detectChanges();
        },
        error: () => {
          // Like not found - sync state: mark as not liked
          post.isLiked = false;
          this.cdr.detectChanges();
        }
      });
    } else {
      this.postService.likePost(post.postId).subscribe({
        next: () => {
          post.isLiked = true;
          post.likeCount++;
          this.cdr.detectChanges();
        },
        error: () => {
          // Already liked in DB - sync state: mark as liked
          post.isLiked = true;
          this.cdr.detectChanges();
        }
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
