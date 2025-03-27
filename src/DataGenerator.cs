using Avalonia.Media;
using System;
using System.Collections;
using System.Collections.Generic;

namespace DataGridPerformance
{
    public static class DataGenerator
    {
        static Random random = new Random();

        public static IList<string> Items => GetComboBoxItems();

        static IList<Color> colors = new List<Color>() 
        {
            Colors.Red, Colors.Green, Colors.Blue, Colors.Yellow, Colors.Aqua, Colors.Gold, Colors.Purple, Colors.Silver, Colors.Lime, Colors.White
        };

        public static IList<string> GetComboBoxItems()
        {
            var list = new List<string>();
            for (int i = 0; i < 100; i++)
            {
                list.Add("Item" + i);
            }
            return list;
        }

        public static IList GetItems(int itemsCount, ColumnType columnType)
        {
            return columnType switch
            {
                ColumnType.Bool => GetItems<bool?>(itemsCount, columnType),
                ColumnType.DateTime => GetItems<DateTime>(itemsCount, columnType),
                ColumnType.Spin => GetItems<int>(itemsCount, columnType),
                ColumnType.Color => GetItems<Color>(itemsCount, columnType),
                _ => GetItems<string>(itemsCount, columnType)
            };
        }

        static IList GetItems<T>(int itemCount, ColumnType columnType)
        {
            var items = new List<TestData<T>>(itemCount);

            var properties = typeof(TestData<T>).GetProperties();

            for (int i = 0; i < itemCount; i++)
            {
                var testData = new TestData<T>();
                for (int j = 0; j < properties.Length; j++)
                {
                    properties[j].SetValue(testData, GetCellValue(j, i, columnType));
                }
                items.Add(testData);
            }

            return items;
        }

        static object GetCellValue(int columnIndex, int itemIndex, ColumnType columnType)
        {
            return columnType switch
            {
                ColumnType.Bool => GetBoolean(),
                ColumnType.DateTime => DateTime.Now.AddDays(-random.Next(1000)),
                ColumnType.ComboBox or ColumnType.Button => "Item" + random.Next(100),
                ColumnType.Spin => random.Next(10000),
                ColumnType.Color => colors[random.Next(colors.Count)],
                _ => "Col" + columnIndex + " Item" + itemIndex

            };

            bool? GetBoolean()
            {
                var result = random.Next(5);
                if (result == 0)
                    return null;
                return result > 2;
            }
        }
    }

    public class TestData<T>
    {
        public T Column0 { get; set; }

        public T Column1 { get; set; }

        public T Column2 { get; set; }

        public T Column3 { get; set; }

        public T Column4 { get; set; }

        public T Column5 { get; set; }

        public T Column6 { get; set; }

        public T Column7 { get; set; }

        public T Column8 { get; set; }

        public T Column9 { get; set; }

        public T Column10 { get; set; }

        public T Column11 { get; set; }

        public T Column12 { get; set; }

        public T Column13 { get; set; }

        public T Column14 { get; set; }

        public T Column15 { get; set; }

        public T Column16 { get; set; }

        public T Column17 { get; set; }

        public T Column18 { get; set; }

        public T Column19 { get; set; }
    }
}
