using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Camera cam;
    public int maxSize = 10;
    public List<Enemy> enemies;

    public float maxSpawningTime;
    private float timeElapsed;

    private int enemyNumber;
    private Vector2 boundX;
    private Vector2 boundY;

    private BoxCollider2D box;

    // Start is called before the first frame update
    void Start()
    {
        box = GetComponent<BoxCollider2D>();
        GetMapBound();
        Debug.Log(boundX);
        Debug.Log(boundY);
    }

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime;

        if(timeElapsed >= maxSpawningTime)
        {
            TryToSpawnEnemy(10);
            timeElapsed = 0;
        }
        
    }

    /// <summary>
    /// Continuously trying to spawn a random enemy with n trials until an enemy is spawn
    /// </summary>
    /// <param name="n">number of trials</param>
    /// <returns></returns>
    public bool TryToSpawnEnemy(int n)
    {
        while(n > 0)
        {
            Vector2 randPos = new Vector2(Random.Range(boundX.x, boundX.y), Random.Range(boundY.x, boundY.y));
            if(IsValidSpawnLocation(randPos))
            {
                SpawnRandomEnemy(randPos);
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Check for valid spawning location
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    private bool IsValidSpawnLocation(Vector2 position)
    {
        Vector2 wallDelta = new Vector2(1, 1);
        Collider2D collider = Physics2D.OverlapBox(position, wallDelta, 0);
        (Vector2, Vector2) camBound = GetCameraBound();
        bool isCollidedObject = collider != null;
        bool isWithinCamBound = position.x > camBound.Item1.x && position.x < camBound.Item1.y &&
                                position.y > camBound.Item2.x && position.y < camBound.Item2.y;


        return !(isCollidedObject && isWithinCamBound);
    }

    /// <summary>
    /// Spawn a random enemy given a location
    /// </summary>
    /// <param name="position"></param>
    private void SpawnRandomEnemy(Vector2 position)
    {
        Instantiate(enemies[Random.Range(0, enemies.Count - 1)], position, Quaternion.identity);
        enemyNumber++;
    }

    //////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Helper methods for getting map and camera bounds.
    /// </summary>

    private void GetMapBound()
    {
        Vector2 boxSize = box.size;
        Vector2 spawnerLocation = transform.position;
        boundX = new Vector2(spawnerLocation.x - (boxSize.x / 2), spawnerLocation.x + (boxSize.x / 2));
        boundY = new Vector2(spawnerLocation.y - (boxSize.y / 2), spawnerLocation.y + (boxSize.y / 2));
    }

    private (Vector2, Vector2) GetCameraBound()
    {
        Vector2 location = cam.transform.position;
        float camRadius = cam.orthographicSize / 2;

        return (new Vector2(location.x - camRadius, location.x + camRadius), 
                new Vector2(location.y - camRadius, location.y + camRadius));
    }
}
