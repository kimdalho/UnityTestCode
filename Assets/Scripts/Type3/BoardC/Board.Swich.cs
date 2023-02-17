using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Board : MonoBehaviour
{
    Tile temp_mainTile;
    Tile temp_mirrorTile;

    public Vector2Int select_main_tile_pos;
    public Vector2Int mirror_tile_pos;


    public InputManager inputMgr
    {
        get { return InputManager.instance; }
    }

    public void SwitchingGems()
    {
        

        if (This_switch_will_produce_an_explosion())
        {

        }
        else
        {
            if(board_array_master[select_main_tile_pos.x, select_main_tile_pos.y,PuzzleFunctions.ItemLayer] > 0)
            {

            }
            else
            {
                StartCoroutine(StartBadSwichAnim());
            }      
        }
    }

    private IEnumerator StartBadSwichAnim()
    {
        var value_main_tile_block_pos = InputManager.instance.mainTile.block.transform.localPosition;
        var value_mirrorTile_tile_block_pos = InputManager.instance.mirrorTile.block.transform.localPosition;

        InputManager.instance.mainTile.block.ResetColor();
        InputManager.instance.mirrorTile.block.ResetColor();

        InputManager.instance.mainTile.block.DoMove(value_mirrorTile_tile_block_pos);
        InputManager.instance.mirrorTile.block.DoMove(value_main_tile_block_pos);

        //서로간의 잼 이동 하는 시간을 1초로 잡았다.
        yield return new WaitForSeconds(1f);

        yield return StartCoroutine(EndBadSwichAnim());
        InputManager.instance.mainTile = null;
        InputManager.instance.mirrorTile = null;

        BoardManager.instance.isMoveCan = false;
    }

    private IEnumerator EndBadSwichAnim()
    {
        var value_main_tile_block_pos = InputManager.instance.mainTile.block.transform.localPosition;
        var value_mirrorTile_tile_block_pos = InputManager.instance.mirrorTile.block.transform.localPosition;

        InputManager.instance.mainTile.block.DoMove(value_mirrorTile_tile_block_pos);
        InputManager.instance.mirrorTile.block.DoMove(value_main_tile_block_pos);

        yield return new WaitForSeconds(0.3f);
    }



    
    public bool This_switch_will_produce_an_explosion()
    {
        return false;
    }

    //좌표에 따른 잼의 정보 이동
    //잼이 스왑하더라도 잼의 정보는 그대로이다.
    //잼의 정보를 해당 함수를 통해 업데이트한다.
    public void Move_gems_to_target_positions()
    {
        
    }
    //끝날때쯤 이동값을 재정의한다.
    //이거 먼저 만들어보자
    public void Check_move_gems()
    {
        for(int x = 0; x < MAX_X; x++)
        {
            for (int y = 0; y < MAX_Y; y++)
            {
                Check_moves_of_this_gem(x, y);
            }
        }

    }
    //이 잼의 이동들을 재정의한다.
    private void Check_moves_of_this_gem(int x, int y)
    {
        if (CheckThereisAction(x, y))
            return;

        //모두 리셋한다.
        board_array_master[x, y, PuzzleFunctions.useful] = 0 ;
        board_array_master[x, y, PuzzleFunctions.left] = 0;
        board_array_master[x, y, PuzzleFunctions.right] = 0;
        board_array_master[x, y, PuzzleFunctions.up] = 0;
        board_array_master[x, y, PuzzleFunctions.down] = 0;

        //Check_Feasible_Move_To(PuzzleFunctions.left);

        if (board_array_master[x, y, PuzzleFunctions.GemLayer] <= 3)
            return;

        //if((x + 1) <  MAX_X && board_array_master[0,0,0])

    }

    private void Check_Feasible_Move_To(int dir)
    {

    }


}
