export interface DailyLogEntry {
    time: string;
    deviceName: string;
    direction: number;
  }
export interface DailyAttendance {
    arrivalTime:string
    departureTime:string
    insideHours:number
    outsideHours:number
    missedPunch:string
    attendanceStatusId:number
    dailyLog:DailyLogEntry[];
}
