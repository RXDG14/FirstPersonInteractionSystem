using TMPro;
using UnityEngine;

public class PlayerRadiusCheck : MonoBehaviour
{
    [SerializeField] private Canvas _interactionUI;
    [SerializeField] private TMP_Text _promptText;
    [SerializeField] private BoxCollider _playerRadiusCheckCollider;

    private bool bIsPlayerInRadius = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            bIsPlayerInRadius = true;
            SetInteractionUIVisibility(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            bIsPlayerInRadius = false;
            SetInteractionUIVisibility(false);
        }
    }

    public void SetInteractionUIVisibility(bool bVisible)
    {
        bVisible = bIsPlayerInRadius;
        _interactionUI.gameObject.SetActive(bVisible);
    }

    public void SetPromptText(string NewText)
    {
        _promptText.text = NewText;
    }

    public bool GetIsPlayerInRadius()
    {
        return bIsPlayerInRadius;
    }
}
