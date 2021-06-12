using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Navigator : MonoBehaviour
{
    int currentPage;
    int totalPages;
    int totalNumbers;

    public Image prevImage;
    public Image nextImage;

    public Button prevBtn;
    public Button nextBtn;

    public Text pageIndicator;

    private void OnEnable()
    {
        EventBroker.OnGetTotalPages += HandlerOnGetTotalPages;
        EventBroker.OnGetTotalNumbers += HandlerOnGetTotalNumbers;
    }

    private void OnDisable()
    {
        EventBroker.OnGetTotalPages -= HandlerOnGetTotalPages;
        EventBroker.OnGetTotalNumbers -= HandlerOnGetTotalNumbers;
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

    public void GoToNumber(string nr)
    {
        int currentNr;
        bool conv = int.TryParse(nr, out currentNr);
        if (!conv || currentNr < 1 || currentNr > totalNumbers) return;

        currentPage = Mathf.CeilToInt(currentNr * totalPages * 1f / totalNumbers);
        currentPage = currentPage > 0 ? currentPage - 1 : currentPage; // cause, array definitions starts to 0...
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
            nextImage.color = new Color(nextImage.color.r, nextImage.color.g, nextImage.color.b, 1f);
        }
        SetPageIndicator(currentPage + 1, totalPages);
    }

    private void SetPageIndicator(int left, int right)
    {
        pageIndicator.text = $"Page {left} of {right}";
    }

    private void HandlerOnGetTotalPages(int tp)
    {
        totalPages = tp;
    }

    private void HandlerOnGetTotalNumbers(int tn)
    {
        totalNumbers = tn;
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
