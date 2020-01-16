import { Component, OnInit, Input } from '@angular/core';
import { ActionItem } from '../models/action-item';

@Component({
  selector: 'app-todo-item',
  templateUrl: './todo-item.component.html',
  styleUrls: ['./todo-item.component.css']
})
export class TodoItemComponent implements OnInit {

  @Input() item: ActionItem;

  constructor() { }

  ngOnInit() {
  }

}
