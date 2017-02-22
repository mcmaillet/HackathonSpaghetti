package md5a49f02afadc3a80d8926e5c763f27ea6;


public class Service1Binder
	extends android.os.Binder
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("HellowApp6.Service1Binder, HellowApp6, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", Service1Binder.class, __md_methods);
	}


	public Service1Binder () throws java.lang.Throwable
	{
		super ();
		if (getClass () == Service1Binder.class)
			mono.android.TypeManager.Activate ("HellowApp6.Service1Binder, HellowApp6, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	public Service1Binder (com.xamarin.test.testname p0) throws java.lang.Throwable
	{
		super ();
		if (getClass () == Service1Binder.class)
			mono.android.TypeManager.Activate ("HellowApp6.Service1Binder, HellowApp6, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "HellowApp6.Service1, HellowApp6, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", this, new java.lang.Object[] { p0 });
	}

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
