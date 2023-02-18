using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Button2 : MonoBehaviour
{
    private TextMeshProUGUI _text;
    private Button _button;
    private string _name;
    // public GameObject TargetHealth;
    private void Awake()
    {
        _text = GetComponentInChildren<TextMeshProUGUI>();
        _name = _text.text;
        _button = GetComponent<Button>();
        _button.image.color = Color.green;
        //_text.text = "EnemyCount: ";
        EventManager.OnSecondWeapon.AddListener(ResetText);
        ResetText(true);
    }
    private void ResetText(bool on)
    {
        if (on)
        {
            _text.text = _name + "on";
            _button.image.color = Color.green;
        }
        else
        {
            _text.text = _name + "off";
            _button.image.color = Color.red;
        }
    }
}
