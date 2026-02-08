import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest, HttpStatusCode } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { ToastrService } from "ngx-toastr";
import { catchError, Observable, retry, throwError, timer } from "rxjs";
import { ErrorResponse } from "../models/error/error.model";
import { TranslocoService } from "@jsverse/transloco";

export const maxRetries = 1;
export const delayMs = 1000;
@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

  constructor(private toastr: ToastrService, private translationService: TranslocoService) { }

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
        else {
          this.toastr.error(this.translationService.translate('error.unexpected'), this.translationService.translate('error.title'));
        }
        return throwError(() => error);
      })
    );
  }
}