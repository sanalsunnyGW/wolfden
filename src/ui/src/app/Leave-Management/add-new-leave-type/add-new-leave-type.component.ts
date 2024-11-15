// import { Component } from '@angular/core';
// import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
// import { IAddNewLeaveType } from '../../Interface/Add-New-Leave-Type-Interface';
// @Component({
//   selector: 'app-add-new-leave-type',
//   standalone: true,
//   imports: [ReactiveFormsModule],
//   templateUrl: './add-new-leave-type.component.html',
//   styleUrl: './add-new-leave-type.component.scss'
// })
// export class AddNewLeaveTypeComponent {

//   addNewLeaveType : FormGroup<IAddNewLeaveType>
//   constructor(private fb: FormBuilder) {
//     this.addNewLeaveType = this.fb.group<IAddNewLeaveType>({
//       typeName: new FormControl(null,Validators.required),
//       maxDays: new FormControl(null),
//       halfDay: new FormControl(false,[Validators.required]),
//       incrementCount: new FormControl(0),
//       incrementGap: new FormControl(1),
//       carryforward: new FormControl(false),
//       carryforwardLimit: new FormControl(null),
//       daysCheck: new FormControl(null),
//       daysCheckMore: new FormControl(null),
//       daysCheckEqualOrLess: new FormControl(null),
//       dutyDaysRequired: new FormControl(0),
//       hidden: new FormControl(false,Validators.required),
//       restrictionType: new FormControl(2),
//     });
//   }



// }
