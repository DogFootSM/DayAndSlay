using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : MonoBehaviour
{
    private float moveSpeed = 3f;
    public float MoveSpeed {get => moveSpeed;}

    private int atk = 5;
    public int Atk {get => atk;}

    private float atkSpeed = 0.5f;
    public float AtkSpeed {get => atkSpeed;}
}
