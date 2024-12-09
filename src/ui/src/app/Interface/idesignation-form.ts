import { FormControl } from "@angular/forms";

export interface IDesignationForm {
    designationName: FormControl<string | null>;
}
export interface IDesignationData {
    designationName: string | null;
}
