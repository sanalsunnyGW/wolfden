import { ISubordinateLeaveRequest } from "./subordinate-leave-request";

export interface ISubordinateLeavePaginationReceive{
    subordinateLeaveDtosList : Array<ISubordinateLeaveRequest>;
    totalDataCount : number|null;
}