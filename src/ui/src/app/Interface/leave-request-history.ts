export interface ILeaveRequestHistory {

        typeName: string,
        halfDay: boolean,
        fromDate: Date,
        toDate: Date,
        applyDate: Date,
        leaveRequestStatus:number, //takes an enum value to get the request status
        description:string,
        processedBy:string   //takes the employee(manager) name who procesed it
    
}
