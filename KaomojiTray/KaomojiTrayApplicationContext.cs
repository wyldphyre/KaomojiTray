using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using KaomojiTray.Properties;

namespace KaomojiTray
{
    internal class KaomojiTrayApplicationContext : SystemTrayApplicationContext
    {
        public KaomojiTrayApplicationContext()
        {
            SetIconImage(Resources.MainIcon32x32);

            var assembly = System.Reflection.Assembly.GetExecutingAssembly();

            var serializer = new DataContractJsonSerializer(typeof(KaomojiLibrary));
            Library = serializer.ReadObject(
                assembly.GetManifestResourceStream(assembly.GetName().Name + ".kaomoji.json")) as KaomojiLibrary;

            IconClickEvent += KaomojiTrayApplicationContext_IconClickEvent;

            AddMenuItem("Exit", null, ExitMenuItemClick);
        }

        public KaomojiLibrary Library { get; }

        private void KaomojiTrayApplicationContext_IconClickEvent()
        {
            ShowPicker();
        }

        private void ShowPicker()
        {
            var window = new MainWindow();
            window.Title = "Kaomoji Library";
            window.LoadLibrary(Library);
            window.Show();
        }

        private void ExitMenuItemClick(object sender, EventArgs e)
        {
            ExitApplication();
        }
    }

    [DataContract]
    public class KaomojiLibrary
    {
        [DataMember] public List<KaomojiCategory> category { get; set; }
    }

    [DataContract]
    public class KaomojiCategory
    {
        [DataMember] public string id { get; set; }

        [DataMember] public List<KaomojiSection> sections { get; set; }
    }

    [DataContract]
    public class KaomojiSection
    {
        [DataMember] public string id { get; set; }

        [DataMember] public List<string> kaomoji { get; set; }
    }
}