using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Interactivity;
using Avalonia.Threading;
using Eremex.AvaloniaUI.Controls.DataControl;
using Eremex.AvaloniaUI.Controls.DataGrid;
using Eremex.AvaloniaUI.Controls.Editors;
using Eremex.AvaloniaUI.Controls.Utils;
using Eremex.AvaloniaUI.Icons;
using System;
using System.Collections;
using System.Diagnostics;
using System.Reflection;

namespace DataGridPerformance
{
    public partial class MainWindow : Window
    {
        const int ColumnCount = 20;

        public MainWindow()
        {
            InitializeComponent();
            Title += GetVersionString();
        }

        string GetVersionString()
        {
            var version = typeof(DataGridControl).Assembly.GetName().Version;
            return $" (ver. {version.Major}.{version.Minor}.{version.Build})";
        }

        MainViewModel ViewModel => (MainViewModel)DataContext;
        
        void Button_Click_1(object sender, RoutedEventArgs e)
        {   
            var items = GetItems();
            DoActionAndCountTime(() =>
            {
                var gridControl = new DataGridControl();
                gridControl.EditorButtonShowMode = EditorButtonShowMode.ShowAlways;
                for (int i = 0; i < ColumnCount; i++)
                {   
                    gridControl.Columns.Add(new GridColumn() { FieldName = "Column" + (i % 20), Header = "Column" + i, EditorProperties = GetEditorProperties() });
                }
                gridControl.ItemsSource = items;
                contentPresenter.Content = gridControl;
            }, "Creating EMX GridControl");
        }


        IList GetItems()
        {
            return DataGenerator.GetItems(ViewModel.SelectedItemCount, ViewModel.SelectedColumnType.DataType);
        }

        BaseEditorProperties GetEditorProperties()
        {
            return ViewModel.SelectedColumnType.DataType switch
            {
                ColumnType.ComboBox => new ComboBoxEditorProperties() { ItemsSource = DataGenerator.GetComboBoxItems() },
                ColumnType.Spin => new SpinEditorProperties(),
                ColumnType.Button => GetButtonEditorProperties(),
                ColumnType.Memo => new MemoEditorProperties(),
                ColumnType.Color => new PopupColorEditorProperties(),
                _ => null
            };

            ButtonEditorProperties GetButtonEditorProperties()
            {
                var buttonEditorProperties = new ButtonEditorProperties() { };
                buttonEditorProperties.Buttons.Add(new ButtonSettings() { Content = "..." });
                buttonEditorProperties.Buttons.Add(new ButtonSettings() { Glyph = Basic.Add });
                return buttonEditorProperties;
            };
        }

        void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var items = GetItems();
            DoActionAndCountTime(() =>
            {
                var dataGrid = new DataGrid();
                dataGrid.ColumnWidth = new DataGridLength(120);
                for (int i = 0; i < ColumnCount; i++)
                {
                    var dataGridColumn = GetDataGridColumn();
                    dataGridColumn.Header = "Column" + i;
                    dataGridColumn.Binding = new Binding("Column" + (i % 20));
                    dataGrid.Columns.Add(dataGridColumn);
                }
                dataGrid.ItemsSource = items;
                contentPresenter.Content = dataGrid;
            }, "Creating Avalonia DataGrid");
        }

        DataGridBoundColumn GetDataGridColumn()
        {
            if (ViewModel.SelectedColumnType.DataType == ColumnType.Bool)
            {
                return new DataGridCheckBoxColumn();
            }

            return new DataGridTextColumn();
        }

        void DoActionAndCountTime(Action action, string caption)
        {
            var stopwatch = Stopwatch.StartNew();
            action();
            Dispatcher.UIThread.Post(() => textBlock.Text = $"{caption} takes {stopwatch.ElapsedMilliseconds} ms.");
        }

        void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if(contentPresenter.Content is DataGridControl)
            {
                var scrollViewer = contentPresenter.FindVisualChild<ScrollViewer>();
                DoScrollAndCountTime(scrollViewer);
            }
            else
            {
                var scrollBar = contentPresenter.FindVisualChild<ScrollBar>();
                scrollBarOnScroll = typeof(ScrollBar).GetMethod("OnScroll", BindingFlags.Instance | BindingFlags.NonPublic);
                DoScrollAndCountTime(scrollBar);
            }
        }

        Stopwatch stopwatch = new Stopwatch();

        void DoScrollAndCountTime(ScrollViewer scrollViewer)
        {
            if (!stopwatch.IsRunning)
            {
                scrollViewer.ScrollToHome();
                textBlock.Text = string.Empty;
                UpdateLayout();
                stopwatch.Start();
            }
            if (scrollViewer.Offset.Y + scrollViewer.Viewport.Height < scrollViewer.Extent.Height)
            {
                scrollViewer.PageDown();
                UpdateLayout();
                Dispatcher.UIThread.Post(() => DoScrollAndCountTime(scrollViewer));
            }
            else
            {
                textBlock.Text = $"Scrolling EMX GridControl takes {stopwatch.ElapsedMilliseconds} ms.";
                stopwatch.Reset();
            }
        }

        MethodInfo scrollBarOnScroll;

        void DoScrollAndCountTime(ScrollBar scrollBar)
        {
            if (!stopwatch.IsRunning)
            {
                scrollBar.Value = 0;
                scrollBarOnScroll.Invoke(scrollBar, new object[] { ScrollEventType.ThumbTrack });
                UpdateLayout();
                textBlock.Text = string.Empty;
                stopwatch.Start();
            }
            if (scrollBar.Value < scrollBar.Maximum)
            {
                scrollBar.Value += scrollBar.ViewportSize;
                scrollBarOnScroll.Invoke(scrollBar, new object[] { ScrollEventType.ThumbTrack });
                UpdateLayout();
                Dispatcher.UIThread.Post(() => DoScrollAndCountTime(scrollBar));
            }
            else
            {
                textBlock.Text = $"Scrolling Avalonia DataGrid takes {stopwatch.ElapsedMilliseconds} ms.";
                stopwatch.Reset();
            }
        }

        void Button_Click_4(object sender, RoutedEventArgs e)
        {
            textBlock.Text = string.Empty;
            contentPresenter.Content = null;
            GC.Collect();
        }
    }
}