import { Location } from '@angular/common';
import { Component } from '@angular/core';
import { NbAuthComponent, NbAuthService } from '@nebular/auth';

@Component({
  standalone: false,
  templateUrl: './auth-layout.component.html',
  styleUrls: ['./auth-layout.component.scss'],
})
export class AuthLayoutComponent extends NbAuthComponent {
  constructor(service: NbAuthService, location: Location) {
    super(service, location);
  }
}
