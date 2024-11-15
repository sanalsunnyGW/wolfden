import { FormControl } from "@angular/forms";

export interface IAddNewLeaveType
{
    typeName : FormControl<string|null>;
    maxDays : FormControl<number|null>;
    halfDay : FormControl<boolean|null>;
    incrementCount : FormControl<number|null>;
    incrementGap : FormControl<number|null>;
    carryforward : FormControl<boolean|null>
    carryforwardLimit : FormControl<number|null>;
    daysCheck : FormControl<number|null>;
    daysCheckMore : FormControl<number|null>;
    daysCheckEqualOrLess : FormControl<number|null>;
    dutyDaysRequired : FormControl<number|null>;
    hidden : FormControl<boolean|null>;
    restrictionType : FormControl<number|null>;
}