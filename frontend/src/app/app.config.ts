import { ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { HTTP_INTERCEPTORS, provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import { provideCharts } from 'ng2-charts';
import { ErrorInterceptor } from './shared/interceptors/error.interceptor';
import { ArcElement, Colors, Legend, PieController, Tooltip } from 'chart.js';
import { provideToastr } from 'ngx-toastr';

export const appConfig: ApplicationConfig = {
  providers:
    [
      provideZoneChangeDetection({ eventCoalescing: true }),
      provideRouter(routes),
      provideToastr(),
      provideCharts({ registerables: [PieController, ArcElement, Colors, Tooltip, Legend] }),
      provideHttpClient(withInterceptorsFromDi()),
      {
        provide: HTTP_INTERCEPTORS,
        useClass: ErrorInterceptor,
        multi: true
      }
    ]
};
