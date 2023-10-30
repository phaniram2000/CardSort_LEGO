using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using System;
using DG.Tweening;

[Serializable]
public enum DeckState
{
    Deck,
    MergeDeck,
    Merging
}
public enum ColorType
{
    Yellow,
    Green,
    Blue,
    Red,
    Violet,
    White,
    Black,
    Grey
}


//
// [Serializable]
// public class LayerInfo
// {
//     public List<GameObject> Layer;
//     public Material Color;
// }
//
// [Serializable]
// public class DeckInfo
// {
//     [EnumToggleButtons] public DeckState DeckState;
//
//     [ShowIf("DeckState", DeckState.Fill)] public List<LayerInfo> Layer1;
//     [ShowIf("DeckState", DeckState.Fill)] public List<LayerInfo> Layer2;
//     [ShowIf("DeckState", DeckState.Fill)] public List<LayerInfo> Layer3;
// }

public class RowManager : MonoBehaviour
{
    public static RowManager instance;
    public int Id;

  

    [Serializable]
    public class Deck1
    {
        public GameObject Deck;
        public bool isactive;
        public DeckManager Deck1_G;
        public Transform DeckPos;
        public int noOfLayers;
        public ColorType layerColor0;
        public ColorType layerColor1;
        public ColorType layerColor2;
        public DeckState deckState;

        public void GenerateCard() => CardGeneration(Deck, Deck1_G,
            layerColor0, layerColor1, layerColor2, noOfLayers, isactive, DeckPos, deckState);
    }

    [Serializable]
    public class Deck2
    {
        public GameObject Deck;
        public bool isactive;
        public DeckManager Deck2_G;
        public Transform DeckPos;
        public int noOfLayers;
        public ColorType layerColor0;
        public ColorType layerColor1;
        public ColorType layerColor2;
        public DeckState deckState;

        public void GenerateCard() => CardGeneration(Deck, Deck2_G,
            layerColor0, layerColor1, layerColor2, noOfLayers, isactive, DeckPos, deckState);
    }

    public class Deck3
    {
        public GameObject Deck;
        public bool isactive;
        public DeckManager Deck3_G;
        public Transform DeckPos;
        public int noOfLayers;
        public ColorType layerColor0;
        public ColorType layerColor1;
        public ColorType layerColor2;
        public DeckState deckState;

        public void GenerateCard() => CardGeneration(Deck, Deck3_G,
            layerColor0, layerColor1, layerColor2, noOfLayers, isactive, DeckPos, deckState);
    }

    public class Deck4
    {
        public GameObject Deck;
        public bool isactive;
        public DeckManager Deck4_G;
        public Transform DeckPos;
        public int noOfLayers;
        public ColorType layerColor0;
        public ColorType layerColor1;
        public ColorType layerColor2;
        public DeckState deckState;

        public void GenerateCard() => CardGeneration(Deck, Deck4_G,
            layerColor0, layerColor1, layerColor2, noOfLayers, isactive, DeckPos, deckState);
    }

    public class Deck5
    {
        public GameObject Deck;
        public bool isactive;
        public DeckManager Deck5_G;
        public Transform DeckPos;
        public int noOfLayers;
        public ColorType layerColor0;
        public ColorType layerColor1;
        public ColorType layerColor2;
        public DeckState deckState;

        public void GenerateCard() => CardGeneration(Deck, Deck5_G,
            layerColor0, layerColor1, layerColor2, noOfLayers, isactive, DeckPos, deckState);
    }

    public Deck1 Deck_1;
    public Deck2 Deck_2;
    public Deck2 Deck_3;
    public Deck2 Deck_4;
    public Deck2 Deck_5;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Deck_1.GenerateCard();
        Deck_2.GenerateCard();
        Deck_3.GenerateCard();
        Deck_4.GenerateCard();
        Deck_5.GenerateCard();
    }

    // public static void CardGeneration(DeckManager _deck,
    //     ColorType layerColor0, ColorType layerColor1, ColorType layerColor2, int noOfLayers)
    // {
    //     _deck.DeckScript[0].ColorType = layerColor0;
    //     _deck.DeckScript[1].ColorType = layerColor1;
    //     _deck.DeckScript[2].ColorType = layerColor2;
    //     _deck.layercount = noOfLayers;
    // }
    public static void CardGeneration(GameObject Deck, DeckManager _deck,
        ColorType layerColor0, ColorType layerColor1, ColorType layerColor2, int noOfLayers, bool isactive,
        Transform Deckposition, DeckState deckState)
    {
        if (isactive)
        {
            Deck.SetActive(true);
            Deck.transform.position = Deckposition.position;
            _deck.DeckScript[0].ColorType = layerColor0;
            _deck.DeckScript[1].ColorType = layerColor1;
            _deck.DeckScript[2].ColorType = layerColor2;
            DeckLayersDesider(noOfLayers, _deck.layers);
            _deck.deckState = deckState;
            _deck.isActive = isactive;
        }
        else
        {
            Deck.SetActive(false);
            _deck.isActive = isactive;
        }
    }


    public static void DeckLayersDesider(int LayerCount, List<GameObject> Layers)
    {
        switch (LayerCount)
        {
            case 1:
                Layers[0].SetActive(true);
                break;
            case 2:
                Layers[0].SetActive(true);
                Layers[1].SetActive(true);
                break;
            case 3:
                Layers[0].SetActive(true);
                Layers[1].SetActive(true);
                Layers[2].SetActive(true);
                break;
            default:
                break;
        }
    }
}