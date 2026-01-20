using System;

public class MovieLibraryManagementSystem
{
    public static void Main(string[] args)
    {
        IFilmLibrary library = new FilmLibrary();

        // TODO: Add hardcoded test cases
        // TODO: Call library methods
        // TODO: Display output
        // TEST CASE 1
        Console.WriteLine("TEST CASE 1: Adding films");
        library.AddFilm(new Film("Inception", "Christopher Nolan", 2010));
        library.AddFilm(new Film("Interstellar", "Christopher Nolan", 2014));
        library.AddFilm(new Film("Titanic", "James Cameron", 1997));
        Console.WriteLine("Films added successfully\n");

        // TEST CASE 2
        Console.WriteLine("TEST CASE 2: Displaying all films");
        foreach (var film in library.GetFilms())
        {
            Film f = (Film)film;
            Console.WriteLine($"{f.Title} | {f.Director} | {f.Year}");
        }
        Console.WriteLine();

        // TEST CASE 3
        Console.WriteLine("TEST CASE 3: Search films by director 'Nolan'");
        foreach (var film in library.SearchFilms("Nolan"))
        {
            Film f = (Film)film;
            Console.WriteLine($"{f.Title} | {f.Director}");
        }
        Console.WriteLine();

        // TEST CASE 4
        Console.WriteLine("TEST CASE 4: Removing film 'Titanic'");
        library.RemoveFilm("Titanic");
        Console.WriteLine("Remaining films:");
        foreach (var film in library.GetFilms())
        {
            Console.WriteLine(film.Title);
        }
        Console.WriteLine();

        // TEST CASE 5
        Console.WriteLine("TEST CASE 5: Total film count");
        Console.WriteLine("Total Films: " + library.GetTotalFilmCount());
    }
}


public interface IFilm{
    public string Title {get; set;}
}
 
public class Film : IFilm
{
    public string Title { get; set; }
    public string Director { get; set; }
    public int Year { get; set; }

    public Film(string title, string director, int year)
    {
        Title = title;
        Director = director;
        Year = year;
    }
}
public interface IFilmLibrary
{
    void AddFilm(IFilm film);
    void RemoveFilm(string title);
    List<IFilm> GetFilms();
    List<IFilm> SearchFilms(string query);
    int GetTotalFilmCount();
}


public class FilmLibrary : IFilmLibrary
{
    private List<IFilm> _films = new List<IFilm>();

    public void AddFilm(IFilm film)
    {
        // TODO: Implement logic
        if(!_films.Any(f=> f.Title == film.Title)){
            _films.Add(film);
        }
    }

    public void RemoveFilm(string title)
    {
        // TODO: Implement logic
        var film = _films.FirstOrDefault(f=>f.Title == title);
        if(film!=null){
            _films.Remove(film);
        }
    }

    public List<IFilm> GetFilms()
    {
        // TODO: Implement logic
        return new List<IFilm>(_films);
    }

    public List<IFilm> SearchFilms(string query)
    {
        // TODO: Implement logic
        List<IFilm> filmsFound = new List<IFilm>();
        foreach(var item in _films){
            Film film = item as Film;
            if(film.Title.Contains(query) || film.Director.Contains(query)){
                filmsFound.Add(film);
            }
        }
        return filmsFound;
    }

    public int GetTotalFilmCount()
    {
        // TODO: Implement logic
        int result=0;
        result = _films.Count();
        return result;
    }
}
