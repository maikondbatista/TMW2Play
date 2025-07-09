import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest, HttpStatusCode } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { ToastrService } from "ngx-toastr";
import { catchError, Observable, retry, throwError, timer } from "rxjs";
import { ErrorResponse } from "../models/error/error.model";

export const maxRetries = 1;
export const delayMs = 1000;
@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

  constructor(private toastr: ToastrService) { }

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    return next.handle(request).pipe(
      retry({
        count: maxRetries,
        delay: () => timer(delayMs)
      }),
      catchError((error: ErrorResponse) => {
        if (error?.status === HttpStatusCode.BadRequest) {
          error.error.map((err: string) => this.toastr.error(err, 'Error'));
        }
        return throwError(() => error);
      })
    );
  }
}