using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PrefabDatabase", menuName = "ScriptableObjects/PrefabDatabase", order = 2)]
public class PrefabDB : ScriptableObject
{
    public List<GameObject> Prefabs;
}
