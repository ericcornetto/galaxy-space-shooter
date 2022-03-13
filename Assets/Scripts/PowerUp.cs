using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.0f;

    [SerializeField]
    private int _powerUpId;

    [SerializeField]
    private AudioSource _powerUpAudio;
    // Start is called before the first frame update
    void Start()
    {
        _powerUpAudio = GameObject.Find("Power_Up_Audio").GetComponent<AudioSource>();

        if(_powerUpAudio == null)
        {
            Debug.LogError("Power Up Audio is NULL");
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(0, -1, 0) * _speed * Time.deltaTime);

        if(transform.position.y <= -6.87f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            Player player = collision.GetComponent<Player>();

            if(player != null)
            {
               switch(_powerUpId)
                {
                    case 0:
                        player.TripleShotActivate();
                        _powerUpAudio.Play();
                        break;
                    case 1:
                        player.SpeedBoostActivate();
                        _powerUpAudio.Play();
                        break;
                    case 2:
                        player.ShieldActivate();
                        _powerUpAudio.Play();
                        break;
                    default:
                        Debug.Log("Power Up Undefined");
                        break;
                }
            }
            
            Destroy(gameObject);
        }
    }
}
