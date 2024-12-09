import {FormControl } from "@angular/forms";

export interface IroleForm {
    id: FormControl<number | null>;
    role: FormControl<string | null>;

}

export interface IRole {
    id: number | null ;
    role: string | null ;
}
