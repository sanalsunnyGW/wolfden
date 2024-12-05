export interface Employee {
  id: number;
  rfId: string;
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
  managerId: number | null;
  managerName: string | null;
  isActive: boolean;
  address: string;
  country: string;
  state: string;
  employmentType: number;
  photo: string;
}

