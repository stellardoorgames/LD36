using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Character : MonoBehaviour {

	public string characterName;

	public float ego = 8f;

	public Sprite texture;
	public Sprite textureHurt;

	protected virtual void Start () {
	}

	public virtual bool TakeAttack(float damage)
	{
		if (Random.value > 0.8f)
			return false;
		
		TakeDamage (damage);

		return true;
	}

	public void TakeDamage (float damage)
	{
		ego -= damage;
	}

	public virtual void Die()
	{
		
	}
}
