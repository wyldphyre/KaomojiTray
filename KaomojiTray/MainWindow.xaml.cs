using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace KaomojiTray
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private bool isClosing;

        public MainWindow()
        {
            InitializeComponent();
        }

        private Brush BackgroundBrush { get; } = Brushes.WhiteSmoke;

        private Brush ForegroundBrush { get; } = Brushes.Black;

        public KaomojiLibrary Library { get; private set; }

        public void LoadLibrary(KaomojiLibrary library)
        {
            Library = library;

            MainGrid.Background = BackgroundBrush;

            foreach (var category in Library.category)
            {
                var categoryCaption = new TextBlock
                {
                    Text = category.id.ToUpper(),
                    Padding = new Thickness(5),
                    FontFamily = new FontFamily("Segoe UI"),
                    FontSize = 16,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Background = BackgroundBrush,
                    Foreground = ForegroundBrush
                };

                var categoryBorder = new Border
                {
                    Child = categoryCaption,
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    Background = BackgroundBrush,
                    Margin = new Thickness(5)
                };
                categoryBorder.MouseUp += (sender, e) =>
                {
                    ShowCategory(category);
                    SectionScrollViewer.ScrollToTop();
                };
                categoryBorder.MouseEnter += (sender, e) =>
                {
                    categoryCaption.TextDecorations = TextDecorations.Underline;
                    categoryCaption.Background = ForegroundBrush;
                    categoryCaption.Foreground = BackgroundBrush;
                };
                categoryBorder.MouseLeave += (sender, e) =>
                {
                    categoryCaption.TextDecorations = null;
                    categoryCaption.Background = BackgroundBrush;
                    categoryCaption.Foreground = ForegroundBrush;
                };

                CategoryStackPanel.Children.Add(categoryBorder);
            }

            ShowCategory(Library.category.First());
        }

        private void ShowCategory(KaomojiCategory Category)
        {
            SectionStackPanel.Children.Clear();

            foreach (var section in Category.sections)
            {
                var sectionDock = new DockPanel();
                sectionDock.LastChildFill = true;
                sectionDock.Margin = new Thickness(3, 3, 3, 5);
                SectionStackPanel.Children.Add(sectionDock);

                var heading = new TextBlock
                {
                    Text = section.id.ToUpper(),
                    FontSize = 18,
                    FontWeight = FontWeights.Bold,
                    TextAlignment = TextAlignment.Center,
                    TextDecorations = TextDecorations.Underline,
                    Background = BackgroundBrush,
                    Foreground = ForegroundBrush
                };
                sectionDock.Children.Add(heading);
                DockPanel.SetDock(heading, Dock.Top);

                var kaomojiStack = new WrapPanel();
                sectionDock.Children.Add(kaomojiStack);

                foreach (var kaomoji in section.kaomoji)
                {
                    var kaomojiTextBlock = new TextBlock
                    {
                        Text = kaomoji,
                        FontFamily = new FontFamily("Arial Unicode MS"),
                        FontSize = 15,
                        Margin = new Thickness(5),
                        Padding = new Thickness(7),
                        Foreground = ForegroundBrush
                    };
                    kaomojiTextBlock.MouseEnter += (sender, e) =>
                    {
                        kaomojiTextBlock.Background = ForegroundBrush;
                        kaomojiTextBlock.Foreground = BackgroundBrush;
                    };
                    kaomojiTextBlock.MouseLeave += (sender, e) =>
                    {
                        kaomojiTextBlock.Background = BackgroundBrush;
                        kaomojiTextBlock.Foreground = ForegroundBrush;
                    };
                    kaomojiTextBlock.MouseUp += (sender, e) =>
                    {
                        Clipboard.SetText(kaomoji);
                        Close();
                    };

                    kaomojiStack.Children.Add(kaomojiTextBlock);
                }
            }
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            if (!isClosing)
                Close();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            isClosing = true;
        }
    }
}