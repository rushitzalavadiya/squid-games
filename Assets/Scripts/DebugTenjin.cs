// using System.Collections.Generic;
// using Voodoo.Sauce.Internal;
//
// public class DebugTenjin : BaseTenjin
// {
// 	private const string TAG = "DebugTenjin";
//
// 	public override void Connect()
// 	{
// 		VoodooLog.Log("DebugTenjin", "Connecting " + base.ApiKey);
// 	}
//
// 	public override void Connect(string deferredDeeplink)
// 	{
// 		VoodooLog.Log("DebugTenjin", "Connecting with deferredDeeplink " + deferredDeeplink);
// 	}
//
// 	public override void Init(string apiKey)
// 	{
// 		VoodooLog.Log("DebugTenjin", "Initializing " + apiKey);
// 		base.ApiKey = apiKey;
// 	}
//
// 	public override void Init(string apiKey, string sharedSecret)
// 	{
// 		VoodooLog.Log("DebugTenjin", "Initializing with secret " + apiKey);
// 		base.ApiKey = apiKey;
// 		base.SharedSecret = sharedSecret;
// 	}
//
// 	public override void SendEvent(string eventName)
// 	{
// 		VoodooLog.Log("DebugTenjin", "Sending Event " + eventName);
// 	}
//
// 	public override void SendEvent(string eventName, string eventValue)
// 	{
// 		VoodooLog.Log("DebugTenjin", "Sending Event " + eventName + " : " + eventValue);
// 	}
//
// 	public override void Transaction(string productId, string currencyCode, int quantity, double unitPrice, string transactionId, string receipt, string signature)
// 	{
// 		VoodooLog.Log("DebugTenjin", "Transaction " + productId + ", " + currencyCode + ", " + quantity + ", " + unitPrice + ", " + transactionId + ", " + receipt + ", " + signature);
// 	}
//
// 	public override void GetDeeplink(Tenjin.DeferredDeeplinkDelegate deferredDeeplinkDelegate)
// 	{
// 		VoodooLog.Log("DebugTenjin", "Sending DebugTenjin::GetDeeplink");
// 	}
//
// 	public override void OptIn()
// 	{
// 		VoodooLog.Log("DebugTenjin", "OptIn ");
// 	}
//
// 	public override void OptOut()
// 	{
// 		VoodooLog.Log("DebugTenjin", "OptOut ");
// 	}
//
// 	public override void OptInParams(List<string> parameters)
// 	{
// 		VoodooLog.Log("DebugTenjin", "OptInParams");
// 	}
//
// 	public override void OptOutParams(List<string> parameters)
// 	{
// 		VoodooLog.Log("DebugTenjin", "OptOutParams");
// 	}
//
// 	public override void AppendAppSubversion(int subversion)
// 	{
// 		VoodooLog.Log("DebugTenjin", "AppendAppSubversion: " + subversion);
// 	}
// }
