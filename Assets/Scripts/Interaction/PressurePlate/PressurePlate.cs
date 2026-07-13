using System;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    [SerializeField] private BoxCollider _collider;
    [SerializeField] private AudioSource _audioSource;

    public event Action OnPressurePlateActivated;
    public event Action OnPressurePlateDeactivated;
    
    private bool _IsPressurePlateActive = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PressureCube"))
        {
            _IsPressurePlateActive = true;
            OnPressurePlateActivated?.Invoke();
            PlayObjectAudio();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("PressureCube"))
        {
            _IsPressurePlateActive = false;
            OnPressurePlateDeactivated?.Invoke();
            PlayObjectAudio();
        }
    }

    public bool GetIsPressurePlateActive()
    {
        return _IsPressurePlateActive;
    }

    private void PlayObjectAudio()
    {
        if (_audioSource && _audioSource.clip)
        {
            _audioSource.PlayOneShot(_audioSource.clip);
        }
    }
}
