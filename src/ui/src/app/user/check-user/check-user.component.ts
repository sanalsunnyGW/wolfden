import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { WolfDenService } from '../../Service/wolf-den.service';
import { IcheckForm } from './icheck-form';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-check-user',
  standalone: true,
  imports: [ReactiveFormsModule,RouterLink],
  templateUrl: './check-user.component.html',
  styleUrl: './check-user.component.scss'
})
export class CheckUserComponent {
  userForm: FormGroup;

  constructor(private fb: FormBuilder,
              private router: Router, 
              private userService: WolfDenService,
              private toastr : ToastrService
            ) {
    this.userForm = this.fb.group<IcheckForm>({
      rfid: new FormControl('', [Validators.required, ]),
      employeeCode: new FormControl('', Validators.required)
    });
  }

  isSubmitted: boolean = false;

  onSubmit() {
    this.isSubmitted = true;
    ;
    if(this.userForm.valid){
     
      this.userService.getEmployeeSignUp(this.userForm.value.employeeCode,this.userForm.value.rfId).subscribe({
        next: (response: any) => {
        this.toastr.success('Sucess')
        this.router.navigate(['/user/sign-in'])
      },
      error: (error: any) => {
        this.toastr.error('Invalid Credentials')


    }
  });
  }else{
    this.toastr.error('Unsuccessful')
  }
}
hasDisplayableError(controlName: string ):Boolean {
  const control = this.userForm.get(controlName);
  return Boolean(control?.invalid) && (this.isSubmitted || Boolean(control?.touched) || Boolean(control?.dirty))
}

}
