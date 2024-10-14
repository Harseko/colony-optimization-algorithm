using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject cellObject;
    public GameObject baseObject;
    public GameObject field;
    public int startCount;

    // Start is called before the first frame update
    void Start()
    {
        SpriteRenderer meshCollider = field.GetComponent<SpriteRenderer>();
        Dictionary<GameObject, float> ways = new Dictionary<GameObject, float>();
        {
            for (int index = 0; index < 3; index++)
            {
                float screenX = Random.Range(meshCollider.bounds.min.x, meshCollider.bounds.max.x);
                float screenY = Random.Range(meshCollider.bounds.min.y, meshCollider.bounds.max.y);
                Vector2 position = new Vector2(screenX * 0.8f, screenY * 0.8f);
                GameObject obj = Instantiate(baseObject, position, baseObject.transform.rotation);
                ways[obj] = 600f;
            }


            for (int index = 0; index < startCount; index++)
            {
                float screenX = Random.Range(meshCollider.bounds.min.x, meshCollider.bounds.max.x);
                float screenY = Random.Range(meshCollider.bounds.min.y, meshCollider.bounds.max.y);
                Vector2 position = new Vector2(screenX * 0.9f, screenY * 0.9f);
                GameObject obj = Instantiate(cellObject, position, cellObject.transform.rotation);
                obj.GetComponent<CellScript>().ways = new Dictionary<GameObject, float>(ways);
                obj.GetComponent<CellScript>().selectNextFocus();
            }
        }
    }
}
