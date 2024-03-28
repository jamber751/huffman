namespace Algo9;

public partial class MainPage : ContentPage
{

    public class Symbol
    {
        public Char Char { get; set; }
        public int Freq { get; set; }
    }
    List<Symbol> symbols;

    public MainPage()
    {
        InitializeComponent();
        symbols = new List<Symbol>()
        {
            new Symbol(){ Char = 'a', Freq= 1},
            new Symbol(){ Char = 'b', Freq= 2},
            new Symbol(){ Char = 'c', Freq= 10},
            new Symbol(){ Char = 'd', Freq= 3},
            new Symbol(){ Char = 'e', Freq= 5},
            new Symbol(){ Char = 'f', Freq= 8}
        };
        symbolListView.ItemsSource = symbols;
    }

    void addSymbol_Clicked(System.Object sender, System.EventArgs e)
    {
        symbols.Add(new Symbol() { Char = symbolEntry.Text[0], Freq = int.Parse(freqEntry.Text) });
        symbols = new List<Symbol>(symbols);
        symbolListView.ItemsSource = symbols;
        symbolListView.ScrollTo(symbols[symbols.Count - 1], 0, false);
    }

    void symbolListView_ItemTapped(System.Object sender, Microsoft.Maui.Controls.ItemTappedEventArgs e)
    {
        symbols.RemoveAt(e.ItemIndex);
        symbols = new List<Symbol>(symbols);
        symbolListView.ItemsSource = symbols;
    }

}
