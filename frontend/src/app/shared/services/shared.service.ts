import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { CookieService } from 'ngx-cookie-service';
import { map, Observable, of, tap } from 'rxjs';
import { PlayerModel, PlayerSummaryModel } from '../models/steam/player-summary.model';
import { GameModel } from '../models/steam/owned-games.motel';

@Injectable({
  providedIn: 'root'
})
export class SharedService {
  private steamController: string = '/steam/';
  private apiUrl!: string;

  constructor(private http: HttpClient, private cookie: CookieService) {
    this.apiUrl = environment.apiUrl;
  }

  public getUserId(username: string): Observable<string> {
    let cookie = !!this.cookie.get(username);
    return (cookie
      ? of(JSON.parse(this.cookie.get(username))) : of(null)
        // : this.http.get<SteamModel<UserModel>>(`${this.apiUrl + this.steamController}steam-user-id/${username}`)
        .pipe(tap((response) => this.cookie.set(username, JSON.stringify(response), { expires: 1 })))
    ).pipe(
      map((response) => response?.response?.steamid || null)
    );
  }

  public playerSummary(steamId: string): Observable<PlayerModel> {
    return of(JSON.parse(`{
    "players": [
        {
            "steamid": "76561198066505445",
            "communityvisibilitystate": 3,
            "profilestate": 1,
            "personaname": "Thisgraça",
            "profileurl": "https://steamcommunity.com/id/IAmViviOrnitier/",
            "avatar": "https://avatars.steamstatic.com/a33208ee81e5841d3a72511fe2483e23534000e2.jpg",
            "avatarmedium": "https://avatars.steamstatic.com/a33208ee81e5841d3a72511fe2483e23534000e2_medium.jpg",
            "avatarfull": "https://avatars.steamstatic.com/a33208ee81e5841d3a72511fe2483e23534000e2_full.jpg",
            "avatarhash": "a33208ee81e5841d3a72511fe2483e23534000e2",
            "lastlogoff": 1743903831,
            "personastate": 0,
            "realname": "Mochila",
            "primaryclanid": "103582791429521408",
            "timecreated": 1341416341,
            "personastateflags": 0,
            "loccountrycode": "CA"
        }
    ]
}`)).pipe(map((response) => response?.players[0] || null));
    // return this.http.get<SteamModel<playerSummaryModel>>(`${this.apiUrl + this.steamController}player-summary/${steamId}`)
    // .pipe(map((response) => response?.response.players[0] || null));;
  }

  public steamOwnedGames(steamId: string): Observable<GameModel[]> {
    return of(JSON.parse(`[
    {
                "appid": 570,
                "name": "Dota 2",
                "playtime_forever": 407104,
                "playtime_2weeks": 81,
                "img_icon_url": "0bbb630d63262dd66d2fdd0f7d37e8661a410075",
                "playtime_windows_forever": 76973,
                "playtime_mac_forever": 0,
                "playtime_linux_forever": 0,
                "playtime_deck_forever": 0,
                "rtime_last_played": 1744958835,
                "content_descriptorids": [
                    5
                ],
                "playtime_disconnected": 32
            },
    {
        "appid": 6880,
        "name": "Just Cause",
        "playtime_2weeks": 155,
        "playtime_forever": 0,
        "img_icon_url": "aeaef41cac61d12ef93cf5eddc86859b1f0c73fa",
        "playtime_windows_forever": 0,
        "playtime_mac_forever": 0,
        "playtime_linux_forever": 0,
        "playtime_deck_forever": 0,
        "rtime_last_played": 0,
        "content_descriptorids": null,
        "playtime_disconnected": 0
    },
    {
        "appid": 6910,
        "name": "Deus Ex: Game of the Year Edition",
        "playtime_2weeks": 0,
        "playtime_forever": 0,
        "img_icon_url": "03c8e5e89d83c536b44798e77ead5d813103991f",
        "playtime_windows_forever": 0,
        "playtime_mac_forever": 0,
        "playtime_linux_forever": 0,
        "playtime_deck_forever": 0,
        "rtime_last_played": 0,
        "content_descriptorids": null,
        "playtime_disconnected": 0
    },
    {
        "appid": 6920,
        "name": "Deus Ex: Invisible War",
        "playtime_2weeks": 200,
        "playtime_forever": 0,
        "img_icon_url": "8feb3c4e692882dbd1f30f3a8bfe89b9928c68c1",
        "playtime_windows_forever": 0,
        "playtime_mac_forever": 0,
        "playtime_linux_forever": 0,
        "playtime_deck_forever": 0,
        "rtime_last_played": 0,
        "content_descriptorids": null,
        "playtime_disconnected": 0
    },
    {
        "appid": 6980,
        "name": "Thief: Deadly Shadows",
        "playtime_2weeks": 0,
        "playtime_forever": 0,
        "img_icon_url": "d396bd55b366909ffda529158fd91513e4098a53",
        "playtime_windows_forever": 0,
        "playtime_mac_forever": 0,
        "playtime_linux_forever": 0,
        "playtime_deck_forever": 0,
        "rtime_last_played": 0,
        "content_descriptorids": null,
        "playtime_disconnected": 0
    },
    {
        "appid": 7000,
        "name": "Tomb Raider: Legend",
        "playtime_2weeks": 0,
        "playtime_forever": 0,
        "img_icon_url": "df2e4400b953ab62c43ddd590684ecafd339134d",
        "playtime_windows_forever": 0,
        "playtime_mac_forever": 0,
        "playtime_linux_forever": 0,
        "playtime_deck_forever": 0,
        "rtime_last_played": 0,
        "content_descriptorids": null,
        "playtime_disconnected": 0
    },
    {
        "appid": 7010,
        "name": "Project: Snowblind",
        "playtime_2weeks": 0,
        "playtime_forever": 0,
        "img_icon_url": "2c591be8d5a0827df772bf136cdc0fa4176dfe5f",
        "playtime_windows_forever": 0,
        "playtime_mac_forever": 0,
        "playtime_linux_forever": 0,
        "playtime_deck_forever": 0,
        "rtime_last_played": 0,
        "content_descriptorids": null,
        "playtime_disconnected": 0
    },
    {
        "appid": 8000,
        "name": "Tomb Raider: Anniversary",
        "playtime_2weeks": 0,
        "playtime_forever": 0,
        "img_icon_url": "13243b33c45bbde67ed6538641173e556548aac3",
        "playtime_windows_forever": 0,
        "playtime_mac_forever": 0,
        "playtime_linux_forever": 0,
        "playtime_deck_forever": 0,
        "rtime_last_played": 0,
        "content_descriptorids": null,
        "playtime_disconnected": 0
    },
    {
        "appid": 8080,
        "name": "Kane & Lynch: Dead Men",
        "playtime_2weeks": 0,
        "playtime_forever": 0,
        "img_icon_url": "e6f76146b9573fd904dbde49578df5063fa747f9",
        "playtime_windows_forever": 0,
        "playtime_mac_forever": 0,
        "playtime_linux_forever": 0,
        "playtime_deck_forever": 0,
        "rtime_last_played": 0,
        "content_descriptorids": null,
        "playtime_disconnected": 0
    },
    {
        "appid": 8100,
        "name": "Conflict: Denied Ops",
        "playtime_2weeks": 0,
        "playtime_forever": 0,
        "img_icon_url": "5828e44292de181667cfbffb4b8e7a0cba81463c",
        "playtime_windows_forever": 0,
        "playtime_mac_forever": 0,
        "playtime_linux_forever": 0,
        "playtime_deck_forever": 0,
        "rtime_last_played": 0,
        "content_descriptorids": null,
        "playtime_disconnected": 0
    },
    {
        "appid": 8140,
        "name": "Tomb Raider: Underworld",
        "playtime_2weeks": 0,
        "playtime_forever": 0,
        "img_icon_url": "7182f90258cee15aed17cd0318a270276c21b3b3",
        "playtime_windows_forever": 0,
        "playtime_mac_forever": 0,
        "playtime_linux_forever": 0,
        "playtime_deck_forever": 0,
        "rtime_last_played": 0,
        "content_descriptorids": null,
        "playtime_disconnected": 0
    },
    {
        "appid": 8170,
        "name": "Battlestations: Pacific",
        "playtime_2weeks": 0,
        "playtime_forever": 0,
        "img_icon_url": "1478f897e2f800b26a85a9ea259096111091fea1",
        "playtime_windows_forever": 0,
        "playtime_mac_forever": 0,
        "playtime_linux_forever": 0,
        "playtime_deck_forever": 0,
        "rtime_last_played": 0,
        "content_descriptorids": null,
        "playtime_disconnected": 0
    },
    {
        "appid": 35000,
        "name": "Mini Ninjas",
        "playtime_2weeks": 0,
        "playtime_forever": 0,
        "img_icon_url": "02f702c604e0692b8185f1b0e94e013d0a242d3a",
        "playtime_windows_forever": 0,
        "playtime_mac_forever": 0,
        "playtime_linux_forever": 0,
        "playtime_deck_forever": 0,
        "rtime_last_played": 0,
        "content_descriptorids": null,
        "playtime_disconnected": 0
    },
    {
        "appid": 34600,
        "name": "Order of War",
        "playtime_2weeks": 0,
        "playtime_forever": 0,
        "img_icon_url": "b2535fceaeabe10f4f5022c425d320c722d366ad",
        "playtime_windows_forever": 0,
        "playtime_mac_forever": 0,
        "playtime_linux_forever": 0,
        "playtime_deck_forever": 0,
        "rtime_last_played": 0,
        "content_descriptorids": null,
        "playtime_disconnected": 0
    },
    {
        "appid": 35070,
        "name": "Flora's Fruit Farm",
        "playtime_2weeks": 0,
        "playtime_forever": 0,
        "img_icon_url": "d392ea74c7186341486ea31fa4712ef7901a7623",
        "playtime_windows_forever": 0,
        "playtime_mac_forever": 0,
        "playtime_linux_forever": 0,
        "playtime_deck_forever": 0,
        "rtime_last_played": 0,
        "content_descriptorids": null,
        "playtime_disconnected": 0
    },
    {
        "appid": 22370,
        "name": "Fallout 3 - Game of the Year Edition",
        "playtime_2weeks": 0,
        "playtime_forever": 0,
        "img_icon_url": "21d7090bdea8f6685ca730850b7b55acfdb92732",
        "playtime_windows_forever": 0,
        "playtime_mac_forever": 0,
        "playtime_linux_forever": 0,
        "playtime_deck_forever": 0,
        "rtime_last_played": 0,
        "content_descriptorids": null,
        "playtime_disconnected": 0
    },
    {
        "appid": 40100,
        "name": "Supreme Commander 2",
        "playtime_2weeks": 0,
        "playtime_forever": 0,
        "img_icon_url": "96e22cb9c9b063c9f0398f248fef850a679ced5a",
        "playtime_windows_forever": 0,
        "playtime_mac_forever": 0,
        "playtime_linux_forever": 0,
        "playtime_deck_forever": 0,
        "rtime_last_played": 0,
        "content_descriptorids": null,
        "playtime_disconnected": 0
    },
    {
        "appid": 8190,
        "name": "Just Cause 2",
        "playtime_2weeks": 0,
        "playtime_forever": 0,
        "img_icon_url": "73582e392a2b9413fe93b011665a5b9cf26ff175",
        "playtime_windows_forever": 0,
        "playtime_mac_forever": 0,
        "playtime_linux_forever": 0,
        "playtime_deck_forever": 0,
        "rtime_last_played": 0,
        "content_descriptorids": null,
        "playtime_disconnected": 0
    },
    {
        "appid": 22380,
        "name": "Fallout: New Vegas",
        "playtime_2weeks": 0,
        "playtime_forever": 0,
        "img_icon_url": "1711fd8c46d739feec76bd4a64eaeeca5b87e3a7",
        "playtime_windows_forever": 0,
        "playtime_mac_forever": 0,
        "playtime_linux_forever": 0,
        "playtime_deck_forever": 0,
        "rtime_last_played": 0,
        "content_descriptorids": null,
        "playtime_disconnected": 0
    },
    {
        "appid": 35130,
        "name": "Lara Croft and the Guardian of Light",
        "playtime_2weeks": 0,
        "playtime_forever": 0,
        "img_icon_url": "b06fc7740d5e2e02ffd47fec3c56f7d38534f598",
        "playtime_windows_forever": 0,
        "playtime_mac_forever": 0,
        "playtime_linux_forever": 0,
        "playtime_deck_forever": 0,
        "rtime_last_played": 0,
        "content_descriptorids": null,
        "playtime_disconnected": 0
    },
    {
        "appid": 20920,
        "name": "The Witcher 2: Assassins of Kings Enhanced Edition",
        "playtime_2weeks": 0,
        "playtime_forever": 0,
        "img_icon_url": "62dd5c627664df1bcabc47727c7dcd7ccab353e9",
        "playtime_windows_forever": 0,
        "playtime_mac_forever": 0,
        "playtime_linux_forever": 0,
        "playtime_deck_forever": 0,
        "rtime_last_played": 0,
        "content_descriptorids": [
            1,
            2,
            5
        ],
        "playtime_disconnected": 0
    },
    {
        "appid": 39160,
        "name": "Dungeon Siege III",
        "playtime_2weeks": 0,
        "playtime_forever": 0,
        "img_icon_url": "c9dba11084b0a049b273819dba062ea89ecf3c05",
        "playtime_windows_forever": 0,
        "playtime_mac_forever": 0,
        "playtime_linux_forever": 0,
        "playtime_deck_forever": 0,
        "rtime_last_played": 0,
        "content_descriptorids": null,
        "playtime_disconnected": 0
    },
    {
        "appid": 203630,
        "name": "Warlock - Master of the Arcane",
        "playtime_2weeks": 0,
        "playtime_forever": 460,
        "img_icon_url": "540410e6c4528bb8e2ac38d54e6f7e49129a12bb",
        "playtime_windows_forever": 0,
        "playtime_mac_forever": 0,
        "playtime_linux_forever": 0,
        "playtime_deck_forever": 0,
        "rtime_last_played": 1507162310,
        "content_descriptorids": null,
        "playtime_disconnected": 0
    },
    {
        "appid": 211780,
        "name": "Conflict Desert Storm",
        "playtime_2weeks": 0,
        "playtime_forever": 0,
        "img_icon_url": "b76250da53c98f53cd2ec0491342b2f96dd4f817",
        "playtime_windows_forever": 0,
        "playtime_mac_forever": 0,
        "playtime_linux_forever": 0,
        "playtime_deck_forever": 0,
        "rtime_last_played": 0,
        "content_descriptorids": null,
        "playtime_disconnected": 0
    },
    {
        "appid": 211600,
        "name": "Thief Gold",
        "playtime_2weeks": 0,
        "playtime_forever": 0,
        "img_icon_url": "70d127f48cd8a1bc6e3cddfdf14b791f45b5f086",
        "playtime_windows_forever": 0,
        "playtime_mac_forever": 0,
        "playtime_linux_forever": 0,
        "playtime_deck_forever": 0,
        "rtime_last_played": 0,
        "content_descriptorids": null,
        "playtime_disconnected": 0
    },
    {
        "appid": 211740,
        "name": "Thief™ II: The Metal Age",
        "playtime_2weeks": 0,
        "playtime_forever": 0,
        "img_icon_url": "86a6e81966e727e1fa48f7e3c1057b60741a56c7",
        "playtime_windows_forever": 0,
        "playtime_mac_forever": 0,
        "playtime_linux_forever": 0,
        "playtime_deck_forever": 0,
        "rtime_last_played": 0,
        "content_descriptorids": null,
        "playtime_disconnected": 0
    },
    {
        "appid": 206440,
        "name": "To the Moon",
        "playtime_2weeks": 0,
        "playtime_forever": 0,
        "img_icon_url": "6e29eb4076a6253fdbccb987a2a21746d2df54d7",
        "playtime_windows_forever": 0,
        "playtime_mac_forever": 0,
        "playtime_linux_forever": 0,
        "playtime_deck_forever": 0,
        "rtime_last_played": 0,
        "content_descriptorids": null,
        "playtime_disconnected": 0
    },
    {
        "appid": 209080,
        "name": "Guns of Icarus Online",
        "playtime_2weeks": 0,
        "playtime_forever": 0,
        "img_icon_url": "968e8c0b7a55f0229392278123dfd486140c9421",
        "playtime_windows_forever": 0,
        "playtime_mac_forever": 0,
        "playtime_linux_forever": 0,
        "playtime_deck_forever": 0,
        "rtime_last_played": 0,
        "content_descriptorids": null,
        "playtime_disconnected": 0
    },
    {
        "appid": 49520,
        "name": "Borderlands 2",
        "playtime_2weeks": 0,
        "playtime_forever": 1895,
        "img_icon_url": "a3f4945226e69b6196074df4c776e342d3e5a3be",
        "playtime_windows_forever": 1895,
        "playtime_mac_forever": 0,
        "playtime_linux_forever": 0,
        "playtime_deck_forever": 0,
        "rtime_last_played": 1605975714,
        "content_descriptorids": null,
        "playtime_disconnected": 0
    },
    {
        "appid": 224300,
        "name": "Legacy of Kain: Defiance",
        "playtime_2weeks": 0,
        "playtime_forever": 0,
        "img_icon_url": "ca46370c6c55bed7a3bcbd4eb2c684a2fd1b50e5",
        "playtime_windows_forever": 0,
        "playtime_mac_forever": 0,
        "playtime_linux_forever": 0,
        "playtime_deck_forever": 0,
        "rtime_last_played": 0,
        "content_descriptorids": null,
        "playtime_disconnected": 0
    },
    {
        "appid": 224920,
        "name": "Legacy of Kain: Soul Reaver",
        "playtime_2weeks": 0,
        "playtime_forever": 0,
        "img_icon_url": "6c32e0d044dde141a46297b486e08c0fa9ccf3b8",
        "playtime_windows_forever": 0,
        "playtime_mac_forever": 0,
        "playtime_linux_forever": 0,
        "playtime_deck_forever": 0,
        "rtime_last_played": 0,
        "content_descriptorids": null,
        "playtime_disconnected": 0
    },
    {
        "appid": 224940,
        "name": "Legacy of Kain: Soul Reaver 2 (2001)",
        "playtime_2weeks": 0,
        "playtime_forever": 0,
        "img_icon_url": "ae768d1809857e57d67eb3238e01e9ac31a60187",
        "playtime_windows_forever": 0,
        "playtime_mac_forever": 0,
        "playtime_linux_forever": 0,
        "playtime_deck_forever": 0,
        "rtime_last_played": 0,
        "content_descriptorids": null,
        "playtime_disconnected": 0
    },
    {
        "appid": 224960,
        "name": "Tomb Raider I",
        "playtime_2weeks": 0,
        "playtime_forever": 0,
        "img_icon_url": "ba7d3a70fa6300f72bef961595addd1ff6b17a36",
        "playtime_windows_forever": 0,
        "playtime_mac_forever": 0,
        "playtime_linux_forever": 0,
        "playtime_deck_forever": 0,
        "rtime_last_played": 0,
        "content_descriptorids": null,
        "playtime_disconnected": 0
    },
    {
        "appid": 225300,
        "name": "Tomb Raider II",
        "playtime_2weeks": 0,
        "playtime_forever": 0,
        "img_icon_url": "c8c783d30296c155e92509e784b9dc1eafe23320",
        "playtime_windows_forever": 0,
        "playtime_mac_forever": 0,
        "playtime_linux_forever": 0,
        "playtime_deck_forever": 0,
        "rtime_last_played": 0,
        "content_descriptorids": null,
        "playtime_disconnected": 0
    },
    {
        "appid": 225320,
        "name": "Tomb Raider III: Adventures of Lara Croft",
        "playtime_2weeks": 0,
        "playtime_forever": 0,
        "img_icon_url": "fc783ec72af280a4e985155b1d006cd0049f833f",
        "playtime_windows_forever": 0,
        "playtime_mac_forever": 0,
        "playtime_linux_forever": 0,
        "playtime_deck_forever": 0,
        "rtime_last_played": 0,
        "content_descriptorids": null,
        "playtime_disconnected": 0
    },
    {
        "appid": 224980,
        "name": "Tomb Raider: The Last Revelation (1999)",
        "playtime_2weeks": 0,
        "playtime_forever": 0,
        "img_icon_url": "ee055131684cb66da1a3f97373a6ca8acfe2e651",
        "playtime_windows_forever": 0,
        "playtime_mac_forever": 0,
        "playtime_linux_forever": 0,
        "playtime_deck_forever": 0,
        "rtime_last_played": 0,
        "content_descriptorids": null,
        "playtime_disconnected": 0
    },
    {
        "appid": 225000,
        "name": "Tomb Raider: Chronicles (2000)",
        "playtime_2weeks": 0,
        "playtime_forever": 0,
        "img_icon_url": "24501dd872679dd9b2842052d0dcf866da6a0981",
        "playtime_windows_forever": 0,
        "playtime_mac_forever": 0,
        "playtime_linux_forever": 0,
        "playtime_deck_forever": 0,
        "rtime_last_played": 0,
        "content_descriptorids": null,
        "playtime_disconnected": 0
    },
    {
        "appid": 225020,
        "name": "Tomb Raider (VI): The Angel of Darkness (2003)",
        "playtime_2weeks": 0,
        "playtime_forever": 0,
        "img_icon_url": "a177f35b4a171ec04bcaa9c98c3664f00b10786e",
        "playtime_windows_forever": 0,
        "playtime_mac_forever": 0,
        "playtime_linux_forever": 0,
        "playtime_deck_forever": 0,
        "rtime_last_played": 0,
        "content_descriptorids": null,
        "playtime_disconnected": 0
    },
    {
        "appid": 238960,
        "name": "Path of Exile",
        "playtime_2weeks": 0,
        "playtime_forever": 183,
        "img_icon_url": "1110764aac57ac28d7ffd8c43071c75d5482a9c9",
        "playtime_windows_forever": 183,
        "playtime_mac_forever": 0,
        "playtime_linux_forever": 0,
        "playtime_deck_forever": 0,
        "rtime_last_played": 1596335300,
        "content_descriptorids": [
            1,
            2,
            5
        ],
        "playtime_disconnected": 0
    },
    {
        "appid": 217200,
        "name": "Worms Armageddon",
        "playtime_2weeks": 0,
        "playtime_forever": 10,
        "img_icon_url": "68c6d17bde9c578d91dd1e207b58eb4d8308ce40",
        "playtime_windows_forever": 0,
        "playtime_mac_forever": 0,
        "playtime_linux_forever": 0,
        "playtime_deck_forever": 0,
        "rtime_last_played": 1526216600,
        "content_descriptorids": null,
        "playtime_disconnected": 0
    },
    {
        "appid": 39200,
        "name": "Dungeon Siege 2",
        "playtime_2weeks": 0,
        "playtime_forever": 0,
        "img_icon_url": "f5902b5bb4057129ab67ad9f202a254e1f0cc0fb",
        "playtime_windows_forever": 0,
        "playtime_mac_forever": 0,
        "playtime_linux_forever": 0,
        "playtime_deck_forever": 0,
        "rtime_last_played": 0,
        "content_descriptorids": null,
        "playtime_disconnected": 0
    },
    {
        "appid": 39190,
        "name": "Dungeon Siege",
        "playtime_2weeks": 0,
        "playtime_forever": 0,
        "img_icon_url": "93bc26efb692cca3964ec8ba424572f5e1bf0e70",
        "playtime_windows_forever": 0,
        "playtime_mac_forever": 0,
        "playtime_linux_forever": 0,
        "playtime_deck_forever": 0,
        "rtime_last_played": 0,
        "content_descriptorids": null,
        "playtime_disconnected": 0
    }]`)).pipe(map((response) => response || null));
    // return this.http.get<SteamModel<OwnedGamesModel>>(`${this.apiUrl + this.steamController}owned-games/${steamId}`)
    // .pipe(map((response) => response?.response || null));;
  }
}
