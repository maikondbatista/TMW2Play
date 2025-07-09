import { ApplicationConfig, provideZoneChangeDetection, isDevMode } from '@angular/core';
import { provideRouter, withHashLocation } from '@angular/router';

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

export const appConfig: ApplicationConfig = {
  providers:
    [
      provideZoneChangeDetection({ eventCoalescing: true }),
      provideRouter(routes, withHashLocation()),
      provideToastr({ positionClass: 'toast-bottom-right' }),
      provideCharts({ registerables: [PieController, ArcElement, Colors, Tooltip, Legend] }),
      provideHttpClient(withInterceptorsFromDi()),
      provideHttpClient(),
      provideAnimations(),
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
