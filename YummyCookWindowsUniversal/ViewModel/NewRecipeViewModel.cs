using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using YummyCookWindowsUniversal.Interface;
using System.Runtime.InteropServices.WindowsRuntime;
using System.IO;
using YummyCookWindowsUniversal.Helper;
using YummyCookWindowsUniversal.Model;
using JP.Utils.Image;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using JP.Utils.Data;

namespace YummyCookWindowsUniversal.ViewModel
{
    public class NewRecipeViewModel : ViewModelBase, INavigable
    {
        private StorageFile tempTitleFile;
        private int ingredientCount = 0;
        private int stepCount = 0;

        private string _uploadBtnContent;
        public string UploadBtnContent
        {
            get
            {
                return _uploadBtnContent;
            }
            set
            {
                if(_uploadBtnContent!=value)
                {
                    _uploadBtnContent = value;
                    RaisePropertyChanged(() => UploadBtnContent);
                }
            }
        }

        private Recipe _newRecipe;
        public Recipe NewRecipe
        {
            get
            {
                return _newRecipe;
            }
            set
            {
                if(_newRecipe!=value)
                {
                    _newRecipe = value;
                    RaisePropertyChanged(() => NewRecipe);
                }
            }
        }

        private Visibility _showLoadingVisibility;
        public Visibility ShowLoadingVisibility
        {
            get
            {
                return _showLoadingVisibility;
            }
            set
            {
                if (_showLoadingVisibility != value)
                {
                    _showLoadingVisibility = value;
                    RaisePropertyChanged(() => ShowLoadingVisibility);
                }
            }
        }

        private RelayCommand _uploadTitleImgCommand;
        public RelayCommand UploadTitleImgCommand
        {
            get
            {
                if (_uploadTitleImgCommand != null) return _uploadTitleImgCommand;
                return _uploadTitleImgCommand=new RelayCommand(async()=>
                {
                    var picker = new FileOpenPicker();
                    picker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
                    picker.CommitButtonText = "提交";
                    picker.FileTypeFilter.Add(".jpg");
                    picker.FileTypeFilter.Add(".png");
                    tempTitleFile = await picker.PickSingleFileAsync();
                    if(tempTitleFile!=null)
                    {
                        using (var stream = await tempTitleFile.OpenStreamForReadAsync())
                        {
                            NewRecipe.TitleImage = new BitmapImage();
                            await NewRecipe.TitleImage.SetSourceAsync(stream.AsRandomAccessStream());
                            UploadBtnContent = "修改题图";
                        }
                    }
                });
            }
        }

        private RelayCommand _addIngredientCommand;
        public RelayCommand AddIngredientCommand
        {
            get
            {
                if (_addIngredientCommand != null) return _addIngredientCommand;
                return _addIngredientCommand = new RelayCommand(() =>
                  {
                      var newlng=new Ingredient();
                      newlng.ID= ingredientCount.ToString();
                      NewRecipe.IngredientList.Add(newlng);
                      ingredientCount++;
                  });
            }
        }

        private RelayCommand<string> _deleteIngredientCommand;
        public RelayCommand<string> DeleteIngredientCommand
        {
            get
            {
                if (_deleteIngredientCommand != null) return _deleteIngredientCommand;
                return _deleteIngredientCommand = new RelayCommand<string>(content =>
                {
                    var id = (string)content;
                    NewRecipe.IngredientList.Remove(NewRecipe.IngredientList.ToList().Find(s =>
                    {
                        if (s.ID == id) return true;
                        else return false;
                    }));

                });
            }
        }

        private RelayCommand _addStepCommand;
        public RelayCommand AddStepCommand
        {
            get
            {
                if (_addStepCommand != null) return _addStepCommand;
                return _addStepCommand = new RelayCommand(() =>
                {
                    NewRecipe.StepsList.Add(new Step()
                    {
                        ID = stepCount.ToString()
                    });
                    stepCount++;
                });
            }
        }

        private RelayCommand<string> _deleteStepCommand;
        public RelayCommand<string> DeleteStepCommand
        {
            get
            {
                if (_deleteStepCommand != null) return _deleteStepCommand;
                return _deleteStepCommand = new RelayCommand<string>(content =>
                {
                    var id = (string)content;
                    NewRecipe.StepsList.Remove(NewRecipe.StepsList.ToList().Find(s =>
                    {
                        if (s.ID == id) return true;
                        else return false;
                    }));

                });
            }
        }

        private RelayCommand _cancelCommand;
        public RelayCommand CancelCommand
        {
            get
            {
                if (_cancelCommand != null) return _cancelCommand;
                return _cancelCommand = new RelayCommand(async() =>
                  {
                      MessageDialog md = new MessageDialog("不发布将会丢失所有已编辑的内容。", "不发布？");
                      md.Commands.Add(new UICommand("确认", act =>
                          {
                              var rootFrame = Window.Current.Content as Frame;
                              if(rootFrame.CanGoBack)
                              {
                                  rootFrame.GoBack();
                              }
                          }));
                      md.Commands.Add(new UICommand("取消", act =>
                       {

                       }));
                      await md.ShowAsync();
                  });
            }
        }

        private RelayCommand _publishCommand;
        public RelayCommand PublishCommand
        {
            get
            {
                if (_publishCommand != null) return _publishCommand;
                return _publishCommand=new RelayCommand(async()=>
                {
                    await Publish();
                });
            }
        }

        public NewRecipeViewModel()
        {
            NewRecipe = new Recipe();
            ShowLoadingVisibility = Visibility.Collapsed;
            UploadBtnContent = "上传题图";

            
        }

        private void ShowProgressBar()
        {
            ShowLoadingVisibility = Visibility.Visible;
        }

        private void HideProgressBar()
        {
            ShowLoadingVisibility = Visibility.Collapsed;
        }

        private bool ValidDataFormat()
        {
            if(tempTitleFile==null)
            {
                Messenger.Default.Send(new GenericMessage<string>("请添加题图"), MessengerToken.ToastToken);
                return false;
            }
            if(string.IsNullOrEmpty(NewRecipe.Title))
            {
                Messenger.Default.Send(new GenericMessage<string>("请添加标题"), MessengerToken.ToastToken);

                return false;
            }
            if(NewRecipe.IngredientList.Count==0)
            {
                Messenger.Default.Send(new GenericMessage<string>("请添加食材"), MessengerToken.ToastToken);

                return false;
            }
            if(NewRecipe.StepsList.Count==0)
            {
                Messenger.Default.Send(new GenericMessage<string>("请添加步骤"), MessengerToken.ToastToken);

                return false;
            }
            return true;
        }

        private async Task Publish()
        {
            if (ValidDataFormat())
            {
                try
                {
                    ShowProgressBar();

                    //upload title image
                    var newTitleFile = await ImageHandleHelper.CompressImageAsync(tempTitleFile, 1000);
                    using (var stream = await newTitleFile.OpenStreamForReadAsync())
                    {
                        byte[] data = new byte[stream.Length];
                        stream.Read(data, 0, (int)stream.Length);

                        string resultUrl = null;
                        for (int i = 0; i < 5; i++)
                        {
                            var result = await RequestHelper.UploadImageAsync("titleImage.jpg", data);
                            if (!string.IsNullOrEmpty(result))
                            {
                                resultUrl = result;
                                break;
                            }
                        }
                        
                        if(resultUrl!= null)
                        {
                            bool isUploadAllImageOfSteps = true;
                            foreach (var step in NewRecipe.StepsList)
                            {
                                if (step._tempFile != null)
                                {
                                    var isSuccess = await step.UploadImage();
                                    if (!isSuccess)
                                    {
                                        isUploadAllImageOfSteps = false;
                                        break;
                                    }
                                }
                            }

                            if(!isUploadAllImageOfSteps)
                            {
                                HideProgressBar();
                                Messenger.Default.Send(new GenericMessage<string>("后台问题，上传图片失败，请再次尝试"), MessengerToken.ToastToken);
                            }
                            else
                            {
                                NewRecipe.TitleImageUrl = resultUrl;
                                NewRecipe.CookUser.UserName = LocalSettingHelper.GetValue("username");

                                var recipeStr = NewRecipe.MakeJson();
                                var result = await RequestHelper.PublishRecipeAsync(recipeStr);
                                if (result)
                                {
                                    var rootFrame = Window.Current.Content as Frame;
                                    if (rootFrame.CanGoBack)
                                    {
                                        rootFrame.GoBack();
                                    }
                                }
                                else
                                {
                                    HideProgressBar();
                                    Messenger.Default.Send(new GenericMessage<string>("发布失败，请再次尝试"), MessengerToken.ToastToken);
                                }
                            }
                        }
                        else
                        {
                            HideProgressBar();
                            Messenger.Default.Send(new GenericMessage<string>("上传题图失败，请再次尝试"), MessengerToken.ToastToken);
                        }
                    }
                }
                catch (Exception e)
                {
                    Messenger.Default.Send<GenericMessage<string>>(new GenericMessage<string>(e.Message), MessengerToken.ToastToken);
                }

            }
        }

        public void Activate(object param)
        {
            
        }

        public void Deactivate(object param)
        {
            
        }

        public override void Cleanup()
        {
            base.Cleanup();
            this.NewRecipe.TitleImage = null;
            this.NewRecipe = null;
            NewRecipe = new Recipe();
            ShowLoadingVisibility = Visibility.Collapsed;
            UploadBtnContent = "上传题图";
        }
    }
}
