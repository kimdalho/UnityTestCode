using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public enum eState
{
    None = 0,
    Idle = 1,
    Action = 2,
    post_action = 3,
}

public static class PuzzleFunctions
{
    public static int MAX_LAYER = 10;
    public static int EmptyLayer = 0;
    public static int RegenLayer = 1;
    public static int GemLayer = 2;
    public static int ItemLayer = 4;


    public static int useful = 5; //유효한? 의미를 아직 모르겠다.
    public static int left = 6;
    public static int right = 7;
    public static int up = 8;
    public static int down = 9;


    public static int ActionLayer = 11;

    public static int Action_Explosion = 111; //폭발
    public static int Action_Falling = 222; //낙하 또는 생성
    public static int Action_Ongoing = 333; //전진 애니메이션

    public static int Empty_Block = -99;
    public static int GetBoardValue(int x, int y, int layer)
    {
        return BoardManager.instance.board.board_array_master[x, y, layer];
    }

    public static bool IsBlock(int x, int y)
    {
        return true;
    }
}


public class BoardManager : MonoBehaviour
{
    public static BoardManager instance;

    public GameObject prefab_tile;
    public GameObject prefab_block;

    public Transform tile_parent;
    public Transform block_parent;

    public Board board;

    public bool isMoveCan;


    private void Awake()
    {
        Application.targetFrameRate = 60;
        instance = this;
        isMoveCan = true;
        board.LoadStage(prefab_tile.gameObject, tile_parent);
        board.InitateGame();
    }

    public bool BlockCompareValue(int value,int x, int y)
    {
        return board.board_array_master[x, y, PuzzleFunctions.GemLayer] == value;
    }
}

