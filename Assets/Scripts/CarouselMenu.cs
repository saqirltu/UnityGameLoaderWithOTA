using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class CarouselMenu : MonoBehaviour
{
    float selectionItemWidth;
    float selectionItemGap;
    int selectionItemCount;
    float unitOffsetPixel;
    float midIndex;
    string selectedSceneTag;


    ////////////////////////////////////////////////////////
    /// I'm using a ScrollView to achieve a self looping Carousel Menu
    ///
    /// The menu items are currently manually arranged, planning to automate it later.
    ///
    /// For current example, there are actually 3 options.
    ///
    /// They are listed as below:
    ///
    /// 3 , 1 2 3 , 1 2 3 , 1 2 3 , 1
    ///
    /// We can find the items are repeated for 3 times. This is for giving selections while scrolling left and right.
    ///
    /// So I am reseting the index to the middel section after each animation. 
    ////////////////////////////////////////////////////////

    public void SelectItem()
    {
        GameObject selectedItem = EventSystem.current.currentSelectedGameObject;
        selectedSceneTag = selectedItem.transform.GetChild(0).GetComponent<TMP_Text>().text;
        int selectedItemIndex = selectedItem.transform.GetSiblingIndex();
        StartCoroutine(SetSelectedItemWithAnim(selectedItemIndex));
    }

    public void LoadSelectedScene()
    {
        MainLoader.Instance.LoadScene(selectedSceneTag);
    }

    // Start is called before the first frame update
    void Start()
    {
        HorizontalLayoutGroup layoutGroup = GetComponent<HorizontalLayoutGroup>();
        
        selectionItemGap = layoutGroup.spacing;
        selectionItemWidth = transform.GetChild(0).GetComponent<RectTransform>().sizeDelta.x;
        Debug.Log("selectionItemWidth: " + selectionItemWidth);
        selectionItemCount = transform.childCount;
        unitOffsetPixel = selectionItemWidth + selectionItemGap;

        midIndex = (selectionItemCount - 1) / 2f;
        SetSelectedItem((int)midIndex);
    }

    void SetSelectedItem(int index)
    {
        gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(unitOffsetPixel * (midIndex - index), 0f);
        transform.GetChild(lastSelectionIndex).localScale = new Vector2(1f, 1f);
        transform.GetChild(index).localScale = new Vector2(1.2f, 1.2f);
        lastSelectionIndex = index;
        selectedSceneTag = transform.GetChild(index).GetChild(0).GetComponent<TMP_Text>().text;
    }

    IEnumerator SetSelectedItemWithAnim(int index)
    {
        yield return StartCoroutine(AnimateSelection(index));
        lastSelectionIndex = index;

        // Reset the index to the middel section.
        int actualOptionCount = (selectionItemCount - 2) / 3;
        if ((index - 1) < actualOptionCount)
        {
            SetSelectedItem(index + actualOptionCount);
        }
        else if ((index - 1) > actualOptionCount * 2 - 1)
        {
            SetSelectedItem(index - actualOptionCount);
        }
    }

    int lastSelectionIndex;
    IEnumerator AnimateSelection(int index)
    {
        float journey = 0f;
        float duration = 0.3f;

        while (journey <= duration)
        {
            journey += Time.deltaTime;
            float percent = Mathf.Clamp01(journey / duration);
            gameObject.GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(gameObject.GetComponent<RectTransform>().anchoredPosition, new Vector2(unitOffsetPixel * (midIndex - index), 0f), percent);
            transform.GetChild(lastSelectionIndex).localScale = Vector2.Lerp(transform.GetChild(lastSelectionIndex).localScale, new Vector2(1f, 1f), percent);
            transform.GetChild(index).localScale = Vector2.Lerp(transform.GetChild(index).localScale, new Vector2(1.2f, 1.2f), percent);
            yield return null;
        }
    }
}
