using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// PrefabAi
/// 
/// 1. 최단의 단 하나의 지점을 최종 목표지점으로 삼는다. 
/// 
/// </summary>
public class PathAI : MonoBehaviour
{
    private Node lastTarget;

    private void Awake()
    {
        //최종 목적지 노드를 받는다.
        lastTarget = NodeManager2.instance.GetLastNode();

    }


}
