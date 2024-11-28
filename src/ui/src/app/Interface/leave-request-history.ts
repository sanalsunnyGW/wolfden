export interface ILeaveRequestHistory {
        id:number,
        typeName: string,
        halfDay: boolean,
        fromDate: Date,
        toDate: Date,
        applyDate: Date,
        leaveRequestStatusId: number,
        description: string,
        processedBy: string
}

export interface ILeaveRequestHistoryResponse {
        leaveRequests: ILeaveRequestHistory[];
        totalPages: number;
}

