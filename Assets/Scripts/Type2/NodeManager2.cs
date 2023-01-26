using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��� �� ���
/// 
/// SearchTargetTile()
/// StartSearchTile()
/// 1. �ִ��� �� �ϳ��� ������ ���� ��ǥ�������� ��´�. 
/// 
/// GetNotWallWithMyUnder()
/// 2. ������ �������⿡ �Ʒ�, �Ʒ� ����, �Ʒ� �������� ���ʷ� Ž���Ѵ�.
/// 
/// GetNotWallWithMySide()
/// 3. Ž�������� IsWall�� �����̶�� �ش� Tile�� �̵��� ��ġ�� ��´�.
/// 
/// 3-1. �ϴ� Ÿ���� ��� ���� ���ٸ�, ����, �������� Ž���Ѱ� IsWall �����̶�� �ش� Tile�� �̵��� ��ġ�� ��´�.
/// 4. �̵��� ��ġ�� ó�� ���� �������� �´��� ���Ѵ�.
/// 4-1. 4�� ����� �����̶�� 2���� �ٽ� �����Ѵ�.
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
                    board_array_master[x, y, EmptyLayer] = 1; //����
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


    
    //�ӽ�: ���� ������ 0~5 = 3,1,2,0,4 

    //������������ x�� �켱������ �迭�� ����ȭ ��Ŵ.
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

        }//¦��
        else if(MAX_X % 2 == 1)
        {
            //��� ���� �˼��ִ� ������ ���ؾ��Ѵ�.
            medain = (MAX_X + 1) / 2;
            finalIndex.Add(medain);           
            while (medain + count < MAX_X)
            {
                count++;
                finalIndex.Add((int)medain - count);
                finalIndex.Add((int)medain + count);
                yield return null;
            }

        }// Ȧ��

        for(int i  =0;  i < finalIndex.Count; i++)
        {
            Debug.Log($"{finalIndex[i]}");
        }
    }


    /// <summary>
    /// StartSearchTile()
    /// 1. Ai�� �����ϰ� ��ġ�Ҽ��ִ� Ÿ���� �ִ��� ���θ� üŬ�Ѵ�.
    /// 2. ���ο� ������ ���ٸ� Ai�� �����Ѵ�.
    /// </summary>
    private void StartSearchTile()
    {

    }
    /// <summary>
    /// [0,2],[0,1],[0,3],[0,0],[0,4]�� �켱������ ��ǥ�������� ��´�
    /// �ش� Ÿ���� isWall�� ���̸� ���� Ÿ�ϰ��� ��ȯ�Ѵ�.
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
