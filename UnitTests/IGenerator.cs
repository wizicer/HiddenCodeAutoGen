namespace UnitTests.Contracts
{
    using System;
    using System.Collections.Generic;

    public interface IGenerator
    {
        string Generate(string className, IList<IList<string>> parameters);
    }
}
