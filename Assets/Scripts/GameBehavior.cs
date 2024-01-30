using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBehavior : MonoBehaviour
{
    private PlayerBehavior player;

    private void Start()
    {
        player = GetComponent<PlayerBehavior>();
    }
}
