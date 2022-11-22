using Bot.Extensions;
using System.Collections.Immutable;

namespace Bot.Abstractions;

public class AsyncEvent<T> where T : class
{
    private readonly object _subLock = new();

    private ImmutableArray<T> _subscriptions;

    public IReadOnlyList<T> Subscriptions => _subscriptions;

    public AsyncEvent() => _subscriptions = ImmutableArray.Create<T>();

    public void Add(T subscriber)
    {
        subscriber.NotNull(nameof(subscriber));

        lock (_subLock)
            _subscriptions = _subscriptions.Add(subscriber);
    }

    public void Remove(T subscriber)
    {
        subscriber.NotNull(nameof(subscriber));

        lock (_subLock)
            _subscriptions = _subscriptions.Remove(subscriber);
    }
}
