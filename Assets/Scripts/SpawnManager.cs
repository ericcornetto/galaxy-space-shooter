using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;

    [SerializeField]
    private GameObject _enemyContainer;

    [SerializeField]
    private GameObject[] _powerUp;

    private bool _stopSpawning = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine(5.0f));
        StartCoroutine(SpawnPowerUpRoutine(Random.Range(3, 7)));
    }

    IEnumerator SpawnEnemyRoutine(float waitTime)
    {
        yield return new WaitForSeconds(3.0f);

        while(_stopSpawning == false)
        {
            GameObject newEnemy = Instantiate(_enemyPrefab, new Vector3(Random.Range(-9.46f, 9.46f), 6.51f, 0), Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(waitTime);
        }
    }

    IEnumerator SpawnPowerUpRoutine(float waitTime)
    {
        yield return new WaitForSeconds(3.0f);

        while (_stopSpawning == false)
        {
            int randomPowerUp = Random.Range(0, 3);
            Instantiate(_powerUp[randomPowerUp], new Vector3(Random.Range(-9.46f, 9.46f), 6.51f, 0), Quaternion.identity);
            yield return new WaitForSeconds(waitTime);
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}
