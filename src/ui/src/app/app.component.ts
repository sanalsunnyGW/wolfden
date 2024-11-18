import { Component } from '@angular/core';

import { SigninComponent } from "./user/signin/signin.component";
import { UserComponent } from "./user/user.component";

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [ UserComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'wolf-den';
}
