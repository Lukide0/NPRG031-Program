namespace Tuif.Event;

/// <summary>
/// Delegát pro posluchače události.
/// </summary>
/// <typeparam name="TEvent">Typ události.</typeparam>
/// <param name="e">Instance události, která se posluchači předává.</param>
/// <returns>
/// True, pokud posluchač úspěšně zpracoval událost.
/// False, pokud posluchač nezpracoval událost nebo došlo k chybě.
/// </returns>
public delegate bool EventListener<TEvent>(TEvent e);

// <summary>
/// Správce událostí.
/// </summary>
/// <typeparam name="TEvent">Typ události.</typeparam>
public class EventManager<TEvent>
{
    private List<EventListener<TEvent>> _observers;

    /// <summary>
    /// Konstruktor třídy EventManager.
    /// Inicializuje seznam pozorovatelů událostí.
    /// </summary>
    public EventManager()
    {
        _observers = new List<EventListener<TEvent>>();
    }

    /// <summary>
    /// Připojí pozorovatele události.
    /// </summary>
    /// <param name="observer">Pozorovatel události.</param>
    public void Attach(EventListener<TEvent> observer)
    {
        _observers.Add(observer);
    }

    /// <summary>
    /// Odebere posledního pozorovatele události.
    /// </summary>
    /// <returns>True, pokud byl pozorovatel úspěšně odebrán. False, pokud nebyl žádný pozorovatel k odebrání.</returns>
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

    /// <summary>
    /// Vrátí počet pozorovatelů události.
    /// </summary>
    /// <returns>Počet pozorovatelů události.</returns>
    public int Count()
    {
        return _observers.Count;
    }

    /// <summary>
    /// Oznámí událost všem pozorovatelům.
    /// </summary>
    /// <param name="info">Informace o události.</param>
    /// <returns>True, pokud byla událost úspěšně zpracována alespoň jedním pozorovatelem. False, pokud nebyla událost zpracována žádným pozorovatelem.</returns>
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

    /// <summary>
    /// Oznámí všechny registrované posluchače o události a vrátí výsledek.
    /// </summary>
    /// <param name="info">Informace o události.</param>
    /// <returns>
    /// True, pokud alespoň jeden posluchač úspěšně zpracoval událost.
    /// False, pokud žádný posluchač nezpracoval událost nebo došlo k chybě.
    /// </returns>
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
