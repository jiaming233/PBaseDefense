using UnityEngine;
using System.Collections;

/*
 * 策略模式 Strategy
 * 
 * 定义一组算法，可以彼此交换使用
 * 针对不同条件进行不同计算
 * 
 * IAttrStrategy 策略接口类
 * 
 * EnemyAttrStrategy/SoldierAttrStrategy 策略实现类
 * 
 * ICharacterAttr 策略客户端，拥有一个IAttrStrategy的引用，通过对象引用获取想要的计算结果
 */

// 角色數值計算界面
/// <summary>
/// 三个方法都和角色在攻击流程中计算属性数值相关
/// </summary>
public abstract class IAttrStrategy
{
	// 初始的數值
	public abstract void InitAttr( ICharacterAttr CharacterAttr );
	
	// 攻擊加乘
	public abstract int GetAtkPlusValue( ICharacterAttr CharacterAttr );
	
	// 取得減傷害值
	public abstract int GetDmgDescValue( ICharacterAttr CharacterAttr );
}
