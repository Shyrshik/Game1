
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class EnemyCount : MonoBehaviour
{
    private TextMeshProUGUI _text;
    
    // public GameObject TargetHealth;
    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
        //_text.text = "EnemyCount: ";
        EventManager.OnEnemyCount.AddListener(ResetText);
    }
    private void ResetText(float count)
    {
        //if (TargetHealth.layer == Layer) 
        _text.text = "EnemyCount: " + count;
    }
}
