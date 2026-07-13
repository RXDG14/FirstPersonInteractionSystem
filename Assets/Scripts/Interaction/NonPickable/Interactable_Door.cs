using UnityEngine;

public class Interactable_Door : InteractableBase
{
    [SerializeField] private MoveableObject _objectToInteractWith;

    public override void OnInteracted(PlayerController _playerController)
    {
        if (!_playerRadiusCheck.GetIsPlayerInRadius() || GetIsPickable())
            return;

        if (_objectToInteractWith && !bInteractionCompleted)
        {
            _objectToInteractWith.StartMoveObject();
            bInteractionCompleted = true;
        }
    }
}
