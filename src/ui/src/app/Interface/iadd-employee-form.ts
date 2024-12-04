import { FormControl } from "@angular/forms";

export interface IaddEmployeeForm {
    employeeCode: FormControl<number | null>;
    rfId: FormControl<string | null>;
}
