using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Finders
{
    public class LookupDto<T>
    {
        public int Count { get; private set; }
        public List<T> Items { get; private set; }
        public LookupDto()
        {

        }
        public LookupDto(int count, List<T> items)
        {
            Count = count;
            Items = items;
        }
    }
}
