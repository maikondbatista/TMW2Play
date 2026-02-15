export interface UpcomingGameModel {
  id: number;
  name: string;
  releaseDate: any;
  genres: string[];
  platforms: string[];
  description: string;
  anticipationScore?: number;
}