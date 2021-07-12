using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace KaomojiTray
{
    internal class SystemTrayApplicationContext : ApplicationContext
    {
        private readonly Container components;

        public SystemTrayApplicationContext()
        {
            components = new Container();
            notifyIcon = new NotifyIcon(components)
            {
                ContextMenuStrip = new ContextMenuStrip(),
                Text = Application.ProductName,
                Visible = true
            };
            notifyIcon.ContextMenuStrip.Opening += ContextMenuStrip_Opening;
            notifyIcon.DoubleClick += NotifyIcon_DoubleClick;
            notifyIcon.MouseUp += NotifyIcon_MouseUp;
        }

        public NotifyIcon notifyIcon { get; }

        public string IconText
        {
            get => notifyIcon.Text;
            set => notifyIcon.Text = value;
        }

        public Icon IconImage => notifyIcon.Icon;

        public event Action IconClickEvent;
        public event Action IconDoubleClickEvent;
        public event CancelEventHandler ContextMenuOpeningEvent;

        public void SetIconImage(Bitmap image)
        {
            notifyIcon.Icon = Icon.FromHandle(image.GetHicon());
        }

        protected ToolStripItem AddMenuItem(string text, Image image = null, EventHandler clickHandler = null)
        {
            return notifyIcon.ContextMenuStrip.Items.Add(text, image, clickHandler);
        }

        protected void AddMenuSeparator()
        {
            notifyIcon.ContextMenuStrip.Items.Add(new ToolStripSeparator());
        }

        protected void ExitApplication()
        {
            ExitThread();
        }

        protected void SetIcon(Icon image)
        {
            notifyIcon.Icon = image;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
        }

        protected override void ExitThreadCore()
        {
            //if (mainForm != null) { mainForm.Close(); }
            notifyIcon.Visible = false; // should remove lingering tray icon!
            base.ExitThreadCore();
        }

        private void NotifyIcon_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && IconClickEvent != null)
                IconClickEvent();
        }

        private void NotifyIcon_DoubleClick(object sender, EventArgs e)
        {
            IconDoubleClickEvent?.Invoke();
        }

        private void ContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuOpeningEvent?.Invoke(sender, e);
        }
    }
}