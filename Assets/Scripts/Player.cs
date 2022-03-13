using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.5f;

    [SerializeField]
    private float _speedMultiplier = 2.0f;

    [SerializeField]
    private GameObject _laserPrefab;

    [SerializeField]
    private float _fireRate = 0.15f;

    private float _nextFire = -1f;

    [SerializeField]
    private int _lives = 3;

    private SpawnManager _spawnManager;

    [SerializeField]
    private GameObject _tripleShotPrefab;

    [SerializeField]
    private bool _isTripleShotActivate = false;

    [SerializeField]
    private bool _isSpeedBoostActivate = false;

    [SerializeField]
    private bool _isShieldActivate = false;

    [SerializeField]
    private GameObject _shield;

    [SerializeField]
    private int _score;

    [SerializeField]
    private UIManager _uiManager;

    [SerializeField]
    private GameObject _rightEngine;

    [SerializeField]
    private GameObject _leftEngine;

    [SerializeField]
    private AudioSource _laserAudio;

    [SerializeField]
    private AudioSource _explosionAudio;

    private GameManager _gameManager;

    private Animator _playerAnim;

    [SerializeField]
    private GameObject _explosionPrefab;

    [SerializeField]
    private bool _isPlayerOne = false;

    [SerializeField]
    private bool _isPlayerTwo = false;


    // Start is called before the first frame update
    void Start()
    {
        

        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        _playerAnim = GetComponent<Animator>();
        

        if (_gameManager._isCoopMode == false)
        {
            transform.position = new Vector3(0, -3, 0);
        }
        else
        {
            Debug.Log("Co Op Mode is True");
        }

        if (_uiManager == null)
        {
            Debug.LogError("UI Manager is NULL");
        }

        if(_spawnManager == null)
        {
            Debug.LogError("Spawn Manager is NULL");
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();



        if(Input.GetKeyDown(KeyCode.Space) && Time.time > _nextFire && _isPlayerOne == true)
        {
            FireLaser();
        }

        if(Input.GetKeyDown(KeyCode.RightControl) && Time.time > _nextFire && _isPlayerTwo == true)
        {
            FireLaser();
        }


    }

    void CalculateMovement()
    {
        if(_isPlayerOne == true)
        {
            float horizontalInput = Input.GetAxis("Horizontal_Player_One");
            float verticalInput = Input.GetAxis("Vertical_Player_One");

            if (Input.GetKeyDown(KeyCode.A))
            {
                _playerAnim.SetBool("Turn_Left", true);
                _playerAnim.SetBool("Turn_Right", false);

            }
            else if (Input.GetKeyUp(KeyCode.A))
            {
                _playerAnim.SetBool("Turn_Left", false);
                _playerAnim.SetBool("Turn_Right", false);

            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                _playerAnim.SetBool("Turn_Right", true);
                _playerAnim.SetBool("Turn_Left", false);

            }
            else if (Input.GetKeyUp(KeyCode.D))
            {
                _playerAnim.SetBool("Turn_Right", false);
                _playerAnim.SetBool("Turn_Left", false);
            }

            transform.Translate(new Vector3(horizontalInput, verticalInput, 0) * _speed * Time.deltaTime);

        } else if(_isPlayerTwo == true)
        {
            float horizontalInput = Input.GetAxis("Horizontal_Player_Two");
            float verticalInput = Input.GetAxis("Vertical_Player_Two");

            if (Input.GetKeyDown(KeyCode.Keypad4))
            {
                _playerAnim.SetBool("Turn_Left", true);
                _playerAnim.SetBool("Turn_Right", false);

            }
            else if (Input.GetKeyUp(KeyCode.Keypad4))
            {
                _playerAnim.SetBool("Turn_Left", false);
                _playerAnim.SetBool("Turn_Right", false);

            }
            else if (Input.GetKeyDown(KeyCode.Keypad6))
            {
                _playerAnim.SetBool("Turn_Right", true);
                _playerAnim.SetBool("Turn_Left", false);

            }
            else if (Input.GetKeyUp(KeyCode.Keypad6))
            {
                _playerAnim.SetBool("Turn_Right", false);
                _playerAnim.SetBool("Turn_Left", false);
            }

            transform.Translate(new Vector3(horizontalInput, verticalInput, 0) * _speed * Time.deltaTime);
        }

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -4.5f, 0), 0);
        
       

        if (transform.position.x >= 11)
        {
            transform.position = new Vector3(-11, transform.position.y, 0);
        }
        else if (transform.position.x <= -11)
        {
            transform.position = new Vector3(11, transform.position.y, 0);
        }

    }

    void FireLaser()
    {

        _nextFire = Time.time + _fireRate;

        if (_isTripleShotActivate)
        {
            Instantiate(_tripleShotPrefab, new Vector3(transform.position.x + -0.3456335f, transform.position.y + -0.09605488f, -2.087145f), Quaternion.identity); ;
        }
        else
        {
            Instantiate(_laserPrefab, new Vector3(transform.position.x, transform.position.y + 1.05f, 0), Quaternion.identity);
        }

        _laserAudio.Play();
        
    }

    public void Damage()
    {

        if(_isShieldActivate == true)
        {
            _isShieldActivate = false;
            _shield.SetActive(false);
        }
        else
        {
            _lives -= 1;

            if(_lives == 2)
            {
                _rightEngine.SetActive(true);
            }
            else if(_lives == 1)
            {
                _leftEngine.SetActive(true);
            }

            if (_lives < 1)
            {
                _spawnManager.OnPlayerDeath();
                Destroy(gameObject);
                _explosionAudio.Play();
                Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            }
        }

        _uiManager.UpdatedLives(_lives);
      

    }

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }

    public void TripleShotActivate()
    {
        _isTripleShotActivate = true;
        StartCoroutine(TripleShotPowerDownRoutine(5.0f));
    }

    public void SpeedBoostActivate()
    {
        _isSpeedBoostActivate = true;
        _speed *= _speedMultiplier;
        StartCoroutine(SpeedBoostPowerDownRoutine(5.0f));
        
    }

    public void ShieldActivate()
    {
        _isShieldActivate = true;
        _shield.SetActive(true);
        StartCoroutine(ShieldPowerDownRoutine(7.0f));
    }


    IEnumerator TripleShotPowerDownRoutine(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        _isTripleShotActivate = false;
    }

    IEnumerator SpeedBoostPowerDownRoutine(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        _isSpeedBoostActivate = false;
        _speed /= _speedMultiplier;
    }

    IEnumerator ShieldPowerDownRoutine(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        _isShieldActivate = false;
        _shield.SetActive(false);
    }
}
