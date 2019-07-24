using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class ProductTypeService : IProductTypeService
    {
        public string Show(string Name)
        {
            return Name;
        }
    }
}
