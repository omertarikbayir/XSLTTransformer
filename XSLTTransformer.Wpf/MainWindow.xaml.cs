using System.Windows;
using XSLTTransformer.Core;

namespace XSLTTransformer.Wpf;

public partial class MainWindow : Window
{
    private readonly XsltEngine _engine = new();
    private bool _isTr = true;

    public MainWindow()
    {
        InitializeComponent();
        SetTurkish(null, null);
    }

    private void SetEnglish(object s, RoutedEventArgs e)
    {
        _isTr = false;
        XmlInput.Text = "Paste XML here";
        XsltInput.Text = "Paste XSLT here";
        StatusText.Text = "Ready";
    }

    private void SetTurkish(object s, RoutedEventArgs e)
    {
        _isTr = true;
        XmlInput.Text = "XML buraya yapıştırın";
        XsltInput.Text = "XSLT buraya yapıştırın";
        StatusText.Text = "Hazır";
    }

    private void OnTransform(object s, RoutedEventArgs e)
    {
        try
        {
            var output = _engine.Transform(XmlInput.Text, XsltInput.Text);
            ResultOutput.Text = output;
            StatusText.Text = _isTr ? "Başarılı" : "Success";
        }
        catch (System.Exception ex)
        {
            ResultOutput.Text = ex.Message;
            StatusText.Text = _isTr ? "Hata" : "Error";
        }
    }
}