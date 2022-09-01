import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Params, Router, RouterStateSnapshot } from '@angular/router';
import { AuthService } from '../services/auth.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(private authService: AuthService, private router: Router) { }

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): boolean {
      let loggedIn = this.authService.isLoggedIn();
      if (!loggedIn) {
        let params: Params = {
          'ReturnUrl': location.pathname
        }
        this.router.navigate(['login'], { queryParams: params });
      }
      return loggedIn;
  }

}
