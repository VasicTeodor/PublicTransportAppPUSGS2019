
import { Injectable } from '@angular/core';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { AlertifyService } from '../_services/alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Station } from '../_models/station';
import { AdminService } from '../_services/admin.service';
import { Line } from '../_models/line';
import { UserService } from '../_services/user.service';

@Injectable()
export class LinesResolver implements Resolve<Line[]> {

    constructor(private router: Router, private alertify: AlertifyService, private userService: UserService) {}

    resolve(route: ActivatedRouteSnapshot): Observable<Line[]> {
        const day = this.getDate();
        return this.userService.getLines('In City', day).pipe(
            catchError(error => {
                this.alertify.error('Problem retrieving data');
                this.router.navigate(['/home']);
                return of(null);
            })
        );
    }

    private getDate(): string {
        let result = '';
        var d = new Date();
        var day = d.getDay();

        switch (day) {
            case 6:
                result = 'Saturday';
                break;
            case 7:
                result = 'Sunday';
                break;
            default:
                result = 'Working day'
                break;
        }

        return result;
    }
}
