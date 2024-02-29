using System.Collections.Generic;

namespace ReactiveTestApp.Models;

public class TestOuterClass
{
    public bool Boolean { get; set; }

    public List<string> ListBoxItems { get; set; } = new();
    
    public TestInnerClass? InnerObject { get; set; }

    public List<TestInnerClass> InnerObjects { get; set; } = new();
}