using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;

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
      Window.LoadLibrary(Library);
      Window.Show();
    }

    private void ExitMenuItemClick(object sender, EventArgs e)
    {
      ExitApplication();
    }
  }

  [DataContract]
  public class KaomojiLibrary
  {
    [DataMember]
    public List<KaomojiCategory> category { get; set; }
  }

  [DataContract]
  public class KaomojiCategory
  {
    [DataMember]
    public string id { get; set; }
    [DataMember]
    public List<KaomojiSection> sections { get; set; }
  }

  [DataContract]
  public class KaomojiSection
  {
    [DataMember]
    public string id { get; set; }
    [DataMember]
    public List<string> kaomoji { get; set; }
  }
}
