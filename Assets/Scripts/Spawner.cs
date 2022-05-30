using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject[] _powerUp; 
    private bool _stopSpawning = false;

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }
    void Update()
    {
        if(_stopSpawning == true)
        {
            Destroy(this.gameObject);
        }
    }
    IEnumerator SpawnEnemyRoutine()
    {
        while (_stopSpawning == false)
        {
            yield return new WaitForSeconds(1.0f);
            Vector3 posToSpawn = new Vector3(Random.Range(-10f, 10f), 8.3f, 0); 
            GameObject newEnemy =  Instantiate(_enemyPrefab,posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(1.0f);
        }
    }

    IEnumerator SpawnPowerupRoutine()
    {
        while (_stopSpawning == false)
        {
            yield return new WaitForSeconds(3.0f);
            Vector3 posToSpawn = new Vector3(Random.Range(-10f, 10f), 8.3f, 0);
            int randomPowerUp = Random.Range(0, _powerUp.Length);
            if (_powerUp[randomPowerUp] != null)
            {
                GameObject newPower = Instantiate(_powerUp[randomPowerUp], posToSpawn, Quaternion.identity);

                yield return new WaitForSeconds(Random.Range(5, 15));
            }
            else
            {
                yield return new WaitForSeconds(1);
            }    
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}
