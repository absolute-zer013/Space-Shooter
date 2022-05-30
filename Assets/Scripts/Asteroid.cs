using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private float _speedrotation = 3.0f;

    [SerializeField]
    private GameObject _explosionPrefab;
    private Spawner _spawnManager;

    [SerializeField]
    private AudioClip _explosionSound;
    private AudioSource _audioSource;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 5.17f, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<Spawner>();
        
        if(_spawnManager == null)
        {
            Debug.LogError("Spawn Manager not found");
        }

        _audioSource = GetComponent<AudioSource>();

        if (_audioSource == null)
        {
            Debug.LogError("Explosion Audio Not Found");
        }
        else 
        {
        _audioSource.clip = _explosionSound;
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * _speedrotation * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            _spawnManager.StartSpawning();
            _audioSource.Play();
            Destroy(GetComponent<Collider2D>());
            Destroy(other.gameObject);
            Destroy(this.gameObject, 0.7f);
        }
    }

    //check collision trigger
    //instantiate explosion on location (us)
    //delay object destory
}
