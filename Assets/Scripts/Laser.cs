using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 8f;

    [SerializeField]
    private bool _isEnemyLaser = false;

    private GameObject[] _laserEnemy;

    // Start is called before the first frame update
    void Start()
    {
        _laserEnemy = GameObject.FindGameObjectsWithTag("Laser_Enemy");

        if(_laserEnemy == null)
        {
            Debug.Log("Laser Enemy is NULL");
        }
    }

    // Update is called once per frame
    void Update()
    {
       if(_isEnemyLaser == false)
        {
            MoveUp();
        }
       else
        {
            MoveDown();
        }
    }

    private void MoveUp()
    {
        transform.Translate(new Vector3(0, 1, 0) * _speed * Time.deltaTime);

        if (transform.position.y >= 8)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }

            Destroy(gameObject);

        }
    }

    private void MoveDown()
    {
        transform.Translate(new Vector3(0, -1, 0) * _speed * Time.deltaTime);

        if (transform.position.y <= -8)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }

            Destroy(gameObject);

        }
    }

    public void AssignEnemyLaser()
    {
        _isEnemyLaser = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" && _isEnemyLaser == true) {

            Player player = collision.GetComponent<Player>();
            
            foreach(var gameObj in _laserEnemy)
            {
                Destroy(gameObj);
            }

            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }

            if (player != null)
            {
                player.Damage();
            }

        }
    }
}
