using TMPro;
using UnityEngine;

public class HealthMenu : MonoBehaviour
{
    private TextMeshProUGUI _text;
    // public GameObject TargetHealth;
    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
        //_text.text = "Health: ";
        EventManager.OnHealthToInterface.AddListener(ResetText);
    }
    private void ResetText(int health)
    {
        //if (TargetHealth.layer == Layer) 
        _text.text = "Health: " + health;
    }
}
