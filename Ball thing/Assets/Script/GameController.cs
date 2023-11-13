using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class GameController : MonoBehaviour
{
    public GameObject ringPrefab;
    public int ringCount = 10;
    public Player player;

    void Start()
    {
        StartCoroutine(SpawnRings(ringCount));
    }

    bool isLucky = false;
    IEnumerator SpawnRings(int amount)
    {
        List<int> variants = new List<int>();
        int lastVariant = 0;
        for(int i = 0; i < amount; i++)
        {
            if(Random.value > 0.5f) { isLucky = true; }
            if (isLucky)
            {
                variants.Add(lastVariant);
            }
            else
            {
                variants.Add(Random.Range(0, 4));
                lastVariant = variants[i];
            }
            if(Random.value > 0.7f) { isLucky = false; }
        }

        for (int i = 0; i < amount; i++)
        {
            yield return new WaitUntil(() => i - player.score < 10);
            var ring = Instantiate(ringPrefab,new Vector3(0, 0 + -i * 0.71f, 0), Quaternion.Euler(0, variants[i] * 90 + transform.eulerAngles.y,0), transform);
            var pieces = ring.GetComponentsInChildren<Renderer>();
        }
    }
}