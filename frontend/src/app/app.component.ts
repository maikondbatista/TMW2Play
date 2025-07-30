import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { TranslocoModule, TranslocoService } from '@jsverse/transloco';
import { NgbDropdownModule } from '@ng-bootstrap/ng-bootstrap';
import { CookieService } from 'ngx-cookie-service';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, TranslocoModule, NgbDropdownModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  repoUrl = "https://github.com/maikondbatista/TMW2Play";
  gitHubUrl = "https://github.com/maikondbatista";
  translationPrefix = 'app.';

  constructor(private cookieService: CookieService, private transloco: TranslocoService) {
    const lang = cookieService.get('language') || transloco.getDefaultLang();
    cookieService.set('language', lang, { expires: 1, sameSite: 'Strict', secure: true });
    transloco.setActiveLang(lang);
  }

  setLanguage(lang: string) {
    this.cookieService.set('language', lang, { expires: 1, sameSite: 'Strict', secure: true });
    this.transloco.setActiveLang(lang);
  }

  get selectedLanguageImg() {
    return 'assets/img/' + this.transloco.getActiveLang() + '.png';
  }
}
