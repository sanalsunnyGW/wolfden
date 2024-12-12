import { FormControl } from "@angular/forms";

export interface IteamManager {
    managerId: FormControl<number | null>;
}
export interface IteamManagerData {
    id: number;
    managerId: number|null;
}

