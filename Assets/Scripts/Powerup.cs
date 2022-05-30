using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.0f;
    [SerializeField] //0 = Triple Shot, 1 = Speed, 2 = Shields
    private int powerUpID;
    [SerializeField]
    private AudioClip _powerUpsSound;
    private AudioSource _audioSource;


    void Start()
    {
        _audioSource = GetComponent<AudioSource>();

        if (_audioSource == null)
        {
            Debug.LogError("Power Up Sound Not Found");
        }
        else
        {
            _audioSource.clip = _powerUpsSound;
            _audioSource.Play();
        }
    }
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if(transform.position.y < -6.17f)
        {
            Destroy(this.gameObject);
        }


    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                switch(powerUpID)
                {
                    case 0:
                        player.TripleShotActive();
                        Debug.Log("TripleShot Collected");
                        break;
                    case 1:
                        player.SpeedBoost();
                        Debug.Log("Speed Boost Collected");
                        break;
                    case 2:
                        player.ShieldPower();
                        Debug.Log("Shield PowerUp Collected");
                        break;
                    default:
                        Debug.Log("Default Value");
                        break;
                }
            }
            Destroy(this.gameObject);
        }
        else if (other.tag == "Player1")
        {
            Player1 player = other.transform.GetComponent<Player1>();
            if (player != null)
            {
                switch (powerUpID)
                {
                    case 0:
                        player.TripleShotActive();
                        Debug.Log("TripleShot Collected");
                        break;
                    case 1:
                        player.SpeedBoost();
                        Debug.Log("Speed Boost Collected");
                        break;
                    case 2:
                        player.ShieldPower();
                        Debug.Log("Shield PowerUp Collected");
                        break;
                    default:
                        Debug.Log("Default Value");
                        break;
                }
            }
            Destroy(this.gameObject);
        }
        else if (other.tag == "Player2")
        {
            Player2 player = other.transform.GetComponent<Player2>();
            if (player != null)
            {
                switch (powerUpID)
                {
                    case 0:
                        player.TripleShotActive();
                        Debug.Log("TripleShot Collected");
                        break;
                    case 1:
                        player.SpeedBoost();
                        Debug.Log("Speed Boost Collected");
                        break;
                    case 2:
                        player.ShieldPower();
                        Debug.Log("Shield PowerUp Collected");
                        break;
                    default:
                        Debug.Log("Default Value");
                        break;
                }
            }
            Destroy(this.gameObject);
        }
    }
}
