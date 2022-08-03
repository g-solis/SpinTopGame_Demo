using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteoritesController : MonoBehaviour
{
    [SerializeField] List<GameObject> meteoritesPool = new List<GameObject>();
    [SerializeField] float MeteoriteSpeed = 10;
    [SerializeField] Vector2 MeteoriteCooldownRange = new Vector2(0.5f,2.5f);

    private void OnEnable()
    {
        foreach(GameObject go in meteoritesPool)
        {
            go.transform.localPosition =
                new Vector3(go.transform.position.x,Random.Range(-150,150),Random.Range(-150,150));

            go.transform.rotation = Random.rotation;

            CheckSecret(go.transform);
        }

        StartCoroutine(MeteoriteSpawnCoroutine());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void Update()
    {
        Vector3 movement = Vector3.right * MeteoriteSpeed * Time.deltaTime;

        foreach(GameObject go in meteoritesPool)
        {
            go.transform.position += movement;
        }
    }

    private IEnumerator MeteoriteSpawnCoroutine()
    {
        while(true)
        {
            if(meteoritesPool.Count == 0) yield return new WaitUntil(() => meteoritesPool.Count > 0);

            GameObject meteorite = meteoritesPool[0];

            for(int i=0;i<meteoritesPool.Count;i++)
            {
                GameObject temp_m = meteoritesPool[i];
                if(meteorite.transform.position.x < temp_m.transform.position.x)
                {
                    meteorite = temp_m;
                }
            }

            Vector3 startPos = new Vector3(-700,Random.Range(-150,150),Random.Range(-150,150));
            meteorite.transform.localPosition = startPos;
            meteorite.transform.rotation = Random.rotation;

            CheckSecret(meteorite.transform);

            yield return new WaitForSeconds(RandomUtils.Vector2Range(MeteoriteCooldownRange));
        }
    }

    private void CheckSecret(Transform tr)
    {
            bool secret = Random.Range(0,50) == 0;
            tr.GetChild(0).gameObject.SetActive(!secret);
            tr.GetChild(1).gameObject.SetActive(secret);
    }
}
