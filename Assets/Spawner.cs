using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    const float DANGER_DISTANCE = 4;
    const float SPAWN_TELL_TIME = 2;

    [SerializeField] Transform _bound1;
    [SerializeField] Transform _bound2;

    private int _preparing = 0;

    private int EnemyCount => GameObject.FindGameObjectsWithTag("Enemy").Length + _preparing;

    [SerializeField] GameObject _spawnTellPrefab;
    [SerializeField] GameObject _enemy;

    private void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            if (EnemyCount < Mathf.Sqrt(GameManager.Instance.Score * 5) + 5)
                StartCoroutine(Spawn(_enemy));

            yield return new WaitForSeconds(1);
        }
    }

    private IEnumerator Spawn(GameObject enemy)
    {
        Vector3 spawnPoint = GetSpawnPoint();

        //tell
        Destroy(Instantiate(_spawnTellPrefab, spawnPoint, Quaternion.identity), SPAWN_TELL_TIME);
        _preparing++;

        yield return new WaitForSeconds(SPAWN_TELL_TIME);

        _preparing--;
        Instantiate(enemy, spawnPoint, Quaternion.identity);
        
    }


    private Vector3 GetSpawnPoint()
    {
        Vector3 point = new Vector3(
            Random.Range(_bound1.transform.position.x, _bound2.transform.position.x),
            Random.Range(_bound1.transform.position.y, _bound2.transform.position.y));

        if (Vector3.Distance(Player.Instance.transform.position, point) < DANGER_DISTANCE)
            return GetSpawnPoint();
        
        return point;
    }
}
