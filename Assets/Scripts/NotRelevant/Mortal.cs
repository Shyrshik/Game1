using TMPro;
using UnityEngine;

public class Mortal : MonoBehaviour
{

    public float SpeedBafs { get; set; } = 1f;

    [SerializeField] private protected float MaxHealth = 1f;
    [SerializeField] private protected float BaseSpeed = 1;
    [SerializeField] private protected float RunMultiplier = 2;
    //private Weapon _BaseKnuckle = new Weapon(GetComponent<LayerMask>() == LayerMask.NameToLayer("Player") ? LayerMask.NameToLayer("Enemy"): LayerMask.NameToLayer("Player"), 1f, 0.5f, 2f);
     private protected Weapon BaseKnuckle { get; set; }

    private protected float CurrentHealth;
    private protected float CurrentSpeed;
    //private float TimeDamageEffect;
    private protected SpriteRenderer SR;
    private TextMeshProUGUI Heals;

    private void Start()

    {
        CurrentHealth = MaxHealth;
        CurrentSpeed = BaseSpeed;
        //_spriteRenderer = GetComponent<SpriteRenderer>();
        Heals = GetComponent<TextMeshProUGUI>();
    }

    public void TakeDamage(float Damage = 0f)
    {
        CurrentHealth -= Damage;
        //TimeDamageEffect = Time.time + 0.2f;
        if (SR != null)
            SR.color = Color.red;
        if (Heals != null) Heals.text = "Health: " + CurrentHealth;
    }




}
