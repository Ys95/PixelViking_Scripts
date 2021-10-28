using System.Collections;

public interface IWaitsForSingleton
{
    IEnumerator WaitTillSingletonReady();

    void AfterSingletonIsReady();
}