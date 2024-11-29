import { LeaveRequestStatus } from "../enum/leave-request-status-enum";

export interface IApproveRejectLeave{
    superiorId : number;
    leaveRequestId : number |null;
    statusId : LeaveRequestStatus |null;
}