using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using JP.Utils.Data.Json;
using JP.Utils.Image;
using System;
using System.Collections.Generic;
using System.IO;
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
        public StorageFile _tempFile;

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

        public string ImageUrl { get; set; }

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

        private RelayCommand _showCurrentPictureCommand;
        public RelayCommand ShowCurrentPictureCommand
        {
            get
            {
                if (_showCurrentPictureCommand != null) return _showCurrentPictureCommand;
                return _showCurrentPictureCommand = new RelayCommand(() =>
                  {
                      Messenger.Default.Send(new GenericMessage<BitmapImage>(this.InsertedImage), MessengerToken.ShowPictureToken);
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

        public async Task<bool> UploadImage()
        {
            var newTitleFile = await ImageHandleHelper.CompressImageAsync(_tempFile, 600);
            using (var stream = await newTitleFile.OpenStreamForReadAsync())
            {
                byte[] data = new byte[stream.Length];
                stream.Read(data, 0, (int)stream.Length);
                bool isSuccess = false;
                for(int i=0;i<5;i++)
                {
                    if(isSuccess)
                    {
                        return true;
                    }
                    var resultUrl = await RequestHelper.UploadImageAsync("step.jpg", data);
                    if (!string.IsNullOrEmpty(resultUrl))
                    {
                        this.ImageUrl = resultUrl;
                        isSuccess = true;
                    }
                }
                return isSuccess;
            }
        }

        /// <summary>
        /// 获得用于上传服务器的JSON 内容
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public string MakeJson()
        {
            var content = JsonMaker.MakeJsonObj("content", this.StepContent,true);
            var url=JsonMaker.MakeJsonObj("image_url",string.IsNullOrEmpty(this.ImageUrl)?"":this.ImageUrl,true);
            var json = JsonMaker.MakeJsonString(new List<string> { content,url });
            return json;
        }
    }
}
