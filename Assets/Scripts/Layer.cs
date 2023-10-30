using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Layer : MonoBehaviour
{
    public int ID;
    public DeckManager DeckManager;

    private void Start()
    {
        // DOVirtual.DelayedCall(.5f, (() =>
        // {
        //     if (!GameManager.instance.ColorID.Contains(ID))
        //     {
        //         GameManager.instance.ColorID.Add(ID);
        //     }
        // }));
    }

    private void Update()
    {
    }
}