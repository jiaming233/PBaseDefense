using UnityEngine;
using System;
using System.Collections;

/*
 *  类比GameFramework
 *  
 *  指定初始状态：
 *  GameLoop + SceneStateController ~= ProcedureComponent has FsmManager 由有限状态机管理器轮询所有注册的状态
 *  
 *  ISceneState ~= ProcedureBase:FsmState 游戏状态/流程 ==> “场景类”	
 *	“场景类”维护一个场景/流程，负责初始化；进入、轮询[判断状态转换条件，负责状态转换]、离开状态
 *	
 *	让多个场景类相互合作、转换，可使用 状态模式 State ：随内部状态的改变而变化行为
 *	
 *	
 *	使用状态模式进行场景、状态切换的优点：
 *	
 *	1. 不再使用switch/case判断，避免新增状态时大量修改现有代码
 *	2. 在每个具体的状态类中执行相关的操作
 *	3. 项目之间可以共享场景，节省开发成本，例如游戏初始化场景定义好登录、验证、数据同步等步骤
 *	
 *	其他应用方式：
 *	1. 角色AI
 *	2. 游戏服务器连线状态，一般包含开始连线、连线中、断线等
 *	3. 关卡进行状态
 */

// 遊戲主迴圈
public class GameLoop : MonoBehaviour 
{
	// 場景狀態
	SceneStateController m_SceneStateController = new SceneStateController();

	// 
	void Awake()
	{
		// 切換場景不會被刪除
		GameObject.DontDestroyOnLoad( this.gameObject );		 

		// 亂數種子
		UnityEngine.Random.seed =(int)DateTime.Now.Ticks;
	}

	// Use this for initialization
	void Start () 
	{
		// 設定起始的場景
		m_SceneStateController.SetState(new StartState(m_SceneStateController), "");
	}

    /// <summary>
    /// 游戏主循环
    /// </summary>
	// Update is called once per frame
	void Update () 
	{
		m_SceneStateController.StateUpdate();	
	}
}
