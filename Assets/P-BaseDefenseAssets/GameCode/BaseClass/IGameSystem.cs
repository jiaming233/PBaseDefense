using UnityEngine;
using System.Collections;

// 遊戲子系統共用界面
public abstract class IGameSystem
{
	protected PBaseDefenseGame m_PBDGame = null;
    /// <summary>
    /// 构造时设置中介者成员
    /// </summary>
    /// <param name="PBDGame"></param>
	public IGameSystem( PBaseDefenseGame PBDGame )
	{
		m_PBDGame = PBDGame;
	}

	public virtual void Initialize(){}
	public virtual void Release(){}
	public virtual void Update(){}

}
