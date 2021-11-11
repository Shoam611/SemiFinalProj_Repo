using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tWpfMashUp_v0._0._1.MVVM.ViewModels;

namespace tWpfMashUp_v0._0._1.Assets.Components.CustomModal
{
    public static class Modal
    {
        private static string value;
        private static TaskCompletionSource<string> chooseOpt;
        //caption and title optional
        public static void ShowModal(string caption, string title = " ")
        {
            var view = new ModalView();
            var loc = App.Current.Resources["Locator"] as ViewModelLocator;
            loc.Main.Modal = view;
            view.ModalLoaded += (out string Title, out string Caption) =>
              {
                  Caption = caption;
                  Title = title;
              };
            view.ModalClosing += (s, e) => loc.Main.Modal = null;
            view.Init();
        }
        //caption title and two buttons
        public static string ShowModal(string caption, string title, string Button1, string Button2)
        {
            var view = new ModalView();
            var loc = App.Current.Resources["Locator"] as ViewModelLocator;
             loc.Main.Modal = view;
            view.ModalClosing += OnClosingHandler;
            view.ModalLoadedWithButtons += (out string[] Vals, out string Title, out string Caption) =>
            {
                Caption = caption;
                Title = title;
                Vals = new string[] { Button1, Button2 };
            };
            view.ModalClosing += (s, e) => { loc.Main.Modal = null; };
            view.ModalClosing += OnClosingHandler;
            view.Init();
            //showDialog()
            return value;
        }
        //caption title and three buttons
        public static string ShowModal(string caption, string title, string Button1, string Button2, string Button3)
        {
            PopupWindow popupWindow = new PopupWindow();
            popupWindow.ModalClosing += OnClosingHandler; // fetch back data
            popupWindow.ModalLoadedWithButtons += (out string[] Vals, out string Title, out string Caption) =>
            {
                Caption = caption;
                Title = title;
                Vals = new string[] { Button1, Button2, Button3 };
            };
            popupWindow.ShowDialog();
            return value;
        }

        private static void OnClosingHandler(object sender, EventArgs e)
        {
            var args = e as ModalClosingEventArgs;
            if (args != null)
                value = args.ValueSelected;
        }
    }
}
