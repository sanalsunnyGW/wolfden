import { FormControl } from "@angular/forms";

export interface IAddNewLeaveTypeFormcontrol
{   
    adminId : FormControl<number|null>;
    typeName : FormControl<string|null>;
    maxDays : FormControl<number|null>;
    isHalfDayAllowed : FormControl<boolean|null>;
    incrementCount : FormControl<number|null>;
    incrementGapId : FormControl<number|null>;
    carryForward : FormControl<boolean|null>
    carryForwardLimit : FormControl<number|null>;
    daysCheck : FormControl<number|null>;
    daysCheckMore : FormControl<number|null>;
    daysCheckEqualOrLess : FormControl<number|null>;
    dutyDaysRequired : FormControl<number|null>;
    sandwich : FormControl<boolean|null>;
}

export interface IAddNewLeaveType
{   
    adminId : number|null;
    typeName : string|null;
    maxDays : number|null;
    isHalfDayAllowed : boolean|null;
    incrementCount : number|null;
    incrementGapId : number|null;
    carryForward : boolean|null;
    carryForwardLimit : number|null;
    daysCheck : number|null;
    daysCheckMore : number|null;
    daysCheckEqualOrLess : number|null;
    dutyDaysRequired : number|null;
    sandwich : boolean|null;
}