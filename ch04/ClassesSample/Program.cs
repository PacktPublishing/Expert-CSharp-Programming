using ClassesSample;

Film film = new("Jaws", "Steven Spielberg", 1975);

Film film2 = new("Jaws", "Steven Spielberg", 1975);

// comparing for equality
Console.WriteLine(ReferenceEquals(film, film2)); // False
Console.WriteLine(film == film2); // True
Console.WriteLine(film.Equals(film2));

// deconstructing
(_, string? director, _) = film;
Console.WriteLine(director);

// cloning and using with expressions for modification
// Film film3 = film with { Title = "E.T. the Extra-Terrestrial", Year = 1982 };
// with expressions not supported with classes
Film film3 = film.Clone(title: "E.T. the Extra-Terrestrial", year: 1982);
Console.WriteLine(film3);

ExtendedFilm film4 = new("Schindler's List", "Steven Spielberg", "Historical drama", 1993);
Console.WriteLine(film4);
