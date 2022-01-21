using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [SerializeField]
    private GameObject fadeScreen;

    protected void SetNavigationUp(GameObject previous, GameObject current)
    {
        Navigation NewNav = current.GetComponent<Button>().navigation;
        NewNav.selectOnUp = previous.GetComponent<Button>();
        current.GetComponent<Button>().navigation = NewNav;
    }

    protected void SetNavigationDown(GameObject previous, GameObject current)
    {
        Navigation NewNav = previous.GetComponent<Button>().navigation;
        NewNav.selectOnDown = current.GetComponent<Button>();
        previous.GetComponent<Button>().navigation = NewNav;
    }

    protected void ClearItems(Transform itemTemplate, Transform itemContainer)
    {
        foreach (Transform child in itemContainer)
        {
            if (child == itemTemplate) continue;
            Destroy(child.gameObject);
        }
    }

    protected void Fade()
    {
        fadeScreen.SetActive(true);
    }

    protected void UnFade()
    {
        fadeScreen.SetActive(false);
    }
}
