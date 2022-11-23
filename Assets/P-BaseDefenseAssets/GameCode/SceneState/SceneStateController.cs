﻿using UnityEngine;
using System.Collections;

// 場景狀態控制者
public class SceneStateController
{
	private ISceneState m_State;	
	private bool 	m_bRunBegin = false;
	
	public SceneStateController()
	{}

	// 設定狀態
	public void SetState(ISceneState State, string LoadSceneName)
	{
		//Debug.Log ("SetState:"+State.ToString());
		m_bRunBegin = false;

		// 載入場景
		LoadScene( LoadSceneName );

		// 通知前一個State結束
		if( m_State != null )
			m_State.StateEnd();

		// 設定
		m_State=State;	
	}

	// 載入場景
	private void LoadScene(string LoadSceneName)
	{
		if( LoadSceneName==null || LoadSceneName.Length == 0 )
			return ;
		Application.LoadLevel( LoadSceneName );
	}

    /// <summary>
    /// 声明需要被定期调用的函数
    /// 在GameLoop.Update中调用
    /// 达到类不用继承Monobehaviour也能定期更新，不必与Unity引擎产生依赖关系
    /// 
    /// 类比GameFramework  GameFrameworkEntry/GameFrameworkModule
    /// </summary>
    // 更新
    public void StateUpdate()
	{
		// 是否還在載入
		if( Application.isLoadingLevel)
			return ;

		// 通知新的State開始
		if( m_State != null && m_bRunBegin==false)
		{
			m_State.StateBegin();
			m_bRunBegin = true;
		}

		if( m_State != null)
			m_State.StateUpdate();
	}
}
