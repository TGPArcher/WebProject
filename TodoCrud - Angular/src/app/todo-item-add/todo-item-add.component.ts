import { Component } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-todo-item-add',
  templateUrl: './todo-item-add.component.html',
  styleUrls: ['./todo-item-add.component.css']
})
export class TodoItemAddComponent {

  task = '';
  checked = false;

  constructor(public dialogRef: MatDialogRef<TodoItemAddComponent>) { }

  onCancel() {
    this.dialogRef.close();
  }

  onAdd() {
    this.dialogRef.close({task: this.task, checked: this.checked});
  }
}
