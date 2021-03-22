using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }

public class BattleSystem : MonoBehaviour
{
	[SerializeField]
	public GameObject _player;
	public Vector3 positionStart = new Vector3(40, 3, 5);
	[SerializeField]
	public GameObject _enemy;
	public Vector3 enemyFightPos = new Vector3(-11.62f, 1.60f, 5.35f);
	public GameObject enemyGO;

	[SerializeField]
	private Camera mainCamera;
	[SerializeField]
	private Camera battleCamera;

	//Data player attack
	public const float atk1 = 0f;
	public const float atk2 = 0.33f;
	public const float atk3 = 0.66f;
	public const float atk4 = 1f;
	public bool oneAttackOnly = true;
	public const int dmgBuff = 8;
	public const int defaultDmg = 5;
	public const int defaultXpWin = 10;

	//Data enemy attack
	public const float enemyAtk1 =0f;
	public const float enemyAtk2 =0.5f;
	public const float enemyAtk3 =1f;
	public const int minDmg = 1;

	//Data animator
	public Animator _animatorPlayer;
	public Animator _animatorEnemy;

	//WaitSecond
	public const float oneS = 1f;
	public const float twoS = 2f;

	Unit playerUnit;
	Unit enemyUnit;

	public Text dialogueText;
	public Text playerHP;
	public Text enemyHP;

	public BattleState state;

	// Start is called before the first frame update
	void Start()
	{
		state = BattleState.START;
		StartCoroutine(SetupBattle());
	}

	IEnumerator SetupBattle()
	{
		enemyGO = Instantiate(_enemy, enemyFightPos, Quaternion.AngleAxis(-90, Vector3.up)); //Instantiate enemy
																							 //magic numbers
		_animatorPlayer = _player.GetComponent<Animator>(); //Get player animator
		_animatorEnemy = enemyGO.GetComponent<Animator>(); //Get enemy animator
		enemyUnit = enemyGO.GetComponent<Unit>(); //Get enemy unit data
		playerUnit = _player.GetComponent<Unit>(); //Get player unit data

		//Update dialogue text 
		dialogueText.text = "Battle Start!"; 
		playerHP.text = "Lv : "+playerUnit.unitLevel + " " + playerUnit.currentHP + "/" + playerUnit.maxHP;
		enemyHP.text = "Lv : " + enemyUnit.unitLevel + " " + enemyUnit.currentHP + "/" + enemyUnit.maxHP;

		yield return new WaitForSeconds(oneS);

		//Start battle
		state = BattleState.PLAYERTURN;
		PlayerTurn();
	}

	IEnumerator PlayerAttack()
	{
		//Check no double attaque button
		if (oneAttackOnly)
		{
			oneAttackOnly = false;
			StartCoroutine(AtkAnim()); //Coroutine for animation
			bool isDead = enemyUnit.TakeDamage(playerUnit.damage); //damage taken

			dialogueText.text = "You deal "+playerUnit.damage+" damages!";
			enemyHP.text = "Lv : " + enemyUnit.unitLevel + " " + enemyUnit.currentHP + "/" + enemyUnit.maxHP;

			yield return new WaitForSeconds(twoS);

			//Check if enemy dead
			if (isDead)
			{
				state = BattleState.WON;
				_animatorEnemy.SetBool("IsDead", true);
				EndBattle();
			}
			else
			{
				state = BattleState.ENEMYTURN;
				StartCoroutine(EnemyTurn());
			}
		}
	}

	IEnumerator EnemyTurn()
	{
		StartCoroutine(EnemyAtk()); //Coroutine for enemy animation
		dialogueText.text = enemyUnit.unitName + " attacks!";

		yield return new WaitForSeconds(oneS);

		//pourquoi passer par un timer alors que tu aurais pu utiliser les anim events ?
		dialogueText.text = enemyUnit.unitName + " deals " + enemyUnit.damage + " damages!";
		bool isDead = playerUnit.TakeDamage(enemyUnit.damage);
		playerHP.text = "Lv : " + playerUnit.unitLevel +" "+ playerUnit.currentHP + "/" + playerUnit.maxHP;

		yield return new WaitForSeconds(twoS);

		//Check if player is dead
		if (isDead)
		{	
			state = BattleState.LOST;
			_animatorPlayer.SetBool("IsDead", true);
			EndBattle();
		}
		else
		{
			state = BattleState.PLAYERTURN;
			PlayerTurn();
		}

	}

	IEnumerator SwitchScene()
    {
		yield return new WaitForSeconds(twoS);
		yield return new WaitForSeconds(twoS);//deux wait pour 2 fois plus de plaisir ?
		//Update player stat after fight
		playerUnit.damage = defaultDmg * playerUnit.unitLevel;
		playerUnit.ExperienceGain(enemyUnit.experience);
		//Allow player to move again
		_player.GetComponent<PlayerController>().enabled = true;
		_player.GetComponent<AnimationHandler>().enabled = true;
		//Destroy enemy GameObjet
		Destroy(enemyGO);

		//Teleport player to position start
		_player.transform.rotation = Quaternion.AngleAxis(0, Vector3.up);//magic numbers
		_player.transform.position = positionStart;
		battleCamera.gameObject.SetActive(false);
		mainCamera.gameObject.SetActive(true);

		//Reset battle state
		state = BattleState.START;
		StartCoroutine(SetupBattle());
	}
	void EndBattle()
	{
		if (state == BattleState.WON)
		{
			dialogueText.text = "You won the battle!";
			new WaitForSeconds(twoS);
			StartCoroutine(SwitchScene());
		}
		else if (state == BattleState.LOST)
		{
			dialogueText.text = "You were defeated.";
		}

	}

	void PlayerTurn()
	{
		oneAttackOnly = true;
		dialogueText.text = "Choose an action:";
	}

	public void OnButtonClick()
	{
		if (state != BattleState.PLAYERTURN)
			return;

		StartCoroutine(PlayerAttack());
	}

	IEnumerator AtkAnim()
	{
		
		//Check button press
		var go = EventSystem.current.currentSelectedGameObject;
		_animatorPlayer.SetBool("IsAttacking", true);
		if (go.name == "Attaque 1")
		{
			playerUnit.damage += defaultDmg + playerUnit.unitLevel;
			enemyUnit.damage += defaultDmg + playerUnit.unitLevel;
			_animatorPlayer.SetFloat("AttackType", atk1);
		}
		if (go.name == "Attaque 2")
		{
			playerUnit.damage = dmgBuff + playerUnit.unitLevel;
			_animatorPlayer.SetFloat("AttackType", atk2);
		}
		if (go.name == "Attaque 3")
		{
			playerUnit.damage += playerUnit.unitLevel;
			_animatorPlayer.SetFloat("AttackType", atk3);
		}
		if (go.name == "Attaque 4")
		{
			playerUnit.damage = defaultDmg + playerUnit.unitLevel;
			enemyUnit.damage -= minDmg;
			_animatorPlayer.SetFloat("AttackType", atk4);
		}
		_animatorEnemy.SetBool("IsHit", true);


		yield return new WaitForSeconds(oneS);

		_animatorEnemy.SetBool("IsHit", false);
		_animatorPlayer.SetBool("IsAttacking", false);
		//tu aurais passer par des trigger, comme ça, pas besoin de reset les bool des animators
	}

	IEnumerator EnemyAtk()
    {
		//Random attack check from enemy
		int RandomAttack = Random.Range(1, 4);//magic numbers
		switch (RandomAttack)
		{
			case 1:
				enemyUnit.damage -= minDmg;//il peut s'attaquer lui-même ?
				break;
			case 2:
				playerUnit.damage -= minDmg;
				break;
			case 3:
				enemyUnit.damage += minDmg;
				break;
		}
		_animatorEnemy.SetFloat("AttackType",(float)1/RandomAttack);//magic numbers, mais c'est astucieux
		_animatorEnemy.SetBool("IsAttacking", true);
		_animatorPlayer.SetBool("IsHit", true);


		yield return new WaitForSeconds(oneS);

		_animatorPlayer.SetBool("IsHit", false);
		_animatorEnemy.SetBool("IsAttacking", false);
	}
}