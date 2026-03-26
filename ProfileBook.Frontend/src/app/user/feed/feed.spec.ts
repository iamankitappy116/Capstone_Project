import { ComponentFixture, TestBed } from '@angular/core/testing';
import { Feed } from './feed';
import { PostService } from '../../core/services/post';
import { UserService } from '../../core/services/user';
import { FollowService } from '../../core/services/follow';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { of } from 'rxjs';
import { ChangeDetectorRef, NO_ERRORS_SCHEMA } from '@angular/core';

describe('Feed Component', () => {
  let component: Feed;
  let fixture: ComponentFixture<Feed>;
  let postService: any;
  let userService: any;
  let followService: any;

  beforeEach(async () => {
    postService = {
      getPosts: vi.fn().mockReturnValue(of([])),
      createPost: vi.fn().mockReturnValue(of({})),
      likePost: vi.fn().mockReturnValue(of({})),
      removeLike: vi.fn().mockReturnValue(of({})),
      isLiked: vi.fn().mockReturnValue(of({ isLiked: false })),
      getComments: vi.fn().mockReturnValue(of([])),
      addComment: vi.fn().mockReturnValue(of({ content: 'test' })),
      uploadMedia: vi.fn().mockReturnValue(of({ url: '/uploads/test.jpg' }))
    };

    userService = {
      getMyProfile: vi.fn().mockReturnValue(of({ userId: 1, username: 'testuser', profileImage: null })),
      getAllUsers: vi.fn().mockReturnValue(of([]))
    };

    followService = {
      follow: vi.fn().mockReturnValue(of({})),
      unfollow: vi.fn().mockReturnValue(of({})),
      isFollowing: vi.fn().mockReturnValue(of({ isFollowing: false }))
    };

    await TestBed.configureTestingModule({
      declarations: [Feed],
      imports: [HttpClientTestingModule],
      schemas: [NO_ERRORS_SCHEMA],
      providers: [
        { provide: PostService, useValue: postService },
        { provide: UserService, useValue: userService },
        { provide: FollowService, useValue: followService },
        ChangeDetectorRef
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(Feed);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should load posts on init', () => {
    expect(postService.getPosts).toHaveBeenCalled();
  });

  it('should not create a post if content is empty', () => {
    component.newPostContent = '';
    component.selectedFile = null;

    component.createPost();

    expect(postService.createPost).not.toHaveBeenCalled();
  });

  it('should toggle like: like a post', () => {
    const post = { postId: 1, isLiked: false, likeCount: 0 };

    component.toggleLike(post);

    expect(postService.likePost).toHaveBeenCalledWith(1);
  });

  it('should toggle like: unlike a post', () => {
    const post = { postId: 1, isLiked: true, likeCount: 1 };

    component.toggleLike(post);

    expect(postService.removeLike).toHaveBeenCalledWith(1);
  });

  it('should toggle comments visibility', () => {
    const post = { postId: 1, showComments: false, comments: ['existing'] };
    
    component.toggleComments(post);

    expect(post.showComments).toBe(true);
  });

  it('should open report modal with correct target', () => {
    const post = { postId: 1, userId: 5, userName: 'baduser' };
    const mockEvent = { stopPropagation: vi.fn() } as any;

    component.openReportModal(post, mockEvent);

    expect(component.showReportModal).toBe(true);
    expect(component.reportTarget.userId).toBe(5);
    expect(component.reportTarget.username).toBe('baduser');
  });

  it('should close report modal and reset state', () => {
    component.showReportModal = true;
    component.reportTarget = { userId: 1, username: 'test' };

    component.closeReportModal();

    expect(component.showReportModal).toBe(false);
    expect(component.reportTarget).toBeNull();
  });
});
