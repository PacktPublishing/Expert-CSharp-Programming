X x1 = new();
x1.Value = 1;

ShowAddressX("x1 in Main", x1);

ChangeX(x1);
Console.WriteLine($"the new value of x.Value: {x1.Value}");

Y y1 = new();
y1.Value = 1;

ShowAddressY("y1 in Main", y1);

ChangeY(y1);
Console.WriteLine($"the new value of y.Value: {y1.Value}");

ChangeXRef(ref x1);
Console.WriteLine($"the new value of x.Value: {x1.Value}");

ChangeYRef(ref y1);
Console.WriteLine($"the new value of y.Value: {y1.Value}");

unsafe void ChangeX(X x)
{
    ShowAddressX("x in ChangeX", x);
    x.Value = 2;
    Console.WriteLine("Create a new x instance");
    x = new();
    x.Value = 3;
    ShowAddressX("x after new instance in ChangeX", x);
}

void ChangeXRef(ref X x)
{
    ShowAddressX("x in ChangeXRef", x);
    x.Value = 2;
    x = new X();
    ShowAddressX("x after new instance in ChangeXRef", x);
    x.Value = 3;
}

unsafe void ChangeY(Y y)
{
    ShowAddressY("y in ChangeY", y);
    y.Value = 2;
    y = new();
    ShowAddressY("y after new instance in ChangeY", y);
    y.Value = 3;
}

void ChangeYRef(ref Y y)
{
    ShowAddressY("y in ChangeYRef", y);
    y.Value = 2;
    y = new Y();
    ShowAddressY("y after new instance in ChangeYRef", y);
    y.Value = 3;
}

unsafe void ShowAddressX(string name, X x)
{
#if SHOWADDRESSES
    int* p = (int*)&x;
    int* pv = &x.Value;
    Console.WriteLine($"address of {name} {(int)p:x}");
    Console.WriteLine($"address of {name}.Value {(int)pv:x}");
#endif
}

unsafe void ShowAddressY(string name, Y y)
{
#if SHOWADDRESSES
    int* p = (int*)&y;
    fixed (int* pv = &y.Value)
    {
        Console.WriteLine($"address of {name} {(int)p:x}");
        Console.WriteLine($"address of {name}.Value {(int)pv:x}");
    }
#endif
}

public struct X
{
    public int Value;
}

public class Y
{
    public int Value;
}