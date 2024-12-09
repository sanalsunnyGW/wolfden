import { ChangeDetectorRef, Component, inject } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { Gender } from '../enum/gender-enum';
import { EmploymentType } from '../enum/employment-type-enum';
import { ToastrService } from 'ngx-toastr';
import { Employee } from '../interface/iemployee';
import { IProfileForm } from '../interface/iprofile-from';
import { EmployeeService } from '../service/employee.service';
import { IEmployeeUpdate } from '../interface/iemployee-update';
import { WolfDenService } from '../service/wolf-den.service';
import { IDepartment } from '../interface/idepartment';
import { IDesignation } from '../interface/idesignation';
import { IadminForm } from '../interface/iadmin-form';
import { ImanagerForm } from '../interface/imanager-form';
import { ImanagerData } from '../interface/imanager-data';
import { IadminUpdate } from '../interface/iadmin-update';
import { IEmployeeData } from '../interface/employee-data';
import { Modal } from 'bootstrap';
import { IteamManager, IteamManagerData } from '../interface/iteam-manager';


@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [RouterLink, RouterLinkActive, ReactiveFormsModule],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.scss'
})
export class ProfileComponent {
  userForm!: FormGroup<IProfileForm>;
  teamForm!: FormGroup<IteamManager>;
  adminForm!: FormGroup<IadminForm>;
  isTeamClicked:boolean=false;
  teamId:number=0;
  employeeUpdate: IEmployeeUpdate = {} as IEmployeeUpdate;
  inDate = new Date();
  employeeData: Employee = {
    id: 0,
    rfId: '',
    employeeCode: 0,
    firstName: '',
    lastName: '',
    email: '',
    phoneNumber: '',
    dateofBirth: this.inDate,
    joiningDate: this.inDate,
    gender: 0,
    designationId: 0,
    designationName: '',
    departmentId: 0,
    departmentName: '',
    managerId: null,
    managerName: null,
    isActive: false,
    address: '',
    country: '',
    state: '',
    employmentType: 0,
    photo: ''
  };
  departmentData: IDepartment[] = [{
    id: 0,
    departmentName: ''
  }];
  designationData: IDesignation[] = [{
    id: 0,
    designationName: ''
  }];
  managerForm!: FormGroup<ImanagerForm>;
  managerData: ImanagerData[] = [{
    id: 0,
    firstName: '',
    lastName: '',
    role: ''
  }]
  myTeam: IEmployeeData[] = {} as IEmployeeData[];
  displayMyteam: boolean = false;

  wolfDenService=inject(WolfDenService)
  isDataLoaded: boolean = false;
  isDataClicked: boolean = false;
  employeeIdClicked: number = 0;
  constructor(private fb: FormBuilder, private employeeService: EmployeeService, private toastr: ToastrService, private cdr: ChangeDetectorRef) {
    this.buildForm();
  }
  private buildForm() {
    this.userForm = this.fb.group({
      firstName: new FormControl('', Validators.required),
      lastName: new FormControl(''),
      gender: new FormControl<number | null>(null, Validators.required),
      dateofBirth: new FormControl(this.inDate, Validators.required),
      phoneNumber: new FormControl('', [Validators.required, Validators.minLength(10), Validators.pattern(/^\d{1,10}$/)]),
      email: new FormControl('', Validators.email),
      address: new FormControl('', Validators.required),
      country: new FormControl('', Validators.required),
      state: new FormControl('', Validators.required),
      photo: new FormControl(''),
    }), this.adminForm = this.fb.group({
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
  selectEmployee(employeeId: number): void {
    this.employeeIdClicked = employeeId;
    this.adminForm.patchValue({
      managerId: employeeId
    });
    this.isDataClicked = true;
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

  getEmploymentType(employmentType: number): string {
    switch (employmentType) {
      case EmploymentType.Contract:
        return "Contract";
      case EmploymentType.Permanent:
        return "Permanent";
      default:
        return "Unknown"
    }
  }
  ngOnInit() {
    this.loadEmployeeData();
  }
  loadForm(employeeData: Employee) {
    this.userForm.patchValue({
      firstName: this.employeeData.firstName,
      lastName: this.employeeData.lastName,
      gender: this.employeeData.gender,
      dateofBirth: this.employeeData.dateofBirth,
      phoneNumber: this.employeeData.phoneNumber,
      email: this.employeeData.email,
      address: this.employeeData.address,
      country: this.employeeData.country,
      state: this.employeeData.state,
      photo: this.employeeData.photo,
    });

  }
  adminLoadForm(employeeData: Employee) {
    this.adminForm.patchValue({
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
  loadEmployeeData() {
    this.employeeService.getEmployeeProfile(this.wolfDenService.userId).subscribe({
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
  onImageUpload(event: any): void {
    const file = event.target.files[0];
    if (file) {
      const reader = new FileReader();
      reader.onload = (e: any) => {
        const base64Image = e.target.result;
        this.userForm.value.photo = base64Image;
      };
      reader.readAsDataURL(file);
    }
  }

  removePicture() {
    this.userForm.patchValue({
      photo: null,
    });
    this.onSubmit()
  }
  onSubmit() {
    if (this.userForm.valid) {
      const employee = this.employeeService.decodeToken();
      const updateDetails: IEmployeeUpdate = {
        id: employee.EmployeeId,
        firstName: this.userForm.value.firstName ?? '',
        lastName: this.userForm.value.lastName ?? '',
        email: this.userForm.value.email ?? '',
        phoneNumber: this.userForm.value.phoneNumber ?? '',
        gender: Number(this.userForm.value.gender),
        address: this.userForm.value.address ?? '',
        country: this.userForm.value.country ?? '',
        state: this.userForm.value.state ?? '',
        photo: this.userForm.value.photo ?? '',
        dateofBirth: this.userForm.value.dateofBirth ?? this.inDate,
      }
      this.employeeService.employeeUpdateEmployee(updateDetails).subscribe({
        next: (response: boolean) => {
          if (response == true) {
            this.toastr.success('Profile Updated Successfully')
            this.loadEmployeeData();
          }
        },
        error: (error) => {
          this.toastr.error('An error occurred while  Updating Profile')
        }
      })
    }
  }
  adminSubmit() {
    if (this.adminForm.valid) {
      const formData = this.adminForm.value;
      const params: IadminUpdate = {
        id: this.wolfDenService.userId,
        designationId: formData.designationId ?? 0,
        departmentId: Number(formData.departmentId) ?? 0,
        managerId: formData.managerId ?? null,
        isActive: formData.isActive ?? null,
        joiningDate: formData.joiningDate ?? this.inDate,
        employmentType: Number(formData.employmentType),

      }
      this.employeeService.adminUpdateEmployee(params).subscribe({
        next: (response) => {
          if (response == true) {
            this.toastr.success('Profile Updated Successfully')
            this.loadEmployeeData();
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
  displayTeam() {
    this.displayMyteam = false;
  }
  myTeamMembers() {
    if (this.adminForm.value.isActive == false) {
      const employee = this.employeeService.decodeToken();
      this.employeeService.getSubordinates(employee.EmployeeId).subscribe({
        next: (response: IEmployeeData[]) => {
          if (response.length>0) {
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
          else {
            this.adminSubmit()
          }
        },
        error: (error) => {
          this.toastr.error('An error occurred while displaying My Team')
        },
      });
    }
    else {
      this.adminSubmit()
    }
  }

teamManagerUpdate(){
  if(this.teamForm.valid){
 const formData = this.teamForm.value;
      const params: IteamManagerData = {
        id: this.teamId,
        managerId: formData.managerId ?? null,
      }
      this.employeeService.updateTeamManager(params).subscribe({
        next: (response) => {
          console.log("res",response)
          if (response == true) {
            this.toastr.success('Profile Updated Successfully')
            this.loadEmployeeData();
            this.teamForm.reset();
            this.isTeamClicked=false;
            this.myTeam.splice(0,1);
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
    this.teamId=employee;
    this.isTeamClicked = true;   
  }
}