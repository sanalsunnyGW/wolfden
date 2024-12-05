import { Form, FormControl } from "@angular/forms";

export interface IEditleaveFormControl{
    leaveRequestId : FormControl<number | null>;
    typeId : FormControl<number | null>;
    halfDay : FormControl<boolean|null>;
    fromDate : FormControl<Date | null>;
    toDate : FormControl<Date | null >;
    description : FormControl<string | null>;
}

export interface IEditleave{
    leaveRequestId : number | null
    typeId : number | null
    halfDay : boolean|null
    fromDate : Date | null
    toDate : Date | null 
    description : string | null
}