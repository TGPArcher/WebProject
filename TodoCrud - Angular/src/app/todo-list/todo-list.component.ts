import { Component, OnInit, OnDestroy, ViewChild, ElementRef } from '@angular/core';
import { TodoService } from '../services/todo.service';
import { ActionItem } from '../models/action-item';
import { Subject, Observable, BehaviorSubject } from 'rxjs';
import { take, takeUntil } from 'rxjs/operators';
import { MatDialog } from '@angular/material/dialog';
import { TodoItemAddComponent } from '../todo-item-add/todo-item-add.component';
import { TodoItemDeleteComponent } from '../todo-item-delete/todo-item-delete.component';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatCheckboxChange } from '@angular/material/checkbox';

@Component({
  selector: 'app-todo-list',
  templateUrl: './todo-list.component.html',
  styleUrls: ['./todo-list.component.css']
})
export class TodoListComponent implements OnInit, OnDestroy {

  items: ActionItem[];
  $items = new BehaviorSubject<ActionItem[]>(this.items);

  displayedColumns: string[] = ['content', 'created', 'completed', 'actions'];
  editing = false;
  editIndex: number;
  editId: string;
  @ViewChild('nameInput', {read: ElementRef, static: false}) nameInput: ElementRef;

  private onDestroyNotifier$ = new Subject<any>();

  constructor(
    private todoService: TodoService,
    public dialog: MatDialog,
    private snackBar: MatSnackBar) { }

  ngOnInit() {
    this.getActionItems();
  }

  ngOnDestroy() {
    this.onDestroyNotifier$.next();
  }

  getActionItems(): void {
    this.todoService.getActionItems()
    .pipe(
      take(1),
      takeUntil(this.onDestroyNotifier$)
    )
    .subscribe(
      result => {
        this.items = result;
        this.updateItems();
      },
      error => console.error(error)
    );
  }

  openAddItemDialog(): void {
    const dialogRef = this.dialog.open(TodoItemAddComponent);

    dialogRef.afterClosed().subscribe(
      result => {
        if (result !== undefined) {
          this.todoService.addActionItem(result.task, result.checked)
          .pipe(
            take(1),
            takeUntil(this.onDestroyNotifier$)
          )
          .subscribe(
            resultItem => this.itemFromResponse(resultItem)
          );
        }
      }
    );
  }

  checkActionItem(item: ActionItem, value: MatCheckboxChange): void {
    item.completed = value.checked;
    this.sendEditRequest(item);
  }

  editActionItem(item: ActionItem): void {
    this.setEditing(this.itemIndex(item));
  }

  saveEditActionItem(): void {
    if (!this.editing) {
      return;
    }

    const itemToEdit = this.items[this.editIndex];
    itemToEdit.content = this.nameInput.nativeElement.value;
    this.sendEditRequest(itemToEdit);
  }

  cancelEditActionItem(): void {
    this.setEditing(-1);
  }

  triggerDeleteActionItem(item: ActionItem): void {
    const dialogRef = this.dialog.open(TodoItemDeleteComponent, {data: item.content});

    dialogRef.afterClosed().subscribe(
      result => {
        if (result) {
          this.deleteActionItem(item);
        }
      }
    );
  }

  private deleteActionItem(item: ActionItem): void {
    if (item === undefined) {
      return;
    }

    this.todoService.deleteActionItem(item.id)
    .subscribe(
      () => {
        this.items = this.items.filter(i => i.id !== item.id);
        this.updateItems();
        this.snackBar.open('Item deleted successfully!', '', {duration: 1000});
      }
    );
  }

  private sendEditRequest(item: ActionItem): void {
    this.todoService.updateActionItem(item)
    .pipe(
      take(1),
      takeUntil(this.onDestroyNotifier$)
    )
    .subscribe(
      resultItem => {
        this.itemFromResponse(resultItem);
        this.setEditing(-1);
      }
    );
  }

  private updateItems() {
    this.$items.next(this.items);
  }

  private itemIndex(item: ActionItem): number {
    return this.items.findIndex(i => i.id === item.id);
  }

  private setEditing(index: number): void {
    this.editIndex = index;
    if (this.editIndex === -1) {
      this.editing = false;
      this.editId = undefined;
    } else {
      this.editing = true;
      this.editId = this.items[this.editIndex].id;
      this.updateItems();
    }
  }

  private itemFromResponse(item: ActionItem, showNotification?: boolean): void {
    if (item === undefined) {
      return;
    }
    if (showNotification === undefined) {
      showNotification = true;
    }

    const index = this.itemIndex(item);
    if (index !== -1) {
      this.items[index] = item;
      this.updateItems();
      this.snackBar.open('Item updateded!', '', {duration: 1000});
    } else {
      this.items.push(item);
      this.updateItems();
      this.snackBar.open('Item created!', '', {duration: 1000});
    }
  }
}
