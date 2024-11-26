import { FormControl } from "@angular/forms";

export interface ICheckForm {

    rfId: FormControl<string | null>;
    employeeCode: FormControl<string | null>;
}
