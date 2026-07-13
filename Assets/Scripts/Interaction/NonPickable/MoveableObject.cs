using DG.Tweening;
using UnityEngine;

public class MoveableObject : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private Vector3 _origin;
    [SerializeField] private Vector3 _destination;
    [SerializeField] private float durationToDestination = 2f;
    [SerializeField] private float durationToOrigin = 0.5f;
    [SerializeField] private bool OnlyYDirection = false;

    private Tween _moveTween;

    private void Start()
    {
        _origin = transform.position;
        
        _destination = _origin;
        _destination.y += 10;
    }

    public void StartMoveObject()
    {
        if (OnlyYDirection)
        {
            _moveTween?.Kill();
            _moveTween = transform.DOMove(_destination, durationToDestination);

            PlayObjectAudio();
        }
        else
        {
            _moveTween?.Kill();
            _moveTween = transform.DOMove(_destination, durationToDestination);

            PlayObjectAudio();
        }
    }

    public void ReverseMoveObject()
    {
        _moveTween?.Kill();
        _moveTween = transform.DOMove(_origin, durationToOrigin);

        PlayObjectAudio();
    }

    private void PlayObjectAudio()
    {
        if (_audioSource && _audioSource.clip)
        {
            _audioSource.PlayOneShot(_audioSource.clip);
        }
    }
}
