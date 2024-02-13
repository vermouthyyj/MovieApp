import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title: string = 'Movie app';
  movies: any;
  defaultPosterUrl = 'assets/default-poster.jpg';
  movie: any;

  constructor(private http: HttpClient) { }

  handlePosterError(event: any) {
    // Handle the error here, you can log it or perform any other action
    console.error('Error loading poster:', event);

    // Update the poster source to the default poster image
    event.target.src = 'assets/default-poster.jpg';
  }
}
