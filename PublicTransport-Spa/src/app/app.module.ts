import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AppComponent } from './app.component';
import { RegisterComponent } from './user/register/register.component';
import { BsDropdownModule, BsDatepickerModule, ButtonsModule } from 'ngx-bootstrap';
import { HttpClientModule } from '@angular/common/http';
import { appRoutes } from './routes';
import { RouterModule } from '@angular/router';
import { JwtModule } from '@auth0/angular-jwt';
import { LoginComponent } from './user/login/login.component';
import { NavbarComponent } from './navbar/navbar.component';
import { AuthService } from './_services/auth.service';
import { UserService } from './_services/user.service';
import { AlertifyService } from './_services/alertify.service';
import { AdminService } from './_services/admin.service';
import { AuthGuard } from './_guards/auth.guard';
import { PricelistResolver } from './_resolvers/pricelist.resolver';
import { HomeComponent } from './home/home.component';

export function getToken() {
  return localStorage.getItem('token');
}

@NgModule({
   declarations: [
      AppComponent,
      RegisterComponent,
      LoginComponent,
      NavbarComponent,
      HomeComponent
   ],
   imports: [
      BrowserModule,
      RouterModule.forRoot(appRoutes),
      JwtModule.forRoot({
         config: {
            tokenGetter: getToken,
            whitelistedDomains: ['localhost:5000'],
            blacklistedRoutes: ['localhost:5000/api/authorization']
         }
      }),
      HttpClientModule,
      FormsModule,
      ReactiveFormsModule,
      BsDropdownModule.forRoot(),
      BsDatepickerModule.forRoot(),
      ButtonsModule.forRoot(),
  ],
  providers: [
     AuthService,
     UserService,
     AlertifyService,
     AdminService,
     AuthGuard,
     PricelistResolver
  ],
  bootstrap: [
     AppComponent
   ]
})
export class AppModule { }
