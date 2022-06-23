using System.Collections.Generic;

namespace EdcentralizedNet.Models
{
    public class CursorPagedList<T>
    {
        public string NextPageCursor { get; set; }
        public string PrevPageCursor { get; set; }
        public List<T> DataList { get; set; }

        public CursorPagedList()
        {
            DataList = new List<T>();
        }
    }
}
