import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { CheckUserComponent } from "./check-user/check-user.component";

@Component({
  selector: 'app-user',
  standalone: true,
  imports: [RouterOutlet],
  templateUrl: './user.component.html',
  styleUrl: './user.component.scss'
})
export class UserComponent {

}
