import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ENABLE_CORS } from "../config/config";
import { ApiService } from './api.service';
import { AuthService } from "./auth.service";

@Injectable()

export class ApiInterceptor implements HttpInterceptor {
    constructor(private authService: AuthService, private apiService: ApiService) { }

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
      if (ENABLE_CORS) {
        req = req.clone({
          withCredentials: true
        });
      }
      return next.handle(req);
    }
}
