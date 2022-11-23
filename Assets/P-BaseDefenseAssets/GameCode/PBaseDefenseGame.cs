using UnityEngine;
using System.Collections;

/*
 * 游戏主要类，整合各个子功能
 * 类比GameFramework，相当于GameEntry
 * 
 * 外观模式 Facade ：
 *  整合子系统，为子系统定义一组统一对外的接口，将系统内部互动细节隐藏，让子系统更容易被使用 
 *  例如，汽车的方向盘、微波炉的操作面板，都是制造商提供给用户的外观
 * 
 * 优点：
 * 1. 减少耦合度
 * 2. 提高系统安全性
 * 
 * 注意：所有子系统整合在一个外观类中会导致接口类过于庞大，可以将功能相近的子系统进行整合，减少内部系统的依赖性
 * 
 * 游戏各系统的初始化和流程串接与使用它们的类没有关系，此时就需要一个外观模式类重新组织
 * 
 * 与其他模式的合作
 * 1. 使用单例模式 Singleton 产生唯一的类对象
 * 2. 子系统间使用中介者模式 Mediator 互相沟通
 * 3. 使用观察者模式 Observer 实现游戏事件系统 GameEventSystem
 *
 *
 * 其他应用
 * 1. 网络引擎
 * 2. 数据库引擎
 * 
 * 
 * 
 * 中介者模式 Mediator 
 * 
 * 负责内部 游戏系统IGameSystem 和 玩家界面 IUserInterface 
 * 两大系统以及游戏系统下的子系统之间都不会直接互相引用，而是通过PBaseDefenseGame类的成员发出请求
 * 
 * 搭配观察者模式 Observer 可以避免接口爆炸的情况
 */

public class PBaseDefenseGame
{
	//------------------------------------------------------------------------
	// Singleton模版

    /*
     * 单例模式：确认类只有一个对象 [静态类属性]，并提供一个全局的方法 [静态类方法] 来获取这个对象
     * 
     */
	private static PBaseDefenseGame _instance;
	public static PBaseDefenseGame Instance
	{
		get//getter 存取运算符
		{
			if (_instance == null)			
				_instance = new PBaseDefenseGame();
			return _instance;
		}
	}

	// 場景狀態控制
	private bool m_bGameOver = false;
	
	// 遊戲系統
	private GameEventSystem m_GameEventSystem = null;	 // 遊戲事件系統
	private CampSystem m_CampSystem	 = null; 			 // 兵營系統
	private StageSystem m_StageSystem = null; 			 // 關卡系統
	private CharacterSystem m_CharacterSystem = null; 	 // 角色管理系統
	private APSystem m_ApSystem = null; 				 // 行動力系統
	private AchievementSystem m_AchievementSystem = null;// 成就系統
	// 界面
	private CampInfoUI m_CampInfoUI = null;				 // 兵營界面
	private SoldierInfoUI m_SoldierInfoUI = null;		 // 戰士資訊界面
	private GameStateInfoUI m_GameStateInfoUI = null;	 // 遊戲狀態界面
	private GamePauseUI m_GamePauseUI = null;			 // 遊戲暫停界面
		
    /// <summary>
    /// 私有构造方法 无法被外部调用，防止产生多个类对象
    /// </summary>
	private PBaseDefenseGame()
	{}

	// 初始P-BaseDefense遊戲相關設定
	public void Initinal()
	{
		// 場景狀態控制
		m_bGameOver = false;
		// 遊戲系統
		m_GameEventSystem = new GameEventSystem(this);	// 遊戲事件系統
		m_CampSystem = new CampSystem(this);			// 兵營系統
		m_StageSystem = new StageSystem(this);			// 關卡系統
		m_CharacterSystem = new CharacterSystem(this); 	// 角色管理系統
		m_ApSystem = new APSystem(this);				// 行動力系統
		m_AchievementSystem = new AchievementSystem(this); // 成就系統
		// 界面
		m_CampInfoUI = new CampInfoUI(this); 			// 兵營資訊
		m_SoldierInfoUI = new SoldierInfoUI(this); 		// Soldier資訊									
		m_GameStateInfoUI = new GameStateInfoUI(this); 	// 遊戲資料
		m_GamePauseUI = new GamePauseUI (this);			// 遊戲暫停

		// 注入到其它系統
		EnemyAI.SetStageSystem( m_StageSystem );

		// 載入存檔
		LoadData();

		// 註冊遊戲事件系統
		ResigerGameEvent();
	}

	// 註冊遊戲事件系統
	private void ResigerGameEvent()
	{
		// 事件註冊
		m_GameEventSystem.RegisterObserver( ENUM_GameEvent.EnemyKilled, new EnemyKilledObserverUI(this));

		// Combo
		/*ComboObserver theComboObserver =new ComboObserver(this);
		m_GameEventSystem.RegisterObserver( ENUM_GameEvent.EnemyKilled, theComboObserver);
		m_GameEventSystem.RegisterObserver( ENUM_GameEvent.SoldierKilled, theComboObserver);*/

	}

	// 釋放遊戲系統
	public void Release()
	{
		// 遊戲系統
		m_GameEventSystem.Release();
		m_CampSystem.Release();
		m_StageSystem.Release();
		m_CharacterSystem.Release();
		m_ApSystem.Release();
		m_AchievementSystem.Release();
		// 界面
		m_CampInfoUI.Release();
		m_SoldierInfoUI.Release();
		m_GameStateInfoUI.Release();
		m_GamePauseUI.Release();
		UITool.ReleaseCanvas();

		// 存檔
		SaveData();
	}

	// 更新
	public void Update()
	{
		// 玩家輸入
		InputProcess();

		// 遊戲系統更新
		m_GameEventSystem.Update();
		m_CampSystem.Update();
		m_StageSystem.Update();
		m_CharacterSystem.Update();
		m_ApSystem.Update();
		m_AchievementSystem.Update();

		// 玩家界面更新
		m_CampInfoUI.Update();
		m_SoldierInfoUI.Update();
		m_GameStateInfoUI.Update();
		m_GamePauseUI.Update();
	}

	// 玩家輸入
	private void InputProcess()
	{
		//  Mouse左鍵
		if(Input.GetMouseButtonUp( 0 ) ==false)
			return ;
		
		//由攝影機產生一條射線
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit[] hits = Physics.RaycastAll(ray);		
		
		// 走訪每一個被Hit到的GameObject
		foreach (RaycastHit hit in hits)
		{
			// 是否有兵營點擊
			CampOnClick CampClickScript = hit.transform.gameObject.GetComponent<CampOnClick>();
			if( CampClickScript!=null )
			{
				CampClickScript.OnClick();
				return;
			}
			
			// 是否有角色點擊
			SoldierOnClick SoldierClickScript = hit.transform.gameObject.GetComponent<SoldierOnClick>();
			if( SoldierClickScript!=null )
			{
				SoldierClickScript.OnClick();
				return ;
			}
		}
	}
	
	// 遊戲狀態
	public bool ThisGameIsOver()
	{
		return m_bGameOver;
	}

	// 換回主選單
	public void ChangeToMainMenu()
	{
		m_bGameOver = true;
	}

	// 增加Soldier
	public void AddSoldier( ISoldier theSoldier)
	{
		if( m_CharacterSystem !=null)
			m_CharacterSystem.AddSoldier( theSoldier );
	}

	// 移除Soldier
	public void RemoveSoldier( ISoldier theSoldier)
	{
		if( m_CharacterSystem !=null)
			m_CharacterSystem.RemoveSoldier( theSoldier );
	}
	
	// 增加Enemy
	public void AddEnemy( IEnemy theEnemy)
	{
		if( m_CharacterSystem !=null)
			m_CharacterSystem.AddEnemy( theEnemy );
	}

	// 移除Enemy
	public void RemoveEnemy( IEnemy theEnemy)
	{
		if( m_CharacterSystem !=null)
			m_CharacterSystem.RemoveEnemy( theEnemy );
	}

	// 目前敵人數量
	public int GetEnemyCount()
	{
		if( m_CharacterSystem !=null)
			return m_CharacterSystem.GetEnemyCount();
		return 0;
	}

	// 增加敵人陣亡數量(不透過GameEventSystem呼叫) 
	public void AddEnemyKilledCount()
	{
		m_StageSystem.AddEnemyKilledCount();
	}

	// 執行角色系統的Visitor
	public void RunCharacterVisitor(ICharacterVisitor Visitor)
	{
		m_CharacterSystem.RunVisitor( Visitor );
	}

	// 註冊遊戲事件
	public void RegisterGameEvent( ENUM_GameEvent emGameEvent, IGameEventObserver Observer)
	{
		m_GameEventSystem.RegisterObserver( emGameEvent , Observer );
	}

	// 通知遊戲事件
	public void NotifyGameEvent( ENUM_GameEvent emGameEvent, System.Object Param )
	{
		m_GameEventSystem.NotifySubject( emGameEvent, Param);
	}

	// 顯示兵營資訊
	public void ShowCampInfo( ICamp Camp )
	{
		m_CampInfoUI.ShowInfo( Camp );
		m_SoldierInfoUI.Hide();
	}

	// 顯示Soldier資訊
	public void ShowSoldierInfo( ISoldier Soldier )
	{
		m_SoldierInfoUI.ShowInfo( Soldier );
		m_CampInfoUI.Hide();
	}

	// 通知AP更動
	public void APChange( int NowAP)
	{
		m_GameStateInfoUI.ShowAP( NowAP);
	}

	// 花費AP
	public bool CostAP( int ApValue)
	{
		return m_ApSystem.CostAP( ApValue );
	}

	// 顯示關卡
	public void ShowNowStageLv( int Lv)
	{
		m_GameStateInfoUI.ShowNowStageLv(Lv);
	}

	//  遊戲訊息
	public void ShowGameMsg( string Msg)
	{
		m_GameStateInfoUI.ShowMsg( Msg );
	}

	// 顯示Heart
	public void ShowHeart(int Value)
	{
		m_GameStateInfoUI.ShowHeart( Value);
		ShowGameMsg("陣營被攻擊");
	}

	// 顯示暫停
	public void GamePause()
	{
		if( m_GamePauseUI.IsVisible()==false)
			m_GamePauseUI.ShowGamePause( m_AchievementSystem.CreateSaveData() );
		else
			m_GamePauseUI.Hide();
	}

	// 存檔
	private void SaveData()
	{
		AchievementSaveData SaveData = m_AchievementSystem.CreateSaveData();
		SaveData.SaveData();
	}

	// 取回存檔
	private AchievementSaveData LoadData()
	{
		AchievementSaveData OldData = new AchievementSaveData();
		OldData.LoadData();
		m_AchievementSystem.SetSaveData( OldData );
		return OldData;
	}
	
	/*#region 直接取得角色數量的實作方式
	// 目前Soldier數量
	public int GetSoldierCount()
	{
		if( m_CharacterSystem !=null)
			return m_CharacterSystem.GetSoldierCount();
		return 0;
	}

	// 目前Soldier數量
	public int GetSoldierCount( ENUM_Soldier emSoldier)
	{
		if( m_CharacterSystem !=null)
			return m_CharacterSystem.GetSoldierCount(emSoldier);
		return 0;
	}	
	#endregion*/

}
