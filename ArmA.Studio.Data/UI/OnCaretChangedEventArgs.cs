namespace ArmA.Studio.Data.UI
{
    public class OnCaretChangedEventArgs
    {
        public int Column { get; internal set; }
        public int Line { get; internal set; }
        public int Offset { get; internal set; }
    }
}