using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidBody;
    [SerializeField] private AudioSource _audioSource;

    private void Start()
    {
        Invoke("DestroyBullet", 2);
        _audioSource.PlayOneShot(_audioSource.clip);
    }

    public void MoveBullet(Vector3 _aimDirection, float _bulletSpeed)
    {
        _rigidBody.AddForce(_aimDirection * _bulletSpeed, ForceMode.Impulse);
    }

    private void DestroyBullet()
    {
        Destroy(gameObject);
    }
}
