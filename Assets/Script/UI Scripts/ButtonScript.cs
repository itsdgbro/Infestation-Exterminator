using UnityEngine;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour
{
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClick);
        button.interactable = true;
    }

    public void OnButtonClick()
    {
        button.interactable = false;
    }

}
