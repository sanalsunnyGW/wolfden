import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { IaddHoliday } from '../interface/iadd-holiday';
import { WolfDenService } from '../service/wolf-den.service';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { formatDate } from 'date-fns/format';
import { CommonModule } from '@angular/common';
import { MatNativeDateModule } from '@angular/material/core'; 
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { IaddHolidayService } from '../interface/iadd-holiday-service';
import { IDepartment } from '../interface/idepartment';

@Component({
  selector: 'app-admin-add-holiday',
  standalone: true,
  imports: [  CommonModule,
              ReactiveFormsModule, MatNativeDateModule, MatDatepickerModule,
               MatInputModule,MatFormFieldModule, MatSelectModule],
  templateUrl: './admin-add-holiday.component.html',
  styleUrl: './admin-add-holiday.component.scss'
})
export class AdminAddHolidayComponent {
  userForm: FormGroup<IaddHoliday>;

  constructor(private fb: FormBuilder,
    private userService: WolfDenService,
    private router: Router,
    private toastr: ToastrService
  ){
    this.userForm = this.fb.group({
      date: new FormControl<Date | null>(null, Validators.required),  
      type: new FormControl<number | null>(null, Validators.required),
      description: new FormControl<string | null>(null, Validators.required),
  });
}
isSubmitted: boolean = false;
departmentData: IDepartment[] = [{
  id: 0,
  departmentName: ''
}];
  onSubmit() {

   this.isSubmitted=true;
    if (this.userForm.valid) {
      const form : IaddHolidayService = {
        date: this.userForm.value.date ?? null,
        type: Number(this.userForm.value.type) ?? 0,
        description: this.userForm.value.description ?? '',
       }
      this.userService.addHoliday(form).subscribe({
        next: (response) => {
          if (response) {
           this.toastr.success("Added Holiday");
           this.userForm.reset();
           this.isSubmitted = false; 
            
          }
          else {
            this.toastr.error("form invalid");
          }


        },
        error: (error: any) => {
          console.log(this.userForm);
          this.toastr.error('Error');

        }
      });

    } else {
      this.toastr.error('Enter valid details', 'Unsuccessful')
    }
  }
}
