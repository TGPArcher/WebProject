import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { TodoListComponent } from './todo-list/todo-list.component';
import { AuthComponent } from './auth/auth.component';
import { FullscreenOverlayContainer } from '@angular/cdk/overlay';
import { AuthGuard } from './auth.guard';


const routes: Routes = [
  {
    path: 'items',
    component: TodoListComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'register',
    component: AuthComponent,
    data: { action: 'register' }
  },
  {
    path: 'login',
    component: AuthComponent,
    data: { action: 'login'}
  },
  {
    path: '',
    redirectTo: 'register',
    pathMatch: 'full'
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
