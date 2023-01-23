using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 노드 길 기능
/// 1. 최단의 단 하나의 지점을 최종 목표지점으로 삼는다.
/// 2. 밑으로 내려가기에 아래, 아래 왼쪽, 아래 오른쪽을 차례로 탐색한다.
/// 3. 탐색순으로 IsWall이 거짓이라면 해당 Tile을 이동할 위치로 삼는다.
/// 3-1. 하단 타일이 모두 길이 없다면, 왼쪽, 오른쪽을 탐색한고 IsWall 거짓이라면 해당 Tile을 이동할 위치로 삼는다.
/// 4. 이동한 위치가 처음 정한 목적지가 맞는지 비교한다.
/// 4-1. 4의 결과가 거짓이라면 2번을 다시 진행한다.
/// </summary>
public class Dscription : MonoBehaviour
{
    public string summary;
}
