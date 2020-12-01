using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New EnemyWeight", menuName = "Enemy/Enemy Weight")]
[Serializable]
public class EnemyWeight : ScriptableObject
{
    [SerializeField] public float firstPerc;
    [SerializeField] public float lastPerc;
}
    