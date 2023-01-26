using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 노드 길 기능
/// 
/// SearchTargetTile()
/// StartSearchTile()
/// 1. 최단의 단 하나의 지점을 최종 목표지점으로 삼는다. 
/// 
/// GetNotWallWithMyUnder()
/// 2. 밑으로 내려가기에 아래, 아래 왼쪽, 아래 오른쪽을 차례로 탐색한다.
/// 
/// GetNotWallWithMySide()
/// 3. 탐색순으로 IsWall이 거짓이라면 해당 Tile을 이동할 위치로 삼는다.
/// 
/// 3-1. 하단 타일이 모두 길이 없다면, 왼쪽, 오른쪽을 탐색한고 IsWall 거짓이라면 해당 Tile을 이동할 위치로 삼는다.
/// 4. 이동한 위치가 처음 정한 목적지가 맞는지 비교한다.
/// 4-1. 4의 결과가 거짓이라면 2번을 다시 진행한다.
/// </summary>

public class NodeManager2 : MonoBehaviour
{
    public GameObject prefab_node;
    public GameObject prefab_Ai;


    public List<Node> RegenNodeList = new List<Node>();

    public int[,,] board_array_master;
    public Node[,] board_array;

    public Transform nodes;
    public Transform entitys;

    public int MAX_X;
    public int MAX_Y;
    public int MAX_LAYER = 10;
    public static NodeManager2 instance;

    private const int EmptyLayer = 0;

    private List<Node> creatnodes = new List<Node>();

    private void Awake()
    {

        instance = this;
    }
    private void Start()
    {
        CreateBaordObject(prefab_node, nodes);

        for(int i =0; i < MAX_X; i++)
        {
            RegenNodeList.Add(board_array[i, MAX_Y - 1]);
        }

        StartCoroutine(SetupLastIndex());
    }

    private void CreateBaordObject(GameObject prefab, Transform parent)
    {
        board_array_master = new int[MAX_X, MAX_Y, MAX_LAYER];
        board_array = new Node[MAX_X, MAX_Y];

        for (int y = MAX_Y - 1; y >= 0; y--)
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
                    board_array_master[x, y, EmptyLayer] = 1; //공허
                }
                catch (Exception e)
                {
                    Debug.LogError($"{x} {y}");
                }


                node.SetPos(x, y);
            }
        }
    }
    private void CreateEntity()
    {
        board_array[3, MAX_Y - 1].CreateEntity();
    }

    private void CreateAi()
    {
       int rnd = UnityEngine.Random.Range(0, MAX_X);
       RegenNodeList[rnd].CreateAi();
    }


    
    //임시: 최종 목적지 0~5 = 3,1,2,0,4 

    //최종목적지의 x축 우선순위를 배열로 목차화 시킴.
    public List<int> finalIndex = new List<int>();

   

    private IEnumerator SetupLastIndex()
    {
        int medain = 0;
        int count = 0;

        if (MAX_X  % 2 == 0)
        {
            medain = MAX_X / 2;
            finalIndex.Add(medain);
            finalIndex.Add(medain + 1);
            while (medain + count < MAX_X)
            {
                count++;
                finalIndex.Add((int)finalIndex[0] - count);
                finalIndex.Add((int)finalIndex[1] + count);
                medain = finalIndex[1];
                yield return null;
            }

        }//짝수
        else if(MAX_X % 2 == 1)
        {
            //가운데 수를 알수있는 공식을 구해야한다.
            medain = (MAX_X + 1) / 2;
            finalIndex.Add(medain);           
            while (medain + count < MAX_X)
            {
                count++;
                finalIndex.Add((int)medain - count);
                finalIndex.Add((int)medain + count);
                yield return null;
            }

        }// 홀수

        for(int i  =0;  i < finalIndex.Count; i++)
        {
            Debug.Log($"{finalIndex[i]}");
        }
    }


    /// <summary>
    /// StartSearchTile()
    /// 1. Ai가 생성하고 배치할수있는 타일이 있는지 여부를 체클한다.
    /// 2. 여부에 문제가 없다면 Ai를 생성한다.
    /// </summary>
    private void StartSearchTile()
    {

    }
    /// <summary>
    /// [0,2],[0,1],[0,3],[0,0],[0,4]를 우선순위의 목표지점으로 삼는다
    /// 해당 타일이 isWall이 참이면 다음 타일값을 반환한다.
    /// </summary>
    public Node GetLastNode()
    {
        for(int y =0; y < MAX_Y; y++)
        {
            for(int x = 0; x < finalIndex.Count; x++)
            {
                Node cur = board_array[x, y];
                if (cur.isWall == false)
                    return cur;
            }
        }
        return null;
    }


    /// <summary>
    /// C = Create
    /// </summary>
    /// 

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.X))
        {
            Debug.Log("?");
            Node cur = GetLastNode();
            if(cur == null)
            {
                Debug.Log("There is Notthing");
            }

            cur.GetComponent<SpriteRenderer>().color = Color.yellow;
            cur.isWall = true;
        }

        if(Input.GetKeyDown(KeyCode.Z))
        {
            CreateAi();
            CreateAi();
        }

    }


}
