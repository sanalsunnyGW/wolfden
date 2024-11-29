import { IEmployeeDirectoryDto } from "./iemployee-directory";


export interface IEmployeeDirectoryWithPagecount {

    employeeDirectoryDTOs: IEmployeeDirectoryDto[];
    totalPages:number;
    totalRecords:number;
}
