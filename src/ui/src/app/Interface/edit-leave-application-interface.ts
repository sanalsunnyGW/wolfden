import { Form, FormControl } from "@angular/forms";

export interface IEditleave{
    leaveRequestId : FormControl<number | null>;
    typeId : FormControl<number | null>;
    halfDay : FormControl<boolean|null>;
    fromDate : FormControl<Date | null>;
    toDate : FormControl<Date | null >;
    description : FormControl<string | null>;
}