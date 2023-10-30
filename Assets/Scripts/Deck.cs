using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    public List<Transform> Layer;
    public ColorType ColorType;

    void Start()
    {
        Invoke(nameof(LayerSetup), .2f);
    }


    public void LayerSetup()
    {
        switch (ColorType)
        {
            case ColorType.Yellow:
                Fillcolor(Layer, GameManager.instance.CardProperties_.CardMatYellow, 0);
                break;
            case ColorType.Green:
                Fillcolor(Layer, GameManager.instance.CardProperties_.CardMatGreen, 1);
                break;
            case ColorType.Blue:
                Fillcolor(Layer, GameManager.instance.CardProperties_.CardMatBlue, 2);
                break;
            case ColorType.Red:
                Fillcolor(Layer, GameManager.instance.CardProperties_.CardMatRed, 3);
                break;
            case ColorType.Violet:
                Fillcolor(Layer, GameManager.instance.CardProperties_.CardMatViolet, 4);
                break;
            case ColorType.White:
                Fillcolor(Layer, GameManager.instance.CardProperties_.CardMatWhite, 5);
                break;
            case ColorType.Black:
                Fillcolor(Layer, GameManager.instance.CardProperties_.CardMatBlack, 6);
                break;
            case ColorType.Grey:
                Fillcolor(Layer, GameManager.instance.CardProperties_.CardMatGrey, 7);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void Fillcolor(List<Transform> Layers, Material Mat, int Id)
    {
        for (int i = 0; i < Layers.Count; i++)
        {
            Layers[i].GetComponent<MeshRenderer>().material = Mat;
            Layers[i].GetComponent<Layer>().ID = Id;
        }
    }
}