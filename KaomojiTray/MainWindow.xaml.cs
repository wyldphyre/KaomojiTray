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
    private Brush BackgrorundBrush { get; } = Brushes.Black;
    private Brush ForegroundBrush { get; } = Brushes.White;

    public KaomojiLibrary Library { get; private set; }

    public MainWindow()
    {
      InitializeComponent();
    }

    public void LoadLibrary(KaomojiLibrary Library)
    {
      this.Library = Library;

      MainGrid.Background = BackgrorundBrush;

      foreach (var Category in Library.category)
      {
        var CategoryCaption = new TextBlock()
        {
          Text = Category.id.ToUpper(),
          Padding = new Thickness(5),
          FontFamily = new FontFamily("Segoe UI"),
          FontSize = 16,
          HorizontalAlignment = HorizontalAlignment.Center,
          Background = BackgrorundBrush,
          Foreground = ForegroundBrush
        };
        
        var CategoryBorder = new Border();
        CategoryBorder.Child = CategoryCaption;
        CategoryBorder.HorizontalAlignment = HorizontalAlignment.Stretch;
        CategoryBorder.Background = BackgrorundBrush;
        CategoryBorder.Margin = new Thickness(5);
        CategoryBorder.MouseUp += (sender, e) =>
        {
          ShowCategory(Category);
          SectionScrollViewer.ScrollToTop();
        };
        CategoryBorder.MouseEnter += (sender, e) =>
        {
          CategoryCaption.TextDecorations = TextDecorations.Underline;
          CategoryCaption.Background = ForegroundBrush;
          CategoryCaption.Foreground = BackgrorundBrush;
        };
        CategoryBorder.MouseLeave += (sender, e) =>
        {
          CategoryCaption.TextDecorations = null;
          CategoryCaption.Background = BackgrorundBrush;
          CategoryCaption.Foreground = ForegroundBrush;
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
          TextAlignment = TextAlignment.Center,
          TextDecorations = TextDecorations.Underline,
          Background = BackgrorundBrush,
          Foreground = ForegroundBrush
        };
        SectionDock.Children.Add(Heading);
        DockPanel.SetDock(Heading, Dock.Top);

        var KaomojiStack = new WrapPanel();
        SectionDock.Children.Add(KaomojiStack);

        foreach (var Kaomoji in Section.kaomoji)
        {
          var KaomojiTextBlock = new TextBlock()
          {
            Text = Kaomoji,
            FontFamily = new FontFamily("Arial Unicode MS"),
            FontSize = 15,
            Margin = new Thickness(5),
            Padding = new Thickness(7),
            Foreground = ForegroundBrush
          };
          KaomojiTextBlock.MouseEnter += (sender, e) =>
          {
            KaomojiTextBlock.Background = ForegroundBrush;
            KaomojiTextBlock.Foreground = BackgrorundBrush;
          };
          KaomojiTextBlock.MouseLeave += (sender, e) =>
          {
            KaomojiTextBlock.Background = BackgrorundBrush;
            KaomojiTextBlock.Foreground = ForegroundBrush;
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