using System.Collections.Generic;

namespace EdcentralizedNet.Helpers
{
    public class CursorPagedList<T>
    {
        public string NextPageCursor { get; set; }
        public string PrevPageCursor { get; set; }
        public System.Collections.Generic.List<T> DataList { get; set; }

        public CursorPagedList()
        {
            DataList = new System.Collections.Generic.List<T>();
        }
    }
}
