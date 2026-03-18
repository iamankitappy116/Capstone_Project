import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { Login } from './auth/login/login';
import { Register } from './auth/register/register';
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
import { AuthGuard, AdminGuard } from './core/guards/auth.guard';

const routes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: 'login', component: Login },
  { path: 'register', component: Register },
  { path: 'dashboard', component: UserDashboard, canActivate: [AuthGuard], children: [
    { path: 'feed', component: Feed },
    { path: 'profile', component: Profile },
    { path: 'messages', component: Messages },
    { path: 'groups', component: Groups }
  ] },
  {
    path: 'admin-dashboard',
    component: AdminLayoutComponent,
    canActivate: [AuthGuard, AdminGuard],
    children: [
      { path: '', redirectTo: 'stats', pathMatch: 'full' },
      { path: 'stats', component: AdminDashboardComponent },
      { path: 'users', component: UserManagementComponent },
      { path: 'approvals', component: PostApprovalsComponent },
      { path: 'reports', component: ReportedUsersComponent },
      { path: 'groups', component: GroupManagementComponent },
    ]
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
