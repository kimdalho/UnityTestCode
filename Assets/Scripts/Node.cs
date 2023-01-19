using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

/// <summary>
/// 1. 입구에서 생성된 노드는 바로 아래로 일정한 속도를 가지고 내려간다.
/// 2. 
/// </summary>
public class Node : MonoBehaviour
{
    public Entity content;
    public int x, y;

    //isDiagonalFall
    public GameObject check;

    //iscontent
    public GameObject check2;

    public bool isWall;

    public void SetPos(int _x, int _y)
    {
        this.x = _x;
        this.y = _y;
    }

    bool isDiagonalFall = false;

    private void Update()
    {
        check.SetActive(isDiagonalFall);

        if(content == null)
            check2.SetActive(true);
        else
        {
            check2.SetActive(false);
        }

        if(isWall)
        {
            GetComponent<SpriteRenderer>().color = Color.black;
        }
        else
        {
            GetComponent<SpriteRenderer>().color = Color.white;
        }

    }

    public void CreateEntity()
    {
        GameObject newEntity = Instantiate(NodeManager.instance.prefab_entity);
        content = newEntity.GetComponent<Entity>();
        content.transform.position = this.transform.position;
        content.transform.SetParent(NodeManager.instance.entitys);
        NodeManager.instance.fallingNodes.Add(this);
    }


    private void EndFalling()
    {
        Debug.Log("바닥에 정착");
        //Check_diagonal_falling();
        //왼쪽에서 아래 오른쪽
        NodeManager.instance.board_array_master[x, y, 0] = 0;
        NodeManager.instance.fallingNodes.Remove(this);
        Debug.Log($"X: {x}Y: {y}");
        isDiagonalFall = false;
        LR = -1;
    }


    private int LR = -1;

    private Node GetTargetNode(bool check)
    {
        Node targetNode = null;


        int underY = y - 1;

        if (check == false)
        {
            targetNode = NodeManager.instance.board_array[x, underY];
        }
        else
        {
            if (LR == 0) // 왼쪽
            {
                int num = x - 1;
                while (num >= 0)
                {
                    if (NodeManager.instance.board_array_master[num, underY, 0] == 1)
                    {
                        isDiagonalFall = true;
                        targetNode = NodeManager.instance.board_array[num, underY];

                        return targetNode;
                    }
                    else
                    {
                        num--;
                    }
                }
            }
            else // 오른쪽
            {
                int num = x + 1;

                while (num < NodeManager.instance.MAX_X - 1)
                {
                    if (NodeManager.instance.board_array_master[num, underY, 0] == 1)
                    {
                        isDiagonalFall = true;
                        targetNode = NodeManager.instance.board_array[num, underY];
                        return targetNode;
                    }
                    else
                    {
                        num++;
                    }
                }
            }
        }
        return targetNode;
    }


        private Node GetTargetNode()
    {
        Node targetNode = null;
        /*

                int underY = y - 1;

                if (check == false)
                {
                    targetNode = NodeManager.instance.board_array[x, underY];
                }
                else
                {
                    if (LR == 0) // 왼쪽
                    {
                        int num = x - 1;
                        while (num >= 0)
                        {
                            if (NodeManager.instance.board_array_master[num, underY, 0] == 1)
                            {
                                isDiagonalFall = true;
                                targetNode = NodeManager.instance.board_array[num, underY];

                                return targetNode;
                            }
                            else
                            {
                                num--;
                            }
                        }
                    }
                    else // 오른쪽
                    {
                        int num = x + 1;

                        while (num < NodeManager.instance.MAX_X - 1)
                        {
                            if (NodeManager.instance.board_array_master[num, underY, 0] == 1)
                            {
                                isDiagonalFall = true;
                                targetNode = NodeManager.instance.board_array[num, underY];
                                return targetNode;
                            }
                            else
                            {
                                num++;
                            }
                        }
                    }
                }*/

        Queue<Node> buffer = new Queue<Node>();
        for(int i =0; i < NodeManager.instance.MAX_X; i++)
        {
            if (NodeManager.instance.board_array_master[i, y-1, 0] == 1 &&
                NodeManager.instance.board_array[i, y - 1].isWall == false)
            {
                buffer.Enqueue(NodeManager.instance.board_array[i, y - 1]);
            }
        }

        targetNode = buffer.Dequeue();

        if(targetNode == null)
        {
            Debug.Log("thereis 함수 버그");
        }

       
        while (buffer.Count > 0)
        {
           Node next = buffer.Dequeue();

          float target_Distance = Vector2.Distance(targetNode.transform.localPosition, gameObject.transform.localPosition);
          float next_Distance = Vector2.Distance(next.transform.localPosition, gameObject.transform.localPosition);

            if(next_Distance >= target_Distance)
            {
                

                continue;
            }
            else if(next_Distance < target_Distance)
            {
                Debug.Log($"old {targetNode.x} {targetNode.y}  next {next.x} {next.y}");


                targetNode = next;
            }

        }


        return targetNode;
    }




    public void Falling()
    {
        if (!ThereIsEmpty() == true)
        {
            EndFalling();

        }//도착
        else
        {
            Node targetNode = GetTargetNode();

            try
            {
                if (content.transform.localPosition != targetNode.transform.localPosition)
                {
                    content.transform.localPosition = Vector3.Lerp(content.transform.localPosition, targetNode.transform.localPosition, 0.1f);
                }//목표점에 이동중일시 
                else
                {
                    targetNode.GetComponent<Node>().content = content;
                    content = null;
                    NodeManager.instance.fallingNodes.Remove(this);
                    isDiagonalFall = false;
                    NodeManager.instance.fallingNodes.Add(targetNode.GetComponent<Node>());
                }//목표점에 도작했지만 더 내려가야함

            }
            catch (System.Exception e)
            {
                if(content == null)
                {
                    Debug.Log("is Null");
                }
            }



        }
    }

    private bool isMoving;



    private bool ThereIsEmpty()
    {
        if (isDiagonalFall == true)
            return true;

        if (CheckDir() || 
            Check_diagonal_falling_Right() ||
            Check_diagonal_falling_Left()) // That was mean Empty
        {
            return true;
        }
        else
        {
            return false;
        }
        
    }

    private bool CheckDir()
    {
        if (y - 1 < 0)
        {
            Debug.Log($"check");
            return false;
        }//맨 밑 도착

        if (NodeManager.instance.board_array[x, y - 1].isWall)
        {
            return false;
        }

        if(NodeManager.instance.board_array_master[x, y - 1, 0] == 1)
        {

            return true;
        }
        else
        {
            return false;
        }
    }

    private bool Check_diagonal_falling_Right()
    {
        if (y == 0)
            return false;


        int rightX = x + 1;

        if (rightX == NodeManager.instance.MAX_X)
            return false;

        if (NodeManager.instance.board_array[rightX, y - 1].isWall)
        {
            return false;
        }

       
        
        while (rightX < NodeManager.instance.MAX_X -1)
        {
            if(NodeManager.instance.board_array_master[rightX, y - 1, 0] == 1 &&
               NodeManager.instance.board_array[rightX, y - 1].isWall == false)
            {
                isDiagonalFall = true;
                LR = 1;
                return true;
            }
            else
            {
                rightX++;
            }
        }

        return false;




    }

    private bool Check_diagonal_falling_Left()
    {
        //범위검사
        if (y == 0)
            return false;
        if (x - 1 < 0)
            return false;

        //벽검사
        if (NodeManager.instance.board_array[x - 1, y - 1].isWall)
        {
            return false;
        }
/*
        if (NodeManager.instance.board_array_master[x - 1, y - 1, 0] == 1) // That was mean Empty;
        {
            //Debug.Log("Check");
            isDiagonalFall = true;
            //NodeManager.instance.fallingNodes.Add(this);
            LR = 0;
            return true;
        }//목표점에 도작했지만 더 내려가야함*/

        int num = x -1;

        while (num >= 0 )
        {
            if (NodeManager.instance.board_array_master[num, y - 1, 0] == 1 &&
                NodeManager.instance.board_array[num, y - 1].isWall == false)
            {
                isDiagonalFall = true;
                LR = 0;
                return true;
            }
            else
            {
                num--;
            }
        }
        return false;
    }
}
