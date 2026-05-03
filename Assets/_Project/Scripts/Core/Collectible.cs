using UnityEngine;

public class Collectible : MonoBehaviour, ICollectible
{
    private SO_BaseDrop _data;

    public void Initialize(SO_BaseDrop data)
    {
        _data = data;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Collect(other.gameObject);
        }
    }

    public void Collect(GameObject collector)
    {
        if (_data == null) return;

        // Effetto specifico SO (Polimorfismo)
        _data.ApplyEffect(collector);

        if (AudioManager.Instance != null && _data.PickupSound != null)
        {
            AudioManager.Instance.PlayAudio(_data.PickupSound, _data.TargetChannel, _data.SoundVolume);
        }

        gameObject.SetActive(false);
    }
}
