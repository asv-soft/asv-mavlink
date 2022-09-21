using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Asv.Common;

namespace Asv.Mavlink.Shell
{
    public abstract class TextTableBorder
    {
        protected abstract string Source { get; }

        public char TopLeft => Source[0];
        public char TopCenter => Source[1];
        public char TopIntercection => Source[2];
        public char TopRight => Source[3];

        public char Middle1Left => Source[4];
        public char Middle1Center => Source[5];
        public char Middle1Intercection => Source[6];
        public char Middle1Right => Source[7];

        public char Middle2Left => Source[8];
        public char Middle2Center => Source[9];
        public char Middle2Intercection => Source[10];
        public char Middle2Right => Source[11];

        public char BottomLeft => Source[12];
        public char BottomCenter => Source[13];
        public char BottomIntercection => Source[14];
        public char BottomRight => Source[15];

    }

    public class EmptyTextTableBorder : TextTableBorder
    {
        protected override string Source => "    " +
                                            "    " +
                                            "    " +
                                            "    ";
    }

    public class DoubleTextTableBorder : TextTableBorder
    {
        protected override string Source => "╔═╦╗" +
                                            "║ ║║" +
                                            "╠═╬╣" +
                                            "╚═╩╝";
    }

    public class SingleTextTableBorder : TextTableBorder
    {
        protected override string Source => "┌─┬┐" +
                                            "│ ││" +
                                            "├─┼┤" +
                                            "└─┴┘";
    }

    public class AsciiTableBorderSingle : TextTableBorder
    {


        protected override string Source => "+-++" +
                                            "| ||" +
                                            "+-++" +
                                            "+-++";
    }


    public static class TextTable
    {
        public static TextTableBorder Ascii = new AsciiTableBorderSingle();
        public static TextTableBorder Single = new SingleTextTableBorder();
        public static TextTableBorder Double = new DoubleTextTableBorder();
        public static TextTableBorder Empty = new EmptyTextTableBorder();


        public static void PrintKeyValue(Action<string> write, TextTableBorder border, int keyWidth, int valueWidth, string name,IEnumerable<KeyValuePair<string,string>> values)
        {
            var key = name.PadCenter(keyWidth + valueWidth + 1);
            write(TableBegin(border, key));
            write(TableRow(border, key));
            write(border.Middle2Left + string.Empty.PadLeft(keyWidth, border.Middle2Center) + border.TopIntercection +
                  string.Empty.PadLeft(valueWidth, border.Middle2Center) + border.Middle2Right);

            foreach (var pair in values)
            {
                write(TableRow(border, pair.Key.PadRight(keyWidth), pair.Value.PadRight(valueWidth)));
            }
            write(TableEnd(border, keyWidth, valueWidth));
        }

        public static string TableBegin(TextTableBorder border, IEnumerable<int> values)
        {
            return border.TopLeft + string.Join(border.TopIntercection.ToString(), values.Select(_ => string.Empty.PadLeft(_, border.TopCenter))) + border.TopRight;
        }

        public static string TableBegin(TextTableBorder border, IEnumerable<string> values)
        {
            return TableBegin(border, values.Select(_ => _.Length));
        }

        public static string TableBegin(TextTableBorder border, params string[] values)
        {
            return TableBegin(border, values.AsEnumerable());
        }

        public static string TableBegin(TextTableBorder border, params int[] values)
        {
            return TableBegin(border, values.AsEnumerable());
        }

        public static string TableRow(TextTableBorder border, params string[] values)
        {
            return TableRow(border, values.AsEnumerable());
        }

        private static string TableRow(TextTableBorder border, IEnumerable<string> values)
        {
            return border.Middle1Left + string.Join(border.Middle1Intercection.ToString(), values) + border.Middle1Right;
        }

        public static string TableEndRow(TextTableBorder border, params string[] values)
        {
            return TableEndRow(border, values.AsEnumerable());
        }

        public static string TableEndRow(TextTableBorder border, params int[] values)
        {
            return TableEndRow(border, values.AsEnumerable());
        }

        private static string TableEndRow(TextTableBorder border, IEnumerable<string> values)
        {
            return TableEndRow(border, values.Select(_ => _.Length));
        }

        private static string TableEndRow(TextTableBorder border, IEnumerable<int> values)
        {
            return border.Middle2Left + string.Join(border.Middle2Intercection.ToString(), values.Select(_ => string.Empty.PadLeft(_, border.Middle2Center))) + border.Middle2Right;
        }

        public static string TableEnd(TextTableBorder border, params string[] values)
        {
            return TableEnd(border, values.AsEnumerable());
        }

        public static string TableEnd(TextTableBorder border, IEnumerable<string> values)
        {
            return TableEnd(border, values.Select(_ => _.Length));
        }

        public static string TableEnd(TextTableBorder border, IEnumerable<int> values)
        {
            return border.BottomLeft + string.Join(border.BottomIntercection.ToString(), values.Select(_ => string.Empty.PadLeft(_, border.BottomCenter))) + border.BottomRight;
        }

        public static string TableEnd(TextTableBorder border, params int[] values)
        {
            return TableEnd(border, values.AsEnumerable());
        }

        public static void PrintTableFromObject<T>(Action<string> write, TextTableBorder border, int padding, int maxLength,
            IEnumerable<T> items)
        {
            if (!items.Any()) return;
            var props = items.First().GetType().GetProperties().ToDictionary(_ => _, _ => _.Name);
            PrintTableFromObject(write, border, padding, maxLength, items, props);
        }

        public static void PrintTableFromObject<T>(Action<string> write, TextTableBorder border,int padding, int maxLength, IEnumerable<T> items,
            params Expression<Func<T, object>>[] properties)
        {
            PrintTableFromObject(write, border, padding, maxLength, items, properties.Select(_ =>GetPropertyInfo(_).Name));
        }

        public static void PrintTableFromObject<T>(Action<string> write, TextTableBorder border, int padding, int maxLength, IEnumerable<T> items,
            IEnumerable<string> properties)
        {
            PrintTableFromObject(write, border, padding, maxLength, items, properties.ToDictionary(_ => _, _ => _));
        }

        public static void PrintTableFromObject<T>(Action<string> write, TextTableBorder border, int padding, int maxLength,
            IEnumerable<T> items, IEnumerable<KeyValuePair<string, string>> properties)
        {
            var t = typeof(T);
            PrintTableFromObject<T>(write, border, padding, maxLength, items, properties.ToDictionary(_ => t.GetProperty(_.Key),_=>_.Value));
        }

        public static void PrintTable(Action<string> write, TextTableBorder border, int padding,int maxLength,
            string[][] headerWithRows)
        {
            var columns = headerWithRows.First().Count();
            var rows = headerWithRows.Count();

            var width = Enumerable.Range(0, columns * rows).GroupBy(_ => _ % columns, _ => headerWithRows[_ / columns][_ % columns].TrimToMaxLength(maxLength)).Select(_ => _.Max(__ => __?.Length??0))
                .Select(_ => _ + padding*2).ToArray();

            write(TableBegin(border, width));
            write(TableRow(border, headerWithRows.First().Select((_, i) => string.Empty.PadLeft(padding)+_.TrimToMaxLength(maxLength).PadRight(width[i]- padding))));
            write(TableEndRow(border, width));

            foreach (var item in headerWithRows.Skip(1))
            {
                write(TableRow(border, item.Select((_, i) => string.Empty.PadLeft(padding) + (_??string.Empty).TrimToMaxLength(maxLength).PadRight(width[i]-padding))));
            }
            write(TableEnd(border, width));
        }

        public static void PrintTable(Action<string> write, TextTableBorder border, int padding, int maxLength, IEnumerable<IEnumerable<string>> headerWithRows)
        {
            var arr = headerWithRows.Select(_ => _.ToArray()).ToArray();
            PrintTable(write, border, padding, maxLength, arr);
        }

        public static void PrintTableFromObject<T>(Action<string> write, TextTableBorder border, int padding, int maxLength, IEnumerable<T> items, IEnumerable<KeyValuePair<PropertyInfo,string>> properties)
        {
            var props = properties.ToArray();
            var values = items.Select(
                __ => props.Select(prop => prop.Key.GetValue(__)?.ToString()).ToArray())
                .ToList();

            values.Insert(0, props.Select(_ => _.Value).ToArray());

            PrintTable(write, border, padding, maxLength, values);
        }

        public static PropertyInfo GetPropertyInfo<TSource>(Expression<Func<TSource, object>> propertyLambda)
        {
            var member = propertyLambda.Body as MemberExpression;
            if (member == null)
            {// value types return Convert(x.property) which can't be cast to MemberExpression
                var expression = propertyLambda.Body as UnaryExpression;
                member = expression.Operand as MemberExpression;
            }
            return member.Member as PropertyInfo;
        }
    }


}
