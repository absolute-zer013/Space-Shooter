using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{   
    [SerializeField]
    private GameObject _enemyPrefab;

    [SerializeField]
    private AudioClip _explosionSound;
    private AudioSource _audioSource;
    private bool _isCoop;

    private float _speed;
    //handel to animation component
    private Animator _DeathAnim;
    private Player _ployerScore;
    private Player1 _ployerScore1;
    private Player2 _ployerScore2;
    void Start()
    {   
        gameObject.tag = "Enemy";
        CoopCheck();

        if (_isCoop == false)
        {
            PlayerCallTrue();
            Debug.Log(_isCoop);
            Debug.Log("SinglePlayerCalled");
        }
        else if (_isCoop == true)
        {
            PlayerCallCoop();
            Debug.Log(_isCoop);
            Debug.Log("MultiPlayerCalled");
        }
        //assign the component
        //null check
        _DeathAnim = GetComponent<Animator>();

        if(_DeathAnim == null)
        {
            Debug.LogError("Death Animator not found");
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

    public void CoopCheck()
    {
        if (SceneManager.GetActiveScene().name == "Single_Player")
        {
            _isCoop = false;
            Debug.Log("Now Single Player Scene");
        }
        else if (SceneManager.GetActiveScene().name == "Co-op")
        {
            _isCoop = true;
            Debug.Log("Now in Coop mode");
        }
    }


    void Update()
    { 
        _speed = Random.Range(3f, 7f);
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -6.22f)
        {
            float RandomPosition = Random.Range(-10f, 10f);
            transform.position = new Vector3(RandomPosition, 8.3f, 0);
        }
    }

    void PlayerCallTrue()
    {
            _ployerScore = GameObject.Find("Player").GetComponent<Player>();
            if (_ployerScore == null)
            {
                Debug.LogError("The Player is Null");
            }
    }

    void PlayerCallCoop()
    {
            _ployerScore1 = GameObject.Find("Player_1").GetComponent<Player1>();
            _ployerScore2 = GameObject.Find("Player_2").GetComponent<Player2>();

            if (_ployerScore1 == null)
            {
                Debug.LogError("The Player1 is Null");
            }
            if (_ployerScore2 == null)
            {
                Debug.LogError("The Player2 is Null");
            }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
                if (player!= null)
            {
                player.Damage();
            }
            _DeathAnim.SetTrigger("OnDeathtrigger");
            _speed = 0;
            _audioSource.Play();
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 0.7f);
        }
        else if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            if(_ployerScore!=null)
            {
                _ployerScore.AddScore(Random.Range(10,50));
            }
            else if (_ployerScore1 != null)
            {
                _ployerScore1.AddScore(Random.Range(10, 50));
            }
            else if (_ployerScore2 != null)
            {
                _ployerScore2.AddScore(Random.Range(10, 50));
            }
            _DeathAnim.SetTrigger("OnDeathtrigger");
            _speed = 0;
            _audioSource.Play();
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 0.7f);
        }
        else 
        if (other.tag == "Player1")
        {
            Player1 player1 = other.transform.GetComponent<Player1>();
            if (player1 != null)
            {
                player1.Damage();
            }
            _DeathAnim.SetTrigger("OnDeathtrigger");
            _speed = 0;
            _audioSource.Play();
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 0.7f);
        }
        else 
        if (other.tag == "Player2")
        {
            Player2 player2 = other.transform.GetComponent<Player2>();
            if (player2 != null)
            {
                player2.Damage();
            }
            _DeathAnim.SetTrigger("OnDeathtrigger");
            _speed = 0;
            _audioSource.Play();
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 0.7f);
        }
    }
}
