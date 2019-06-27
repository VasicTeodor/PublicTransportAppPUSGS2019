import { Component, OnInit } from '@angular/core';
import { User } from 'src/app/_models/user';
import { ActivatedRoute } from '@angular/router';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { ControllerService } from 'src/app/_services/controller.service';
import { AuthService } from 'src/app/_services/auth.service';
import * as moment from 'moment';
import { Pagination, PaginatedResult } from 'src/app/_models/Pagination';
import { UserService } from 'src/app/_services/user.service';

@Component({
  selector: 'app-userVerification',
  templateUrl: './userVerification.component.html',
  styleUrls: ['./userVerification.component.css']
})
export class UserVerificationComponent implements OnInit {
  users: User[];
  selectedUser: User;
  pagination: Pagination;

  constructor(private route: ActivatedRoute, private alertify: AlertifyService, private userService: UserService,
               private controllerService: ControllerService, private authService: AuthService) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.users = data.users.result;
      this.pagination = data.users.pagination;
    });
  }

  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    this.loadUsers();
  }

  loadUsers() {
  this.userService.getUsers(this.pagination.currentPage, this.pagination.itemsPerPage).subscribe((res: PaginatedResult<User[]>) => {
    this.users = res.result;
    this.pagination = this.pagination;
  }, error => {
    this.alertify.error(error);
  })
}

verificateUserApprove() {
    this.controllerService.verificateUser(this.selectedUser.id, true).subscribe(() => {
      this.selectedUser.accountStatus = 'Active';
      this.selectedUser.verified = true;
      this.alertify.success('User account approved.');
    }, error => {this.alertify.error('Operation failed.')});
  }

  verificateUserReject() {
    this.controllerService.verificateUser(this.selectedUser.id, false).subscribe(() => {
      this.selectedUser.accountStatus = 'Rejected';
      this.selectedUser.verified = false;
      this.alertify.success('User account rejected.');
    }, error => {this.alertify.error('Operation failed.')});
  }

  selectUser(user: User) {
    const myMoment: moment.Moment = moment(user.dateOfBirth);
    this.selectedUser = user;
    this.selectedUser.dateOfBirth = myMoment.toDate();
  }

}
