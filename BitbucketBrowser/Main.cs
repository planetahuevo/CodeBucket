using System;
using System.Collections.Generic;
using System.Linq;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

using BitbucketBrowser.UI;
using MonoTouch.Dialog;
using MonoTouch.SlideoutNavigation;
using System.Drawing;
using System.Threading;

namespace BitbucketBrowser
{
	
	// The UIApplicationDelegate for the application. This class is responsible for launching the 
	// User Interface of the application, as well as listening (and optionally responding) to 
	// application events from iOS.
	[Register ("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
	{
		// class-level declarations
		UIWindow window;
        SlideoutNavigationController _nav;

		// This is the main entry point of the application.
		static void Main(string[] args)
		{
			// if you want to use a different Application Delegate class from "AppDelegate"
			// you can specify it here.
			UIApplication.Main(args, null, "AppDelegate");
		}
		
		//
		// This method is invoked when the application has loaded and is ready to run. In this 
		// method you should instantiate the window, load the UI into it and then make the window
		// visible.
		//
		// You have 17 seconds to return from this method, or iOS will terminate your application.
		//
		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{

            UIApplication.SharedApplication.StatusBarStyle = UIStatusBarStyle.BlackTranslucent;

            //Set the theming
            UINavigationBar.Appearance.SetBackgroundImage(Images.Titlebar, UIBarMetrics.Default);
            UIBarButtonItem.Appearance.SetBackgroundImage(Images.BarButton.CreateResizableImage(new UIEdgeInsets(15, 6, 15, 6)), UIControlState.Normal, UIBarMetrics.Default);
            UIBarButtonItem.Appearance.SetBackButtonBackgroundImage(Images.BackButton.CreateResizableImage(new UIEdgeInsets(0, 14, 0, 5)), UIControlState.Normal, UIBarMetrics.Default);
            UISegmentedControl.Appearance.SetBackgroundImage(Images.BarButton.CreateResizableImage(new UIEdgeInsets(15, 6, 14, 6)), UIControlState.Normal, UIBarMetrics.Default);
            UISearchBar.Appearance.BackgroundImage = Images.Searchbar;


            Application.Client = new BitbucketSharp.Client("thedillonb", "djames");

            window = new UIWindow(UIScreen.MainScreen.Bounds);
            _nav = new SlideoutNavigationController();

            _nav.MenuView = new MenuController();
            _nav.TopView = new AccountRepositoryController("thedillonb");
                //new ChangesetInfoController("thedillonb", "bitbucketsharp", "e9d8cf73c610"); //new EventsController("thedillonb", false) { Title = "Events", ReportUser = false };



            window.RootViewController = _nav;
			window.MakeKeyAndVisible();
			
			return true;
		}
	}

    /// <summary>
    /// Application.
    /// </summary>
    public static class Application
    {
        public static BitbucketSharp.Client Client { get; set; }
    }

    public class MenuController : DialogViewController
    {
        public MenuController()
            : base(UITableViewStyle.Plain, new RootElement("BitbucketBrowser"))
        {
        }

        class NavElement : StyledElement
        {
            public NavElement(string title, NSAction tapped, UIImage image)
                : base(title, tapped, image)
            {
                BackgroundColor = UIColor.Clear;
                TextColor = UIColor.White;
                DetailColor = UIColor.White;
            }


            public override UITableViewCell GetCell(UITableView tv)
            {
                var cell = base.GetCell(tv);
                cell.SelectedBackgroundView = new UIView() { BackgroundColor = UIColor.FromRGBA(41, 41, 41, 200) };

                var f = cell.Subviews.Count(x => x.Tag == 1111);
                if (f == 0)
                {

                    var v2 = new UIView(new RectangleF(0, cell.Frame.Height - 3, cell.Frame.Width, 1));
                    v2.BackgroundColor = UIColor.FromRGBA(41, 41, 41, 64);
                    v2.Tag = 1111;
                    cell.AddSubview(v2);


                    var v = new UIView(new RectangleF(0, cell.Frame.Height - 2, cell.Frame.Width, 1));
                    v.BackgroundColor = UIColor.FromRGBA(41, 41, 41, 200);
                    v.Tag = 1111;
                    cell.AddSubview(v);
                }

                return cell;
            }

        }

        private void DoShit(UIViewController controller)
        {
            NavigationController.PushViewController(controller, false);

        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            NavigationController.SetNavigationBarHidden(true, false);

            Root.Add(new Section() {
                new NavElement("Profile", () => DoShit(new ProfileController("thedillonb", false) { Title = "Profile" }), Images.Person),
                new NavElement("Events", () => DoShit(new EventsController("thedillonb", false) { Title = "Events", ReportUser = false, ReportRepository = true }), Images.Event),
                new NavElement("Repositories", () => DoShit(new AccountRepositoryController("thedillonb") { Title = "Repositories" }), Images.Repo),
                new NavElement("Groups", () => DoShit(new GroupController("thedillonb", false) { Title = "Groups" }), Images.Group),
                new NavElement("Explore", () => DoShit(new ExploreController() { Title = "Explore" }), UIImage.FromBundle("/Images/Tabs/search")),
            });

            /*
            Root.Add(new Section("Settings") {
                new NavElement("Login", () => { 
                    PresentModalViewController(new LoginViewController(), true);
                
                }, UIImage.FromBundle("/Images/Tabs/person"))
            });
            */

            TableView.BackgroundColor = UIColor.Clear;
            UIImage background = UIImage.FromBundle("/Images/Linen");
            View.BackgroundColor = UIColor.FromPatternImage(background);

            TableView.SeparatorColor = UIColor.FromRGBA(128, 128, 128, 128);


            /*
            Root.Add(new Section("Settings") {
                new CustomImageStringElement("Change User", () => NavigationController.PushViewController(new ProfileController("thedillonb", false) { Title = "Profile" }, false), UIImage.FromBundle("/Images/Tabs/person")),
            });
            */

            var view = new UIView(new RectangleF(0, 0, View.Bounds.Width, 10));
            view.BackgroundColor = UIColor.Clear;
            TableView.TableFooterView = view;
        }
    }
}

