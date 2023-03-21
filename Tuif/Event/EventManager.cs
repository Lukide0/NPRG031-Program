namespace Tuif.Event;

public delegate bool EventListener<TEvent>(TEvent e);
public class EventManager<TEvent>
{
    private List<EventListener<TEvent>> _observers;

    public EventManager()
    {
        _observers = new List<EventListener<TEvent>>();
    }

    public void Attach(EventListener<TEvent> observer)
    {
        _observers.Add(observer);
    }

    public bool RemoveLast() 
    {
        if (_observers.Count > 0) 
        {
            _observers.RemoveAt(_observers.Count - 1);
            return true;
        }
        else
        {
            return false;
        }
    }

    public int Count() 
    {
        return _observers.Count;
    }

    public bool Notify(TEvent info)
    {
        bool status = false;
        foreach (var fun in _observers)
        {
            status = fun(info);
            if (status)
            {
                break;
            }
        }

        return status;
    }

    public bool NotifyAll(TEvent info) 
    {
        bool status = false;
        foreach (var fun in _observers)
        {
            status |= fun(info);
        }

        return status;
    }
}