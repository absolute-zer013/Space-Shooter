using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1 : MonoBehaviour
{

    private float _speed = 5f;
    private float _speedMultiplier = 2.0f;

    private Animator _leftTrigger;
    private Animator _rightTrigger;

    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private float _FireRate = 0.15f;
    private float _canFire = -1f;
    private int _score = 0;

    private UIManagerCoop _uiManager;

    [SerializeField]
    private int _lives = 3;
    private Spawner _spawnManager;

    private bool _tripleShot = false;
    [SerializeField]
    private bool _speedboost = false;
    [SerializeField]
    private bool _shieldPower = false;
    [SerializeField]
    private GameObject _tripleLaserPrefab;
    [SerializeField]
    private GameObject _shieldPrefab;
    [SerializeField]
    private GameObject _leftEngine;
    [SerializeField]
    private GameObject _rightEngine;

    [SerializeField]
    private AudioClip _laserSound;
    private AudioSource _audioSource;

    private Player2 _deleteP2;

    void Start()
    {
        gameObject.tag = "Player1";

        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<Spawner>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManagerCoop>();
        _audioSource = GetComponent<AudioSource>();
        _deleteP2 = GameObject.Find("Player_2").GetComponent<Player2>();


        if (_spawnManager == null)
        {
            Debug.LogError("spawn manager not found");
        }
        if(_uiManager == null)
        {
            Debug.LogError("UI Manager not found");
        }
        
        if(_audioSource == null)
        {
            Debug.LogError("Audio Source not found");
        }
        else
        {
            _audioSource.clip = _laserSound;
        }

        if(_deleteP2 == null)
        {
            Debug.LogError("Player 2 not found");
        }

    }

    void Update()
    {

        CalculateMovement();
        if (Input.GetKey(KeyCode.Space) && Time.time > _canFire)
        {
            FireRate();
        }

    }

    void CalculateMovement()
    {
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.left * _speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * _speed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.up * _speed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.A) && _speedboost == false)
        {
            transform.Translate(Vector3.left * _speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D) && _speedboost == false)
        {
            transform.Translate(Vector3.right * _speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.W) && _speedboost == false)
        {
            transform.Translate(Vector3.up * _speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S) && _speedboost == false)
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime);
        }

        if (_speedboost == true && Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.left * (_speed * _speedMultiplier) * Time.deltaTime);
        }
        if (_speedboost == true && Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * (_speed * _speedMultiplier) * Time.deltaTime);
        }
        if (_speedboost == true && Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.up * (_speed * _speedMultiplier) * Time.deltaTime);
        }
        if (_speedboost == true && Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.down * (_speed * _speedMultiplier) * Time.deltaTime);
        }

        if (transform.position.y >= 6.83f)
            {

                transform.position = new Vector3(transform.position.x, 6.83f, 0);
            }
            else if (transform.position.y <= -4.47f)
            {
                transform.position = new Vector3(transform.position.x, -4.47f, 0);
            }

            if (transform.position.x >= 12.56f)
            {
                transform.position = new Vector3(-12.56f, transform.position.y, 0);
            }
            else if (transform.position.x <= -12.56f)
            {
                transform.position = new Vector3(12.56f, transform.position.y, 0);
            }


        }

    void FireRate()
    {

    _canFire = Time.time + _FireRate;

        if (_tripleShot == true)
        {
            Instantiate(_tripleLaserPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.1f, 0), Quaternion.identity);
        }

        _audioSource.Play();

    }

    public void Damage()
    {
            if (_shieldPower == true)
            {
                Debug.Log("No Damage");
                _shieldPower = false;
                _shieldPrefab.SetActive(false);
                return;
            }

        _lives--;

        if(_lives == 2)
        {
            _leftEngine.SetActive(true);
        }
        else if (_lives == 1)
        {
            _rightEngine.SetActive(true);
        }
        _uiManager.UpdateLivesP1(_lives);

       if(_lives < 1)
        {
            _spawnManager.OnPlayerDeath();
            _uiManager.FinalScore(_score);
            _deleteP2.deleteObjectP2();
            Destroy(this.gameObject);
        }
    }

    public void ShieldPower()
    {
        _shieldPower = true;
        _shieldPrefab.SetActive(true);
        StartCoroutine(ShieldCoolDown());
    }

    IEnumerator ShieldCoolDown()
    {
        yield return new WaitForSeconds(5.0f);
        _shieldPower = false;
        _shieldPrefab.SetActive(false);
    }

    public void TripleShotActive()
    {
        _tripleShot = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }
    IEnumerator TripleShotPowerDownRoutine()
    {   
        while(_tripleShot == true)
        {
            yield return new WaitForSeconds(5.0f);
            _tripleShot = false;
        }
    }

    public void SpeedBoost()
    {
    _speedboost = true;
    StartCoroutine(SpeedBoostCooldown());
        
    }
    IEnumerator SpeedBoostCooldown()
    {
     yield return new WaitForSeconds(5.0f);
     _speedboost = false;
    }

    public void AddScore(int point)
    {   
        _score += point;
        _uiManager.ScoreUpdate(_score);
    }

    public void deleteObjectP1()
    {
        Destroy(this.gameObject);
    }
}