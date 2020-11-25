using System;

namespace Memo
{
    [Serializable]
    public class MemoData
    {
        public string Title;
        public string Content;
        public uint ThemeId;
        public double Width;
        public double Height;
        public bool Topmost;
    }
}
