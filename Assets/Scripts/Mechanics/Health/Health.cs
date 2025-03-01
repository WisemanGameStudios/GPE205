using UnityEngine;

public class Health : MonoBehaviour
{
    // Health Variables 
    public float currentHealth;
    public float maxHealth;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {

    }




    public void TakeDamage(float amount, Pawn source)
    {
        currentHealth = currentHealth - amount;

        Debug.Log(source.name + " did " + amount + " damage to " + gameObject.name);

        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (currentHealth <= 0)
        {
            Die(source);
        }
    }

    public void Heal(float amount, Pawn source)
    {
        currentHealth = currentHealth + amount;

        Debug.Log(source.name + " did " + amount + " healing to " + gameObject.name);

        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    }
    public void Die(Pawn source)
    {
        Destroy(gameObject);
    }
}