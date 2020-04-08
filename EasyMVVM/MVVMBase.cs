/* Copyright (C) 2020  Johannes Schiemer 
	This program is free software: you can redistribute it and/or modify 
	it under the terms of the GNU General Public License as published by 
	the Free Software Foundation, either version 3 of the License, or 
	(at your option) any later version. 
	This program is distributed in the hope that it will be useful, 
	but WITHOUT ANY WARRANTY; without even the implied warranty of 
	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the 
	GNU General Public License for more details. 
	You should have received a copy of the GNU General Public License 
	along with this program.  If not, see <https://www.gnu.org/licenses/>. 
*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace EasyMVVM
{
    /// <summary>
    /// This class is the base class for all viewmodels used in thís project
    /// </summary>
    public abstract class ViewModelBase : INotifyPropertyChanged
    {

        /// <summary>
        /// This event is the PropertyChangedEvent raised when the UI needs to be updated
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// This action should be set to call your logging framework. The first string should be the Message, the second string should be the LogLevel.
        /// </summary>
        public static Action<string, string> LogMessageNoError = (string msg, string level) => { throw new InvalidOperationException("To use this functionality, set a corresponding action during startup."); };

        /// <summary>
        /// This action should be set to call your logging framework. The first string should be the Message, the second string should be the LogLevel.
        /// The Exception should be the exception that occured.
        /// </summary>
        public static Action<string, Exception, string> LogMessage = (string msg, Exception ex, string level) => { throw new InvalidOperationException("To use this functionality, set a corresponding action during startup."); };

        /// <summary>
        /// This Function sets a property of the passed type
        /// </summary>
        /// <typeparam name="T">The type of the property</typeparam>
        /// <param name="field">The private variable the data is stored in</param>
        /// <param name="newValue">The new value</param>
        /// <param name="propertyName">The name of the property</param>
        /// <returns>returns a bool on the state of the function</returns>
        protected bool SetProperty<T>(ref T field, T newValue, [CallerMemberName]string propertyName = null)
        {
            if (!EqualityComparer<T>.Default.Equals(field, newValue))
            {
                field = newValue;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
                return true;
            }
            return false;
        }
    }

    /// <summary>
    /// This class implements the ICommand interface and is used when a command is triggered
    /// </summary>
    public class DelegateCommand : ICommand
    {
        private readonly Action<object> _executeAction;
        private readonly Func<object, bool> _canExecuteAction;
        private ICommand changeTheme;

        /// <summary>
        /// This constructor takes the Action to be executed as well as a function to check wether the action can be executed
        /// </summary>
        /// <param name="executeAction">The action to be executed</param>
        /// <param name="canExecuteAction">A function to call before executing the action</param>
        public DelegateCommand(Action<object> executeAction, Func<object, bool> canExecuteAction)
        {
            _executeAction = executeAction;
            _canExecuteAction = canExecuteAction;
        }

        /// <summary>
        /// This constructor only takes the action to be executed
        /// </summary>
        /// <param name="executeAction">The action to be executed</param>
        public DelegateCommand(Action<object> executeAction)
        {
            _executeAction = executeAction;
            _canExecuteAction = (object o) => { return true; };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="changeTheme"></param>
        public DelegateCommand(ICommand changeTheme)
        {
            this.changeTheme = changeTheme;
        }

        /// <summary>
        /// This executes the action
        /// </summary>
        /// <param name="parameter">an object to perform the action on</param>
        public void Execute(object parameter) => _executeAction(parameter);

        /// <summary>
        /// This invokes the specified function
        /// </summary>
        /// <param name="parameter">The object to be executed</param>
        /// <returns>Returns wether the function can be executed</returns>
        public bool CanExecute(object parameter) => _canExecuteAction?.Invoke(parameter) ?? true;

        /// <summary>
        /// This event is raised when the state of the execution of the object changes
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// This functions invokes the CanExecuteChanged Event
        /// </summary>
        public void InvokeCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}