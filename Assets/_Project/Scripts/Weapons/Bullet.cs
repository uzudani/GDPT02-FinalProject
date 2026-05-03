using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float _speed;
    private float _damage;
    private float _lifeTimer;

    private bool _isInitialized = false;

    private void Update()
    {
        if (!_isInitialized) return;

        transform.Translate(Vector3.forward * _speed * Time.deltaTime);

        _lifeTimer -= Time.deltaTime;
        if (_lifeTimer <= 0)
        {
            gameObject.SetActive(false); // Riciclo
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            LifeController enemyLife = other.GetComponent<LifeController>();

            if (enemyLife != null)
            {
                enemyLife.TakeDamage(_damage);
            }

            gameObject.SetActive(false); // Riciclo nel momento di impatto
        }
    }

    public void Initialize(float speed, float damage, float lifetime)
    {
        _speed = speed;
        _damage = damage;
        _lifeTimer = lifetime;
        _isInitialized = true;
    }

    private void OnDisable()
    {
        _isInitialized = false;
    }
}
