using UnityEngine;
using UnityEngine.Purchasing;

public class StoreListener : IStoreListener
{
	private IStoreController controller;

	private IExtensionProvider extensions;

	public void InitializeIAP()
	{
		ConfigurationBuilder configurationBuilder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
		configurationBuilder.AddProduct("com.battlecreek.offroadoutlaws.100gold", ProductType.Consumable);
		configurationBuilder.AddProduct("com.battlecreek.offroadoutlaws.250gold", ProductType.Consumable);
		configurationBuilder.AddProduct("com.battlecreek.offroadoutlaws.500gold", ProductType.Consumable);
		configurationBuilder.AddProduct("com.battlecreek.offroadoutlaws.750gold", ProductType.Consumable);
		configurationBuilder.AddProduct("com.battlecreek.offroadoutlaws.4000gold", ProductType.Consumable);
		configurationBuilder.AddProduct("com.battlecreek.offroadoutlaws.10000gold", ProductType.Consumable);
		configurationBuilder.AddProduct("com.battlecreek.offroadoutlaws.monthlyvip", ProductType.Subscription);
		configurationBuilder.AddProduct("com.battlecreek.offroadoutlaws.timedvehiclepurchase", ProductType.Consumable);
		configurationBuilder.AddProduct("com.battlecreek.offroadoutlaws.premiumvehiclepurchase", ProductType.Consumable);
		UnityPurchasing.Initialize(this, configurationBuilder);
	}

	public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
	{
		this.controller = controller;
		this.extensions = extensions;
	}

	public void OnInitializeFailed(InitializationFailureReason error)
	{
		UnityEngine.Debug.Log("Could not initialize UnityIAP");
	}

	public void OnPurchaseFailed(Product i, PurchaseFailureReason p)
	{
		MenuManager.Instance.ShowMessage("Could not complete: " + p.ToString());
	}

	public void RestoreIAP()
	{
	}

	public void PurchaseIAP(string product)
	{
		UnityEngine.Debug.Log("Purchasing: " + product);
		if (controller == null)
		{
			MenuManager.Instance.ShowMessage("Cannot purchase right now. Make sure you are online?");
		}
		else
		{
			controller.InitiatePurchase(product);
		}
	}

	public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
	{
		UnityEngine.Debug.Log("Processing purchase: " + purchaseEvent.purchasedProduct);
		if (purchaseEvent.purchasedProduct != null)
		{
			StatsData statsData = GameState.LoadStatsData();
			int num = 0;
			bool flag = false;
			bool flag2 = false;
			if (purchaseEvent.purchasedProduct.definition.id == "com.battlecreek.offroadoutlaws.monthlyvip" && statsData.IsMember)
			{
				statsData.IsMember = true;
				GameState.SaveStatsData(statsData);
				return PurchaseProcessingResult.Complete;
			}
			switch (purchaseEvent.purchasedProduct.definition.id)
			{
			case "com.battlecreek.offroadoutlaws.100gold":
				num = 100;
				break;
			case "com.battlecreek.offroadoutlaws.250gold":
				num = 250;
				break;
			case "com.battlecreek.offroadoutlaws.500gold":
				num = 500;
				break;
			case "com.battlecreek.offroadoutlaws.750gold":
				num = 750;
				break;
			case "com.battlecreek.offroadoutlaws.4000gold":
				num = 4000;
				break;
			case "com.battlecreek.offroadoutlaws.10000gold":
				num = 10000;
				break;
			case "com.battlecreek.offroadoutlaws.monthlyvip":
				num = 200;
				flag = true;
				break;
			case "com.battlecreek.offroadoutlaws.timedvehiclepurchase":
				flag2 = true;
				break;
			case "com.battlecreek.offroadoutlaws.premiumvehiclepurchase":
				flag2 = true;
				break;
			}
			string str = "Thanks! You now have " + num + " more gold";
			if (flag)
			{
				str += " and are a member";
			}
			str += "!";
			if (!flag2)
			{
				MenuManager.Instance.ShowMessage(str);
			}
			else
			{
				MenuManager.Instance.HideMessage();
				MenuManager.Instance.StopStoreCallbackTimer();
				MenuManager.Instance.BuyVehicle(Currency.Cash, iapPurchase: true);
			}
			GameState.AddCurrency(num, Currency.Gold);
			if (!statsData.IsMember && flag)
			{
				GameState.SetMembership(isMember: true);
			}
			MenuManager.Instance.UpdateScreen();
		}
		else
		{
			UnityEngine.Debug.Log("Product was null!");
		}
		return PurchaseProcessingResult.Complete;
	}

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        throw new System.NotImplementedException();
    }
}
