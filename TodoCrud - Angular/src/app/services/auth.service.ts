import { Injectable } from '@angular/core';
import { User } from '../models/user';
import { HttpClient } from '@angular/common/http';
import { ConfigService } from './config.service';
import { catchError } from 'rxjs/operators';
import { throwError, BehaviorSubject } from 'rxjs';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private user: User;
  public $user = new BehaviorSubject<User>(this.user);

  constructor(
    private config: ConfigService,
    private http: HttpClient,
    private router: Router) {
      this.user = this.getLocalUser();
      this.$user.next(this.user);
    }

  login(username: string, password: string) {
    this.user = new User();
    this.user.username = username;
    this.user.password = password;
    this.setLocalUser(this.user);

    this.http.get<string>(this.route(`auth/login/${this.user.username}`))
    .pipe(
      catchError(this.handleError)
    ).subscribe(
      result => {
        const response = new User();
        response.id = result;
        response.username = username;
        response.password = password;
        this.loginIntoApp(response);
      },
      () => this.logout()
    );
  }

  register(username: string, password: string) {
    const user = new User();
    user.username = username;
    user.password = password;

    this.http.post<User>(this.route('auth/register'), user)
    .pipe(
      catchError(this.handleError)
    ).subscribe(
      response => this.loginIntoApp(response)
    );
  }

  logout() {
    console.log('logged out');
    this.user = undefined;
    this.$user.next(this.user);
    this.clearLocalUser();
    this.router.navigate(['/login']);
  }

  isLoggedIn(): boolean {
    return this.user !== undefined && this.user !== null;
  }

  getUserAuthData(): string {
    return btoa(`${this.user.username}:${this.user.password}`);
  }

  getUsername(): string {
    if (this.user === undefined) {
      return undefined;
    }
    return this.user.username;
  }

  redirectLogged() {
    this.router.navigateByUrl('items');
  }

  private loginIntoApp(user: User) {
    this.user = user;
    this.$user.next(this.user);
    this.setLocalUser(user);
    if (user === undefined || user.id === undefined || user.username === undefined || user.password === undefined) {
      return;
    }

    console.log('logged in');
    this.redirectLogged();
  }

  private getLocalUser(): User {
    return JSON.parse(localStorage.getItem('user')) as User;
  }

  private setLocalUser(user: User) {
    localStorage.setItem('user', JSON.stringify(user));
  }

  private clearLocalUser() {
    localStorage.removeItem('user');
  }

  private route(path: string): string {
    return this.config.getWebApiRoot() + path;
  }

  private handleError(error) {
    let errorMessage = '';
    if (error.error instanceof ErrorEvent) {
      // client-side error
      errorMessage = `Error: ${error.error.message}`;
    } else {
      // server-side error
      errorMessage = `Error Code: ${error.status}\nMessage: ${error.message}`;
    }
    window.alert(errorMessage);
    return throwError(errorMessage);
  }
}
