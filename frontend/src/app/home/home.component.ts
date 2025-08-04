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
    const { id, type } = this.extractSteamUserId(this.steamUser);
    this.router.navigate(['Profile', id], { queryParams: { type } });
  }

  private extractSteamUserId(url: string): { id: string, type: 'id' | 'vanity' } {
    // Profile url
    const profileMatch = url.match(/steamcommunity\.com\/profiles\/(\d+)/);
    if (profileMatch) return { id: profileMatch[1], type: 'id' };

    // Vanity url
    const vanityMatch = url.match(/steamcommunity\.com\/id\/([^/]+)/);
    if (vanityMatch) return { id: vanityMatch[1], type: 'vanity' };

    // Only user id or nickname
    const input = url.trim();
    return {
      id: input,
      type: /^\d+$/.test(input) ? 'id' : 'vanity'
    };
  }
}
