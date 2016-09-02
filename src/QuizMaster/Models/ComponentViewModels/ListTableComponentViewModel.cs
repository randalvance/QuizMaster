using QuizMaster.Models.CoreViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace QuizMaster.Models.ComponentViewModels
{
    public abstract class ListTableComponentViewModel
    {
        public string Controller { get; set; }
        public string ListAction { get; set; } = "Index";
        public string DetailAction { get; set; } = "Detail";
        public string EditAction { get; set; } = "Edit";
        public string AddAction { get; set; } = "Edit";
        public string DeleteAction { get; set; } = "Delete";
        public bool ShowAddButton { get; set; } = true;
        public bool ShowEditButton { get; set; } = true;
        public bool ShowDeleteButton { get; set; } = true;
        public string IdProperty { get; set; }
        public PagedViewModelBase PagedViewModel { get; set; }
        public virtual List<object> Items { get; set; }
        public virtual List<ListTableColumnInfo> Columns { get; set; }
        public virtual List<ListTableAction> Actions { get; set; } = new List<ListTableAction>();
        public Type ItemType { get; set; }
    }

    public class ListTableComponentViewModel<T> : ListTableComponentViewModel where T : class
    {
        public ListTableComponentViewModel(List<T> items)
        {
            IdProperty = $"{typeof(T).Name}Id";
            Items = items;
            ItemType = typeof(T);
        }

        public new List<T> Items
        {
            get
            {
                return base.Columns.Cast<T>().ToList();
            }
            set
            {
                base.Items = value.Cast<object>().ToList();
            }
        }
    }

    public class ListTableColumnInfo
    {
        public ListTableColumnInfo(string headerText, bool isDetailLinkColumn = false, string format = null)
        {
            HeaderText = headerText;
            IsDetailLinkColumn = isDetailLinkColumn;
            Format = format;
        }

        public string HeaderText { get; set; }
        public string Format { get; set; }
        public LambdaExpression PropertyAccessor { get; set; }
        public bool IsDetailLinkColumn { get; set; }
        public bool Sortable { get; set; } = false;
    }

    public class ListTableColumnInfo<T> : ListTableColumnInfo
    {
        public ListTableColumnInfo(string headerText, Expression<Func<T, object>> propertyAccessor, bool isDetailLinkColumn = false, string format = null) : 
            base(headerText, isDetailLinkColumn, format)
        {
            PropertyAccessor = propertyAccessor;
        }
    }

    public class ListTableAction
    {
        public string Controller { get; set; }
        public string Action { get; set; }
        public string IconClass { get; set; }
        public string Title { get; set; }
        public LambdaExpression IdExpression { get; set; }
    }

    public class ListTableAction<T> : ListTableAction
    {
        public new Expression<Func<T, object>> IdExpression
        {
            set
            {
                base.IdExpression = value;
            }
        }
    }
}
