import { Component } from '@angular/core';
import { SigninComponent } from "./signin/signin.component";

@Component({
  selector: 'app-user',
  standalone: true,
  imports: [SigninComponent],
  templateUrl: './user.component.html',
  styleUrl: './user.component.scss'
})
export class UserComponent {

}
