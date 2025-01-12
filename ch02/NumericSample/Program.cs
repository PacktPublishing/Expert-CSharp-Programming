SimpleNumber i1 = new(3);
SimpleNumber i2 = new(39);
SimpleNumber iSum = i1 + i2;
Console.WriteLine(iSum);


SimpleNumber<int> n1 = new(3);
SimpleNumber<int> n2 = new(39);
SimpleNumber<int> nSum = n1 + n2;
Console.WriteLine(nSum);

SimpleNumber<double> d1 = new(3.4);
SimpleNumber<double> d2 = new(39.11);
SimpleNumber<double> dSum = d1 + d2;
Console.WriteLine(dSum);

int[] values = [3, 5, 11, 22, 25, 33];
int sum = values.Sum();
Console.WriteLine($"sum: {sum}");

var average = values.Average<int, double>();
Console.WriteLine($"average: {average}");
