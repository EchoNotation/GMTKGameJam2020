using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEdge : MonoBehaviour
{
    public enum Direction {Right, Up, Left, Down};
    public Direction side;

    private int mapSize;

    // Start is called before the first frame update
    void Start()
    {
        mapSize = this.transform.root.GetComponent<Player>().mapSize;
    }

    void OnCollisonEnter2D(Collision2D col)
    {
        Debug.Log("hit");
        Destroy(gameObject);
    }

}
