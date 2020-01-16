import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-todo-item-delete',
  templateUrl: './todo-item-delete.component.html',
  styleUrls: ['./todo-item-delete.component.css']
})
export class TodoItemDeleteComponent {

  constructor(
    public dialogRef: MatDialogRef<TodoItemDeleteComponent>,
    @Inject(MAT_DIALOG_DATA) public name: string
  ) { }
}
