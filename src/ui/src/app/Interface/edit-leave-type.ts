import { FormControl } from "@angular/forms";

export interface IEditLeaveTypeFormControl {
    id: FormControl<number | null>;
    maxDays: FormControl<number | null>;
    isHalfDayAllowed: FormControl<boolean | null>;
    incrementCount: FormControl<number | null>;
    incrementGapId: FormControl<number | null>;
    carryForward: FormControl<boolean | null>
    carryForwardLimit: FormControl<number | null>;
    daysCheck: FormControl<number | null>;
    daysCheckMore: FormControl<number | null>;
    daysCheckEqualOrLess: FormControl<number | null>;
    dutyDaysRequired: FormControl<number | null>;
    sandwich: FormControl<boolean | null>;
}

export interface IEditLeaveType {
    id: number | null;
    maxDays: number | null;
    isHalfDayAllowed: boolean | null;
    incrementCount: number | null;
    incrementGapId: number | null;
    carryForward: boolean | null;
    carryForwardLimit: number | null;
    daysCheck: number | null;
    daysCheckMore: number | null;
    daysCheckEqualOrLess: number | null;
    dutyDaysRequired: number | null;
    sandwich: boolean | null;
}






