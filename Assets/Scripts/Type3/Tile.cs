using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Tile : MonoBehaviour
{
    public bool bSwitching;
    public Image icon;
    public Block block;
    public Button button;
    public int move_direction;
    public InputManager inputMgr
    {
        get { return InputManager.instance; }
    }
    private Board board
    {
        get
        {
            return BoardManager.instance.board;
        }
    }

    public int x, y;
    private void Awake()
    {
        icon = GetComponent<Image>();
        button = GetComponent<Button>();

    }

    public void Setup(int _x, int _y)
    {
        this.x = _x;
        this.y = _y;
    }

    private void CreationEnd()
    {
        board.number_of_new_gems_to_create--;
    }

    //Move
    public void SwitchGemAnimation()
    {
        if (bSwitching)
        {

        }
        else
        {
            StartCoroutine(CoSwitchingGemAnimation());
        }
    }

    private IEnumerator CoSwitchingGemAnimation()
    {
       while(block.transform.localPosition != this.gameObject.transform.localPosition)
        {
            block.transform.localPosition = Vector3.Lerp(block.transform.localPosition, this.gameObject.transform.localPosition, Time.deltaTime);
            yield return null;
        }
        
        yield return null;
    }


    public void OnClickButton()
    {
        Debug.Log("xx");
        if (inputMgr.mainTile != null && inputMgr.mirrorTile != null)
        {
            Debug.Log("두 값이 존재함 인풋을 받지않음.");
            return;
        }

        if (InputManager.instance.mainTile == null)
        {
           IbecomeMainTile();
        }
        else
        {
            var main = InputManager.instance.mainTile;
            try
            {
                if (board.board_array[main.x - 1, main.y] == this)
                {
                    IbecomeMinorTile(PuzzleFunctions.left);
                }
                else if (board.board_array[main.x + 1, main.y] == this)
                {
                    IbecomeMinorTile(PuzzleFunctions.right);
                }
                else if (board.board_array[main.x, main.y + 1] == this)
                {
                    IbecomeMinorTile(PuzzleFunctions.up);
                }
                else if (board.board_array[main.x, main.y - 1] == this)
                {
                    IbecomeMinorTile(PuzzleFunctions.down);
                }
                else
                {
                    inputMgr.mainTile = null;
                    inputMgr.mirrorTile = null;
                }
            }
            catch(Exception )
            {

            }


            if (inputMgr.mainTile != null && inputMgr.mirrorTile != null)
            {
                BoardManager.instance.isMoveCan = false;
                BoardManager.instance.board.SwitchingGems();
            }
        }
    }

    public void IbecomeMainTile()
    {
       InputManager.instance.mainTile = this;
       InputManager.instance.mainTile.block.SetSelectColor();
       InputManager.instance.mirrorTile = null;
    }

    public void IbecomeMinorTile(int direction)
    {
        move_direction = direction;
        InputManager.instance.mirrorTile = this;
        InputManager.instance.mirrorTile.block.SetSelectColor();

    }



    public void CreationStart()
    {
        ///222의 의미는 생성
        board.board_array_master[x, y, PuzzleFunctions.ActionLayer] = PuzzleFunctions.Action_Falling;
        //잼에 입힐 랜덤한 색
        //Generate_a_new_gem_in_this_tile
    }
}
