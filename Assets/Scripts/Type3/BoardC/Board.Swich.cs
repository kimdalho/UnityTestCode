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

        //���ΰ��� �� �̵� �ϴ� �ð��� 1�ʷ� ��Ҵ�.
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

    //��ǥ�� ���� ���� ���� �̵�
    //���� �����ϴ��� ���� ������ �״���̴�.
    //���� ������ �ش� �Լ��� ���� ������Ʈ�Ѵ�.
    public void Move_gems_to_target_positions()
    {
        
    }
    //�������� �̵����� �������Ѵ�.
    //�̰� ���� ������
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
    //�� ���� �̵����� �������Ѵ�.
    private void Check_moves_of_this_gem(int x, int y)
    {
        if (CheckThereisAction(x, y))
            return;

        //��� �����Ѵ�.
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
