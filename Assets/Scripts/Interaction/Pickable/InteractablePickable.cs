using UnityEngine;

public class InteractablePickable : InteractableBase
{
    [SerializeField] protected Rigidbody _rigidBody;
    [SerializeField] protected BoxCollider _meshCollider;
    [SerializeField] protected bool IsWeapon = false;
    
    protected Vector3 _origin;

    protected void Start()
    {
        _origin = transform.position;

        InvokeRepeating(nameof(CheckPosition), 0.5f, 0.5f);
    }

    private void CheckPosition()
    {
        if (transform.position.y < -100f)
        {
            _rigidBody.linearVelocity = Vector3.zero;
            _rigidBody.angularVelocity = Vector3.zero;

            transform.position = _origin;
            transform.rotation = Quaternion.identity;
        }
    }

    public override void OnInteracted(PlayerController _playerController)
    {
        if (!_playerRadiusCheck.GetIsPlayerInRadius() || !GetIsPickable())
            return;

        if (_playerController)
        {
            _interactor = _playerController;
        }

        PlayInteractedAudio();
    }

    protected virtual void AttachToInteractor() {}

    protected virtual void DetachFromInteractor() 
    {
        if (_interactor != null)
        {
            transform.SetParent(null);
            SetPhysics(true);

            _interactor.ClearHeldObject();
            bIsAttachedToInteractor = false;

            _playerRadiusCheck.SetPromptText("...");
        }

        PlayInteractedAudio();
    }

    protected virtual void SetPhysics(bool NewPhysics)
    {
        if (!_rigidBody)
            return;

        if (NewPhysics) // dropped
        {
            _rigidBody.isKinematic = false;
            _rigidBody.useGravity = true;

            _meshCollider.isTrigger = false;
        }
        else // equipped
        {
            _rigidBody.isKinematic = true;
            _rigidBody.useGravity = false;

            _rigidBody.linearVelocity = Vector3.zero;
            _rigidBody.angularVelocity = Vector3.zero;

            _meshCollider.isTrigger = true;
        }
    }
}
