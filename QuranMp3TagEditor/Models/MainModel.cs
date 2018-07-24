using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using QuranMp3TagEditor.Annotations;
using File = TagLib.File;

namespace QuranMp3TagEditor.Models
{
    public class MainModel : INotifyPropertyChanged
    {
        private string _path;
        private Style _fileNameStyle;
        private bool _fileNameChange;
        private bool _fileNameSuratNumber;
        private bool _fileNameSuratName;
        private bool _titleChange;
        private bool _titleSuratNumber;
        private bool _titleSuratName;
        private Style _titleStyle;
        private bool _artistChange;
        private string _artist;
        private bool _albumChange;
        private string _album;
        private bool _yearChange;
        private string _year;
        private bool _recorgnizeByFileName;

        public string Path
        {
            get => _path;
            set
            {
                _path = value;
                OnPropertyChanged();
            }
        }

        public bool RecorgnizeByFileName
        {
            get => _recorgnizeByFileName;
            set
            {
                _recorgnizeByFileName = value;
                OnPropertyChanged();
            }
        }

        public bool FileNameChange
        {
            get => _fileNameChange;
            set
            {
                _fileNameChange = value;
                OnPropertyChanged();
            }
        }

        public bool FileNameSuratNumber
        {
            get => _fileNameSuratNumber;
            set
            {
                _fileNameSuratNumber = value;
                OnPropertyChanged();
            }
        }

        public bool FileNameSuratName
        {
            get => _fileNameSuratName;
            set
            {
                _fileNameSuratName = value;
                OnPropertyChanged();
            }
        }

        public Style FileNameStyle
        {
            get => _fileNameStyle;
            set
            {
                _fileNameStyle = value;
                OnPropertyChanged();
            }
        }

        public bool TitleChange
        {
            get => _titleChange;
            set
            {
                _titleChange = value;
                OnPropertyChanged();
            }
        }

        public bool TitleSuratNumber
        {
            get => _titleSuratNumber;
            set
            {
                _titleSuratNumber = value;
                OnPropertyChanged();
            }
        }

        public bool TitleSuratName
        {
            get => _titleSuratName;
            set
            {
                _titleSuratName = value;
                OnPropertyChanged();
            }
        }

        public Style TitleStyle
        {
            get => _titleStyle;
            set
            {
                _titleStyle = value;
                OnPropertyChanged();
            }
        }

        public bool ArtistChange
        {
            get => _artistChange;
            set
            {
                _artistChange = value;
                OnPropertyChanged();
            }
        }

        public string Artist
        {
            get => _artist;
            set
            {
                _artist = value;
                OnPropertyChanged();
            }
        }

        public bool AlbumChange
        {
            get => _albumChange;
            set
            {
                _albumChange = value;
                OnPropertyChanged();
            }
        }

        public string Album
        {
            get => _album;
            set
            {
                _album = value;
                OnPropertyChanged();
            }
        }

        public bool YearChange
        {
            get => _yearChange;
            set
            {
                _yearChange = value;
                OnPropertyChanged();
            }
        }

        public string Year
        {
            get => _year;
            set
            {
                _year = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<MP3File> Files { get; set; }

        public ObservableCollection<Style> Styles { get; set; }


        public MainModel()
        {
            Files = new ObservableCollection<MP3File>();
            Styles = new ObservableCollection<Style>();
            Styles = Deserialize(Styles, "Styles.xml");
            if (Styles.Any())
            {
                FileNameStyle = Styles[0];
                TitleStyle = Styles[0];
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public T Deserialize<T>(T file, string path)
        {
            var serializer = new XmlSerializer(typeof(T));

            using (var reader = new StreamReader(path))
            {
                return (T)serializer.Deserialize(reader);
            }
        }
    }

    public class MP3File : INotifyPropertyChanged
    {
        private string _name;
        private File _file;

        public MP3File(string mp3File)
        {
            Name = Path.GetFileNameWithoutExtension(mp3File);
            File = File.Create(mp3File);
        }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        public File File
        {
            get => _file;
            set
            {
                _file = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}