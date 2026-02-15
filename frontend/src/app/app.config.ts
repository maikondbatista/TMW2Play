import { ApplicationConfig, provideZoneChangeDetection, isDevMode, LOCALE_ID } from '@angular/core';
import { provideRouter } from '@angular/router';
import { registerLocaleData, DatePipe } from '@angular/common';
import ptBr from '@angular/common/locales/pt';
import enUs from '@angular/common/locales/en';

import { routes } from './app.routes';
import { HTTP_INTERCEPTORS, provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import { provideCharts } from 'ng2-charts';
import { ErrorInterceptor } from './shared/interceptors/error.interceptor';
import { ArcElement, Colors, Legend, PieController, Tooltip } from 'chart.js';
import { provideToastr } from 'ngx-toastr';
import { provideAnimations } from '@angular/platform-browser/animations';
import { TranslocoHttpLoader } from './transloco-loader';
import { provideTransloco } from '@jsverse/transloco';
import { LanguagesConstant } from './shared/constants/language/languages.constant';
import { LanguageInterceptor } from './shared/interceptors/language.interceptor';

// Register locales for date pipe
registerLocaleData(ptBr);
registerLocaleData(enUs);

export const appConfig: ApplicationConfig = {
  providers:
    [
      provideZoneChangeDetection({ eventCoalescing: true }),
      provideRouter(routes),
      provideToastr({ positionClass: 'toast-bottom-right' }),
      provideCharts({ registerables: [PieController, ArcElement, Colors, Tooltip, Legend] }),
      provideHttpClient(withInterceptorsFromDi()),
      provideHttpClient(),
      provideAnimations(),
      DatePipe,
      {
        provide: HTTP_INTERCEPTORS,
        useClass: ErrorInterceptor,
        multi: true
      },
      {
        provide: HTTP_INTERCEPTORS,
        useClass: LanguageInterceptor,
        multi: true
      },
      {
        provide: LOCALE_ID,
        useValue: LanguagesConstant.enUs
      },
      provideTransloco({
        config: {
          availableLangs: LanguagesConstant.supportedLanguages,
          defaultLang: LanguagesConstant.enUs,
          reRenderOnLangChange: true,
          prodMode: !isDevMode(),
        },
        loader: TranslocoHttpLoader
      })
    ]
};

