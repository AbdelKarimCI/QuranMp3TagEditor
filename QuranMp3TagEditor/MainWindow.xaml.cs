using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Serialization;
using QuranMp3TagEditor.Models;
using QuranMp3TagEditor.ViewModels;
using File = TagLib.File;
using MessageBox = System.Windows.MessageBox;
using Path = System.IO.Path;
using Style = QuranMp3TagEditor.Models.Style;

namespace QuranMp3TagEditor
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                if (dialog.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;
                var main = ((MainViewModel)DataContext).Main;
                main.Path = dialog.SelectedPath;
                var mp3Files = Directory.GetFiles(main.Path, "*.mp3", SearchOption.TopDirectoryOnly);
                foreach (var mp3File in mp3Files)
                {
                    main.Files.Add(new MP3File(mp3File));
                }
            }
        }

        private void Apply_Click(object sender, RoutedEventArgs e)
        {
            var main = ((MainViewModel)DataContext).Main;
            if (main.FileNameChange && !(main.FileNameSuratNumber || main.FileNameSuratName))
            {
                MessageBox.Show("إذا أردت تغيير إسم الملف، فعليك إختيار رقم السورة أو اسمها أو كليهما.", "تنبيه", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (main.TitleChange && !(main.TitleSuratNumber || main.TitleSuratName))
            {
                MessageBox.Show("إذا أردت تغيير ال عنوان، فعليك إختيار رقم السورة أو اسمها أو كليهما.", "تنبيه", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            uint year = 0;
            if (main.YearChange)
            {
                try
                {
                    year = uint.Parse(main.Year);
                }
                catch (Exception exception)
                {
                    MessageBox.Show("السنة التي ادخلتها غير صحيحة.", "تنبيه", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }

            foreach (var file in main.Files)
            {
                var resultString = Regex.Match(main.RecorgnizeByFileName ? file.Name : file.File.Tag.Title, @"\d+").Value;
                if (resultString.Length > 0)
                {
                    var index = int.Parse(resultString);

                    if (main.TitleChange)
                    {
                        file.File.Tag.Title = "";
                        if (main.TitleSuratNumber) file.File.Tag.Title = index.ToString("000");
                        if (main.TitleSuratNumber && main.TitleSuratName) file.File.Tag.Title += " - ";
                        if (main.TitleSuratName) file.File.Tag.Title += main.TitleStyle.Values[index - 1];
                    }

                    if (main.ArtistChange)
                    {
                        file.File.Tag.AlbumArtists = new []{ main.Artist };
                    }

                    if (main.AlbumChange)
                    {
                        file.File.Tag.Album = main.Album;
                    }

                    if (main.YearChange)
                    {
                        file.File.Tag.Year = year;
                    }

                    file.File.Save();

                    if (main.FileNameChange)
                    {
                        var fileName = "";
                        if (main.TitleSuratNumber) fileName = index.ToString("000");
                        if (main.TitleSuratNumber && main.TitleSuratName) fileName += " - ";
                        if (main.TitleSuratName) fileName += main.TitleStyle.Values[index - 1];
                        fileName = Path.Combine(main.Path, $"{fileName}.mp3");

                        System.IO.File.Move(file.File.Name, fileName);
                        //file = new MP3File(fileName);
                    }
                }
            }

            MessageBox.Show("تمت المهمة بنجاح..", "تنبيه", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }

    public class BoolInverterConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            if (value is bool)
            {
                return !(bool)value;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            if (value is bool)
            {
                return !(bool)value;
            }
            return value;
        }

        #endregion
    }
}
