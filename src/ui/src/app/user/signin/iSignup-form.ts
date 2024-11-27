import { DatePipe } from '@angular/common';
import { FormControl, } from '@angular/forms';

export interface ISignupForm {
    email: FormControl<string | null>;
    firstName: FormControl<string|null>;
    lastName: FormControl<string|null>;
    dateofBirth:FormControl<Date|null>;
    gender:FormControl<number|null>;
    phoneNumber:FormControl<string|null>;
    password: FormControl<string|null>;
    confirmPassword: FormControl<string|null>;
}
