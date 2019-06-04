import { Routes } from '@angular/router';
import { LoginComponent } from './user/login/login.component';
import { RegisterComponent } from './user/register/register.component';
import { HomeComponent } from './home/home.component';

export const appRoutes: Routes = [
    {path: '', component: HomeComponent},
    {path: 'login', component: LoginComponent},
    {path: 'register', component: RegisterComponent},
    { path: '**', redirectTo: '', pathMatch: 'full'} // order is important and this need to be last
//     { path: '', component: HomeComponent},
//     { path: '',
//         runGuardsAndResolvers: 'always',
//         canActivate: [AuthGuard],
//         children: [
//             { path: 'members', component: MemberListComponent, resolve: {users: MemberListResolver}},
//             { path: 'members/:id', component: MemberDetailComponent, resolve: {user: MemberDetailResolver}},
//             { path: 'member/edit', component: MemberEditComponent, resolve: {user: MemberEditResolver},
//                 canDeactivate: [PreventUnsavedChanges]},
//             { path: 'messages', component: MessagesComponent},
//             { path: 'lists', component: ListsComponent, resolve: {users: ListsResolver}},
//             { path: 'admin', component: AdminPanelComponent, data: {roles: ['Admin', 'Moderator']}}
//         ]
//     },
];
