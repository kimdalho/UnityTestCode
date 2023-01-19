using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Graph :MonoBehaviour
{
    int[,] adj = new int[6, 6]
     {
            { 0, 1, 0, 1, 0, 0 },
            { 1, 0, 1, 1, 0, 0 },
            { 0, 1, 0, 0, 0, 0 },
            { 1, 1, 0, 0, 1, 0 },
            { 0, 0, 0, 1, 0, 1 },
            { 0, 0, 0, 0, 1, 0 },
     };

    private void Start()
    {
        BFS(0);
    }

    public void BFS(int start)
    {
        bool[] found = new bool[6];  // false로 초기화 되어 있음

        Queue<int> q = new Queue<int>();
        q.Enqueue(start);
        found[start] = true;

        while (q.Count > 0)
        {
            // 1. 방문
            int now = q.Dequeue();
            Console.WriteLine(now);

            // 2. 나와 가까운 정점들 중 방문하지 않은 애가 있다면 큐에 추가하여 예약
            for (int next = 0; next < 6; next++)
            {
                if (adj[now, next] == 0)  // 연결 되어 있지 않으면 스킵
                    continue;
                if (found[next])  // 이미 방문한 애라면 스킵
                    continue;

                // 예약
                q.Enqueue(next);
                found[next] = true;
            }
        }
    }
}

