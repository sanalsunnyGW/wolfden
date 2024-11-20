import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { WolfDenService } from '../../wolf-den.service';
import { IcheckForm } from './icheck-form';

@Component({
  selector: 'app-check-user',
  standalone: true,
  imports: [ReactiveFormsModule,RouterLink],
  templateUrl: './check-user.component.html',
  styleUrl: './check-user.component.scss'
})
export class CheckUserComponent {
  userForm: FormGroup;

  constructor(private fb: FormBuilder, private router: Router, private userService: WolfDenService) {
    this.userForm = this.fb.group<IcheckForm>({
      rfid: new FormControl('', [Validators.required, ]),
      employeeCode: new FormControl('', Validators.required)
    });
  }

  isSubmitted: boolean = false;

  onSubmit() {
    this.isSubmitted = true;
    this.router.navigate(['/user/sign-in']);
    //api logic
  }

  hasDisplayableError(controlName: string ):Boolean {
    const control = this.userForm.get(controlName);
    return Boolean(control?.invalid) && (this.isSubmitted || Boolean(control?.touched) || Boolean(control?.dirty))
    }
}
