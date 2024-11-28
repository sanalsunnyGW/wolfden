import { FormControl } from "@angular/forms";

export interface IEditLeaveType 
{
    id : FormControl<number|null>;
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









