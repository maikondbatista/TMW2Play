import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { ToastrService } from "ngx-toastr";
import { catchError, Observable, retry, throwError, timer } from "rxjs";

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
      catchError((error: any) => {
        if (error) {
          console.log(error);
          this.toastr.error('An error occurred after multiple attempts.', 'Error');
        }
        return throwError(() => error);
      })
    );
  }
}