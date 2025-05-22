using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TestPlayer : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    private void Update()
    {
        PlayerMove();
    }


    void PlayerMove()
    {
        if(Input.GetAxis("Horizontal") > 0)
        {
            gameObject.transform.position += new Vector3(moveSpeed,0,0) * Time.deltaTime;
        }

        if (Input.GetAxis("Horizontal") < 0)
        {
            gameObject.transform.position += new Vector3(-moveSpeed, 0, 0) * Time.deltaTime;
        }

        if (Input.GetAxis("Vertical") > 0)
        {
            gameObject.transform.position += new Vector3(0, moveSpeed, 0) * Time.deltaTime;
        }

        if (Input.GetAxis("Vertical") < 0)
        {
            gameObject.transform.position += new Vector3(0, -moveSpeed, 0) * Time.deltaTime;
        }
    }

}
