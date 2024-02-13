import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

import { Observable, of } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';

import { Movie } from './movie';
import { MessageService } from './message.service';

@Injectable({
  providedIn: 'root'
})
export class MovieService {

  constructor(private http: HttpClient, private messageService: MessageService) { }

    /** GET movies from the server */
    getMovies(): Observable<Movie[]> {
      return this.http.get<Movie[]>('https://localhost:5001/movies')
        .pipe(
          tap(_ => this.log('fetched movies')),
          catchError(this.handleError<Movie[]>('getMovies', []))
        );
    }
  
    /** GET movie price from the server */
    getMoviePrice(id: string): Observable<string> {
      return this.http.get<string>(`https://localhost:5001/movie/${id}/price`)
        .pipe(
          tap(_ => this.log(`fetched price for movie ${id}`)),
          catchError(this.handleError<string>(`getMoviePrice id=${id}`))
        );
    }

    /**
     * Handle Http operation that failed.
     * Let the app continue.
     *
     * @param operation - name of the operation that failed
     * @param result - optional value to return as the observable result
    */
  private handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {

      // TODO: send the error to remote logging infrastructure
      console.error(error); // log to console instead

      // TODO: better job of transforming error for user consumption
      this.log(`${operation} failed: ${error.message}`);

      // Let the app keep running by returning an empty result.
      return of(result as T);
    };
  }

  /** Log a MovieService message with the MessageService */
  private log(message: string) {
    this.messageService.add(`MovieService: ${message}`);
  }
}
