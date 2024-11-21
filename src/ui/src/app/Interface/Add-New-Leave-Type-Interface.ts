import { FormControl } from "@angular/forms";

export interface IAddNewLeaveType
{
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