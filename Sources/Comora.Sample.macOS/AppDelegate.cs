namespace Comora.Sample.macOS
{
	using AppKit;
	using Foundation;

	[Register("AppDelegate")]
	class Program : NSApplicationDelegate
	{
		private static Game1 game;

		internal static void RunGame()
		{
			game = new Game1();
			game.Window.AllowUserResizing = true;
			game.Run();
		}

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		static void Main(string[] args)
		{
			NSApplication.Init();

			using (var p = new NSAutoreleasePool())
			{
				NSApplication.SharedApplication.Delegate = new Program();
				NSApplication.Main(args);
			}
		}

		public override void DidFinishLaunching(NSNotification notification)
		{
			RunGame();
		}

		public override bool ApplicationShouldTerminateAfterLastWindowClosed(NSApplication sender)
		{
			return true;
		}
	}
}
