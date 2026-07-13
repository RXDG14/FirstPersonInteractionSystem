using UnityEngine;

public class Interactable_PressureCube : InteractablePickable
{
    public override void OnInteracted(PlayerController _playerController)
    {
        base.OnInteracted(_playerController);

        if (!bIsAttachedToInteractor)
        {
            AttachToInteractor();
        }
    }

    public override void OnDropped(PlayerController _playerController)
    {
        if (bIsAttachedToInteractor)
        {
            DetachFromInteractor();
        }
    }

    protected override void AttachToInteractor()
    {
        if(_interactor != null)
        {
            SetPhysics(false);
            
            Transform _newParent = _interactor.GetObjectHolderTransform();
            transform.SetParent(_newParent);
            _rigidBody.transform.localPosition = Vector3.zero;
            _rigidBody.transform.localRotation = Quaternion.identity;

            _playerRadiusCheck.SetPromptText("F");

            bIsAttachedToInteractor = true;
            _interactor.SetHeldObject(this);
        }
    }
}
