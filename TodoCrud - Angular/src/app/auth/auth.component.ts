import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { Subject } from 'rxjs';
import { takeUntil, take } from 'rxjs/operators';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-auth',
  templateUrl: './auth.component.html',
  styleUrls: ['./auth.component.css']
})
export class AuthComponent implements OnInit, OnDestroy {

  form: FormGroup;
  register = false;
  onDestroyNotifier$ = new Subject();

  constructor(
    private formBuilder: FormBuilder,
    private route: ActivatedRoute,
    private authService: AuthService) { }

  ngOnInit() {
    this.redirectIfLogged();
    this.checkAction();
    this.createAuthForm();
  }

  ngOnDestroy() {
    this.onDestroyNotifier$.next();
  }

  checkAction() {
    this.route.data
    .pipe(
      take(1),
      takeUntil(this.onDestroyNotifier$)
    )
    .subscribe(
      d => {
        this.register = d.action === 'register';
      }
    );
  }

  createAuthForm() {
    this.form = this.formBuilder.group({
      username: ['', [Validators.required, Validators.minLength(3)]],
      password: ['', [Validators.required, Validators.minLength(3)]]
    });
  }

  submitAuthForm() {
    if (this.register) {
      this.authService.register(this.form.get('username').value, this.form.get('password').value);
    } else {
      this.authService.login(this.form.get('username').value, this.form.get('password').value);
    }
  }

  private redirectIfLogged() {
    if (this.authService.isLoggedIn()) {
      this.authService.redirectLogged();
    }
  }
}
