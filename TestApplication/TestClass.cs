using System.Collections.Generic;

namespace TestApplication;

public class TestClass
{
    public bool Boolean { get; set; }

    public List<string> ListBoxItems { get; set; } = new();
    
    public ChildClass? ChildObject { get; set; }

    public List<ChildClass> ChildObjects { get; set; } = new();
}