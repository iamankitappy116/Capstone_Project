import { Component, OnInit, OnDestroy, ChangeDetectorRef } from '@angular/core';
import { MessageService } from '../../core/services/message';
import { UserService } from '../../core/services/user';
import { interval, Subscription } from 'rxjs';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-messages',
  standalone: false,
  templateUrl: './messages.html',
  styleUrl: './messages.css'
})
export class Messages implements OnInit, OnDestroy {
  inbox: any[] = [];
  selectedContact: any = null;
  messages: any[] = [];
  newMessageContent: string = '';
  currentUserId: number = 0;
  private pollingSub?: Subscription;

  constructor(
    private messageService: MessageService,
    private userService: UserService,
    private cdr: ChangeDetectorRef,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    const userIdStr = localStorage.getItem('userId');
    if (userIdStr) {
      this.currentUserId = parseInt(userIdStr);
      this.loadInbox(() => {
        // Check for query param after inbox is loaded
        this.route.queryParams.subscribe(params => {
          const targetUserId = params['userId'];
          if (targetUserId) {
            this.handleDirectMessage(parseInt(targetUserId));
          }
        });
      });
      
      // Setup polling for new messages every 5 seconds
      this.pollingSub = interval(5000).subscribe(() => {
        this.loadInbox();
        if (this.selectedContact) {
          this.loadConversation(this.selectedContact.contactId);
        }
      });
    }
  }

  ngOnDestroy(): void {
    if (this.pollingSub) {
      this.pollingSub.unsubscribe();
    }
  }

  loadInbox(callback?: Function): void {
    this.messageService.getInbox(this.currentUserId).subscribe({
      next: (data) => {
        this.inbox = data;
        this.cdr.detectChanges();
        if (callback) callback();
      },
      error: (err) => console.error('Error loading inbox:', err)
    });
  }

  handleDirectMessage(targetUserId: number): void {
    if (targetUserId === this.currentUserId) return;

    // Find in inbox
    const existing = this.inbox.find(c => c.contactId === targetUserId);
    if (existing) {
      this.selectContact(existing);
    } else {
      // Not in inbox, fetch user details to start new conversation interface
      this.userService.getUserById(targetUserId).subscribe({
        next: (user) => {
          this.selectedContact = {
            contactId: user.userId,
            contactName: user.username,
            contactProfileImage: user.profileImage,
            lastMessage: '',
            lastMessageTime: new Date().toISOString(),
            unreadCount: 0
          };
          this.loadConversation(user.userId);
        },
        error: (err: any) => console.error('Error fetching user for direct message:', err)
      });
    }
  }

  selectContact(contact: any): void {
    this.selectedContact = contact;
    this.loadConversation(contact.contactId);
  }

  loadConversation(contactId: number): void {
    this.messageService.getConversation(this.currentUserId, contactId).subscribe({
      next: (data) => {
        this.messages = data;
        this.cdr.detectChanges();
        this.scrollToBottom();
      },
      error: (err) => console.error('Error loading conversation:', err)
    });
  }

  sendMessage(): void {
    if (!this.newMessageContent.trim() || !this.selectedContact) return;

    const messageData = {
      senderId: this.currentUserId,
      receiverId: this.selectedContact.contactId,
      messageContent: this.newMessageContent
    };

    this.messageService.sendMessage(messageData).subscribe({
      next: () => {
        this.newMessageContent = '';
        this.loadConversation(this.selectedContact.contactId);
        this.loadInbox();
      },
      error: (err) => console.error('Error sending message:', err)
    });
  }

  scrollToBottom(): void {
    setTimeout(() => {
      const chatBody = document.querySelector('.chat-body');
      if (chatBody) {
        chatBody.scrollTop = chatBody.scrollHeight;
      }
    }, 100);
  }

  getProfileImage(imageUrl: string | null, name: string): string {
    if (imageUrl) {
      return `http://localhost:5134${imageUrl}`;
    }
    return `https://ui-avatars.com/api/?name=${name || 'User'}&background=random`;
  }

  formatTime(dateStr: string): string {
    const date = new Date(dateStr);
    return date.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' });
  }
}
