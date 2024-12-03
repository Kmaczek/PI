import { Component, EventEmitter, Output } from '@angular/core';
import { IdentityService } from '../../services/identity.service';

@Component({
  selector: 'pi-login',
  templateUrl: './pi-login.component.html',
  styleUrls: ['./pi-login.component.scss'],
})
export class PiLoginComponent {
  username: string;
  password: string;
  @Output() loggedIn = new EventEmitter<boolean>();

  constructor(private identityService: IdentityService) {}

  login() {
    this.identityService
      .login(this.username, this.password)
      .subscribe(isLoggedIn => {
        this.loggedIn.emit(isLoggedIn);
    });
  }
}
