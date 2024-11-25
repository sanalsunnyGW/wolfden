export interface IEmployeeDirectoryDto {
    employeeCode: number;
    firstName: string;
    lastName: string;
    email: string;
    phoneNumber: string;
    dateofBirth: Date;
    joiningDate: Date;
    gender: number;
    designationId: number;
    designationName: string;
    departmentId: number;
    departmentName: string;
    managerId: number;
    managerName: string;
    isActive: boolean;
}
