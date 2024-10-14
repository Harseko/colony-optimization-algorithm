using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Experimental.GraphView.GraphView;

public class CellScript : MonoBehaviour
{
    Rigidbody2D rigidbody;

    public float movement;
    public Vector2 direction;
    public Dictionary<GameObject, float> ways;
    public GameObject focus;

    private float time = 0.0f;
    public float interpolationPeriod = 0.1f;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        direction = DegreeToVector2(UnityEngine.Random.Range(0.0f, 360.0f)) * Mathf.Max(movement, 0f);
        Physics2D.IgnoreLayerCollision(8, 8);
    }

    void Update()
    {
        rigidbody.velocity = direction.normalized * movement;

        time += Time.deltaTime;
        if (time >= interpolationPeriod)
        {
            List<GameObject> keys = new List<GameObject>(ways.Keys);
            foreach (GameObject key in keys)
            {
                ways[key] = ways[key] + 1;
            }
            Listen();
            time = time - interpolationPeriod;
        }
    }

    //private void FixedUpdate()
    //{

    //    //rigidbody.velocity = direction.normalized * movement;

    //    time += Time.deltaTime;
    //    if (time >= interpolationPeriod)
    //    {
    //        List<GameObject> keys = new List<GameObject>(ways.Keys);
    //        foreach (GameObject key in keys)
    //        {
    //            ways[key] = ways[key] + 1;
    //        }
    //        Listen();
    //        time = time - interpolationPeriod;
    //    }
    //}

    private Vector2 RadianToVector2(float radian)
    {
        return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
    }

    private Vector2 DegreeToVector2(float degree)
    {
        return RadianToVector2(degree * Mathf.Deg2Rad);
    }

    private float Vector2ToDegree(Vector2 vector)
    {
        float value = (float)((Mathf.Atan2(vector.x, vector.y) / Math.PI) * 180f);
        if (value < 0) value += 360f;

        return value;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 10)
        {
            direction = Vector2.Reflect(direction.normalized, collision.contacts[0].normal);
        }
    }

    public void handleCollissionWithBase(Collider2D collision)
    {
        if (focus == collision.gameObject)
        {
            ways[collision.gameObject] = 1;
            selectNextFocus();
            direction = -(direction.normalized);
            Listen();
        }
        else
        {
            ways[collision.gameObject] = 0;
        }
    }

    public void handleFindBaseTrigger(Collider2D collision)
    {
        direction = ((Vector2)collision.gameObject.transform.position - (Vector2)transform.position).normalized;
    }

    private GameObject[] GetCellsInRange()
    {
        float range = 1.0f;
        int layer = LayerMask.NameToLayer("Cell");
        GameObject[] objects = FindObjectsOfType(typeof(GameObject)) as GameObject[];
        var objInRange = new List<GameObject>();
        foreach (GameObject obj in objects)
        {
            if ((obj.layer == layer) && (obj != this))
            {
                Vector2 offset = obj.transform.position - transform.position;
                float sqrLen = offset.sqrMagnitude;

                // square the distance we compare with
                if (sqrLen < range * range)
                {
                    objInRange.Add(obj);
                }
            }
        }
        return objInRange.ToArray();
    }

    private void Listen()
    {
        float offset = 1f;
        var cells = GetCellsInRange();
        foreach (GameObject cell in cells)
        {
            var w = cell.GetComponent<CellScript>().ways;
            foreach (KeyValuePair<GameObject, float> item in w)
            {
                if (ways[item.Key] > (item.Value + offset))
                {
                    ways[item.Key] = item.Value + offset;
                    if (focus == item.Key)
                    {
                        direction = ((Vector2)cell.transform.position - (Vector2)transform.position).normalized;
                    }
                }
            }
        }
    }

    public void selectNextFocus()
    {
        List<GameObject> keyList = new List<GameObject>(ways.Keys);
        var random = new System.Random();
        int i;
        do
        {
            i = random.Next(keyList.Count);
        } while (focus == keyList[i]);
        focus = keyList[i];
    }
}
