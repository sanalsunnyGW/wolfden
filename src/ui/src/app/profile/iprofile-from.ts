import { Form, FormControl } from "@angular/forms";

export interface IProfileForm{
    firstName:FormControl<string|null>;
    lastName:FormControl<string|null>;
    gender:FormControl<string|null>;
    birthday:FormControl<Date|null>;
    joiningDate:FormControl<Date|null>;
    phoneNumber:FormControl<string|null>;
    email:FormControl<string|null>;
    address:FormControl<string|null>;
    country:FormControl<string|null>;
    state:FormControl<string|null>;

    
}