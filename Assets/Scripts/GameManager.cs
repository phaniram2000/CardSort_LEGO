using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public List<int> ColorID;
    public List<Transform> ColorID0, ColorID1, ColorID2, ColorID3, ColorID4, ColorID5, ColorID6, ColorID7;
    public List<Transform> LegoPosotions;
    public int totalColors;
    [HideInInspector] public int LEGOCount;
    public GameObject Addon;
    public Transform Cam, Cammove;

    [Serializable]
    public struct CardProperties
    {
        public Material CardMatYellow;
        public Material CardMatGreen;
        public Material CardMatBlue;
        public Material CardMatRed;
        public Material CardMatViolet;
        public Material CardMatWhite;
        public Material CardMatBlack;
        public Material CardMatGrey;
    }

    public CardProperties CardProperties_;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        LEGOCount = 1;
        LegoLocationDesider();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            RemoveSameElements();
        }
    }

    void RemoveSameElements()
    {
        for (int i = 0; i < ColorID.Count; i++)
        {
            if (ColorID[i] == ColorID[i])
            {
                ColorID.Remove(ColorID[i]);
            }
        }
    }

    public void LegoLocationDesider()
    {
        switch (LEGOCount)
        {
            case 1:
                LegoPosotions = ColorID0;
                break;
            case 2:
                LegoPosotions = ColorID1;
                break;
            case 3:
                LegoPosotions = ColorID2;
                break;
            case 4:
                LegoPosotions = ColorID3;
                break;
            case 5:
                LegoPosotions = ColorID4;
                break;
            case 6:
                LegoPosotions = ColorID5;
                break;
            case 7:
                LegoPosotions = ColorID6;
                break;
            case 8:
                LegoPosotions = ColorID7;
                break;
            default:
                break;
        }
    }

    public void CamTrans()
    {
        Cam.DORotate(new Vector3(37.087f, 0, 0), .5f);
        Cam.DOMove(Cammove.position, 1f).SetEase(Ease.Linear);
    }
}