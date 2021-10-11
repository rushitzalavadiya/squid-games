using System.Collections.Generic;

public abstract class PackageConfig
{
	public enum Platform
	{
		ANDROID,
		IOS
	}

	public abstract string Name
	{
		get;
	}

	public abstract string Version
	{
		get;
	}

	public abstract Dictionary<Platform, string> NetworkSdkVersions
	{
		get;
	}

	public abstract Dictionary<Platform, string> AdapterClassNames
	{
		get;
	}

	public string NetworkSdkVersion => NetworkSdkVersions[Platform.ANDROID];

	public string AdapterClassName => AdapterClassNames[Platform.ANDROID];
}
