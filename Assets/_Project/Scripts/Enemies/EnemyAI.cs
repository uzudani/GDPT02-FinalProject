using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator _animator;

    private Rigidbody _rb;
    private Transform _playerTransform;
    private float _currentSpeed;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        if (_animator == null) _animator = GetComponent<Animator>();

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) _playerTransform = player.transform;
    }

    private void FixedUpdate()
    {
        if (_playerTransform != null && _currentSpeed > 0f)
        {
            Vector3 direction = _playerTransform.position - transform.position;
            direction.y = 0f;
            direction.Normalize();

            if (direction != Vector3.zero)
            {
                _rb.velocity = new Vector3(direction.x * _currentSpeed, _rb.velocity.y, direction.z * _currentSpeed);

                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.fixedDeltaTime * 10f);
            }
        }
    }

    public void SetSpeed(float newSpeed)
    {
        _currentSpeed = newSpeed;

        // Velocita' animazione basata sulla velocita' di movimento [TEST]
        if (_animator != null)
        {
            _animator.speed = newSpeed / 3f;
        }
    }
}
