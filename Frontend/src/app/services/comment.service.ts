import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { SettingsService } from './settings.service';
import { Observable, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { HttpRequestResult } from '../interfaces/http-request-result.interface';
import { Comment } from '../models/comment.model';

@Injectable({
    providedIn: 'root'
})
export class CommentService {

    private httpOptions;
    private baseUrl;

    constructor(
        private http: HttpClient,
        private settingsService: SettingsService,
    ) {
        this.httpOptions = this.settingsService.httpOptions;
        this.baseUrl = this.settingsService.baseUrl;
    }

    public getAllComment(): Observable<HttpRequestResult<Comment[]>> {
        const requestUrl: string = this.baseUrl + 'comment';
        return this.http.get<HttpRequestResult<Comment[]>>(requestUrl, this.httpOptions)
            .pipe(
                map(result => {
                    return result;
                }),
                catchError(e => {
                    return throwError(e)
                })
            );
    }

    public getCountComment(): Observable<HttpRequestResult<number>> {
        const requestUrl: string = this.baseUrl + 'comment/count';
        return this.http.get<HttpRequestResult<number>>(requestUrl, this.httpOptions)
            .pipe(
                map(result => {
                    return result;
                }),
                catchError(e => {
                    return throwError(e)
                })
            );
    }

    public register(requestData: any): Observable<HttpRequestResult<number>> {
        const requestUrl: string = this.baseUrl + 'comment/register';
        console.log(requestUrl);
        return this.http.post<HttpRequestResult<number>>(requestUrl, requestData, this.httpOptions)
            .pipe(
                map(result => {
                    return result;
                }),
                catchError(e => {
                    return throwError(e)
                })
            );
    }
}