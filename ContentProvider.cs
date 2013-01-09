using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using cloudmusic2upnp.IContentProvider;

//using cloudmusic2upnp.IContentProvider;

namespace cloudmusic2upnp
{
	class ContentProvider
	{
		/// <summary>
		/// The path to the Content Provider plugin DLLs.
		/// </summary>
		private const string PLUGIN_PATH = ".";

		/// <summary>
		/// The singleton instance for this class.
		/// </summary>
		private static ContentProvider instance;

		/// <summary>
		/// Gets the instance of <see cref="cloudmusic2upnp.ContentProvider"/>.
		/// </summary>
		public static ContentProvider Instance {
			get {
				if (instance == null) {
					instance = new ContentProvider ();
				}
				return instance;
			}
		}

		/// <summary>
		/// A dict of all providers with its name and class.
		/// </summary>
		public Dictionary<string, IContentProvider.IContentProvider> allPlugins { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="cloudmusic2upnp.ContentProvider"/> class.
		/// </summary>
		private ContentProvider ()
		{
			allPlugins = LoadPlugins ();
		}


		/// <summary>
		/// Search all files in the working directory for DLL files, that
		/// implements the <see cref="IContentProvider.IContentProvider"/> interface.
		/// </summary>
		/// <returns>
		/// A dict of all providers with its name and class.
		/// </returns>
		private Dictionary<string, IContentProvider.IContentProvider> LoadPlugins ()
		{
			String[] allFiles = Directory.GetFiles (PLUGIN_PATH);
			Type requiredInterface = typeof(IContentProvider.IContentProvider);
			var foundInterfaces = new Dictionary<string, IContentProvider.IContentProvider> ();

			foreach (String file in allFiles) {
				FileInfo fileInfo = new FileInfo (file);

				if (fileInfo.Extension.Equals (".dll")) {
					Assembly assembly = Assembly.LoadFrom (file);

					foreach (Type type in assembly.GetTypes()) {
						if (type.IsPublic && !type.IsAbstract) {
							Type typeInterface = type.GetInterface (requiredInterface.ToString (), true);

							if (typeInterface != null) {
								try {
									object activedInstance = Activator.CreateInstance (type);
									if (activedInstance != null) {
										foundInterfaces.Add (type.Name, (IContentProvider.IContentProvider)activedInstance);
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

			return foundInterfaces;
		}
	}
}

