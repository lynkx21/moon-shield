using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateStars : MonoBehaviour
{
    [SerializeField] private GameObject starPrefab;
    [SerializeField] private int starNumber;
    [SerializeField] private float rotationSpeed = 1f;
    float rotationAngle = 0f;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        Random.InitState(2121);
        float height = Camera.main.orthographicSize;
        float width = Camera.main.aspect * height;
        for (int i = 0; i < starNumber; i++)
        {
            Vector3 randomPos = new Vector3(Random.Range(-width * 2f, width * 2f), Random.Range(-height * 2f, height * 2f), 0f);
            Instantiate(starPrefab, randomPos, Quaternion.identity, transform);
        }
    }

    private void Update()
    {
        rotationAngle += rotationSpeed * Time.deltaTime;
        transform.rotation = Quaternion.AngleAxis(rotationAngle, Vector3.forward);
    }
}
