namespace Tuif.Event;

public interface IListener<TEvent>
{
    public bool HandleEvent(TEvent info);
}