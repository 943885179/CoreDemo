using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public interface IProductTypeService : IAutoInject
    {
        string Show(string  Name);
    }
}
