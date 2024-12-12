import { ChangeDetectorRef, Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { Employee } from '../../interface/iemployee';
import { EmployeeService } from '../../service/employee.service';
import { Gender } from '../../enum/gender-enum';
import { EmploymentType } from '../../enum/employment-type-enum';
import { IadminForm } from '../../interface/iadmin-form';
import { ToastrService } from 'ngx-toastr';
import { ImanagerData } from '../../interface/imanager-data';
import { CommonModule } from '@angular/common';
import { ImanagerForm } from '../../interface/imanager-form';
import { IDepartment } from '../../interface/idepartment';
import { IDesignation } from '../../interface/idesignation';
import { IadminUpdate } from '../../interface/iadmin-update';
import { IEmployeeData } from '../../interface/employee-data';
import { Modal } from 'bootstrap';
import { IteamManager, IteamManagerData } from '../../interface/iteam-manager';

@Component({
  selector: 'app-emloyee-hierarchy-display',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './emloyee-hierarchy-display.component.html',
  styleUrl: './emloyee-hierarchy-display.component.scss'
})
export class EmloyeeHierarchyDisplayComponent {
  inDate = new Date();
  userForm!: FormGroup<IadminForm>;
  managerForm!: FormGroup<ImanagerForm>;
  isDataLoaded: boolean = false;
  isDataClicked: boolean = false;
  displayMyteam: boolean = false;
  employeeIdClicked: number = 0;
  employeeData: Employee = {} as Employee;
  myTeam: IEmployeeData[] = {} as IEmployeeData[];
  isTeamClicked: boolean = false;
  teamId: number = 0;
  managerData: ImanagerData[] = [{
    id: 0,
    firstName: '',
    lastName: '',
    role: '',
    employeeCode: 0,
  }]

  loginRole: string = '';
  departmentData: IDepartment[] = [{
    id: 0,
    departmentName: ''
  }];
  designationData: IDesignation[] = [{
    id: 0,
    designationName: ''
  }];
  constructor(private route: ActivatedRoute, private employeeService: EmployeeService, private fb: FormBuilder, private toastr: ToastrService, private cdr: ChangeDetectorRef) {
    this.buildForm();
  }
  employeeId: number = 0;
  private buildForm() {
    this.userForm = this.fb.group({
      designationId: new FormControl<number | null>(null, Validators.required),
      departmentId: new FormControl<number | null>(null, Validators.required),
      managerId: new FormControl<number | null>(null),
      isActive: new FormControl<boolean | null>(null),
      joiningDate: new FormControl(this.inDate, Validators.required),
      employmentType: new FormControl<number | null>(null, Validators.required),

    }),
      this.managerForm = this.fb.group({
        firstName: new FormControl('', Validators.required),
        lastName: new FormControl(''),

      }), this.teamForm = this.fb.group({
        managerId: new FormControl<number | null>(null,Validators.required),  
      })
  }
  profileImage() {
    if (this.employeeData.gender == Gender.Male) {
      return "male.png"
    }
    else if (this.employeeData.gender == Gender.Female) {
      return "female.jpg"
    }
    else {
      return "default.png"
    }
  }
  selectEmployee(employeeId: number): void {
    this.employeeIdClicked = employeeId;
    this.userForm.patchValue({
      managerId: employeeId
    });
    this.isDataClicked = true;
  }

  ngOnInit() {
    this.route.queryParams.subscribe(params => {
      this.employeeId = params['id'];
      if (this.employeeId) {
        this.loadEmployeeData();
        const login = this.employeeService.decodeToken()
        this.loginRole = login.role;
      }
    });
  }
  getGenderDisplay(gender: number): string {
    switch (gender) {
      case Gender.Male:
        return "Male";
      case Gender.Female:
        return "Female";
      case Gender.Other:
        return "Other";
      default:
        return "Unknown"
    }
  }
  getEmploymentType(employmentType: number): string {
    switch (employmentType) {
      case EmploymentType.Contract:
        return "Contract";
      case EmploymentType.Permanent:
        return "Permanent";
      case EmploymentType.Other:
        return "Other"
      default:
        return "Unknown"
    }
  }
  loadEmployeeData() {
    this.employeeService.getEmployeeProfile(this.employeeId).subscribe({
      next: (response: Employee) => {
        if (response) {
          this.employeeData = response;
        }
      },
      error: (error) => {
        this.toastr.error('An error occurred while  Displaying Profile')
      }
    })
  }
  loadForm(employeeData: Employee) {
    this.userForm.patchValue({
      designationId: this.employeeData.designationId,
      departmentId: this.employeeData.departmentId,
      managerId: this.employeeData.managerId,
      isActive: this.employeeData.isActive,
      joiningDate: this.employeeData.joiningDate,
      employmentType: this.employeeData.employmentType,

    }),
      this.employeeService.getAllDepartment().subscribe({
        next: (response: IDepartment[]) => {
          if (response) {
            this.departmentData = response
          }
        },
        error: (error) => {
          this.toastr.error('An error occurred while Displaying Departments')

        }
      }),
      this.employeeService.getAllDesignation().subscribe({
        next: (response: IDesignation[]) => {
          if (response) {
            this.designationData = response;
          }
        },
        error: (error) => {
          this.toastr.error('An error occurred while Displaying Designations')

        }
      })
  }

  onSubmitManager() {
    if (this.managerForm.valid) {
      const formData = this.managerForm.value;
      const params = {
        firstName: formData.firstName ?? '',
        lastName: formData.lastName ?? ''
      }
      this.employeeService.getEmployeeByName(params.firstName, params.lastName).subscribe({
        next: (response: ImanagerData[]) => {
          this.managerData = response;
          this.isDataLoaded = true;
          this.managerForm.get('firstName')?.setValue('');
          this.managerForm.get('lastName')?.setValue('');
        },
        error: (err) => {
          this.toastr.error('Error fetching managers');
        }
      });
    }
  }

  onSubmit() {
    if (this.userForm.valid) {
      const formData = this.userForm.value;
      const params: IadminUpdate = {
        id: this.employeeId,
        designationId: formData.designationId ?? 0,
        departmentId: Number(formData.departmentId) ?? 0,
        managerId: formData.managerId ?? null,
        isActive: formData.isActive ?? false,
        joiningDate: formData.joiningDate ?? this.inDate,
        employmentType: Number(formData.employmentType),
      }
      this.employeeService.adminUpdateEmployee(params).subscribe({
        next: (response) => {
          if (response == true) {
            this.toastr.success('Profile Updated Successfully')
            this.loadEmployeeData();
            this.isDataLoaded = false;
            this.isDataClicked=false;
            this.managerForm.get('firstName')?.setValue('');
            this.managerForm.get('lastName')?.setValue('');          }
          else {
            this.toastr.error('Profile Update Failed')
          }
        },
        error: (error) => {
          this.toastr.error('An error occurred while  Updating Profile')
        }
      })
    }
  }
  displayTeam() {
    this.displayMyteam = false;
  }

  myTeamMembers() {
    if (this.userForm.value.isActive == false) {
      this.employeeService.getMyTeamHierarchy(false, this.employeeData.id).subscribe({
        next: (response: IEmployeeData[]) => {
          if (response) {
            this.myTeam = response;
            this.displayMyteam = true;
            this.cdr.detectChanges();
            const myTeamElement = document.getElementById('myTeam');
            if (myTeamElement) {
              const myTeamModal = new Modal(myTeamElement);
              myTeamModal.show();
            } else {
              console.error('Modal element not found');
            }
          }
        },
        error: (error) => {
          this.toastr.error('An error occurred while displaying My Team')
        },
      });
    }
    else {
      this.onSubmit()
    }
  }
  teamForm!: FormGroup<IteamManager>;
  teamManagerUpdate() {
    if (this.teamForm.valid) {
      const formData = this.teamForm.value;
      const params: IteamManagerData = {
        id: this.teamId,
        managerId: formData.managerId ?? null,
      }
      this.employeeService.updateTeamManager(params).subscribe({
        next: (response) => {
          console.log("res", response)
          if (response == true) {
            this.toastr.success('Profile Updated Successfully')
            this.loadEmployeeData();
            this.teamForm.reset();
            this.isTeamClicked = false;
            this.myTeam.splice(0, 1);
            this.managerForm.get('firstName')?.setValue('');
            this.managerForm.get('lastName')?.setValue('');
            this.isDataLoaded = false;
            this.isDataClicked=false;
          }
          else {
            this.toastr.error('Profile Update Failed')
          }
        },
        error: (error) => {
          this.toastr.error('An error occurred while  Updating Profile')
        }
      })
    }
  }
  selectManager(employeeId: number): void {
    this.employeeIdClicked = employeeId;
    this.teamForm.patchValue({
      managerId: employeeId
    });
    this.isDataClicked = true;
  }
  selectTeam(employee: number): void {
    this.teamId = employee;
    this.isTeamClicked = true;
  }
}







