import { Routes } from '@angular/router';
import { LoginComponent } from './user/login/login.component';
import { RegisterComponent } from './user/register/register.component';
import { HomeComponent } from './home/home.component';
import { TicketsComponent } from './passenger/tickets/tickets.component';
import { PricelistResolver } from './_resolvers/pricelist.resolver';
import { TimetableComponent } from './passenger/timetable/timetable.component';
import { UpdateAccountComponent } from './passenger/update-account/update-account.component';
import { UpdateUserResolver } from './_resolvers/update-user.resolver';
import { MapComponent } from './passenger/map/map.component';
import { UserVerificationComponent } from './controller/userVerification/userVerification.component';
import { UserVerificationResolver } from './_resolvers/userVerification.resolver';
import { TicketVerificationComponent } from './controller/ticketVerification/ticketVerification.component';
import { TicketVerificationResolver } from './_resolvers/ticketVerification.resolver';
import { PricelistComponent } from './passenger/pricelist/pricelist.component';
import { NewLineComponent } from './admin/newLine/newLine.component';
import { ViewChild } from '@angular/core';
import { ViewLinesComponent } from './admin/viewLines/viewLines.component';
import { NewStationComponent } from './admin/newStation/newStation.component';
import { ViewStationsComponent } from './admin/viewStations/viewStations.component';
import { NewTimetableComponent } from './admin/newTimetable/newTimetable.component';
import { ViewTimetablesComponent } from './admin/viewTimetables/viewTimetables.component';
import { ViewPricelistComponent } from './admin/viewPricelist/viewPricelist.component';

export const appRoutes: Routes = [
    {path: '', component: HomeComponent},
    {path: 'login', component: LoginComponent},
    {path: 'register', component: RegisterComponent},
    {path: 'timetable', component: TimetableComponent},
    {path: 'map', component: MapComponent},
    {path: 'tickets', component: TicketsComponent, resolve: {pricelists: PricelistResolver}},
    {path: 'pricelist', component: PricelistComponent},// resolve: {pricelists: PricelistResolver}},
    {path: 'updateAccount', component: UpdateAccountComponent, resolve: {user: UpdateUserResolver}},
    {path: 'userVerification', component: UserVerificationComponent, resolve: {users: UserVerificationResolver}},
    {path: 'ticketVerification', component: TicketVerificationComponent},// resolve: {tickets: TicketVerificationResolver}},
    {path: 'newLine', component: NewLineComponent},
    {path: 'viewLines', component: ViewLinesComponent},
    {path: 'newStation', component: NewStationComponent},
    {path: 'viewStations', component: ViewStationsComponent},
    {path: 'newTimetable', component: NewTimetableComponent},
    {path: 'viewTimetables', component: ViewTimetablesComponent},
    {path: 'viewPricelist', component: ViewPricelistComponent},
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
