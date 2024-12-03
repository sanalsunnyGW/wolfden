import { Form, FormControl } from "@angular/forms";

export interface IroleForm {
    id: FormControl<number | null>;
    role: FormControl<string | null>;

}
