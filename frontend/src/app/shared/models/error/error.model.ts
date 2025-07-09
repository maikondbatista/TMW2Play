import { HttpHeaders, HttpStatusCode } from "@angular/common/http";

export interface ErrorResponse {
	headers: HttpHeaders;
	status: HttpStatusCode;
	statusText: string;
	url: string;
	ok: boolean;
	name: string;
	message: string;
	error: string[];
}