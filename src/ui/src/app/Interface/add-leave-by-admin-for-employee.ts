import { FormControl } from "@angular/forms";

export interface IAddLeaveByAdminForEmployeeFormControl{
    adminId : FormControl<number|null>;
    employeeCode : FormControl<number|null>;
    typeId : FormControl<number|null>;
    halfDay : FormControl<number|null>;
    fromDate : FormControl<boolean|null>;
    toDate : FormControl<boolean|null>;
    description : FormControl<string|null>;
}

export interface IAddLeaveByAdminForEmployee{
    adminId : number|null;
    employeeCode : number|null;
    typeId : number|null;
    halfDay : number|null;
    fromDate : boolean|null;
    toDate : boolean|null;
    description : string|null;
}