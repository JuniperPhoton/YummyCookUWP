using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;
using YummyCookWindowsUniversal.Helper;
using YummyCookWindowsUniversal.Interface;

namespace YummyCookWindowsUniversal.Model
{
    public class Step:ViewModelBase, ICanMakeJson
    {
        private StorageFile _tempFile;

        private string _id;
        public string ID
        {
            get
            {
                return _id;
            }
            set
            {
                if(_id != value)
                {
                    _id = value;
                    RaisePropertyChanged(() => ID);
                }
            }
        }

        private string _stepContent;
        public string StepContent
        {
            get
            {
                return _stepContent;
            }
            set
            {
                if(_stepContent!=value)
                {
                    _stepContent = value;
                    RaisePropertyChanged(() => StepContent);
                }
            }
        }

        private BitmapImage _insertedImage;
        public BitmapImage InsertedImage
        {
            get
            {
                return _insertedImage;
            }
            set
            {
                if(_insertedImage!=value)
                {
                    _insertedImage = value;
                    RaisePropertyChanged(() => InsertedImage);
                }
            }
        }

        private Visibility _showImageVisibility;
        public Visibility ShowImageVisibility
        {
            get
            {
                return _showImageVisibility;
            }
            set
            {
                if(_showImageVisibility!=value)
                {
                    _showImageVisibility = value;
                    RaisePropertyChanged(() => ShowImageVisibility);
                }
            }
        }

        public string ImageUrl;

        private RelayCommand _insertImageCommand;
        public RelayCommand InsertImageCommand
        {
            get
            {
                if (_insertImageCommand != null) return _insertImageCommand;
                return _insertImageCommand=new RelayCommand(async() =>
                {
                    FileOpenPicker picker = new FileOpenPicker();
                    picker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
                    picker.FileTypeFilter.Add(".jpg");
                    picker.FileTypeFilter.Add(".png");
                    var file = await picker.PickSingleFileAsync();
                    if (file != null)
                    {
                        this._tempFile = file;
                        var fileStream = await this._tempFile.OpenAsync(FileAccessMode.Read);
                        this.InsertedImage = new BitmapImage();
                        await this.InsertedImage.SetSourceAsync(fileStream);
                        ShowImageVisibility = Visibility.Visible;
                    }
                });
            }
        }

        private RelayCommand _deleteInsertedImageCommand;
        public RelayCommand DeleteInsertedImageCommand
        {
            get
            {
                if (_deleteInsertedImageCommand != null) return _deleteInsertedImageCommand;
                return _deleteInsertedImageCommand = new RelayCommand(() =>
                  {
                      if(_tempFile!=null)
                      {
                          _tempFile = null;
                          ShowImageVisibility = Visibility.Collapsed;
                          InsertedImage = null;
                      }
                  });
            }
        }

        public Step()
        {
            ID = "";
            StepContent = "";
            ImageUrl = "";
            ShowImageVisibility = Visibility.Collapsed;
        }

        /// <summary>
        /// 获得用于上传服务器的JSON 内容
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public string MakeJson()
        {
            var content = JsonMaker.MakeJsonObj("content", this.StepContent,true);
            var url=JsonMaker.MakeJsonObj("imge_url","",true);
            var json = JsonMaker.MakeJsonString(new List<string> { content });
            return json;
        }
    }
}
