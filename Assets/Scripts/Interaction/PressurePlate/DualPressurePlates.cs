using UnityEngine;

public class DualPressurePlates : MonoBehaviour
{
    [SerializeField] private PressurePlate _pressurePlate1;
    [SerializeField] private PressurePlate _pressurePlate2;
    [SerializeField] private MoveableObject _objectToInteractWith;

    private void Awake()
    {
        _pressurePlate1.OnPressurePlateActivated += CheckPlateActivation;
        _pressurePlate1.OnPressurePlateDeactivated += CheckPlatedeactivation;
        
        _pressurePlate2.OnPressurePlateActivated += CheckPlateActivation;
        _pressurePlate2.OnPressurePlateDeactivated += CheckPlatedeactivation;
    }

    private void OnDestroy()
    {
        _pressurePlate1.OnPressurePlateActivated -= CheckPlateActivation;
        _pressurePlate1.OnPressurePlateDeactivated -= CheckPlatedeactivation;
        
        _pressurePlate2.OnPressurePlateActivated -= CheckPlateActivation;
        _pressurePlate2.OnPressurePlateDeactivated -= CheckPlatedeactivation;
    }

    private void CheckPlateActivation()
    {
        if(_pressurePlate1.GetIsPressurePlateActive() && _pressurePlate2.GetIsPressurePlateActive())
        {
            _objectToInteractWith.StartMoveObject();
        }
    }

    private void CheckPlatedeactivation()
    {
        if(!_pressurePlate1.GetIsPressurePlateActive() || !_pressurePlate2.GetIsPressurePlateActive())
        {
            _objectToInteractWith.ReverseMoveObject();
        }
    }
}
