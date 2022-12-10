using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public GameObject player;

    private void Start() 
    {
        // player = GameObject.FindGameObjectWithTag("Player");
    }
    private void LateUpdate()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10);
    }
}
