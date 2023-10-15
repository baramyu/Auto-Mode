using UnityEngine;

public interface IInteractor
{
    public string interactorName { get; }
    public Sprite interactorImage { get; }
    public Vector3 position { get; }
    public bool IsInteractAbleDistance(Vector3 position);
    public void Interact();
    public void StopInteract();
}
