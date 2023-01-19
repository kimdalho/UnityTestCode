using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Text;
using System;

public class NodeManager : MonoBehaviour
{
    public GameObject prefab_node;
    public GameObject prefab_entity;

    public int[,,] board_array_master;
    public Node[,] board_array;
    public List<Node> fallingNodes = new List<Node>();

    public Transform nodes;
    public Transform entitys;

    public int MAX_X;
    public int MAX_Y;
    public int MAX_LAYER = 10;
    public static NodeManager instance;

    private const int EmptyLayer = 0;

    private List<Node> creatnodes = new List<Node>();

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        CreateBaordObject(prefab_node, nodes);


        //board_array[3,3].isWall = true;
        //board_array[4,2].isWall = true;

        board_array[0, 4].isWall = true;
        board_array[1, 4].isWall = true;
        board_array[3, 4].isWall = true;

        board_array[4, 4].isWall = true;
        board_array[5, 4].isWall = true;

        board_array[7, 4].isWall = true;
        board_array[8, 4].isWall = true;
        board_array[9, 4].isWall = true;

        board_array[0, 3].isWall = true;
        board_array[1, 3].isWall = true;
        board_array[3, 3].isWall = true;

        board_array[4, 3].isWall = true;
        board_array[5, 3].isWall = true;

        board_array[7, 3].isWall = true;
        board_array[8, 3].isWall = true;
        board_array[9, 3].isWall = true;


        StartCoroutine(CoGen());
    }



    private void CreateBaordObject(GameObject prefab, Transform parent)
    {
        board_array_master = new int[MAX_X, MAX_Y, MAX_LAYER];
        board_array = new Node[MAX_X, MAX_Y];

        for (int y = MAX_Y-1; y >= 0; y--)
        {
            for (int x = 0; x < MAX_X; x++)
            {
                GameObject go = Instantiate(prefab) as GameObject;
                go.transform.position = new Vector3(x * 1.5f, y * 1.5f, 0);
                go.transform.SetParent(parent, false);

                var node = go.GetComponent<Node>();
                try
                {
                    board_array[x, y] = node;
                    board_array_master[x, y, EmptyLayer] = 1; //°øÇã
                }
                catch(Exception e)
                {
                    Debug.LogError($"{x} {y}");
                }
                
                
                node.SetPos(x, y);
            }
        }
    }
    
    
    private void CreateEntity()
    {
        board_array[3, MAX_Y-1].CreateEntity();
    }

    private void FallingManager()
    {
        for(int i =0; i < fallingNodes.Count; i++)
        {
            fallingNodes[i].Falling();
        }
    }

    /// <summary>
    /// C = Create
    /// </summary>
    /// 

    
    private IEnumerator CoGen()
    {
        while(true)
        {
            yield return new WaitForSeconds(1);
            CreateEntity();
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            CreateEntity();
        }

        if(fallingNodes.Count == 0)
        {
            Debug.Log("1");
            return;
        }
        else
        {
            Debug.Log("2");
            FallingManager();
        }
    }
}
