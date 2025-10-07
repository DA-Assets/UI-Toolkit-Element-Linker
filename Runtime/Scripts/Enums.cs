namespace DA_Assets.ULB
{
    public enum UitkLinkingMode
    {
        None = 0,
        Name = 1,
        IndexNames = 2,
        Guid = 3,
        Guids = 4,
    }

    public interface IHaveElement<T>
    {
        T E { get; }
        T Element { get; }
    }
}
