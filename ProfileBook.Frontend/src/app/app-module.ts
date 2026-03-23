import { FormsModule } from '@angular/forms';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import {
  NgModule,
  provideBrowserGlobalErrorListeners,
  provideZoneChangeDetection,
} from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing-module';
import { App } from './app';
import { Login } from './auth/login/login';
import { Register } from './auth/register/register';
import { Navbar } from './shared/navbar/navbar';
import { Sidebar } from './shared/sidebar/sidebar';
import { UserDashboard } from './pages/user-dashboard/user-dashboard';
import { Feed } from './user/feed/feed';
import { Profile } from './user/profile/profile';
import { Messages } from './user/messages/messages';
import { Groups } from './user/groups/groups';
import { AdminDashboardComponent } from './pages/admin-dashboard/stats/admin-dashboard';
import { AdminLayoutComponent } from './pages/admin-dashboard/admin-layout';
import { UserManagementComponent } from './pages/admin-dashboard/users/user-management';
import { PostApprovalsComponent } from './pages/admin-dashboard/approvals/post-approvals';
import { ReportedUsersComponent } from './pages/admin-dashboard/reports/reported-users';
import { GroupManagementComponent } from './pages/admin-dashboard/groups/group-management';
import { AutoFocus } from './directives/auto-focus';
import { ReactiveFormsModule } from '@angular/forms';
@NgModule({
  declarations: [
    App,
    Login,
    Register,
    UserDashboard,
    AdminDashboardComponent,
    AdminLayoutComponent,
    UserManagementComponent,
    PostApprovalsComponent,
    ReportedUsersComponent,
    GroupManagementComponent,
    Sidebar,
    Navbar,
    Feed,
    Profile,
    Messages,
    Groups,
    AutoFocus,
  ],
  imports: [BrowserModule, AppRoutingModule, FormsModule, ReactiveFormsModule],
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideHttpClient(),
    provideZoneChangeDetection({ eventCoalescing: true }),
  ],
  bootstrap: [App],
})
export class AppModule {}
