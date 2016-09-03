﻿using QuizMaster.Models.CoreViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizMaster.Models.QuizViewModels
{
    public class QuizCategoryListViewModel : PagedViewModelBase
    {
        public List<QuizCategory> Categories { get; set; }
    }
}
