import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { TranslocoModule } from '@jsverse/transloco';

@Component({
  selector: 'app-home',
  imports: [FormsModule, TranslocoModule],
  standalone: true,
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss'
})
export class HomeComponent {
  constructor(private router: Router) { }
  protected title = "Tell me what to play";
  protected steamUser!: string;
  public UserProfile() {
    const steamUser = this.extractSteamUserId(this.steamUser);
    this.router.navigate(['Profile', steamUser]);
  }

   private extractSteamUserId(url: string): string {
    const match = url.match(/steamcommunity\.com\/id\/([^/]+)/);
    return match ? match[1] : url;
  }
}
