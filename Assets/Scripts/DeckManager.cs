using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class DeckManager : MonoBehaviour
{
    public Deck[] DeckScript;
    public List<GameObject> layers;
    public List<GameObject> cubesInDeck = new List<GameObject>();
    public DeckState deckState;
    public Slider uiSlider;
    public int maxCubeCount;
    public bool isActive;
    public float spacing;
    private float colorid;
    private UserInput _userInput;

    private void Start()
    {
        _userInput = FindObjectOfType<UserInput>();
        switch (deckState)
        {
            case DeckState.Deck:
                doThisOnstart();
                break;
            case DeckState.MergeDeck:
                Debug.Log("Deck");
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void Update()
    {
        switch (deckState)
        {
            case DeckState.Deck:

                break;
            case DeckState.MergeDeck:
                UpdateSliderValue();

                break;
            case DeckState.Merging:
                CheckForExcessLegos();

                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void doThisOnstart()
    {
        DOVirtual.DelayedCall(.3f, (() =>
        {
            CheckActiveObjects();
            for (int i = 0; i < cubesInDeck.Count; i++)
            {
                cubesInDeck[i].transform.parent = transform;
            }
        }));
    }

    void CheckActiveObjects()
    {
        foreach (GameObject layer in layers)
        {
            if (layer.activeSelf)
            {
                AddActiveLayerChildren(layer);
            }
        }

        ReverseActiveChildObjectsList();
    }

    void AddActiveLayerChildren(GameObject layer)
    {
        Transform layerTransform = layer.transform;
        foreach (Transform child in layerTransform)
        {
            cubesInDeck.Add(child.gameObject);
        }
    }

    void ReverseActiveChildObjectsList()
    {
        cubesInDeck.Reverse();
    }

    public List<GameObject> CheckForSameId(int targetID)
    {
        List<GameObject> selectedCubes = new List<GameObject>();
        foreach (var cube in cubesInDeck)
        {
            Layer cubeScript = cube.GetComponent<Layer>();
            if (cubeScript != null && cubeScript.ID == targetID)
            {
                selectedCubes.Add(cube);
            }
            else
            {
                break; // Stop adding cubes when a different ID is encountered
            }
        }

        return selectedCubes;
    }

    public void UpdateSliderValue()
    {
        // Calculate the percentage based on cubesInDeck count and a maximum limit
        float percentage = (cubesInDeck.Count / uiSlider.maxValue) * maxCubeCount;

        // Set the slider value using the calculated percentage
        // Assuming sliderValue is a reference to your UI Slider
        uiSlider.value = Mathf.Lerp(percentage, uiSlider.value, 20 * Time.deltaTime);

        if (uiSlider.value >= uiSlider.maxValue && !_userInput.jump.IsActive())
        {
            // Color ID handling goes here

            moveToModel(cubesInDeck, GameManager.instance.LegoPosotions);
            deckState = DeckState.Merging;
        }
    }

    void moveToModel(List<GameObject> Lego, List<Transform> LegoBuilding)
    {
        StartCoroutine(MoveCubesToLegoBuilding(Lego, LegoBuilding));
    }

    Tween jump;

    IEnumerator MoveCubesToLegoBuilding(List<GameObject> Lego, List<Transform> LegoBuilding)
    {
        int cubeIndex = 0;
        List<int> indicesToRemove = new List<int>();

        foreach (var targetTransform in LegoBuilding)
        {
            if (cubeIndex >= cubesInDeck.Count || cubeIndex >= LegoBuilding.Count)
            {
                Debug.LogWarning("Not enough cubes or target positions in LegoBuilding.");
                break;
            }

            var cube = cubesInDeck[cubeIndex];
            cube.transform.parent = null;
            var seq = DOTween.Sequence();
            seq.Join(jump = cube.transform.DOJump(targetTransform.position, 10, 1, .3f)
                .SetEase(Ease.Flash));
            cube.gameObject.layer = 2;
            gameObject.layer = 2;
            cube.transform.localScale = targetTransform.localScale;
            cube.transform.rotation = targetTransform.rotation;

            indicesToRemove.Add(cubeIndex);

            yield return new WaitForSeconds(0.05f);
            cubeIndex++;
            yield return new WaitForEndOfFrame();
        }

        // Remove marked indices in reverse order to avoid shifting elements
        for (int i = indicesToRemove.Count - 1; i >= 0; i--)
        {
            int indexToRemove = indicesToRemove[i];
            cubesInDeck.RemoveAt(indexToRemove);
            LegoBuilding.RemoveAt(indexToRemove);
        }
    }

    private void CheckForExcessLegos()
    {
        if (!jump.IsActive())
        {
            deckState = DeckState.MergeDeck;
            gameObject.layer = 6;
            // GameManager.instance.LEGOCount++;
            GameManager.instance.LegoLocationDesider();
            if (GameManager.instance.LegoPosotions.Count <= 0)
            {
                GameManager.instance.Addon.SetActive(true);
                DOVirtual.DelayedCall(.2f, () => GameManager.instance.CamTrans());
            }
            // if (cubesInDeck.Count > 0)
            // {
            //     for (int i = 0; i < cubesInDeck.Count; i++)
            //     {
            //         cubesInDeck[i].SetActive(false);
            //         cubesInDeck.Remove(cubesInDeck[i]);
            //     }
            // }
            // else
            // {
            //     deckState = DeckState.MergeDeck;
            //     gameObject.layer = 6;
            //     GameManager.instance.LEGOCount++;
            //     GameManager.instance.LegoLocationDesider();
            // }
        }
    }
}