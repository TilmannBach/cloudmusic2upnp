using System;

namespace cloudmusic2upnp.DeviceController
{
	/// <summary>
	/// Interface for DeviceControllers, like a UPnP Control Point.
	/// </summary>
	public interface IController
	{
		/// <summary>
		/// Gets a list with all avalible devices (eg UPnP Renderers).
		/// </summary>
		IDevice[] GetDevices();
	}

	public interface IDevice
	{
		/// <summary>
		/// Play the currently selected media.
		/// </summary>
		void Play();

		/// <summary>
		/// Pause the currently playing media.
		/// </summary>
		void Pause();

		/// <summary>
		/// Stop the currently playing media.
		/// </summary>
		void Stop();

		/// <summary>
		/// Sets the media ressource to a specific URL.
		/// </summary>
		/// <param name='url'>
		/// The URL, that should be played.
		/// </param>
		void SetMediaUrl(String url);
	}
}

