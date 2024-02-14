# MoveApp

Welcome to the Movie App! This application allows users to browse, search, and view details about movies from different providers. Currently we have two prividers **cinemaworld** and **filmworld**. The app enable user to get the cheapest price for movies from these two providers in a timely manner.

## Features

- Browse a list of movies from various providers.
- Search for movies by title, genre, or keyword.
- View detailed information about each movie, including title, release year, genre etc.
- Filter movies by provider and genre

## Technologies Used

- .NET Core
- C# Programming Language
- ASP.NET Core MVC for building web applications
- Angular for frontend development (Angular16)
- Node: Node.js v18.19.0

## Step by step to run the project

### Step1: Navigate to the API directory

```bash
cd /API
```

### Step2: Install dependencies

```bash
dotnet restore
```

### Step3: Run the application

```bash
dotnet run
```

### Step4： Open your web browser and navigate to `https://localhost:5001` to view the application.

There are three APIs

- GetMovies: https://localhost:5001/movies
  : This API consolidates movies from Cinemaworld and Filmworld providers. Movie IDs, such as 'cw0076759' and 'fw0076759', are considered equivalent.

- GetMovieDetails: https://localhost:5001/movie/{provider}/{movieID}
  : This API retrieves movie details based on the specified provider name and movie ID

- GetMoviePrice: https://localhost:5001/movie/{movieID}/price
  : This API returns the price of the movie

### Highlights

The getMovies function may not always return the movie list reliably due to potential issues with the real-world API. Therefore,

1. a retry mechanism has been implemented to recall the real-world API in case it returns an unsuccessful response code.
2. Movies from both providers are fetched asynchronously using Task.WhenAll.

The getPrice API retrieves the price attribute from the getMovieDetail API. However, due to potential unreliability in obtaining the required detail from getMovieDetail, we have implemented

1. a retry mechanism. This mechanism allows the getPrice API to be recalled up to three times, ensuring that the API returns the price if it exists.
