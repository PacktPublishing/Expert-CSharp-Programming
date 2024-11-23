MyRefStruct myRefStruct = new MyRefStruct();
string s = myRefStruct.ToString();

// object o = myRefStruct; // cannot implicitly convert type 'MyRefStruct' to 'ValueType'

public ref struct MyRefStruct
{
    public int x;
    public int y;

    public override string ToString() => $"{x}, {y}";
};
