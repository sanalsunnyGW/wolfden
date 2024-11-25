export interface IEmployeeData {
    id: number;
    employeeCode: number;
    firstName: string;
    lastName: string;
    email: string;
    phoneNumber: string;
    dateOfBirth: Date; 
    designationId: number;
    designationName: string;
    departmentId: number;
    departmentName: string;
    managerId: number;
    managerName: string;
    isActive: boolean;
    address: string;
    country: string;
    state: string;
    employmentType: number; 
    photo: string; 
    subordinates: IEmployeeData[]; 
  }
  