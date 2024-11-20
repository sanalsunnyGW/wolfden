export interface ILeaveRequestHistory {
        typeName: string,
        halfDay: boolean,
        fromDate: Date,
        toDate: Date,
        applyDate: Date,
        leaveRequestStatus:number, 
        description:string,
        processedBy:string  
    
}
