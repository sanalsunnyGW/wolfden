import { FormControl } from "@angular/forms";

export interface IadminForm {
    designationId: FormControl<number | null>;
    departmentId: FormControl<number | null>;
    managerId: FormControl<number | null>;
    isActive: FormControl<boolean| null>;
    joiningDate:  FormControl<Date | null>;
    employmentType: FormControl<number | null>;
}
