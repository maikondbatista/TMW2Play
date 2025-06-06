export interface RecommendationModel {
	id: number;
	genres: string[];
	referenceGame: string;
	name: string;
	pitch: string;
	why: string;
	score: string;
    isWildcard: boolean;
}