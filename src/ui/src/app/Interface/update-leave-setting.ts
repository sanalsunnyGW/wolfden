import { FormControl } from "@angular/forms";

export interface IUpdateLeaveSetting{
    minDaysForLeaveCreditJoining : FormControl<number|null>;
    maxNegativeBalanceLimit : FormControl<number|null>;
}

export interface ILeaveUpdate{
    minDaysForLeaveCreditJoining : number;
    maxNegativeBalanceLimit : number;
}