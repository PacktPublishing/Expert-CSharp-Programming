namespace Sorting;
internal class ListSortingRacers
{
    public static void Run()
    {
        List<Racer> racers = [.. Formula1.GetRacers()];
        racers.Sort(new RacerComparer(RacerCompareType.ByLastName));
        foreach (Racer racer in racers)
        {
            Console.WriteLine(racer);
        }

       
    }
}


