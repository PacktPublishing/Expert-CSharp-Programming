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

int[] intValues = [3, 5, 11, 22, 25, 33];
int intSum = intValues.Sum();
Console.WriteLine($"sum of int: {intSum}");

double[] doubleValues = [3.4, 4.8, 4.9, 6.3];
double doubleSum = doubleValues.Sum();
Console.WriteLine($"sum of doubles: {doubleSum}");

var average = intValues.Average<int, double>();
Console.WriteLine($"average: {average}");
