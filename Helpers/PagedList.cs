using System.Collections.Generic;

namespace EdcentralizedNet.Helpers
{
    public class List<T>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public System.Collections.Generic.List<T> Data { get; set; }

        public List()
        {
            Data = new System.Collections.Generic.List<T>();
        }
    }
}
