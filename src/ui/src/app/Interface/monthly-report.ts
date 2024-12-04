export interface allEmployeesMonthlyReports {
    employeeId:number ,
    employeeName:string,
    noOfAbsentDays:number,
    absentDays:string,
    nofIncompleteShiftDays:number,
    incompleteShiftDays:string,
    halfDays:number,
    halfDayLeaves:string
}
export interface MonthlyReports{
    allEmployeesMonthlyReports: allEmployeesMonthlyReports[];
    pageCount: number;
}