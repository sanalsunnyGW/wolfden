import { Component, HostListener } from '@angular/core';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './header.component.html',
  styleUrl: './header.component.scss'
})
export class HeaderComponent {

  isDropdownOpen = false;

  toggleDropdown() {
    this.isDropdownOpen = !this.isDropdownOpen;
  }

  // Close dropdown when clicking outside
  @HostListener('document:click', ['$event'])
  onDocumentClick(event: MouseEvent) {
    const userMenuContainer = document.querySelector('.user-menu-container');
    if (userMenuContainer && !userMenuContainer.contains(event.target as Node)) {
      this.isDropdownOpen = false;
    }
  }

openProfile(){
// route link-------------------------------------
}

}
