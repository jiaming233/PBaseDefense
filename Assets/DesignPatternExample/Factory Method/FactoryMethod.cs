﻿using UnityEngine;
using System.Collections;

namespace DesignPattern_FactoryMethod
{
	// 成品物件類型
	public abstract class Product
	{
	}
	
	// 成品物件類型A
	public class ConcreteProductA : Product
	{
		public ConcreteProductA()
		{
			Debug.Log("生成物件類型A");
		}
	}
	
	// 成品物件類型B
	public class ConcreteProductB : Product
	{
		public ConcreteProductB()
		{
			Debug.Log("生成物件類型B");
		}
	}
	
	// 宣告factory , 子類別會回傳對應的Product型別之物件
	public abstract class Creator
	{
		public abstract Product FactoryMethod();
	}

	/// <summary>
	/// 1. 工厂子类产生对应的产品类对象
	/// 
	/// 可能发生类暴增的情况
	/// </summary>

	// 產生ProductA的工廠
	public class ConcreteCreatorProductA : Creator
	{
		public ConcreteCreatorProductA()
		{
			Debug.Log("產生工廠:ConcreteCreatorProductA");
		}

		public override Product FactoryMethod()
		{
			return new ConcreteProductA();
		}
	}

	// 產生ProductB的工廠
	public class ConcreteCreatorProductB : Creator
	{
		public ConcreteCreatorProductB()
		{
			Debug.Log("產生工廠:ConcreteCreatorProductB");
		}
		public override Product FactoryMethod()
		{
			return new ConcreteProductB();
		}
	}

	// 宣告factory method，它會依參數Type的提示回傳對應Product類別物件
	public abstract class Creator_MethodType
	{
		public abstract Product FactoryMethod(int Type);
	}

	// 覆寫factory method，以回傳Product型別之物件
	public class ConcreteCreator_MethodType: Creator_MethodType
	{
		public ConcreteCreator_MethodType()
		{
			Debug.Log("產生工廠:ConcreteCreator_MethodType");
		}

		/// <summary>
		/// 2. 工厂方法添加参数 
		/// 
		/// 根据参数类型产生对应的产品类
		/// 
		/// switch case
		/// </summary>
		/// <param name="Type"></param>
		/// <returns></returns>
		public override Product FactoryMethod(int Type)
		{
			switch( Type )
			{
			case 1: 
				return new ConcreteProductA();
			case 2:
				return new ConcreteProductB();			
			}
			Debug.Log("Type["+Type+"]無法產生物件");
			return null;
		}
	}
		
	// 宣告factory method界箅,並使用Generic定義方法
	interface Creator_GenericMethod
	{
		Product FactoryMethod<T>() where T: Product, new();
	}

	// 覆寫factory method，以回傳Product型別之物件
	public class ConcreteCreator_GenericMethod : Creator_GenericMethod
	{
		public ConcreteCreator_GenericMethod()
		{
			Debug.Log("產生工廠:ConcreteCreator_GenericMethod");
		}

		/// <summary>
		/// 3. 泛型方法
		/// 
		/// 相对于泛型类，可以获取工厂接口
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public Product FactoryMethod<T>() where T: Product, new()
		{
			return new T();
		}
	}

	/// <summary>
	/// 4. 泛型类
	/// 
	/// 相对于1. 省去了继承
	/// </summary>
	/// <typeparam name="T"></typeparam>
	// 宣告Generic factory類別
	public class Creator_GenericClass<T> where T : Product,new()
	{
		public Creator_GenericClass()
		{
			Debug.Log("產生工廠:Creator_GenericClass<"+typeof(T).ToString()+">");
		}

		public Product FactoryMethod()
		{
			return new T();
		}
	}


}