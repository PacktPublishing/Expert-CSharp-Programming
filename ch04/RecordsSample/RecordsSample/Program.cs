using RecordsSample;

Film film = new("Jaws", "Steven Spielberg", 1975);

Film film2 = new("Jaws", "Steven Spielberg", 1975);

Console.WriteLine(ReferenceEquals(film, film2)); // False
Console.WriteLine(film == film2); // True
Console.WriteLine(film.Equals(film2));

(_, string? director, _) = film;
Console.WriteLine(director);

Film film3 = film with { Title = "E.T. the Extra-Terrestrial", Year = 1982 };
Console.WriteLine(film3);

ExtendedFilm film4 = new("Schindler's List", "Steven Spielberg", "Historical drama", 1993);
Console.WriteLine(film4);
