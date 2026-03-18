import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { PostService } from './post';

describe('Post Service', () => {
  let service: PostService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [PostService]
    });
    service = TestBed.inject(PostService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should fetch posts', () => {
    const mockPosts = [{ postId: 101, content: 'Hello Content' }];

    service.getPosts().subscribe((posts: any[]) => {
      expect(posts.length).toBe(1);
      expect(posts[0].content).toBe('Hello Content');
    });

    const req = httpMock.expectOne('http://localhost:5134/api/post');
    expect(req.request.method).toBe('GET');
    req.flush(mockPosts);
  });

  it('should create a post', () => {
    const mockResponse = { postId: 102, content: 'New Test Post' };

    service.createPost('New Test Post').subscribe((res: any) => {
      expect(res.postId).toBe(102);
    });

    const req = httpMock.expectOne('http://localhost:5134/api/post');
    expect(req.request.method).toBe('POST');
    req.flush(mockResponse);
  });
});
