using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private List<GameObject> enemies;
    [SerializeField] private GameObject arrowPrefab;
    private ScreenBounds screenBounds;
    private List<GameObject> enemiesToRemove;

    // Difficulty variables
    [Header("Difficulty Management")]
    [SerializeField] private float spawnRate = 0.1f;  // enemy/s
    [SerializeField] private float minSpawnRate = 0.1f;  // enemy/s
    [SerializeField] private float maxSpawnRate = 2f; // enemy/s
    [SerializeField] private float timeToReachMaxSR = 180f; // seconds
    private float timeToSpawn;

    // Enemy Choice variables
    [Header("Enemy Choice")]
    [SerializeField] private float[] currentEnemyWeights;
    [SerializeField] private EnemyWeight[] enemyWeightLimits;

    [Header("Debug")]
    [SerializeField] private float pickedNumber;
    [SerializeField] private float computedPerc;


    private void Start()
    {
        enemies = new List<GameObject>();
        screenBounds = GetBounds();
        enemiesToRemove = new List<GameObject>();

        currentEnemyWeights = new float[enemyPrefabs.Length];
        for (int i = 0; i < currentEnemyWeights.Length; i++)
        {
            currentEnemyWeights[i] = enemyWeightLimits[i].firstPerc;
        }
    }

    private void Update()
    {
        if (timeToSpawn <= 0)
        // if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnEnemy();

            // Lerp
            float t = Time.timeSinceLevelLoad / timeToReachMaxSR;
            spawnRate = Mathf.Lerp(minSpawnRate, maxSpawnRate, t * t);
            timeToSpawn = 1 / spawnRate;
        }
        
        timeToSpawn -= Time.deltaTime;

        foreach (GameObject enemy in enemies)
        {
            if (enemy != null)
            {
                Vector3 enemyPosition = enemy.transform.position;
                if (CheckIfOutsideScreen(enemyPosition))
                {
                    float arrowAngle;
                    Vector3 intersectionPoint = FindClosestIntersectionPointWithScreenBounds(enemyPosition, Vector3.zero,
                        screenBounds, out arrowAngle);

                    if (enemy.transform.childCount > 0)
                    {
                        enemy.transform.GetChild(0).position = intersectionPoint;
                        enemy.transform.GetChild(0).rotation = Quaternion.AngleAxis(arrowAngle, Vector3.forward);
                    }

                    // Debug.DrawLine(enemyPosition, Vector3.zero, Color.white, 100f);
                    if (enemy.transform.childCount == 0)
                    {
                        GameObject arrow = Instantiate(arrowPrefab, intersectionPoint,
                            Quaternion.AngleAxis(arrowAngle, Vector3.forward), enemy.transform);
                    }
                }
                else
                {
                    if (enemy.transform.childCount > 0)
                    {
                        Destroy(enemy.transform.GetChild(0).gameObject);
                    }
                }
            }
            else
            {
                enemiesToRemove.Add(enemy);
            }
        }

        for (int i = 0; i < enemiesToRemove.Count; i++)
        {
            enemies.Remove(enemiesToRemove[i]);
        }
        enemiesToRemove.Clear();
        
    }

    private void SpawnEnemy()
    {
        float spawnPosX;
        float spawnPosY;
        Vector3 spawnPosition;
        do
        {
            spawnPosX = Random.Range(screenBounds.lowerX * 2.5f, screenBounds.upperX * 2.5f);
            spawnPosY = Random.Range(screenBounds.lowerY * 2.5f, screenBounds.upperY * 2.5f);
            spawnPosition = new Vector3(spawnPosX, spawnPosY, 0f);
        }
        while (!CheckIfOutsideScreen(spawnPosition, 1.5f));


        int enemyIndex = SelectEnemy();
        // enemyIndex = Random.Range(0, enemyPrefabs.Length);
        GameObject go = Instantiate(enemyPrefabs[enemyIndex], spawnPosition, Quaternion.identity, transform);
        enemies.Add(go);
    }

    public void UpdateEnemyWeights(float t)
    {
        // NOTE(leo): this is ugly as fuck, but fix it only if we have time
        float perc;

        if (t < 30f)
        {
            perc = t / 30f;

            // Basic enemy
            enemyWeightLimits[0].firstPerc = 100f;
            enemyWeightLimits[0].lastPerc = 50f;

            // Sin enemy
            enemyWeightLimits[1].firstPerc = 0f;
            enemyWeightLimits[1].lastPerc = 50f;

            // Random enemy
            enemyWeightLimits[2].firstPerc = 0f;
            enemyWeightLimits[2].lastPerc = 0f;
        }
        else if (t < 50f)
        {
            perc = (t - 30f) / (50f - 30f);

            // Basic enemy
            enemyWeightLimits[0].firstPerc = 50f;
            enemyWeightLimits[0].lastPerc = 25f;

            // Sin enemy
            enemyWeightLimits[1].firstPerc = 50f;
            enemyWeightLimits[1].lastPerc = 50f;

            // Random enemy
            enemyWeightLimits[2].firstPerc = 0f;
            enemyWeightLimits[2].lastPerc = 25f;
        }
        else if (t < 70f)
        {
            perc = (t - 50f) / (70f - 50f);

            // Basic enemy
            enemyWeightLimits[0].firstPerc = 25f;
            enemyWeightLimits[0].lastPerc = 20f;

            // Sin enemy
            enemyWeightLimits[1].firstPerc = 50f;
            enemyWeightLimits[1].lastPerc = 50f;

            // Random enemy
            enemyWeightLimits[2].firstPerc = 25f;
            enemyWeightLimits[2].lastPerc = 30f;
        }
        else
        {
            perc = (t - 70f) / (90f - 70f);

            // Basic enemy
            enemyWeightLimits[0].firstPerc = 20f;
            enemyWeightLimits[0].lastPerc = 10f;

            // Sin enemy
            enemyWeightLimits[1].firstPerc = 50f;
            enemyWeightLimits[1].lastPerc = 30f;

            // Random enemy
            enemyWeightLimits[2].firstPerc = 30f;
            enemyWeightLimits[2].lastPerc = 60f;
        }

        for (int i = 0; i < currentEnemyWeights.Length; i++)
        {
            computedPerc = perc;
            currentEnemyWeights[i] = Mathf.Lerp(enemyWeightLimits[i].firstPerc, enemyWeightLimits[i].lastPerc,
                Mathf.Clamp(perc, 0, 1));
        }
    }

    public int SelectEnemy()
    {
        float t = Time.timeSinceLevelLoad;
        UpdateEnemyWeights(t);

        float randomValue = Random.Range(0f, 100f);
        float offset = 0f;
        pickedNumber = randomValue;

        for (int i = 0; i < currentEnemyWeights.Length; i++)
        {
            if (randomValue < currentEnemyWeights[i] + offset)
            {
                return i;
            }
            else
            {
                offset += currentEnemyWeights[i];
            }
        }

        return -1;
    }

    public static bool LineLineIntersection(out Vector3 intersection, Vector3 linePoint1,
        Vector3 lineVec1, Vector3 linePoint2, Vector3 lineVec2)
    {
        Debug.DrawLine(linePoint1, (lineVec1 + linePoint1), Color.red, Time.deltaTime);
        Debug.DrawLine(linePoint2, (lineVec2 + linePoint2), Color.red, Time.deltaTime);

        Vector3 lineVec3 = linePoint2 - linePoint1;
        Vector3 crossVec1and2 = Vector3.Cross(lineVec1, lineVec2);
        Vector3 crossVec3and2 = Vector3.Cross(lineVec3, lineVec2);

        float planarFactor = Vector3.Dot(lineVec3, crossVec1and2);

        //is coplanar, and not parrallel
        if (Mathf.Abs(planarFactor) < 0.0001f && crossVec1and2.sqrMagnitude > 0.0001f)
        {
            float s = Vector3.Dot(crossVec3and2, crossVec1and2) / crossVec1and2.sqrMagnitude;
            intersection = linePoint1 + (lineVec1 * s);
            if (IsCBetweenAB(linePoint2, linePoint2 + lineVec2, intersection))
            // if (Vector3.Distance(intersection, linePoint1 + lineVec1) < Vector3.Distance(linePoint1, linePoint1 + lineVec1))
                return true;
        }
        intersection = Vector3.zero;
        return false;
    }

    private static bool IsCBetweenAB(Vector3 A, Vector3 B, Vector3 C)
    {
        return Vector3.Dot((B - A).normalized, (C - B).normalized) < 0f && Vector3.Dot((A - B).normalized, (C - A).normalized) < 0f;
    }

    private Vector3 FindClosestIntersectionPointWithScreenBounds(Vector3 position, Vector3 destination,
        ScreenBounds sb, out float arrowAngle)
    {
        Vector3 closestIntersection = Vector3.zero;
        arrowAngle = 0f;
        Vector3[] closestBound = new Vector3[2];

        Vector3[] bottomBound = { new Vector3(sb.lowerX, sb.lowerY), new Vector3(sb.upperX, sb.lowerY) };
        Vector3[] topBound = { new Vector3(sb.lowerX, sb.upperY), new Vector3(sb.upperX, sb.upperY) };
        Vector3[] leftBound = { new Vector3(sb.lowerX, sb.upperY), new Vector3(sb.lowerX, sb.lowerY) }; 
        Vector3[] rightBound = { new Vector3(sb.upperX, sb.lowerY), new Vector3(sb.upperX, sb.upperY) };
        Vector3[][] bounds = { bottomBound, topBound, leftBound, rightBound };
        float[] arrowAngles = { 0f, 180f, -90f, 90f };

        for (int i = 0; i < bounds.Length; i++)
        {
            // Vector3 intersection = FindIntersectionPoint(position, destination, bound[0], bound[1]);

            Vector3 intersection;
            bool existsIntersection = LineLineIntersection(out intersection, position, destination - position,
                bounds[i][0], bounds[i][1] - bounds[i][0]);

            if (Vector3.Distance(position, intersection) < Vector3.Distance(position, closestIntersection))
            {
                closestIntersection = intersection;
                arrowAngle = arrowAngles[i];
                closestBound[0] = bounds[i][0];
                closestBound[1] = bounds[i][1];
            }
        }

        // Debug.Log($"[EnemySpawner] Closest bound: {closestBound[0]}, {closestBound[1]}");

        return closestIntersection;
    }



    private bool CheckIfOutsideScreen(Vector3 position, float multiplier=1f)
    {
        return (position.x < screenBounds.lowerX * multiplier || position.x > screenBounds.upperX * multiplier ||
                position.y < screenBounds.lowerY * multiplier || position.y > screenBounds.upperY * multiplier);
    }

    private ScreenBounds GetBounds()
    {
        float screenHeight = Camera.main.orthographicSize * 2f;
        float screenWidth = Camera.main.aspect * screenHeight;
        float lowerX = -screenWidth / 2f;
        float upperX = screenWidth / 2f;
        float lowerY = -screenHeight / 2f;
        float upperY = screenHeight / 2f;
        return new ScreenBounds(lowerX, upperX, lowerY, upperY);
    }
}

public struct ScreenBounds
{
    public float lowerX;
    public float upperX;
    public float lowerY;
    public float upperY;

    public ScreenBounds(float lX, float uX, float lY, float uY)
    {
        lowerX = lX;
        upperX = uX;
        lowerY = lY;
        upperY = uY;
    }
}
