﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Push_License
{
    public interface INavigationService
    {
        void NavigateToViewModel<TViewModel>() where TViewModel : ViewModelBase;
    }
}
