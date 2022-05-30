using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2 : MonoBehaviour
{

    private float _speed = 5f;
    private float _speedMultiplier = 2.0f;

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

    private Player1 _deleteP1;

    void Start()
    {
        gameObject.tag = "Player2";

        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<Spawner>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManagerCoop>();
        _deleteP1 = GameObject.Find("Player_1").GetComponent<Player1>();
        _audioSource = GetComponent<AudioSource>();

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

        if(_deleteP1 == null)
        {
            Debug.LogError("Player 1 Not Found");
        }
    }

    // Update is called once per frame, consider as game loop
    // All the logic and user input will go
    // Typically run on 60 frames per sec on typical computer
    void Update()
    {
        CalculateMovement();
        if (Input.GetKey(KeyCode.Return) && Time.time > _canFire)
        {
            FireRate();
        }

    }

    void CalculateMovement()
    {

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(Vector3.left * _speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(Vector3.right * _speed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(Vector3.up * _speed * Time.deltaTime);
        }
       
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime);
        }
           
        if (Input.GetKey(KeyCode.LeftArrow) && _speedboost == false)
        {
            transform.Translate(Vector3.left * _speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.RightArrow) && _speedboost == false)
        {
            transform.Translate(Vector3.right * _speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.UpArrow) && _speedboost == false)
        {
            transform.Translate(Vector3.up * _speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.DownArrow) && _speedboost == false)
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime);
        }

        if (_speedboost == true && Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(Vector3.left *( _speed * _speedMultiplier) * Time.deltaTime);
        }
        if (_speedboost == true && Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(Vector3.right * (_speed * _speedMultiplier) * Time.deltaTime);
        }
        if (_speedboost == true && Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(Vector3.up * (_speed * _speedMultiplier) * Time.deltaTime);
        }
        if (_speedboost == true && Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(Vector3.down * (_speed * _speedMultiplier) * Time.deltaTime);
        }

        //if player position = y > 0
        //y position = 0

        if (transform.position.y >= 6.83f)
        {
            //we cannot simply type transform.position.y = 0;an error will occur because we neglecting x and z axis
            //we cannot use vector3(0, 0, 0) because when the condition is triggered, it will aslo reset the x value
            //so we need to take the current value of x axis and reset the y axis only.
            transform.position = new Vector3(transform.position.x, 6.83f, 0);
        }
        else if (transform.position.y <= -4.47f)
        {
            transform.position = new Vector3(transform.position.x, -4.47f, 0);
        }


        //you can also do clamping method where you clamp the min and max value of y
        //transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -4.82f, 6.83f), 0);

        //the reason why this code little bit different than the y axis, because we not only want to restrict
        //the x axis but also this will create a wrap illusion where the player will reappear on the opposite side
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
                //visual disable
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
        _uiManager.UpdateLivesP2(_lives);

       if(_lives < 1)
        {
            //communicate with spawn manager
            _spawnManager.OnPlayerDeath();
            _uiManager.FinalScore(_score);
            // tell them you die
            _deleteP1.deleteObjectP1();
            Destroy(this.gameObject);
        }
    }

    public void ShieldPower()
    {
        _shieldPower = true;
        //enable visual
        _shieldPrefab.SetActive(true);
        StartCoroutine(ShieldCoolDown());
    }

    IEnumerator ShieldCoolDown()
    {
        yield return new WaitForSeconds(5.0f);
        _shieldPower = false;
        //visual disable
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

    public void deleteObjectP2()
    {
        Destroy(this.gameObject);
    }
}