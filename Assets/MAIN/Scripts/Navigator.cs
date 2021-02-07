using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Navigator : MonoBehaviour
{
    int currentPage;
    int totalPages;

    public Image prevImage;
    public Image nextImage;

    public Button prevBtn;
    public Button nextBtn;

    private void OnEnable()
    {
        EventBroker.OnGetTotalPages += HandlerOnGetTotalPages;
    }

    private void OnDisable()
    {
        EventBroker.OnGetTotalPages -= HandlerOnGetTotalPages;
    }

    private void Start()
    {
        UpdateNavigator();
    }

    public void GoToPage(Button from)
    {
        if (from.name.Equals("Prev"))
        {
            if (currentPage == 0)
                return;
            else
                currentPage--;
        }
        else if (from.name.Equals("Next"))
        {
            if (currentPage == totalPages - 1)
                return;
            else
                currentPage++;
        }

        EventBroker.TriggerOnGoToPage(currentPage);
        UpdateNavigator();
    }

    void UpdateNavigator()
    {
        if (currentPage == 0)
        {
            prevImage.color = new Color(prevImage.color.r, prevImage.color.g, prevImage.color.b, .2f);
            nextImage.color = new Color(nextImage.color.r, nextImage.color.g, nextImage.color.b, 1f);
        }
        else if (currentPage == totalPages - 1)
        {
            prevImage.color = new Color(prevImage.color.r, prevImage.color.g, prevImage.color.b, 1f);
            nextImage.color = new Color(nextImage.color.r, nextImage.color.g, nextImage.color.b, .2f);
        }
        else
        {
            prevImage.color = new Color(prevImage.color.r, prevImage.color.g, prevImage.color.b, 1f);
            nextImage.color = new Color(nextImage.color.r, nextImage.color.g, nextImage.color.b, .1f);
        }
    }

    private void HandlerOnGetTotalPages(int tp)
    {
        totalPages = tp;
    }

    public void SetCursor(Button from)
    {
        if (from.name.Equals("Prev"))
        {
            if (currentPage == 0)
                EventBroker.TriggerOnSetCursor(false);
            else
                EventBroker.TriggerOnSetCursor(true);
        }
        else if (from.name.Equals("Next"))
        {
            if (currentPage == totalPages - 1)
                EventBroker.TriggerOnSetCursor(false);
            else
                EventBroker.TriggerOnSetCursor(true);
        }
    }

}
