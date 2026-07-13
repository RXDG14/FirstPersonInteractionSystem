using System;
using UnityEngine;
using DG.Tweening;

public class Interactable_Button : InteractableBase
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

        PlayInteractedAudio();
    }
}
