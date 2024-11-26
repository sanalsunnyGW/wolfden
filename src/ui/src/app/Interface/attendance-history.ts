import { FormControl } from "@angular/forms";

export interface MonthlyHistory {
    date:Date,
    arrivalTime: string,
    departureTime: string,
    insideDuration: number,
    outsideDuration: number,
    missedPunch: string,
    attendanceStatusId:number
}
export interface AttendanceHistory{
    attendancehistory: MonthlyHistory[];
    totalpages: number;
}