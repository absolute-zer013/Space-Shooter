using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //serializeField is an attribute that serialize the data so that we/dev can read it in the inspector
    //they can also override the data from the inspector
    [SerializeField]
    //public or private reference
    //data type(int, float, bool, string)
    //every variable has a name
    //optional value assigned
    private float _speed = 4f;
    private float _speedMultiplier = 2.0f;

    private Animator _leftTrigger;
    private Animator _rightTrigger;

    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private float _FireRate = 0.15f;
    private float _canFire = -1f;
    private int _score = 0;

    private UIManager _uiManager;

    [SerializeField]
    private int _lives = 3;
    private Spawner _spawnManager;
    //Variable for triple shot

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

    //underscore indicate that the variable is private

    // Start is called before the first frame update
    // This will called when you start the game
    void Start()
    {
        gameObject.tag = "Player";
        //take the current position = new position (0, 0, 0)
        
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<Spawner>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        
        AnimationLoad();
        _audioSource = GetComponent<AudioSource>();

       transform.position = new Vector3(0, 0, 0);

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

    }

    // Update is called once per frame, consider as game loop
    // All the logic and user input will go
    // Typically run on 60 frames per sec on typical computer
    void Update()
    {
        CalculateMovement();
        if (Input.GetKey(KeyCode.Space) && Time.time > _canFire)
        {
            FireRate();
        }
        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            _leftTrigger.SetTrigger("turnLeftTrigger");
        }
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            _rightTrigger.SetTrigger("turnRightTrigger");
        }


    }

    void AnimationLoad()
    {
        _leftTrigger = GetComponent<Animator>();
        _rightTrigger = GetComponent<Animator>();

        if (_leftTrigger == null)
        {
            Debug.LogError("Left Animation Not Found");
        }
        if (_rightTrigger == null)
        {
            Debug.LogError("right Animation Not Found");
        }


    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        //new Vector3(1, 0, 0) = Vector3.right if left x = -1
        //this will move the player 1 meter/frame to the right at 60 m/s
        //we want to move the plater 1 m/s
        //we need to convert 1 meter/frame to 1 m/s
        //then we need to multiply it wih time.deltatime which will do the conversion
        //process from frame rate dependent to real, min,sec, and hour
        //new Vector3(-1, 0, 0) x 5 x real time

        transform.Translate(Vector3.right * horizontalInput *_speed * Time.deltaTime);
        transform.Translate(Vector3.up * verticalInput * _speed * Time.deltaTime);

        //you can also combine both line into single line of code

        //Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        //if speed power up = false
        //speed == normal
        //else if power up = true
        //speed == x2
        if(_speedboost == false)
        {
            transform.Translate(Vector3.right* horizontalInput * _speed * Time.deltaTime);
            transform.Translate(Vector3.up * verticalInput * _speed * Time.deltaTime);
        }
        else if (_speedboost == true)
        {
            transform.Translate(Vector3.right * horizontalInput * (_speed * _speedMultiplier)* Time.deltaTime);
            transform.Translate(Vector3.up * verticalInput * (_speed * _speedMultiplier)* Time.deltaTime);
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
        _uiManager.UpdateLives(_lives);

       if(_lives < 1)
        {
            //communicate with spawn manager
            _spawnManager.OnPlayerDeath();
            _uiManager.FinalScore(_score);
            // tell them you die
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
}