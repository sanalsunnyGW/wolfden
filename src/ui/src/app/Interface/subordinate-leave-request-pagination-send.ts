import { LeaveRequestStatus } from "../enum/leave-request-status-enum";

export interface ISubordinateLeavePaginationSend{
    id : number|null;
    statusId : LeaveRequestStatus|null;
    pageSize : number|null;
    pageNumber : number|null;
}