export interface allEmployeesMonthlyReports {
    employeeId:number ,
    employeeName:string,
    noOfAbsentDays:number,
    absentDays:string,
    nofIncompleteShiftDays:number,
    incompleteShiftDays:string
}
export interface MonthlyReports{
    allEmployeesMonthlyReports: allEmployeesMonthlyReports[];
    pageCount: number;
}