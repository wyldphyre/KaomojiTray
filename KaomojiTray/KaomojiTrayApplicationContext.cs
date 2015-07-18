using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace KaomojiTray
{
  class KaomojiTrayApplicationContext : SystemTrayApplicationContext
  {
    public KaomojiLibrary Library { get; private set; }

    public KaomojiTrayApplicationContext() : base()
    {
      SetIconImage(KaomojiTray.Properties.Resources.Close_16);

      var Assembly = System.Reflection.Assembly.GetExecutingAssembly();

      using (var StreamReader = new StreamReader(Assembly.GetManifestResourceStream(Assembly.GetName().Name + ".kaomoji.json")))
      {
        this.Library = Newtonsoft.Json.JsonConvert.DeserializeObject<KaomojiLibrary>(StreamReader.ReadToEnd());
      }

      this.IconClickEvent += KaomojiTrayApplicationContext_IconClickEvent;
      
      AddMenuItem("Exit", null, ExitMenuItemClick);
    }

    void KaomojiTrayApplicationContext_IconClickEvent()
    {
      ShowPicker();
    }

    private void ShowPicker()
    {
      var Window = new MainWindow();
      Window.Title = "Kaomoji Library";
      Window.Show();
      //var Popup = new Popup();
      ////Popup.Parent = this.notifyIcon as FrameworkElement;

      //var Dock = new DockPanel();
      //Popup.Child = Dock;
      //Dock.Height = 400;
      //Dock.Width = 600;

      //var CategoryStack = new StackPanel();
      ////CategoryStack.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
      //Dock.Children.Add(CategoryStack);
      //DockPanel.SetDock(CategoryStack, System.Windows.Controls.Dock.Left);
      //Dock.LastChildFill = true;

      //var SectionStack = new StackPanel();
      //Dock.Children.Add(SectionStack);

      //foreach (var Category in Library.category)
      //{
      //  var CategoryCaption = new TextBlock() { Text = Category.id };
      //  CategoryStack.Children.Add(CategoryCaption);
      //}

      //Popup.HasDropShadow = true;
      //Popup.
    }

    private void ExitMenuItemClick(object sender, EventArgs e)
    {
      ExitApplication();
    }
  }

  class KaomojiLibrary
  {
    public List<KaomojiCategory> category { get; set; }
  }

  class KaomojiCategory
  {
    public string id { get; set; }
    public List<KaomojiSection> sections { get; set; }
  }

  class KaomojiSection
  {
    public string id { get; set; }
    public List<string> kaomoji { get; set; }
  }
}
