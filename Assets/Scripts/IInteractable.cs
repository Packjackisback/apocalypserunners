using UnityEngine;

public interface IInteractable
{
    void Interact();
    bool IsTutorialInteraction { get; }
}