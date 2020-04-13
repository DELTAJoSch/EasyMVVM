using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using EasyMVVM;
using EasyMVVM_Tutorial.Models;

namespace EasyMVVM_Tutorial.ViewModels
{
    internal class MainWindowViewModel: ViewModelBase
    {
        private string _counterText;

        private CounterModel counterModel = new CounterModel();

        private readonly DelegateCommand _increaseCounterClicked;

        public MainWindowViewModel()
        {
            _increaseCounterClicked = new DelegateCommand(IncreaseCounterClicked_Command);
        }

        public string CounterText { get { return _counterText; } set { SetProperty(ref _counterText, value); } }

        public ICommand IncreaseCounterClicked => _increaseCounterClicked;

        private async void IncreaseCounterClicked_Command(object sender)
        {
            ulong curr = await counterModel.IncreaseCounter();
            CounterText = $"Current Value: {curr}";
        }
    }
}
