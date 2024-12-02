import { FormControl } from "@angular/forms";

export interface ImanagerForm {
    firstName: FormControl<string|null>;
    lastName: FormControl<string | null>;

}
