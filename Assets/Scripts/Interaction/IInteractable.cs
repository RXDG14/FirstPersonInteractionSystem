using UnityEngine;

public interface IInteractable
{
    void OnFocused();
    void OnUnfocused();
    void OnInteracted(PlayerController player);
    void OnDropped(PlayerController player);
}
