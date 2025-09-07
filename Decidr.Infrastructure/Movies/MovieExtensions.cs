using Decidr.Infrastructure.Movies.Models;
using Decidr.Operations.BusinessObjects;

namespace Decidr.Infrastructure.Movies;

public static class MovieExtensions
{
    public static Movie ToBusinessObject(this TheMovieDbMovie movie)
    {
        return new Movie
        {
            Title = movie.MovieTitle,
            Description = movie.Overview,
            ImageUrl = movie.ImageUrl,
            Popularity = movie.Popularity,
            ReleaseDate = movie.ReleaseDate == null 
                            ? null
                            : DateTimeOffset.Parse(movie.ReleaseDate)
        };
    }
}
