using UnityEngine;
using System.Collections;

/*
 * 工厂方法模式 Factory Method
 * 
 * 定义产生对象的接口，由子类决定要产生哪一个类的对象
 * 
 * 将对象的产生和相关初始化工作集中到一个地方完成
 * 
 * 
 * 结合建造者模式
 * 
 * 建造指示者 CharacterBuilderSystem
 *	负责对象构建时的流程分析安排
 *	Construct 方法	定义对象组装流程 即调用Builder接口方法的顺序
 * 	
 * 功能实现者接口 ICharacterBuilder 
 * 功能实现者 SoldierBuilder
 * 
 * SoldierBuildParam提供方法让SoldierBuilder将各部位功能设置给具体的Soldier
 */

// 產生遊戲角色工廠
public class CharacterFactory : ICharacterFactory
{
	// 角色建立指導者
	private CharacterBuilderSystem m_BuilderDirector = new CharacterBuilderSystem( PBaseDefenseGame.Instance );

	// 建立Soldier
	public override ISoldier CreateSoldier( ENUM_Soldier emSoldier, ENUM_Weapon emWeapon, int Lv, Vector3 SpawnPosition)
	{
		// 產生Soldier的參數
		SoldierBuildParam SoldierParam = new SoldierBuildParam();

		/*
		 * 工厂只负责简单的“产生”
		 * 
		 */

		// 產生對應的Character
		switch( emSoldier)
		{
		case ENUM_Soldier.Rookie:
			SoldierParam.NewCharacter = new SoldierRookie();
			break;
		case ENUM_Soldier.Sergeant:
			SoldierParam.NewCharacter = new SoldierSergeant();
			break;
		case ENUM_Soldier.Captain:
			SoldierParam.NewCharacter = new SoldierCaptain();
			break;
		default:
            Debug.LogWarning("CreateSoldier:無法建立[" + emSoldier + "]");
			return null;
		}

		if( SoldierParam.NewCharacter == null)
			return null;

		// 設定共用參數
		SoldierParam.emWeapon = emWeapon;
		SoldierParam.SpawnPosition = SpawnPosition;
		SoldierParam.Lv		  = Lv;

		/*
		 * 搭配建造者模式 Builder
		 * 将复杂的构造流程以一个类封装
		 * 
		 * 传递需要的参数，产生不同阵营的角色
		 * 
		 * 
		 * 完成角色功能的组装
		 */


		//  產生對應的Builder及設定參數
		SoldierBuilder theSoldierBuilder = new SoldierBuilder();
		theSoldierBuilder.SetBuildParam( SoldierParam ); 
		
		// 產生
		m_BuilderDirector.Construct( theSoldierBuilder );
		return SoldierParam.NewCharacter  as ISoldier;
	}
	
	// 建立Enemy
	public override IEnemy CreateEnemy( ENUM_Enemy emEnemy, ENUM_Weapon emWeapon, Vector3 SpawnPosition, Vector3 AttackPosition)
	{
		// 產生Enemy的參數
		EnemyBuildParam EnemyParam = new EnemyBuildParam();

		// 產生對應的Character
		switch( emEnemy)
		{
		case ENUM_Enemy.Elf:
			EnemyParam.NewCharacter = new EnemyElf();
			break;
		case ENUM_Enemy.Troll:
			EnemyParam.NewCharacter = new EnemyTroll();
			break;
		case ENUM_Enemy.Ogre:
			EnemyParam.NewCharacter = new EnemyOgre();
			break;
		default:
			Debug.LogWarning("無法建立["+emEnemy+"]");
			return null;
		}

		if( EnemyParam.NewCharacter == null)
			return null;

		// 設定共用參數
		EnemyParam.emWeapon = emWeapon;
		EnemyParam.SpawnPosition = SpawnPosition;
		EnemyParam.AttackPosition = AttackPosition;
				
		//  產生對應的Builder及設定參數
		EnemyBuilder theEnemyBuilder = new EnemyBuilder();
		theEnemyBuilder.SetBuildParam( EnemyParam ); 
		
		// 產生
		m_BuilderDirector.Construct( theEnemyBuilder );
		return EnemyParam.NewCharacter  as IEnemy;
	}

}


