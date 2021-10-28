using UnityEngine;

public interface IInteractable
{
    public bool WasInteractedWith { get; }

    public void Interact(GameObject whoInteracts);

    public void LoadState(bool state);
}