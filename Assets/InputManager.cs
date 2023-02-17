using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public Tile mainTile;
    public Tile mirrorTile;

    public static InputManager instance;
    private void Awake()
    {
        instance = this;
    }




}
