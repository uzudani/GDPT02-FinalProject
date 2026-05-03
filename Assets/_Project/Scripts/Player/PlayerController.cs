using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerStats))]
public class PlayerController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float _speed = 7f;
    [SerializeField] private float _rotationSpeed = 500f;

    [Header("References")]
    [SerializeField] private Camera _cam;
    [SerializeField] private Animator _animator;

    private Rigidbody _rb;
    private Vector3 _inputDirection;
    private PlayerStats _stats;

    private void Awake()
    {
        PrepareComponents();
    }

    private void Update()
    {
        PlayerInputSystem();
    }

    private void FixedUpdate()
    {
        MovePlayer();
        RotatePlayer();
    }

    private void PlayerInputSystem()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 camForward = _cam.transform.forward;
        Vector3 camRight = _cam.transform.right;

        // Direzioni sul pavimento
        camForward.y = 0f;
        camRight.y = 0f;

        // Vettori a lunghezza 1
        camForward.Normalize();
        camRight.Normalize();

        Vector3 desiredDirection = camForward * v + camRight * h; // WASD direzioni cam

        // ClampMgnitude invece di Normalized per inclinazione levetta pad
        _inputDirection = Vector3.ClampMagnitude(desiredDirection, 1f);
    }

    private void PrepareComponents()
    {
        _rb = GetComponent<Rigidbody>();
        _stats = GetComponent<PlayerStats>();

        _rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
        _rb.interpolation = RigidbodyInterpolation.Interpolate;
        _rb.useGravity = false;
    }

    private void MovePlayer()
    {
        bool isMoving = _inputDirection != Vector3.zero;

        if (_animator != null)
        {
            _animator.SetBool("IsMoving", isMoving);
        }

        if (isMoving)
        {
            float currentSpeed = _speed * _stats.MoveSpeedMultiplier;
            Vector3 targetPosition = _rb.position + _inputDirection * (currentSpeed * Time.fixedDeltaTime);
            _rb.MovePosition(targetPosition);
        }

        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
    }

    private void RotatePlayer()
    {
        if (_inputDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(_inputDirection, Vector3.up);
            _rb.MoveRotation(Quaternion.RotateTowards(_rb.rotation, targetRotation, _rotationSpeed * Time.fixedDeltaTime));
        }
    }

    public void TriggerDeathAnimation()
    {
        if (_animator != null)
        {
            _animator.SetTrigger("Die");
        }
    }
}
