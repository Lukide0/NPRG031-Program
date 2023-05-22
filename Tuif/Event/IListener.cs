namespace Tuif.Event;

/// <summary>
/// Rozhraní pro posluchač událostí.
/// </summary>
/// <typeparam name="TEvent">Typ události.</typeparam>
public interface IListener<TEvent>
{
    /// <summary>
    /// Zpracuje událost a vrátí výsledek.
    /// </summary>
    /// <param name="info">Informace o události.</param>
    /// <returns>
    /// True, pokud úspěšně zpracoval událost.
    /// False, pokud nezpracoval událost nebo došlo k chybě.
    /// </returns>
    bool HandleEvent(TEvent info);
}
