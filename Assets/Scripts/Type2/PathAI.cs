using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.XR;

/// <summary>
/// PrefabAi
/// 
/// 1.목적지는 현 노드의 x축이 0이 될때까지
/// 
/// </summary>
public class PathAI : MonoBehaviour
{
    public Node curNode;
    public bool oldisLeft;
    public bool oldisRight;
    /// <summary>
    /// curNode의 초기화는 NodeManager에서 처리한다.
    /// Initialization of curNode is handled by NodeManager.
    /// </summary>
    /// <param name="cur"></param>
    public void Setup(Node cur)
    {
        curNode = cur;
        curNode.isWall = true;
    }

    public void Go()
    {
        StartCoroutine(CoGo());
    }

    private IEnumerator CoGo()
    {
       
        while (curNode.y > 0 || curNode == null)
        {
            Node targetNode = CheckNeighborNode();
            if (gameObject.transform.localPosition != targetNode.transform.localPosition)
            {
                gameObject.transform.localPosition = Vector3.Lerp(gameObject.transform.localPosition, targetNode.transform.localPosition, 0.1f);
            }//목표점에 이동중일시 
            else
            {
                curNode.isWall = false;
                curNode.content2 = null;
                curNode = targetNode;
                curNode.isWall = true;
                targetNode.GetComponent<Node>().content2 = this;                
            }//목표점에 도작했지만 더 내려가야함

            yield return null;
        }
    }
    

    private Node CheckNeighborNode()
    {
        Node node = null;
        // 아래, 사선 왼쪽, 사선 오른쪽, 왼쪽, 오른쪽
       if (curNode.y - 1 >= 0)
       {
            node = NodeManager2.instance.board_array[curNode.x, curNode.y - 1];
            if (node != null && node.isWall == false)
            {
                oldisLeft = false;
                oldisRight = false;
                return node;
            }
        }


        if (curNode.x - 1 >= 0 && curNode.y - 1 >= 0)
        {
            node = NodeManager2.instance.board_array[curNode.x - 1, curNode.y - 1];
            if (node != null && node.isWall == false)
            {
                oldisLeft = false;
                oldisRight = false;
                return node;
            }
        }
        if (curNode.x + 1 < NodeManager2.instance.MAX_X && curNode.y - 1 >= 0)
        {
            node = NodeManager2.instance.board_array[curNode.x + 1, curNode.y - 1];
            if (node != null && node.isWall == false)
            {
                oldisLeft = false;
                oldisRight = false;
                return node;
            }
        }


        if (curNode.x - 1 >= 0)
        {
            node = NodeManager2.instance.board_array[curNode.x - 1, curNode.y];
            if (node != null && node.isWall == false && oldisRight == false)
            {
                oldisLeft = true;
                oldisRight = false;
                return node;
            }
        }
        if (curNode.x + 1 < NodeManager2.instance.MAX_X)
        {
            node = NodeManager2.instance.board_array[curNode.x + 1, curNode.y];
            if (node != null && node.isWall == false && oldisLeft == false)
            {
                oldisLeft = false;
                oldisRight = true;
                return node;
            }
        }




        return null;
    }


}
