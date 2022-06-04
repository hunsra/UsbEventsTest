using System.Diagnostics;
using AppKit;
using Foundation;
using Usb.Events;

namespace UsbEventsTest
{
	[Register ("AppDelegate")]
	public class AppDelegate : NSApplicationDelegate
	{
		IUsbEventWatcher _watcher;

		public AppDelegate ()
		{
		}

		public override void DidFinishLaunching (NSNotification notification)
		{
			_watcher = new UsbEventWatcher();
            _watcher.UsbDeviceRemoved += _watcher_UsbDeviceRemoved;
            _watcher.UsbDeviceAdded += _watcher_UsbDeviceAdded;
            _watcher.UsbDriveEjected += _watcher_UsbDriveEjected;
            _watcher.UsbDriveMounted += _watcher_UsbDriveMounted;

            var list = _watcher.UsbDeviceList;
            foreach (var device in list)
            {
                Debug.WriteLine($"Device: {device.DeviceName}");
            }
		}

        private void _watcher_UsbDriveMounted(object sender, string e)
        {
            Debug.WriteLine($"Mounted: {e}");
        }

        private void _watcher_UsbDriveEjected(object sender, string e)
        {
            Debug.WriteLine($"Ejected: {e}");
        }

        private void _watcher_UsbDeviceAdded(object sender, UsbDevice e)
        {
            Debug.WriteLine($"Added: {e.DeviceName}");
        }

        private void _watcher_UsbDeviceRemoved(object sender, UsbDevice e)
        {
            Debug.WriteLine($"Removed: {e.DeviceName}");
        }

        public override void WillTerminate (NSNotification notification)
		{
            _watcher.UsbDeviceRemoved -= _watcher_UsbDeviceRemoved;
            _watcher.UsbDeviceAdded -= _watcher_UsbDeviceAdded;
            _watcher.UsbDriveEjected -= _watcher_UsbDriveEjected;
            _watcher.UsbDriveMounted -= _watcher_UsbDriveMounted;
            _watcher.Dispose();
            _watcher = null;
        }
    }
}

