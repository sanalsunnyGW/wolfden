import { FormControl } from "@angular/forms";

export interface IUpdateLeaveSettingFormControl{
    adminId : FormControl<number|null>;
    minDaysForLeaveCreditJoining : FormControl<number|null>;
    maxNegativeBalanceLimit : FormControl<number|null>;
}

export interface ILeaveUpdate{
    minDaysForLeaveCreditJoining : number;
    maxNegativeBalanceLimit : number;
}

export interface IUpdateLeaveSetting{
    adminId : number|null;
    minDaysForLeaveCreditJoining : number|null;
    maxNegativeBalanceLimit : number|null;
}