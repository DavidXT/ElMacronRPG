using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
	//Data unit et stat
	public string unitName;
	public int unitLevel;
	public int experience;

	public int damage;

	public int maxHP;
	public int currentHP;


	public bool TakeDamage(int dmg)
	{
		currentHP -= dmg;

		if (currentHP <= 0)//magic numbers
			return true;//Si unit en vie
		else
			return false;//Si unit morte
		//tu peux simplifer :
		//return currentHP <= 0;
	}

	public bool ExperienceGain(int exp)
    {
		experience += exp;//Gain d'xp
		if(experience == 100 * unitLevel)//Condition de lv up
		{//magic numbers
			unitLevel += 1;//Gain de lv
			experience -= 100;//Reset experience
			//tu ne reset pas l'exp, vu que tu ne supprimes que 100 d'exp et qu'il faut 2*100 pour le niveau 3
			maxHP = 100 * unitLevel;
			currentHP = maxHP;//Heal quand lv up
        }
		return true;//tu renvoies toujours true, pourquoi ?
    }
}
