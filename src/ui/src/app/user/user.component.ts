import { Component } from '@angular/core';
import { LoginComponent } from "./login/login.component";

@Component({
  selector: 'app-user',
  standalone: true,
  imports: [LoginComponent],
  templateUrl: './user.component.html',
  styleUrl: './user.component.scss'
})
export class UserComponent {

}
