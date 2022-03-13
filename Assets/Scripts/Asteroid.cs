using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{

    [SerializeField]
    private float _speed = 15.0f;

    [SerializeField]
    private GameObject _explosionPrefab;

    [SerializeField]
    private SpawnManager _spawnManager;

    [SerializeField]
    private AudioSource _explosionAudio;

    // Start is called before the first frame update
    void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(transform.rotation.x, transform.rotation.y, -1) * _speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Laser")
        {
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(collision.gameObject);
            _spawnManager.StartSpawning();
            _explosionAudio.Play();
            Destroy(gameObject, 0.25f);
        }
    }
}
