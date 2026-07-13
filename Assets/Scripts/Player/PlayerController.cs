using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private GameObject _playerCameraHolder;
    [SerializeField] private Transform _objectHolder;
    [SerializeField] private Transform _gunHolder;
    [SerializeField] private Rigidbody _rigidBody;

    [Header("Movement")]
    [SerializeField] private float _walkSpeed = 5f;
    [SerializeField] private float _sprintSpeed = 8f;
    
    [Header("Camera")]
    [SerializeField] private float _lookSpeed = 35f;
    
    [Header("Interaction")]
    [SerializeField] private LayerMask _interactionLayer;
    [SerializeField] private float _maxInteractionDistance = 50f;

    private InteractableBase _currentInteractable;
    private InteractablePickable _currentHeldObject;
    private Interactable_Weapon _playerWeapon;

    private Vector2 _moveInput;
    private Vector2 _lookInput;

    private bool bIsSprinting = false;
    private bool bCanInteract = false;
    
    private float _playerYaw;
    private float _cameraPitch = 0;

    private void Awake()
    {
        _playerInput.actions["Move"].performed += GetMoveInput;
        _playerInput.actions["Move"].canceled += GetMoveInput;
        _playerInput.actions["Look"].performed += GetLookInput;
        _playerInput.actions["Look"].canceled += GetLookInput;
        _playerInput.actions["Interact"].performed += OnInteract;
        _playerInput.actions["Drop"].performed += OnDropped;
        _playerInput.actions["Shoot"].performed += OnShoot;
        _playerInput.actions["Restart"].performed += OnRestart;
        _playerInput.actions["Menu"].performed += OnMenuKeyPressed;
    }

    private void OnDestroy()
    {
        _playerInput.actions["Move"].performed -= GetMoveInput;
        _playerInput.actions["Move"].canceled -= GetMoveInput;
        _playerInput.actions["Look"].performed -= GetLookInput;
        _playerInput.actions["Look"].canceled -= GetLookInput;
        _playerInput.actions["Interact"].performed -= OnInteract;
        _playerInput.actions["Drop"].performed -= OnDropped;
        _playerInput.actions["Shoot"].performed -= OnShoot;
        _playerInput.actions["Restart"].performed -= OnRestart;
        _playerInput.actions["Menu"].performed -= OnMenuKeyPressed;
    }

    private void GetMoveInput(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
    }

    private void GetLookInput(InputAction.CallbackContext context)
    {
        _lookInput = context.ReadValue<Vector2>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        GetLookRotation();
        CheckForInteractables();
    }

    private void FixedUpdate()
    {
        RotatePlayer();
        MovePlayer();
    }

    private void LateUpdate()
    {
        RotateCamera();
    }

    private void GetLookRotation()
    {
        float mouseX = _lookInput.x * _lookSpeed * Time.deltaTime;
        float mouseY = _lookInput.y * _lookSpeed * Time.deltaTime;

        // left-right
        _playerYaw += mouseX;

        // up-down
        _cameraPitch -= mouseY;
        _cameraPitch = Mathf.Clamp(_cameraPitch, -85f, 85f);
    }

    private void RotateCamera()
    {
        _playerCameraHolder.transform.localRotation = Quaternion.Euler(_cameraPitch, 0, 0); // only rotate vertically
    }

    private void MovePlayer()
    {
        // Get input direction
        Vector3 _input = new Vector3(_moveInput.x, 0, _moveInput.y); // x = sideways, z = forward

        // Get camera direction
        Vector3 _cameraForward = _playerCameraHolder.transform.forward;
        Vector3 _cameraRight = _playerCameraHolder.transform.right;
        _cameraForward.y = 0;
        _cameraRight.y = 0;
        _cameraForward.Normalize();
        _cameraRight.Normalize();

        Vector3 moveDirection = (_cameraForward * _input.z) + (_cameraRight * _input.x);

        // Get Speed
        bool isSprinting = _playerInput.actions["Sprint"].IsPressed();
        float _speed = isSprinting ? _sprintSpeed : _walkSpeed;

        _rigidBody.MovePosition(_rigidBody.position + moveDirection * _speed * Time.fixedDeltaTime);
    }

    private void RotatePlayer()
    {
        Quaternion newRotation = Quaternion.Euler(0f, _playerYaw, 0f);
        _rigidBody.MoveRotation(newRotation);
    }

    private void CheckForInteractables()
    {
        InteractableBase _hitInteractable = null;
        Ray _ray = new Ray(_playerCameraHolder.transform.position, _playerCameraHolder.transform.forward);

        if (Physics.Raycast(_ray, out RaycastHit _raycastHitInfo, _maxInteractionDistance, _interactionLayer))
        {
            _hitInteractable = (InteractableBase)_raycastHitInfo.collider.GetComponentInParent<IInteractable>();
        }

        if (_currentInteractable != _hitInteractable) // make currentInt the Int we just hit
        {
            _currentInteractable?.OnUnfocused(); // unfocus the last one
            _currentInteractable = _hitInteractable; // assign new
        }

        if (_currentHeldObject != null) // if we are holding an object, we cant interact with other
        {
            bCanInteract = false;
            return;
        }

        if (_currentInteractable != null)
        {
            bCanInteract = true;
            _currentInteractable.OnFocused();
        }
        else
        {
            bCanInteract = false;
        }
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        if (bCanInteract)
        {
            _currentInteractable?.OnInteracted(this);
        }
    }

    private void OnDropped(InputAction.CallbackContext context)
    {
        _currentHeldObject?.OnDropped(this);
    }

    private void OnShoot(InputAction.CallbackContext context)
    {
        if (_playerWeapon && _playerCameraHolder)
        {
            _playerWeapon.FireBullet(_playerCameraHolder.transform.forward);
        }
    }

    private void OnRestart(InputAction.CallbackContext context)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnMenuKeyPressed(InputAction.CallbackContext context)
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

        SceneManager.LoadScene(0);
    }
    
    public void SetWeapon(Interactable_Weapon _weapon)
    {
        _playerWeapon = _weapon;
    }

    public void SetHeldObject(InteractablePickable heldObject)
    {
        _currentHeldObject = heldObject;
    }

    public void ClearHeldObject()
    {
        _currentHeldObject = null;
    }

    public Transform GetObjectHolderTransform() { return _objectHolder; }

    public Transform GetGunHolderTransform() { return _gunHolder; }

    public InteractablePickable GetHeldObject(){ return _currentHeldObject; }
}
