using UnityEngine;

public class Interactable_Weapon : InteractablePickable
{
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private float _bulletSpeed = 10f;
    [SerializeField] private Transform _muzzleLocation;

    public override void OnInteracted(PlayerController _playerController)
    {
        base.OnInteracted(_playerController);

        if (!bIsAttachedToInteractor)
        {
            AttachToInteractor();
            _playerController.SetWeapon(this);
        }
    }

    public override void OnDropped(PlayerController _playerController)
    {
        if (bIsAttachedToInteractor)
        {
            DetachFromInteractor();
            _playerController.SetWeapon(null);
        }
    }

    protected override void AttachToInteractor()
    {
        if (_interactor != null)
        {
            SetPhysics(false);

            Transform _newParent = _interactor.GetGunHolderTransform();
            transform.SetParent(_newParent);
            _rigidBody.transform.localPosition = Vector3.zero;
            _rigidBody.transform.localRotation = Quaternion.identity;

            _playerRadiusCheck.SetPromptText("F");

            bIsAttachedToInteractor = true;
            _interactor.SetHeldObject(this);
        }
    }

    public void FireBullet(Vector3 _aimDirection)
    {
        Bullet _bullet = GameObject.Instantiate(_bulletPrefab, _muzzleLocation.transform.position, Quaternion.identity, null);
        if( _bullet != null )
        {
            _bullet.MoveBullet(_aimDirection, _bulletSpeed);
        }
    }
}
