import { FormControl } from "@angular/forms";

export interface IProfileForm {
    id: FormControl<number | null>;
    firstName: FormControl<string | null>;
    lastName: FormControl<string | null>;
    gender: FormControl<number | null>;
    dateofBirth: FormControl<Date | null>;
    phoneNumber: FormControl<string | null>;
    email: FormControl<string | null>;
    address: FormControl<string | null>;
    country: FormControl<string | null>;
    state: FormControl<string | null>;
    photo: FormControl<string | null>;


}