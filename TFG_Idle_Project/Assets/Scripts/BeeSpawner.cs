using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeSpawner : MonoBehaviour
{

    [SerializeField]
    private Bee beePrefab;

    // Update is called once per frame
    private void Start()
    {
        StartCoroutine(InstantiateBee());
    }

    IEnumerator InstantiateBee()
    {
        while (true)
        {
            Vector3 BeePos = new Vector2(Random.Range(0f, 1920f), Random.Range(0f, 1080f));
            Quaternion BeeRotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
            Bee newBee = Instantiate(beePrefab,BeePos,BeeRotation);

            yield return new WaitForSeconds(Random.Range(1f, 5f));
        }
    }
}
