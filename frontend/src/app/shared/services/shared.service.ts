import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { CookieService } from 'ngx-cookie-service';
import { map, Observable, of, tap } from 'rxjs';
import { PlayerModel, PlayerSummaryModel } from '../models/steam/player-summary.model';
import { GameModel, OwnedGamesModel } from '../models/steam/owned-games.model';
import { SteamModel } from '../models/steam/steam.model';
import { RecommendationModel } from '../models/llm/recommendation.model';
import { PartModel } from '../models/llm/part.model';
import { UserModel } from '../models/steam/user.model';
import { HumiliateMyLibraryRequest } from '../models/llm/humiliate-my-library.model';

@Injectable({
    providedIn: 'root'
})
export class SharedService {

    private steamController: string = '/steam/';
    private geminiController: string = '/gemini/';
    private apiUrl!: string;

    constructor(private http: HttpClient, private cookie: CookieService) {
        this.apiUrl = environment.apiUrl;
    }

    public humiliateMyLibrary(request: HumiliateMyLibraryRequest): Observable<string> {
        return this.http.post(`${this.apiUrl + this.geminiController}humiliate-my-library`, request, { responseType: 'text' });
    }

    tellMeWhatToPlay(lastTwoWeeks: string[], allGames: string[]): Observable<RecommendationModel[]> {
        return this.http.post<RecommendationModel[]>(`${this.apiUrl + this.geminiController}tell-me-what-to-play`, { lastTwoWeeks, allGames });
    }

    public getUserId(username: string): Observable<string> {
        let cookie = this.cookie.get(username);
        return (cookie
            ? of(JSON.parse(this.cookie.get(username))) //: of(null)
            : this.http.get<SteamModel<UserModel>>(`${this.apiUrl + this.steamController}steam-user-id/${username}`)
                .pipe(tap((response) => this.cookie.set(username, JSON.stringify(response), { expires: 1, sameSite: 'Strict', secure: true })))
        ).pipe(
            map((response) => response?.response?.steamid || null)
        );
    }

    public playerSummary(steamId: string): Observable<PlayerModel> {
        return this.http.get<SteamModel<PlayerSummaryModel>>(`${this.apiUrl + this.steamController}player-summary/${steamId}`)
            .pipe(map((response) => {
                return response?.response?.players?.[0] || null;
            }));
    }

    public steamOwnedGames(steamId: string): Observable<GameModel[]> {
        return this.http.get<SteamModel<OwnedGamesModel>>(`${this.apiUrl + this.steamController}owned-games/${steamId}`)
            .pipe(map((response) => response?.response.games || null));;
    }
}
