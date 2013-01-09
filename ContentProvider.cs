using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;

//using cloudmusic2upnp.IContentProvider;

namespace cloudmusic2upnp
{
	class ContentProvider
	{

		private static Dictionary<string, object> allPlugins;
		public static Dictionary<string, object> All {
			get {
				if (allPlugins == null) {

					allPlugins = LoadPlugins();

				}

				return allPlugins;
			}
		}

		public ContentProvider ()
		{

		}

		private static Dictionary<string, object> LoadPlugins ()
		{
			String[] files = Directory.GetFiles (".");
			Type pTypeInterface = typeof(IContentProvider.IContentProvider);

			Dictionary<string, object> interfaceinstances = new Dictionary<string, object> ();

			foreach (String file in files) {
				FileInfo fileInfo = new FileInfo (file);

				if (fileInfo.Extension.Equals (".dll")) {

					Assembly assembly = Assembly.LoadFrom (file);

					foreach (Type type in assembly.GetTypes()) {
						if (type.IsPublic && !type.IsAbstract) {
							Type typeInterface = type.GetInterface (pTypeInterface.ToString (), true);

							if (typeInterface != null) {
								try {
									object activedInstance = Activator.CreateInstance (type);
									if (activedInstance != null) {
										interfaceinstances.Add (type.Name, activedInstance);
									}
								} catch (Exception exception) {
									System.Diagnostics.Debug.WriteLine (exception);
								}
							}
							typeInterface = null;
						}
					}
				}
			}

			return interfaceinstances;
		}
	}
}

