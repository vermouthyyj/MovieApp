import { Component } from '@angular/core';
import { Movie } from '../movie';
import { MovieService } from '../movie.service';

@Component({
  selector: 'app-movies',
  templateUrl: './movies.component.html',
  styleUrls: ['./movies.component.css']
})
export class MoviesComponent {
  movies: Movie[] = [];
  isLoading: boolean = true;
  isLoadingPrice: { [key: string]: boolean } = {};
  moviePrices: { [key: string]: any } = {}; 
  
  constructor(private movieService: MovieService) { }

  ngOnInit(): void {
    this.loadMovies();
  }

  loadMovies(): void { 
    this.movieService.getMovies()
      .subscribe(
        (movies: Movie[]) => { 
          this.movies = movies;
          this.loadMoviePrices();
          this.isLoading = false;
        },
        (error) => {
          console.error('Error loading movies:', error);
          this.isLoading = false;
        }
      )
  }

  loadMoviePrices(): void { 
    this.movies.forEach(movie => {
      const id = movie.id.substring(2); // Assuming each movie has an ID property
      this.isLoadingPrice[id] = true;

      this.getMoviePrice('cw' + id);
      this.getMoviePrice('fw' + id);
    });
  }

  getMoviePrice(id: string): void { 
    // Fetch movie price asynchronously
    this.movieService.getMoviePrice(id)
    .subscribe(
      (price: any) => {
        this.moviePrices[id] = price; // Store movie price in the object
        this.isLoadingPrice[id] = false;
      },
      (error) => {
        console.error(`Error loading price for movie ${id}:`, error);
        this.isLoadingPrice[id] = false;
      }
    );
  }

  handleImageError(event: any) {
    event.target.src = '../assets/default-poster.jpg';
  }
}
