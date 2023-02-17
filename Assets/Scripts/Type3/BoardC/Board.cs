using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public enum eMapContentsType
{
    None = 0,
    AdventureMod = 1,
    PuzzleQuest = 2,
    Challenge = 3,
    Tutorial = 4,
}

public partial class Board : MonoBehaviour
{
    public int[,,] board_array_master;
    public Tile[,] board_array;
    public Tile[] leader_arry;



    public int MAX_X;
    public int MAX_Y;


    public int fallingTiles;
    //생성갯수
    public int number_of_new_gems_to_create;

    public eState state = eState.Idle;
    public eMapContentsType currentGame = eMapContentsType.PuzzleQuest;

    private void Update()
    {
        switch (state)
        {
            case eState.Idle:
                if(fallingTiles == 0)
                {

                }
                else
                {
                    Failling();
                }
                break;
            case eState.Action:
                if(fallingTiles == 0)
                {
                    CheckReaderTiles();
                    GenerateMoveCheck();
                }
                    
                break;
            case eState.post_action:
                break;
        }
    }

    private void GenerateMoveCheck()
    {
        Check_new_gem_to_generate();

        //number_of_new_gems_to_create는 위 함수에서 발생한다.
        //생성 할 보석의 숫자
        if (number_of_new_gems_to_create == 0)
        {
            Generate_new_gems();
        }
    }

    private void Generate_new_gems()
    {
        for(int i =0; i < leader_arry.Length; i++)
        {
            int curX = leader_arry[i].x;
            int curY = leader_arry[i].y;
            if( board_array_master[curX,curY, PuzzleFunctions.ActionLayer] == 2)
            {
                board_array[curX, curY].CreationStart();
            }
        }
    }



    private void Check_new_gem_to_generate()
    {
        Debug.Log("새로운 잼을 입구에서 생성한다.");
        for(int i =0; i  < leader_arry.Length; i++)
        {
            int curX = leader_arry[i].x;
            int curY = leader_arry[i].y;
            

            if (board_array_master[curX, curY, PuzzleFunctions.GemLayer] == PuzzleFunctions.Empty_Block) //입구측 잼블럭은 -99이다.
            {
                if (CheckThereisAction(curX, curY))
                    continue;
                    

                number_of_new_gems_to_create++;

                board_array_master[curX, curY, PuzzleFunctions.ActionLayer] = 2; // 잼 생성
                //board_array_master[curX, curY, PuzzleFunctions.체키드] = 1; 아직까지는 필요이유를 모르겠음

                //빈 타일은 이 생성된 새 보석에 예약되어 있습니다(이것은 폭발이 보드 상단의 세로일 때만 필요하며, 대각선 세로줄 에서 떨어지는 대각선을 방지하기 위해 필요합니다)
                //empty tile are reserved to this generated new gem (this is needed only when the explosion is a column on the top of the board, to prevet diagona falling from adiacent columns)

                //생성할 대상의 세로줄의 타일에 잼이 존재하지않다면
                //해당 타일의 메모레이어는 1로 정의
            }
        }
    }

    private bool CheckThereisAction(int x, int y)
    {
        if(board_array_master[x, y, PuzzleFunctions.ActionLayer] == PuzzleFunctions.Action_Explosion    ||
           board_array_master[x, y, PuzzleFunctions.ActionLayer] == PuzzleFunctions.Action_Falling      ||
           board_array_master[x, y, PuzzleFunctions.ActionLayer] == PuzzleFunctions.Action_Ongoing)
        {
            return true;
        } 


        return false;
    }

    private void CheckReaderTiles()
    {

    }


    public void LoadStage(GameObject prefab, Transform parent)
    {
        board_array_master = new int[MAX_X, MAX_Y, PuzzleFunctions.MAX_LAYER];
        board_array = new Tile[MAX_X, MAX_Y];

        for (int y = MAX_Y - 1; y >= 0; y--)
        {
            for (int x = 0; x < MAX_X; x++)
            {
                GameObject go = Instantiate(prefab) as GameObject;
                go.transform.position = new Vector3(x * 105f - 200f, y * 105f, 0);
                go.transform.SetParent(parent, false);

                var tile = go.GetComponent<Tile>();
                try
                {
                    board_array[x, y] = tile;
                    board_array_master[x, y, PuzzleFunctions.EmptyLayer] = 1; //공허
                }
                catch (Exception e)
                {
                    Debug.LogError($"{x} {y}");
                }
                //leader_arry
                if (y == MAX_Y - 1)
                {
                    board_array_master[x, y, PuzzleFunctions.RegenLayer] = 1;
                }
                
                tile.Setup(x, y);
                GameObject objblock = Instantiate(BoardManager.instance.prefab_block);
                tile.block = objblock.GetComponent<Block>();
                tile.block.transform.SetParent(BoardManager.instance.block_parent);
                tile.block.transform.localPosition = tile.gameObject.transform.localPosition;
                int rnd = UnityEngine.Random.Range(0, 3);
                board_array_master[x, y, PuzzleFunctions.GemLayer] = rnd;
                tile.block.SetBlock(rnd);
            }
        }
        
        //초기에 3개 매칭이 나오지 않도록 처리
        while (Has3Match())
        {
            for (int y = MAX_Y - 1; y >= 0; y--)
            {
                for (int x = 0; x < MAX_X; x++)
                {
                    var tile =  board_array[x, y];

                    tile.Setup(x, y);
                    GameObject objblock = Instantiate(BoardManager.instance.prefab_block);
                    tile.block.Self_destruct();
                    tile.block = objblock.GetComponent<Block>();
                    tile.block.transform.SetParent(BoardManager.instance.block_parent);
                    tile.block.transform.localPosition = tile.gameObject.transform.localPosition;
                    int rnd = UnityEngine.Random.Range(0, 3);
                    board_array_master[x, y, PuzzleFunctions.GemLayer] = rnd;
                    tile.block.SetBlock(rnd);
                }
            }
        }

        for (int y = MAX_Y - 1; y >= 0; y--)
        {
            for (int x = 0; x < MAX_X; x++)
            {
                var tile = board_array[x, y];
                tile.button.onClick.AddListener(tile.OnClickButton);
            }
        }
    }

    private bool Has3Match()
    {
        int[,] checks = new int[11, 11];
        for (int y = 0; y < 11; y++)
        {
            for (int x = 0; x < 11; x++)
            {
                checks[x, y] = -1;
            }
        }

        for (int y = MAX_Y - 1; y >= 0; y--)
        {
            for (int x = MAX_X - 1; x >= 0; x--)
            {
                checks[x, y] = 0;

                if (PuzzleFunctions.IsBlock(x, y))
                {
                    //int ex_count = 0;
                    int[,] tmpMap = new int[11, 11];
                    for (int yy = 11 - 1; yy >= 0; yy--)
                    {
                        for (int xx = 11 - 1; xx >= 0; xx--)
                        {
                            tmpMap[xx, yy] = -1;
                        }
                    }

                    int compareValue = PuzzleFunctions.GetBoardValue(x, y, PuzzleFunctions.GemLayer);
                    //horizontal
                    if ((x + 1 <= MAX_X) && (x - 1 >= 0))
                    {
                        int x_count = 0;
                        bool bSkip = false;
                        int[] tmpArray = new int[11];
                        for (int tmpi = 0; tmpi < 11; tmpi++)
                            tmpArray[tmpi] = -1;

                        for (int i_x = x - 1; i_x >= 0; i_x--)
                        {
                            if (BoardManager.instance.BlockCompareValue(compareValue, i_x, y))
                            {
                                if (tmpArray[0] == -1)
                                {
                                    tmpArray[0] = x;
                                    x_count++;
                                }

                                tmpArray[x_count] = i_x;
                                x_count++;
                            }
                            else
                            {
                                break;
                            }
                        }

                        if (x_count >= 3)
                        {
                            Debug.Log("찾았어");
                            return true;
                        }
                        else
                        {
                            if (bSkip)
                                continue;
                        }
                    }

                    if ((y + 1 <= MAX_Y) && (y - 1 >= 0))
                    {
                        int y_count = 0;
                        bool bSkip = false;
                        int[] tmpArray = new int[11];
                        for (int tmpi = 0; tmpi < 11; tmpi++)
                            tmpArray[tmpi] = -1;    

                        for (int i_y = y - 1; i_y >= 0; i_y--)
                        {
                            if (BoardManager.instance.BlockCompareValue(compareValue, x, i_y))
                            {
                                if (tmpArray[0] == -1)
                                {
                                    tmpArray[0] = y;
                                    y_count++;
                                }

                                tmpArray[y_count] = i_y;
                                y_count++;
                            }
                            else
                            {
                                break;
                            }
                        }

                        if (y_count >= 3)
                        {
                            Debug.Log("찾았어");
                            return true;
                        }
                        else
                        {
                            if (bSkip)
                                continue;
                        }
                    }

                }

            }
        }

        return false;
    }

    private void Failling()
    {

    }
    private bool Nameless()
    {
        return false;
    }

    public void InitateGame()
    {
        Check_ALL_possible_moves_And_FinalizeTurn();
    }

    //모든 움직임을 감지및 턴마무리를 처리
    public void Check_ALL_possible_moves_And_FinalizeTurn()
    {
       StartCoroutine(TurnPostProcessAndUpdateTurn());
    }
    
    private IEnumerator TurnPostProcessAndUpdateTurn()
    {
        switch(currentGame)
        {
            case eMapContentsType.PuzzleQuest:
                yield return StartCoroutine(CheckGimmickProcessAndAfter());
                break;
        }

        yield return null;
    }

    private IEnumerator CheckGimmickProcessAndAfter()
    {
        Debug.Log("모든 기믹이 처리되었습니다");
        yield return null;
    }

}
