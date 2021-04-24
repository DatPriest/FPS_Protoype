using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public bool isInteractable;
    enum Types
    {
        Item = 0,
        Enemy = 1,

    }
    protected virtual void Start()
    {

    }
}
