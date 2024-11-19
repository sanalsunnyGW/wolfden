export interface Employee {
    id: number | null;
    employeeCode: number | null;
    firstName: string | null;
    lastName: string | null;
    email: string | null;
    phoneNumber: string | null;
    dateofBirth: Date | null;
    designationId: number | null;
    departmentId: number | null;
    managerId: number | null;
    isActive: boolean | null;
    subordinates: Employee[];  
  }
  