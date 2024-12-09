import { FormControl } from "@angular/forms";

export interface IDepartmentForm {
    departmentName: FormControl<string | null>;
}
export interface IDepartmentData {
    departmentName: string | null;
}

