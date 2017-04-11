using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMove : MonoBehaviour
{
    Animator CharacterToMove;
     
    void Start()
    {
        CharacterToMove = GetComponent<Animator>();
    }

    public void StartCharacterAnimator()
    {
        CharacterToMove.SetBool("IsStarted", true);
    }

}
