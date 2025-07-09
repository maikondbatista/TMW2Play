import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { TranslocoService } from "@jsverse/transloco";

@Injectable()
export class LanguageInterceptor implements HttpInterceptor {

    constructor(private languageService: TranslocoService) { }

    intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
        const lang = this.languageService.getActiveLang();
        const req = request.clone({ setHeaders: { 'Accept-Language': lang } });
        return next.handle(req);
    }
}