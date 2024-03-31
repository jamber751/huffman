namespace Algo9;
using System.Collections.ObjectModel;

public partial class MainPage : ContentPage
{
    public class Node
    {
        public int Freq { get; set; }
        public Node Parent { get; set; }
        public Node Left { get; set; }
        public Node Right { get; set; }

        public Node(int freq)
        {
            Freq = freq;
            Parent = null;
            Left = null;
            Right = null;
        }
    }

    public class Symbol : Node
    {
        public Char Char { get; set; }
        public string Code { get; set; }

        public Symbol(Char character, int freq) : base(freq)
        {
            Char = character;
            Code = String.Empty;
        }
    }

    public class Huffman
    {
        public ObservableCollection<Symbol> Symbols { get; set; }
        public Node Root { get; set; } = null;
        public Dictionary<Char, String> Table { get; set; }

        public Huffman(ObservableCollection<Symbol> symbols)
        {
            if (symbols.Count == 0) return;
            Symbols = new ObservableCollection<Symbol>(symbols.OrderBy(i => i.Freq));
            CreateTree();
            CreateTable();
        }

        public string Encode(string text)
        {
            string code = string.Empty;
            foreach (Char c in text)
            {
                code += Table[c];
            }
            return code;
        }

        public string Decode(string code)
        {
            string text = string.Empty;
            Node currentNode = Root;
            for (int i = 0; i < code.Length; i++)
            {
                int number = int.Parse(code[i].ToString());
                if (number == 0) currentNode = currentNode.Left;
                else currentNode = currentNode.Right;

                if (currentNode.GetType().Equals(typeof(Symbol)))
                {
                    Symbol symbol = (Symbol)currentNode;
                    text += symbol.Char;
                    currentNode = Root;
                }
            }
            return text;
        }

        private void CreateTree()
        {
            LinkedList<Node> list = new LinkedList<Node>(Symbols);

            while (list.Count != 1)
            {
                Node node1 = list.First();
                list.RemoveFirst();
                Node node2 = list.First();
                list.RemoveFirst();

                int newFreq = node1.Freq + node2.Freq;
                Node parentNode = new Node(newFreq);
                node1.Parent = parentNode;
                node2.Parent = parentNode;

                if (node1.Freq < node2.Freq)
                {
                    parentNode.Left = node1;
                    parentNode.Right = node2;
                }
                else
                {
                    parentNode.Left = node2;
                    parentNode.Right = node1;
                }

                if (list.Count > 0)
                {
                    LinkedListNode<Node> currentNode = list.First;
                    while (currentNode.Next != null && currentNode.Next.Value.Freq < newFreq)
                    {
                        currentNode = currentNode.Next;
                    }
                    list.AddAfter(currentNode, parentNode);
                }
                else list.AddFirst(parentNode);

            }
            Root = list.First();
        }

        private void CreateTable()
        {
            Table = new Dictionary<char, string>();
            foreach (Symbol symbol in Symbols)
            {
                string code = string.Empty;
                Node currentNode = symbol;
                while (currentNode != Root)
                {
                    Node parent = currentNode.Parent;
                    if (currentNode == parent.Left) code = '0' + code;
                    else code = '1' + code;
                    currentNode = parent;
                }
                symbol.Code = code;
                Table.Add(symbol.Char, code);
            }
        }
    }

    ObservableCollection<Symbol> symbols = new ObservableCollection<Symbol>();
    Huffman huffman;

    void addSymbol_Clicked(System.Object sender, System.EventArgs e)
    {
        foreach (Symbol symbol in symbols) symbol.Code = string.Empty;
        symbolListView.ItemsSource = symbols;
        symbols.Add(new Symbol(char.Parse(symbolEntry.Text), int.Parse(freqEntry.Text)));
        symbolListView.ScrollTo(symbols[symbols.Count - 1], 0, true);
        symbolEntry.Text = string.Empty;
        freqEntry.Text = string.Empty;
    }

    void symbolListView_ItemTapped(System.Object sender, Microsoft.Maui.Controls.ItemTappedEventArgs e)
    {
        foreach (Symbol symbol in symbols) symbol.Code = string.Empty;
        symbolListView.ItemsSource = symbols;
        symbols.RemoveAt(e.ItemIndex);
    }

    void confirmSymbols_Clicked(System.Object sender, System.EventArgs e)
    {
        huffman = new Huffman(symbols);
        symbolListView.ItemsSource = huffman.Symbols;
    }

    public MainPage()
    {
        InitializeComponent();
        symbolListView.ItemsSource = symbols;
    }

    void codingAction_Clicked(System.Object sender, System.EventArgs e)
    {
        if (decodeSwitch.IsToggled)
        {
            textEditor.Text = huffman.Decode(codeEditor.Text);
        }
        else
        {
            codeEditor.Text = huffman.Encode(textEditor.Text);
        }
    }
}