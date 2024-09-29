using System;

public interface IObserver
{
    void CheckNotify(IObservable observable, Enum value);
}