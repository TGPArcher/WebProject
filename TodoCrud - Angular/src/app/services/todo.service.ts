import { Injectable } from '@angular/core';
import { Observable, throwError, of } from 'rxjs';
import { ActionItem } from '../models/action-item';
import { HttpClient } from '@angular/common/http';
import { ConfigService } from './config.service';
import { catchError } from 'rxjs/operators';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class TodoService {

  private root: string;

  constructor(
    private http: HttpClient,
    private configService: ConfigService) {
    this.root = this.configService.getWebApiRoot();
  }

  getActionItems(): Observable<ActionItem[]> {
    return this.http.get<ActionItem[]>(this.route(''))
      .pipe(
        catchError(this.handleError)
      );
  }

  getActionItem(id: string): Observable<ActionItem> {
    return this.http.get<ActionItem>(this.route(`/${id}`))
      .pipe(
        catchError(this.handleError)
      );
  }

  addActionItem(name: string, completed: boolean): Observable<ActionItem> {
    const obj: any = {
      Content: name,
      Completed: completed
    };
    return this.http.post<ActionItem>(this.route(''), obj)
      .pipe(
        catchError(this.handleError)
      );
  }

  updateActionItem(item: ActionItem): Observable<ActionItem> {
    if (item === undefined) {
      return throwError('[updateActionItem]: item is null');
    }

    return this.http.put<ActionItem>(this.route(`/${item.id}`), item)
      .pipe(
        catchError(this.handleError)
      );
  }

  deleteActionItem(id: string): Observable<any> {
    return this.http.delete(this.route(`/${id}`))
      .pipe(
        catchError(this.handleError)
      );
  }

  private route(path: string): string {
    return this.root + 'todo' + path;
  }

  handleError(error) {
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
