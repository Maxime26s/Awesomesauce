using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public float maxHp, hp;
    public DropTable dropTable;
    // Start is called before the first frame update
    void Start()
    {
        hp = maxHp;
    }

    public void TakeDamage(float damage)
    {
        hp -= damage;
        if (hp <= 0)
            Death();
    }

    void Death()
    {
        for (int i = 0; i < dropTable.item.Length; i++)
        {
            if (Random.Range(0f, 100f) <= dropTable.dropRate[i])
            {
                for (int j=0; j<Random.Range(dropTable.amountMin[i], dropTable.amountMax[i]); j++)
                {
                    Instantiate(dropTable.item[i], transform.position, transform.rotation);
                }
            }
        }
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Weapon")
        {
            TakeDamage(5);
        }
    }
}
