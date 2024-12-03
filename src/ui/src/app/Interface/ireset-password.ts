import { FormControl } from "@angular/forms";

export interface IresetPassword {
    password: FormControl<string | null>;

    confirmPassword: FormControl<string | null>;
}
