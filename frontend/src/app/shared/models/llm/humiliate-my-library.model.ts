export interface HumiliateMyLibraryRequest {
  lastTwoWeeks: HumiliateMyLibraryGameRequest[];
  allGames: HumiliateMyLibraryGameRequest[];
}

export interface HumiliateMyLibraryGameRequest {
  game: string;
  time?: number;
}