export interface ISubordinateLeaveRequest {
    leaveRequestId: number ;
    name: string ;
    employeeCode: number ;
    typeName: string ;
    halfDay: boolean ;
    applyDate: Date ;
    fromDate: Date ;
    toDate: Date ;
    description: string ;
}