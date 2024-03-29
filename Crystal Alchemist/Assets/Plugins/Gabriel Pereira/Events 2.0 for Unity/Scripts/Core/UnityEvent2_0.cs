﻿using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngineInternal;

namespace UnityEngine.Events
{
	/// <summary>
	/// Events 2.0 for Unity
	/// </summary>
	[Serializable]
	public class UnityEvent2 : UnityEventBase2
	{
		private readonly object[] m_InvokeArray = new object[0];

		/// <summary>
		///   <para>Constructor.</para>
		/// </summary>
		public UnityEvent2() { }

		/// <summary>
		/// Add a non persistent listener to the UnityEvent.
		/// </summary>
		/// <param name="call">Callback function.</param>
		public void AddListener(UnityAction call)
		{
			AddCall(GetDelegate(call));
		}

		/// <summary>
		/// Remove a non persistent listener from the UnityEvent.
		/// </summary>
		/// <param name="call">Callback function.</param>
		public void RemoveListener(UnityAction call)
		{
			RemoveListener(call.Target, call.Method);
		}

		protected override MethodInfo FindMethod_Impl(object targetObj, string name)
		{
			return GetValidMethodInfo(targetObj, name, new Type[0]);
		}

		internal override BaseInvokableCall2 GetDelegate(object target, MethodInfo theFunction)
		{
			return new InvokableCall2(target, theFunction);
		}

		private static BaseInvokableCall2 GetDelegate(UnityAction action)
		{
			return new InvokableCall2(action);
		}

		/// <summary>
		/// Invoke all registered callbacks (runtime and persistent).
		/// </summary>
		public void Invoke()
		{
			List<BaseInvokableCall2> calls = PrepareInvoke();
			for (var i = 0; i < calls.Count; i++)
			{
				var curCall = calls[i] as InvokableCall2;
				if (curCall != null)
					curCall.Invoke();
				else
					calls[i].Invoke(m_InvokeArray);
			}
		}

		internal void AddPersistentListener(UnityAction call)
		{
			AddPersistentListener(call, UnityEventCallState.RuntimeOnly);
		}

		internal void AddPersistentListener(UnityAction call, UnityEventCallState callState)
		{
			int persistentEventCount = GetPersistentEventCount();
			AddPersistentListener();
			RegisterPersistentListener(persistentEventCount, call);
			SetPersistentListenerState(persistentEventCount, callState);
		}

		internal void RegisterPersistentListener(int index, UnityAction call)
		{
			if (call == null)
				Debug.LogWarning("Registering a Listener requires an action");
			else
				RegisterPersistentListener(index, call.Target, call.Method);
		}
	}
}