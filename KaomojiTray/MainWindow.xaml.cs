using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace KaomojiTray
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
  {
    private bool IsClosing = false;
    public  KaomojiLibrary Library { get; private set; }

    public MainWindow()
    {
      InitializeComponent();
    }

    public void LoadLibrary(KaomojiLibrary Library)
    {
      this.Library = Library;

      foreach (var Category in Library.category)
      {
        var CategoryCaption = new TextBlock()
        {
          Text = Category.id.ToUpper(),
          Foreground = Brushes.White,
          Padding = new Thickness(5),
          FontFamily = new FontFamily("Segoe UI"),
          FontSize = 16,
          HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
          Width = 110,
        };

        var CategoryBorder = new Border();
        CategoryBorder.Child = CategoryCaption;
        CategoryBorder.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
        CategoryBorder.BorderBrush = Brushes.Gray;
        CategoryBorder.Background = Brushes.SteelBlue;
        CategoryBorder.Margin = new Thickness(5);
        CategoryBorder.MouseUp += (sender, e) =>
        {
          ShowCategory(Category);
          SectionScrollViewer.ScrollToTop();
        };

        CategoryStackPanel.Children.Add(CategoryBorder);
      }

      ShowCategory(Library.category.First());
    }

    private void ShowCategory(KaomojiCategory Category)
    {
      SectionStackPanel.Children.Clear();

      foreach (var Section in Category.sections)
      {
        var SectionDock = new DockPanel();
        SectionDock.LastChildFill = true;
        SectionDock.Margin = new Thickness(3, 3, 3, 5);
        SectionStackPanel.Children.Add(SectionDock);

        var Heading = new TextBlock()
        { 
          Text = Section.id.ToUpper(), 
          FontSize = 18, 
          FontWeight = FontWeights.Bold,
          TextAlignment = TextAlignment.Center
        };
        SectionDock.Children.Add(Heading);
        DockPanel.SetDock(Heading, Dock.Top);

        var KaomojiStack = new WrapPanel();
        SectionDock.Children.Add(KaomojiStack); 

        foreach (var Kaomoji in Section.kaomoji)
        {
          var KaomojiTextBlock = new TextBlock() { Text = Kaomoji, FontSize = 14 };
          KaomojiTextBlock.Margin = new Thickness(5);
          KaomojiTextBlock.Padding = new Thickness(7);
          KaomojiTextBlock.MouseEnter += (sender, e) =>
          {
            KaomojiTextBlock.Background = Brushes.SteelBlue;
            KaomojiTextBlock.Foreground = Brushes.White;
          };
          KaomojiTextBlock.MouseLeave += (sender, e) =>
          {
            KaomojiTextBlock.Background = Brushes.White;
            KaomojiTextBlock.Foreground = Brushes.Black;
          };
          KaomojiTextBlock.MouseUp += (sender, e) =>
          {
            Clipboard.SetText(Kaomoji);
            Close();
          };

          KaomojiStack.Children.Add(KaomojiTextBlock);
        }
      }
    }

    private void Window_Deactivated(object sender, EventArgs e)
    {
      if (!IsClosing)
        Close();
    }

    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
      IsClosing = true;
    }
  }
}