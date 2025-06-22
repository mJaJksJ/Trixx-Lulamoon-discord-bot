import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-root',
  template: `
    <router-outlet></router-outlet>
  `,
  standalone: false,
})
export class AppComponent implements OnInit {
  constructor() {}

  ngOnInit(): void {}
}
