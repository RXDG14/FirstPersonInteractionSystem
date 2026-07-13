using TMPro;
using UnityEngine;

public class InteractableBase : MonoBehaviour , IInteractable
{
    [SerializeField] protected AudioSource _audioSource;
    
    [SerializeField] protected SphereCollider _interactionSphere;
    [SerializeField] protected PlayerRadiusCheck _playerRadiusCheck;

    [SerializeField] protected bool IsPickable = false;

    protected PlayerController _interactor;

    protected bool bInteractionCompleted = false;
    protected bool bIsAttachedToInteractor = false;

    // IInteractable
    public virtual void OnInteracted(PlayerController _playerController){}
    
    public virtual void OnDropped(PlayerController _playerController) {}
    
    public virtual void OnFocused() 
    {
        if (!_playerRadiusCheck.GetIsPlayerInRadius())
            return;

        if (bIsAttachedToInteractor)
        {
            _playerRadiusCheck.SetPromptText("F");
        }
        else
        {
            _playerRadiusCheck.SetPromptText("E");
        }
    }
    
    public virtual void OnUnfocused() 
    {
        if (bIsAttachedToInteractor)
        {
            _playerRadiusCheck.SetPromptText("F");
        }
        else
        {
            _playerRadiusCheck.SetPromptText("...");
        }
    }

    protected void PlayInteractedAudio()
    {
        if (_audioSource && _audioSource.clip)
        {
            _audioSource.PlayOneShot(_audioSource.clip);
        }
    }

    public bool GetIsPickable() { return IsPickable; }

    public bool GetIsAttachedToInteractor() {  return bIsAttachedToInteractor; }
}
