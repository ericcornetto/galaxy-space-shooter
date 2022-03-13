using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;

    [SerializeField]
    private Player _player;

    private Animator _enemyAnim;

    [SerializeField]
    private AudioSource _explosionAudio;

    [SerializeField]
    private GameObject _laserPrefab;

    [SerializeField]
    private float _fireRate = 3.0f;
    private float _nextFire = -1f;

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        _explosionAudio = GameObject.Find("Explosion_Audio").GetComponent<AudioSource>();

        if(_player == null)
        {
            Debug.LogError("Player is NULL");
        }

        if(_explosionAudio == null)
        {
            Debug.LogError("Explosion Audio is NULL");
        }

        _enemyAnim = gameObject.GetComponent<Animator>();


    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if(Time.time > _nextFire)
        {
            _fireRate = Random.Range(3, 7);
            _nextFire = _nextFire + _fireRate;
            GameObject enemyLaser = Instantiate(_laserPrefab, transform.position + new Vector3(-0.23f, -1.43f, -77.39558f), Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

            for(int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();
            }
        }

    }

    private void CalculateMovement()
    {
        transform.Translate(new Vector3(0, -1, 0) * _speed * Time.deltaTime);

        if (transform.position.y <= -6.36)
        {
            transform.position = new Vector3(Random.Range(-9.46f, 9.46f), 6.51f, 0);
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.transform.tag == "Player")
        {
            Player player = other.GetComponent<Player>();

            if(player != null)
            {
                player.Damage();
            }

            _enemyAnim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _explosionAudio.Play();

            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            Destroy(gameObject, 2.36f);

        }
        else if(other.transform.tag == "Laser")
        {
            Destroy(other.gameObject);

            if (_player != null)
            {
                
                _player.AddScore(10);
                
            }
            _enemyAnim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _explosionAudio.Play();
            Destroy(GetComponent<Collider2D>());

            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            Destroy(gameObject, 2.36f);
           

        }
    }
}
