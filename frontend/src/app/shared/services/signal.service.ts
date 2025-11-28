import { Injectable, signal, WritableSignal } from '@angular/core';
import { GameModel } from '../models/steam/owned-games.model';

@Injectable({
  providedIn: 'root'
})
export class SignalService {
  public allGamesSignal: WritableSignal<GameModel[]> = signal([]);
  public ownedGamesSignal: WritableSignal<GameModel[]> = signal([]);
  public playerSummarySignal: WritableSignal<any> = signal({});
  
  constructor() { }
}
