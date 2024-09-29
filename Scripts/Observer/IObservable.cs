using System;

public interface IObservable
{
    void Subscribe(IObserver observer);
    void UnSubscribe(IObserver observer);
    void Notify(Enum value);
}