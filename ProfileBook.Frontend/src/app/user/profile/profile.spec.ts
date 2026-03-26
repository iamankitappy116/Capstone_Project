import { ComponentFixture, TestBed } from '@angular/core/testing';
import { Profile } from './profile';
import { UserService } from '../../core/services/user';
import { PostService } from '../../core/services/post';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { of } from 'rxjs';
import { ChangeDetectorRef, NO_ERRORS_SCHEMA } from '@angular/core';

describe('Profile Component', () => {
  let component: Profile;
  let fixture: ComponentFixture<Profile>;
  let userService: any;
  let postService: any;

  const mockProfile = {
    userId: 1,
    username: 'testuser',
    email: 'test@test.com',
    bio: 'Hello world',
    location: 'New York',
    profileImage: null
  };

  beforeEach(async () => {
    userService = {
      getMyProfile: vi.fn().mockReturnValue(of(mockProfile)),
      updateUser: vi.fn().mockReturnValue(of(mockProfile))
    };

    postService = {
      getPostsByUser: vi.fn().mockReturnValue(of([])),
      uploadMedia: vi.fn().mockReturnValue(of({ url: '/uploads/pic.jpg' }))
    };

    await TestBed.configureTestingModule({
      declarations: [Profile],
      imports: [HttpClientTestingModule],
      schemas: [NO_ERRORS_SCHEMA],
      providers: [
        { provide: UserService, useValue: userService },
        { provide: PostService, useValue: postService },
        ChangeDetectorRef
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(Profile);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should load profile on init', () => {
    expect(userService.getMyProfile).toHaveBeenCalled();
    expect(component.userProfile).toEqual(mockProfile);
  });

  it('should load user posts after profile is loaded', () => {
    expect(postService.getPostsByUser).toHaveBeenCalledWith(1);
  });

  it('should toggle edit mode on/off', () => {
    expect(component.isEditing).toBe(false);

    component.toggleEditMode();
    expect(component.isEditing).toBe(true);

    component.toggleEditMode();
    expect(component.isEditing).toBe(false);
  });

  it('should reset editProfile when cancelling edit mode', () => {
    component.isEditing = true;
    component.editProfile.username = 'changed';
    
    component.toggleEditMode(); // Toggle off = cancel

    expect(component.editProfile.username).toBe(mockProfile.username);
  });

  it('should call updateUser when saveProfile is called', () => {
    component.currentUserId = 1;
    component.editProfile = { ...mockProfile };

    component.saveProfile();

    expect(userService.updateUser).toHaveBeenCalledWith(1, {
      username: 'testuser',
      email: 'test@test.com',
      bio: 'Hello world',
      location: 'New York',
      profileImage: null
    });
  });

  it('should return avatar URL when no profile image is set', () => {
    component.userProfile = { ...mockProfile, profileImage: null };
    const url = component.getProfileImageUrl();
    expect(url).toContain('ui-avatars.com');
  });

  it('should return full image URL when profile image is set', () => {
    component.userProfile = { ...mockProfile, profileImage: '/uploads/pic.jpg' };
    const url = component.getProfileImageUrl();
    expect(url).toContain('http://localhost:5134/uploads/pic.jpg');
  });
});
