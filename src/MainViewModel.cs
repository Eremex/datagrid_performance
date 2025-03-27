using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;

namespace DataGridPerformance
{
    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        ColumnTypeItem selectedColumnType;

        [ObservableProperty]
        int selectedItemCount;

        public IReadOnlyList<ColumnTypeItem> ColumnTypes { get; }

        public IReadOnlyList<int> ItemCounts { get; }

        public MainViewModel() 
        {
            ColumnTypes = new List<ColumnTypeItem>()
            {
                new ColumnTypeItem() { Text = "TextEditor", DataType = ColumnType.Text, DataGridSupported = true },
                new ColumnTypeItem() { Text = "CheckEditor", DataType = ColumnType.Bool, DataGridSupported = true },
                new ColumnTypeItem() { Text = "ComboBoxEditor", DataType = ColumnType.ComboBox },
                new ColumnTypeItem() { Text = "SpinEditor", DataType = ColumnType.Spin },
                new ColumnTypeItem() { Text = "ButtonEditor", DataType = ColumnType.Button },
                new ColumnTypeItem() { Text = "DateEditor", DataType = ColumnType.DateTime },
                new ColumnTypeItem() { Text = "MemoEditor", DataType = ColumnType.Memo },
                new ColumnTypeItem() { Text = "PopupColorEditor", DataType = ColumnType.Color }
            };
            SelectedColumnType = ColumnTypes[0];

            ItemCounts = new List<int>() { 1000, 10000, 100000 };
            SelectedItemCount = ItemCounts[1];
        }
    }

    public class ColumnTypeItem
    {
        public string Text { get; set; }

        public ColumnType DataType { get; set; }

        public bool DataGridSupported { get; set; } = false;
    }

    public enum ColumnType
    {
        Text,
        Bool,
        ComboBox,
        Button,
        Spin,
        DateTime,
        Memo,
        Color
    }
}