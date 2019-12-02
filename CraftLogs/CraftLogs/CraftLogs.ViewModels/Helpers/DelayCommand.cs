/*
Copyright 2019 Gyirán Márton Áron

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License. 
*/

using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace CraftLogs.ViewModels
{
    public class DelayCommand : DelegateCommand
    {
        private readonly CommandExecutionValidator _executionValidator = new CommandExecutionValidator();

        private Func<bool> _canExecuteMethod;

        public DelayCommand(Action executeMethod)
            : base(executeMethod)
        {
        }

        public DelayCommand(Action executeMethod, Func<bool> canExecuteMethod)
            : base(executeMethod)
        {
            _canExecuteMethod = canExecuteMethod;
        }

        public new DelayCommand ObservesCanExecute(Expression<Func<bool>> canExecuteExpression)
        {
            _canExecuteMethod = canExecuteExpression.Compile();
            ObservesPropertyInternal(canExecuteExpression);
            return this;
        }

        public new bool CanExecute()
        {
            return _canExecuteMethod?.Invoke() ?? true;
        }

        protected override void Execute(object parameter)
        {
            if (_executionValidator.ExecutionIsAllowed() && CanExecute(parameter))
            {
                base.Execute(parameter);
            }
        }

        protected override bool CanExecute(object parameter)
        {
            return _canExecuteMethod?.Invoke() ?? true;
        }
    }

    public class DelayCommand<TParameter> : DelegateCommand<TParameter>
    {
        private readonly CommandExecutionValidator _executionValidator = new CommandExecutionValidator();

        private Func<TParameter, bool> _canExecuteMethod;

        public DelayCommand(Action<TParameter> executeMethod)
            : base(executeMethod)
        {
        }

        public DelayCommand(Action<TParameter> executeMethod, Func<TParameter, bool> canExecuteMethod)
            : base(executeMethod)
        {
            _canExecuteMethod = canExecuteMethod;
        }

        public new DelegateCommand<TParameter> ObservesCanExecute(Expression<Func<bool>> canExecuteExpression)
        {
            var expression = Expression.Lambda<Func<TParameter, bool>>(
                canExecuteExpression.Body,
                Expression.Parameter(typeof(TParameter), "o"));
            _canExecuteMethod = expression.Compile();
            ObservesPropertyInternal(canExecuteExpression);
            return this;
        }

        protected override void Execute(object parameter)
        {
            if (_executionValidator.ExecutionIsAllowed() && CanExecute(parameter))
            {
                base.Execute(parameter);
            }
        }

        protected override bool CanExecute(object parameter)
        {
            switch (parameter)
            {
                case null:
                    return _canExecuteMethod?.Invoke(default) ?? true;
                case TParameter validParameter:
                    return _canExecuteMethod?.Invoke(validParameter) ?? true;
                default:
                    return false;
            }
        }
    }

    public class CommandExecutionValidator
    {
        private readonly TimeSpan _minimumExecutionInterval = TimeSpan.FromMilliseconds(500);

        private DateTime _lastExecution;

        public bool ExecutionIsAllowed()
        {
            var delta = DateTime.Now - _lastExecution;
            var allowed = delta > _minimumExecutionInterval;
            _lastExecution = DateTime.Now;

            return allowed;
        }
    }
}
