import { FormControl } from "@angular/forms";

export interface IcheckForm {

    rfid: FormControl<string | null>;
    employeeCode: FormControl<string | null>;
}
