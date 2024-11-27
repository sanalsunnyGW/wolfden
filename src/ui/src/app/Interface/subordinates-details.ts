export interface SubordinatesDetails {
    id: number,
    employeeCode: string,
    name: string,
    email: string,
    photo: string,
    department: string,
    designation: string,
    manager:string,
    subOrdinates?:SubordinatesDetails[]
}
